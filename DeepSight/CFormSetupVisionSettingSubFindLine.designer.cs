namespace DeepSight
{
	partial class CFormSetupVisionSettingSubFindLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFormSetupVisionSettingSubFindLine));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnTitleFindLineSetting = new System.Windows.Forms.Button();
            this.BtnNumber = new System.Windows.Forms.Button();
            this.BtnTitleNumber = new System.Windows.Forms.Button();
            this.BtnTitleIgnoreNumber = new System.Windows.Forms.Button();
            this.BtnIgnoreNumber = new System.Windows.Forms.Button();
            this.BtnTitleContrastThreshold = new System.Windows.Forms.Button();
            this.BtnContrastThreshold = new System.Windows.Forms.Button();
            this.BtnTitleFilterHalfSizePixels = new System.Windows.Forms.Button();
            this.BtnFilterHalfSizePixels = new System.Windows.Forms.Button();
            this.BtnTitlePolarity = new System.Windows.Forms.Button();
            this.BtnPolarity = new System.Windows.Forms.Button();
            this.BtnSearchDirection = new System.Windows.Forms.Button();
            this.cogDisplayRunImage = new Cognex.VisionPro.Display.CogDisplay();
            this.BtnRun = new System.Windows.Forms.Button();
            this.BtnImageGrab = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayRunImage)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnTitleFindLineSetting
            // 
            this.BtnTitleFindLineSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleFindLineSetting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleFindLineSetting.Location = new System.Drawing.Point(1, 1);
            this.BtnTitleFindLineSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitleFindLineSetting.Name = "BtnTitleFindLineSetting";
            this.BtnTitleFindLineSetting.Size = new System.Drawing.Size(310, 46);
            this.BtnTitleFindLineSetting.TabIndex = 3;
            this.BtnTitleFindLineSetting.Text = "FIND LINE SETTING";
            this.BtnTitleFindLineSetting.UseVisualStyleBackColor = true;
            this.BtnTitleFindLineSetting.Click += new System.EventHandler(this.BtnTitleFindLineSetting_Click);
            // 
            // BtnNumber
            // 
            this.BtnNumber.BackColor = System.Drawing.Color.White;
            this.BtnNumber.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnNumber.Location = new System.Drawing.Point(222, 103);
            this.BtnNumber.Margin = new System.Windows.Forms.Padding(2);
            this.BtnNumber.Name = "BtnNumber";
            this.BtnNumber.Size = new System.Drawing.Size(89, 46);
            this.BtnNumber.TabIndex = 38;
            this.BtnNumber.Text = "0";
            this.BtnNumber.UseVisualStyleBackColor = true;
            this.BtnNumber.Click += new System.EventHandler(this.BtnNumber_Click);
            // 
            // BtnTitleNumber
            // 
            this.BtnTitleNumber.BackColor = System.Drawing.Color.White;
            this.BtnTitleNumber.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleNumber.Location = new System.Drawing.Point(1, 103);
            this.BtnTitleNumber.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleNumber.Name = "BtnTitleNumber";
            this.BtnTitleNumber.Size = new System.Drawing.Size(217, 46);
            this.BtnTitleNumber.TabIndex = 39;
            this.BtnTitleNumber.Text = "NUMBER";
            this.BtnTitleNumber.UseVisualStyleBackColor = true;
            // 
            // BtnTitleIgnoreNumber
            // 
            this.BtnTitleIgnoreNumber.BackColor = System.Drawing.Color.White;
            this.BtnTitleIgnoreNumber.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleIgnoreNumber.Location = new System.Drawing.Point(1, 153);
            this.BtnTitleIgnoreNumber.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleIgnoreNumber.Name = "BtnTitleIgnoreNumber";
            this.BtnTitleIgnoreNumber.Size = new System.Drawing.Size(217, 46);
            this.BtnTitleIgnoreNumber.TabIndex = 39;
            this.BtnTitleIgnoreNumber.Text = "IGNORE NUMBER";
            this.BtnTitleIgnoreNumber.UseVisualStyleBackColor = true;
            // 
            // BtnIgnoreNumber
            // 
            this.BtnIgnoreNumber.BackColor = System.Drawing.Color.White;
            this.BtnIgnoreNumber.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnIgnoreNumber.Location = new System.Drawing.Point(222, 153);
            this.BtnIgnoreNumber.Margin = new System.Windows.Forms.Padding(2);
            this.BtnIgnoreNumber.Name = "BtnIgnoreNumber";
            this.BtnIgnoreNumber.Size = new System.Drawing.Size(89, 46);
            this.BtnIgnoreNumber.TabIndex = 38;
            this.BtnIgnoreNumber.Text = "0";
            this.BtnIgnoreNumber.UseVisualStyleBackColor = true;
            this.BtnIgnoreNumber.Click += new System.EventHandler(this.BtnIgnoreNumber_Click);
            // 
            // BtnTitleContrastThreshold
            // 
            this.BtnTitleContrastThreshold.BackColor = System.Drawing.Color.White;
            this.BtnTitleContrastThreshold.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleContrastThreshold.Location = new System.Drawing.Point(1, 203);
            this.BtnTitleContrastThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleContrastThreshold.Name = "BtnTitleContrastThreshold";
            this.BtnTitleContrastThreshold.Size = new System.Drawing.Size(217, 46);
            this.BtnTitleContrastThreshold.TabIndex = 39;
            this.BtnTitleContrastThreshold.Text = "CONTRAST THRESHOLD";
            this.BtnTitleContrastThreshold.UseVisualStyleBackColor = true;
            // 
            // BtnContrastThreshold
            // 
            this.BtnContrastThreshold.BackColor = System.Drawing.Color.White;
            this.BtnContrastThreshold.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnContrastThreshold.Location = new System.Drawing.Point(222, 203);
            this.BtnContrastThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.BtnContrastThreshold.Name = "BtnContrastThreshold";
            this.BtnContrastThreshold.Size = new System.Drawing.Size(89, 46);
            this.BtnContrastThreshold.TabIndex = 38;
            this.BtnContrastThreshold.Text = "0";
            this.BtnContrastThreshold.UseVisualStyleBackColor = true;
            this.BtnContrastThreshold.Click += new System.EventHandler(this.BtnContrastThreshold_Click);
            // 
            // BtnTitleFilterHalfSizePixels
            // 
            this.BtnTitleFilterHalfSizePixels.BackColor = System.Drawing.Color.White;
            this.BtnTitleFilterHalfSizePixels.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleFilterHalfSizePixels.Location = new System.Drawing.Point(1, 253);
            this.BtnTitleFilterHalfSizePixels.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleFilterHalfSizePixels.Name = "BtnTitleFilterHalfSizePixels";
            this.BtnTitleFilterHalfSizePixels.Size = new System.Drawing.Size(217, 46);
            this.BtnTitleFilterHalfSizePixels.TabIndex = 39;
            this.BtnTitleFilterHalfSizePixels.Text = "FILTER HALF SIZE PIXELS";
            this.BtnTitleFilterHalfSizePixels.UseVisualStyleBackColor = true;
            // 
            // BtnFilterHalfSizePixels
            // 
            this.BtnFilterHalfSizePixels.BackColor = System.Drawing.Color.White;
            this.BtnFilterHalfSizePixels.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnFilterHalfSizePixels.Location = new System.Drawing.Point(222, 253);
            this.BtnFilterHalfSizePixels.Margin = new System.Windows.Forms.Padding(2);
            this.BtnFilterHalfSizePixels.Name = "BtnFilterHalfSizePixels";
            this.BtnFilterHalfSizePixels.Size = new System.Drawing.Size(89, 46);
            this.BtnFilterHalfSizePixels.TabIndex = 38;
            this.BtnFilterHalfSizePixels.Text = "0";
            this.BtnFilterHalfSizePixels.UseVisualStyleBackColor = true;
            this.BtnFilterHalfSizePixels.Click += new System.EventHandler(this.BtnFilterHalfSizePixels_Click);
            // 
            // BtnTitlePolarity
            // 
            this.BtnTitlePolarity.BackColor = System.Drawing.Color.White;
            this.BtnTitlePolarity.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitlePolarity.Location = new System.Drawing.Point(1, 303);
            this.BtnTitlePolarity.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitlePolarity.Name = "BtnTitlePolarity";
            this.BtnTitlePolarity.Size = new System.Drawing.Size(153, 46);
            this.BtnTitlePolarity.TabIndex = 39;
            this.BtnTitlePolarity.Text = "POLARITY";
            this.BtnTitlePolarity.UseVisualStyleBackColor = true;
            // 
            // BtnPolarity
            // 
            this.BtnPolarity.BackColor = System.Drawing.Color.White;
            this.BtnPolarity.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPolarity.Location = new System.Drawing.Point(158, 303);
            this.BtnPolarity.Margin = new System.Windows.Forms.Padding(2);
            this.BtnPolarity.Name = "BtnPolarity";
            this.BtnPolarity.Size = new System.Drawing.Size(153, 46);
            this.BtnPolarity.TabIndex = 38;
            this.BtnPolarity.Text = "DARK TO LIGHT";
            this.BtnPolarity.UseVisualStyleBackColor = true;
            this.BtnPolarity.Click += new System.EventHandler(this.BtnPolarity_Click);
            // 
            // BtnSearchDirection
            // 
            this.BtnSearchDirection.BackColor = System.Drawing.Color.White;
            this.BtnSearchDirection.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSearchDirection.Location = new System.Drawing.Point(1, 353);
            this.BtnSearchDirection.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSearchDirection.Name = "BtnSearchDirection";
            this.BtnSearchDirection.Size = new System.Drawing.Size(310, 46);
            this.BtnSearchDirection.TabIndex = 39;
            this.BtnSearchDirection.Text = "SEARCH DIRECTION";
            this.BtnSearchDirection.UseVisualStyleBackColor = true;
            this.BtnSearchDirection.Click += new System.EventHandler(this.BtnSearchDirection_Click);
            // 
            // cogDisplayRunImage
            // 
            this.cogDisplayRunImage.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplayRunImage.ColorMapLowerRoiLimit = 0D;
            this.cogDisplayRunImage.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplayRunImage.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplayRunImage.ColorMapUpperRoiLimit = 1D;
            this.cogDisplayRunImage.DoubleTapZoomCycleLength = 2;
            this.cogDisplayRunImage.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplayRunImage.Location = new System.Drawing.Point(1, 550);
            this.cogDisplayRunImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cogDisplayRunImage.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplayRunImage.MouseWheelSensitivity = 1D;
            this.cogDisplayRunImage.Name = "cogDisplayRunImage";
            this.cogDisplayRunImage.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplayRunImage.OcxState")));
            this.cogDisplayRunImage.Size = new System.Drawing.Size(310, 239);
            this.cogDisplayRunImage.TabIndex = 41;
            this.cogDisplayRunImage.TabStop = false;
            // 
            // BtnRun
            // 
            this.BtnRun.BackColor = System.Drawing.Color.White;
            this.BtnRun.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnRun.Location = new System.Drawing.Point(1, 794);
            this.BtnRun.Margin = new System.Windows.Forms.Padding(2);
            this.BtnRun.Name = "BtnRun";
            this.BtnRun.Size = new System.Drawing.Size(310, 46);
            this.BtnRun.TabIndex = 42;
            this.BtnRun.Text = "RUN";
            this.BtnRun.UseVisualStyleBackColor = true;
            this.BtnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // BtnImageGrab
            // 
            this.BtnImageGrab.BackColor = System.Drawing.Color.White;
            this.BtnImageGrab.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnImageGrab.Location = new System.Drawing.Point(1, 53);
            this.BtnImageGrab.Margin = new System.Windows.Forms.Padding(2);
            this.BtnImageGrab.Name = "BtnImageGrab";
            this.BtnImageGrab.Size = new System.Drawing.Size(310, 46);
            this.BtnImageGrab.TabIndex = 39;
            this.BtnImageGrab.Text = "IMAGE GRAB";
            this.BtnImageGrab.UseVisualStyleBackColor = true;
            this.BtnImageGrab.Click += new System.EventHandler(this.BtnImageGrab_Click);
            // 
            // CFormSetupVisionSettingSubFindLine
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(312, 840);
            this.Controls.Add(this.BtnRun);
            this.Controls.Add(this.cogDisplayRunImage);
            this.Controls.Add(this.BtnPolarity);
            this.Controls.Add(this.BtnFilterHalfSizePixels);
            this.Controls.Add(this.BtnContrastThreshold);
            this.Controls.Add(this.BtnIgnoreNumber);
            this.Controls.Add(this.BtnNumber);
            this.Controls.Add(this.BtnImageGrab);
            this.Controls.Add(this.BtnSearchDirection);
            this.Controls.Add(this.BtnTitlePolarity);
            this.Controls.Add(this.BtnTitleFilterHalfSizePixels);
            this.Controls.Add(this.BtnTitleContrastThreshold);
            this.Controls.Add(this.BtnTitleIgnoreNumber);
            this.Controls.Add(this.BtnTitleNumber);
            this.Controls.Add(this.BtnTitleFindLineSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CFormSetupVisionSettingSubFindLine";
            this.Text = "CFormSetupVisionSettingSubFindLine";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupVisionSettingSubFindLine_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupVisionSettingSubFindLine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayRunImage)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button BtnTitleFindLineSetting;
		private System.Windows.Forms.Button BtnNumber;
		private System.Windows.Forms.Button BtnTitleNumber;
		private System.Windows.Forms.Button BtnTitleIgnoreNumber;
		private System.Windows.Forms.Button BtnIgnoreNumber;
		private System.Windows.Forms.Button BtnTitleContrastThreshold;
		private System.Windows.Forms.Button BtnContrastThreshold;
		private System.Windows.Forms.Button BtnTitleFilterHalfSizePixels;
		private System.Windows.Forms.Button BtnFilterHalfSizePixels;
		private System.Windows.Forms.Button BtnTitlePolarity;
		private System.Windows.Forms.Button BtnPolarity;
		private System.Windows.Forms.Button BtnSearchDirection;
		private Cognex.VisionPro.Display.CogDisplay cogDisplayRunImage;
		private System.Windows.Forms.Button BtnRun;
		private System.Windows.Forms.Button BtnImageGrab;
	}
}