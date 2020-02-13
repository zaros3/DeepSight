namespace DeepSight
{
	partial class CFormReportProcess60
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
            this.BtnResultTypeNg.Size = new System.Drawing.Size(196, 46);
            this.BtnResultTypeNg.TabIndex = 16;
            this.BtnResultTypeNg.Text = "NG";
            this.BtnResultTypeNg.UseVisualStyleBackColor = false;
            this.BtnResultTypeNg.Click += new System.EventHandler(this.BtnResultTypeNg_Click);
            // 
            // BtnSaveToCsv
            // 
            this.BtnSaveToCsv.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveToCsv.Location = new System.Drawing.Point(1709, 11);
            this.BtnSaveToCsv.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSaveToCsv.Name = "BtnSaveToCsv";
            this.BtnSaveToCsv.Size = new System.Drawing.Size(196, 46);
            this.BtnSaveToCsv.TabIndex = 16;
            this.BtnSaveToCsv.Text = "SAVE TO CSV";
            this.BtnSaveToCsv.UseVisualStyleBackColor = false;
            this.BtnSaveToCsv.Click += new System.EventHandler(this.BtnSaveToCsv_Click);
            // 
            // BtnSelectAsc
            // 
            this.BtnSelectAsc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectAsc.Location = new System.Drawing.Point(1220, 11);
            this.BtnSelectAsc.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectAsc.Name = "BtnSelectAsc";
            this.BtnSelectAsc.Size = new System.Drawing.Size(196, 46);
            this.BtnSelectAsc.TabIndex = 16;
            this.BtnSelectAsc.Text = "SELECT (ASC)";
            this.BtnSelectAsc.UseVisualStyleBackColor = false;
            this.BtnSelectAsc.Click += new System.EventHandler(this.BtnSelectAsc_Click);
            // 
            // BtnSelectDesc
            // 
            this.BtnSelectDesc.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectDesc.Location = new System.Drawing.Point(1420, 11);
            this.BtnSelectDesc.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectDesc.Name = "BtnSelectDesc";
            this.BtnSelectDesc.Size = new System.Drawing.Size(196, 46);
            this.BtnSelectDesc.TabIndex = 16;
            this.BtnSelectDesc.Text = "SELECT (DESC)";
            this.BtnSelectDesc.UseVisualStyleBackColor = false;
            this.BtnSelectDesc.Click += new System.EventHandler(this.BtnSelectDesc_Click);
            // 
            // BtnResultTypeOk
            // 
            this.BtnResultTypeOk.BackColor = System.Drawing.Color.Transparent;
            this.BtnResultTypeOk.Location = new System.Drawing.Point(819, 11);
            this.BtnResultTypeOk.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResultTypeOk.Name = "BtnResultTypeOk";
            this.BtnResultTypeOk.Size = new System.Drawing.Size(196, 46);
            this.BtnResultTypeOk.TabIndex = 16;
            this.BtnResultTypeOk.Text = "OK";
            this.BtnResultTypeOk.UseVisualStyleBackColor = false;
            this.BtnResultTypeOk.Click += new System.EventHandler(this.BtnResultTypeOk_Click);
            // 
            // BtnResultTypeAll
            // 
            this.BtnResultTypeAll.BackColor = System.Drawing.Color.DimGray;
            this.BtnResultTypeAll.Location = new System.Drawing.Point(1019, 11);
            this.BtnResultTypeAll.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResultTypeAll.Name = "BtnResultTypeAll";
            this.BtnResultTypeAll.Size = new System.Drawing.Size(196, 46);
            this.BtnResultTypeAll.TabIndex = 16;
            this.BtnResultTypeAll.Text = "ALL";
            this.BtnResultTypeAll.UseVisualStyleBackColor = false;
            this.BtnResultTypeAll.Click += new System.EventHandler(this.BtnResultTypeAll_Click);
            // 
            // GridViewAlignList
            // 
            this.GridViewAlignList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewAlignList.Location = new System.Drawing.Point(11, 61);
            this.GridViewAlignList.Margin = new System.Windows.Forms.Padding(2);
            this.GridViewAlignList.Name = "GridViewAlignList";
            this.GridViewAlignList.RowTemplate.Height = 23;
            this.GridViewAlignList.Size = new System.Drawing.Size(1204, 800);
            this.GridViewAlignList.TabIndex = 17;
            this.GridViewAlignList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAlignList_CellClick);
            this.GridViewAlignList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAlignList_CellEnter);
            this.GridViewAlignList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GridViewAlignList_CellValueNeeded);
            this.GridViewAlignList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridViewAlignList_KeyDown);
            this.GridViewAlignList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GridViewAlignList_KeyUp);
            // 
            // panelDisplayPMS
            // 
            this.panelDisplayPMS.Location = new System.Drawing.Point(1220, 61);
            this.panelDisplayPMS.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayPMS.Name = "panelDisplayPMS";
            this.panelDisplayPMS.Size = new System.Drawing.Size(685, 398);
            this.panelDisplayPMS.TabIndex = 46;
            // 
            // panelDisplayVidi6
            // 
            this.panelDisplayVidi6.Location = new System.Drawing.Point(1220, 798);
            this.panelDisplayVidi6.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi6.Name = "panelDisplayVidi6";
            this.panelDisplayVidi6.Size = new System.Drawing.Size(685, 63);
            this.panelDisplayVidi6.TabIndex = 47;
            // 
            // panelDisplayVidi5
            // 
            this.panelDisplayVidi5.Location = new System.Drawing.Point(1220, 731);
            this.panelDisplayVidi5.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi5.Name = "panelDisplayVidi5";
            this.panelDisplayVidi5.Size = new System.Drawing.Size(685, 63);
            this.panelDisplayVidi5.TabIndex = 48;
            // 
            // panelDisplayVidi4
            // 
            this.panelDisplayVidi4.Location = new System.Drawing.Point(1220, 664);
            this.panelDisplayVidi4.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi4.Name = "panelDisplayVidi4";
            this.panelDisplayVidi4.Size = new System.Drawing.Size(685, 63);
            this.panelDisplayVidi4.TabIndex = 49;
            // 
            // panelDisplayVidi3
            // 
            this.panelDisplayVidi3.Location = new System.Drawing.Point(1220, 597);
            this.panelDisplayVidi3.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi3.Name = "panelDisplayVidi3";
            this.panelDisplayVidi3.Size = new System.Drawing.Size(685, 63);
            this.panelDisplayVidi3.TabIndex = 50;
            // 
            // panelDisplayVidi2
            // 
            this.panelDisplayVidi2.Location = new System.Drawing.Point(1220, 530);
            this.panelDisplayVidi2.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi2.Name = "panelDisplayVidi2";
            this.panelDisplayVidi2.Size = new System.Drawing.Size(685, 63);
            this.panelDisplayVidi2.TabIndex = 51;
            // 
            // panelDisplayVidi1
            // 
            this.panelDisplayVidi1.Location = new System.Drawing.Point(1220, 463);
            this.panelDisplayVidi1.Margin = new System.Windows.Forms.Padding(2);
            this.panelDisplayVidi1.Name = "panelDisplayVidi1";
            this.panelDisplayVidi1.Size = new System.Drawing.Size(685, 63);
            this.panelDisplayVidi1.TabIndex = 52;
            // 
            // CFormReportProcess60
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1916, 863);
            this.Controls.Add(this.panelDisplayVidi6);
            this.Controls.Add(this.panelDisplayVidi5);
            this.Controls.Add(this.panelDisplayVidi4);
            this.Controls.Add(this.panelDisplayVidi3);
            this.Controls.Add(this.panelDisplayVidi2);
            this.Controls.Add(this.panelDisplayVidi1);
            this.Controls.Add(this.panelDisplayPMS);
            this.Controls.Add(this.GridViewAlignList);
            this.Controls.Add(this.BtnResultTypeAll);
            this.Controls.Add(this.BtnResultTypeOk);
            this.Controls.Add(this.BtnResultTypeNg);
            this.Controls.Add(this.BtnSelectDesc);
            this.Controls.Add(this.BtnSelectAsc);
            this.Controls.Add(this.BtnSaveToCsv);
            this.Controls.Add(this.DateTimeTo);
            this.Controls.Add(this.DateTimeFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFormReportProcess60";
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
    }
}