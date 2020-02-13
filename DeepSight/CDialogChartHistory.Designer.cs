namespace DeepSight
{
    partial class CDialogChartHistory {
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
            this.BtnCancel = new System.Windows.Forms.Button();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.BtnInspectionPosition = new System.Windows.Forms.Button();
            this.BtnViewImageCrop = new System.Windows.Forms.Button();
            this.BtnPositionCrop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.Location = new System.Drawing.Point(665, 704);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(430, 70);
            this.BtnCancel.TabIndex = 10;
            this.BtnCancel.Text = "CLOSE";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
            this.winChartViewer1.Location = new System.Drawing.Point(13, 13);
            this.winChartViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(1082, 697);
            this.winChartViewer1.TabIndex = 11;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged);
            this.winChartViewer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseDown);
            this.winChartViewer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseMove);
            this.winChartViewer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseUp);
            // 
            // BtnInspectionPosition
            // 
            this.BtnInspectionPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInspectionPosition.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnInspectionPosition.Location = new System.Drawing.Point(230, 704);
            this.BtnInspectionPosition.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInspectionPosition.Name = "BtnInspectionPosition";
            this.BtnInspectionPosition.Size = new System.Drawing.Size(213, 70);
            this.BtnInspectionPosition.TabIndex = 46;
            this.BtnInspectionPosition.UseVisualStyleBackColor = true;
            this.BtnInspectionPosition.Click += new System.EventHandler(this.BtnInspectionPosition_Click);
            // 
            // BtnViewImageCrop
            // 
            this.BtnViewImageCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnViewImageCrop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnViewImageCrop.Location = new System.Drawing.Point(13, 704);
            this.BtnViewImageCrop.Margin = new System.Windows.Forms.Padding(2);
            this.BtnViewImageCrop.Name = "BtnViewImageCrop";
            this.BtnViewImageCrop.Size = new System.Drawing.Size(213, 70);
            this.BtnViewImageCrop.TabIndex = 47;
            this.BtnViewImageCrop.Text = "VIEW CROP";
            this.BtnViewImageCrop.UseVisualStyleBackColor = true;
            this.BtnViewImageCrop.Click += new System.EventHandler(this.BtnViewImageCrop_Click);
            // 
            // BtnPositionCrop
            // 
            this.BtnPositionCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPositionCrop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPositionCrop.Location = new System.Drawing.Point(447, 704);
            this.BtnPositionCrop.Margin = new System.Windows.Forms.Padding(2);
            this.BtnPositionCrop.Name = "BtnPositionCrop";
            this.BtnPositionCrop.Size = new System.Drawing.Size(213, 70);
            this.BtnPositionCrop.TabIndex = 46;
            this.BtnPositionCrop.UseVisualStyleBackColor = true;
            this.BtnPositionCrop.Click += new System.EventHandler(this.BtnPositionCrop_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CDialogChartHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1107, 782);
            this.ControlBox = false;
            this.Controls.Add(this.BtnPositionCrop);
            this.Controls.Add(this.BtnInspectionPosition);
            this.Controls.Add(this.BtnViewImageCrop);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.winChartViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDialogChartHistory";
            this.ShowIcon = false;
            this.Text = "CDialogChartHistory";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnCancel;
        private ChartDirector.WinChartViewer winChartViewer1;
        private System.Windows.Forms.Button BtnInspectionPosition;
        private System.Windows.Forms.Button BtnViewImageCrop;
        private System.Windows.Forms.Button BtnPositionCrop;
        private System.Windows.Forms.Timer timer1;
    }
}