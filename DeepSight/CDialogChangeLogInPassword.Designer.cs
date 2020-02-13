namespace DeepSight
{
    partial class CDialogChangeLogInPassword
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
            this.BtnTitlePassword = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.TextPassword = new System.Windows.Forms.TextBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnTitlePassword
            // 
            this.BtnTitlePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePassword.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitlePassword.Location = new System.Drawing.Point(12, 9);
            this.BtnTitlePassword.Name = "BtnTitlePassword";
            this.BtnTitlePassword.Size = new System.Drawing.Size(98, 28);
            this.BtnTitlePassword.TabIndex = 11;
            this.BtnTitlePassword.Text = "PASSWORD";
            this.BtnTitlePassword.UseVisualStyleBackColor = true;
            // 
            // BtnOK
            // 
            this.BtnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOK.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOK.Location = new System.Drawing.Point(12, 43);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(150, 69);
            this.BtnOK.TabIndex = 10;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // TextPassword
            // 
            this.TextPassword.Font = new System.Drawing.Font("굴림", 12.25F, System.Drawing.FontStyle.Bold);
            this.TextPassword.Location = new System.Drawing.Point(116, 11);
            this.TextPassword.Name = "TextPassword";
            this.TextPassword.Size = new System.Drawing.Size(202, 26);
            this.TextPassword.TabIndex = 9;
            // 
            // BtnCancel
            // 
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.Location = new System.Drawing.Point(168, 43);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(150, 69);
            this.BtnCancel.TabIndex = 10;
            this.BtnCancel.Text = "CANCEL";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CDialogChangeLogInPassword
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(326, 122);
            this.ControlBox = false;
            this.Controls.Add(this.BtnTitlePassword);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.TextPassword);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogChangeLogInPassword";
            this.ShowIcon = false;
            this.Text = "CHANGE PASSWORD";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnTitlePassword;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.TextBox TextPassword;
        private System.Windows.Forms.Button BtnCancel;
    }
}