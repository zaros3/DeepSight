using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ENC.MemoryMap.Manager;
using ENC.MemoryMap.Pages;
using System.Data;
using HLDeviceAlign;
using System.Windows.Forms;
using Crevis.VirtualFG40Library;
using System.Threading;

namespace DeepSight
{
	public class CDocument
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//structure
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 자재 수량
		public struct structureMaterialCount : ICloneable
		{
			// 생산수량
			public int[] iMaterialCount;
			// 시간당 생산수량
			public int iMaterialCountHour;
			// 시간 비교하여 시간 당 생산량 저장하기 위함
			public DateTime objSystemTime;

			public object Clone()
			{
				structureMaterialCount objData = new structureMaterialCount();
				objData.iMaterialCount = this.iMaterialCount.Clone() as int[];
				objData.iMaterialCountHour = this.iMaterialCountHour;
				objData.objSystemTime = this.objSystemTime;
				return objData;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 싱글톤 패턴 도큐먼트 객체 
		private static CDocument m_objDocument = null;
		// 싱글톤 카메라 객체 크레비스는 하나의 객체만 생성가능함
		protected static VirtualFG40Library m_objCrevisCamera = null;
		// 설비 운용 상태
		private CDefine.enumRunMode m_eRunMode;
		// 트리거
		private CDefine.enumTrigger[] m_eTrigger;
		// 라이브 모드
		private CDefine.enumLiveMode[] m_eLiveMode;

		// 검사 결과 ( 얼라인 )
		private CInspectionResult.CResult[] m_objInspectionResultAlign;
		// 검사 결과 락 ( 얼라인 )
		private object m_objLockInspectionResultAlign;
		// 이미지 데이터
		private Cognex.VisionPro.CogImage8Grey[] m_objLiveImage;
		// 이미지 데이터 락
		private object m_objLockLiveImage;
		// 검사 보정량 데이터 락
		private object m_objLockInspectionRevisionData;
		// 현재 로그인된 유저 정보
		private CUserInformation m_objUserInformation;
        // 현재 검사인덱스
        private int m_iInspectionIndex;
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Config 클래스
		public CConfig m_objConfig;
		// 레시피 클래스
		public CRecipe m_objRecipe;
		// 로그인 클래스
		public CLogin m_objLogin;
		// 프로세스 메인
		public CProcessMain m_objProcessMain;
		// 프로세스 데이터베이스
		public CProcessDatabase m_objProcessDatabase;
		// 마스터 로그인 유무 ( 초기 셋업용 )
		public bool m_bMasterLogin = false;
		// 권한 파라미터
		public CDefine.CAuthorityParameter m_objAuthorityParameter;
		// 프로그램 종료 플래그
		public bool m_bProgramExit;
		// 프로그래스 바 ( 스레드 )
		public Thread m_ThreadSplashWindow;
		// 켈리브레이션 로딩 결과
		public bool m_bCalibrationComplete = false;
        // 레시피 변경여부
        public bool m_bRecipeChange;
        // 이미지 검사모드
        public bool m_bImageInspectionMode;

