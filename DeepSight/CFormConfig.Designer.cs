namespace DeepSight
{
    partial class CFormConfig
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
            this.panelForm = new System.Windows.Forms.Panel();
            this.panelFormView = new System.Windows.Forms.Panel();
            this.panelFormMenu = new System.Windows.Forms.Panel();
            this.BtnBase = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelForm.SuspendLayout();
            this.panelFormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelForm
            // 
            this.panelForm.Controls.Add(this.panelFormView);
            this.panelForm.Controls.Add(this.panelFormMenu);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 0);
            this.panelForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(1916, 922);
            this.panelForm.TabIndex = 0;
            // 
            // panelFormView
            // 
            this.panelFormView.BackColor = System.Drawing.Color.White;
            this.panelFormView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormView.Location = new System.Drawing.Point(0, 59);
            this.panelFormView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelFormView.Name = "panelFormView";
            this.panelFormView.Size = new System.Drawing.Size(1916, 863);
            this.panelFormView.TabIndex = 4;
            // 
            // panelFormMenu
            // 
            this.panelFormMenu.BackColor = System.Drawing.Color.White;
            this.panelFormMenu.Controls.Add(this.BtnBase);
            this.panelFormMenu.Location = new System.Drawing.Point(0, 4);
            this.panelFormMenu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelFormMenu.Name = "panelFormMenu";
            this.panelFormMenu.Size = new System.Drawing.Size(1916, 58);
            this.panelFormMenu.TabIndex = 7;
            // 
            // BtnBase
            // 
            this.BtnBase.Enabled = false;
            this.BtnBase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBase.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnBase.Location = new System.Drawing.Point(0, 0);
            this.BtnBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnBase.Name = "BtnBase";
            this.BtnBase.Size = new System.Drawing.Size(159, 58);
            this.BtnBase.TabIndex = 4;
            this.BtnBase.Text = "BASE";
            this.BtnBase.UseVisualStyleBackColor = true;
            this.BtnBase.Visible = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // CFormConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1916, 922);
            this.Controls.Add(this.panelForm);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFormConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormConfig";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormConfig_FormClosed);
            this.Load += new System.EventHandler(this.CFormConfig_Load);
            this.panelForm.ResumeLayout(false);
            this.panelFormMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Panel panelFormView;
        private System.Windows.Forms.Panel panelFormMenu;
        private System.Windows.Forms.Button BtnBase;
        private System.Windows.Forms.Timer timer;
    }
}