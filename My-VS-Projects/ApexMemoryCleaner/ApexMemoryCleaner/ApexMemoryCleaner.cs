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
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ScreenCapture mainApp;
        public ApexMemoryCleaner()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            switch (button1.Text)
            {
                case "Run Application":
                    logger.Info("Starting The Application....");
                    mainApp = new ScreenCapture();
                    mainApp.Show();
                    button1.Text = "Stop Application";
                    break;

                case "Stop Application":
                    logger.Info("Stopping The Application....");
                    ScreenCapture.ingame = false;
                    mainApp.Close();
                    button1.Text = "Run Application";
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked){
                ScreenCapture.ingame = true;
            }
            else
            {
                ScreenCapture.ingame = false;
            }
        }


    }
}
