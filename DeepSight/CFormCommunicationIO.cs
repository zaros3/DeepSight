using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;
using HLDevice;
namespace DeepSight
{
    public partial class CFormCommunicationIO : Form, CFormInterface
    {
        // IO Input & Output
        private enum enumIO
        {
            INPUT = 0,
            OUTPUT,
            IO_FINAL
        };
        // 현재 IO 페이지
        private int[] m_iCurrentPage;
        // 최대 IO 페이지
        private int[] m_iMaxPage;
        // 페이지 당 표시할 IO 수
        private const int m_iDisplayPageIO = 32;
        // Input IO 버튼 배열
        Button[] m_BtnInputAddress = new Button[ m_iDisplayPageIO ];
        Button[] m_BtnInputName = new Button[ m_iDisplayPageIO ];
        // Output IO 버튼 배열
        Button[] m_BtnOutputAddress = new Button[ m_iDisplayPageIO ];
        Button[] m_BtnOutputName = new Button[ m_iDisplayPageIO ];
        // IO 정보 객체 
        CConfig.CIOInitializeParameter m_objIOParameter;
        // IO 객체
        HLDevice.CDeviceIO m_objIO;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
		// 유저 권한 레벨에 따른 버튼 상태 변경
		public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
		public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CFormCommunicationIO()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormSetupVisionOptical_Load( object sender, EventArgs e )
        {
            // 초기화
            Initialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 종료
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormSetupVisionOptical_FormClosed( object sender, FormClosedEventArgs e )
        {
            // 해제
            DeInitialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Initialize()
        {
            bool bReturn = false;

            do {

                // 폼 초기화
                if( false == InitializeForm() ) break;

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
                var pFormCommon = CFormCommon.GetFormCommon;
                var pDocument = CDocument.GetDocument;

                // 페이지 변수 초기화
                m_iCurrentPage = new int[ ( int )enumIO.IO_FINAL ];
                m_iMaxPage = new int[ ( int )enumIO.IO_FINAL ];

                // 버튼 매칭
                SetButtonControlInitialize();
               
                // IO 초기화 정보를 가져온다
                m_objIOParameter = pDocument.m_objConfig.GetIOParameter();

                // 해당 부분은 IO 디바이스에서 전체 수를 구해와서 나눠서 구해야 함
                int iInputIOCount = ( m_objIOParameter.iInputModuleCount * m_iDisplayPageIO ) + ( m_objIOParameter.iAnalogInputModuleCount * 16 );
                int iOutputIOCount = ( m_objIOParameter.iOutPutModuleCount * m_iDisplayPageIO ) + ( m_objIOParameter.iAnalogOutputModuleCount * 16 );
                int[] iIOCount = { iInputIOCount, iOutputIOCount };

                for( int iLoopIO = 0; iLoopIO < ( int )enumIO.IO_FINAL; iLoopIO++ ) {
                    // 현재 페이지 설정
                    m_iCurrentPage[ iLoopIO ] = 0;
                    // 최대 페이지 설정
                    m_iMaxPage[ iLoopIO ] = ( iIOCount[ iLoopIO ] / m_iDisplayPageIO );
                }
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetDisplayInput();
                SetDisplayOutput();
                // IO 객체 
                m_objIO = pDocument.m_objProcessMain.m_objIO;
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

            for( int iLoopCount = 0; iLoopCount < m_BtnInputName.Length; iLoopCount++ )
                pFormCommon.SetButtonColor( m_BtnInputName[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            for( int iLoopCount = 0; iLoopCount < m_BtnInputAddress.Length; iLoopCount++ )
                pFormCommon.SetButtonColor( m_BtnInputAddress[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

            for( int iLoopCount = 0; iLoopCount < m_BtnOutputName.Length; iLoopCount++ )
                pFormCommon.SetButtonColor( m_BtnOutputName[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            for( int iLoopCount = 0; iLoopCount < m_BtnOutputAddress.Length; iLoopCount++ )
                pFormCommon.SetButtonColor( m_BtnOutputAddress[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

            pFormCommon.SetButtonColor( BtnPreviousInput, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( BtnPreviousOutput, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( BtnNextInput, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( BtnNextOutput, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( BtnTitleCurrentPageInput, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( BtnTitleCurrentPageOutput, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

            pFormCommon.SetButtonColor( BtnTitleInputList, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( BtnTitleOutputList, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 스타일 달라붙음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetFormDockStyle( Form objForm, Panel objPanel )
        {
            objForm.Owner = this;
            objForm.TopLevel = false;
            objForm.Visible = true;
            objForm.Dock = DockStyle.Fill;
            objPanel.Controls.Add( objForm );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 데이터 불러오기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDataReLoad()
        {
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 언어 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do {

                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
//                 SetControlChangeLanguage( this.BtnTitleInputList );
//                 SetControlChangeLanguage( this.BtnTitleOutputList );
                SetControlChangeLanguage( this.BtnPreviousInput );
                SetControlChangeLanguage( this.BtnNextInput );
                SetControlChangeLanguage( this.BtnPreviousOutput );
                SetControlChangeLanguage( this.BtnNextOutput );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 언어 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetControlChangeLanguage( Control obj )
        {
			var pDocument = CDocument.GetDocument;
			obj.Text = pDocument.GetDatabaseUIText( obj.Name, this.Name );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetTimer( bool bTimer )
        {

            if( true == bTimer ) {
                timer.Enabled = true;
            }
            else {
                timer.Enabled = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : Visible 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetVisible( bool bVisible )
        {
            var pDocument = CDocument.GetDocument;
            this.Visible = bVisible;

            if( true == bVisible ) {
				var pFormCommon = CFormCommon.GetFormCommon;
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
				m_delegateSetChangeButtonStatus = new DelegateSetChangeButtonStatus( pFormCommon.SetChangeButtonStatus );
                // 페이지 편집 권한에 따라 버튼 상태 변경
				//pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_IO );
                // 해당 폼을 말단으로 설정
                pDocument.GetMainFrame().SetCurrentForm( this );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetLibrary()
        {
            bool bReturn = false;

            do {


                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer_Tick( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            var pDocument = CDocument.GetDocument;

            bool bResult = false;
            // 페이지 표시
            pFormCommon.SetButtonText( this.BtnTitleCurrentPageInput, string.Format( "PAGE ( {0} / {1} )", m_iCurrentPage[ ( int )enumIO.INPUT ], m_iMaxPage[ ( int )enumIO.INPUT ] ) );
            pFormCommon.SetButtonText( this.BtnTitleCurrentPageOutput, string.Format( "PAGE ( {0} / {1} )", m_iCurrentPage[ ( int )enumIO.OUTPUT ], m_iMaxPage[ ( int )enumIO.OUTPUT ] ) );
            // IO 상태 표시
            for( int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++ ) {
                if( iLoopCount < m_objIOParameter.objListIONameInput.Count ) {
                    string strNo = m_objIOParameter.objListIONameInput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ];
                    m_objIO.HLGetDigitalBit( m_objIOParameter.objIOParameter[ strNo ].strIOName, ref bResult );
                    if( true == bResult ) {
                        pFormCommon.SetButtonBackColor( m_BtnInputName[ iLoopCount ], pFormCommon.COLOR_ACTIVATE );
                    } else {
                        pFormCommon.SetButtonBackColor( m_BtnInputName[ iLoopCount ], pFormCommon.COLOR_UNACTIVATE );
                    }
                }
                
            }

            for( int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++ ) {
                if( iLoopCount < m_objIOParameter.objListIONameOutput.Count ) {
                    string strNo = m_objIOParameter.objListIONameOutput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ];
                    m_objIO.HLGetDigitalBit( m_objIOParameter.objIOParameter[ strNo ].strIOName, ref bResult );
                    if( true == bResult ) {
                        pFormCommon.SetButtonBackColor( m_BtnOutputName[ iLoopCount ], pFormCommon.COLOR_ACTIVATE );
                    } else {
                        pFormCommon.SetButtonBackColor( m_BtnOutputName[ iLoopCount ], pFormCommon.COLOR_UNACTIVATE );
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 입력 IO 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayInput()
        {
            for( int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++ ) {
                if( m_objIOParameter.objListIOAddressInput.Count() > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ) {
                    string strName = m_objIOParameter.objListIOAddressInput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ];
                    m_BtnInputAddress[ iLoopCount ].Text = strName;
                    strName = m_objIOParameter.objListIONameInput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ];
                    m_BtnInputName[ iLoopCount ].Text = strName;
                } else {
                    m_BtnInputAddress[ iLoopCount ].Text = "";
                    m_BtnInputName[ iLoopCount ].Text = "";
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 출력 IO 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayOutput()
        {
            for( int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++ ) {
                if( m_objIOParameter.objListIOAddressOutput.Count() > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ) {
                    string strName = m_objIOParameter.objListIOAddressOutput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ];
                    m_BtnOutputAddress[ iLoopCount ].Text = strName;
                    strName = m_objIOParameter.objListIONameOutput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ];
                    m_BtnOutputName[ iLoopCount ].Text = strName;
                } else {
                    m_BtnOutputAddress[ iLoopCount ].Text = "";
                    m_BtnOutputName[ iLoopCount ].Text = "";
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : IO 버튼 매칭
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonControlInitialize()
        {
            for( int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++ ) {
                string strNo = string.Format( "{0:D2}", iLoopCount + 1 );
                // 입력 어드레스 버튼 매칭
                m_BtnInputAddress[ iLoopCount ] = ( Controls.Find( "BtnInputAddress" + strNo, true ) )[ 0 ] as Button;
                // 입력 IO 이름 버튼 매칭
                m_BtnInputName[ iLoopCount ] = ( Controls.Find( "BtnInputName" + strNo, true ) )[ 0 ] as Button;
                // 출력 어드레스 버튼 매칭
                m_BtnOutputAddress[ iLoopCount ] = ( Controls.Find( "BtnOutputAddress" + strNo, true ) )[ 0 ] as Button;
                // 출력 IO 이름 버튼 매칭
                m_BtnOutputName[ iLoopCount ] = ( Controls.Find( "BtnOutputName" + strNo, true ) )[ 0 ] as Button;
            }
        }




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 입력 이전 페이지
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnPreviousInput_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                if( 0 >= m_iCurrentPage[ ( int )enumIO.INPUT ] ) break;
                m_iCurrentPage[ ( int )enumIO.INPUT ]--;
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Current Input Page : {1:D}]", "BtnPreviousInput_Click", m_iCurrentPage[ ( int )enumIO.INPUT ] );
                pDocument.SetUpdateButtonLog( this, strLog );
                // IO 관련 페이지 Repaint
                SetDisplayInput();
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 입력 다음 페이지
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnNextInput_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                if( m_iMaxPage[ ( int )enumIO.INPUT ] <= m_iCurrentPage[ ( int )enumIO.INPUT ] ) break;
                m_iCurrentPage[ ( int )enumIO.INPUT ]++;
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Current Input Page : {1:D}]", "BtnNextInput_Click", m_iCurrentPage[ ( int )enumIO.INPUT ] );
                pDocument.SetUpdateButtonLog( this, strLog );
                // IO 관련 페이지 Repaint
                SetDisplayInput();
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 출력 이전 페이지
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnPreviousOutput_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                if( 0 >= m_iCurrentPage[ ( int )enumIO.OUTPUT ] ) break;
                m_iCurrentPage[ ( int )enumIO.OUTPUT ]--;
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Current Output Page : {1:D}]", "BtnPreviousOutput_Click", m_iCurrentPage[ ( int )enumIO.OUTPUT ] );
                pDocument.SetUpdateButtonLog( this, strLog );
                // IO 관련 페이지 Repaint
                SetDisplayOutput();
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 출력 다음 페이지
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnNextOutput_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                if( m_iMaxPage[ ( int )enumIO.OUTPUT ] <= m_iCurrentPage[ ( int )enumIO.OUTPUT ] ) break;
                m_iCurrentPage[ ( int )enumIO.OUTPUT ]++;
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Current Output Page : {1:D}]", "BtnNextOutput_Click", m_iCurrentPage[ ( int )enumIO.OUTPUT ] );
                pDocument.SetUpdateButtonLog( this, strLog );
                // IO 관련 페이지 Repaint
                SetDisplayOutput();
            } while( false );
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : IO 버튼 클릭 ( 출력 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnOutputName_Click( object sender, EventArgs e )
        {
            Button BtnSelected = ( Button )sender;
            bool bResult = false;
            var pDocument = CDocument.GetDocument;
            // 현재 유저 정보 받음
            CUserInformation objUserInformation = pDocument.GetUserInformation();
            if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )CDefine.FormView.FORM_VIEW_IO ].eLevelWrite ) {
                return;
            }

            if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() )
                return;

            if( true == m_objIO.HLGetDigitalBit( BtnSelected.Text, ref bResult ) ) {
                if( true == bResult ) {
                    m_objIO.HLSetDigitalBit( BtnSelected.Text, false );
                    // 버튼 로그 추가
                    string strLog = string.Format( "[{0}] [Output Name : {1}] [Bit : {2}]", "BtnOutputName_Click", BtnSelected.Text, false );
                    pDocument.SetUpdateButtonLog( this, strLog );
                } else if( false == bResult ) {
                    m_objIO.HLSetDigitalBit( BtnSelected.Text, true );
                    // 버튼 로그 추가
                    string strLog = string.Format( "[{0}] [Output Name : {1}] [Bit : {2}]", "BtnOutputName_Click", BtnSelected.Text, true );
                    pDocument.SetUpdateButtonLog( this, strLog );
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : IO 버튼 클릭 ( 입력 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnInputName_Click( object sender, EventArgs e )
        {
            // 시뮬레이션 모드에서만 동작
            var pDocument = CDocument.GetDocument;
            // 현재 유저 정보 받음
            CUserInformation objUserInformation = pDocument.GetUserInformation();
            if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )CDefine.FormView.FORM_VIEW_IO ].eLevelWrite ) {
                return;
            }

            if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() )
                return;

            if( CDefine.enumSimulationMode.SIMULATION_MODE_ON == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                Button BtnSelected = ( Button )sender;
                bool bResult = false;

                if( true == m_objIO.HLGetDigitalBit( BtnSelected.Text, ref bResult ) ) {
                    if( true == bResult ) {
                        m_objIO.HLSetDigitalBit( BtnSelected.Text, false );
                        // 버튼 로그 추가
                        string strLog = string.Format( "[{0}] [Input Name : {1}] [Bit : {2}]", "BtnInputName_Click", BtnSelected.Text, false );
                        pDocument.SetUpdateButtonLog( this, strLog );
                    }
                    else if( false == bResult ) {
                        m_objIO.HLSetDigitalBit( BtnSelected.Text, true );
                        // 버튼 로그 추가
                        string strLog = string.Format( "[{0}] [Input Name : {1}] [Bit : {2}]", "BtnInputName_Click", BtnSelected.Text, true );
                        pDocument.SetUpdateButtonLog( this, strLog );
                    }
                }
            }
        }

    }
}