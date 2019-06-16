using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApexMemoryCleaner
{
    public partial class ApexMemoryCleaner : Form
    {
        public ApexMemoryCleaner()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (button1.Text)
            {
                case "Run Application":
                    button1.Text = "Stop Application";
                    break;

                case "Stop Application":
                    button1.Text = "Run Application";
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
