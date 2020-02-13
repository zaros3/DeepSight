namespace DeepSight
{
	partial class CDialogCogFindLine
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
			this.BtnClose = new System.Windows.Forms.Button();
			this.cogFindLineEditV21 = new Cognex.VisionPro.Caliper.CogFindLineEditV2();
			((System.ComponentModel.ISupportInitialize)(this.cogFindLineEditV21)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnClose
			// 
			this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnClose.Location = new System.Drawing.Point(844, 569);
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.Size = new System.Drawing.Size(130, 60);
			this.BtnClose.TabIndex = 4;
			this.BtnClose.Text = "CLOSE";
			this.BtnClose.UseVisualStyleBackColor = true;
			this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
			// 
			// cogFindLineEditV21
			// 
			this.cogFindLineEditV21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cogFindLineEditV21.Location = new System.Drawing.Point(12, 12);
			this.cogFindLineEditV21.MinimumSize = new System.Drawing.Size(489, 0);
			this.cogFindLineEditV21.Name = "cogFindLineEditV21";
			this.cogFindLineEditV21.Size = new System.Drawing.Size(962, 551);
			this.cogFindLineEditV21.SuspendElectricRuns = false;
			this.cogFindLineEditV21.TabIndex = 5;
			// 
			// CDialogCogFindLine
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(986, 641);
			this.ControlBox = false;
			this.Controls.Add(this.cogFindLineEditV21);
			this.Controls.Add(this.BtnClose);
			this.Name = "CDialogCogFindLine";
			this.Text = "CDialogCogFindLine";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCogFindLine_FormClosed);
			this.Load += new System.EventHandler(this.CDialogCogFindLine_Load);
			((System.ComponentModel.ISupportInitialize)(this.cogFindLineEditV21)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button BtnClose;
		private Cognex.VisionPro.Caliper.CogFindLineEditV2 cogFindLineEditV21;
    }
}