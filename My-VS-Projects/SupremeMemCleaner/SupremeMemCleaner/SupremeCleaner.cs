using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectX_Renderer;

namespace SupremeMemCleaner
{
    public partial class SupremeCleaner : Form
    {
        private Overlay overlay;
        public SupremeCleaner()
        {
            InitializeComponent();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            switch (Button1.Text)
            {
                case "Run":
                    LogControl.Info("Button Press: Run");
                    overlay = new Overlay();
                    overlay.Show();
                    Button1.Text = "Stop";
                    break;

                case "Stop":
                    LogControl.Info("Button Press: Stop");
                    Overlay.ingame = false;
                    overlay.Close();
                    overlay = null;
                    Button1.Text = "Run";
                    break;
            }
        }

        private void Overlay_CheckedChanged(object sender, EventArgs e)
        {
            Overlay.ingame = CheckBox1.Checked? true: false;   
        }
    }
}
