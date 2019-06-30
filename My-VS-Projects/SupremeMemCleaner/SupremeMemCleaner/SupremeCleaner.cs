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
    }
}
