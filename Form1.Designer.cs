namespace OS
{
    partial class Form1
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
            this.InputButton = new System.Windows.Forms.Button();
            this.HPFButton = new System.Windows.Forms.Button();
            this.FCFSButton = new System.Windows.Forms.Button();
            this.RRButton = new System.Windows.Forms.Button();
            this.SRTNButton = new System.Windows.Forms.Button();
            this.textBoxIn = new System.Windows.Forms.TextBox();
            this.inputfile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxQ = new System.Windows.Forms.TextBox();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InputButton
            // 
            this.InputButton.Location = new System.Drawing.Point(12, 58);
            this.InputButton.Name = "InputButton";
            this.InputButton.Size = new System.Drawing.Size(75, 23);
            this.InputButton.TabIndex = 0;
            this.InputButton.Text = "Confirm";
            this.InputButton.UseVisualStyleBackColor = true;
            this.InputButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // HPFButton
            // 
            this.HPFButton.Location = new System.Drawing.Point(602, 173);
            this.HPFButton.Name = "HPFButton";
            this.HPFButton.Size = new System.Drawing.Size(75, 23);
            this.HPFButton.TabIndex = 1;
            this.HPFButton.Text = "HPF";
            this.HPFButton.UseVisualStyleBackColor = true;
            this.HPFButton.Click += new System.EventHandler(this.HPFButton_Click);
            // 
            // FCFSButton
            // 
            this.FCFSButton.Location = new System.Drawing.Point(602, 207);
            this.FCFSButton.Name = "FCFSButton";
            this.FCFSButton.Size = new System.Drawing.Size(75, 23);
            this.FCFSButton.TabIndex = 2;
            this.FCFSButton.Text = "FCFS";
            this.FCFSButton.UseVisualStyleBackColor = true;
            this.FCFSButton.Click += new System.EventHandler(this.FCFSButton_Click);
            // 
            // RRButton
            // 
            this.RRButton.Location = new System.Drawing.Point(602, 242);
            this.RRButton.Name = "RRButton";
            this.RRButton.Size = new System.Drawing.Size(75, 23);
            this.RRButton.TabIndex = 3;
            this.RRButton.Text = "RR";
            this.RRButton.UseVisualStyleBackColor = true;
            this.RRButton.Click += new System.EventHandler(this.RRButton_Click);
            // 
            // SRTNButton
            // 
            this.SRTNButton.Location = new System.Drawing.Point(602, 271);
            this.SRTNButton.Name = "SRTNButton";
            this.SRTNButton.Size = new System.Drawing.Size(75, 23);
            this.SRTNButton.TabIndex = 4;
            this.SRTNButton.Text = "SRTN";
            this.SRTNButton.UseVisualStyleBackColor = true;
            this.SRTNButton.Click += new System.EventHandler(this.SRTNButton_Click);
            // 
            // textBoxIn
            // 
            this.textBoxIn.Location = new System.Drawing.Point(102, 30);
            this.textBoxIn.Name = "textBoxIn";
            this.textBoxIn.Size = new System.Drawing.Size(100, 20);
            this.textBoxIn.TabIndex = 5;
            // 
            // inputfile
            // 
            this.inputfile.AutoSize = true;
            this.inputfile.Location = new System.Drawing.Point(12, 33);
            this.inputfile.Name = "inputfile";
            this.inputfile.Size = new System.Drawing.Size(82, 13);
            this.inputfile.TabIndex = 6;
            this.inputfile.Text = "Input File Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(457, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Switch Time";
            // 
            // textBoxS
            // 
            this.textBoxS.Location = new System.Drawing.Point(548, 28);
            this.textBoxS.Name = "textBoxS";
            this.textBoxS.Size = new System.Drawing.Size(100, 20);
            this.textBoxS.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(456, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Quantum Time";
            // 
            // textBoxQ
            // 
            this.textBoxQ.Location = new System.Drawing.Point(548, 60);
            this.textBoxQ.Name = "textBoxQ";
            this.textBoxQ.Size = new System.Drawing.Size(100, 20);
            this.textBoxQ.TabIndex = 9;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 97);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(579, 310);
            this.zedGraphControl1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(656, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Sec";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(656, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Sec";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 419);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxQ);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxS);
            this.Controls.Add(this.inputfile);
            this.Controls.Add(this.textBoxIn);
            this.Controls.Add(this.SRTNButton);
            this.Controls.Add(this.RRButton);
            this.Controls.Add(this.FCFSButton);
            this.Controls.Add(this.HPFButton);
            this.Controls.Add(this.InputButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InputButton;
        private System.Windows.Forms.Button HPFButton;
        private System.Windows.Forms.Button FCFSButton;
        private System.Windows.Forms.Button RRButton;
        private System.Windows.Forms.Button SRTNButton;
        private System.Windows.Forms.TextBox textBoxIn;
        private System.Windows.Forms.Label inputfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxQ;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

