namespace visual1
{
    partial class loading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loading));
            this.loadingBar = new System.Windows.Forms.ProgressBar();
            this.loadtxt = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // loadingBar
            // 
            this.loadingBar.BackColor = System.Drawing.Color.Maroon;
            this.loadingBar.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.loadingBar.Location = new System.Drawing.Point(19, 22);
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(407, 24);
            this.loadingBar.TabIndex = 0;
            // 
            // loadtxt
            // 
            this.loadtxt.AutoSize = true;
            this.loadtxt.Location = new System.Drawing.Point(179, 56);
            this.loadtxt.Name = "loadtxt";
            this.loadtxt.Size = new System.Drawing.Size(10, 13);
            this.loadtxt.TabIndex = 1;
            this.loadtxt.Text = " ";
            this.loadtxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 78);
            this.Controls.Add(this.loadtxt);
            this.Controls.Add(this.loadingBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "loading";
            this.Text = "loading";
            this.Load += new System.EventHandler(this.loading_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar loadingBar;
        private System.Windows.Forms.Label loadtxt;
        private System.Windows.Forms.Timer timer1;
    }
}