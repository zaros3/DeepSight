using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepSight
{
    public partial class CFormMenu : Form, CFormInterface
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // CFormView
        private CFormView m_objView = null;
        // Button Menu
        private Button[] m_objMenu;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormMenu()
        {   
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormMenu_Load(object sender, EventArgs e)
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
        private void CFormMenu_FormClosed( object sender, FormClosedEventArgs e )
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
        private bool Initialize()
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
                CMainFrame objFormMain = Owner as CMainFrame;
                m_objView = objFormMain.GetFormView() as CFormView;
                // 버튼 이어줌
                m_objMenu = new Button[ ( int )CDefine.FormView.FORM_VIEW_FINAL ];
                m_objMenu[ ( int )CDefine.FormView.FORM_VIEW_MAIN ] = this.BtnMain;
                m_objMenu[ ( int )CDefine.FormView.FORM_VIEW_SETUP ] = this.BtnSetup;
                m_objMenu[ ( int )CDefine.FormView.FORM_VIEW_CONFIG ] = this.BtnConfig;
				m_objMenu[ ( int )CDefine.FormView.FORM_VIEW_IO ] = this.BtnIO;
				m_objMenu[ ( int )CDefine.FormView.FORM_VIEW_REPORT ] = this.BtnReport;
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 시작
                timer.Interval = 100;
                timer.Enabled = true;

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
            for( int iLoopCount = 0; iLoopCount < ( int )CDefine.FormView.FORM_VIEW_FINAL; iLoopCount++ ) {
				pFormCommon.SetButtonColor( this.m_objMenu[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }

            pFormCommon.SetButtonColor( this.BtnLanguage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnExit, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            this.BackColor = pFormCommon.COLOR_FORM_VIEW;
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
                SetControlChangeLanguage( this.BtnMain );
                SetControlChangeLanguage( this.BtnConfig );
                SetControlChangeLanguage( this.BtnSetup );
				SetControlChangeLanguage( this.BtnIO );
				SetControlChangeLanguage( this.BtnReport );
                SetControlChangeLanguage( this.BtnExit );

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
        //목적 : 언어 UI 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetLanguageUI()
        {
            var pDocument = CDocument.GetDocument;
			var pFormCommon = CFormCommon.GetFormCommon;
            if( CDefine.enumLanguage.LANGUAGE_KOREA == pDocument.m_objConfig.GetSystemParameter().eLanguage ) {
				pFormCommon.SetButtonText( this.BtnLanguage, "KOREA" );
            }
			else if( CDefine.enumLanguage.LANGUAGE_CHINA == pDocument.m_objConfig.GetSystemParameter().eLanguage ) {
				pFormCommon.SetButtonText( this.BtnLanguage, "CHINA" );
            }
			else if( CDefine.enumLanguage.LANGUAGE_ENGLISH == pDocument.m_objConfig.GetSystemParameter().eLanguage ) {
				pFormCommon.SetButtonText( this.BtnLanguage, "ENGLISH" );
            } else if( CDefine.enumLanguage.LANGUAGE_VIETNAM == pDocument.m_objConfig.GetSystemParameter().eLanguage ) {
                pFormCommon.SetButtonText( this.BtnLanguage, "VIETNAM" );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 Form 색상 강조
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetCurrentFormColor()
        {
			var pFormCommon = CFormCommon.GetFormCommon;
            for( int iLoopFormView = 0; iLoopFormView < ( int )CDefine.FormView.FORM_VIEW_FINAL; iLoopFormView++ ) {
				pFormCommon.SetButtonColor( m_objMenu[ iLoopFormView ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }
            pFormCommon.SetButtonColor( m_objMenu[ ( int )m_objView.GetCurrentForm() ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 권한에 따른 버튼 상태 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetChangeButtonStatus()
        {
            var pDocument = CDocument.GetDocument;
            CUserInformation objUserInformation = pDocument.GetUserInformation();

            if( CDefine.enumRunMode.RUN_MODE_STOP == pDocument.GetRunMode() || CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER == objUserInformation.m_eAuthorityLevel )  {
                for( int iLoopMenu = 0; iLoopMenu < m_objMenu.Length; iLoopMenu++ ) {
                    // 현재 유저 권한 레벨이 폼 Read 레벨보다 낮으면 메뉴 버튼 막음
                    if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelForm[ iLoopMenu ].eLevelRead ) {
                        //Report는 살리자
                        if (iLoopMenu == (int)CDefine.FormView.FORM_VIEW_REPORT)
                            continue;
                        // 버튼 Disable 상태 아니면 Disable로 변경
                        if ( true == m_objMenu[ iLoopMenu ].Enabled ) {
                            m_objMenu[ iLoopMenu ].Enabled = false;
                        }
                    }
                        // 아니면 메뉴 버튼 살림
                    else {
                        // 버튼 Enable 상태 아니면 Enable로 변경
                        if( false == m_objMenu[ iLoopMenu ].Enabled ) {
                            m_objMenu[ iLoopMenu ].Enabled = true;
                        }
                    }
                }
            } else {
                for( int iLoopMenu = 0; iLoopMenu < m_objMenu.Length-1; iLoopMenu++ ) {
                    // 버튼 Enable 상태 아니면 Enable로 변경
                    if( ( int )CDefine.FormView.FORM_VIEW_MAIN != iLoopMenu) {
                        if( true == m_objMenu[ iLoopMenu ].Enabled ) {
                            m_objMenu[ iLoopMenu ].Enabled = false;
                        }
                    }
                }
            }
           
            // 언어 버튼
            if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelLanguage.eLevelRead ) {
                // 버튼 Disable 상태 아니면 Disable로 변경
                if( true == this.BtnLanguage.Enabled ) {
                    this.BtnLanguage.Enabled = false;
                }
            }
            else {
                // 버튼 Enable 상태 아니면 Enable로 변경
                if( false == this.BtnLanguage.Enabled ) {
                    this.BtnLanguage.Enabled = true;
                }
            }
            // 종료 버튼
            if( ( int )objUserInformation.m_eAuthorityLevel < ( int )pDocument.m_objAuthorityParameter.m_objLevelExit.eLevelRead ) {
                // 버튼 Disable 상태 아니면 Disable로 변경
                if( true == this.BtnExit.Enabled ) {
                    this.BtnExit.Enabled = false;
                }
            }
            else {
                // 버튼 Enable 상태 아니면 Enable로 변경
                if( false == this.BtnExit.Enabled ) {
                    this.BtnExit.Enabled = true;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetTimer( bool bTimer )
        {
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : Visible 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetVisible( bool bVisible )
        {
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer_Tick( object sender, EventArgs e )
        {
            // 언어 UI 변경
            SetLanguageUI();
            // 현재 Form View 색 강조
            SetCurrentFormColor();
            // 권한에 따른 버튼 상태 변경
            SetChangeButtonStatus();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 메인 메뉴 전환
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMain_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Form Change : {1} -> {2}]", "BtnMain_Click", m_objView.GetCurrentForm().ToString(), CDefine.FormView.FORM_VIEW_MAIN.ToString() );
                pDocument.SetUpdateButtonLog( this, strLog );

                m_objView.SetChangeForm( CDefine.FormView.FORM_VIEW_MAIN );

                pDocument.SetUpdateButtonLog(this, "Complete " + CDefine.FormView.FORM_VIEW_MAIN.ToString());
            } while ( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 셋업 메뉴 전환
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnSetup_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Form Change : {1} -> {2}]", "BtnSetup_Click", m_objView.GetCurrentForm().ToString(), CDefine.FormView.FORM_VIEW_SETUP.ToString() );
                pDocument.SetUpdateButtonLog( this, strLog );

                m_objView.SetChangeForm( CDefine.FormView.FORM_VIEW_SETUP );

                pDocument.SetUpdateButtonLog(this, "Complete " + CDefine.FormView.FORM_VIEW_SETUP.ToString());
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 설정 메뉴 전환
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnConfig_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Form Change : {1} -> {2}]", "BtnConfig_Click", m_objView.GetCurrentForm().ToString(), CDefine.FormView.FORM_VIEW_CONFIG.ToString() );
                pDocument.SetUpdateButtonLog( this, strLog );

                m_objView.SetChangeForm( CDefine.FormView.FORM_VIEW_CONFIG );

                pDocument.SetUpdateButtonLog(this, "Complete " + CDefine.FormView.FORM_VIEW_CONFIG.ToString());
            } while( false );
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : IO 메뉴 전환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnIO_Click( object sender, EventArgs e )
		{
            var pDocument = CDocument.GetDocument;
			do {
				// 버튼 로그 추가
				string strLog = string.Format( "[{0}] [Form Change : {1} -> {2}]", "BtnIO_Click", m_objView.GetCurrentForm().ToString(), CDefine.FormView.FORM_VIEW_IO.ToString() );
				pDocument.SetUpdateButtonLog( this, strLog );

				m_objView.SetChangeForm( CDefine.FormView.FORM_VIEW_IO );

                pDocument.SetUpdateButtonLog(this, "Complete " + CDefine.FormView.FORM_VIEW_IO.ToString());
            } while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레포트 메뉴 전환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnReport_Click( object sender, EventArgs e )
		{
            var pDocument = CDocument.GetDocument;
			do {
				// 버튼 로그 추가
				string strLog = string.Format( "[{0}] [Form Change : {1} -> {2}]", "BtnReport_Click", m_objView.GetCurrentForm().ToString(), CDefine.FormView.FORM_VIEW_REPORT.ToString() );
				pDocument.SetUpdateButtonLog( this, strLog );

				m_objView.SetChangeForm( CDefine.FormView.FORM_VIEW_REPORT );

                pDocument.SetUpdateButtonLog(this, "Complete " + CDefine.FormView.FORM_VIEW_REPORT.ToString());
            } while( false );
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 언어 변환
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLanguage_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 언어 변경
            CConfig.CSystemParameter objOptionParameter = pDocument.m_objConfig.GetSystemParameter();
            //objOptionParameter.eLanguage = ( CDefine.enumLanguage )( ( ( int )objOptionParameter.eLanguage + 1 ) % ( int )CDefine.enumLanguage.LANGUAGE_FINAL );
            objOptionParameter.eLanguage = CDefine.enumLanguage.LANGUAGE_ENGLISH;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [Language : {1} -> {2}]", "BtnLanguage_Click", pDocument.m_objConfig.GetSystemParameter().eLanguage.ToString(), objOptionParameter.eLanguage.ToString() );
            pDocument.SetUpdateButtonLog( this, strLog );

            pDocument.m_objConfig.SaveSystemParameter( objOptionParameter );

            do {
                CMainFrame objMain = pDocument.GetMainFrame();
                if( null == objMain ) break;
                // 타이틀 변경
                CFormInterface objInterface = objMain.GetFormTitle() as CFormInterface;
                if( null == objInterface ) break;
                objInterface.SetChangeLanguage();
                // 메뉴 변경
                SetChangeLanguage();
                // View에 있는 현재 폼도 바꿔줌
                m_objView.SetChangeLanguage();

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 종료
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnExit_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
				if( CDefine.enumRunMode.RUN_MODE_STOP != pDocument.GetRunMode() ) {
					// 설비가 시작 상태입니다. 정지 상태로 변경해주세요.
					pDocument.SetMessage( CDefine.enumAlarmType.ALARM_WARNING, 10110 );
					break;
				}
				// 프로그램을 종료하시겠습니까?
				if( DialogResult.No == pDocument.SetMessage( CDefine.enumAlarmType.ALARM_QUESTION, 10001 ) ) break;
				// 카메라 라이브 종료하고 빠져나감
                for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
                    pDocument.SetLiveMode( iLoopCount, CDefine.enumLiveMode.LIVE_MODE_OFF );
                }
				// 라이브 종료 확인 3 msec ~
				int iTimeout = 3000;
				int iSleep = 1000;
				while( true ) {
					if( 0 >= iTimeout ) break;
					Application.DoEvents();
					iTimeout -= iSleep;
					System.Threading.Thread.Sleep( iSleep );
				}
				var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
				objMMFVisionData[ ( int )CDefine.enumCamera.CAMERA_1 ].iProgramExitStatus = ( int )CDefine.enumProgramExitStatus.PROGRAM_EXIT_STATUS_ON;

                pDocument.m_bProgramExit = true;
				
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}]", "BtnExit_Click" );
                pDocument.SetUpdateButtonLog( this, strLog );
				// 폼 메인 폼을 닫으면서 관련 부분 해제
				Application.Exit();
            } while( false );
        }
    }
}