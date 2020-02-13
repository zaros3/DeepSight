namespace DeepSight
{
    partial class CDialogMessage
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
            this.RichTextBoxAlarmDescriptionKorea = new System.Windows.Forms.RichTextBox();
            this.RichTextBoxAlarmDescriptionVietnam = new System.Windows.Forms.RichTextBox();
            this.TextBoxTitleAlarmDescription = new System.Windows.Forms.TextBox();
            this.TextBoxTitleAlarmPosition = new System.Windows.Forms.TextBox();
            this.TextBoxTitleAlarmObject = new System.Windows.Forms.TextBox();
            this.TextBoxTitleAlarmCode = new System.Windows.Forms.TextBox();
            this.TextBoxTitleAlarmTime = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmTime = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmCode = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmObject = new System.Windows.Forms.TextBox();
            this.TextBoxAlarmPosition = new System.Windows.Forms.TextBox();
            this.TextBoxTitleAlarmType = new System.Windows.Forms.TextBox();
            this.BtnYes = new System.Windows.Forms.Button();
            this.BtnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RichTextBoxAlarmDescriptionKorea
            // 
            this.RichTextBoxAlarmDescriptionKorea.BackColor = System.Drawing.Color.White;
            this.RichTextBoxAlarmDescriptionKorea.Location = new System.Drawing.Point(12, 316);
            this.RichTextBoxAlarmDescriptionKorea.Name = "RichTextBoxAlarmDescriptionKorea";
            this.RichTextBoxAlarmDescriptionKorea.Size = new System.Drawing.Size(960, 80);
            this.RichTextBoxAlarmDescriptionKorea.TabIndex = 6;
            this.RichTextBoxAlarmDescriptionKorea.TabStop = false;
            this.RichTextBoxAlarmDescriptionKorea.Text = "";
            this.RichTextBoxAlarmDescriptionKorea.Enter += new System.EventHandler(this.RichTextBoxKorea_Enter);
            // 
            // RichTextBoxAlarmDescriptionVietnam
            // 
            this.RichTextBoxAlarmDescriptionVietnam.BackColor = System.Drawing.Color.White;
            this.RichTextBoxAlarmDescriptionVietnam.Location = new System.Drawing.Point(12, 402);
            this.RichTextBoxAlarmDescriptionVietnam.Name = "RichTextBoxAlarmDescriptionVietnam";
            this.RichTextBoxAlarmDescriptionVietnam.Size = new System.Drawing.Size(960, 80);
            this.RichTextBoxAlarmDescriptionVietnam.TabIndex = 6;
            this.RichTextBoxAlarmDescriptionVietnam.TabStop = false;
            this.RichTextBoxAlarmDescriptionVietnam.Text = "";
            this.RichTextBoxAlarmDescriptionVietnam.Enter += new System.EventHandler(this.RichTextBoxVietnam_Enter);
            // 
            // TextBoxTitleAlarmDescription
            // 
            this.TextBoxTitleAlarmDescription.BackColor = System.Drawing.Color.White;
            this.TextBoxTitleAlarmDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitleAlarmDescription.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTitleAlarmDescription.Location = new System.Drawing.Point(12, 275);
            this.TextBoxTitleAlarmDescription.Name = "TextBoxTitleAlarmDescription";
            this.TextBoxTitleAlarmDescription.ReadOnly = true;
            this.TextBoxTitleAlarmDescription.Size = new System.Drawing.Size(960, 35);
            this.TextBoxTitleAlarmDescription.TabIndex = 9;
            this.TextBoxTitleAlarmDescription.TabStop = false;
            this.TextBoxTitleAlarmDescription.Text = "ALARM DESCRIPTION";
            this.TextBoxTitleAlarmDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBoxTitleAlarmDescription.Enter += new System.EventHandler(this.TextBoxTitleAlarmDescription_Enter);
            // 
            // TextBoxTitleAlarmPosition
            // 
            this.TextBoxTitleAlarmPosition.BackColor = System.Drawing.Color.White;
            this.TextBoxTitleAlarmPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitleAlarmPosition.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTitleAlarmPosition.Location = new System.Drawing.Point(12, 234);
            this.TextBoxTitleAlarmPosition.Name = "TextBoxTitleAlarmPosition";
            this.TextBoxTitleAlarmPosition.ReadOnly = true;
            this.TextBoxTitleAlarmPosition.Size = new System.Drawing.Size(229, 35);
            this.TextBoxTitleAlarmPosition.TabIndex = 9;
            this.TextBoxTitleAlarmPosition.TabStop = false;
            this.TextBoxTitleAlarmPosition.Text = "POSITION";
            this.TextBoxTitleAlarmPosition.Enter += new System.EventHandler(this.TextBoxTitleAlarmPosition_Enter);
            // 
            // TextBoxTitleAlarmObject
            // 
            this.TextBoxTitleAlarmObject.BackColor = System.Drawing.Color.White;
            this.TextBoxTitleAlarmObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitleAlarmObject.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTitleAlarmObject.Location = new System.Drawing.Point(12, 193);
            this.TextBoxTitleAlarmObject.Name = "TextBoxTitleAlarmObject";
            this.TextBoxTitleAlarmObject.ReadOnly = true;
            this.TextBoxTitleAlarmObject.Size = new System.Drawing.Size(229, 35);
            this.TextBoxTitleAlarmObject.TabIndex = 9;
            this.TextBoxTitleAlarmObject.TabStop = false;
            this.TextBoxTitleAlarmObject.Text = "OBJECT";
            this.TextBoxTitleAlarmObject.Enter += new System.EventHandler(this.TextBoxTitleAlarmObject_Enter);
            // 
            // TextBoxTitleAlarmCode
            // 
            this.TextBoxTitleAlarmCode.BackColor = System.Drawing.Color.White;
            this.TextBoxTitleAlarmCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitleAlarmCode.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTitleAlarmCode.Location = new System.Drawing.Point(12, 152);
            this.TextBoxTitleAlarmCode.Name = "TextBoxTitleAlarmCode";
            this.TextBoxTitleAlarmCode.ReadOnly = true;
            this.TextBoxTitleAlarmCode.Size = new System.Drawing.Size(229, 35);
            this.TextBoxTitleAlarmCode.TabIndex = 9;
            this.TextBoxTitleAlarmCode.TabStop = false;
            this.TextBoxTitleAlarmCode.Text = "CODE";
            this.TextBoxTitleAlarmCode.Enter += new System.EventHandler(this.TextBoxTitleAlarmCode_Enter);
            // 
            // TextBoxTitleAlarmTime
            // 
            this.TextBoxTitleAlarmTime.BackColor = System.Drawing.Color.White;
            this.TextBoxTitleAlarmTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitleAlarmTime.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTitleAlarmTime.Location = new System.Drawing.Point(12, 111);
            this.TextBoxTitleAlarmTime.Name = "TextBoxTitleAlarmTime";
            this.TextBoxTitleAlarmTime.ReadOnly = true;
            this.TextBoxTitleAlarmTime.Size = new System.Drawing.Size(229, 35);
            this.TextBoxTitleAlarmTime.TabIndex = 9;
            this.TextBoxTitleAlarmTime.TabStop = false;
            this.TextBoxTitleAlarmTime.Text = "TIME";
            this.TextBoxTitleAlarmTime.Enter += new System.EventHandler(this.TextBoxTitleAlarmTime_Enter);
            // 
            // TextBoxAlarmTime
            // 
            this.TextBoxAlarmTime.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmTime.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxAlarmTime.Location = new System.Drawing.Point(247, 111);
            this.TextBoxAlarmTime.Name = "TextBoxAlarmTime";
            this.TextBoxAlarmTime.ReadOnly = true;
            this.TextBoxAlarmTime.Size = new System.Drawing.Size(724, 35);
            this.TextBoxAlarmTime.TabIndex = 9;
            this.TextBoxAlarmTime.TabStop = false;
            this.TextBoxAlarmTime.Text = "ALARM TIME";
            this.TextBoxAlarmTime.Enter += new System.EventHandler(this.TextBoxAlarmTime_Enter);
            // 
            // TextBoxAlarmCode
            // 
            this.TextBoxAlarmCode.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmCode.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxAlarmCode.Location = new System.Drawing.Point(247, 152);
            this.TextBoxAlarmCode.Name = "TextBoxAlarmCode";
            this.TextBoxAlarmCode.ReadOnly = true;
            this.TextBoxAlarmCode.Size = new System.Drawing.Size(724, 35);
            this.TextBoxAlarmCode.TabIndex = 9;
            this.TextBoxAlarmCode.TabStop = false;
            this.TextBoxAlarmCode.Text = "ALARM CODE";
            this.TextBoxAlarmCode.Enter += new System.EventHandler(this.TextBoxAlarmCode_Enter);
            // 
            // TextBoxAlarmObject
            // 
            this.TextBoxAlarmObject.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmObject.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxAlarmObject.Location = new System.Drawing.Point(247, 193);
            this.TextBoxAlarmObject.Name = "TextBoxAlarmObject";
            this.TextBoxAlarmObject.ReadOnly = true;
            this.TextBoxAlarmObject.Size = new System.Drawing.Size(724, 35);
            this.TextBoxAlarmObject.TabIndex = 9;
            this.TextBoxAlarmObject.TabStop = false;
            this.TextBoxAlarmObject.Text = "ALARM OBJECT";
            this.TextBoxAlarmObject.Enter += new System.EventHandler(this.TextBoxAlarmObject_Enter);
            // 
            // TextBoxAlarmPosition
            // 
            this.TextBoxAlarmPosition.BackColor = System.Drawing.Color.White;
            this.TextBoxAlarmPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAlarmPosition.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxAlarmPosition.Location = new System.Drawing.Point(247, 234);
            this.TextBoxAlarmPosition.Name = "TextBoxAlarmPosition";
            this.TextBoxAlarmPosition.ReadOnly = true;
            this.TextBoxAlarmPosition.Size = new System.Drawing.Size(724, 35);
            this.TextBoxAlarmPosition.TabIndex = 9;
            this.TextBoxAlarmPosition.TabStop = false;
            this.TextBoxAlarmPosition.Text = "ALARM POSITION";
            this.TextBoxAlarmPosition.Enter += new System.EventHandler(this.TextBoxAlarmPosition_Enter);
            // 
            // TextBoxTitleAlarmType
            // 
            this.TextBoxTitleAlarmType.BackColor = System.Drawing.Color.White;
            this.TextBoxTitleAlarmType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxTitleAlarmType.Font = new System.Drawing.Font("맑은 고딕", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTitleAlarmType.Location = new System.Drawing.Point(12, 12);
            this.TextBoxTitleAlarmType.Name = "TextBoxTitleAlarmType";
            this.TextBoxTitleAlarmType.ReadOnly = true;
            this.TextBoxTitleAlarmType.Size = new System.Drawing.Size(959, 93);
            this.TextBoxTitleAlarmType.TabIndex = 9;
            this.TextBoxTitleAlarmType.TabStop = false;
            this.TextBoxTitleAlarmType.Text = "TYPE";
            this.TextBoxTitleAlarmType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBoxTitleAlarmType.Enter += new System.EventHandler(this.TextBoxTitleAlarmType_Enter);
            // 
            // BtnYes
            // 
            this.BtnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnYes.Location = new System.Drawing.Point(397, 487);
            this.BtnYes.Name = "BtnYes";
            this.BtnYes.Size = new System.Drawing.Size(284, 46);
            this.BtnYes.TabIndex = 10;
            this.BtnYes.Text = "YES";
            this.BtnYes.UseVisualStyleBackColor = true;
            this.BtnYes.Click += new System.EventHandler(this.BtnYes_Click);
            // 
            // BtnNo
            // 
            this.BtnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNo.Location = new System.Drawing.Point(688, 487);
            this.BtnNo.Name = "BtnNo";
            this.BtnNo.Size = new System.Drawing.Size(284, 46);
            this.BtnNo.TabIndex = 10;
            this.BtnNo.Text = "NO";
            this.BtnNo.UseVisualStyleBackColor = true;
            this.BtnNo.Click += new System.EventHandler(this.BtnNo_Click);
            // 
            // CDialogMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 545);
            this.ControlBox = false;
            this.Controls.Add(this.BtnNo);
            this.Controls.Add(this.BtnYes);
            this.Controls.Add(this.TextBoxAlarmPosition);
            this.Controls.Add(this.TextBoxAlarmObject);
            this.Controls.Add(this.TextBoxAlarmCode);
            this.Controls.Add(this.TextBoxTitleAlarmType);
            this.Controls.Add(this.TextBoxAlarmTime);
            this.Controls.Add(this.TextBoxTitleAlarmTime);
            this.Controls.Add(this.TextBoxTitleAlarmCode);
            this.Controls.Add(this.TextBoxTitleAlarmObject);
            this.Controls.Add(this.TextBoxTitleAlarmPosition);
            this.Controls.Add(this.TextBoxTitleAlarmDescription);
            this.Controls.Add(this.RichTextBoxAlarmDescriptionVietnam);
            this.Controls.Add(this.RichTextBoxAlarmDescriptionKorea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CDialogMessage";
            this.Text = "[ Message ]";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CDialogMessage_FormClosed);
            this.Load += new System.EventHandler(this.CDialogMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBoxAlarmDescriptionKorea;
		private System.Windows.Forms.RichTextBox RichTextBoxAlarmDescriptionVietnam;
        private System.Windows.Forms.TextBox TextBoxTitleAlarmDescription;
        private System.Windows.Forms.TextBox TextBoxTitleAlarmPosition;
        private System.Windows.Forms.TextBox TextBoxTitleAlarmObject;
        private System.Windows.Forms.TextBox TextBoxTitleAlarmCode;
        private System.Windows.Forms.TextBox TextBoxTitleAlarmTime;
        private System.Windows.Forms.TextBox TextBoxAlarmTime;
        private System.Windows.Forms.TextBox TextBoxAlarmCode;
        private System.Windows.Forms.TextBox TextBoxAlarmObject;
        private System.Windows.Forms.TextBox TextBoxAlarmPosition;
        private System.Windows.Forms.TextBox TextBoxTitleAlarmType;
		private System.Windows.Forms.Button BtnYes;
		private System.Windows.Forms.Button BtnNo;
    }
}