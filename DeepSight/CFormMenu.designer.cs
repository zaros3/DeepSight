namespace DeepSight
{
    partial class CFormMenu
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
            this.BtnMain = new System.Windows.Forms.Button();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.BtnSetup = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnLanguage = new System.Windows.Forms.Button();
            this.BtnIO = new System.Windows.Forms.Button();
            this.BtnReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnMain
            // 
            this.BtnMain.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnMain.Location = new System.Drawing.Point(12, 5);
            this.BtnMain.Name = "BtnMain";
            this.BtnMain.Size = new System.Drawing.Size(146, 65);
            this.BtnMain.TabIndex = 2;
            this.BtnMain.Text = "MAIN";
            this.BtnMain.Click += new System.EventHandler(this.BtnMain_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfig.Location = new System.Drawing.Point(468, 5);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(146, 65);
            this.BtnConfig.TabIndex = 2;
            this.BtnConfig.Text = "CONFIG";
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // BtnSetup
            // 
            this.BtnSetup.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSetup.Location = new System.Drawing.Point(316, 5);
            this.BtnSetup.Name = "BtnSetup";
            this.BtnSetup.Size = new System.Drawing.Size(146, 65);
            this.BtnSetup.TabIndex = 2;
            this.BtnSetup.Text = "SETUP";
            this.BtnSetup.Click += new System.EventHandler(this.BtnSetup_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.Location = new System.Drawing.Point(1758, 5);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(146, 65);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "EXIT";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BtnLanguage
            // 
            this.BtnLanguage.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLanguage.Location = new System.Drawing.Point(1606, 5);
            this.BtnLanguage.Name = "BtnLanguage";
            this.BtnLanguage.Size = new System.Drawing.Size(146, 65);
            this.BtnLanguage.TabIndex = 2;
            this.BtnLanguage.Text = "LANGUAGE";
            this.BtnLanguage.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // BtnIO
            // 
            this.BtnIO.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnIO.Location = new System.Drawing.Point(164, 5);
            this.BtnIO.Name = "BtnIO";
            this.BtnIO.Size = new System.Drawing.Size(146, 65);
            this.BtnIO.TabIndex = 2;
            this.BtnIO.Text = "IO";
            this.BtnIO.Click += new System.EventHandler(this.BtnIO_Click);
            // 
            // BtnReport
            // 
            this.BtnReport.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnReport.Location = new System.Drawing.Point(620, 5);
            this.BtnReport.Name = "BtnReport";
            this.BtnReport.Size = new System.Drawing.Size(146, 65);
            this.BtnReport.TabIndex = 2;
            this.BtnReport.Text = "REPORT";
            this.BtnReport.Click += new System.EventHandler(this.BtnReport_Click);
            // 
            // CFormMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1916, 75);
            this.ControlBox = false;
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnSetup);
            this.Controls.Add(this.BtnLanguage);
            this.Controls.Add(this.BtnReport);
            this.Controls.Add(this.BtnIO);
            this.Controls.Add(this.BtnConfig);
            this.Controls.Add(this.BtnMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFormMenu";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormMenu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormMenu_FormClosed);
            this.Load += new System.EventHandler(this.CFormMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button BtnMain;
		private System.Windows.Forms.Button BtnConfig;
		private System.Windows.Forms.Button BtnSetup;
		private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button BtnLanguage;
		private System.Windows.Forms.Button BtnIO;
		private System.Windows.Forms.Button BtnReport;

    }
}