        //150공정 배면 모니터링 다이얼로그
        private CDialogResultMonitor m_objResultMonitor;
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Process Message
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		[System.Runtime.InteropServices.DllImport( "user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto )]
		public static extern IntPtr FindWindow( string strClassName, string strWindowName );

		[System.Runtime.InteropServices.DllImport( "user32.dll" )]
		public static extern int SendMessage( IntPtr hWindow, Int32 iMessage, IntPtr wParam, ref CDefine.COPYDATA_STRUCT objCopyData );

		[System.Runtime.InteropServices.DllImport( "user32.dll" )]
		public static extern int SendMessage( IntPtr hWindow, Int32 iMessage, IntPtr wParam, IntPtr lParam );

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDocument()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소멸자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		~CDocument()
		{
			m_objDocument = null;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 싱글톤 패턴 생성
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static CDocument GetDocument
		{
			get
			{
				if( null == m_objDocument ) {
					m_objDocument = new CDocument();
				}
				return m_objDocument;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 싱글톤 패턴 생성
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static VirtualFG40Library GetCrevisCamera
		{
			get
			{
				if( null == m_objCrevisCamera ) {
					m_objCrevisCamera = new VirtualFG40Library();
					m_objCrevisCamera.InitSystem();
					m_objCrevisCamera.UpdateDevice();
				}
				return m_objCrevisCamera;
			}
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
				// 프로그래스 바 생성
				m_ThreadSplashWindow = new Thread( new ThreadStart( CLoadingScreen.ShowSplashScreen ) );
				m_ThreadSplashWindow.IsBackground = true;
				m_ThreadSplashWindow.Start();
				// 프로그램 종료 플래그
				m_bProgramExit = false;
				m_objUserInformation = new CUserInformation();
				m_objUserInformation.m_strID = "Admin";
                m_bRecipeChange = false;
                m_bImageInspectionMode = false;
                // Config 객체 생성
                m_objConfig = new CConfig();
				if( false == m_objConfig.Initialize() ) break;
				// 권한 설정
				if( CDefine.enumSimulationMode.SIMULATION_MODE_ON == m_objConfig.GetSystemParameter().eSimulationMode ) {
					m_objUserInformation.m_eAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER;
				}
				else {
					m_objUserInformation.m_eAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
				}
				m_objUserInformation.m_strName = "SFA";
				// 권한 파라미터
				m_objAuthorityParameter = new CDefine.CAuthorityParameter();
				// 로그인 객체 생성
				m_objLogin = new CLogin();
				if( false == m_objLogin.Initialize() ) break;
				
				// 카메라 카운트 맥스로 생성
				int iCameraCount = ( int )CDefine.enumCamera.CAMERA_FINAL;
				// 트리거 생성 
				m_eTrigger = new CDefine.enumTrigger[ iCameraCount ];
                
				// 라이브모드
				m_eLiveMode = new CDefine.enumLiveMode[ iCameraCount ];
				
                // 검사 결과 ( 검사 포인트만큼 생성한다 )
				m_objInspectionResultAlign = new CInspectionResult.CResult[ CDefine.DEF_MAX_COUNT_INSPECTION_POSITION ];
				for( int iLoopCount = 0; iLoopCount < m_objInspectionResultAlign.Length; iLoopCount++ ) {
					m_objInspectionResultAlign[ iLoopCount ] = new CInspectionResult.CResult();
				}

				m_objLockInspectionResultAlign = new object();
				// 이미지 데이터
				m_objLiveImage = new Cognex.VisionPro.CogImage8Grey[ iCameraCount ];
				m_objLockLiveImage = new object();
				
				m_objLockInspectionRevisionData = new object();
				// 스마트 로그 실행
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "CConfig Initialize Start.", TypeOfMessage.Warning );
				SetCreateSmartLog();
				CLoadingScreen.UpdateStatusTextWithStatus( 5, "CConfig Initialize Completed.", TypeOfMessage.Success );
				// 레시피 초기화
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "CRecipe Initialize Start.", TypeOfMessage.Warning );
				m_objRecipe = new CRecipe();
				if( false == m_objRecipe.Initialize() ) break;
				CLoadingScreen.UpdateStatusTextWithStatus( 10, "CRecipe Initialize Completed.", TypeOfMessage.Success );
				// 프로세스 메인 초기화
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "CProcessMain Initialize Start.", TypeOfMessage.Warning );
				m_objProcessMain = new CProcessMain();
				if( false == m_objProcessMain.Initialize() ) break;
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "CProcessMain Initialize Completed.", TypeOfMessage.Success );
				// 프로세스 데이터베이스 초기화
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "DataBase Initialize Start.", TypeOfMessage.Warning );
				m_objProcessDatabase = new CProcessDatabase();
				if( false == m_objProcessDatabase.Initialize() ) break;
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "DataBase Initialize Completed.", TypeOfMessage.Success );

                m_iInspectionIndex = 0;

                //전/후면 구분해야하지 않나?
                if( CDefine.enumMachineType.PROCESS_150 == m_objConfig.GetSystemParameter().eMachineType ) {
                    m_objResultMonitor = new CDialogResultMonitor();
                    m_objResultMonitor.Show();

                }
                    

