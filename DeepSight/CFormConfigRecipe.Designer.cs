namespace DeepSight
{
    partial class CFormConfigRecipe
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
            this.GridViewRecipeList = new System.Windows.Forms.DataGridView();
            this.BtnTitleCreateName = new System.Windows.Forms.Button();
            this.BtnCreateName = new System.Windows.Forms.Button();
            this.BtnTitleCreatePPID = new System.Windows.Forms.Button();
            this.BtnCreatePPID = new System.Windows.Forms.Button();
            this.BtnSelectedPPID = new System.Windows.Forms.Button();
            this.BtnTitleSelectedPPID = new System.Windows.Forms.Button();
            this.BtnTitleRecipeList = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewRecipeList)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // GridViewRecipeList
            // 
            this.GridViewRecipeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewRecipeList.Location = new System.Drawing.Point(14, 134);
            this.GridViewRecipeList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GridViewRecipeList.Name = "GridViewRecipeList";
            this.GridViewRecipeList.RowTemplate.Height = 23;
            this.GridViewRecipeList.Size = new System.Drawing.Size(394, 492);
            this.GridViewRecipeList.TabIndex = 8;
            this.GridViewRecipeList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewRecipeList_CellClick);
            // 
            // BtnTitleCreateName
            // 
            this.BtnTitleCreateName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCreateName.Location = new System.Drawing.Point(14, 808);
            this.BtnTitleCreateName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitleCreateName.Name = "BtnTitleCreateName";
            this.BtnTitleCreateName.Size = new System.Drawing.Size(194, 46);
            this.BtnTitleCreateName.TabIndex = 6;
            this.BtnTitleCreateName.Text = "NAME";
            this.BtnTitleCreateName.UseVisualStyleBackColor = true;
            this.BtnTitleCreateName.Click += new System.EventHandler(this.BtnCreatePPID_Click);
            // 
            // BtnCreateName
            // 
            this.BtnCreateName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCreateName.Location = new System.Drawing.Point(214, 808);
            this.BtnCreateName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnCreateName.Name = "BtnCreateName";
            this.BtnCreateName.Size = new System.Drawing.Size(194, 46);
            this.BtnCreateName.TabIndex = 6;
            this.BtnCreateName.UseVisualStyleBackColor = true;
            this.BtnCreateName.Click += new System.EventHandler(this.BtnCreateName_Click);
            // 
            // BtnTitleCreatePPID
            // 
            this.BtnTitleCreatePPID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleCreatePPID.Location = new System.Drawing.Point(14, 754);
            this.BtnTitleCreatePPID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitleCreatePPID.Name = "BtnTitleCreatePPID";
            this.BtnTitleCreatePPID.Size = new System.Drawing.Size(194, 46);
            this.BtnTitleCreatePPID.TabIndex = 6;
            this.BtnTitleCreatePPID.Text = "PPID";
            this.BtnTitleCreatePPID.UseVisualStyleBackColor = true;
            this.BtnTitleCreatePPID.Click += new System.EventHandler(this.BtnCreatePPID_Click);
            // 
            // BtnCreatePPID
            // 
            this.BtnCreatePPID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCreatePPID.Location = new System.Drawing.Point(214, 754);
            this.BtnCreatePPID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnCreatePPID.Name = "BtnCreatePPID";
            this.BtnCreatePPID.Size = new System.Drawing.Size(194, 46);
            this.BtnCreatePPID.TabIndex = 6;
            this.BtnCreatePPID.UseVisualStyleBackColor = true;
            this.BtnCreatePPID.Click += new System.EventHandler(this.BtnCreatePPID_Click);
            // 
            // BtnSelectedPPID
            // 
            this.BtnSelectedPPID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSelectedPPID.Location = new System.Drawing.Point(214, 80);
            this.BtnSelectedPPID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSelectedPPID.Name = "BtnSelectedPPID";
            this.BtnSelectedPPID.Size = new System.Drawing.Size(194, 46);
            this.BtnSelectedPPID.TabIndex = 6;
            this.BtnSelectedPPID.UseVisualStyleBackColor = true;
            // 
            // BtnTitleSelectedPPID
            // 
            this.BtnTitleSelectedPPID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleSelectedPPID.Location = new System.Drawing.Point(14, 80);
            this.BtnTitleSelectedPPID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitleSelectedPPID.Name = "BtnTitleSelectedPPID";
            this.BtnTitleSelectedPPID.Size = new System.Drawing.Size(194, 46);
            this.BtnTitleSelectedPPID.TabIndex = 6;
            this.BtnTitleSelectedPPID.Text = "SELECTED PPID";
            this.BtnTitleSelectedPPID.UseVisualStyleBackColor = true;
            // 
            // BtnTitleRecipeList
            // 
            this.BtnTitleRecipeList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTitleRecipeList.Location = new System.Drawing.Point(14, 15);
            this.BtnTitleRecipeList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnTitleRecipeList.Name = "BtnTitleRecipeList";
            this.BtnTitleRecipeList.Size = new System.Drawing.Size(394, 58);
            this.BtnTitleRecipeList.TabIndex = 4;
            this.BtnTitleRecipeList.Text = "RECIPE LIST";
            this.BtnTitleRecipeList.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(14, 634);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(128, 46);
            this.BtnSave.TabIndex = 9;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(147, 634);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(128, 46);
            this.BtnLoad.TabIndex = 9;
            this.BtnLoad.Text = "LOAD";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(280, 634);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(128, 46);
            this.BtnDelete.TabIndex = 9;
            this.BtnDelete.Text = "DELETE";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnCreate
            // 
            this.BtnCreate.Location = new System.Drawing.Point(14, 688);
            this.BtnCreate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnCreate.Name = "BtnCreate";
            this.BtnCreate.Size = new System.Drawing.Size(394, 58);
            this.BtnCreate.TabIndex = 9;
            this.BtnCreate.Text = "CREATE";
            this.BtnCreate.UseVisualStyleBackColor = true;
            this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // CFormConfigRecipe
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1916, 863);
            this.Controls.Add(this.BtnDelete);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.BtnCreate);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.GridViewRecipeList);
            this.Controls.Add(this.BtnTitleCreateName);
            this.Controls.Add(this.BtnCreateName);
            this.Controls.Add(this.BtnTitleCreatePPID);
            this.Controls.Add(this.BtnCreatePPID);
            this.Controls.Add(this.BtnSelectedPPID);
            this.Controls.Add(this.BtnTitleSelectedPPID);
            this.Controls.Add(this.BtnTitleRecipeList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFormConfigRecipe";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormConfigRecipe";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CFormConfigRecipe_FormClosed);
            this.Load += new System.EventHandler(this.CFormConfigRecipe_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewRecipeList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button BtnTitleRecipeList;
		private System.Windows.Forms.Button BtnCreatePPID;
        private System.Windows.Forms.Button BtnTitleSelectedPPID;
        private System.Windows.Forms.Button BtnSelectedPPID;
		private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataGridView GridViewRecipeList;
        private System.Windows.Forms.Button BtnTitleCreatePPID;
        private System.Windows.Forms.Button BtnCreateName;
        private System.Windows.Forms.Button BtnTitleCreateName;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.Button BtnLoad;
		private System.Windows.Forms.Button BtnDelete;
		private System.Windows.Forms.Button BtnCreate;
    }
}