using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HLDevice;
using HLDevice.Camera;
using System.Drawing;
using System.Diagnostics;
using Cognex.VisionPro;
using OpenCvSharp;
using PMSProcessing;
using System.Runtime.InteropServices;
using Cognex.VisionPro.ImageProcessing;

namespace DeepSight {
    public class CProcessVisionProcess60 : CProcessAbstract {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //enum
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 유닛 명령
        public enum enumCommand {
            CMD_IDLE = 0, CMD_STOP,
            CMD_START_INSPECTION, CMD_LIVE_START, CMD_LIVE_END,
            CMD_START_GRAB, CMD_START_GRAB_MEASURE, CMD_START_PMS, CMD_START_PMS_MEASURE,
            CMD_ALIGN_RESULT,
            CMD_START_LIVE_INSPECTION,
            CMD_FINAL
        };

        // 유닛 상태
        public enum enumStatus {
            STS_UNKNOWN = 0, STS_ERROR, STS_STOP,
            STS_START_INSPECTION, STS_LIVE_START, STS_LIVE_END,
            STS_START_GRAB, STS_START_GRAB_MEASURE, STS_START_PMS, STS_START_PMS_MEASURE,
            STS_ALIGN_RESULT,
            STS_START_LIVE_INSPECTION,
            STS_FINAL
        };

        public enum enumEvent { EVENT_GRAB, EVENT_PMS, EVENT_FINAL };
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //private property
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 명령
        private enumCommand m_eCommand;
        // 상태
        private enumStatus m_eStatus;
        // 카메라 인덱스
        private int m_iCameraIndex;
        // 카메라 객체
        private CDeviceCamera m_refCamera;
        // 조명컨트롤 객체
        private CDeviceLightController m_refLight;
        // 비전 라이브러리 얼라인 객체
        private List<CVisionLibrary> m_refVisionLibrary;
        // 비디객체
        private List<CVidiClass> m_refVidi;
        // 비전 그랩 및 검사 완료 이벤트
        private EventWaitHandle[][] m_objWaitHandle;

        // 이미지 
        private CogImage8Grey m_objCogInputImage;
        private CogImage8Grey m_objCogOutputImage;
        // 그랩 성공 여부
        private bool m_bGrabComplete;
        // 카메라 이름 ( 로그 남기기 위함 )
        private string m_strCameraName;
        // 로그 타입
        // 라이브용 멤버
        private HLDevice.Abstract.CDeviceCameraAbstract.CImageData m_objImageData;

        // PMS
        private CPMSManager m_objPMSManager;
        // 검사
        private CMeasureManager m_objMeasureManager;

        private class CVidiParameter {
            public bool bBusy;
            public int iVidiIndex;
            public int iInspectionIndex;
            // VIDI Input
            public CogImage8Grey objInputImage;
            public Bitmap objInputImageBitmap;
            public string strVidiStreamName;
            public string strVidiToolName;
            public CDefine.enumVidiType eVidiType;

            // Vidi Output
            public Bitmap objResultImage;
            public Bitmap objResultOverlay;
            public double dScore;
            public string strTactTime;

            public CVidiParameter()
            {
                bBusy = false;
                iVidiIndex = 0;
                iInspectionIndex = 0;
                objInputImage = new CogImage8Grey();
                objInputImageBitmap = new Bitmap( 10, 10 );
                objResultImage = new Bitmap( 10, 10 );
                objResultOverlay = new Bitmap( 10, 10 );
                strVidiStreamName = "";
                strVidiToolName = "";
                eVidiType = CDefine.enumVidiType.RED;
                dScore = 0;
            }

            public void Init()
            {
                bBusy = false;
                iInspectionIndex = 0;
                objInputImage = new CogImage8Grey();
                objInputImageBitmap = new Bitmap( 10, 10 );
                objResultImage = new Bitmap( 10, 10 );
                objResultOverlay = new Bitmap( 10, 10 );
                strVidiStreamName = "";
                strVidiToolName = "";
                eVidiType = CDefine.enumVidiType.RED;
                dScore = 0;
            }
        }

        public struct structureDataRegionMeasure {
            public double dStartX;
            public double dStartY;
            public double dEndX;
            public double dEndY;
        }

        private class CMeasureParameter {
            public bool bBusy;
            public int iMeasureIndex;
            // Measure Input
            public CogImage8Grey objInputImage;
            public Bitmap objInputImageBitmap;
            public Bitmap objInputImageBitmapSecond;
            public double dWidth;
            public double dHeight;
            public structureDataRegionMeasure objResultRegion;
            // Measure Output
            public Bitmap objResultImage;
            public Bitmap objResultOverlay;
            public double dScore;
            public string strTactTime;

            public CMeasureParameter()
            {
                bBusy = false;
                iMeasureIndex = 0;
                objInputImage = new CogImage8Grey();
                objInputImageBitmap = new Bitmap( 10, 10 );
                objInputImageBitmapSecond = new Bitmap( 10, 10 );
                objResultImage = new Bitmap( 10, 10 );
                objResultOverlay = new Bitmap( 10, 10 );
                dWidth = 0;
                dHeight = 0;
                objResultRegion = new structureDataRegionMeasure();
                dScore = 0;
            }

            public void Init()
            {
                bBusy = false;
                objInputImage = new CogImage8Grey();
                objInputImageBitmap = new Bitmap( 10, 10 );
                objInputImageBitmapSecond = new Bitmap( 10, 10 );
                objResultImage = new Bitmap( 10, 10 );
                objResultOverlay = new Bitmap( 10, 10 );
                dWidth = 0;
                dHeight = 0;
                objResultRegion.dStartX = 0;
                objResultRegion.dStartY = 0;
                objResultRegion.dEndX = 0;
                objResultRegion.dEndY = 0;
                dScore = 0;
            }
        }

