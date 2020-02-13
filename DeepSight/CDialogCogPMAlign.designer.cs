namespace DeepSight
{
    partial class CDialogCogPMAlign
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
			this.cogPMAlignEdit = new Cognex.VisionPro.PMAlign.CogPMAlignEditV2();
			this.BtnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.cogPMAlignEdit)).BeginInit();
			this.SuspendLayout();
			// 
			// cogPMAlignEdit
			// 
			this.cogPMAlignEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cogPMAlignEdit.Location = new System.Drawing.Point(11, 12);
			this.cogPMAlignEdit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.cogPMAlignEdit.MinimumSize = new System.Drawing.Size(447, 0);
			this.cogPMAlignEdit.Name = "cogPMAlignEdit";
			this.cogPMAlignEdit.Size = new System.Drawing.Size(879, 550);
			this.cogPMAlignEdit.SuspendElectricRuns = false;
			this.cogPMAlignEdit.TabIndex = 0;
			// 
			// BtnClose
			// 
			this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnClose.Location = new System.Drawing.Point(772, 569);
			this.BtnClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.Size = new System.Drawing.Size(119, 60);
			this.BtnClose.TabIndex = 5;
			this.BtnClose.Text = "CLOSE";
			this.BtnClose.UseVisualStyleBackColor = true;
			this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
			// 
			// CDialogCogPMAlign
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(902, 641);
			this.ControlBox = false;
			this.Controls.Add(this.BtnClose);
			this.Controls.Add(this.cogPMAlignEdit);
			this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.Name = "CDialogCogPMAlign";
			this.Text = "CDialogCogPMAlign";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCogPMAlign_FormClosed);
			this.Load += new System.EventHandler(this.CDialogCogPMAlign_Load);
			((System.ComponentModel.ISupportInitialize)(this.cogPMAlignEdit)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private Cognex.VisionPro.PMAlign.CogPMAlignEditV2 cogPMAlignEdit;
		private System.Windows.Forms.Button BtnClose;
    }
}