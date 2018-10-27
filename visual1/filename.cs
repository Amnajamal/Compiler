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
    public partial class filename : Form
    {
        public filename()
        {
            InitializeComponent();
            
        }
 

        private void filename_Load(object sender, EventArgs e)
        {
           // this.TopLevel = true;
            this.TopMost = true;
         
            this.ControlBox = false;
            button2.Hide();
            
        }

private string x;
        private void button1_Click(object sender, EventArgs e)
        {
            

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (!textBox1.Text.Equals(x))
                { // text = textBox1.Text;
                    txtbf txt = new txtbf(textBox1.Text);
                    txt.Name = textBox1.Text;

                    //txt.ParentForm = this;
                    txt.MdiParent = this.ParentForm;
                    // txt.Owner = this;
                    //txt.Show(this);
                    txt.Visible = true;
                    this.Hide();
                }
            }
            else
            {
                textBox1.Text = "Please Enter a filename";
                this.Show();
                button2.Show();
                button2.Visible = true;
                button2.CreateControl();
                button2.CreateGraphics();
                x=textBox1.Text;
                
               
            }
               
               
               
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Please Enter a filename";
            this.Hide();
            x = textBox1.Text=null;
        }

      
    }
}