        private List<CVidiParameter> m_objVidiParameter;
        private List<CMeasureParameter> m_objMeasureParameter;
        private CDefine.enumInspectionType m_objGrabType;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CProcessVisionProcess60()
        {
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool Initialize()
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
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Initialize( int iCameraIndex )
        {
            bool bReturn = false;

            do {
                var pDocument = CDocument.GetDocument;


                // 인덱스 정의
                m_iCameraIndex = iCameraIndex;
                m_objGrabType = CDefine.enumInspectionType.TYPE_VIDI;
                m_strCameraName = ( ( CDefine.enumCamera )m_iCameraIndex ).ToString() + ", ";
                // 그랩 완료 이벤트
                m_objWaitHandle = new EventWaitHandle[ ( int )enumEvent.EVENT_FINAL ][];
                for( int iLoopCount = 0; iLoopCount < ( int )enumEvent.EVENT_FINAL; iLoopCount++ ) {
                    m_objWaitHandle[ iLoopCount ] = new EventWaitHandle[ 1 ];
                }
                m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ][ 0 ] = new EventWaitHandle( false, EventResetMode.ManualReset );
                m_objWaitHandle[ ( int )enumEvent.EVENT_PMS ][ 0 ] = new EventWaitHandle( false, EventResetMode.ManualReset );

                // 카메라 및 비전 라이브러리 참조 객체
                m_refCamera = pDocument.m_objProcessMain.m_objCamera[ m_iCameraIndex ];
                m_refVisionLibrary = pDocument.m_objProcessMain.m_objVisionLibraryPMAlign;
                m_refVidi = pDocument.m_objProcessMain.m_objVidi;
                m_refLight = pDocument.m_objProcessMain.m_objLightController[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ];

                m_objVidiParameter = new List<CVidiParameter>();
                for( int iLoopCount = 0; iLoopCount < ( CDefine.DEF_MAX_COUNT_CROP_REGION - 3 ); iLoopCount++ ) {
                    CVidiParameter obj = new CVidiParameter();
                    // 툴이 1개밖에 생성이 안됨. 다 0번으로 실행하자
                    obj.iVidiIndex = iLoopCount;
                    m_objVidiParameter.Add( obj );
                }


                m_objMeasureParameter = new List<CMeasureParameter>();
                for( int iLoopCount = 0; iLoopCount < ( CDefine.DEF_MAX_COUNT_CROP_REGION - 3 ); iLoopCount++ ) {
                    CMeasureParameter obj = new CMeasureParameter();
                    // 툴이 1개밖에 생성이 안됨. 다 0번으로 실행하자
                    obj.iMeasureIndex = 0;
                    m_objMeasureParameter.Add( obj );
                }

                // 라이브용 이미지 멤버
                m_objImageData = new HLDevice.Abstract.CDeviceCameraAbstract.CImageData();
                // 카메라 그랩 콜백
                m_refCamera.HLSetCallbackFunctionGrabImage( SetCallbackGrabImage );
                // 트레이스 메세지 콜백
                m_refCamera.HLSetCallbackTraceMessage( SetCallbackTraceMessage );
                // 그랩 에러 콜백
                m_refCamera.HLSetCallbackGrabError( SetCallbackGrabError );

                m_objPMSManager = new CPMSManager();
                m_objPMSManager.Initialize( m_iCameraIndex );
                m_objPMSManager.ProcessDoneEvent += M_objPMSManager_ProcessDoneEvent;

                m_objMeasureManager = new CMeasureManager();
                m_objMeasureManager.Initialize( m_iCameraIndex );

                // 스레드
                m_ThreadProcess = new Thread( ThreadProcess );
                m_ThreadProcess.Start( this );

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
            m_bThreadExit = true;
            m_ThreadProcess.Join();

            SetEvent( enumEvent.EVENT_GRAB );
            SetEvent( enumEvent.EVENT_PMS );

            m_objPMSManager.DeInitialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 커맨드 입력
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetCommand( enumCommand eCommand )
        {
            m_eCommand = eCommand;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 명령 반환
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public enumCommand GetCommand()
        {
            return m_eCommand;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 상태 리턴
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public enumStatus GetStatus()
        {
            return m_eStatus;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이벤트 활성화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetEvent( enumEvent eEvent )
        {
            m_objWaitHandle[ ( int )eEvent ][ 0 ].Set();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이벤트 비활성화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ResetEvent( enumEvent eEvent )
        {
            m_objWaitHandle[ ( int )eEvent ][ 0 ].Reset();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 트레이스 메세지 콜백
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetCallbackTraceMessage( string strMessage )
        {
            // 바슬러 내부에서 Trace 에러나는 부분 스마트 로그로 전송
            // 쓸데 없는 정보 너무 많음
            //var pDocument = CDocument.GetDocument;
            //pDocument.SetUpdateLog( m_eLogType, m_eStageCameraIndex.ToString() + " " + strMessage );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그랩 에러 콜백
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetCallbackGrabError()
        {
            // 그랩 에러인 경우에 라이브 온 상태면 라이브 오프 상태로 전환해줌
            if( enumStatus.STS_LIVE_START == m_eStatus ) {
                m_eCommand = enumCommand.CMD_LIVE_END;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PMS 완료 이벤트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void M_objPMSManager_ProcessDoneEvent()
        {
            // PMS 이미지 완료되면, Inspect를 호출해서 비디 검사를 태웁시다
            var pDocument = CDocument.GetDocument;

            pDocument.SetUpdateDisplayOriginal( pDocument.GetInspectionIndex(), ( int )CFormMainProcess60.enumDisplayIndex.PMS );
            SetEvent( enumEvent.EVENT_PMS );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 그랩 콜백
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetCallbackGrabImage( HLDevice.Abstract.CDeviceCameraAbstract.CImageData objImageData )
        {
            var pDocument = CDocument.GetDocument;

            do {
                m_bGrabComplete = objImageData.bGrabComplete;
                if( false == m_bGrabComplete ) {
                    string strErrorLog = string.Format( "CProcessVisionProcess60 {0} SetCallbackGrabImage Grab Complete false", m_iCameraIndex );
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0 + m_iCameraIndex ), strErrorLog );
                    break;
                }
                // 설비 시작인 경우 무조건 트리거로 간주
                if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) {
                    //pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "Image Grab_" + objImageData.iMultiGrabImageIndex.ToString() );
                    // 이미지 검사
                    SetOriginalImage( objImageData );
                    // 인덱스는 확인해봅시다
                    if( CDefine.enumMultiGrabIndex.GRAB_4 <= ( CDefine.enumMultiGrabIndex )objImageData.iMultiGrabImageIndex ) {
                        // 트리거 그랩 신호 왔으므로 이벤트 클리어
                        SetEvent( enumEvent.EVENT_GRAB );
                    }
                } else {
                    // 트리거인 경우
                    if( enumCommand.CMD_START_GRAB == m_eCommand || enumCommand.CMD_START_GRAB_MEASURE == m_eCommand ) {
                        // 이미지 검사
                        SetOriginalImage( objImageData );
                        // 인덱스는 확인해봅시다
                        //pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "Image Grab_" + objImageData.iMultiGrabImageIndex.ToString() );
                        if( CDefine.enumMultiGrabIndex.GRAB_4 <= ( CDefine.enumMultiGrabIndex )objImageData.iMultiGrabImageIndex ) {
                            // 멀티샷4장 그랩 신호 왔으므로 이벤트 클리어
                            SetEvent( enumEvent.EVENT_GRAB );
                        }
                    }
                    // 라이브인 경우
                    else {
                        // 이미지 라이브
                        SetLive( objImageData );
                    }
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 4장의 이미지를 PMS통해 합친다. 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetOriginalImage( HLDevice.Abstract.CDeviceCameraAbstract.CImageData objImageData )
        {
            var pDocument = CDocument.GetDocument;
            // 검사 결과 구조체
            CInspectionResult.CResult objResult = new CInspectionResult.CResult();
            // 에러 로그
            string strErrorLog = string.Format( "CProcessVisionProcess60 {0} SetPMSImage ", m_iCameraIndex );
            // 검사 제대로 동작했는지 유무
            bool bInspection = false;

            do {
                // 첫번째장은 새 데이터로 시작하기 위해 데이터를 읽어오지 않음
                if( 0 != objImageData.iMultiGrabImageIndex || CDefine.enumInspectionType.TYPE_MEASURE == m_objGrabType )
                    objResult = pDocument.GetInspectionResultAlign( pDocument.GetInspectionIndex() );

                objResult.objResultCommon.strTriggerTime = DateTime.Now.ToString( "HH:mm:ss.fff" );
                objResult.objResultCommon.iIndexDisplayOriginalImage = objImageData.iMultiGrabImageIndex;
                objResult.objResultCommon.objInputGrabOriginImage[ objImageData.iMultiGrabImageIndex ] = new Cognex.VisionPro.CogImage8Grey( objImageData.bitmapImage );
                objResult.objResultCommon.objInputGrabOriginalImageBitmap[ objImageData.iMultiGrabImageIndex ] = objImageData.bitmapImage;

                if( CDefine.enumInspectionType.TYPE_VIDI == m_objGrabType ) {
                    objResult.objResultCommon.objInputGrabOriginImageVIDI[ objImageData.iMultiGrabImageIndex ] = new Cognex.VisionPro.CogImage8Grey( objImageData.bitmapImage );
                    objResult.objResultCommon.objInputGrabOriginalImageBitmapVIDI[ objImageData.iMultiGrabImageIndex ] = objImageData.bitmapImage;
                } else {
                    objResult.objResultCommon.objInputGrabOriginImageMeasure[ objImageData.iMultiGrabImageIndex ] = new Cognex.VisionPro.CogImage8Grey( objImageData.bitmapImage );
                    objResult.objResultCommon.objInputGrabOriginalImageBitmapMeasure[ objImageData.iMultiGrabImageIndex ] = objImageData.bitmapImage;
                }

                bInspection = true;
            } while( false );

            // 양불 판정
            // OK 판정 조건
            // 여기서 Tolerance 체크해서 ok, ng 하는 건지는 판단에 맡김
            if( true == bInspection ) {
                objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_OK;
            } else {
                objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_NG;
            }

            // 검사 결과값 카피해서 도큐먼트 내 올려줌
            pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );
            // 디스플레이 결과 업데이트 ( 도큐먼트 내부에서 Begininvoke 호출 )
            pDocument.SetUpdateDisplayOriginal( pDocument.GetInspectionIndex(), ( int )CFormMainProcess60.enumDisplayIndex.ORIGIN_1 + objResult.objResultCommon.iIndexDisplayOriginalImage );
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetInspect( HLDevice.Abstract.CDeviceCameraAbstract.CImageData objImageData )
        {
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브 이미지
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetLive( HLDevice.Abstract.CDeviceCameraAbstract.CImageData objImageData )
        {
            var pDocument = CDocument.GetDocument;

            do {
                if( false == objImageData.bGrabComplete ) break;
                // 이미지 삽입
                m_objCogInputImage = new Cognex.VisionPro.CogImage8Grey( objImageData.bitmapImage );
                m_objCogOutputImage = m_objCogInputImage;
                // 이미지 데이터 복사
                m_objImageData = objImageData;
                // 라이브 이미지 도큐먼트 업데이트
                pDocument.SetLiveImage( m_iCameraIndex, m_objCogOutputImage );
                // 디스플레이 업데이트 ( 도큐먼트 내부에서 Begininvoke 호출 )
                pDocument.SetUpdateDisplayLive( m_iCameraIndex );

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 싱글 프레임 그랩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStartGrab()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                m_objGrabType = CDefine.enumInspectionType.TYPE_VIDI;
                int iRetryCount = 3;

                ResetEvent( enumEvent.EVENT_GRAB );

                // 라이브 온 상태면 라이브 중지하고 그랩해야함
                if( enumStatus.STS_LIVE_START == m_eStatus ) {
                    SetLiveEnd();
                }

                // 카메라 설정 변경
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );

                int iInspectionIndex = pDocument.GetInspectionIndex();
                m_refCamera.HLSetGain( objRecipeParameter.objCameraConfig.dGain );
                // 카메라 노출값 바꾸고
                m_refCamera.HLSetExposureTime( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.dCameraExpouseTime );
                // 카메라 그랩 이미지 사이즈 변경
                switch( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.eCropType ) {
                    // 설정크기외 최대값으로 설정해야되기때문에 10000으로 넣으면 카메라 클래스에서 자동으로 제일 크게 맞춰줌
                    case CDefine.enumCameraCropType.CROP_NONE:
                        m_refCamera.HLSetWidth( 10000 );
                        m_refCamera.HLSetHeight( 10000 );
                        break;
                    case CDefine.enumCameraCropType.CROP_WIDTH:
                        m_refCamera.HLSetWidth( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.iCropSize );
                        m_refCamera.HLSetHeight( 10000 );
                        break;
                    case CDefine.enumCameraCropType.CROP_HEIGHT:
                        m_refCamera.HLSetWidth( 10000 );
                        m_refCamera.HLSetHeight( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.iCropSize );
                        break;
                    default:
                        break;
                }



                // 조명 셋팅하고, HLSetAutoIndex는 다시 0부터 초기화해서 스트로브모드로 동작
                for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumLIghtChannel.CHANNEL_FINAL; iLoopCount++ ) {
                    m_refLight.HLSetPulse( iLoopCount, ( int )( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.dLightValue[ iLoopCount ] * 10 ) );
                }
                m_refLight.HLSetAutoIndex( ( int )CDefine.enumLIghtChannel.CHANNEL_FINAL );

                // 30만 쉬자
                Thread.Sleep( 30 );

                // 그랩
                if( false == m_refCamera.HLStartMultiGrab() ) break;
                // 카메라 그랩 완료를 기다린다.
                if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ], 2000 ) ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_TIME_OUT" );
                    break;
                }
                // 그랩 완료까지 재시도
                while( false == m_bGrabComplete && iRetryCount-- >= 0 ) {
                    // 실패시 이벤트를 리셋시키고 다시 한번 더 그랩
                    ResetEvent( enumEvent.EVENT_GRAB );
                    m_refLight.HLSetAutoIndex( ( int )CDefine.enumLIghtChannel.CHANNEL_FINAL );
                    Thread.Sleep( 50 );
                    if( false == m_refCamera.HLStartMultiGrab() ) break;
                    // 카메라 그랩 완료를 기다린다.
                    if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ], 2000 ) ) {
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_TIME_OUT" );
                        break;
                    }
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_RETRY" );
                }


                bReturn = true;
            } while( false );

            if( false == bReturn ) {
            }

            // 트리거 OFF
            //            pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1 + m_iCameraIndex, CDefine.enumTrigger.TRIGGER_OFF );
            ResetEvent( enumEvent.EVENT_GRAB );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 싱글 프레임 그랩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStartGrabMeasure()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                m_objGrabType = CDefine.enumInspectionType.TYPE_MEASURE;
                int iRetryCount = 3;

                ResetEvent( enumEvent.EVENT_GRAB );

                // 라이브 온 상태면 라이브 중지하고 그랩해야함
                if( enumStatus.STS_LIVE_START == m_eStatus ) {
                    SetLiveEnd();
                }

                // 카메라 설정 변경
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );

                int iInspectionIndex = pDocument.GetInspectionIndex();
                m_refCamera.HLSetGain( objRecipeParameter.objCameraConfig.dGain );
                // 카메라 노출값 바꾸고
                m_refCamera.HLSetExposureTime( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.dCameraExpouseTime );
                // 카메라 그랩 이미지 사이즈 변경
                switch( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.eCropType ) {
                    // 설정크기외 최대값으로 설정해야되기때문에 10000으로 넣으면 카메라 클래스에서 자동으로 제일 크게 맞춰줌
                    case CDefine.enumCameraCropType.CROP_NONE:
                        m_refCamera.HLSetWidth( 10000 );
                        m_refCamera.HLSetHeight( 10000 );
                        break;
                    case CDefine.enumCameraCropType.CROP_WIDTH:
                        m_refCamera.HLSetWidth( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.iCropSize );
                        m_refCamera.HLSetHeight( 10000 );
                        break;
                    case CDefine.enumCameraCropType.CROP_HEIGHT:
                        m_refCamera.HLSetWidth( 10000 );
                        m_refCamera.HLSetHeight( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameter.iCropSize );
                        break;
                    default:
                        break;
                }



                // 조명 셋팅하고, HLSetAutoIndex는 다시 0부터 초기화해서 스트로브모드로 동작
                for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumLIghtChannel.CHANNEL_FINAL; iLoopCount++ ) {
                    m_refLight.HLSetPulse( iLoopCount, ( int )( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGrabParameterMeasure.dLightValue[ iLoopCount ] * 10 ) );
                }
                m_refLight.HLSetAutoIndex( ( int )CDefine.enumLIghtChannel.CHANNEL_FINAL );

                // 30만 쉬자
                Thread.Sleep( 30 );
                // 그랩
                if( false == m_refCamera.HLStartMultiGrab() ) break;
                // 카메라 그랩 완료를 기다린다.
                if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ], 2000 ) ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_TIME_OUT" );
                    break;
                }
                // 그랩 완료까지 재시도
                while( false == m_bGrabComplete && iRetryCount-- >= 0 ) {
                    // 실패시 이벤트를 리셋시키고 다시 한번 더 그랩
                    ResetEvent( enumEvent.EVENT_GRAB );
                    m_refLight.HLSetAutoIndex( ( int )CDefine.enumLIghtChannel.CHANNEL_FINAL );
                    Thread.Sleep( 50 );
                    if( false == m_refCamera.HLStartMultiGrab() ) break;
                    // 카메라 그랩 완료를 기다린다.
                    if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ], 2000 ) ) {
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_TIME_OUT" );
                        break;
                    }
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_RETRY" );
                }
                bReturn = true;
            } while( false );

            if( false == bReturn ) {
            }

            // 트리거 OFF
            //            pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1 + m_iCameraIndex, CDefine.enumTrigger.TRIGGER_OFF );
            ResetEvent( enumEvent.EVENT_GRAB );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PMS 시작
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStartPMS()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                ResetEvent( enumEvent.EVENT_PMS );

                m_objPMSManager.SetInspectionType( CDefine.enumInspectionType.TYPE_VIDI );
                // PMS
                if( false == m_objPMSManager.SetPMSImage() ) break;
                // 카메라 그랩 완료를 기다린다.
                if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_PMS ], 20000 ) ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "SetStartPMS_FAIL_TIME_OUT" );
                    break;
                }
                bReturn = true;
            } while( false );

            if( false == bReturn ) {
            }

            ResetEvent( enumEvent.EVENT_PMS );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PMS 시작
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStartPMSMeasure()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                ResetEvent( enumEvent.EVENT_PMS );

                m_objPMSManager.SetInspectionType( CDefine.enumInspectionType.TYPE_MEASURE );
                // PMS
                if( false == m_objPMSManager.SetPMSImage() ) break;
                // 카메라 그랩 완료를 기다린다.
                if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_PMS ], 20000 ) ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "SetStartPMS_FAIL_TIME_OUT" );
                    break;
                }
                bReturn = true;
            } while( false );

            if( false == bReturn ) {
            }

            ResetEvent( enumEvent.EVENT_PMS );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 여기선 검사합시다
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStartInspection()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            CInspectionResult.CResult objResult = new CInspectionResult.CResult();
            objResult = pDocument.GetInspectionResultAlign( pDocument.GetInspectionIndex() );

            Stopwatch objStopwatch = new Stopwatch();
            objStopwatch.Start();
            string strLog = "";

            // 검사인덱스
            int iInspectionIndex = pDocument.GetInspectionIndex();
            // 레시피 파라미터읽어오고.
            CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );

            do {
                // 검사 시작
                // 그랩된 이미지를 가지고 패턴실행
                // 이미지 자르기
                // 비디실행

                // PMS이미지 선택함
                CogImage8Grey objImage = new CogImage8Grey( objResult.objResultCommon.objPMSImage[ ( int )objRecipeParameter.objInspectionParameter[ iInspectionIndex ].ePMSImageTypeVIDI ] );

                // 패턴실행
                HLDevice.Abstract.CVisionLibraryAbstract.CResultData objPatternData;
                m_refVisionLibrary[ iInspectionIndex ].HLRun( objImage, out objPatternData );


                if( false == objPatternData.bResult ) break;
                objResult.objGraphicsPMAlign.Add( ( ICogRecord )objPatternData.objGraphics[ 0 ] );
                objResult.objResultCommon.objOutputGrabImage = objPatternData.objCogImage;

                // 이미지를 자릅시다
                CogAffineTransformTool objCogAffineTransformTool = new CogAffineTransformTool();
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                    Stopwatch objSw = new Stopwatch();
                    objSw.Start();
                    double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                    dStartX = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].dStartX;
                    dStartY = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].dStartY;
                    dEndX = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].dEndX;
                    dEndY = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].dEndY;
                    dRotation = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].dRotation;
                    dSkew = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].dSkew;
                    objCogAffineTransformTool.InputImage = objPatternData.objCogImage;
                    objCogAffineTransformTool.Region.SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                    objCogAffineTransformTool.Run();
                    objResult.objResultCommon.objCropImageVidi.Add( new CogImage8Grey( ( CogImage8Grey )objCogAffineTransformTool.OutputImage ) );
                    objResult.objResultCommon.objCropImageBitmapVidi.Add( new Bitmap( objCogAffineTransformTool.OutputImage.ToBitmap() ) );

                    // 비디 시작 파라미터를 채우고
                    m_objVidiParameter[ iLoopCount ].Init();
                    m_objVidiParameter[ iLoopCount ].iInspectionIndex = iInspectionIndex;
                    m_objVidiParameter[ iLoopCount ].bBusy = true;
                    m_objVidiParameter[ iLoopCount ].objInputImage = objResult.objResultCommon.objCropImageVidi[ iLoopCount ];
                    m_objVidiParameter[ iLoopCount ].objInputImageBitmap = objResult.objResultCommon.objCropImageBitmapVidi[ iLoopCount ];
                    m_objVidiParameter[ iLoopCount ].strVidiStreamName = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.strStreamName;
                    m_objVidiParameter[ iLoopCount ].strVidiToolName = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.strToolName;

                    // 스레드풀로 비디검사 실행
                    ThreadPool.QueueUserWorkItem( new WaitCallback( ThreadVidiProcess ), m_objVidiParameter[ iLoopCount ] );
                }

                objImage = objResult.objResultCommon.objPMSImageMeasure[ ( int )objRecipeParameter.objInspectionParameter[ iInspectionIndex ].ePMSImageTypeMeasure ];
                Cognex.VisionPro.CalibFix.CogFixtureTool objFixtureTool = new Cognex.VisionPro.CalibFix.CogFixtureTool();
                objFixtureTool.InputImage = objImage;
                objFixtureTool.RunParams.UnfixturedFromFixturedTransform = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose();
                objFixtureTool.Run();
                objPatternData.objCogImage = objFixtureTool.OutputImage as CogImage8Grey;

                CogImage8Grey objImageSecond = new CogImage8Grey( objResult.objResultCommon.objPMSImageMeasure[ ( int )CDefine.enumPMSImageType.PMS_TYPE_Q ] );
                Cognex.VisionPro.CalibFix.CogFixtureTool objFixtureToolSecond = new Cognex.VisionPro.CalibFix.CogFixtureTool();
                objFixtureToolSecond.InputImage = objImageSecond;
                objFixtureToolSecond.RunParams.UnfixturedFromFixturedTransform = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose();
                objFixtureToolSecond.Run();
                objImageSecond = objFixtureToolSecond.OutputImage as CogImage8Grey;


                //objResult.objResultCommon.objOutputGrabImage = objPatternData.objCogImage;
                if( null != objPatternData.objCogImage ) {
                    for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                        Stopwatch objSw = new Stopwatch();
                        objSw.Start();
                        double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                        dStartX = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureSerchRegion[ iLoopCount ].dStartX;
                        dStartY = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureSerchRegion[ iLoopCount ].dStartY;
                        dEndX = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureSerchRegion[ iLoopCount ].dEndX;
                        dEndY = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureSerchRegion[ iLoopCount ].dEndY;
                        dRotation = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureSerchRegion[ iLoopCount ].dRotation;
                        dSkew = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureSerchRegion[ iLoopCount ].dSkew;

                        objCogAffineTransformTool.InputImage = objPatternData.objCogImage;
                        objCogAffineTransformTool.Region.SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                        objCogAffineTransformTool.Run();
                        objResult.objResultCommon.objCropImageMeasure.Add( new CogImage8Grey( ( CogImage8Grey )objCogAffineTransformTool.OutputImage ) );
                        objResult.objResultCommon.objCropImageBitmapMeasure.Add( new Bitmap( objCogAffineTransformTool.OutputImage.ToBitmap() ) );

                        objCogAffineTransformTool.InputImage = objImageSecond;
                        objCogAffineTransformTool.Region.SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                        objCogAffineTransformTool.Run();
                        objResult.objResultCommon.objCropImageMeasureSecond.Add( new CogImage8Grey( ( CogImage8Grey )objCogAffineTransformTool.OutputImage ) );
                        objResult.objResultCommon.objCropImageBitmapMeasureSecond.Add( new Bitmap( objCogAffineTransformTool.OutputImage.ToBitmap() ) );



                        // 비디 시작 파라미터를 채우고
                        m_objMeasureParameter[ iLoopCount ].Init();
                        m_objMeasureParameter[ iLoopCount ].bBusy = true;
                        m_objMeasureParameter[ iLoopCount ].objInputImage = objResult.objResultCommon.objCropImageMeasure[ iLoopCount ];
                        m_objMeasureParameter[ iLoopCount ].objInputImageBitmap = objResult.objResultCommon.objCropImageBitmapMeasure[ iLoopCount ];
                        m_objMeasureParameter[ iLoopCount ].objInputImageBitmapSecond = objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ];

                        CMeasureManager.structureMeasureParameter objParameter = new CMeasureManager.structureMeasureParameter();
                        objParameter.objImage = objResult.objResultCommon.objCropImageMeasure[ iLoopCount ];
                        objParameter.objImageBitmap = objResult.objResultCommon.objCropImageBitmapMeasure[ iLoopCount ];

                        objParameter.objImageSecond = objResult.objResultCommon.objCropImageMeasureSecond[ iLoopCount ];
                        objParameter.objImageBitmapSecond = objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ];

                        // L : LEFT, R : RIGHT
                        // 가로방향 끝점찾기
                        objParameter.nLRLowThresh = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureParameter.iBusbarLRLowThresh;
                        // 고정값.
                        objParameter.nLRContinousLen = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureParameter.iBusbarLRContinousLen;
                        // 현재는 안쓰는
                        objParameter.nLRHighThresh = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureParameter.iBusbarLRHighThresh;
                        //T : TOP, B : BOTTOM
                        objParameter.nTBThresh = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureParameter.iBusbarTBThresh;
                        objParameter.nTBContinousLen = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objMeasureParameter.iBusbarTBContinousLen;


                        m_objMeasureManager.SetInspectionBusbar( objParameter );

                        CMeasureManager.structureMeasureResult objMeasureResult = m_objMeasureManager.GetResultBusbar();
                        m_objMeasureParameter[ iLoopCount ].objResultRegion.dStartX = objMeasureResult.iStartX;
                        m_objMeasureParameter[ iLoopCount ].objResultRegion.dStartY = objMeasureResult.iStartY;
                        m_objMeasureParameter[ iLoopCount ].objResultRegion.dEndX = objMeasureResult.iWidth;// objMeasureResult.iEndX;
                        m_objMeasureParameter[ iLoopCount ].objResultRegion.dEndY = objMeasureResult.iEndY - objMeasureResult.iStartY;// objMeasureResult.iEndY;
                        m_objMeasureParameter[ iLoopCount ].dWidth = objMeasureResult.iWidth;
                        m_objMeasureParameter[ iLoopCount ].dHeight = objMeasureResult.iEndY - objMeasureResult.iStartY;

                        if( 1 > objMeasureResult.iWidth || 1 > objMeasureResult.iHeight ) {
                            m_objMeasureManager.SetInspectionBusbar( objParameter );
                            // X위치가 항상 6픽셀 옵셋이 생겨서 강제로 6픽셀 준다
                            objMeasureResult = m_objMeasureManager.GetResultBusbar();
                            m_objMeasureParameter[ iLoopCount ].objResultRegion.dStartX = objMeasureResult.iStartX + 6;
                            m_objMeasureParameter[ iLoopCount ].objResultRegion.dStartY = objMeasureResult.iStartY;
                            m_objMeasureParameter[ iLoopCount ].objResultRegion.dEndX = objMeasureResult.iWidth - 6;// objMeasureResult.iEndX;
                            m_objMeasureParameter[ iLoopCount ].objResultRegion.dEndY = objMeasureResult.iEndY - objMeasureResult.iStartY;//objMeasureResult.iHeight;// objMeasureResult.iEndY;
                            m_objMeasureParameter[ iLoopCount ].dWidth = objMeasureResult.iWidth - 6;
                            m_objMeasureParameter[ iLoopCount ].dHeight = objMeasureResult.iEndY - objMeasureResult.iStartY;// objMeasureResult.iHeight;
                        }
                    }
                }



                bool bVidiRunStatus = true;
                while( true ) {
                    bool bBusy = false;
                    for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                        if( true == m_objVidiParameter[ iLoopCount ].bBusy )
                            bBusy = true;
                    }

                    if( false == bBusy ) break;

                    int iTimeOut = 5000;
                    int iTimePeriod = 3;
                    iTimeOut -= iTimePeriod;
                    Thread.Sleep( iTimePeriod );
                    if( 0 >= iTimeOut ) {
                        bVidiRunStatus = false;
                        break;
                    }
                }

                bool bMeasureRunStatus = true;
                //                 while ( true )
                //                 {
                //                     bool bBusy = false;
                //                     for ( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ )
                //                     {
                //                         if ( true == m_objMeasureParameter[ iLoopCount ].bBusy )
                //                             bBusy = true;
                //                     }
                // 
                //                     if ( false == bBusy ) break;
                // 
                //                     int iTimeOut = 5000;
                //                     int iTimePeriod = 3;
                //                     iTimeOut -= iTimePeriod;
                //                     Thread.Sleep( iTimePeriod );
                //                     if ( 0 >= iTimeOut )
                //                     {
                //                         bMeasureRunStatus = false;
                //                         break;
                //                     }
                //                 }

                if( false == bVidiRunStatus ) {
                    // 비디 타임아웃일경우
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "VIDI Time out" );
                    break;
                }

                if( false == bMeasureRunStatus ) {
                    // 비디 타임아웃일경우
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "Measure Time out" );
                    break;
                }

                objResult.objResultCommon.iVidiResultCount = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion;
                objResult.objResultCommon.iMeasureResultCount = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion;

                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                    objResult.objResultCommon.objVidiResultImage.Add( new CogImage8Grey( m_objVidiParameter[ iLoopCount ].objResultImage ) );
                    objResult.objResultCommon.objVidiResultOverlayGraphic.Add( new CogImage8Grey( m_objVidiParameter[ iLoopCount ].objResultOverlay ) );
                    objResult.objResultCommon.objVidiScore.Add( m_objVidiParameter[ iLoopCount ].dScore );
                    objResult.objResultCommon.objVidiTactTime.Add( m_objVidiParameter[ iLoopCount ].strTactTime );
                }
                double dResolution = CDocument.GetDocument.m_objConfig.GetCameraParameter( m_iCameraIndex ).dResolution;
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                    objResult.objResultCommon.dListMeasureSizeWidth.Add( m_objMeasureParameter[ iLoopCount ].dWidth * dResolution );
                    objResult.objResultCommon.dListMeasureSizeHeight.Add( m_objMeasureParameter[ iLoopCount ].dHeight * dResolution );
                    objResult.objResultCommon.objMeasureTactTime.Add( m_objMeasureParameter[ iLoopCount ].strTactTime );
                    CInspectionResult.structureResultCommon.structureDataRegionMeasure objData = new CInspectionResult.structureResultCommon.structureDataRegionMeasure();
                    objData.dStartX = m_objMeasureParameter[ iLoopCount ].objResultRegion.dStartX;
                    objData.dStartY = m_objMeasureParameter[ iLoopCount ].objResultRegion.dStartY;
                    objData.dEndX = m_objMeasureParameter[ iLoopCount ].objResultRegion.dEndX;
                    objData.dEndY = m_objMeasureParameter[ iLoopCount ].objResultRegion.dEndY;
                    objResult.objResultCommon.objMeasureRegion.Add( objData );
                }

                strLog = "Inspection Process Tact Time : " + objStopwatch.ElapsedMilliseconds.ToString();
                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), strLog );


                bReturn = true;
            } while( false );

            if( false == bReturn ) {
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                    m_objVidiParameter[ iLoopCount ].Init();

                    objResult.objResultCommon.objVidiResultImage.Add( new CogImage8Grey( m_objVidiParameter[ iLoopCount ].objResultImage ) );
                    objResult.objResultCommon.objVidiResultOverlayGraphic.Add( new CogImage8Grey( m_objVidiParameter[ iLoopCount ].objResultOverlay ) );
                    objResult.objResultCommon.objVidiScore.Add( m_objVidiParameter[ iLoopCount ].dScore );
                }
            }

            pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );
            //pDocument.SetUpdateDisplayPMS( pDocument.GetInspectionIndex(), ( int )CFormMainProcess60.enumDisplayIndex.PMS );
            // 트리거 OFF
            // pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1 + m_iCameraIndex, CDefine.enumTrigger.TRIGGER_OFF );
            //SaveImage( ( CInspectionResult.CResult )objResult.Clone() );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ThreadVidiProcess( object obj )
        {
            CVidiParameter objParameter = obj as CVidiParameter;
            Stopwatch objStopwatch = new Stopwatch();
            objStopwatch.Start();

            var pDocument = CDocument.GetDocument;
            do {

                // 비디 스트림 이름 셋
                string strStreamName = objParameter.strVidiStreamName;
                m_refVidi[ 0/*objParameter.iVidiIndex*/ ].SetStreamName( strStreamName );

                string strToolName = objParameter.strVidiToolName;
                CDefine.enumVidiType eVidiType = objParameter.eVidiType;

                if( false == m_refVidi[ 0/*objParameter.iVidiIndex*/ ].Process( objParameter.objInputImageBitmap, strToolName, eVidiType ) ) {
                    objParameter.dScore = 0;
                    break;
                }
                double dVidiScoreThreshold = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex ).objInspectionParameter[ objParameter.iInspectionIndex ].dVidiScore;
                switch( eVidiType ) {
                    case CDefine.enumVidiType.RED:
                        m_refVidi[ 0/*objParameter.iVidiIndex*/ ].GetRedToolResult( strToolName, out objParameter.dScore, out objParameter.objResultOverlay, out objParameter.objResultImage );
                        objParameter.dScore = 1 - objParameter.dScore;
                        string strVidiResult = dVidiScoreThreshold > objParameter.dScore ? "OK" : "NG";
                        string strResult = string.Format( ",POSITION : {0}, REGION : {1}, VIDI_SCORE : {2:F2}, RESULT : {3}", objParameter.iInspectionIndex + 1, objParameter.iVidiIndex + 1, objParameter.dScore, strVidiResult );
                        //pDocument.SetUpdateLog( CDefine.enumLogType.LOG_RESULT_VIDI, strResult, false );
                        break;
                    case CDefine.enumVidiType.GREEN:
                        // 이건좀 달라서 나중에
                        break;
                    case CDefine.enumVidiType.BLUE:
                        // 이건좀 달라서 나중에
                        break;
                    case CDefine.enumVidiType.TYPE_FINAL:
                        break;
                }
            } while( false );
            objStopwatch.Stop();
            objParameter.strTactTime = objStopwatch.ElapsedMilliseconds.ToString();
            objParameter.bBusy = false;
        }

        private void ThreadMeasureProcess( object obj )
        {
            CMeasureParameter objParameter = obj as CMeasureParameter;
            Stopwatch objStopwatch = new Stopwatch();
            objStopwatch.Start();
            do {


            } while( false );
            objStopwatch.Stop();
            objParameter.strTactTime = objStopwatch.ElapsedMilliseconds.ToString();
            objParameter.bBusy = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브 모드에서 이미지 검사 할 경우 호출
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStartLiveInspection()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                // 검사
                pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1 + m_iCameraIndex, CDefine.enumTrigger.TRIGGER_OFF );
                if( null != m_objImageData.bitmapImage ) {
                    SetInspect( ( HLDevice.Abstract.CDeviceCameraAbstract.CImageData )m_objImageData.Clone() );
                }

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
        public bool SetLiveStart()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                if( false == m_refCamera.HLSetContinousGrab() ) break;
                m_refLight.HLSetChannelHold( 2 );
                pDocument.SetLiveMode( ( int )CDefine.enumCamera.CAMERA_1 + m_iCameraIndex, CDefine.enumLiveMode.LIVE_MODE_ON );

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
        public bool SetLiveEnd()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                pDocument.SetLiveMode( ( int )CDefine.enumCamera.CAMERA_1 + m_iCameraIndex, CDefine.enumLiveMode.LIVE_MODE_OFF );
                if( false == m_refCamera.HLSetSingleGrab() ) break;

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
        public bool SetAlignResult()
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
        //목적 : 상태 감시 쓰레드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadProcess( object state )
        {
            CProcessVisionProcess60 pThis = ( CProcessVisionProcess60 )state;

            while( false == pThis.m_bThreadExit ) {
                if( enumCommand.CMD_IDLE != pThis.m_eCommand ) {
                    pThis.m_eStatus = enumStatus.STS_UNKNOWN;

                    switch( pThis.m_eCommand ) {
                        case enumCommand.CMD_START_INSPECTION:
                            if( true == pThis.SetStartInspection() ) pThis.m_eStatus = enumStatus.STS_START_INSPECTION;
                            break;
                        case enumCommand.CMD_START_GRAB:
                            if( true == pThis.SetStartGrab() ) pThis.m_eStatus = enumStatus.STS_START_GRAB;
                            break;
                        case enumCommand.CMD_START_GRAB_MEASURE:
                            if( true == pThis.SetStartGrabMeasure() ) pThis.m_eStatus = enumStatus.STS_START_GRAB_MEASURE;
                            break;
                        case enumCommand.CMD_START_PMS:
                            if( true == pThis.SetStartPMS() ) pThis.m_eStatus = enumStatus.STS_START_PMS;
                            break;
                        case enumCommand.CMD_START_PMS_MEASURE:
                            if( true == pThis.SetStartPMSMeasure() ) pThis.m_eStatus = enumStatus.STS_START_PMS_MEASURE;
                            break;
                        case enumCommand.CMD_START_LIVE_INSPECTION:
                            if( true == pThis.SetStartLiveInspection() ) pThis.m_eStatus = enumStatus.STS_START_LIVE_INSPECTION;
                            break;
                        case enumCommand.CMD_LIVE_START:
                            if( true == pThis.SetLiveStart() ) pThis.m_eStatus = enumStatus.STS_LIVE_START;
                            break;
                        case enumCommand.CMD_LIVE_END:
                            if( true == pThis.SetLiveEnd() ) pThis.m_eStatus = enumStatus.STS_LIVE_END;
                            break;
                        case enumCommand.CMD_ALIGN_RESULT:
                            if( true == pThis.SetAlignResult() ) pThis.m_eStatus = enumStatus.STS_ALIGN_RESULT;
                            break;
                    }

                    if( enumCommand.CMD_STOP == pThis.m_eCommand ) {
                        // 사용자 정지 체크
                        pThis.m_eStatus = enumStatus.STS_STOP;
                    } else {
                        // 날라온 Command에 대한 알람이 있었는지 체크. STS_UNKNOWN 그대로이면 알람.
                        if( enumStatus.STS_UNKNOWN == pThis.m_eStatus ) {
                            pThis.m_eStatus = enumStatus.STS_ERROR;
                        }
                    }

                    pThis.m_eCommand = enumCommand.CMD_IDLE;
                }

                Thread.Sleep( 5 );
            }
        }
    }
}