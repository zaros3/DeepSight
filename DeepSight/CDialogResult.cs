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
    public partial class CDialogResult : Form
    {
        // 검사포지션
        private int m_iPositionInspection;
        private bool m_bResult;
        public delegate void DelegateUpdateDisplay( int iPosition, bool bResult );
        public DelegateUpdateDisplay m_delegateUpdateDisplayResult;
        public bool bShow;
        public CDialogResult()
        {
            InitializeComponent();
            Initialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool Initialize()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            do {
                m_delegateUpdateDisplayResult = new DelegateUpdateDisplay( UpdateDisplayResult );

                m_iPositionInspection = pDocument.GetInspectionIndex();
                // 폼 초기화
                if( false == InitializeForm() ) break;
                timer1.Start();
                bShow = true;
                bReturn = true;
            } while( false );

            return bReturn;
        }

      
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DeInitialize()
        {
        }

        public void SetInsepctionPosition( int iPosition, bool bResult )
        {
            var pDocument = CDocument.GetDocument;
            do {
                m_iPositionInspection = iPosition;
                m_bResult = bResult;
            } while( false );

        }

        public void UpdateDisplayResult( int iPosition, bool bResult )
        {
            do {
                m_iPositionInspection = iPosition;
                m_bResult = bResult;
                bShow = true;

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool InitializeForm()
        {
            bool bReturn = false;

            do {
                // 폼 중앙에서 생성
                this.CenterToParent();

                SetButtonColor();
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼 색상 정의
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonColor()
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            this.BackColor = pFormCommon.COLOR_FORM_VIEW;

            pFormCommon.SetButtonColor( this.BtnInspectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnResult, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCancel, pFormCommon.COLOR_WHITE, pFormCommon.LAMP_RED_OFF );
        }

       

        private void BtnInspectionPosition_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );

                string[] strButtonList = new string[ objRecipeParameter.iCountInspectionPosition ];
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.iCountInspectionPosition; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = "INSPECTION POSITION " + ( iLoopCount + 1 ).ToString();
                }
                CDialogEnumerate objDialog = new CDialogEnumerate( objRecipeParameter.iCountInspectionPosition, strButtonList, m_iPositionInspection );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    m_iPositionInspection = objDialog.GetResult() ;
                    
                }

                if( CDefine.enumResult.RESULT_OK == pDocument.GetInspectionResultAlign( m_iPositionInspection ).objResultCommon.eResult )
                    m_bResult = true;
                else
                    m_bResult = false;

                SetInsepctionPosition( m_iPositionInspection, m_bResult );

            } while( false );
        }

        private void timer1_Tick( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            pFormCommon.SetButtonText( this.BtnInspectionPosition, "INSPECTION POSITION " + ( m_iPositionInspection + 1 ).ToString() );
            string strResult = true == m_bResult ? "OK" : "NG";
            if( true == m_bResult ) {
                pFormCommon.SetButtonText( this.BtnResult, "OK" );
                pFormCommon.SetButtonColor( this.BtnResult, pFormCommon.COLOR_GREEN, pFormCommon.COLOR_UNACTIVATE );
            } else {
                pFormCommon.SetButtonText( this.BtnResult, "NG" );
                pFormCommon.SetButtonColor( this.BtnResult, pFormCommon.COLOR_RED, pFormCommon.COLOR_UNACTIVATE );
            }

        }

        private void BtnCancel_Click( object sender, EventArgs e )
        {
            this.Hide();
            bShow = false;
        }
    }
}
