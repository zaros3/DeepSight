namespace DeepSight
{
    partial class CDialogLogin
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
            this.TextID = new System.Windows.Forms.TextBox();
            this.TextPassword = new System.Windows.Forms.TextBox();
            this.BtnTitleID = new System.Windows.Forms.Button();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.BtnLogout = new System.Windows.Forms.Button();
            this.BtnTitleLogin = new System.Windows.Forms.Button();
            this.BtnOperator = new System.Windows.Forms.Button();
            this.BtnEngineer = new System.Windows.Forms.Button();
            this.BtnMaster = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnTitlePassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextID
            // 
            this.TextID.Location = new System.Drawing.Point(94, 96);
            this.TextID.Name = "TextID";
            this.TextID.Size = new System.Drawing.Size(224, 21);
            this.TextID.TabIndex = 0;
            this.TextID.Visible = false;
            this.TextID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextID_KeyDown);
            // 
            // TextPassword
            // 
            this.TextPassword.Font = new System.Drawing.Font("굴림", 12.25F, System.Drawing.FontStyle.Bold);
            this.TextPassword.Location = new System.Drawing.Point(116, 189);
            this.TextPassword.Name = "TextPassword";
            this.TextPassword.Size = new System.Drawing.Size(202, 26);
            this.TextPassword.TabIndex = 0;
            this.TextPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextPassword_KeyDown);
            // 
            // BtnTitleID
            // 
            this.BtnTitleID.FlatAppearance.BorderSize = 0;
            this.BtnTitleID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleID.Location = new System.Drawing.Point(12, 96);
            this.BtnTitleID.Name = "BtnTitleID";
            this.BtnTitleID.Size = new System.Drawing.Size(75, 23);
            this.BtnTitleID.TabIndex = 7;
            this.BtnTitleID.TabStop = false;
            this.BtnTitleID.Text = "ID";
            this.BtnTitleID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTitleID.UseVisualStyleBackColor = true;
            this.BtnTitleID.Visible = false;
            // 
            // BtnLogin
            // 
            this.BtnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLogin.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLogin.Location = new System.Drawing.Point(12, 219);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(152, 83);
            this.BtnLogin.TabIndex = 8;
            this.BtnLogin.Text = "LOGIN";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // BtnLogout
            // 
            this.BtnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLogout.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.BtnLogout.Location = new System.Drawing.Point(170, 219);
            this.BtnLogout.Name = "BtnLogout";
            this.BtnLogout.Size = new System.Drawing.Size(148, 83);
            this.BtnLogout.TabIndex = 8;
            this.BtnLogout.Text = "CANCEL";
            this.BtnLogout.UseVisualStyleBackColor = true;
            this.BtnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // BtnTitleLogin
            // 
            this.BtnTitleLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleLogin.Font = new System.Drawing.Font("굴림", 50F);
            this.BtnTitleLogin.Location = new System.Drawing.Point(12, 12);
            this.BtnTitleLogin.Name = "BtnTitleLogin";
            this.BtnTitleLogin.Size = new System.Drawing.Size(306, 105);
            this.BtnTitleLogin.TabIndex = 8;
            this.BtnTitleLogin.Text = "LOGIN";
            this.BtnTitleLogin.UseVisualStyleBackColor = true;
            this.BtnTitleLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // BtnOperator
            // 
            this.BtnOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOperator.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOperator.Location = new System.Drawing.Point(12, 123);
            this.BtnOperator.Name = "BtnOperator";
            this.BtnOperator.Size = new System.Drawing.Size(98, 61);
            this.BtnOperator.TabIndex = 8;
            this.BtnOperator.Text = "OPERATOR";
            this.BtnOperator.UseVisualStyleBackColor = true;
            this.BtnOperator.Click += new System.EventHandler(this.BtnOperator_Click);
            // 
            // BtnEngineer
            // 
            this.BtnEngineer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEngineer.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnEngineer.Location = new System.Drawing.Point(116, 123);
            this.BtnEngineer.Name = "BtnEngineer";
            this.BtnEngineer.Size = new System.Drawing.Size(98, 61);
            this.BtnEngineer.TabIndex = 8;
            this.BtnEngineer.Text = "ENGINEER";
            this.BtnEngineer.UseVisualStyleBackColor = true;
            this.BtnEngineer.Click += new System.EventHandler(this.BtnEngineer_Click);
            // 
            // BtnMaster
            // 
            this.BtnMaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMaster.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnMaster.Location = new System.Drawing.Point(220, 123);
            this.BtnMaster.Name = "BtnMaster";
            this.BtnMaster.Size = new System.Drawing.Size(98, 61);
            this.BtnMaster.TabIndex = 8;
            this.BtnMaster.Text = "MASTER";
            this.BtnMaster.UseVisualStyleBackColor = true;
            this.BtnMaster.Click += new System.EventHandler(this.BtnMaster_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnTitlePassword
            // 
            this.BtnTitlePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitlePassword.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTitlePassword.Location = new System.Drawing.Point(12, 189);
            this.BtnTitlePassword.Name = "BtnTitlePassword";
            this.BtnTitlePassword.Size = new System.Drawing.Size(98, 26);
            this.BtnTitlePassword.TabIndex = 8;
            this.BtnTitlePassword.Text = "PASSWORD";
            this.BtnTitlePassword.UseVisualStyleBackColor = true;
            this.BtnTitlePassword.Click += new System.EventHandler(this.BtnTitlePassword_Click);
            // 
            // CDialogLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(330, 314);
            this.ControlBox = false;
            this.Controls.Add(this.BtnLogout);
            this.Controls.Add(this.BtnTitleLogin);
            this.Controls.Add(this.BtnMaster);
            this.Controls.Add(this.BtnEngineer);
            this.Controls.Add(this.BtnTitlePassword);
            this.Controls.Add(this.BtnOperator);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.BtnTitleID);
            this.Controls.Add(this.TextPassword);
            this.Controls.Add(this.TextID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogLogin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDialogLogin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogLogin_FormClosed);
            this.Load += new System.EventHandler(this.CDialogLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextID;
		private System.Windows.Forms.TextBox TextPassword;
        private System.Windows.Forms.Button BtnTitleID;
		private System.Windows.Forms.Button BtnLogin;
		private System.Windows.Forms.Button BtnLogout;
		private System.Windows.Forms.Button BtnTitleLogin;
        private System.Windows.Forms.Button BtnOperator;
        private System.Windows.Forms.Button BtnEngineer;
        private System.Windows.Forms.Button BtnMaster;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BtnTitlePassword;
    }
}