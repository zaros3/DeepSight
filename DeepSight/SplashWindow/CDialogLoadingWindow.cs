using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSight
{
    public partial class CDialogLoadingWindow : Form
    {
        delegate void SteringParameterDelegate(int iIndex, string Text);
        delegate void StringParamterWithStatusDelegate(int iIndex, string Text, TypeOfMessage objTypeOfMessage);
        delegate void SplashShowCloseDelegate();

        bool m_bCloseSplashScreenFlag = false;

        public CDialogLoadingWindow(string strProgramName, string strVersion)
        {
            InitializeComponent();
            Initialize(strProgramName, strVersion);
        }

        public bool Initialize(string strProgramName, string strVersion)
        {
            bool bReturn = false;

            do
            {
                // 프로그램 이름
                this.labelTitle.Text = strProgramName;
                // 버전
                this.labelVersion.Text = strVersion;
                // 라벨 색상 추가.
                this.labelMessage.Text = "WAITING";
                this.labelMessage.ForeColor = Color.Green;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public void ShowSplashScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SplashShowCloseDelegate(ShowSplashScreen));
                return;
            }
            try
            {
                this.Show();
                Application.Run(this);
            }
            catch (Exception)
            {
            }

        }

        public void CloseSplashScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SplashShowCloseDelegate(CloseSplashScreen));
                return;
            }
            m_bCloseSplashScreenFlag = true;
            this.Close();
        }

        public void UpdateStatusText(int iIndex, string Text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SteringParameterDelegate(UpdateStatusText), new object[] { iIndex, Text });
                return;
            }
            labelMessage.ForeColor = Color.Green;
            labelMessage.Text = Text;
            progressBar1.Value = iIndex;
            Application.DoEvents();
        }

        public void UpdateStatusTextWithStatus(int iIndex, string Text, TypeOfMessage objTypeOfMessage)
        {
            if (100 <= iIndex) iIndex = 99;
            if (InvokeRequired)
            {
                BeginInvoke(new StringParamterWithStatusDelegate(UpdateStatusTextWithStatus), new object[] { iIndex, Text, objTypeOfMessage });
                return;
            }

            switch (objTypeOfMessage)
            {
                case TypeOfMessage.Success:
                    labelMessage.ForeColor = Color.Green;
                    break;
                case TypeOfMessage.Warning:
                    labelMessage.ForeColor = Color.Yellow;
                    break;
                case TypeOfMessage.Error:
                    labelMessage.ForeColor = Color.Red;
                    break;
            }
            labelMessage.Text = Text;
            progressBar1.Value = iIndex;

            this.pictureProgress.Refresh();
            Application.DoEvents();
            //this.labelPercent.Text = string.Format( "{0:D3} %", iIndex );	
        }

        private void CDialogLoadingWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (false == m_bCloseSplashScreenFlag) e.Cancel = true;
        }

        public int GetPrograssPoint()
        {
            return progressBar1.Value;
        }

        private void pictureProgress_Paint(object sender, PaintEventArgs e)
        {
            // Clear the background.
            e.Graphics.Clear(pictureProgress.BackColor);

            // Draw the progress bar.
            float fraction =
                (float)(progressBar1.Value - progressBar1.Minimum) /
                (progressBar1.Maximum - progressBar1.Minimum);
            int wid = (int)(fraction * pictureProgress.ClientSize.Width);
            e.Graphics.FillRectangle(
                Brushes.LimeGreen, 0, 0, wid,
                pictureProgress.ClientSize.Height);

            // Draw the text.
            e.Graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                int percent = (int)(fraction * 100);
                e.Graphics.DrawString(
                    percent.ToString() + "%",
                    this.labelMessage.Font, Brushes.Black,
                    pictureProgress.ClientRectangle, sf);
            }
        }
    }
}
