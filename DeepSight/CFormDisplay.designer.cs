namespace DeepSight
{
    partial class CFormDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFormDisplay));
            this.cogDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.BtnTitle = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnGuage = new System.Windows.Forms.Button();
            this.BtnHistogram = new System.Windows.Forms.Button();
            this.BtnExpansion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // cogDisplay
            // 
            this.cogDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay.DoubleTapZoomCycleLength = 2;
            this.cogDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay.Location = new System.Drawing.Point(2, 49);
            this.cogDisplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cogDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay.MouseWheelSensitivity = 1D;
            this.cogDisplay.Name = "cogDisplay";
            this.cogDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay.OcxState")));
            this.cogDisplay.Size = new System.Drawing.Size(551, 376);
            this.cogDisplay.TabIndex = 0;
            this.cogDisplay.TabStop = false;
            this.cogDisplay.DraggingStopped += new System.EventHandler(this.cogDisplay_DraggingStopped);
            // 
            // BtnTitle
            // 
            this.BtnTitle.BackColor = System.Drawing.Color.DimGray;
            this.BtnTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.BtnTitle.ForeColor = System.Drawing.Color.White;
            this.BtnTitle.Location = new System.Drawing.Point(1, 1);
            this.BtnTitle.Name = "BtnTitle";
            this.BtnTitle.Size = new System.Drawing.Size(320, 46);
            this.BtnTitle.TabIndex = 2;
            this.BtnTitle.TabStop = false;
            this.BtnTitle.Text = "???";
            this.BtnTitle.UseVisualStyleBackColor = false;
            this.BtnTitle.Click += new System.EventHandler(this.BtnTitle_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnGuage
            // 
            this.BtnGuage.BackColor = System.Drawing.Color.DimGray;
            this.BtnGuage.Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold);
            this.BtnGuage.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnGuage.Location = new System.Drawing.Point(398, 1);
            this.BtnGuage.Name = "BtnGuage";
            this.BtnGuage.Size = new System.Drawing.Size(69, 46);
            this.BtnGuage.TabIndex = 3;
            this.BtnGuage.TabStop = false;
            this.BtnGuage.Text = "GAUGE";
            this.BtnGuage.UseVisualStyleBackColor = false;
            this.BtnGuage.Click += new System.EventHandler(this.BtnGuage_Click);
            // 
            // BtnHistogram
            // 
            this.BtnHistogram.BackColor = System.Drawing.Color.DimGray;
            this.BtnHistogram.Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold);
            this.BtnHistogram.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnHistogram.Location = new System.Drawing.Point(466, 1);
            this.BtnHistogram.Name = "BtnHistogram";
            this.BtnHistogram.Size = new System.Drawing.Size(87, 46);
            this.BtnHistogram.TabIndex = 3;
            this.BtnHistogram.TabStop = false;
            this.BtnHistogram.Text = "HISTOGRAM";
            this.BtnHistogram.UseVisualStyleBackColor = false;
            this.BtnHistogram.Click += new System.EventHandler(this.BtnHistogram_Click);
            // 
            // BtnExpansion
            // 
            this.BtnExpansion.BackColor = System.Drawing.Color.DimGray;
            this.BtnExpansion.Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold);
            this.BtnExpansion.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnExpansion.Location = new System.Drawing.Point(321, 1);
            this.BtnExpansion.Name = "BtnExpansion";
            this.BtnExpansion.Size = new System.Drawing.Size(78, 46);
            this.BtnExpansion.TabIndex = 4;
            this.BtnExpansion.TabStop = false;
            this.BtnExpansion.Text = "EXPAND\r\n/UNDO";
            this.BtnExpansion.UseVisualStyleBackColor = false;
            this.BtnExpansion.Click += new System.EventHandler(this.BtnExpansion_Click);
            // 
            // CFormDisplay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(557, 428);
            this.ControlBox = false;
            this.Controls.Add(this.BtnExpansion);
            this.Controls.Add(this.BtnHistogram);
            this.Controls.Add(this.BtnGuage);
            this.Controls.Add(this.BtnTitle);
            this.Controls.Add(this.cogDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CFormDisplay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vision Live Display";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.CogRecordDisplay cogDisplay;
        private System.Windows.Forms.Button BtnTitle;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button BtnGuage;
        private System.Windows.Forms.Button BtnHistogram;
        private System.Windows.Forms.Button BtnExpansion;
    }
}