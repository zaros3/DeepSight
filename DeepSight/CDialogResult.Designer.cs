namespace DeepSight
{
    partial class CDialogResult {
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnResult = new System.Windows.Forms.Button();
            this.BtnInspectionPosition = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnResult
            // 
            this.BtnResult.Font = new System.Drawing.Font("맑은 고딕", 150F, System.Drawing.FontStyle.Bold);
            this.BtnResult.Location = new System.Drawing.Point(12, 12);
            this.BtnResult.Name = "BtnResult";
            this.BtnResult.Size = new System.Drawing.Size(513, 403);
            this.BtnResult.TabIndex = 0;
            this.BtnResult.Text = "OK";
            this.BtnResult.UseVisualStyleBackColor = true;
            // 
            // BtnInspectionPosition
            // 
            this.BtnInspectionPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInspectionPosition.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnInspectionPosition.Location = new System.Drawing.Point(11, 420);
            this.BtnInspectionPosition.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInspectionPosition.Name = "BtnInspectionPosition";
            this.BtnInspectionPosition.Size = new System.Drawing.Size(513, 70);
            this.BtnInspectionPosition.TabIndex = 47;
            this.BtnInspectionPosition.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.Location = new System.Drawing.Point(12, 495);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(513, 70);
            this.BtnCancel.TabIndex = 48;
            this.BtnCancel.Text = "CLOSE";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CDialogResult
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(537, 577);
            this.ControlBox = false;
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnInspectionPosition);
            this.Controls.Add(this.BtnResult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogResult";
            this.ShowIcon = false;
            this.Text = "RESULT";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BtnResult;
        private System.Windows.Forms.Button BtnInspectionPosition;
        private System.Windows.Forms.Button BtnCancel;
    }
}