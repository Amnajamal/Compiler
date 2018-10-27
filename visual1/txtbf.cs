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
    public partial class txtbf : Form
    {
        public txtbf()
        {
            InitializeComponent();
        }
        public txtbf(string x)
        {
            InitializeComponent();
            this.Name = x;
            this.Text = x;
        }
       
        public string store
        {
            get
            {
                return this.richTextBox1.Text;
            }
            set
            {
                this.richTextBox1.Text = value;
            }
        }
    }
}
