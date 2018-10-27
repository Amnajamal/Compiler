using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace visual1
{
    public partial class loading : Form
    {
        public loading()
        {
            InitializeComponent();
        }

        private void loading_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
           

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loadingBar.Increment(23);
            int i = loadingBar.Value;
            switch (i)
            {
                case 1:
                    loadtxt.Text = "Initialising Application";
                    break;
                case 11:
                    loadtxt.Text = "Loading Interface";
                    break;
                case 22:
                    loadtxt.Text = "Loading Scanner";
                    break;
                case 33:
                    loadtxt.Text = "Loading Parser";
                    break;
                case 44:
                    loadtxt.Text = "Applying Skin";
                    break;
                case 55:
                    loadtxt.Text = "Loading Code Generator";
                    break;
                case 66:
                    loadtxt.Text = "Looking Professional :P";
                    break;
                case 77:
                    loadtxt.Text = "Getting bored";
                    break;
                case 88:
                    loadtxt.Text = "Serioulsy bored now";
                    break;
                case 100:
                    MainForm x = new MainForm();
                    x.Show();
                    this.Hide();
                    timer1.Stop();
                    timer1.Enabled = false;
                    break;
            }

        }
    }
}
