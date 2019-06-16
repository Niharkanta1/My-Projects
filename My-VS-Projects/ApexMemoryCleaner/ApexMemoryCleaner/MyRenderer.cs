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

namespace BetterRenderer
{
    public class MyRenderer
    {

        public WindowRenderTarget device;
        public MyRenderer() { }
        ~MyRenderer() { }
        public MyRenderer(WindowRenderTarget renderTarget)
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
}
