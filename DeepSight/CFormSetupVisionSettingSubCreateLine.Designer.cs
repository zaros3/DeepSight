namespace DeepSight
{
    partial class CFormSetupVisionSettingSubCreateLine
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
            if ( disposing && ( components != null ) )
            {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFormSetupVisionSettingSubCreateLine));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnTitleCreateLineSetting = new System.Windows.Forms.Button();
            this.cogDisplayRunImage = new Cognex.VisionPro.Display.CogDisplay();
            this.BtnImageGrab = new System.Windows.Forms.Button();
            this.BtnTitleLineDegree = new System.Windows.Forms.Button();
            this.BtnLineDegree = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayRunImage)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnTitleCreateLineSetting
            // 
            this.BtnTitleCreateLineSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCreateLineSetting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleCreateLineSetting.Location = new System.Drawing.Point(1, 1);
            this.BtnTitleCreateLineSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitleCreateLineSetting.Name = "BtnTitleCreateLineSetting";
            this.BtnTitleCreateLineSetting.Size = new System.Drawing.Size(310, 46);
            this.BtnTitleCreateLineSetting.TabIndex = 3;
            this.BtnTitleCreateLineSetting.Text = "CREATE LINE SETTING";
            this.BtnTitleCreateLineSetting.UseVisualStyleBackColor = true;
            this.BtnTitleCreateLineSetting.Click += new System.EventHandler(this.BtnTitleCreateLineSetting_Click);
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
            this.cogDisplayRunImage.Size = new System.Drawing.Size(310, 288);
            this.cogDisplayRunImage.TabIndex = 41;
            this.cogDisplayRunImage.TabStop = false;
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
            // BtnTitleLineDegree
            // 
            this.BtnTitleLineDegree.BackColor = System.Drawing.Color.White;
            this.BtnTitleLineDegree.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitleLineDegree.Location = new System.Drawing.Point(1, 103);
            this.BtnTitleLineDegree.Margin = new System.Windows.Forms.Padding(2);
            this.BtnTitleLineDegree.Name = "BtnTitleLineDegree";
            this.BtnTitleLineDegree.Size = new System.Drawing.Size(217, 46);
            this.BtnTitleLineDegree.TabIndex = 39;
            this.BtnTitleLineDegree.Text = "LINE DEGREE";
            this.BtnTitleLineDegree.UseVisualStyleBackColor = true;
            // 
            // BtnLineDegree
            // 
            this.BtnLineDegree.BackColor = System.Drawing.Color.White;
            this.BtnLineDegree.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLineDegree.Location = new System.Drawing.Point(222, 103);
            this.BtnLineDegree.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLineDegree.Name = "BtnLineDegree";
            this.BtnLineDegree.Size = new System.Drawing.Size(89, 46);
            this.BtnLineDegree.TabIndex = 38;
            this.BtnLineDegree.Text = "0";
            this.BtnLineDegree.UseVisualStyleBackColor = true;
            this.BtnLineDegree.Click += new System.EventHandler(this.BtnLineDegree_Click);
            // 
            // CFormSetupVisionSettingSubCreateLine
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(312, 840);
            this.Controls.Add(this.cogDisplayRunImage);
            this.Controls.Add(this.BtnLineDegree);
            this.Controls.Add(this.BtnImageGrab);
            this.Controls.Add(this.BtnTitleLineDegree);
            this.Controls.Add(this.BtnTitleCreateLineSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CFormSetupVisionSettingSubCreateLine";
            this.Text = "CFormSetupVisionSettingSubCreateLine";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupVisionSettingSubCreateLine_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupVisionSettingSubCreateLine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplayRunImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button BtnTitleCreateLineSetting;
        private Cognex.VisionPro.Display.CogDisplay cogDisplayRunImage;
        private System.Windows.Forms.Button BtnImageGrab;
        private System.Windows.Forms.Button BtnTitleLineDegree;
        private System.Windows.Forms.Button BtnLineDegree;
    }
}