﻿using System;
using System.Windows.Forms;
using DirectX_Renderer;

namespace SupremeMemCleaner
{
    public partial class SupremeCleaner : Form
    {
        private String myUser = "DeadW0Lf";
        private int aimCorrection = 3;
        private Overlay overlay;
        public SupremeCleaner()
        {
            InitializeComponent();
            textBox1.Text = myUser;
            textBox2.Text = aimCorrection.ToString();
            checkBox3.Enabled = false;
            checkBox4.Checked = true;
            checkBox2.Checked = true;
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                aimCorrection = Int32.Parse(textBox2.Text);
            }
            catch(Exception abc)
            {
                MessageBox.Show("Please enter only numbers.");
                textBox2.Text = aimCorrection.ToString();
            }           
            SupremeMemCleaner.aimCorrection = aimCorrection;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                textBox2.Text = "9";
            else
                textBox2.Text = aimCorrection.ToString();
        }
    }
}