                var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
				objMMFVisionData[ ( int )CDefine.enumCamera.CAMERA_1 ].iProgramExitStatus = ( int )CDefine.enumProgramExitStatus.PROGRAM_EXIT_STATUS_OFF;
                
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
		public void Deinitialize()
		{
			// 프로세스 메인 해제
			m_objProcessMain.DeInitialize();
			// 프로세스 데이터베이스 해제
			m_objProcessDatabase.DeInitialize();
			// 로그인 해제
			m_objLogin.DeInitialize();
			// 모델 해제
			m_objRecipe.Deinitialize();
			// 설정 해제
			m_objConfig.DeInitialize();
			// 스마트 로그 종료
			SetCloseSmartLog();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 설비 운용 상태 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDefine.enumRunMode GetRunMode()
		{
			return m_eRunMode;
		}

		public void SetRunMode( CDefine.enumRunMode eRunMode )
		{
			m_eRunMode = eRunMode;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트리거 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDefine.enumTrigger GetTrigger( int iCameraIndex )
		{
			return m_eTrigger[ iCameraIndex ];
		}

		public void SetTrigger( int iCameraIndex, CDefine.enumTrigger eTrigger )
		{
			m_eTrigger[ iCameraIndex ] = eTrigger;
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 라이브 모드 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDefine.enumLiveMode GetLiveMode( int iCameraIndex )
		{
			return m_eLiveMode[ iCameraIndex ];
		}

		public void SetLiveMode( int iCameraIndex, CDefine.enumLiveMode eLiveMode )
		{
			m_eLiveMode[ iCameraIndex ] = eLiveMode;
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 검사 결과 ( 얼라인 ) 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CInspectionResult.CResult GetInspectionResultAlign( int iInspectionIndex )
		{
			return m_objInspectionResultAlign[ iInspectionIndex ];
		}

		public void SetInspectionResultAlign( int iInspectionIndex, CInspectionResult.CResult objResult )
		{
			lock( m_objLockInspectionResultAlign ) {
				m_objInspectionResultAlign[ iInspectionIndex ] = objResult;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 데이터 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public Cognex.VisionPro.CogImage8Grey GetLiveImage( int iCameraIndex )
		{
			return m_objLiveImage[ iCameraIndex ];
		}

		public void SetLiveImage( int iCameraIndex, Cognex.VisionPro.CogImage8Grey objLiveImage )
		{
			lock( m_objLockLiveImage ) {
				m_objLiveImage[ iCameraIndex ] = objLiveImage;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 디스플레이 및 데이터 업데이트
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetUpdateDisplayResultVIDI( int iCameraIndex, int iDisplayIndex )
		{
			CInspectionResult.CResult objResult = null;

			do {
				// 현재 올라가있는 얼라인 데이터
				objResult = m_objInspectionResultAlign[ iCameraIndex ];
				if( null == objResult ) break;

				switch( m_objConfig.GetSystemParameter().eMachineType ) {
					case CDefine.enumMachineType.PROCESS_60:
						CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
						objTypeA.BeginInvoke( objTypeA.m_delegateUpdateDisplayResultVIDI, (CFormMainProcess60.enumDisplayIndex)iDisplayIndex, objResult );
						break;
					case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        objTypeB.BeginInvoke( objTypeB.m_delegateUpdateDisplayResultVIDI, (CFormMainProcess110.enumDisplayIndex)iDisplayIndex, objResult);
                        break;
					case CDefine.enumMachineType.PROCESS_150:
                        if( CDefine.enumCameraType.CAMERA_AREA == m_objConfig.GetSystemParameter().eCameraType ) {
                            CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
                            objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayResultVIDI, ( CFormMainProcess150.enumDisplayIndex )iDisplayIndex, objResult );
                        } else {
                            CFormMainProcess150Gocator objTypeC = GetFormMain() as CFormMainProcess150Gocator;
                            objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayResultVIDI, ( CFormMainProcess150Gocator.enumDisplayIndex )iDisplayIndex, objResult );
                        }

                        break;
				}
				
			} while( false );
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 및 데이터 업데이트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateDisplayResultMeasure( int iCameraIndex, int iDisplayIndex )
        {
            CInspectionResult.CResult objResult = null;

            do {
                // 현재 올라가있는 얼라인 데이터
                objResult = m_objInspectionResultAlign[ iCameraIndex ];
                if( null == objResult ) break;

                switch( m_objConfig.GetSystemParameter().eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
                        objTypeA.BeginInvoke( objTypeA.m_delegateUpdateDisplayResultMeasure, ( CFormMainProcess60.enumDisplayIndex )iDisplayIndex, objResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        objTypeB.BeginInvoke( objTypeB.m_delegateUpdateDisplayResultMeasure, ( CFormMainProcess110.enumDisplayIndex )iDisplayIndex, objResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        if( CDefine.enumCameraType.CAMERA_AREA == m_objConfig.GetSystemParameter().eCameraType ) {
                            CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
                            objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayResultMeasure, ( CFormMainProcess150.enumDisplayIndex )iDisplayIndex, objResult );
                        } else {
                            CFormMainProcess150Gocator objTypeC = GetFormMain() as CFormMainProcess150Gocator;
                            objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayResultMeasure, ( CFormMainProcess150Gocator.enumDisplayIndex )iDisplayIndex, objResult );
                        }

                        break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 및 데이터 업데이트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateDisplayOriginal( int iCameraIndex, int iDisplayIndex )
        {
            CInspectionResult.CResult objResult = null;

            do
            {
                // 현재 올라가있는 얼라인 데이터
                objResult = m_objInspectionResultAlign[ iCameraIndex ];
                if ( null == objResult ) break;

                switch ( m_objConfig.GetSystemParameter().eMachineType )
                {
                    case CDefine.enumMachineType.PROCESS_60:
                        CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
                        objTypeA.BeginInvoke( objTypeA.m_delegateUpdateDisplayOriginal, (CFormMainProcess60.enumDisplayIndex)iDisplayIndex, objResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        objTypeB.BeginInvoke( objTypeB.m_delegateUpdateDisplayOriginal, (CFormMainProcess110.enumDisplayIndex)iDisplayIndex, objResult);
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
                        objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayOriginal, (CFormMainProcess150.enumDisplayIndex)iDisplayIndex, objResult);
                        break;
                }

            } while ( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 및 데이터 업데이트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateDisplay3D( int iCameraIndex, int iDisplayIndex )
        {
            CInspectionResult.CResult objResult = null;

            do {
                //3D 카메라 아니면 탈출!
                if( CDefine.enumCameraType.CAMERA_AREA == m_objConfig.GetSystemParameter().eCameraType ) break;

                // 현재 올라가있는 얼라인 데이터
                objResult = m_objInspectionResultAlign[ iCameraIndex ];
                if( null == objResult ) break;

                switch( m_objConfig.GetSystemParameter().eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
                        //objTypeA.BeginInvoke( objTypeA.m_delegateUpdateDisplayOriginal, ( CFormMainProcess60.enumDisplayIndex )iDisplayIndex, objResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        //objTypeB.BeginInvoke( objTypeB.m_delegateUpdateDisplayOriginal, ( CFormMainProcess110.enumDisplayIndex )iDisplayIndex, objResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        CFormMainProcess150Gocator objTypeC = GetFormMain() as CFormMainProcess150Gocator;
                        objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplay3D, ( CFormMainProcess150.enumDisplayIndex )iDisplayIndex, objResult );
                        break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 및 데이터 업데이트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateDisplayPMS( int iCameraIndex, int iDisplayIndex )
        {
            CInspectionResult.CResult objResult = null;

            do
            {
                // 현재 올라가있는 얼라인 데이터
                objResult = m_objInspectionResultAlign[ iCameraIndex ];
                if ( null == objResult ) break;

                switch ( m_objConfig.GetSystemParameter().eMachineType )
                {
                    case CDefine.enumMachineType.PROCESS_60:
                        CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
                        objTypeA.BeginInvoke( objTypeA.m_delegateUpdateDisplayPMS, ( CFormMainProcess60.enumDisplayIndex )iDisplayIndex, objResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        objTypeB.BeginInvoke( objTypeB.m_delegateUpdateDisplayPMS, (CFormMainProcess110.enumDisplayIndex)iDisplayIndex, objResult);
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
                        objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayPMS, (CFormMainProcess150.enumDisplayIndex)iDisplayIndex, objResult);
                        break;
                }

            } while ( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브 디스플레이 업데이트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateDisplayLive( int iCameraIndex )
		{
			Cognex.VisionPro.CogImage8Grey objLiveImage = null;

			do {
				// 현재 올라와있는 라이브 이미지
				objLiveImage = m_objLiveImage[ iCameraIndex ];
				if( null == objLiveImage ) break;

				switch( m_objConfig.GetSystemParameter().eMachineType ) {
					case CDefine.enumMachineType.PROCESS_60:
						CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
						objTypeA.BeginInvoke( objTypeA.m_delegateUpdateDisplayLive, iCameraIndex, objLiveImage );
						break;
					case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        objTypeB.BeginInvoke( objTypeB.m_delegateUpdateDisplayLive, iCameraIndex, objLiveImage);
                        break;
					case CDefine.enumMachineType.PROCESS_150:
                        CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
                        objTypeC.BeginInvoke( objTypeC.m_delegateUpdateDisplayLive, iCameraIndex, objLiveImage);
                        break;
				}
				
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetSaveImage( int iCameraIndex )
		{
			CInspectionResult.CResult objResult = null;

			do {
				// 현재 올라가있는 얼라인 데이터
				objResult = m_objInspectionResultAlign[ iCameraIndex ];
				if( null == objResult ) break;

// 				switch( m_objConfig.GetSystemParameter().eMachineType ) {
// 					case CDefine.enumMachineType.PROCESS_60:
// 						CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
// 						objTypeA.BeginInvoke( objTypeA.m_delegateSaveImage, iCameraIndex, objResult );
// 						break;
// 					case CDefine.enumMachineType.PROCESS_110:
//                         CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
//                         objTypeB.BeginInvoke(objTypeB.m_delegateSaveImage, iCameraIndex, objResult);
//                         break;
// 					case CDefine.enumMachineType.PROCESS_150:
//                         CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
//                         objTypeC.BeginInvoke(objTypeC.m_delegateSaveImage, iCameraIndex, objResult);
//                         break;
// 				}

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 메인 화면 포인터 받음
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public Form GetFormMain()
		{
			Form objFormMain = null;

			do {
				try {
					CMainFrame objMainFrame = GetMainFrame();
					if( null == objMainFrame ) break;

					CFormView objFormView = objMainFrame.GetFormView() as CFormView;
					if( null == objFormView ) break;

					Form obj = objFormView.GetFormView( CDefine.FormView.FORM_VIEW_MAIN );
					if( null == obj ) break;

					objFormMain = ( obj as CFormMain ).GetFormMain( CDefine.FormViewMain.FORM_VIEW_MAIN_ALIGN );
				}
				catch( Exception ex ) {
					SetUpdateLog( CDefine.enumLogType.LOG_ETC, ex.StackTrace );
				}

			} while( false );
			
			return objFormMain;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 현재 생성된 폼을 받아볼 수 있다.
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool GetIsForm( string strTypeName )
		{
			bool bReturn = false;

			try {
				System.Windows.Forms.FormCollection objOpenForms = System.Windows.Forms.Application.OpenForms;
				int iCount = objOpenForms.Count;
				for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
					if( strTypeName == objOpenForms[ iLoopCount ].GetType().Name ) {
						bReturn = true;
						break;
					}
				}
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.StackTrace );
			}

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 메인 프레임 받음
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CMainFrame GetMainFrame()
		{
			CMainFrame objMain = null;

			try {
				System.Windows.Forms.FormCollection objOpenForms = System.Windows.Forms.Application.OpenForms;
				int iCount = objOpenForms.Count;
				for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
					objMain = objOpenForms[ iLoopCount ] as CMainFrame;
					if( null != objMain ) {
						break;
					}
				}
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.StackTrace );
			}

			return objMain;
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 결과보기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDisplayResult( int iPosition, bool bResult )
        {
            do {
                switch( m_objConfig.GetSystemParameter().eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        CFormMainProcess60 objTypeA = GetFormMain() as CFormMainProcess60;
                        objTypeA.BeginInvoke( objTypeA.m_delegateUpdateResult, iPosition, bResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        CFormMainProcess110 objTypeB = GetFormMain() as CFormMainProcess110;
                        objTypeB.BeginInvoke( objTypeB.m_delegateUpdateResult, iPosition, bResult );
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        if( CDefine.enumCameraType.CAMERA_AREA == m_objConfig.GetSystemParameter().eCameraType ) {
                            CFormMainProcess150 objTypeC = GetFormMain() as CFormMainProcess150;
                            objTypeC.BeginInvoke( objTypeC.m_delegateUpdateResult, iPosition, bResult );
                        } else {
                            CFormMainProcess150Gocator objTypeC = GetFormMain() as CFormMainProcess150Gocator;
                            objTypeC.BeginInvoke( objTypeC.m_delegateUpdateResult, iPosition, bResult );
                        }
                        break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 결과보기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDisplayResultMonitor( int iPosition )
        {
            do {
                switch( m_objConfig.GetSystemParameter().eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        m_objResultMonitor.BeginInvoke( m_objResultMonitor.m_delegateUpdateDisplayResult, iPosition );
                        break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 결과보기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDisplayResultMonitorHistory( string strCellID )
        {
            do {
                switch( m_objConfig.GetSystemParameter().eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        m_objResultMonitor.BeginInvoke( m_objResultMonitor.m_delegateUpdateDisplayResultHistory, strCellID );
                        break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 스마트로그 실행
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetCreateSmartLog()
		{
			do {
				// SVI 프로그램이 켜져있는지 확인.
				Process[] ProcessList = Process.GetProcessesByName( CDefine.DEF_SMART_LOG );
				// 프로그램 실행 중.
				if( ProcessList.Length > 0 ) break;

				// SVI 프로그램 재실행
				Process SVIProcess = new Process();
				SVIProcess.StartInfo.FileName = CDefine.DEF_SMART_LOG + ".exe";
				SVIProcess.StartInfo.WorkingDirectory = m_objConfig.GetCurrentPath();
				SVIProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
				SVIProcess.Start();

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 스마트로그 종료
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetCloseSmartLog()
		{
			do {
				// 이거 말고 다른 Process 방식으로 종료하면 트레이 아이콘이 제대로 종료가 안되더라.
				IntPtr hWindow = FindWindow( null, CDefine.DEF_LOG_PROGRAM );
				if( hWindow == IntPtr.Zero ) break;
				try {
					SendMessage( hWindow, CDefine.WM_DESTROY, ( IntPtr )0, ( IntPtr )0 );
				}
				catch( Exception ex ) {
					SetUpdateLog( CDefine.enumLogType.LOG_ETC, ex.Message + " -> " + ex.StackTrace );
				}
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그메세지 전송
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetUpdateLog( CDefine.structureLog objLog )
		{
			IntPtr hWindow = FindWindow( null, CDefine.DEF_LOG_PROGRAM );
			if( IntPtr.Zero != hWindow ) {
				CDefine.COPYDATA_STRUCT objCopyData = new CDefine.COPYDATA_STRUCT();
				try {
					//구조체에 사이즈 할당 및 메모리 확보
					objCopyData.cbData = System.Runtime.InteropServices.Marshal.SizeOf( objLog );
					objCopyData.dwData = ( IntPtr )0;
					objCopyData.lpData = System.Runtime.InteropServices.Marshal.AllocCoTaskMem( objCopyData.cbData );

					//구조체 메모리 포인터 변환
					System.Runtime.InteropServices.Marshal.StructureToPtr( objLog, objCopyData.lpData, true );

					//구조체 포인터 할당
					IntPtr pSend = System.Runtime.InteropServices.Marshal.AllocCoTaskMem( System.Runtime.InteropServices.Marshal.SizeOf( objCopyData ) );
					System.Runtime.InteropServices.Marshal.StructureToPtr( objCopyData, pSend, false );

					//메세지 전송
					SendMessage( hWindow, CDefine.WM_COPYDATA, ( IntPtr )0, pSend );

					//메모리 해제
					System.Runtime.InteropServices.Marshal.FreeCoTaskMem( pSend );
					System.Runtime.InteropServices.Marshal.FreeCoTaskMem( objCopyData.lpData );
				}
				catch( System.Exception ex ) {
					Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
				}
			}
		}

        struct structureLog
        {
            public CDefine.enumLogType eLogType;
            public string strLogMessage;
            public bool bMainDisplayUpdate;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 로그메세지 전송
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateLog(CDefine.enumLogType eLogType, string strLogMessage, bool bMainDisplayUpdate = true)
        {
            structureLog objLog = new structureLog();
            objLog.eLogType = eLogType;
            objLog.strLogMessage = strLogMessage;
            objLog.bMainDisplayUpdate = bMainDisplayUpdate;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendUpdateLog), objLog);
        }

        public void SendUpdateLog(object obj)
        {
            structureLog objUpdateLog = (structureLog)obj;
            CDefine.structureLog objLog = new CDefine.structureLog();
            objLog.iLogType = (int)objUpdateLog.eLogType;
            objLog.strData = objUpdateLog.strLogMessage;

            IntPtr hWindow = FindWindow(null, CDefine.DEF_LOG_PROGRAM);
            if (IntPtr.Zero != hWindow)
            {
                CDefine.COPYDATA_STRUCT objCopyData = new CDefine.COPYDATA_STRUCT();

                try
                {
                    //구조체에 사이즈 할당 및 메모리 확보
                    objCopyData.cbData = System.Runtime.InteropServices.Marshal.SizeOf(objLog);
                    objCopyData.dwData = (IntPtr)0;
                    objCopyData.lpData = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(objCopyData.cbData);

                    //구조체 메모리 포인터 변환
                    System.Runtime.InteropServices.Marshal.StructureToPtr(objLog, objCopyData.lpData, true);

                    //구조체 포인터 할당
                    IntPtr pSend = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(objCopyData));
                    System.Runtime.InteropServices.Marshal.StructureToPtr(objCopyData, pSend, false);

                    //메세지 전송
                    SendMessage(hWindow, CDefine.WM_COPYDATA, (IntPtr)0, pSend);

                    //메모리 해제
                    System.Runtime.InteropServices.Marshal.FreeCoTaskMem(pSend);
                    System.Runtime.InteropServices.Marshal.FreeCoTaskMem(objCopyData.lpData);

                }
                catch (System.Exception ex)
                {
                    Trace.WriteLine(ex.Message + " -> " + ex.StackTrace);
                }
            }
            string strTime = string.Format("{0:HH:mm:ss:fff}", DateTime.Now);

            Form objMain = null;

            do
            {
                int iIndex = -1;
                for (int iLoopCount = 0; iLoopCount < (int)CDefine.enumCamera.CAMERA_FINAL; iLoopCount++)
                {
                    string strData = string.Format("_{0}", iLoopCount);
                    if (0 < objUpdateLog.eLogType.ToString().IndexOf(strData))
                    {
                        iIndex = iLoopCount;
                        break;
                    }
                }

                if (-1 == iIndex) break;

                objMain = GetFormMain();
                if (null == objMain) break;
                if (false == objUpdateLog.bMainDisplayUpdate) break;
                // 타입에 맞는 메인 폼 찾아서 로그 호출
                if (CDefine.enumMachineType.PROCESS_60 == m_objConfig.GetSystemParameter().eMachineType)
                {
                    CFormMainProcess60 objMainType = objMain as CFormMainProcess60;
                    if (null != objMainType)
                    {
                        objMainType.SetUpdateLog(iIndex, "[" + strTime + "]" + objUpdateLog.strLogMessage);
                    }
                }
                else if (CDefine.enumMachineType.PROCESS_110 == m_objConfig.GetSystemParameter().eMachineType)
                {
                    CFormMainProcess110 objMainType = objMain as CFormMainProcess110;
                    if (null != objMainType)
                    {
                        objMainType.SetUpdateLog(iIndex, "[" + strTime + "]" + objUpdateLog.strLogMessage);
                    }
                }
                else if (CDefine.enumMachineType.PROCESS_150 == m_objConfig.GetSystemParameter().eMachineType)
                {
                    if( CDefine.enumCameraType.CAMERA_AREA == m_objConfig.GetSystemParameter().eCameraType ) {
                        CFormMainProcess150 objMainType = objMain as CFormMainProcess150;
                        if( null != objMainType ) {
                            objMainType.SetUpdateLog( iIndex, "[" + strTime + "]" + objUpdateLog.strLogMessage );
                        }
                    } else {
                        CFormMainProcess150Gocator objMainType = objMain as CFormMainProcess150Gocator;
                        if( null != objMainType ) {
                            objMainType.SetUpdateLog( iIndex, "[" + strTime + "]" + objUpdateLog.strLogMessage );
                        }
                    }


                }
            } while (false);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 얼라인 로그 메인 화면에 뿌려줌
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetUpdateAlignLog( int iCameraIndex, int iProductIndex, double dX, double dY, double dT )
		{
			Form objMain = null;

			do {
				objMain = GetFormMain();
				if( null == objMain ) break;
				// 타입에 맞는 메인 폼 찾아서 로그 호출
				if( CDefine.enumMachineType.PROCESS_60 == m_objConfig.GetSystemParameter().eMachineType ) {
					CFormMainProcess60 objMainType = objMain as CFormMainProcess60;
					if( null != objMainType ) {
						objMainType.SetUpdateAlignLog( iCameraIndex, iProductIndex, dX, dY, dT );
					}
				}
				else if( CDefine.enumMachineType.PROCESS_110 == m_objConfig.GetSystemParameter().eMachineType ) {
                    CFormMainProcess110 objMainType = objMain as CFormMainProcess110;
                    if (null != objMainType)
                    {
                        objMainType.SetUpdateAlignLog(iCameraIndex, iProductIndex, dX, dY, dT);
                    }
                }
				else if( CDefine.enumMachineType.PROCESS_150 == m_objConfig.GetSystemParameter().eMachineType ) {
                    CFormMainProcess150 objMainType = objMain as CFormMainProcess150;
                    if (null != objMainType)
                    {
                        objMainType.SetUpdateAlignLog(iCameraIndex, iProductIndex, dX, dY, dT);
                    }
                }

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그메세지 전송
		//설명 : Button Operation 전용
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetUpdateButtonLog( System.Windows.Forms.Form objForm, string strButton )
		{
			SetUpdateLog( CDefine.enumLogType.LOG_BUTTON_OPERATION, string.Format( "[{0}] [{1}] {2}", GetUserInformation().m_strID, objForm.Name, strButton ) );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : UI 텍스트 데이터 테이블에서 해당 ID에 해당하는 문자열을 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetDatabaseUIText( string strID, string strFormName )
		{
			string strReturn = "";
			CDefine.enumLanguage eLanguage = m_objConfig.GetSystemParameter().eLanguage;
			CManagerTable objManagerTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUIText;

			do {
				try {
					DataTable objDataTable = objManagerTable.HLGetDataTable();
					DataRow[] objDataRow = objDataTable.Select( string.Format( "{0} = '{1}' AND {2} = '{3}'",
						objManagerTable.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumInformationUIText.ID ], strID,
						objManagerTable.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumInformationUIText.FORM_NAME ], strFormName ) );

					// 언어에 따라서 변경
					if( CDefine.enumLanguage.LANGUAGE_KOREA == eLanguage ) {
						strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUIText.TEXT_KOREA ].ToString();
					}
					else if( CDefine.enumLanguage.LANGUAGE_CHINA == eLanguage ) {
						strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUIText.TEXT_CHINA ].ToString();
					}
					else if( CDefine.enumLanguage.LANGUAGE_ENGLISH == eLanguage ) {
						strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUIText.TEXT_ENGLISH ].ToString();
                    } else if( CDefine.enumLanguage.LANGUAGE_VIETNAM == eLanguage ) {
                        strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUIText.TEXT_VIETNAM ].ToString();
                    }
				}
				catch( Exception ex ) {
					Trace.WriteLine( strFormName + " - " + strID + "BUTTON" + ex.Message );
				}
			} while( false );

			return strReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 유저 메세지 데이터 테이블에서 해당 ID에 해당하는 문자열을 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetDatabaseUserMessage( int iID )
		{
			string strReturn = "";
			CDefine.enumLanguage eLanguage = m_objConfig.GetSystemParameter().eLanguage;
			CManagerTable objManagerTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUserMessage;

			do {
				try {
					DataTable objDataTable = objManagerTable.HLGetDataTable();
					DataRow[] objDataRow = objDataTable.Select( string.Format( "{0} = '{1}'", objManagerTable.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumInformationUserMessage.ID ], iID ) );
					// 언어에 따라서 변경
					if( CDefine.enumLanguage.LANGUAGE_KOREA == eLanguage ) {
						strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUserMessage.TEXT_KOREA ].ToString();
					}
					else if( CDefine.enumLanguage.LANGUAGE_CHINA == eLanguage ) {
						strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUserMessage.TEXT_CHINA ].ToString();
					}
					else if( CDefine.enumLanguage.LANGUAGE_ENGLISH == eLanguage ) {
						strReturn = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUserMessage.TEXT_ENGLISH ].ToString();
					}
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}
			} while( false );

			return strReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 해당 유저 아이디에 해당하는 유저 레코드를 뽑아냄
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CUserInformation GetLoginCheck( string strID, string strPassword )
		{
			CUserInformation objUserInformation = null;

			do {
				try {

				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}
			} while( false );

			return objUserInformation;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 현재 유저 정보 확인
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CUserInformation GetUserInformation()
		{
			return m_objUserInformation.Clone() as CUserInformation;
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 검사 인덱스
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int GetInspectionIndex()
        {
            return m_iInspectionIndex;
        }

        public void SetInspectionIndex( int iInspectionIndex)
        {
            m_iInspectionIndex = iInspectionIndex;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 사이즈 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetChangeDisplaySize( int iCameraIndex )
		{
			try {
				if( CDefine.enumMachineType.PROCESS_60 == m_objConfig.GetSystemParameter().eMachineType ) {
					CFormMainProcess60 objForm = GetFormMain() as CFormMainProcess60;
					objForm.SetChangeDisplaySize( iCameraIndex );
				}
				else if( CDefine.enumMachineType.PROCESS_110 == m_objConfig.GetSystemParameter().eMachineType ) {
                    CFormMainProcess110 objForm = GetFormMain() as CFormMainProcess110;
                    objForm.SetChangeDisplaySize(iCameraIndex);
                }
				else if( CDefine.enumMachineType.PROCESS_150 == m_objConfig.GetSystemParameter().eMachineType ) {
                    CFormMainProcess150 objForm = GetFormMain() as CFormMainProcess150;
                    objForm.SetChangeDisplaySize(iCameraIndex);
                }
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.Message );
			}
		}

		

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 디바이스 체크
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool GetCheckDevice()
		{
			bool bReturn = false;

			do {
				// PLC 연결 상태
				if( true != m_objProcessMain.m_objPLC.HLIsConnected() ) {
					break;
				}
				// 카메라 연결상태
				if( CDefine.enumMachineType.PROCESS_60 == m_objConfig.GetSystemParameter().eMachineType ) {
					int iLoopCameraCount;
					for( iLoopCameraCount = 0; iLoopCameraCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCameraCount++ ) {
						if( true != m_objProcessMain.m_objCamera[ iLoopCameraCount ].HLIsConnected() ) {
							break;
						}
					}
					if( iLoopCameraCount != ( int )CDefine.enumCamera.CAMERA_FINAL) break;
				}
				else if( CDefine.enumMachineType.PROCESS_110 == m_objConfig.GetSystemParameter().eMachineType ) {
                    int iLoopCameraCount;
                    for (iLoopCameraCount = 0; iLoopCameraCount < (int)CDefine.enumCamera.CAMERA_FINAL; iLoopCameraCount++)
                    {
                        if (true != m_objProcessMain.m_objCamera[iLoopCameraCount].HLIsConnected())
                        {
                            break;
                        }
                    }
                    if (iLoopCameraCount != (int)CDefine.enumCamera.CAMERA_FINAL) break;
                }
				else if( CDefine.enumMachineType.PROCESS_150 == m_objConfig.GetSystemParameter().eMachineType ) {
                    int iLoopCameraCount;
                    for (iLoopCameraCount = 0; iLoopCameraCount < (int)CDefine.enumCamera.CAMERA_FINAL; iLoopCameraCount++)
                    {
                        if (true != m_objProcessMain.m_objCamera[iLoopCameraCount].HLIsConnected())
                        {
                            break;
                        }
                    }
                    if (iLoopCameraCount != (int)CDefine.enumCamera.CAMERA_FINAL) break;
                }

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그인
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetLogin( CUserInformation objUserInformation )
		{
			m_objUserInformation = objUserInformation.Clone() as CUserInformation;
			// 현재 폼에서 유저 권한에 따른 버튼 상태 변경 델리게이트가 생성되어 있으면 호출
			//             System.Windows.Forms.Form objForm = GetMainFrame().GetCurrentForm();
			//             if( null != objForm ) {
			//                 if( null != objForm ) {
			//                     objForm.Invoke( objForm.m_delegateSetChangeButtonStatus, this, objForm.Controls );
			//                 }
			//             }
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그아웃
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetLogout()
		{
			m_objUserInformation = new CUserInformation();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Message Show (key)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public System.Windows.Forms.DialogResult SetMessage( CDefine.enumAlarmType eAlarmType, int iID )
		{
			CDefine.structureAlarmInformation objAlarmInformation = new CDefine.structureAlarmInformation();
			objAlarmInformation.eAlarmLevel = eAlarmType;
			objAlarmInformation.iAlarmCode = iID;
			objAlarmInformation.strAlarmObject = "";
			objAlarmInformation.strAlarmFunction = "";
			objAlarmInformation.strAlarmDescription = "";
			CDialogMessage objMessage = new CDialogMessage( objAlarmInformation );
			return objMessage.ShowDialog();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Message Show (string)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public System.Windows.Forms.DialogResult SetMessage( CDefine.enumAlarmType eAlarmType, string strMessage )
		{
			CDefine.structureAlarmInformation objAlarmInformation = new CDefine.structureAlarmInformation();
			objAlarmInformation.eAlarmLevel = eAlarmType;
			objAlarmInformation.iAlarmCode = 0;
			objAlarmInformation.strAlarmObject = "";
			objAlarmInformation.strAlarmFunction = "";
			objAlarmInformation.strAlarmDescription = strMessage;
			CDialogMessage objMessage = new CDialogMessage( objAlarmInformation );
			return objMessage.ShowDialog();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 팝업 메뉴 언어 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetPopupMenuChange( ToolStripItem obj, string strText )
		{
			if( obj.Text != strText ) {
				obj.Text = strText;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 코그 디스플레이 팝업 메뉴 언어 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SetCogDisplayPopupMenuChangeLanguage( Cognex.VisionPro.Display.CogDisplay objCogDisplay )
		{
			bool bReturn = false;

			do {
				var pDocument = CDocument.GetDocument;
				CDefine.enumLanguage eLanguage = pDocument.m_objConfig.GetSystemParameter().eLanguage;

				ContextMenuStrip objMenuStrip = objCogDisplay.ContextMenuStrip;
				for( int iLoopMenuCount = 0; iLoopMenuCount < objMenuStrip.Items.Count; iLoopMenuCount++ ) {
					string strText = string.Format( "PopupMenu.Items[ {0} ]", iLoopMenuCount );
					SetPopupMenuChange( objMenuStrip.Items[ iLoopMenuCount ], GetDatabaseUIText( strText, "CFormDisplay" ) );
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}
	}
}