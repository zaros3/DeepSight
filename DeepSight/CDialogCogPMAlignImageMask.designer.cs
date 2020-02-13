namespace DeepSight
{
    partial class CDialogCogPMAlignImageMask
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
			this.cogImageMaskEdit = new Cognex.VisionPro.CogImageMaskEditV2();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnSave = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cogImageMaskEdit
			// 
			this.cogImageMaskEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cogImageMaskEdit.Location = new System.Drawing.Point(11, 12);
			this.cogImageMaskEdit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.cogImageMaskEdit.Name = "cogImageMaskEdit";
			this.cogImageMaskEdit.Size = new System.Drawing.Size(878, 636);
			this.cogImageMaskEdit.TabIndex = 3;
			// 
			// BtnCancel
			// 
			this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnCancel.Location = new System.Drawing.Point(770, 654);
			this.BtnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(119, 60);
			this.BtnCancel.TabIndex = 7;
			this.BtnCancel.Text = "CANCEL";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// BtnSave
			// 
			this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnSave.Location = new System.Drawing.Point(646, 654);
			this.BtnSave.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(119, 60);
			this.BtnSave.TabIndex = 8;
			this.BtnSave.Text = "SAVE";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// CDialogCogPMAlignImageMask
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(902, 726);
			this.ControlBox = false;
			this.Controls.Add(this.BtnCancel);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.cogImageMaskEdit);
			this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.Name = "CDialogCogPMAlignImageMask";
			this.Text = "CDialogPMAlignImageMask";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogCogPMAlignImageMask_FormClosed);
			this.Load += new System.EventHandler(this.CDialogCogPMAlignImageMask_Load);
			this.ResumeLayout(false);

        }

        #endregion

		private Cognex.VisionPro.CogImageMaskEditV2 cogImageMaskEdit;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnSave;
    }
}