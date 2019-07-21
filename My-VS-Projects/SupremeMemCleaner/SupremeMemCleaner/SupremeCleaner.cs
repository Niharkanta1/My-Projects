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
            checkBox3.Enabled = false;
            checkBox4.Checked = true;
            checkBox2.Checked = true;

            //trackbar for AimCorrection
            trackBar1.Maximum = 10;
            trackBar1.TickFrequency = 1;
            trackBar1.LargeChange = 1;
            trackBar1.SmallChange = 1;
            trackBar1.Value = 4;
            label1.Text = "Aim Correction: "+ trackBar1.Value;

            //trackbar for DrawDistance
            trackBar2.Maximum = 300;
            trackBar2.Minimum = 140;
            trackBar2.TickFrequency = 20;
            trackBar2.LargeChange = 20;
            trackBar2.SmallChange = 10;
            trackBar2.Value = 220;
            label2.Text = "Draw distance(" + trackBar2.Value + "m)";

        }

        private void Run_Click(object sender, EventArgs e)
        { 
            switch (Button1.Text)
            {
                case "Run":
                    overlay = new Overlay();
                    overlay.Show();
                    Button1.Text = "Stop";
                    break;

                case "Stop":
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
            if (checkBox2.Checked)
            {
                SupremeMemCleaner.aimbot = true;
                checkBox3.Enabled = true;
                checkBox3.Checked = true;
            }
            else
            {
                SupremeMemCleaner.aimbot = false;
                checkBox3.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SupremeMemCleaner.smooth = checkBox2.Checked ? true : false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SupremeMemCleaner.playingAsScav = checkBox4.Checked ? true : false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "Aim Correction: "+ trackBar1.Value;
            SupremeMemCleaner.aimCorrection = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = "Draw distance("+ trackBar2.Value + "m)";
            SupremeMemCleaner.drawDistance = trackBar2.Value;
        }
    }
}
