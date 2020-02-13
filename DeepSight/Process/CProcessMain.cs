using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using HLDevice;
using HLDevice.Abstract;
using System.Diagnostics;
using HLDevice.VisionLibrary;

namespace DeepSight
{
	public class CProcessMain : CProcessAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 로그 프로그램 감시
		private CProcessMonitorLog m_objProcessMonitorLog;
		// 테스트 프로그램
		private CProcessSimulationTrigger m_objProcessSimulationTrigger;
		// 이미지 삭제 스레드
		private Thread m_ThreadDeleteImage;
		// 가비지 수집 스레드
		private Thread m_ThreadGarbageCollect;
		// 하트 비트 스레드
		private Thread m_ThreadHeartbeat;
        // PLC요청 로그남기기 스레드
        private Thread m_ThreadPLCDataWriteLog1;
        private Thread m_ThreadPLCDataWriteLog2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public property
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 카메라 객체
        public CDeviceCamera[] m_objCamera;
		// 비전 라이브러리 얼라인
		public List<CVisionLibrary> m_objVisionLibraryPMAlign;
		// 검사 프로세스
		public CProcessVision m_objProcessVision;
        // 비디
        // 리스트로 만들어야하나 어쩌나...일단 하나만쓴다고하니 일단 1개로 감
        public List<CVidiClass> m_objVidi;
		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CProcessMain()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소멸자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		~CProcessMain()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool Initialize()
		{
			bool bReturn = false;

			do {
				var pDocument = CDocument.GetDocument;
				// PLC 생성 & 초기화
				{
                    // 프로그래스 바 : PLC Initialize Start.
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain Initialize Start" );
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "PLC Initialize Start", TypeOfMessage.Warning );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain - PLC Initialize" );
                    CConfig.CPLCInitializeParameter objConfigPLCParameter = pDocument.m_objConfig.GetPLCParameter();
					CDevicePLCAbstract.CInitializeParameter objPLCInitializeParameter;
					// PLC 파라미터 정보 설정
					objPLCInitializeParameter = new CDevicePLCAbstract.CInitializeParameter();
					objPLCInitializeParameter.iSocketPortNumber = objConfigPLCParameter.iSocketPortNumber;
					objPLCInitializeParameter.strSocketIPAddress = objConfigPLCParameter.strSocketIPAddress;
					objPLCInitializeParameter.ePLCProtocolType = ( CDevicePLCAbstract.CInitializeParameter.enumPLCProtocolType )objConfigPLCParameter.ePLCProtocolType;
                    objPLCInitializeParameter.ePLCType = (CDevicePLCAbstract.CInitializeParameter.enumPLCType)objConfigPLCParameter.ePLCType;
                    objPLCInitializeParameter.dMultiple = objConfigPLCParameter.dMultiple;
					// PLC 맵 데이터 로딩
					foreach( KeyValuePair<string, CConfig.CPLCInitializeParameter.CPLCParameter> pair in objConfigPLCParameter.objPLCParameter ) {
						CDevicePLCAbstract.CPLCParameter objPLCParameter = new CDevicePLCAbstract.CPLCParameter();
						objPLCParameter.strIndex = pair.Value.strIndex;
						objPLCParameter.eCommunicationType = ( CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType )pair.Value.eCommunicationType;
						objPLCParameter.strAddress = pair.Value.strAddress;
						objPLCParameter.strPLCName = pair.Value.strPLCName;
						objPLCParameter.iInOutIndex = pair.Value.iInOutIndex;
						objPLCInitializeParameter.objPLCParameter.Add( objPLCParameter.strPLCName, objPLCParameter );
					}
                    objPLCInitializeParameter.iInputCountAll = objConfigPLCParameter.iInputCountAll;
                    objPLCInitializeParameter.iOutputCountAll = objConfigPLCParameter.iOutputCountAll;
                    objPLCInitializeParameter.iInputCountBit = objConfigPLCParameter.iInputCountBit;
                    objPLCInitializeParameter.iOutputCountBit = objConfigPLCParameter.iOutputCountBit;
                    objPLCInitializeParameter.iInputCountWord = objConfigPLCParameter.iInputCountWord;
                    objPLCInitializeParameter.iOutputCountWord = objConfigPLCParameter.iOutputCountWord;
                    objPLCInitializeParameter.iInputCountDWord = objConfigPLCParameter.iInputCountDWord;
                    objPLCInitializeParameter.iOutputCountDWord = objConfigPLCParameter.iOutputCountDWord;
					
					if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationModePLC ) {
                        m_objPLC = new CDevicePLC( new HLDevice.PLC.CDevicePLCMCProtocol() );
                        //m_objPLC = new CDevicePLC(new HLDevice.PLC.CDevicePLCVirtual());
                    }
					else {
						m_objPLC = new CDevicePLC( new HLDevice.PLC.CDevicePLCVirtual() );
                        //m_objPLC = new CDevicePLC( new HLDevice.PLC.CDevicePLCMCProtocol() );
					}
					// PLC 객체 초기화
					if( false == m_objPLC.HLInitialize( objPLCInitializeParameter ) ) {
						pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "PLC Initialize Fail" );
						break;
					}
					// 프로그래스 바 : PLC Initialize Completed.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "PLC Initialize Completed", TypeOfMessage.Success );
				}
				// 조명 컨트롤러 생성 & 초기화
				{
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain - Light Controller Initialize" );
                    m_objLightController = new CDeviceLightController[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_FINAL ];
					for( int iLoopCount = 0; iLoopCount < m_objLightController.Length; iLoopCount++ ) {
						// 프로그래스 바 : LightController{0} Initialize Start.
						CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), string.Format( "LightController{0} Initialize Start.", iLoopCount ), TypeOfMessage.Warning );
						CConfig.CLightControllerParameter objConfigLightControllerParameter = pDocument.m_objConfig.GetLightControllerParameter( ( CDefine.enumLightController )iLoopCount );
						CDeviceLightControllerAbstract.CInitializeParameter objLightControllerInitializeParameter;
						// 조명 컨트롤러 파라미터 정보 설정
						objLightControllerInitializeParameter = new CDeviceLightControllerAbstract.CInitializeParameter();
						objLightControllerInitializeParameter.eType = ( CDeviceLightControllerAbstract.CInitializeParameter.enumType )objConfigLightControllerParameter.eType;
						objLightControllerInitializeParameter.strSerialPortName = objConfigLightControllerParameter.strSerialPortName;
						objLightControllerInitializeParameter.iSerialPortBaudrate = objConfigLightControllerParameter.iSerialPortBaudrate;
						objLightControllerInitializeParameter.iSerialPortDataBits = objConfigLightControllerParameter.iSerialPortDataBits;
						objLightControllerInitializeParameter.eParity = ( CDeviceLightControllerAbstract.CInitializeParameter.enumSerialPortParity )objConfigLightControllerParameter.eParity;
						objLightControllerInitializeParameter.eStopBits = ( CDeviceLightControllerAbstract.CInitializeParameter.enumSerialPortStopBits )objConfigLightControllerParameter.eStopBits;
						objLightControllerInitializeParameter.strSocketIPAddress = objConfigLightControllerParameter.strSocketIPAddress;
						objLightControllerInitializeParameter.iSocketPortNumber = objConfigLightControllerParameter.iSocketPortNumber;
						if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                            // 150공정은 고케이터로 붙을꺼같음 조명컨트롤러는 필요없음
                            if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType && CDefine.enumCameraType.CAMERA_3D == pDocument.m_objConfig.GetSystemParameter().eCameraType  )
							    m_objLightController[ iLoopCount ] = new CDeviceLightController( new HLDevice.LightController.CDeviceLightControllerVirtual() );
                            else
                                m_objLightController[ iLoopCount ] = new CDeviceLightController( new HLDevice.LightController.CDeviceLightControllerLVS() );
                        }
						else {
							m_objLightController[ iLoopCount ] = new CDeviceLightController( new HLDevice.LightController.CDeviceLightControllerVirtual() );
						}
						// 조명 컨트롤러 객체 초기화
						m_objLightController[ iLoopCount ].HLInitialize( objLightControllerInitializeParameter );
						// 프로그래스 바 : LightController{0} Initialize Completed.
						CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + iLoopCount, string.Format( "LightController{0} Initialize Completed.", iLoopCount ), TypeOfMessage.Success );
					}
					// 프로그래스 바 : All LightController Initialize Completed.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "All LightController Initialize Completed.", TypeOfMessage.Success );
				}

				// 카메라 생성 & 초기화
				{
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain - Camera Initialize" );
                    int iCameraCount = 0;
                    switch( pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                        case CDefine.enumMachineType.PROCESS_60:
                            iCameraCount = ( int )CDefine.enumCamera.CAMERA_FINAL;
                            break;
                        case CDefine.enumMachineType.PROCESS_110:
                            iCameraCount = (int)CDefine.enumCamera.CAMERA_FINAL;
                            break;
                        case CDefine.enumMachineType.PROCESS_150:
                            iCameraCount = (int)CDefine.enumCamera.CAMERA_FINAL;
                            break;
                    }

					m_objCamera = new CDeviceCamera[ iCameraCount ];
					for( int iLoopCount = 0; iLoopCount < m_objCamera.Length; iLoopCount++ ) {
						// 프로그래스 바 : Camera{0} Initialize Start.
						CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), string.Format( "Camera{0} Initialize Start.", iLoopCount ), TypeOfMessage.Warning );
						// 공통 카메라 데이터 설정
						CConfig.CCameraParameter objCameraParameter = pDocument.m_objConfig.GetCameraParameter( iLoopCount );
						HLDevice.Abstract.CDeviceCameraAbstract.CInitializeParameter objCameraInitializeParameter = new HLDevice.Abstract.CDeviceCameraAbstract.CInitializeParameter();
						objCameraInitializeParameter.eUseCameraType = ( HLDevice.Abstract.CDeviceCameraAbstract.CInitializeParameter.enumUseCameraType )objCameraParameter.eUseCameraType;
						objCameraInitializeParameter.iIndex = objCameraParameter.iCameraIndex;
						objCameraInitializeParameter.strCameraSerialNumber = objCameraParameter.strCameraSerialNumber;
						objCameraInitializeParameter.dResolution = objCameraParameter.dResolution;
                        objCameraInitializeParameter.str3DCameraIP = objCameraParameter.strIPAddress;
                        // 레시피 카메라 데이터 설정
                        CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( iLoopCount );
						objCameraInitializeParameter.objCameraConfig.bReverseX = objRecipeParameter.objCameraConfig.bReverseX;
						objCameraInitializeParameter.objCameraConfig.bReverseY = objRecipeParameter.objCameraConfig.bReverseY;
						objCameraInitializeParameter.objCameraConfig.bRotation90 = objRecipeParameter.objCameraConfig.bRotation90;
						objCameraInitializeParameter.objCameraConfig.bRotation180 = objRecipeParameter.objCameraConfig.bRotation180;
						objCameraInitializeParameter.objCameraConfig.bRotation270 = objRecipeParameter.objCameraConfig.bRotation270;
						objCameraInitializeParameter.objCameraConfig.dGain = objRecipeParameter.objCameraConfig.dGain;
						objCameraInitializeParameter.objCameraConfig.dExposureTime = objRecipeParameter.objCameraConfig.dExposureTime;
						objCameraInitializeParameter.objCameraConfig.iCameraWidth = objRecipeParameter.objCameraConfig.iCameraWidth;
						objCameraInitializeParameter.objCameraConfig.iCameraHeight = objRecipeParameter.objCameraConfig.iCameraHeight;
						objCameraInitializeParameter.objCameraConfig.iCameraXOffset = objRecipeParameter.objCameraConfig.iCameraXOffset;
						objCameraInitializeParameter.objCameraConfig.iCameraYOffset = objRecipeParameter.objCameraConfig.iCameraYOffset;
						objCameraInitializeParameter.objCameraConfig.dFrameRate = objRecipeParameter.objCameraConfig.dFrameRate;
						// 카메라 초기화
						if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                            if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType && CDefine.enumCameraType.CAMERA_3D == pDocument.m_objConfig.GetSystemParameter().eCameraType )
                                // 3D센서로 붙을때 
                                m_objCamera[ iLoopCount ] = new CDeviceCamera( new HLDevice.Camera.CDeviceCameraGocator() );
                            else
                                m_objCamera[ iLoopCount ] = new CDeviceCamera( new HLDevice.Camera.CDeviceCameraBaslse() );
                            
                        }
						else {
                            if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType && CDefine.enumCameraType.CAMERA_3D == pDocument.m_objConfig.GetSystemParameter().eCameraType )
                                m_objCamera[ iLoopCount ] = new CDeviceCamera( new HLDevice.Camera.CDeviceCameraVirtualGocator() );
                            else
                                m_objCamera[ iLoopCount ] = new CDeviceCamera( new HLDevice.Camera.CDeviceCameraVirtual() );
						}
						m_objCamera[ iLoopCount ].HLInitialize( objCameraInitializeParameter );
						// 프로그래스 바 : Camera{0} Initialize Completed.
						CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + iLoopCount, string.Format( "Camera{0} Initialize Completed.", iLoopCount ), TypeOfMessage.Success );
					}
					// 프로그래스 바 : All Camera Initialize Completed.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 13, "All Camera Initialize Completed.", TypeOfMessage.Success );
				}
				// 비전 라이브러리 생성 & 초기화
				{
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain - Vision Library Initialize" );
                    // 레시피 경로
                    string strRecipePath = pDocument.m_objConfig.GetRecipePath();
					// 레시피 이름
					string strRecipeName = pDocument.m_objConfig.GetSystemParameter().strRecipeID;

                    m_objVisionLibraryPMAlign = new List<CVisionLibrary>();
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopCount++ )
                    {
                        CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 1, string.Format( "Vision Library {0} Initialize Start.", iLoopCount ), TypeOfMessage.Warning );
                        m_objVisionLibraryPMAlign.Add( new CVisionLibrary( new CVisionLibraryCogPMAlign() ) );

                        CVisionLibraryAbstract.CInitializeParameter objInit = new CVisionLibraryAbstract.CInitializeParameter();
                        objInit.iIndex = iLoopCount;    objInit.strRecipeName = strRecipeName;  objInit.strRecipePath = strRecipePath;
                        m_objVisionLibraryPMAlign[ iLoopCount ].HLInitialize( objInit );
                        m_objVisionLibraryPMAlign[ iLoopCount ].SetIdle();
                        CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + iLoopCount, string.Format( "Vision Library {0} Initialize Completed.", iLoopCount ), TypeOfMessage.Success );
                    }
					// 프로그래스 바 : All Vision Library Align Initialize Completed.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 1, "All Vision Library Initialize Completed.", TypeOfMessage.Success );
				}

                // 비디 생성
                {
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain - VIDI Library Initialize" );
                    // 비디 워크스페이스 경로
                    string strWorkSpaceFilePath = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 ).strVidiWorkSpaceFilePath;
                    // 비디 워크스페이스 이름
                    string strWorkSpaceName = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 ).strVidiWorkSpaceName;
                    // 검색영역수만큼 로딩함, 택타임이 너무 오래걸림

                    //200205.YJ.수정 - 실제론 하나만 사용 : 동일 Workspace 여러개 동시에 사용 불가
                    m_objVidi = new List<CVidiClass>();
                    for( int iLoopCount = 0; iLoopCount < 1/*CDefine.DEF_MAX_COUNT_CROP_REGION*/; iLoopCount++ )
                    {
                        CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), string.Format( "Vision Library VIDI Initialize Start." ), TypeOfMessage.Warning );
                        CVidiClass obj = new CVidiClass();
                        obj.Initialize( strWorkSpaceFilePath, strWorkSpaceName, "" );
                        m_objVidi.Add( obj );
                        CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + iLoopCount, string.Format( "Vision Library VIDI Initialize Completed." ), TypeOfMessage.Success );
                    }
                }
           

				// 검사 프로세스 생성 & 초기화
				{
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessMain - ProcessVision Initialize" );
                    // 프로그래스 바 : CProcessVision Initialize Start.
                    CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "CProcessVision Initialize Start.", TypeOfMessage.Warning );
					m_objProcessVision = new CProcessVision();
					m_objProcessVision.Initialize();
					// 프로그래스 바 : CProcessVision Initialize Completed.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "CProcessVision Initialize Completed.", TypeOfMessage.Success );
				}
				// 레시피 정보 공유 메모리에 등록
				var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
				objMMFVisionData[ 0 ].strVisionRecipePPID = pDocument.m_objConfig.GetSystemParameter().strRecipeID;
				objMMFVisionData[ 0 ].strVisionRecipeName = pDocument.m_objConfig.GetRecipeInformation().strRecipeName;
				// 프로그래스 바 : Thread Initialize Start.
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "Thread Initialize Start.", TypeOfMessage.Warning );
				// 로그 프로그램 감시
				m_objProcessMonitorLog = new CProcessMonitorLog();
				m_objProcessMonitorLog.Initialize();
				// 테스트 프로그램
				m_objProcessSimulationTrigger = new CProcessSimulationTrigger();
				m_objProcessSimulationTrigger.Initialize();
				// 시퀀스 스레드
				m_ThreadProcess = new Thread( ThreadProcess );
				m_ThreadProcess.Start( this );
				// 이미지 삭제 스레드
				m_ThreadDeleteImage = new Thread( ThreadDeleteImage );
				m_ThreadDeleteImage.Start( this );
				// 가비지 수집 스레드
				m_ThreadGarbageCollect = new Thread( ThreadGarbageCollect );
				m_ThreadGarbageCollect.Start( this );
				// 하트 비트 스레드
				m_ThreadHeartbeat = new Thread( ThreadHeartbeat );
				m_ThreadHeartbeat.Start( this );

                m_ThreadPLCDataWriteLog1 = new Thread( ThreadPLCDataWriteLog1 );
                m_ThreadPLCDataWriteLog1.Start( this );
                m_ThreadPLCDataWriteLog2 = new Thread( ThreadPLCDataWriteLog2 );
                m_ThreadPLCDataWriteLog2.Start( this );

                SetCameraConfig();
				// 프로그래스 바 : Thread Initialize Completed.
				CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "Thread Initialize Completed.", TypeOfMessage.Success );

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
		public override void DeInitialize()
		{
			// 스레드 종료
			m_bThreadExit = true;
			// 로그 프로그램 감시
			m_objProcessMonitorLog.DeInitialize();
			// 테스프 프로그램
			m_objProcessSimulationTrigger.DeInitialize();
			// 시퀀스 스레드
			m_ThreadProcess.Join();
			// 이미지 삭제 스레드
			m_ThreadDeleteImage.Join();
			// 가비지 수집 스레드
			m_ThreadGarbageCollect.Join();
			// 하트비트 스레드
			m_ThreadHeartbeat.Join();
            // PLC데이터 로그 스레드
            m_ThreadPLCDataWriteLog1.Join();
            m_ThreadPLCDataWriteLog2.Join();

            // 검사 프로세스
            if( null != m_objProcessVision ) {
				m_objProcessVision.DeInitialize();
			}
			// 비전 라이브러리 얼라인
			for( int iLoopCount = 0; iLoopCount < m_objVisionLibraryPMAlign.Count; iLoopCount++ ) {
				if( null != m_objVisionLibraryPMAlign[ iLoopCount ] ) {
                    m_objVisionLibraryPMAlign[ iLoopCount ].HLDeInitialize();
				}
			}
            // 비디
            //이거하면 되나?
            for( int iLoopCount = 0; iLoopCount < m_objVidi.Count; iLoopCount++ )
            {
                m_objVidi[ iLoopCount ].VidiDispose();
            }
            m_objVidi.Clear();

            // 카메라 객체
            for ( int iLoopCount = 0; iLoopCount < m_objCamera.Length; iLoopCount++ ) {
				if( null != m_objCamera[ iLoopCount ] ) {
					m_objCamera[ iLoopCount ].HLDeInitialize();
				}
			}
          
			// IO 해제
			//m_objIO.HLDeInitialize();
			// PLC 해제
			m_objPLC.HLDeInitialize();
			// 조명 컨트롤러 해제
			for( int iLoopCount = 0; iLoopCount < m_objLightController.Length; iLoopCount++ ) {
				m_objLightController[ iLoopCount ].HLDeInitialize();
			}

           // CDocument.GetDocument.SetTrigger( 0, CDefine.enumTrigger.TRIGGER_ON );

		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 불러오기
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadRecipe()
		{
			bool bReturn = false;

			do {
				var pDocument = CDocument.GetDocument;
				// 레시피 경로
				string strRecipePath = pDocument.m_objConfig.GetRecipePath();
				// 레시피 이름
				string strRecipeName = pDocument.m_objConfig.GetSystemParameter().strRecipeID;

                // 레시피 파라미터 로드
                pDocument.m_objConfig.LoadRecipeParameter();
                // 카메라 파라미터 로드
                pDocument.m_objConfig.LoadCameraParameter();
                // 카메라 설정값 변경
                SetCameraConfig();


                // 라이브러리 레시피 불러오기
                {
                    for( int iLoopCount = 0; iLoopCount < pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 ).iCountInspectionPosition; iLoopCount++ ) {
                        if( null != m_objVisionLibraryPMAlign[ iLoopCount ] ) {
                            m_objVisionLibraryPMAlign[ iLoopCount ].HLLoadRecipe( strRecipePath, strRecipeName );
                        }
                    }
                }

                // 비디 워크스페이스 경로
                string strWorkSpaceFilePath = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 ).strVidiWorkSpaceFilePath;
                for( int iLoopCount = 0; iLoopCount < 1/*CDefine.DEF_MAX_COUNT_CROP_REGION*/; iLoopCount++ ) {
                    m_objVidi[ iLoopCount ].VidiFileLoad( strWorkSpaceFilePath );
                }


                // 레시피 정보 공유 메모리에 갱신
                var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
				objMMFVisionData[ 0 ].strVisionRecipePPID = pDocument.m_objConfig.GetSystemParameter().strRecipeID;
				objMMFVisionData[ 0 ].strVisionRecipeName = pDocument.m_objConfig.GetRecipeInformation().strRecipeName;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 설정
		//설명 : 레시피마다 변경해서 적용해야 함
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SetCameraConfig()
		{
			bool bReturn = false;

			do {
				var pDocument = CDocument.GetDocument;
				if( null == m_objCamera ) break;

				for( int iLoopCount = 0; iLoopCount < m_objCamera.Length; iLoopCount++ ) {
					CConfig.CCameraParameter objCameraParameter = pDocument.m_objConfig.GetCameraParameter( iLoopCount );
					CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( iLoopCount );
					// 실 카메라 타입인 경우
					if( CConfig.CCameraParameter.enumUseCameraType.USE_REAL_CAMERA_TYPE == objCameraParameter.eUseCameraType ) {
						m_objCamera[ iLoopCount ].HLSetReverseX( objRecipeParameter.objCameraConfig.bReverseX );
						m_objCamera[ iLoopCount ].HLSetReverseY( objRecipeParameter.objCameraConfig.bReverseY );
						m_objCamera[ iLoopCount ].HLSetGain( objRecipeParameter.objCameraConfig.dGain );
						m_objCamera[ iLoopCount ].HLSetExposureTime( objRecipeParameter.objCameraConfig.dExposureTime );
                        // 						m_objCamera[ iLoopCount ].HLSetWidth( objRecipeParameter.objCameraConfig.iCameraWidth );
                        // 						m_objCamera[ iLoopCount ].HLSetHeight( objRecipeParameter.objCameraConfig.iCameraHeight );
                        // 						m_objCamera[ iLoopCount ].HLSetXOffset( objRecipeParameter.objCameraConfig.iCameraXOffset );
                        // 						m_objCamera[ iLoopCount ].HLSetYOffset( objRecipeParameter.objCameraConfig.iCameraYOffset );

                        m_objCamera[ iLoopCount ].HLSetOffsetCenter( true, true );
                        m_objCamera[ iLoopCount ].HLSetFrameRate( objRecipeParameter.objCameraConfig.dFrameRate );
						m_objCamera[ iLoopCount ].HLSetPacketSize( CDefine.DEF_DEFAULT_PACKET_SIZE );
						m_objCamera[ iLoopCount ].HLSetHeartBeatTimeOut( CDefine.DEF_DEFAULT_HEARTBEAT_TIMEOUT );
                        // 카메라 라인셀렉터( 이프로젝트 처음사용, 트리거들어갈때 아웃으로 조명컨트롤 하는듯 )
                        m_objCamera[ iLoopCount ].HLSetLineSelect( CDeviceCameraAbstract.enumLineSelector.LINE2 );
                        m_objCamera[ iLoopCount ].HLSetLineSource( CDeviceCameraAbstract.enumLineSource.EXPOSUREACTIVE );
                    }
					// 공통 적용
					m_objCamera[ iLoopCount ].HLSetRotation90( objRecipeParameter.objCameraConfig.bRotation90 );
					m_objCamera[ iLoopCount ].HLSetRotation180( objRecipeParameter.objCameraConfig.bRotation180 );
					m_objCamera[ iLoopCount ].HLSetRotation270( objRecipeParameter.objCameraConfig.bRotation270 );

                    
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 시퀀스 스레드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static void ThreadProcess( object state )
		{
			CProcessMain pThis = ( CProcessMain )state;
			var pDocument = CDocument.GetDocument;
			var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
			string strRecipeLog = "";
			
			while( false == pThis.m_bThreadExit ) {
				// 현재 레시피 아이디를 PLC에 적는다
				string strPLCRecipeIDAddress = "";
				CDefine.enumMachineType eMachineType = pDocument.m_objConfig.GetSystemParameter().eMachineType;


                strPLCRecipeIDAddress = CDefine.enumPLCInputIndex.PLC_MODEL_CHANGE.ToString();
				short sCurrentRecipeID = 0;
                short[] pReadDwordData = new short[1];
                sCurrentRecipeID = ( short.Parse( pDocument.m_objConfig.GetSystemParameter().strRecipeID ) );
                pThis.m_objPLC.HLReadWordFromPLC( strPLCRecipeIDAddress, pReadDwordData.Length, ref pReadDwordData );


				// 스타트 모드일때만 자동으로 레시피 생성하자
				if( true == pDocument.m_objConfig.GetSystemParameter().bUseAutoRecipeChange ) {
					
					objMMFVisionData[ 0 ].strMachineRecipeID = pReadDwordData[ 0 ].ToString();
					//공유메모리 첫번째 페이지만 사용
					string strMachineRecipeID = objMMFVisionData[ 0 ].strMachineRecipeID;
					string strMachineRecipeName = objMMFVisionData[ 0 ].strMachineRecipeName;
					string strVisionRecipePPID = pDocument.m_objConfig.GetSystemParameter().strRecipeID;

                    if( "" != strMachineRecipeID ) {
						// 현재 저장되어 있는 레시피 아이디가 존재할 경우
						if( true == pDocument.m_objRecipe.GetRecipeIDOverlap( strMachineRecipeID ) ) {
							// 현재 모델과 제어프로그램 모델과 다를 경우 로딩
							if( strMachineRecipeID != strVisionRecipePPID ) {
                                // 레시피를 로드 중입니다…
                                pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10111 ) );

                                // jht recipe id change [ strVisionRecipePPID ] -> [ strMachineRecipeID ] start
                                strRecipeLog = string.Format( "RECIPE ID CHANGE [ {0} ] -> [ {1} ] START", strVisionRecipePPID, strMachineRecipeID );
								pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, strRecipeLog );
								CConfig.CSystemParameter objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
								// 현재 적용된 레시피 아이디 변경. 
								objSystemParameter.strRecipeID = strMachineRecipeID;
								pDocument.m_objConfig.SaveSystemParameter( objSystemParameter );

                                
                                CConfig.CRecipeInformation objRecipeInformation = pDocument.m_objConfig.GetRecipeInformation();
                                objRecipeInformation.strRecipeID = strMachineRecipeID;
                                objRecipeInformation.strRecipeName = pDocument.m_objRecipe.GetRecipeInformationList( strMachineRecipeID ).strRecipeName;
                                pDocument.m_objConfig.SaveRecipeInformation( objRecipeInformation );

								// 레시피 파라미터 로드
								pDocument.m_objConfig.LoadRecipeParameter();
								// 카메라 파라미터 로드
								pDocument.m_objConfig.LoadCameraParameter();
								// 비전 라이브러리 로드
								pThis.LoadRecipe();
                                // 카메라 설정값 변경
                                pThis.SetCameraConfig();
								// jht recipe id change [ strVisionRecipePPID ] -> [ strMachineRecipeID ] complete
								strRecipeLog = string.Format( "RECIPE ID CHANGE [ {0} ] -> [ {1} ] COMPLETE", strVisionRecipePPID, strMachineRecipeID );
								pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, strRecipeLog );
                                pDocument.m_bRecipeChange = true;
                                pDocument.GetMainFrame().ShowWaitMessage( false, "" );
                            }
						}
						else {
							// jht recipe id change [ strVisionRecipePPID ] -> [ strMachineRecipeID ] start
                            pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10111 ) );
							strRecipeLog = string.Format( "RECIPE ID CHANGE [ {0} ] -> [ {1} ] START", strVisionRecipePPID, strMachineRecipeID );
							pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, strRecipeLog );
							// 현재 저장되어 있는 레시피 아이디가 존재 하지 않을 경우
							// 현재 로딩되어 있는 레시피를 복사한다
							string strExistFilePath = string.Format( @"{0}\{1}", pDocument.m_objConfig.GetRecipePath(), pDocument.m_objConfig.GetSystemParameter().strRecipeID );
							string strNewFilePath = string.Format( @"{0}\{1}", pDocument.m_objConfig.GetRecipePath(), strMachineRecipeID );
							// 폴더 복사
							pDocument.m_objRecipe.SetDirectoryCopy( strExistFilePath, strNewFilePath );
							pDocument.m_objRecipe.SetRecipeIDMatch( strMachineRecipeID, System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) );

							CConfig.CSystemParameter objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                            // 현재 적용된 레시피 아이디 변경. 
                            objSystemParameter.strRecipeID = strMachineRecipeID;
                            pDocument.m_objConfig.SaveSystemParameter( objSystemParameter );

                            CConfig.CRecipeInformation objRecipeInformation = pDocument.m_objConfig.GetRecipeInformation();
                            objRecipeInformation.strRecipeID = strMachineRecipeID;
                            objRecipeInformation.strRecipeName = System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" );
                            pDocument.m_objConfig.SaveRecipeInformation( objRecipeInformation );
                            // 레시피 파라미터 로드
                            pDocument.m_objConfig.LoadRecipeParameter();
                            // 카메라 파라미터 로드
                            pDocument.m_objConfig.LoadCameraParameter();
                   
                            // 비전 라이브러리 로드
                            pThis.LoadRecipe();
                            // 카메라 설정값 변경
                            pThis.SetCameraConfig();
							// jht recipe id change [ strVisionRecipePPID ] -> [ strMachineRecipeID ] complete
							strRecipeLog = string.Format( "RECIPE ID CHANGE [ {0} ] -> [ {1} ] COMPLETE", strVisionRecipePPID, strMachineRecipeID );
							pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, strRecipeLog );
                            pDocument.m_bRecipeChange = true;
                            pDocument.GetMainFrame().ShowWaitMessage( false, "" );
                        }
					}
				}

				Thread.Sleep( 1000 );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 삭제 스레드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static void ThreadDeleteImage( object state )
		{
			CProcessMain pThis = ( CProcessMain )state;
			// 2시간 기준으로 이미지 삭제 확인
			TimeSpan objTimeSpan = new TimeSpan( 2, 0, 0 );
			double dMilliseconds = objTimeSpan.TotalMilliseconds;
			// 스레드 2000 ms
			int iThreadPeriod = 2000;
			// 프로그램 처음 킨 시점에서 특정 기간 지난 이미지 삭제
			pThis.SetDeleteImage();

			while( false == pThis.m_bThreadExit ) {
				if( 0.0 >= dMilliseconds ) {
					// 특정 기간 지난 이미지 삭제
					pThis.SetDeleteImage();
					// 초기화
					dMilliseconds = objTimeSpan.TotalMilliseconds;
				}
				dMilliseconds -= ( double )iThreadPeriod;
				Thread.Sleep( iThreadPeriod );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 삭제
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetDeleteImage()
		{
			var pDocument = CDocument.GetDocument;

			do {
				// 폴더 없을 경우 생성 ( Image )
				if( false == Directory.Exists( pDocument.m_objConfig.GetSystemParameter().strImageSavePath ) ) {
					Directory.CreateDirectory( pDocument.m_objConfig.GetSystemParameter().strImageSavePath );
				}
				// 폴더 정보 가져오기.
				string[] strDayDirectors = Directory.GetDirectories( pDocument.m_objConfig.GetSystemParameter().strImageSavePath, "*", SearchOption.TopDirectoryOnly );
				// 이전 날짜 검색 ( 총 폴더 수 + 로그저장기간 + 50일 )
				int iSerchDay = strDayDirectors.Count() + pDocument.m_objConfig.GetSystemParameter().iPeriodImage + 50;
				for( int iLoopCount = pDocument.m_objConfig.GetSystemParameter().iPeriodImage; iLoopCount <= iSerchDay; ++iLoopCount ) {
					DateTime sysNewTime = DateTime.Now.AddDays( iLoopCount * -1 );
					string strPathName = string.Format( "{0}\\{1:D4}-{2:D2}-{3:D2}", pDocument.m_objConfig.GetSystemParameter().strImageSavePath, sysNewTime.Year, sysNewTime.Month, sysNewTime.Day );
					// 해당 폴더 유무 체크
					if( true == Directory.Exists( strPathName ) ) {
						// 예외 처리 c: or d: 통째로 날리면 안됨....
						if( @"C:\\" == strPathName.ToUpper() || @"C:" == strPathName.ToUpper()
							|| @"D:\\" == strPathName.ToUpper() || @"D:" == strPathName.ToUpper() ) {
							break;
						}
						DirectoryInfo objDir = new DirectoryInfo( strPathName );
						// 하위 폴더까지 삭제
						try {
							objDir.Delete( true );
						}
						catch( IOException ex ) {
							pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, ex.StackTrace );
						}
					}
				}
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 가비지 수집 스레드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static void ThreadGarbageCollect( object state )
		{
			CProcessMain pThis = ( CProcessMain )state;
			var pDocument = CDocument.GetDocument;
			CDefine.enumMachineType eMachineType = pDocument.m_objConfig.GetSystemParameter().eMachineType;

			while( false == pThis.m_bThreadExit ) {

                // 모든 스테이지가 레디 상태일 경우에만 가비지 컬렉터 실행

                bool bStatus = 1 == pThis.m_objPLC.HLGetInterfacePLC().sInterfacePlcWordIn[ ( int )CDefine.enumPCOutIndex.PC_BUSY ] ? true : true;

                if( true == bStatus || CDefine.enumRunMode.RUN_MODE_START != pDocument.GetRunMode() ) {
					GC.Collect();
					Thread.Sleep( 5000 );
				}
				Thread.Sleep( 500 );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 하트비트 쓰레드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static void ThreadHeartbeat( object state )
		{
			CProcessMain pThis = ( CProcessMain )state;

			while( false == pThis.m_bThreadExit ) {
				// Heart Beat는 공유메모리 첫번째 페이지만 사용
				var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
				objMMFVisionData[ ( int )0 ].bHeartBeat = !objMMFVisionData[ ( int )0 ].bHeartBeat;
                if( true == objMMFVisionData[ ( int )0 ].bHeartBeat ) {
                    pThis.m_objPLC.HLWriteWordFromPLC("PC_VISION_ALIVE", (short)0 );
                } else {
                    pThis.m_objPLC.HLWriteWordFromPLC("PC_VISION_ALIVE", (short)1 );
                }

				Thread.Sleep( 500 );

                
                //pThis.WaitPLCInterfaceValue( pThis.m_objPLC, "PLC_ALIVE_SIGNAL", 0.0, 1000, 50 );
			}
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 로그상태 확인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool IsPLCDataWriteLog( string strPlcAddress )
        {
            bool bReturn = false;
            do {
                short[] iReadData = new short[ 1 ];
                m_objPLC.HLReadWordFromPLC( strPlcAddress, iReadData.Length, ref iReadData );
                if( ( int )CDefine.enumTrigger.TRIGGER_OFF == iReadData[ 0 ] ) break;

                bReturn = true;
            } while( false );

            return bReturn;
        }

        private bool PLCDataWriteLogLVDT()
        {
            bool bReturn = false;
            do {
                string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
                string strID = "";
                m_objPLC.HLReadWordASCIIFromPLC( "PLC_LVDT_CELL_ID_1", 10, ref strID );
                if( "" == strID ) {
                    strID = System.DateTime.Now.ToString( "HH.mm.ss" );
                }

                string strImagePath = "D:\\PLC_LOG\\" + strToday + "\\" + "LVDT" + "\\";
                //폴더 존재 여부 체크
                if( false == Directory.Exists( strImagePath ) ) {
                    //폴더가 없다면 생성
                    Directory.CreateDirectory( strImagePath );
                }

                double[] dData = new double[ 1 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LVDT", dData.Length, ref dData );

                List<string> strResult = new List<string>();
                strResult.Add( "TIME," + strToday + "_" + System.DateTime.Now.ToString( "HH.mm.ss" ) );
                strResult.Add( "ID," + strID);
                for( int iLoopCount = 0; iLoopCount < dData.Length; iLoopCount++ ) {
                    string strData = string.Format( "{0},{1:F4}", iLoopCount + 1, dData[ iLoopCount ] / 10000 );
                    strResult.Add( strData );
                }

                string[] strWriteLog = strResult.ToArray();
                //SetDataToCsv( strImagePath + strID + ".csv", strWriteLog );

                strResult = new List<string>();
                string strTime = strToday + "_" + System.DateTime.Now.ToString( "HH.mm.ss" );
                for( int iLoopCount = 0; iLoopCount < dData.Length; iLoopCount++ ) {
                    string strData = string.Format( "{0:F4}", dData[ iLoopCount ] / 10000 );
                    strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "," + "]" + strData );
                }
                strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "LVDT.csv", strWriteLog );

                bReturn = true;
            } while( false );

            return bReturn;
        }


        private bool PLCDataWriteLogLeadFactoryA()
        {
            bool bReturn = false;
            do {
                string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
                string strID = "";


                m_objPLC.HLReadWordASCIIFromPLC( "PLC_LEAD_CELL_ID_A_1", 10, ref strID );
                if( "" == strID ) {
                    strID = System.DateTime.Now.ToString( "HH.mm.ss" );
                }

                string strImagePath = "D:\\PLC_LOG\\" + strToday + "\\" + "LEAD" + "\\";
                //폴더 존재 여부 체크
                if( false == Directory.Exists( strImagePath ) ) {
                    //폴더가 없다면 생성
                    Directory.CreateDirectory( strImagePath );
                }

                List<string> strResult = new List<string>();
                double[] dDataFrontTop = new double[ 24 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LEAD_A_1", dDataFrontTop.Length, ref dDataFrontTop );
                string strTime = strToday + "_" + System.DateTime.Now.ToString( "HH.mm.ss" );
                string strLeadDataFrontTop = "";
                for( int iLoopCount = 0; iLoopCount < dDataFrontTop.Length; iLoopCount++ ) {
                    strLeadDataFrontTop += string.Format( "{0:F3},", dDataFrontTop[ iLoopCount ] / 1000 );
                }
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strLeadDataFrontTop );
                string[] strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "LEAD_FRONT_TOP.csv", strWriteLog );


                double[] dDataFrontBottom = new double[ 24 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LEAD_A_25", dDataFrontBottom.Length, ref dDataFrontBottom );
                string strLeadDataFrontBottom = "";
                for( int iLoopCount = 0; iLoopCount < dDataFrontBottom.Length; iLoopCount++ ) {
                    strLeadDataFrontBottom += string.Format( "{0:F3},", dDataFrontBottom[ iLoopCount ] / 1000 );
                }
                strResult.Clear();
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strLeadDataFrontBottom );
                strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "LEAD_FRONT_BOTTOM.csv", strWriteLog );

                double[] dDataRearTop = new double[ 24 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LEAD_B_1", dDataRearTop.Length, ref dDataRearTop );
                string strLeadDataRearTop = "";
                for( int iLoopCount = 0; iLoopCount < dDataRearTop.Length; iLoopCount++ ) {
                    strLeadDataRearTop += string.Format( "{0:F3},", dDataRearTop[ iLoopCount ] / 1000 );
                }
                strResult.Clear();
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strLeadDataRearTop );
                strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "LEAD_REAR_TOP.csv", strWriteLog );


                double[] dDataRearBottom = new double[ 24 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LEAD_B_25", dDataRearBottom.Length, ref dDataRearBottom );
                string strLeadDataRearBottom = "";
                for( int iLoopCount = 0; iLoopCount < dDataRearBottom.Length; iLoopCount++ ) {
                    strLeadDataRearBottom += string.Format( "{0:F3},", dDataRearBottom[ iLoopCount ] / 1000 );
                }
                strResult.Clear();
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strLeadDataRearBottom );
                strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "LEAD_REAR_BOTTOM.csv", strWriteLog );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        private bool PLCDataWriteLogLeadFactoryB()
        {
            bool bReturn = false;
            do {
                string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
                string strID = "";


                m_objPLC.HLReadWordASCIIFromPLC( "PLC_LEAD_CELL_ID_A_1", 10, ref strID );
                if( "" == strID ) {
                    strID = System.DateTime.Now.ToString( "HH.mm.ss" );
                }

                string strImagePath = "D:\\PLC_LOG\\" + strToday + "\\" + "LEAD" + "\\";
                //폴더 존재 여부 체크
                if( false == Directory.Exists( strImagePath ) ) {
                    //폴더가 없다면 생성
                    Directory.CreateDirectory( strImagePath );
                }

                // 702공장은 데이터가 110개, 55개씩 끊어서 데이터를 남긴다. PLC에서 포지션 받아서 처리
                // 1일때는 TOP, 2일때 BOTTOM
                short[] iReadData = new short[ 1 ];
                m_objPLC.HLReadWordFromPLC( "PLC_WRITE_LOG_LEAD_POSITION", iReadData.Length, ref iReadData );
                int iPosition = ( int )iReadData[ 0 ];

                List<string> strResult = new List<string>();
                double[] dDataFrontTop = new double[ 55 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LEAD_A_1", dDataFrontTop.Length, ref dDataFrontTop );
                string strTime = strToday + "_" + System.DateTime.Now.ToString( "HH.mm.ss" );
                string strLeadDataFrontTop = "";
                for( int iLoopCount = 0; iLoopCount < dDataFrontTop.Length; iLoopCount++ ) {
                    strLeadDataFrontTop += string.Format( "{0:F3},", dDataFrontTop[ iLoopCount ] / 1000 );
                }
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strLeadDataFrontTop );
                string[] strWriteLog = strResult.ToArray();
                if( 1 == iPosition )
                    SetDataToCsv( strImagePath + "LEAD_FRONT_TOP.csv", strWriteLog );
                else
                    SetDataToCsv( strImagePath + "LEAD_FRONT_BOTTOM.csv", strWriteLog );

                double[] dDataRearTop = new double[ 55 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LEAD_B_1", dDataRearTop.Length, ref dDataRearTop );
                string strLeadDataRearTop = "";
                for( int iLoopCount = 0; iLoopCount < dDataRearTop.Length; iLoopCount++ ) {
                    strLeadDataRearTop += string.Format( "{0:F3},", dDataRearTop[ iLoopCount ] / 1000 );
                }
                strResult.Clear();
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strLeadDataRearTop );
                strWriteLog = strResult.ToArray();
                if( 1 == iPosition )
                    SetDataToCsv( strImagePath + "LEAD_REAR_TOP.csv", strWriteLog );
                else
                    SetDataToCsv( strImagePath + "LEAD_REAR_BOTTOM.csv", strWriteLog );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        private bool PLCDataWriteLogWeight()
        {
            bool bReturn = false;
            do {
                string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
                string strID = "";
                m_objPLC.HLReadWordASCIIFromPLC( "PLC_WEIGHT_CELL_ID_1", 10, ref strID );
                if( "" == strID ) {
                    strID = System.DateTime.Now.ToString( "HH.mm.ss" );
                }

                string strImagePath = "D:\\PLC_LOG\\" + strToday + "\\" + "WEIGHT" + "\\";
                //폴더 존재 여부 체크
                if( false == Directory.Exists( strImagePath ) ) {
                    //폴더가 없다면 생성
                    Directory.CreateDirectory( strImagePath );
                }

                double[] dData = new double[ 1 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_WEIGHT", dData.Length, ref dData );

                List<string> strResult = new List<string>();
                string strTime = strToday + "_" + System.DateTime.Now.ToString( "HH.mm.ss" );
                string strData ="";
                for( int iLoopCount = 0; iLoopCount < dData.Length; iLoopCount++ ) {
                    strData += string.Format( "{0:F4},", dData[ iLoopCount ] / 100 );
                }
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "," + "]" + strData );
                string[] strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "WEIGHT.csv", strWriteLog );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        private bool PLCDataWriteLogLVDT150()
        {
            bool bReturn = false;
            do {
                string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
                string strID = "";
                string strTime = strToday + "_" + System.DateTime.Now.ToString( "HH.mm.ss" );

                m_objPLC.HLReadWordASCIIFromPLC( "PLC_LVDT_CELL_ID_1", 10, ref strID );
                if( "" == strID ) {
                    strID = System.DateTime.Now.ToString( "HH.mm.ss" );
                }

                string strImagePath = "D:\\PLC_LOG\\" + strToday + "\\" + "LVDT" + "\\";
                //폴더 존재 여부 체크
                if( false == Directory.Exists( strImagePath ) ) {
                    //폴더가 없다면 생성
                    Directory.CreateDirectory( strImagePath );
                }

                double[] dData = new double[ 15 ];
                m_objPLC.HLReadDoubleWordFromPLC( "PLC_LOG_DATA_LVDT_1", dData.Length, ref dData );

                List<string> strResult = new List<string>();
                string strData = "";
                for( int iLoopCount = 0; iLoopCount < dData.Length; iLoopCount++ ) {
                    strData += string.Format( "{0:F4},", dData[ iLoopCount ] / 10000 );
                }
                strResult.Clear();
                strResult.Add( "[" + strTime + "]" + "," + "[" + strID + "]" + "," + strData );
                string[] strWriteLog = strResult.ToArray();
                SetDataToCsv( strImagePath + "LVDT.csv", strWriteLog );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 로그 데이터 남기기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadPLCDataWriteLog1( object state )
        {
            CProcessMain pThis = ( CProcessMain )state;
            CDefine.enumMachineType eMachineType = CDocument.GetDocument.m_objConfig.GetSystemParameter().eMachineType;
            while( false == pThis.m_bThreadExit ) {

                switch( eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        if( pThis.IsPLCDataWriteLog( "PLC_WRITE_LOG_LVDT" ) ) {
                            pThis.PLCDataWriteLogLVDT();
                            Thread.Sleep( 1000 );
                        }
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        if( pThis.IsPLCDataWriteLog( "PLC_WRITE_LOG_WEIGHT" ) ) {
                            pThis.PLCDataWriteLogWeight();
                            Thread.Sleep( 1000 );
                        }
                        break;
                }
                Thread.Sleep( 100 );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 로그 데이터 남기기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadPLCDataWriteLog2( object state )
        {
            CProcessMain pThis = ( CProcessMain )state;
            CDefine.enumMachineType eMachineType = CDocument.GetDocument.m_objConfig.GetSystemParameter().eMachineType;
            CDefine.enumFactoryType eFactoryType = CDocument.GetDocument.m_objConfig.GetSystemParameter().eFactoryType;

            while( false == pThis.m_bThreadExit ) {

                switch( eMachineType ) {
                    case CDefine.enumMachineType.PROCESS_60:
                        if( pThis.IsPLCDataWriteLog( "PLC_WRITE_LOG_LEAD" ) ) {
                            if( CDefine.enumFactoryType.FACTORY_A == eFactoryType )
                                pThis.PLCDataWriteLogLeadFactoryA();
                            else
                                pThis.PLCDataWriteLogLeadFactoryB();
                            Thread.Sleep( 1000 );
                        }
                        break;
                    case CDefine.enumMachineType.PROCESS_110:
                        break;
                    case CDefine.enumMachineType.PROCESS_150:
                        if( pThis.IsPLCDataWriteLog( "PLC_WRITE_LOG_LVDT" ) ) {
                            pThis.PLCDataWriteLogLVDT150();
                            Thread.Sleep( 1000 );
                        }
                        break;
                }
                Thread.Sleep( 100 );
            }
        }


    }
}