namespace DeepSight
{
	partial class CFormSetupVisionSettingSubPMAlign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFormSetupVisionSettingSubPMAlign));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnTitlePatternSetting = new System.Windows.Forms.Button();
            this.BtnTrainImageGrab = new System.Windows.Forms.Button();
            this.cogDisplayTrainImage = new Cognex.VisionPro.Display.CogDisplay();
            this.BtnMaskImage = new System.Windows.Forms.Button();
            this.BtnPatternOriginCenter = new System.Windows.Forms.Button();
            this.BtnScore = new System.Windows.Forms.Button();
            this.BtnTitleScore = new System.Windows.Forms.Button();
            this.BtnTrain = new System.Windows.Forms.Button();
            this.cogDisplayRunImage = new Cognex.VisionPro.Display.CogDisplay();
            this.BtnRun = new System.Windows.Forms.Button();
            this.BtnTrainStatus = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayTrainImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayRunImage)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnTitlePatternSetting
            // 
            this.BtnTitlePatternSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePatternSetting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitlePatternSetting.Location = new System.Drawing.Point(1, 1);
            this.BtnTitlePatternSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitlePatternSetting.Name = "BtnTitlePatternSetting";
            this.BtnTitlePatternSetting.Size = new System.Drawing.Size(310, 46);
            this.BtnTitlePatternSetting.TabIndex = 2;
            this.BtnTitlePatternSetting.Text = "PATTERN SETTING";
            this.BtnTitlePatternSetting.UseVisualStyleBackColor = true;
            this.BtnTitlePatternSetting.Click += new System.EventHandler(this.BtnTitlePatternSetting_Click);
            // 
            // BtnTrainImageGrab
            // 
            this.BtnTrainImageGrab.BackColor = System.Drawing.Color.White;
            this.BtnTrainImageGrab.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTrainImageGrab.Location = new System.Drawing.Point(1, 253);
            this.BtnTrainImageGrab.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTrainImageGrab.Name = "BtnTrainImageGrab";
            this.BtnTrainImageGrab.Size = new System.Drawing.Size(310, 46);
            this.BtnTrainImageGrab.TabIndex = 31;
            this.BtnTrainImageGrab.Text = "TRAIN IMAGE GRAB";
            this.BtnTrainImageGrab.UseVisualStyleBackColor = true;
            this.BtnTrainImageGrab.Click += new System.EventHandler(this.BtnTrainImageGrab_Click);
            // 
            // cogDisplayTrainImage
            // 
            this.cogDisplayTrainImage.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplayTrainImage.ColorMapLowerRoiLimit = 0D;
            this.cogDisplayTrainImage.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplayTrainImage.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplayTrainImage.ColorMapUpperRoiLimit = 1D;
            this.cogDisplayTrainImage.DoubleTapZoomCycleLength = 2;
            this.cogDisplayTrainImage.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplayTrainImage.Location = new System.Drawing.Point(1, 53);
            this.cogDisplayTrainImage.Margin = new System.Windows.Forms.Padding(2);
            this.cogDisplayTrainImage.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplayTrainImage.MouseWheelSensitivity = 1D;
            this.cogDisplayTrainImage.Name = "cogDisplayTrainImage";
            this.cogDisplayTrainImage.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplayTrainImage.OcxState")));
            this.cogDisplayTrainImage.Size = new System.Drawing.Size(310, 196);
            this.cogDisplayTrainImage.TabIndex = 39;
            this.cogDisplayTrainImage.TabStop = false;
            // 
            // BtnMaskImage
            // 
            this.BtnMaskImage.BackColor = System.Drawing.Color.White;
            this.BtnMaskImage.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnMaskImage.Location = new System.Drawing.Point(1, 353);
            this.BtnMaskImage.Margin = new System.Windows.Forms.Padding(2);
            this.BtnMaskImage.Name = "BtnMaskImage";
            this.BtnMaskImage.Size = new System.Drawing.Size(310, 46);
            this.BtnMaskImage.TabIndex = 33;
            this.BtnMaskImage.Text = "MASK IMAGE";
            this.BtnMaskImage.UseVisualStyleBackColor = true;
            this.BtnMaskImage.Click += new System.EventHandler(this.BtnMaskImage_Click);
            // 
            // BtnPatternOriginCenter
            // 
            this.BtnPatternOriginCenter.BackColor = System.Drawing.Color.White;
            this.BtnPatternOriginCenter.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPatternOriginCenter.Location = new System.Drawing.Point(1, 303);
            this.BtnPatternOriginCenter.Margin = new System.Windows.Forms.Padding(2);
            this.BtnPatternOriginCenter.Name = "BtnPatternOriginCenter";
            this.BtnPatternOriginCenter.Size = new System.Drawing.Size(310, 46);
            this.BtnPatternOriginCenter.TabIndex = 35;
            this.BtnPatternOriginCenter.Text = "PATTERN ORIGIN CENTER";
            this.BtnPatternOriginCenter.UseVisualStyleBackColor = true;
            this.BtnPatternOriginCenter.Click += new System.EventHandler(this.BtnPatternOriginCenter_Click);
            // 
            // BtnScore
            // 
            this.BtnScore.BackColor = System.Drawing.Color.White;
            this.BtnScore.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnScore.Location = new System.Drawing.Point(158, 453);
            this.BtnScore.Margin = new System.Windows.Forms.Padding(2);
            this.BtnScore.Name = "BtnScore";
            this.BtnScore.Size = new System.Drawing.Size(153, 46);
            this.BtnScore.TabIndex = 36;
            this.BtnScore.Text = "0";
            this.BtnScore.UseVisualStyleBackColor = true;
            this.BtnScore.Click += new System.EventHandler(this.BtnScore_Click);
            // 
            // BtnTitleScore
            // 
            this.BtnTitleScore.BackColor = System.Drawing.Color.White;
            this.BtnTitleScore.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleScore.Location = new System.Drawing.Point(1, 453);
            this.BtnTitleScore.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleScore.Name = "BtnTitleScore";
            this.BtnTitleScore.Size = new System.Drawing.Size(153, 46);
            this.BtnTitleScore.TabIndex = 37;
            this.BtnTitleScore.Text = "SCORE";
            this.BtnTitleScore.UseVisualStyleBackColor = true;
            // 
            // BtnTrain
            // 
            this.BtnTrain.BackColor = System.Drawing.Color.White;
            this.BtnTrain.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTrain.Location = new System.Drawing.Point(1, 403);
            this.BtnTrain.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTrain.Name = "BtnTrain";
            this.BtnTrain.Size = new System.Drawing.Size(153, 46);
            this.BtnTrain.TabIndex = 38;
            this.BtnTrain.Text = "TRAIN";
            this.BtnTrain.UseVisualStyleBackColor = true;
            this.BtnTrain.Click += new System.EventHandler(this.BtnTrain_Click);
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
            this.cogDisplayRunImage.Location = new System.Drawing.Point(1, 551);
            this.cogDisplayRunImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cogDisplayRunImage.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplayRunImage.MouseWheelSensitivity = 1D;
            this.cogDisplayRunImage.Name = "cogDisplayRunImage";
            this.cogDisplayRunImage.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplayRunImage.OcxState")));
            this.cogDisplayRunImage.Size = new System.Drawing.Size(310, 239);
            this.cogDisplayRunImage.TabIndex = 40;
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
            this.BtnRun.TabIndex = 41;
            this.BtnRun.Text = "RUN";
            this.BtnRun.UseVisualStyleBackColor = true;
            this.BtnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // BtnTrainStatus
            // 
            this.BtnTrainStatus.BackColor = System.Drawing.Color.White;
            this.BtnTrainStatus.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTrainStatus.Location = new System.Drawing.Point(158, 403);
            this.BtnTrainStatus.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTrainStatus.Name = "BtnTrainStatus";
            this.BtnTrainStatus.Size = new System.Drawing.Size(153, 46);
            this.BtnTrainStatus.TabIndex = 34;
            this.BtnTrainStatus.Text = "OK";
            this.BtnTrainStatus.UseVisualStyleBackColor = true;
            // 
            // CFormSetupVisionSettingSubPMAlign
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(312, 840);
            this.Controls.Add(this.BtnRun);
            this.Controls.Add(this.cogDisplayRunImage);
            this.Controls.Add(this.BtnTrainImageGrab);
            this.Controls.Add(this.cogDisplayTrainImage);
            this.Controls.Add(this.BtnMaskImage);
            this.Controls.Add(this.BtnTrainStatus);
            this.Controls.Add(this.BtnPatternOriginCenter);
            this.Controls.Add(this.BtnScore);
            this.Controls.Add(this.BtnTitleScore);
            this.Controls.Add(this.BtnTrain);
            this.Controls.Add(this.BtnTitlePatternSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CFormSetupVisionSettingSubPMAlign";
            this.Text = "CFormSetupVisionSettingSubPMAlign";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupVisionSettingSubPMAlign_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupVisionSettingSubPMAlign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayTrainImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayRunImage)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button BtnTitlePatternSetting;
		private System.Windows.Forms.Button BtnTrainImageGrab;
		private Cognex.VisionPro.Display.CogDisplay cogDisplayTrainImage;
		private System.Windows.Forms.Button BtnMaskImage;
		private System.Windows.Forms.Button BtnPatternOriginCenter;
		private System.Windows.Forms.Button BtnScore;
		private System.Windows.Forms.Button BtnTitleScore;
		private System.Windows.Forms.Button BtnTrain;
		private Cognex.VisionPro.Display.CogDisplay cogDisplayRunImage;
		private System.Windows.Forms.Button BtnRun;
		private System.Windows.Forms.Button BtnTrainStatus;
	}
}