namespace DeepSight
{
    partial class CFormDisplayChangeSize
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.BtnDisplayLayout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnDisplayLayout
            // 
            this.BtnDisplayLayout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnDisplayLayout.Enabled = false;
            this.BtnDisplayLayout.Location = new System.Drawing.Point(0, 0);
            this.BtnDisplayLayout.Name = "BtnDisplayLayout";
            this.BtnDisplayLayout.Size = new System.Drawing.Size(1627, 843);
            this.BtnDisplayLayout.TabIndex = 54;
            this.BtnDisplayLayout.UseVisualStyleBackColor = true;
            this.BtnDisplayLayout.Visible = false;
            // 
            // CFormDisplayChangeSize
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1628, 844);
            this.ControlBox = false;
            this.Controls.Add(this.BtnDisplayLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CFormDisplayChangeSize";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vision Live Display";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button BtnDisplayLayout;
    }
}