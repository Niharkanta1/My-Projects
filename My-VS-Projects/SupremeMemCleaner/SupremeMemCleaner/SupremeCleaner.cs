using System;
using System.Windows.Forms;
using DirectX_Renderer;

namespace SupremeMemCleaner
{
    public partial class SupremeCleaner : Form
    {
        private String myUser = "DeadW0Lf"; 
        private Overlay overlay;
        public SupremeCleaner()
        {
            InitializeComponent();

            textBox1.Text = myUser;
            CheckBox1.Checked = true;
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

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Overlay.ingame = CheckBox1.Checked? true: false;   
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            myUser = textBox1.Text;
            SupremeMemCleaner.user = myUser;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SupremeMemCleaner.playingAsScav = CheckBox1.Checked ? true : false;
        }
    }
}
