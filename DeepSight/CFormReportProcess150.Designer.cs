namespace DeepSight
{
	partial class CFormReportProcess150 {
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.BtnResultTypeNg = new System.Windows.Forms.Button();
            this.BtnSaveToCsv = new System.Windows.Forms.Button();
            this.BtnSelectAsc = new System.Windows.Forms.Button();
            this.BtnSelectDesc = new System.Windows.Forms.Button();
            this.BtnResultTypeOk = new System.Windows.Forms.Button();
            this.BtnResultTypeAll = new System.Windows.Forms.Button();
            this.GridViewAlignList = new System.Windows.Forms.DataGridView();
            this.panelDisplayPMS = new System.Windows.Forms.Panel();
            this.panelDisplayVidi6 = new System.Windows.Forms.Panel();
            this.panelDisplayVidi5 = new System.Windows.Forms.Panel();
            this.panelDisplayVidi4 = new System.Windows.Forms.Panel();
            this.panelDisplayVidi3 = new System.Windows.Forms.Panel();
            this.panelDisplayVidi2 = new System.Windows.Forms.Panel();
            this.panelDisplayVidi1 = new System.Windows.Forms.Panel();
            this.panelDisplayMeasure1 = new System.Windows.Forms.Panel();
            this.panelDisplayMeasure2 = new System.Windows.Forms.Panel();
            this.panelDisplayMeasure3 = new System.Windows.Forms.Panel();
            this.panelDisplayMeasure5 = new System.Windows.Forms.Panel();
            this.panelDisplayMeasure4 = new System.Windows.Forms.Panel();
            this.panelDisplayMeasure6 = new System.Windows.Forms.Panel();
            this.BtnChart = new System.Windows.Forms.Button();
            this.BtnSelectPosition = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAlignList)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // DateTimeFrom
            // 
            this.DateTimeFrom.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeFrom.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeFrom.Location = new System.Drawing.Point(11, 11);
            this.DateTimeFrom.Margin = new System.Windows.Forms.Padding(2);
            this.DateTimeFrom.Name = "DateTimeFrom";
            this.DateTimeFrom.Size = new System.Drawing.Size(300, 46);
            this.DateTimeFrom.TabIndex = 14;
            this.DateTimeFrom.ValueChanged += new System.EventHandler(this.DateTimeFrom_ValueChanged);
            // 
            // DateTimeTo
            // 
            this.DateTimeTo.CalendarFont = new System.Drawing.Font("굴림", 9F);
            this.DateTimeTo.Font = new System.Drawing.Font("굴림", 25F);
            this.DateTimeTo.Location = new System.Drawing.Point(315, 11);
            this.DateTimeTo.Margin = new System.Windows.Forms.Padding(2);
            this.DateTimeTo.Name = "DateTimeTo";
            this.DateTimeTo.Size = new System.Drawing.Size(300, 46);
            this.DateTimeTo.TabIndex = 15;
            this.DateTimeTo.ValueChanged += new System.EventHandler(this.DateTimeTo_ValueChanged);
            // 
            // BtnResultTypeNg
            // 
            this.BtnResultTypeNg.BackColor = System.Drawing.Color.Transparent;
            this.BtnResultTypeNg.Location = new System.Drawing.Point(619, 11);
            this.BtnResultTypeNg.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResultTypeNg.Name = "BtnResultTypeNg";
            this.BtnResultTypeNg.Size = new System.Drawing.Size(156, 46);
            this.BtnResultTypeNg.TabIndex = 16;
            this.BtnResultTypeNg.Text = "NG";
            this.BtnResultTypeNg.UseVisualStyleBackColor = false;
            this.BtnResultTypeNg.Click += new System.EventHandler(this.BtnResultTypeNg_Click);
            // 
            // BtnSaveToCsv
            // 
            this.BtnSaveToCsv.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveToCsv.Location = new System.Drawing.Point(1595, 11);
            this.BtnSaveToCsv.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSaveToCsv.Name = "BtnSaveToCsv";
            this.BtnSaveToCsv.Size = new System.Drawing.Size(156, 46);
            this.BtnSaveToCsv.TabIndex = 16;
            this.BtnSaveToCsv.Text = "SAVE TO CSV";
            this.BtnSaveToCsv.UseVisualStyleBackColor = false;
            this.BtnSaveToCsv.Click += new System.EventHandler(this.BtnSaveToCsv_Click);
            // 
            // BtnSelectAsc
            // 
            this.BtnSelectAsc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectAsc.Location = new System.Drawing.Point(1275, 11);
            this.BtnSelectAsc.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectAsc.Name = "BtnSelectAsc";
            this.BtnSelectAsc.Size = new System.Drawing.Size(156, 46);
            this.BtnSelectAsc.TabIndex = 16;
            this.BtnSelectAsc.Text = "SELECT (ASC)";
            this.BtnSelectAsc.UseVisualStyleBackColor = false;
            this.BtnSelectAsc.Click += new System.EventHandler(this.BtnSelectAsc_Click);
            // 
            // BtnSelectDesc
            // 
            this.BtnSelectDesc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectDesc.Location = new System.Drawing.Point(1435, 11);
            this.BtnSelectDesc.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectDesc.Name = "BtnSelectDesc";
            this.BtnSelectDesc.Size = new System.Drawing.Size(156, 46);
            this.BtnSelectDesc.TabIndex = 16;
            this.BtnSelectDesc.Text = "SELECT (DESC)";
            this.BtnSelectDesc.UseVisualStyleBackColor = false;
            this.BtnSelectDesc.Click += new System.EventHandler(this.BtnSelectDesc_Click);
            // 
            // BtnResultTypeOk
            // 
            this.BtnResultTypeOk.BackColor = System.Drawing.Color.Transparent;
            this.BtnResultTypeOk.Location = new System.Drawing.Point(779, 11);
            this.BtnResultTypeOk.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResultTypeOk.Name = "BtnResultTypeOk";
            this.BtnResultTypeOk.Size = new System.Drawing.Size(156, 46);
            this.BtnResultTypeOk.TabIndex = 16;
            this.BtnResultTypeOk.Text = "OK";
            this.BtnResultTypeOk.UseVisualStyleBackColor = false;
            this.BtnResultTypeOk.Click += new System.EventHandler(this.BtnResultTypeOk_Click);
            // 
            // BtnResultTypeAll
            // 
            this.BtnResultTypeAll.BackColor = System.Drawing.Color.DimGray;
            this.BtnResultTypeAll.Location = new System.Drawing.Point(939, 11);
            this.BtnResultTypeAll.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResultTypeAll.Name = "BtnResultTypeAll";
            this.BtnResultTypeAll.Size = new System.Drawing.Size(156, 46);
            this.BtnResultTypeAll.TabIndex = 16;
            this.BtnResultTypeAll.Text = "ALL";
            this.BtnResultTypeAll.UseVisualStyleBackColor = false;
            this.BtnResultTypeAll.Click += new System.EventHandler(this.BtnResultTypeAll_Click);
            // 
            // GridViewAlignList
            // 
            this.GridViewAlignList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewAlignList.Location = new System.Drawing.Point(12, 61);
            this.GridViewAlignList.Margin = new System.Windows.Forms.Padding(2);
            this.GridViewAlignList.Name = "GridViewAlignList";
            this.GridViewAlignList.RowTemplate.Height = 23;
            this.GridViewAlignList.Size = new System.Drawing.Size(1898, 317);
            this.GridViewAlignList.TabIndex = 17;
            this.GridViewAlignList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAlignList_CellClick);
            this.GridViewAlignList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAlignList_CellEnter);
            this.GridViewAlignList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewAlignList_CellValueNeeded);
            this.GridViewAlignList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridViewAlignList_KeyDown);
            this.GridViewAlignList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GridViewAlignList_KeyUp);
            // 
            // panelDisplayPMS
            // 
            this.panelDisplayPMS.Location = new System.Drawing.Point(12, 382);
            this.panelDisplayPMS.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayPMS.Name = "panelDisplayPMS";
            this.panelDisplayPMS.Size = new System.Drawing.Size(1898, 239);
            this.panelDisplayPMS.TabIndex = 46;
            // 
            // panelDisplayVidi6
            // 
            this.panelDisplayVidi6.Location = new System.Drawing.Point(484, 779);
            this.panelDisplayVidi6.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi6.Name = "panelDisplayVidi6";
            this.panelDisplayVidi6.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayVidi6.TabIndex = 47;
            // 
            // panelDisplayVidi5
            // 
            this.panelDisplayVidi5.Location = new System.Drawing.Point(484, 702);
            this.panelDisplayVidi5.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi5.Name = "panelDisplayVidi5";
            this.panelDisplayVidi5.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayVidi5.TabIndex = 48;
            // 
            // panelDisplayVidi4
            // 
            this.panelDisplayVidi4.Location = new System.Drawing.Point(484, 625);
            this.panelDisplayVidi4.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi4.Name = "panelDisplayVidi4";
            this.panelDisplayVidi4.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayVidi4.TabIndex = 49;
            // 
            // panelDisplayVidi3
            // 
            this.panelDisplayVidi3.Location = new System.Drawing.Point(11, 779);
            this.panelDisplayVidi3.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi3.Name = "panelDisplayVidi3";
            this.panelDisplayVidi3.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayVidi3.TabIndex = 50;
            // 
            // panelDisplayVidi2
            // 
            this.panelDisplayVidi2.Location = new System.Drawing.Point(11, 702);
            this.panelDisplayVidi2.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi2.Name = "panelDisplayVidi2";
            this.panelDisplayVidi2.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayVidi2.TabIndex = 51;
            // 
            // panelDisplayVidi1
            // 
            this.panelDisplayVidi1.Location = new System.Drawing.Point(11, 625);
            this.panelDisplayVidi1.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi1.Name = "panelDisplayVidi1";
            this.panelDisplayVidi1.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayVidi1.TabIndex = 52;
            // 
            // panelDisplayMeasure1
            // 
            this.panelDisplayMeasure1.Location = new System.Drawing.Point(969, 625);
            this.panelDisplayMeasure1.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayMeasure1.Name = "panelDisplayMeasure1";
            this.panelDisplayMeasure1.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayMeasure1.TabIndex = 52;
            // 
            // panelDisplayMeasure2
            // 
            this.panelDisplayMeasure2.Location = new System.Drawing.Point(969, 702);
            this.panelDisplayMeasure2.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayMeasure2.Name = "panelDisplayMeasure2";
            this.panelDisplayMeasure2.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayMeasure2.TabIndex = 51;
            // 
            // panelDisplayMeasure3
            // 
            this.panelDisplayMeasure3.Location = new System.Drawing.Point(969, 779);
            this.panelDisplayMeasure3.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayMeasure3.Name = "panelDisplayMeasure3";
            this.panelDisplayMeasure3.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayMeasure3.TabIndex = 50;
            // 
            // panelDisplayMeasure5
            // 
            this.panelDisplayMeasure5.Location = new System.Drawing.Point(1442, 702);
            this.panelDisplayMeasure5.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayMeasure5.Name = "panelDisplayMeasure5";
            this.panelDisplayMeasure5.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayMeasure5.TabIndex = 49;
            // 
            // panelDisplayMeasure4
            // 
            this.panelDisplayMeasure4.Location = new System.Drawing.Point(1442, 625);
            this.panelDisplayMeasure4.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayMeasure4.Name = "panelDisplayMeasure4";
            this.panelDisplayMeasure4.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayMeasure4.TabIndex = 48;
            // 
            // panelDisplayMeasure6
            // 
            this.panelDisplayMeasure6.Location = new System.Drawing.Point(1442, 779);
            this.panelDisplayMeasure6.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayMeasure6.Name = "panelDisplayMeasure6";
            this.panelDisplayMeasure6.Size = new System.Drawing.Size(469, 73);
            this.panelDisplayMeasure6.TabIndex = 47;
            // 
            // BtnChart
            // 
            this.BtnChart.BackColor = System.Drawing.Color.Transparent;
            this.BtnChart.Location = new System.Drawing.Point(1755, 11);
            this.BtnChart.Margin = new System.Windows.Forms.Padding(2);
            this.BtnChart.Name = "BtnChart";
            this.BtnChart.Size = new System.Drawing.Size(156, 46);
            this.BtnChart.TabIndex = 16;
            this.BtnChart.Text = "CHART";
            this.BtnChart.UseVisualStyleBackColor = false;
            this.BtnChart.Click += new System.EventHandler(this.BtnChart_Click);
            // 
            // BtnSelectPosition
            // 
            this.BtnSelectPosition.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectPosition.Location = new System.Drawing.Point(1115, 11);
            this.BtnSelectPosition.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectPosition.Name = "BtnSelectPosition";
            this.BtnSelectPosition.Size = new System.Drawing.Size(156, 46);
            this.BtnSelectPosition.TabIndex = 16;
            this.BtnSelectPosition.Text = "POSITION";
            this.BtnSelectPosition.UseVisualStyleBackColor = false;
            this.BtnSelectPosition.Click += new System.EventHandler(this.BtnSelectPosition_Click);
            // 
            // CFormReportProcess150
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1916, 863);
            this.Controls.Add(this.panelDisplayMeasure6);
            this.Controls.Add(this.panelDisplayVidi6);
            this.Controls.Add(this.panelDisplayMeasure4);
            this.Controls.Add(this.panelDisplayVidi5);
            this.Controls.Add(this.panelDisplayMeasure5);
            this.Controls.Add(this.panelDisplayVidi4);
            this.Controls.Add(this.panelDisplayMeasure3);
            this.Controls.Add(this.panelDisplayVidi3);
            this.Controls.Add(this.panelDisplayMeasure2);
            this.Controls.Add(this.panelDisplayVidi2);
            this.Controls.Add(this.panelDisplayMeasure1);
            this.Controls.Add(this.panelDisplayVidi1);
            this.Controls.Add(this.panelDisplayPMS);
            this.Controls.Add(this.GridViewAlignList);
            this.Controls.Add(this.BtnResultTypeAll);
            this.Controls.Add(this.BtnSelectPosition);
            this.Controls.Add(this.BtnResultTypeOk);
            this.Controls.Add(this.BtnResultTypeNg);
            this.Controls.Add(this.BtnSelectDesc);
            this.Controls.Add(this.BtnChart);
            this.Controls.Add(this.BtnSelectAsc);
            this.Controls.Add(this.BtnSaveToCsv);
            this.Controls.Add(this.DateTimeTo);
            this.Controls.Add(this.DateTimeFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFormReportProcess150";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormReportAlign";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormSetupVisionOptical_FormClosed);
            this.Load += new System.EventHandler(this.CFormSetupVisionOptical_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAlignList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.DateTimePicker DateTimeFrom;
		private System.Windows.Forms.DateTimePicker DateTimeTo;
		private System.Windows.Forms.Button BtnResultTypeNg;
		private System.Windows.Forms.Button BtnSaveToCsv;
		private System.Windows.Forms.Button BtnSelectAsc;
		private System.Windows.Forms.Button BtnSelectDesc;
		private System.Windows.Forms.Button BtnResultTypeOk;
		private System.Windows.Forms.Button BtnResultTypeAll;
		private System.Windows.Forms.DataGridView GridViewAlignList;
        private System.Windows.Forms.Panel panelDisplayPMS;
        private System.Windows.Forms.Panel panelDisplayVidi6;
        private System.Windows.Forms.Panel panelDisplayVidi5;
        private System.Windows.Forms.Panel panelDisplayVidi4;
        private System.Windows.Forms.Panel panelDisplayVidi3;
        private System.Windows.Forms.Panel panelDisplayVidi2;
        private System.Windows.Forms.Panel panelDisplayVidi1;
        private System.Windows.Forms.Panel panelDisplayMeasure1;
        private System.Windows.Forms.Panel panelDisplayMeasure2;
        private System.Windows.Forms.Panel panelDisplayMeasure3;
        private System.Windows.Forms.Panel panelDisplayMeasure5;
        private System.Windows.Forms.Panel panelDisplayMeasure4;
        private System.Windows.Forms.Panel panelDisplayMeasure6;
        private System.Windows.Forms.Button BtnChart;
        private System.Windows.Forms.Button BtnSelectPosition;
    }
}