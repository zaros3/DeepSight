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
using System.Threading;

namespace DeepSight
{
    public partial class CFormCommunicationPLC : Form, CFormInterface
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
        private Button[] m_BtnInputAddress = new Button[ m_iDisplayPageIO ];
        private Button[] m_BtnInputName = new Button[ m_iDisplayPageIO ];
        // Output IO 버튼 배열
        private Button[] m_BtnOutputAddress = new Button[ m_iDisplayPageIO ];
        private Button[] m_BtnOutputName = new Button[ m_iDisplayPageIO ];
        // IO 정보 객체 
        private CConfig.CPLCInitializeParameter m_objPLCParameter;
        // IO 객체
        private HLDevice.CDevicePLC m_objPLC;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
		// 유저 권한 레벨에 따른 버튼 상태 변경
		public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
		public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;

        // PLC 비트 폴링 스레드
        private Thread m_ThreadProcessPLC;
        private bool m_bThreadExit;
        private bool m_bIsReading;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CFormCommunicationPLC()
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
                m_bThreadExit = false;
                m_ThreadProcessPLC = new Thread( ThreadProcessPLC );
                m_ThreadProcessPLC.Start( this );
                m_bIsReading = false;
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
            m_bThreadExit = true;
            m_ThreadProcessPLC.Join();
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
                m_objPLCParameter = pDocument.m_objConfig.GetPLCParameter();

