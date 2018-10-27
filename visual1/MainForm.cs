using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace visual1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public static scanner scan;
        public static parser pars;
        private FileStream output;
        private string filename;

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Line Number", 100);
            listView1.Columns.Add("Error", 500);
            listView1.View = View.Details;
          
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            
            filename filen = new filename();
            filen.MdiParent = this;
            filen.Visible = true;
            this.ActivateMdiChild(filen);
            filen.TopMost = true;
            statusStrip1.Text = "New File Created";
          

            
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            
           
            //openFileDialog1.ShowDialog(this);
            openFileDialog1.Filter = "Text Files|*.txt; *.text; *.doc|All Files|*.*";
            openFileDialog1.AddExtension = true;
            openFileDialog1.InitialDirectory = "C:\\";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string txtfromfile = openfile(openFileDialog1.FileName);
                txtbf txt = new txtbf(openFileDialog1.FileName);
                //txt.Name=openFileDialog1.FileName;
                txt.store = txtfromfile;
                txt.MdiParent = this;
                txt.Visible = true;
                statusStrip1.Text = "File Opened: " + openFileDialog1.FileName;
            }
            else
                statusStrip1.Text = "Unable to Open File";
         

 
        }
        private string openfile(string name)
        {
            StreamReader file = new StreamReader(name);
            return file.ReadToEnd();
        }
        private void savefile()
        {
            txtbf txt = (txtbf)this.ActiveMdiChild;
            if (txt != null)
            {
                statusStrip1.Text = "Saving File: " + txt.Text;
                saveFileDialog1.FileName = txt.Text;
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Text Files|*.txt; *.text; *.doc|All Files|*.*";
                saveFileDialog1.AddExtension = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo n = new FileInfo(saveFileDialog1.FileName);
                    StreamWriter save = n.CreateText();
                    save.Write(txt.store);
                    save.Close();
                }
            }
            else
                statusStrip1.Text = "No active file";
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            savefile();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename filen = new filename();
            filen.MdiParent = this;
            filen.Visible = true;
            this.ActivateMdiChild(filen);
            filen.TopMost = true;
            statusStrip1.Text = "New File Created";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog1.ShowDialog(this);
            openFileDialog1.Filter = "Text Files|*.txt; *.text; *.doc|All Files|*.*";
            openFileDialog1.AddExtension = true;
            openFileDialog1.InitialDirectory = "C:\\";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string txtfromfile = openfile(openFileDialog1.FileName);
                txtbf txt = new txtbf(openFileDialog1.FileName);
                //txt.Name=openFileDialog1.FileName;
                txt.store = txtfromfile;
                txt.MdiParent = this;
                txt.Visible = true;
                statusStrip1.Text = "File Opened: " + openFileDialog1.FileName;
            }
            else
                statusStrip1.Text = "Unable to Open File";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtbf txt = (txtbf)this.ActiveMdiChild;
            if (txt != null)
            {
                statusStrip1.Text = "Saving File: " + txt.Text;
                saveFileDialog1.FileName = txt.Text;
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Text Files|*.txt; *.text; *.doc|All Files|*.*";
                saveFileDialog1.AddExtension = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo n = new FileInfo(saveFileDialog1.FileName);
                    StreamWriter save = n.CreateText();
                    save.Write(txt.store);
                    save.Close();
                }
            }
            else
                statusStrip1.Text = "No active file";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStrip.Visible == true)
                toolStrip.Visible = false;
            else toolStrip.Visible = true;
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusStrip1.Visible == true)
                statusStrip1.Visible = false;
            else
                statusStrip1.Visible = true;
        }

        private void errorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Visible == true)
                listView1.Visible = false;
            else listView1.Visible = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            txtbf current = (txtbf)this.ActiveMdiChild;
            if (current != null)
            {
                if (!string.IsNullOrEmpty(current.store))
                {
                    string x = current.store;
                    //  MessageBox.Show(x);
                    scan = new scanner(x.ToCharArray());
                }
                for (int i = 0; i < scan.er.Count; i++)
                {
                    ListViewItem list = new ListViewItem(scan.er[i].ln);
                    list.SubItems.Add(scan.er[i].error);
                    this.listView1.Items.Add(list);
                    this.listView1.GridLines = true;
                    this.listView1.CreateGraphics();

                }
            }
            else
            {
                statusStrip1.Text = "No active file";
                ListViewItem list = new ListViewItem("error");
                list.SubItems.Add("No active file");
                this.listView1.Items.Add(list);
                this.listView1.GridLines = true;
                this.listView1.CreateGraphics();
            }
        }

        private void tokeniseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            txtbf current = (txtbf)this.ActiveMdiChild;
            if (current != null)
            {
                if (!string.IsNullOrEmpty(current.store))
                {
                    string x = current.store;
                    //  MessageBox.Show(x);
                    scan = new scanner(x.ToCharArray());
                }
                for (int i = 0; i < scan.er.Count; i++)
                {
                    ListViewItem list = new ListViewItem(scan.er[i].ln);
                    list.SubItems.Add(scan.er[i].error);
                    this.listView1.Items.Add(list);
                    this.listView1.GridLines = true;
                    this.listView1.CreateGraphics();

                }
            }
           
               else
            {
                statusStrip1.Text = "No active file";
                ListViewItem list = new ListViewItem("error");
                list.SubItems.Add("No active file");
                this.listView1.Items.Add(list);
                this.listView1.GridLines = true;
                this.listView1.CreateGraphics();
            }
        }

        private void analyseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (scan != null)
            {
                pars = new parser(scan.Tokens);
                for (int i = 0; i < scan.er.Count; i++)
                {
                    pars.errors.Add(scan.er[i]);
                }
                for (int i = 0; i < pars.errors.Count; i++)
                {
                    ListViewItem list = new ListViewItem(pars.errors[i].ln);
                    list.SubItems.Add(pars.errors[i].error);
                    this.listView1.Items.Add(list);
                    this.listView1.GridLines = true;
                    this.listView1.CreateGraphics();

                }
            }
            else
            {
                statusStrip1.Text = "No active file or Tokenise File First";
                ListViewItem list = new ListViewItem("error");
                list.SubItems.Add("No active file");
                this.listView1.Items.Add(list);
                this.listView1.GridLines = true;
                this.listView1.CreateGraphics();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (scan != null)
            {
                pars = new parser(scan.Tokens);
                for (int i = 0; i < scan.er.Count; i++)
                {
                    pars.errors.Add(scan.er[i]);
                }

                for (int i = 0; i < pars.errors.Count; i++)
                {
                    ListViewItem list = new ListViewItem(pars.errors[i].ln);
                    list.SubItems.Add(pars.errors[i].error);
                    this.listView1.Items.Add(list);
                    this.listView1.GridLines = true;
                    this.listView1.CreateGraphics();

                }
            }
            else
            {
                statusStrip1.Text = "No active file or Tokenise File First";
                ListViewItem list = new ListViewItem("error");
                list.SubItems.Add("No active file");
                this.listView1.Items.Add(list);
                this.listView1.GridLines = true;
                this.listView1.CreateGraphics();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {  txtbf x = (txtbf)this.ActiveMdiChild;
        if (pars.errors.Count != 0)
            MessageBox.Show("Fix Errors First");
        else
            {
                if (x != null)
                {
                    
                    saveFileDialog1.Filter = "Text Files (*.txt|*.txt";
                    DialogResult result = saveFileDialog1.ShowDialog();
                    //string filename;
                    saveFileDialog1.CheckFileExists = false;

                    if (result == DialogResult.Cancel)
                        return;
                    filename = saveFileDialog1.FileName; //gets the file name


                    object obj = x.store;
                    byte[] b = new byte[100];

                    output = new FileStream(Path.GetFullPath(filename), FileMode.Create); //gets the path and creates the file

                    output.Close();



                    MessageBox.Show("Created Successfully");
                    string[] arr = new string[1];
                    arr[0] = x.store; //stores the input text in string arr[0]
                    System.IO.File.WriteAllLines(Path.GetFullPath(filename), arr);//writes the text of textbox into notepad

                    // MessageBox.Show("added successfully");
                    //  MessageBox.Show(Path.GetFullPath(filename).ToString());
                    
                    CodeGen codeGen = new CodeGen(pars.Result, Path.GetFileNameWithoutExtension(filename) + ".exe",pars.counts); // codegen method call

                }
                else
                {
                    listView1.Items.Clear();
                    statusStrip1.Text = "No active file";
                    ListViewItem list = new ListViewItem("error");
                    list.SubItems.Add("No active file");
                    this.listView1.Items.Add(list);
                    this.listView1.GridLines = true;
                    this.listView1.CreateGraphics();
                }
               
            }
           
        }

        private void execuToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(pars.errors.Count.ToString());
            if(pars.errors.Count!=0)
                MessageBox.Show("Fix Errors First");
            else
            {
                txtbf x = (txtbf)this.ActiveMdiChild; if (x != null)
                //if(
                {
                    saveFileDialog1.Filter = "Text Files (*.txt|*.txt";
                    DialogResult result = saveFileDialog1.ShowDialog();
                    //string filename;
                    saveFileDialog1.CheckFileExists = false;


                    if (result == DialogResult.Cancel)
                        return;
                    filename = saveFileDialog1.FileName; //gets the file name


                    object obj = x.store;
                    byte[] b = new byte[100];

                    output = new FileStream(Path.GetFullPath(filename), FileMode.Create); //gets the path and creates the file

                    output.Close();



                    MessageBox.Show("Created Successfully");
                    string[] arr = new string[1];
                    arr[0] = x.store; //stores the input text in string arr[0]
                    System.IO.File.WriteAllLines(Path.GetFullPath(filename), arr);//writes the text of textbox into notepad

                    //  MessageBox.Show("added successfully");
                    // MessageBox.Show(Path.GetFullPath(filename).ToString());
                    CodeGen codeGen = new CodeGen(pars.Result, Path.GetFileNameWithoutExtension(filename) + ".exe",pars.counts); // codegen method call
                }
                else
                {
                    listView1.Items.Clear();
                    statusStrip1.Text = "No active file";
                    ListViewItem list = new ListViewItem("error");
                    list.SubItems.Add("No active file");
                    this.listView1.Items.Add(list);
                    this.listView1.GridLines = true;
                    this.listView1.CreateGraphics();
                }
              

            }
       
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About nform = new About();
            nform.MdiParent=this;
            this.ActivateMdiChild(nform);

            nform.Show();

        }

        private void syntaxRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"documentation.doc");
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"documentation.doc");
        }

        
        

      


            
            


       
    }
}
