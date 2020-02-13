using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; 
using System.Management;
using System.Threading;

namespace DeepSight
{
    public partial class CFormResourceInfo : Form
    {
        delegate void SteringParameterDelegate(int iIndex, string Text);
        delegate void StringParamterWithStatusDelegate( float dCpu, float dMemory, float dHddC, float dHddD );

        private Thread m_ThreadResourceInfo;
        private bool m_bThreadExit;

        PerformanceCounter m_PerformCtrCPU = new PerformanceCounter(); 
        PerformanceCounter m_PerformCtrCdrive = new PerformanceCounter();
        PerformanceCounter m_PerformCtrDdrive = new PerformanceCounter();
        private float m_dCpu;
        private float m_dMemory;
        private float m_dHddC;
        private float m_dHddD;

        Font m_objFont = new System.Drawing.Font( "굴림", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 129 ) ) );

        public CFormResourceInfo()
        {
            InitializeComponent();
            Initialize();
        }

        public bool Initialize()
        {
            bool bReturn = false;
            var pFormCommon = CFormCommon.GetFormCommon;

            do
            {
                pFormCommon.SetButtonColor( this.BtnTitleCpu, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
                pFormCommon.SetButtonColor( this.BtnTitleMemory, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
                pFormCommon.SetButtonColor( this.BtnTitleDriverC, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
                pFormCommon.SetButtonColor( this.BtnTitleDriverD, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

                m_PerformCtrCPU.CategoryName = "Processor";
                m_PerformCtrCPU.CounterName = "% Processor Time";
                m_PerformCtrCPU.InstanceName = "_Total";

                m_PerformCtrCdrive.CategoryName = "LogicalDisk";
                m_PerformCtrCdrive.CounterName = "% Free Space";
                m_PerformCtrCdrive.InstanceName = "C:";

                m_PerformCtrDdrive.CategoryName = "LogicalDisk";
                m_PerformCtrDdrive.CounterName = "% Free Space";
                m_PerformCtrDdrive.InstanceName = "D:";

                m_bThreadExit = false;
                m_ThreadResourceInfo = new Thread( ThreadResourceInfo );
                m_ThreadResourceInfo.Start( this );

                this.BackColor = pFormCommon.COLOR_FORM_VIEW;
                timer1.Interval = 200;
                timer1.Enabled = true;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public void DeInitialize()
        {
            // 스레드 종료
            m_bThreadExit = true;
            m_ThreadResourceInfo.Join();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 하트비트 쓰레드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadResourceInfo( object state )
        {
            CFormResourceInfo pThis = ( CFormResourceInfo )state;

            while( false == pThis.m_bThreadExit ) {
                pThis.ReadResource();
                Thread.Sleep( 200 );
            }
        }

        private void ReadResource()
        {
            m_dCpu = m_PerformCtrCPU.NextValue();
            m_dHddC = 100 - m_PerformCtrCdrive.NextValue();
            m_dHddD = 100 - m_PerformCtrDdrive.NextValue();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT * FROM Win32_OperatingSystem" );// WMI
            foreach( ManagementObject mo in searcher.Get() ) {
                double dbTotal = double.Parse( mo[ "TotalVisibleMemorySize" ].ToString() );
                dbTotal /= 1024;
                dbTotal /= 1024;

                double total_physical_memeory = double.Parse( mo[ "TotalVisibleMemorySize" ].ToString() );
                total_physical_memeory /= 1024;
                double free_physical_memeory = double.Parse( mo[ "FreePhysicalMemory" ].ToString() );
                free_physical_memeory /= 1024;
                double remmain_physical_memory = ( total_physical_memeory - free_physical_memeory ) / total_physical_memeory;
                m_dMemory = (float)remmain_physical_memory * 100;
            }
        }

        public void UpdateStatusTextWithStatus( float dCpu, float dMemory, float dHddC, float dHddD )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            if (100 <= dCpu ) dCpu = 99;
            if( 100 <= dMemory ) dMemory = 99;
            if( 100 <= dHddC ) dHddC = 99;
            if( 100 <= dHddD ) dHddD = 99;
            if (InvokeRequired)
            {
                BeginInvoke(new StringParamterWithStatusDelegate(UpdateStatusTextWithStatus), new object[] { dCpu, dMemory, dHddC, dHddD });
                return;
            }

            this.pictureProgressCPU.Refresh();
            this.pictureProgressMemory.Refresh();
            this.pictureProgressHddC.Refresh();
            this.pictureProgressHddD.Refresh();
            Application.DoEvents();
            //this.labelPercent.Text = string.Format( "{0:D3} %", iIndex );	
        }

        private void pictureProgressCPU_Paint( object sender, PaintEventArgs e)
        {
            // Clear the background.
            e.Graphics.Clear(pictureProgressCPU.BackColor);

            int iMinimum = 0;
            int iMaximum = 100;
            // Draw the progress bar.
            float fraction =
                (float)( m_dCpu - iMinimum ) /
                ( iMaximum - iMinimum );
            int wid = (int)(fraction * pictureProgressCPU.ClientSize.Width);
            if( 50 > m_dCpu )
                e.Graphics.FillRectangle( Brushes.LimeGreen, 0, 0, wid, pictureProgressCPU.ClientSize.Height );
            else if( 50 < m_dCpu && 80 > m_dCpu )
                e.Graphics.FillRectangle( Brushes.Orange, 0, 0, wid, pictureProgressCPU.ClientSize.Height );
            else
                e.Graphics.FillRectangle( Brushes.Red, 0, 0, wid, pictureProgressCPU.ClientSize.Height );

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
                    m_objFont, Brushes.White,
                    pictureProgressCPU.ClientRectangle, sf);
            }
        }

        private void pictureProgressMemory_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.Clear( pictureProgressMemory.BackColor );

            int iMinimum = 0;
            int iMaximum = 100;
            // Draw the progress bar.
            float fraction =
                ( float )( m_dMemory - iMinimum ) /
                ( iMaximum - iMinimum );
            int wid = ( int )( fraction * pictureProgressMemory.ClientSize.Width );
            if( 50 > m_dMemory )
                e.Graphics.FillRectangle( Brushes.LimeGreen, 0, 0, wid, pictureProgressMemory.ClientSize.Height );
            else if( 50 < m_dMemory && 80 > m_dMemory )
                e.Graphics.FillRectangle( Brushes.Orange, 0, 0, wid, pictureProgressMemory.ClientSize.Height );
            else 
                e.Graphics.FillRectangle( Brushes.Red, 0, 0, wid, pictureProgressMemory.ClientSize.Height );

            // Draw the text.
            e.Graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using( StringFormat sf = new StringFormat() ) {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                int percent = ( int )( fraction * 100 );
                e.Graphics.DrawString(
                    percent.ToString() + "%",
                    m_objFont, Brushes.White,
                    pictureProgressMemory.ClientRectangle, sf );
            }
        }

        private void pictureProgressHddC_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.Clear( pictureProgressHddC.BackColor );

            int iMinimum = 0;
            int iMaximum = 100;
            // Draw the progress bar.
            float fraction =
                ( float )( m_dHddC - iMinimum ) /
                ( iMaximum - iMinimum );
            int wid = ( int )( fraction * pictureProgressHddC.ClientSize.Width );
            if( 50 > m_dHddC )
                e.Graphics.FillRectangle( Brushes.LimeGreen, 0, 0, wid, pictureProgressHddC.ClientSize.Height );
            else if( 50 < m_dHddC && 80 > m_dHddC )
                e.Graphics.FillRectangle( Brushes.Orange, 0, 0, wid, pictureProgressHddC.ClientSize.Height );
            else
                e.Graphics.FillRectangle( Brushes.Red, 0, 0, wid, pictureProgressHddC.ClientSize.Height );

            // Draw the text.
            e.Graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            
            using( StringFormat sf = new StringFormat() ) {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                int percent = ( int )( fraction * 100 );
                e.Graphics.DrawString(
                    percent.ToString() + "%",
                    m_objFont, Brushes.White,
                    pictureProgressHddC.ClientRectangle, sf );
            }
        }

        private void pictureProgressHddD_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.Clear( pictureProgressHddD.BackColor );

            int iMinimum = 0;
            int iMaximum = 100;
            // Draw the progress bar.
            float fraction =
                ( float )( m_dHddD - iMinimum ) /
                ( iMaximum - iMinimum );
            int wid = ( int )( fraction * pictureProgressHddD.ClientSize.Width );
            if( 50 > m_dHddD )
                e.Graphics.FillRectangle( Brushes.LimeGreen, 0, 0, wid, pictureProgressHddD.ClientSize.Height );
            else if( 50 < m_dHddD && 80 > m_dHddD )
                e.Graphics.FillRectangle( Brushes.Orange, 0, 0, wid, pictureProgressHddD.ClientSize.Height );
            else
                e.Graphics.FillRectangle( Brushes.Red, 0, 0, wid, pictureProgressHddD.ClientSize.Height );

            // Draw the text.
            e.Graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            
            using( StringFormat sf = new StringFormat() ) {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                int percent = ( int )( fraction * 100 );
                e.Graphics.DrawString(
                    percent.ToString() + "%",
                    m_objFont, Brushes.White,
                    pictureProgressHddD.ClientRectangle, sf );
            }
        }

        private void timer1_Tick( object sender, EventArgs e )
        {
            UpdateStatusTextWithStatus( m_dCpu, m_dMemory, m_dHddC, m_dHddD );
        }

        private void CFormResourceInfo_FormClosing( object sender, FormClosingEventArgs e )
        {
            DeInitialize();
        }
    }
}