                // 해당 부분은 IO 디바이스에서 전체 수를 구해와서 나눠서 구해야 함
                int iInputIOCount = m_objPLCParameter.strPLCInput.Count;
                int iOutputIOCount = m_objPLCParameter.strPLCOutput.Count;
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
                m_objPLC = pDocument.m_objProcessMain.m_objPLC;
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
//                 SetButtonChangeLanguage( this.BtnTitleInputList );
//                 SetButtonChangeLanguage( this.BtnTitleOutputList );
                SetButtonChangeLanguage( this.BtnPreviousInput );
                SetButtonChangeLanguage( this.BtnNextInput );
                SetButtonChangeLanguage( this.BtnPreviousOutput );
                SetButtonChangeLanguage( this.BtnNextOutput );
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
        private void SetButtonChangeLanguage( Button objButton )
        {
            var pDocument = CDocument.GetDocument;
			var pFormCommon = CFormCommon.GetFormCommon;
			pFormCommon.SetButtonText( objButton, pDocument.GetDatabaseUIText( objButton.Name, this.Name ) );
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
                m_bIsReading = true;
            }
            else {
                timer.Enabled = false;
                m_bIsReading = false;
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
//                 // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
 				m_delegateSetChangeButtonStatus = new DelegateSetChangeButtonStatus( pFormCommon.SetChangeButtonStatus );
//                 // 페이지 편집 권한에 따라 버튼 상태 변경
 //				pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_IO );
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

            // 페이지 표시
            pFormCommon.SetButtonText( this.BtnTitleCurrentPageInput, string.Format( "PAGE ( {0} / {1} )", m_iCurrentPage[ ( int )enumIO.INPUT ], m_iMaxPage[ ( int )enumIO.INPUT ] ) );
            pFormCommon.SetButtonText( this.BtnTitleCurrentPageOutput, string.Format( "PAGE ( {0} / {1} )", m_iCurrentPage[ ( int )enumIO.OUTPUT ], m_iMaxPage[ ( int )enumIO.OUTPUT ] ) );

            for( int iLoopCount = 0; iLoopCount < m_iDisplayPageIO; iLoopCount++ ) {
                if( m_objPLCParameter.strPLCOutput.Count() > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ) {

                    // BIT 표시
                    if( m_objPLCParameter.iOutputCountBit > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ) {
                        bool bResult = false;
                        bResult = m_objPLC.HLGetInterfacePLC().bInterfacePlcBitOut[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ];
                        if( true == bResult ) {
                            pFormCommon.SetButtonBackColor( m_BtnOutputName[ iLoopCount ], pFormCommon.COLOR_ACTIVATE );
                            m_BtnOutputName[ iLoopCount ].Text = "ON";
                        } else {
                            pFormCommon.SetButtonBackColor( m_BtnOutputName[ iLoopCount ], pFormCommon.COLOR_UNACTIVATE );
                            m_BtnOutputName[ iLoopCount ].Text = "OFF";
                        }
                    } else if (m_objPLCParameter.iOutputCountWord > iLoopCount + (m_iCurrentPage[(int)enumIO.OUTPUT] * m_iDisplayPageIO)) {
                        // WORD 표시
                        m_BtnOutputName[iLoopCount].Text = string.Format("{0}", m_objPLC.HLGetInterfacePLC().sInterfacePlcWordOut[ ( iLoopCount + (m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ) - m_objPLCParameter.iOutputCountBit ] );
                        pFormCommon.SetButtonBackColor(m_BtnOutputName[iLoopCount], pFormCommon.COLOR_UNACTIVATE);
                    } else {
                        // DWORD 표시
                        m_BtnOutputName[ iLoopCount ].Text = string.Format( "{0}", m_objPLC.HLGetInterfacePLC().dInterfacePlcDWordIn[ ( iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ) - m_objPLCParameter.iOutputCountBit ] );
                        pFormCommon.SetButtonBackColor( m_BtnOutputName[ iLoopCount ], pFormCommon.COLOR_UNACTIVATE );
                    }
                } else {
                    m_BtnOutputName[ iLoopCount ].Text = "";
                }

                if( m_objPLCParameter.strPLCInput.Count() > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ) {
                    // BIT 표시
                    if( m_objPLCParameter.iInputCountBit > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ) {
                        bool bResult = false;
                        bResult = m_objPLC.HLGetInterfacePLC().bInterfacePlcBitIn[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ];
                        if( true == bResult ) {
                            pFormCommon.SetButtonBackColor( m_BtnInputName[ iLoopCount ], pFormCommon.COLOR_ACTIVATE );
                            m_BtnInputName[ iLoopCount ].Text = "ON";
                        } else {
                            pFormCommon.SetButtonBackColor( m_BtnInputName[ iLoopCount ], pFormCommon.COLOR_UNACTIVATE );
                            m_BtnInputName[ iLoopCount ].Text = "OFF";
                        }
                    } else if (m_objPLCParameter.iInputCountWord > iLoopCount + (m_iCurrentPage[(int)enumIO.INPUT] * m_iDisplayPageIO)) {
                        // WORD 표시
                        m_BtnInputName[iLoopCount].Text = string.Format("{0}", m_objPLC.HLGetInterfacePLC().sInterfacePlcWordIn[(iLoopCount + (m_iCurrentPage[(int)enumIO.INPUT] * m_iDisplayPageIO)) - m_objPLCParameter.iInputCountBit]);
                        pFormCommon.SetButtonBackColor(m_BtnInputName[iLoopCount], pFormCommon.COLOR_UNACTIVATE);
                    } else {
                        // DWORD 표시
                        m_BtnInputName[ iLoopCount ].Text = string.Format( "{0:F3}", m_objPLC.HLGetInterfacePLC().dInterfacePlcDWordIn[ ( iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ) - m_objPLCParameter.iInputCountBit - m_objPLCParameter.iInputCountWord ] );
                        pFormCommon.SetButtonBackColor( m_BtnInputName[ iLoopCount ], pFormCommon.COLOR_UNACTIVATE );
                    }
                } else {
                    m_BtnInputName[ iLoopCount ].Text = "";
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
                if( m_objPLCParameter.strPLCInput.Count() > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ) {
                    string strName = m_objPLCParameter.strPLCInput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.INPUT ] * m_iDisplayPageIO ) ];
                    if( strName != m_objPLCParameter.objPLCParameter[ strName ].strPLCName ) {
                        m_BtnInputAddress[ iLoopCount ].Text = strName + " : " + m_objPLCParameter.objPLCParameter[ strName ].strPLCName;
                    } else {
                        m_BtnInputAddress[ iLoopCount ].Text = strName;
                    }
                } else {
                    m_BtnInputAddress[ iLoopCount ].Text = "";
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
                if( m_objPLCParameter.strPLCOutput.Count() > iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ) {
                    string strName = m_objPLCParameter.strPLCOutput[ iLoopCount + ( m_iCurrentPage[ ( int )enumIO.OUTPUT ] * m_iDisplayPageIO ) ];
                    if( strName != m_objPLCParameter.objPLCParameter[ strName ].strPLCName ) {
                        m_BtnOutputAddress[ iLoopCount ].Text = strName + " : " + m_objPLCParameter.objPLCParameter[ strName ].strPLCName;
                    } else {
                        m_BtnOutputAddress[ iLoopCount ].Text = strName;
                    }
                } else {
                    m_BtnOutputAddress[ iLoopCount ].Text = "";
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
//             short[] pReadData = new short[ 3 ];
// 
//             short[] sData = new short[ 3 ];
//             sData[ 0 ] = 111;
//             sData[ 1 ] = 123;
//             sData[ 2 ] = 456;
//             m_objPLC.HLWriteWordFromPLC( "D100", 3, sData, true );
// 
// 
//             m_objPLC.HLReadWordFromPLC( "D101", 3, ref pReadData, true );

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
        //목적 : PLC 버튼 클릭 ( 출력 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnOutputName_Click( object sender, EventArgs e )
        {
            Button BtnSelected = ( Button )sender;
            string strValue = BtnSelected.Text;

            try {
                var pDocument = CDocument.GetDocument;
                // 현재 유저 정보 받음
                CUserInformation objUserInformation = pDocument.GetUserInformation();
                if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )CDefine.FormView.FORM_VIEW_IO ].eLevelWrite ) {
                    return;
                }

                if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() )
                    return;

                // 데이터 입력란이기때문에 주소의 버튼이름을 가져온다 인덱스는 동일하기때문에 인덱스를 제외하고 버튼이름을 변경한다
                string strBtnName = BtnSelected.Name.Replace( "BtnOutputName", "BtnOutputAddress" );

                for( int iLoopCount = 0; iLoopCount < this.Controls.Count; iLoopCount++ ) {
                    if( strBtnName == this.Controls[ iLoopCount ].Name ) {
                        strBtnName = this.Controls[ iLoopCount ].Text;
                    }
                }

                strBtnName.Replace( " ", "" );
                // PLC 접근 이름을 가져온다 Split( : ) 하는 이유는 이름이 설정되어 있지 않는 경우는 다이렉트 주소를 표시하기 때문에 다이렉트 주소가 이름이 되기때문
                string[] strNamePLC = strBtnName.Split( ':' );

                // 이름, 주소가 있는지 판단하여 진행
                if( 1 < strNamePLC.Count() ) {
                    strBtnName = strNamePLC[ 1 ];
                } else {
                    strBtnName = strNamePLC[ 0 ];
                }

                bool bResult = false;
                // WORD OUT
                if( CConfig.CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter.objPLCParameter[ strBtnName ].eCommunicationType ) {
                    FormKeyPad objKeyPad = new FormKeyPad(double.Parse(strValue));
                    if (System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog())
                    {
                        m_objPLC.HLWriteWordFromPLC(strBtnName, (short)objKeyPad.m_dResultValue);
                    }
                    // BIT
                } else if( CConfig.CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_OUT == m_objPLCParameter.objPLCParameter[ strBtnName ].eCommunicationType ) {
                    bResult = m_objPLC.HLGetInterfacePLC().bInterfacePlcBitOut[ m_objPLCParameter.objPLCParameter[ strBtnName ].iInOutIndex ];
                    if( false == bResult ) m_objPLC.HLWriteBitFromPLC( strBtnName, true );
                    else m_objPLC.HLWriteBitFromPLC( strBtnName, false );

                } else {
                    FormKeyPad objKeyPad = new FormKeyPad( double.Parse( strValue ) );
                    if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                        m_objPLC.HLWriteWordFromPLC( strBtnName, objKeyPad.m_dResultValue );
                        //if( false == strBtnName.Contains( "_T_" ) ) {
                        //    m_objPLC.HLWriteWordFromPLC( strBtnName, objKeyPad.m_dResultValue );
                        //} else {
                        //    m_objPLC.HLWriteWordFromPLC( strBtnName, objKeyPad.m_dResultValue * 10 );
                        //}
                    }
                }
            } catch( Exception ex ) {
                System.Diagnostics.Trace.WriteLine( ex.StackTrace );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 버튼 클릭 ( 입력 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnInputName_Click( object sender, EventArgs e )
        {
            Button BtnSelected = ( Button )sender;
            string strValue = BtnSelected.Text;

            try {
                var pDocument = CDocument.GetDocument;
                // 현재 유저 정보 받음
                CUserInformation objUserInformation = pDocument.GetUserInformation();
                if( false == pDocument.m_objConfig.GetSystemParameter().bVidiTeachMode ) {
                    if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )CDefine.FormView.FORM_VIEW_IO ].eLevelWrite ) {
                        return;
                    }
                    if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationModePLC )
                        return;
                }
                

                if ( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationModePLC ) {
                    if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() )
                        return;
                }

                // 데이터 입력란이기때문에 주소의 버튼이름을 가져온다 인덱스는 동일하기때문에 인덱스를 제외하고 버튼이름을 변경한다
                string strBtnName = BtnSelected.Name.Replace( "BtnInputName", "BtnInputAddress" );

                for( int iLoopCount = 0; iLoopCount < this.Controls.Count; iLoopCount++ ) {
                    if( strBtnName == this.Controls[ iLoopCount ].Name ) {
                        strBtnName = this.Controls[ iLoopCount ].Text;
                    }
                }

                strBtnName.Replace( " ", "" );
                // PLC 접근 이름을 가져온다 Split( : ) 하는 이유는 이름이 설정되어 있지 않는 경우는 다이렉트 주소를 표시하기 때문에 다이렉트 주소가 이름이 되기때문
                string[] strNamePLC = strBtnName.Split( ':' );

                // 이름, 주소가 있는지 판단하여 진행
                if( 1 < strNamePLC.Count() ) {
                    strBtnName = strNamePLC[ 1 ];
                } else {
                    strBtnName = strNamePLC[ 0 ];
                }

                bool bResult = false;
                if( CConfig.CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter.objPLCParameter[ strBtnName ].eCommunicationType ) {
                    FormKeyPad objKeyPad = new FormKeyPad(double.Parse(strValue));
                    if (System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog())
                    {
                        m_objPLC.HLWriteWordFromPLC( strBtnName, ( short )objKeyPad.m_dResultValue );
                    }

                } else if( CConfig.CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter.objPLCParameter[ strBtnName ].eCommunicationType ) {
                    bResult = m_objPLC.HLGetInterfacePLC().bInterfacePlcBitIn[ m_objPLCParameter.objPLCParameter[ strBtnName ].iInOutIndex ];
                    if( false == bResult ) m_objPLC.HLWriteBitFromPLC( strBtnName, true );
                    else m_objPLC.HLWriteBitFromPLC( strBtnName, false );

                } else {
                    FormKeyPad objKeyPad = new FormKeyPad( double.Parse( strValue ) );
                    if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                        m_objPLC.HLWriteWordFromPLC( strBtnName, objKeyPad.m_dResultValue );
                        //if( false == strBtnName.Contains( "_T_" ) ) {
                        //    m_objPLC.HLWriteWordFromPLC( strBtnName, objKeyPad.m_dResultValue );
                        //} else {
                        //    m_objPLC.HLWriteWordFromPLC( strBtnName, objKeyPad.m_dResultValue * 10 );
                        //}
                    }
                }
            } catch( Exception ex ) {
                System.Diagnostics.Trace.WriteLine( ex.StackTrace );
            }
        }

      

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC READ
        //설명 : G5 설비 데이터 리딩 맵 확인 후 매칭작업
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ReadPLCDataMachineTypeProcess60()
        {
            var pDocument = CDocument.GetDocument;
            CConfig.CPLCInitializeParameter objPLCParameter = pDocument.m_objConfig.GetPLCParameter();
            short[] pReadData = new short[ objPLCParameter.iInputCountWord ];
            double[] pReadDwordData = new double[ objPLCParameter.iInputCountDWord ];

            short[] pWriteData = new short[ objPLCParameter.iOutputCountWord ];
            double[] pWriteDwordData = new double[ objPLCParameter.iOutputCountDWord ];

            // READ 영역 READ
            // DOUBLE WORD영역
            m_objPLC.HLReadWordFromPLC("PLC_MODEL_CHANGE", objPLCParameter.iInputCountWord, ref pReadData);

            // LVDT 로그데이터 갯수는 50개
            m_objPLC.HLReadDoubleWordFromPLC( "PLC_LVDT_CELL_ID_1", 50, ref pReadDwordData );
            // LEAD돌출검서 데이터 갯수는 A,B합쳐서 150개
            m_objPLC.HLReadDoubleWordFromPLC( "PLC_LEAD_CELL_ID_A_1", 150, ref pReadDwordData );

            // WRITE 영역 READ
            // DOUBLE WORD영역
            m_objPLC.HLReadWordFromPLC("PC_VISION_ALIVE", objPLCParameter.iOutputCountWord, ref pWriteData);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC READ
        //설명 : 2M 설비 데이터 리딩 맵 확인 후 매칭작업
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ReadPLCDataMachineTypeProcess110()
        {
            var pDocument = CDocument.GetDocument;
            CConfig.CPLCInitializeParameter objPLCParameter = pDocument.m_objConfig.GetPLCParameter();
            short[] pReadData = new short[objPLCParameter.iInputCountWord];
            double[] pReadDwordData = new double[objPLCParameter.iInputCountDWord];

            short[] pWriteData = new short[objPLCParameter.iOutputCountWord];
            double[] pWriteDwordData = new double[objPLCParameter.iOutputCountDWord];

            // READ 영역 READ
            // DOUBLE WORD영역
            m_objPLC.HLReadWordFromPLC("PLC_MODEL_CHANGE", objPLCParameter.iInputCountWord, ref pReadData);

            // WRITE 영역 READ
            // DOUBLE WORD영역
            m_objPLC.HLReadWordFromPLC("PC_VISION_ALIVE", objPLCParameter.iOutputCountWord, ref pWriteData);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC READ
        //설명 : G4 설비 데이터 리딩 맵 확인 후 매칭작업
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ReadPLCDataMachineTypeProcess150()
        {
            var pDocument = CDocument.GetDocument;
            CConfig.CPLCInitializeParameter objPLCParameter = pDocument.m_objConfig.GetPLCParameter();
            short[] pReadData = new short[objPLCParameter.iInputCountWord];
            double[] pReadDwordData = new double[objPLCParameter.iInputCountDWord];

            short[] pWriteData = new short[objPLCParameter.iOutputCountWord];
            double[] pWriteDwordData = new double[objPLCParameter.iOutputCountDWord];

            // READ 영역 READ
            // DOUBLE WORD영역
            m_objPLC.HLReadWordFromPLC("PLC_MODEL_CHANGE", objPLCParameter.iInputCountWord, ref pReadData);

            // READ 영역 READ
            // LVDT 로그데이터 갯수는 50개
            m_objPLC.HLReadDoubleWordFromPLC( "PLC_WEIGHT_CELL_ID_1", 50, ref pReadDwordData );
            // LEAD돌출검서 데이터 갯수는 A,B합쳐서 150개
            m_objPLC.HLReadDoubleWordFromPLC( "PLC_LVDT_CELL_ID_1", 50, ref pReadDwordData );

            // WRITE 영역 READ
            // DOUBLE WORD영역
            m_objPLC.HLReadWordFromPLC("PC_VISION_ALIVE", objPLCParameter.iOutputCountWord, ref pWriteData);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 스레드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadProcessPLC( object state )
        {
            CFormCommunicationPLC pThis = ( CFormCommunicationPLC )state;
            var pDocument = CDocument.GetDocument;
            CDefine.enumMachineType eMachineType = pDocument.m_objConfig.GetSystemParameter().eMachineType;

            while( false == pThis.m_bThreadExit ) {
                if( true == pThis.m_bIsReading ) {
                    // 설비 타입에 맞게 리드함수 호출
                    if( CDefine.enumMachineType.PROCESS_60 == eMachineType ) {
                        pThis.ReadPLCDataMachineTypeProcess60();
                    } else if( CDefine.enumMachineType.PROCESS_110 == eMachineType ) {
                        pThis.ReadPLCDataMachineTypeProcess110();
                    } else if( CDefine.enumMachineType.PROCESS_150 == eMachineType ) {
                        pThis.ReadPLCDataMachineTypeProcess150();
                    }
                }
                
                Thread.Sleep( 700 );
            }
        }
    }
}