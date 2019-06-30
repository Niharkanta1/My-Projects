using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Format = SharpDX.DXGI.Format;
using SharpDX;
using SharpDX.DirectWrite;
using System.Threading;
using System.Runtime.InteropServices;
using SharpDX.Mathematics.Interop;
using System.IO;

namespace DirectX_Renderer
{
    public class Renderer
    {
        public WindowRenderTarget device;
        public Renderer() { }
        ~Renderer() { }
        public Renderer(WindowRenderTarget renderTarget)
        {
            device = renderTarget;
        }
        public void DrawBoxESP(SolidColorBrush brush, System.Drawing.Rectangle boxRect)
        {
            DrawRectangle(brush, false, boxRect.X, boxRect.Y, boxRect.Width, boxRect.Height); // Middle
        }
        public void DrawRectangle(SolidColorBrush brush, bool Filled, float x, float y, float width, float height)
        {
            if (!Filled)
                device.DrawRectangle(new RawRectangleF(x, y, x + width, y + height), brush);
            else
                device.FillRectangle(new RawRectangleF(x, y, x + width, y + height), brush);
        }
        public void DrawText(string text, SharpDX.Mathematics.Interop.RawVector2 pos, SolidColorBrush brush, TextFormat font)
        {
            device.DrawText(text, font, new RawRectangleF(pos.X, pos.Y, pos.X + (font.FontSize * text.Length), pos.Y + font.FontSize), brush);
        }
        public void DrawLine(float x1, float y1, float x2, float y2, SolidColorBrush solidColorBrush)
        {
            device.DrawLine(new RawVector2(x1, y1), new RawVector2(x2, y2), solidColorBrush);
        }
        public void DrawCircle(Ellipse circle, SolidColorBrush brush, bool Filled)
        {
            if (!Filled)
                device.DrawEllipse(circle, brush);
            else
                device.FillEllipse(circle, brush);
        }
        public void RectHealthBar(float x, float y, float w, float h, int HP)
        {
            SolidColorBrush healthBrush = new SolidColorBrush(device, SharpDX.Color.Green);

            if (HP > 100) HP = 100;
            int size = (int)((h * HP) / 100);

            if (size == h)
                size -= 2;

            if (HP != 100)
            {
                SolidColorBrush lackOfHealthBrush = new SolidColorBrush(device, SharpDX.Color.Red);
                DrawRectangle(lackOfHealthBrush, true, x + 1, y + 1, w - 2, h - size);
                lackOfHealthBrush.Dispose();
            }

            DrawRectangle(healthBrush, true, x + 1, y + 1 + h - size, w - 2, size);

            healthBrush.Dispose();
        }
    }

    public partial class Overlay : Form
    {
        private WindowRenderTarget device;
        private HwndRenderTargetProperties renderProperties;
        private Factory factory;

        const string WINDOW_NAME = "EscapeFromTarkov";

        private IntPtr handle;
        private IntPtr gameHandle = FindWindow(null, WINDOW_NAME);
        private Thread threadDX = null;

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        //DllImports
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr handle);

        //Styles
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WS_EX_TOPMOST = 0x00000008;
        private const int WM_ACTIVATE = 6;
        private const int WA_INACTIVE = 0;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_NOACTIVATEANDEAT = 0x0004;

        EasyMemoryCleaner.EasyMemoryCleaner cheat = null;

        public Overlay()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            this.handle = Handle;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            RECT rect;
            GetWindowRect(gameHandle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Top = rect.top;
            this.Left = rect.left;
        }

        private void Overlay_SharpDX_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.TopMost = true;
            this.Visible = true;

            factory = new Factory();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |// reduce the flicker too
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.Opaque |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor, true);

            renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(this.Width, this.Height),
                PresentOptions = PresentOptions.None
            };

            //Init DirectX
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties);

            cheat = new EasyMemoryCleaner.EasyMemoryCleaner(new UnityEngine.Vector2(this.Size.Width, this.Size.Height));

            Thread refreshSetupThread = new Thread(new ThreadStart(cheat.ReadWorlds));
            refreshSetupThread.IsBackground = true;
            refreshSetupThread.Start();

            threadDX = new Thread(new ParameterizedThreadStart(_loop_DXThread));
            threadDX.Priority = ThreadPriority.Highest;
            threadDX.IsBackground = true;
            threadDX.Start();
        }

        private void _loop_DXThread(object sender)
        {
            while (true)
            {
                device.BeginDraw();
                device.Clear(SharpDX.Color.Transparent);
                device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Default;
                //cheat.Draw(device);
                cheat.DrawAll(device);
                device.EndDraw();
            }
        }

        /// <summary>
        /// Used to not show up the form in alt-tab window. 
        /// Tested on Windows 7 - 64bit and Windows 10 64bit
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams pm = base.CreateParams;
                pm.ExStyle |= 0x80;
                pm.ExStyle |= WS_EX_TOPMOST; // make the form topmost
                pm.ExStyle |= WS_EX_NOACTIVATE; // prevent the form from being activated
                return pm;
            }
        }

        /// <summary>
        /// Makes the form unable to gain focus at all time, 
        /// which should prevent lose focus
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)MA_NOACTIVATEANDEAT;
                return;
            }
            if (m.Msg == WM_ACTIVATE)
            {
                if (((int)m.WParam & 0xFFFF) != WA_INACTIVE)
                    if (m.LParam != IntPtr.Zero)
                        SetActiveWindow(m.LParam);
                    else
                        SetActiveWindow(IntPtr.Zero);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
