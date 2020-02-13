namespace DeepSight
{
	partial class CDialogLoadingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDialogLoadingWindow));
            this.labelMessage = new System.Windows.Forms.Label();
            this.labelCopyLight = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureProgress = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgress)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.BackColor = System.Drawing.Color.Black;
            this.labelMessage.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMessage.ForeColor = System.Drawing.Color.White;
            this.labelMessage.Location = new System.Drawing.Point(7, 216);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(185, 48);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "Loading...";
            // 
            // labelCopyLight
            // 
            this.labelCopyLight.AutoSize = true;
            this.labelCopyLight.BackColor = System.Drawing.Color.Black;
            this.labelCopyLight.Font = new System.Drawing.Font("굴림", 7F);
            this.labelCopyLight.ForeColor = System.Drawing.Color.White;
            this.labelCopyLight.Location = new System.Drawing.Point(17, 360);
            this.labelCopyLight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCopyLight.Name = "labelCopyLight";
            this.labelCopyLight.Size = new System.Drawing.Size(370, 14);
            this.labelCopyLight.TabIndex = 1;
            this.labelCopyLight.Text = "CopyLight ⓒ 2018-2019 HoonLab. All rights reserved. ";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.Black;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(17, 27);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(684, 81);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "INSPECTION PROGRAM";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.BackColor = System.Drawing.Color.Black;
            this.labelVersion.Font = new System.Drawing.Font("굴림", 12F);
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(27, 129);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(271, 24);
            this.labelVersion.TabIndex = 1;
            this.labelVersion.Text = "Ver.1.0.7.201904101700";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.White;
            this.progressBar1.Location = new System.Drawing.Point(826, 202);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.MarqueeAnimationSpeed = 10;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(170, 62);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            // 
            // pictureProgress
            // 
            this.pictureProgress.BackColor = System.Drawing.Color.White;
            this.pictureProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureProgress.Location = new System.Drawing.Point(16, 270);
            this.pictureProgress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureProgress.Name = "pictureProgress";
            this.pictureProgress.Size = new System.Drawing.Size(980, 73);
            this.pictureProgress.TabIndex = 7;
            this.pictureProgress.TabStop = false;
            this.pictureProgress.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureProgress_Paint);
            // 
            // CDialogLoadingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1009, 392);
            this.ControlBox = false;
            this.Controls.Add(this.pictureProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.labelCopyLight);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CDialogLoadingWindow";
            this.Text = "CDialogLoadingWindow";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogLoadingWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureProgress)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.Label labelCopyLight;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.PictureBox pictureProgress;
	}
}