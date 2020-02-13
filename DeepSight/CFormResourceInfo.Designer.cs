namespace DeepSight
{
	partial class CFormResourceInfo
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFormResourceInfo));
            this.pictureProgressCPU = new System.Windows.Forms.PictureBox();
            this.pictureProgressMemory = new System.Windows.Forms.PictureBox();
            this.pictureProgressHddC = new System.Windows.Forms.PictureBox();
            this.pictureProgressHddD = new System.Windows.Forms.PictureBox();
            this.BtnTitleCpu = new System.Windows.Forms.Button();
            this.BtnTitleMemory = new System.Windows.Forms.Button();
            this.BtnTitleDriverC = new System.Windows.Forms.Button();
            this.BtnTitleDriverD = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressHddC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressHddD)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureProgressCPU
            // 
            this.pictureProgressCPU.BackColor = System.Drawing.Color.Black;
            this.pictureProgressCPU.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureProgressCPU.Location = new System.Drawing.Point(86, 2);
            this.pictureProgressCPU.Name = "pictureProgressCPU";
            this.pictureProgressCPU.Size = new System.Drawing.Size(160, 28);
            this.pictureProgressCPU.TabIndex = 7;
            this.pictureProgressCPU.TabStop = false;
            this.pictureProgressCPU.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureProgressCPU_Paint);
            // 
            // pictureProgressMemory
            // 
            this.pictureProgressMemory.BackColor = System.Drawing.Color.Black;
            this.pictureProgressMemory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureProgressMemory.Location = new System.Drawing.Point(86, 32);
            this.pictureProgressMemory.Name = "pictureProgressMemory";
            this.pictureProgressMemory.Size = new System.Drawing.Size(160, 28);
            this.pictureProgressMemory.TabIndex = 7;
            this.pictureProgressMemory.TabStop = false;
            this.pictureProgressMemory.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureProgressMemory_Paint);
            // 
            // pictureProgressHddC
            // 
            this.pictureProgressHddC.BackColor = System.Drawing.Color.Black;
            this.pictureProgressHddC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureProgressHddC.Location = new System.Drawing.Point(86, 62);
            this.pictureProgressHddC.Name = "pictureProgressHddC";
            this.pictureProgressHddC.Size = new System.Drawing.Size(160, 28);
            this.pictureProgressHddC.TabIndex = 7;
            this.pictureProgressHddC.TabStop = false;
            this.pictureProgressHddC.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureProgressHddC_Paint);
            // 
            // pictureProgressHddD
            // 
            this.pictureProgressHddD.BackColor = System.Drawing.Color.Black;
            this.pictureProgressHddD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureProgressHddD.Location = new System.Drawing.Point(86, 92);
            this.pictureProgressHddD.Name = "pictureProgressHddD";
            this.pictureProgressHddD.Size = new System.Drawing.Size(160, 28);
            this.pictureProgressHddD.TabIndex = 7;
            this.pictureProgressHddD.TabStop = false;
            this.pictureProgressHddD.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureProgressHddD_Paint);
            // 
            // BtnTitleCpu
            // 
            this.BtnTitleCpu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCpu.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Bold);
            this.BtnTitleCpu.Location = new System.Drawing.Point(1, 2);
            this.BtnTitleCpu.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleCpu.Name = "BtnTitleCpu";
            this.BtnTitleCpu.Size = new System.Drawing.Size(84, 28);
            this.BtnTitleCpu.TabIndex = 35;
            this.BtnTitleCpu.Text = "CPU";
            this.BtnTitleCpu.UseVisualStyleBackColor = true;
            // 
            // BtnTitleMemory
            // 
            this.BtnTitleMemory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleMemory.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Bold);
            this.BtnTitleMemory.Location = new System.Drawing.Point(1, 32);
            this.BtnTitleMemory.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleMemory.Name = "BtnTitleMemory";
            this.BtnTitleMemory.Size = new System.Drawing.Size(84, 28);
            this.BtnTitleMemory.TabIndex = 35;
            this.BtnTitleMemory.Text = "MEMORY";
            this.BtnTitleMemory.UseVisualStyleBackColor = true;
            // 
            // BtnTitleDriverC
            // 
            this.BtnTitleDriverC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleDriverC.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Bold);
            this.BtnTitleDriverC.Location = new System.Drawing.Point(1, 62);
            this.BtnTitleDriverC.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleDriverC.Name = "BtnTitleDriverC";
            this.BtnTitleDriverC.Size = new System.Drawing.Size(84, 28);
            this.BtnTitleDriverC.TabIndex = 35;
            this.BtnTitleDriverC.Text = "DRIVER C";
            this.BtnTitleDriverC.UseVisualStyleBackColor = true;
            // 
            // BtnTitleDriverD
            // 
            this.BtnTitleDriverD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleDriverD.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Bold);
            this.BtnTitleDriverD.Location = new System.Drawing.Point(1, 92);
            this.BtnTitleDriverD.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleDriverD.Name = "BtnTitleDriverD";
            this.BtnTitleDriverD.Size = new System.Drawing.Size(84, 28);
            this.BtnTitleDriverD.TabIndex = 35;
            this.BtnTitleDriverD.Text = "DRIVER D";
            this.BtnTitleDriverD.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CFormResourceInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(247, 123);
            this.ControlBox = false;
            this.Controls.Add(this.BtnTitleDriverD);
            this.Controls.Add(this.BtnTitleDriverC);
            this.Controls.Add(this.BtnTitleMemory);
            this.Controls.Add(this.BtnTitleCpu);
            this.Controls.Add(this.pictureProgressHddD);
            this.Controls.Add(this.pictureProgressHddC);
            this.Controls.Add(this.pictureProgressMemory);
            this.Controls.Add(this.pictureProgressCPU);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CFormResourceInfo";
            this.Text = "CDialogLoadingWindow";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CFormResourceInfo_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressHddC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgressHddD)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PictureBox pictureProgressCPU;
        private System.Windows.Forms.PictureBox pictureProgressMemory;
        private System.Windows.Forms.PictureBox pictureProgressHddC;
        private System.Windows.Forms.PictureBox pictureProgressHddD;
        private System.Windows.Forms.Button BtnTitleCpu;
        private System.Windows.Forms.Button BtnTitleMemory;
        private System.Windows.Forms.Button BtnTitleDriverC;
        private System.Windows.Forms.Button BtnTitleDriverD;
        private System.Windows.Forms.Timer timer1;
    }
}