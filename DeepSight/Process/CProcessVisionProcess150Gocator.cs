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
using System.IO;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Blob;

namespace DeepSight {
    public class CProcessVisionProcess150Gocator : CProcessAbstract {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //enum
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 유닛 명령
        public enum enumCommand {
            CMD_IDLE = 0, CMD_STOP,
            CMD_START_INSPECTION,
            CMD_START_GRAB, CMD_STOP_GRAB, CMD_PROCESS_IMAGE_DATA,
            CMD_FINAL
        };

        // 유닛 상태
        public enum enumStatus {
            STS_UNKNOWN = 0, STS_ERROR, STS_STOP,
            STS_START_INSPECTION,
            STS_START_GRAB, STS_STOP_GRAB, STS_PROCESS_IMAGE_DATA,
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

        // 그랩 성공 여부
        private bool m_bGrabComplete;
        // 카메라 이름 ( 로그 남기기 위함 )
        private string m_strCameraName;
        // 로그 타입
        // 라이브용 멤버
        private HLDevice.Abstract.CDeviceCameraAbstract.CImageData m_objImageData;
        private bool bSimulationMode;
        // 검사
        private CMeasureManager m_objMeasureManager;

        private class CVidiParameter {
            public bool bBusy;
            // 트리거 검사포지션
            public int iInspectionIndex;
            public int iVidiIndex;
            // VIDI Input
            public CogImage8Grey objInputImage;
            public Bitmap objInputImageBitmap;
            public string strVidiStreamName;
            public string strVidiToolName;
            public CDefine.enumVidiType eVidiType;

            // 검사영역
            public double dStartX, dStartY, dEndX, dEndY, dRotation;
            public double dPatternPositionX, dPatternPositionY;
            public CInspectionResult.structureResultCommon.structureDataRegion3D objRegionData;
            public double[,] obj3DDataHeightCrop2d;
            public double[,] objHeightData;

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
                dStartX = dStartY = dEndX = dEndY = dRotation = 0;
                dPatternPositionX = dPatternPositionY = 0;
                objRegionData = new CInspectionResult.structureResultCommon.structureDataRegion3D();
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
                dStartX = dStartY = dEndX = dEndY = dRotation = 0;
                dPatternPositionX = dPatternPositionY = 0;
            }
        }

        private class CMeasureParameter {
            public bool bBusy;
            // 트리거 검사포지션
            public int iInspectionIndex;
            // 검사포지션 -> 검사영역n개
            public int iMeasureIndex;
            // 검사영역
            public double dStartX, dStartY, dEndX, dEndY, dRotation;
            public double dGradientStartX, dGradientStartY, dGradientEndX, dGradientEndY, dGradientRotation;
            public double dPatternPositionX, dPatternPositionY;
            public CInspectionResult.structureResultCommon.structureDataRegion3D objRegionData;
            public double[,] obj3DDataHeightCrop2d;
            public double[,] objHeightData;
            public int iHeightThresholdOverCount;
            public bool bHeightLimitOverHigh;
            public bool bHeightLimitOverLow;
            public int iHeightLimitOverBlobCountHigh;
            public int iHeightLimitOverBlobCountLow;
            public List<double> objListSampleHeightData;
            // Measure Input
            public CogImage8Grey objInputImage;
            public CogImage8Grey objInputImageSecond;
            public Bitmap objInputImageBitmap;
            public Bitmap objInputImageBitmapSecond;

            // Measure Output
            public CInspectionResult.structureResultCommon.structureDataLineMeasure objLineData;

            public Bitmap objResultImage;
            public Bitmap objResultOverlay;
            public double dScore;
            public string strTactTime;

            public CMeasureParameter()
            {
                bBusy = false;
                bHeightLimitOverHigh = false;
                bHeightLimitOverLow = false;
                iMeasureIndex = 0;
                objInputImage = new CogImage8Grey();
                objInputImageSecond = new CogImage8Grey();
                objInputImageBitmap = new Bitmap( 10, 10 );
                objInputImageBitmapSecond = new Bitmap( 10, 10 );
                objResultImage = new Bitmap( 10, 10 );
                objResultOverlay = new Bitmap( 10, 10 );
                iHeightThresholdOverCount = 0;
                dStartX = dStartY = dEndX = dEndY = dRotation = 0;
                dGradientStartX = dGradientStartY = dGradientEndX = dGradientEndY = dGradientRotation = 0;
                dPatternPositionX = dPatternPositionY = 0;
                iHeightLimitOverBlobCountHigh = iHeightLimitOverBlobCountLow = 0;
                objRegionData = new CInspectionResult.structureResultCommon.structureDataRegion3D();
                objLineData = new CInspectionResult.structureResultCommon.structureDataLineMeasure();
                objListSampleHeightData = new List<double>();
                dScore = 0;
            }

            public void Init()
            {
                bBusy = false;
                bHeightLimitOverHigh = false;
                bHeightLimitOverLow = false;
                objInputImage = new CogImage8Grey();
                objInputImageSecond = new CogImage8Grey();
                objInputImageBitmap = new Bitmap( 10, 10 );
                objInputImageBitmapSecond = new Bitmap( 10, 10 );
                objResultImage = new Bitmap( 10, 10 );
                objResultOverlay = new Bitmap( 10, 10 );
                iHeightThresholdOverCount = 0;
                dStartX = dStartY = dEndX = dEndY = dRotation = 0;
                dGradientStartX = dGradientStartY = dGradientEndX = dGradientEndY = dGradientRotation = 0;
                dPatternPositionX = dPatternPositionY = 0;
                dScore = 0;
                objLineData.dStartX = objLineData.dStartY = objLineData.dEndX = objLineData.dEndY = objLineData.dDistance = 0;
                iHeightLimitOverBlobCountHigh = iHeightLimitOverBlobCountLow = 0;
                objListSampleHeightData.Clear();
            }
        }

        //폭측정용 라인
        CogFindLineTool[] m_objFindLineToolTop;
        CogFindLineTool[] m_objFindLineToolBottom;
        CogBlobTool[] m_objBlobHigh;
        CogBlobTool[] m_objBlobLow;
        CogIntersectLineLineTool[] m_objIntersectLine;
        CoverWeldingDll[] m_objCoverWeld;

        private List<CVidiParameter> m_objVidiParameter;
        private List<CMeasureParameter> m_objMeasureParameter;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CProcessVisionProcess150Gocator()
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
                bSimulationMode = CDefine.enumSimulationMode.SIMULATION_MODE_ON == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ? true : false;

                // 인덱스 정의
                m_iCameraIndex = iCameraIndex;
                m_strCameraName = ( ( CDefine.enumCamera )m_iCameraIndex ).ToString() + ", ";
                // 그랩 완료 이벤트
                m_objWaitHandle = new EventWaitHandle[ ( int )enumEvent.EVENT_FINAL ][];
                for( int iLoopCount = 0; iLoopCount < ( int )enumEvent.EVENT_FINAL; iLoopCount++ ) {
                    m_objWaitHandle[ iLoopCount ] = new EventWaitHandle[ 1 ];
                }
                m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ][ 0 ] = new EventWaitHandle( false, EventResetMode.ManualReset );
                m_objWaitHandle[ ( int )enumEvent.EVENT_PMS ][ 0 ] = new EventWaitHandle( false, EventResetMode.ManualReset );

                // PLC 객체 참조
                m_objPLC = pDocument.m_objProcessMain.m_objPLC;
                // 카메라 및 비전 라이브러리 참조 객체
                m_refCamera = pDocument.m_objProcessMain.m_objCamera[ m_iCameraIndex ];
                m_refVisionLibrary = pDocument.m_objProcessMain.m_objVisionLibraryPMAlign;
                m_refVidi = pDocument.m_objProcessMain.m_objVidi;
                m_refLight = pDocument.m_objProcessMain.m_objLightController[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ];

                m_objVidiParameter = new List<CVidiParameter>();
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    CVidiParameter obj = new CVidiParameter();
                    // 툴이 1개밖에 생성이 안됨. 다 0번으로 실행하자
                    obj.iVidiIndex = iLoopCount;
                    m_objVidiParameter.Add( obj );
                }


                m_objMeasureParameter = new List<CMeasureParameter>();
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    CMeasureParameter obj = new CMeasureParameter();
                    // 툴이 1개밖에 생성이 안됨. 다 0번으로 실행하자
                    obj.iMeasureIndex = iLoopCount;
                    m_objMeasureParameter.Add( obj );
                }

                m_objFindLineToolTop = new CogFindLineTool[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                m_objFindLineToolBottom = new CogFindLineTool[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                m_objBlobHigh = new CogBlobTool[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                m_objBlobLow = new CogBlobTool[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                m_objIntersectLine = new CogIntersectLineLineTool[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                m_objCoverWeld = new CoverWeldingDll[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    m_objFindLineToolTop[ iLoopCount ] = new CogFindLineTool();
                    m_objFindLineToolBottom[ iLoopCount ] = new CogFindLineTool();
                    m_objIntersectLine[ iLoopCount ] = new CogIntersectLineLineTool();
                    m_objFindLineToolTop[ iLoopCount ].InputImage = new CogImage8Grey( 10, 10 );
                    m_objFindLineToolBottom[ iLoopCount ].InputImage = new CogImage8Grey( 10, 10 );
                    m_objIntersectLine[ iLoopCount ].InputImage = new CogImage8Grey( 10, 10 );
                    m_objBlobHigh[ iLoopCount ] = new CogBlobTool();
                    m_objBlobHigh[ iLoopCount ].InputImage = new CogImage8Grey( 10, 10 );
                    m_objBlobHigh[ iLoopCount ].Run();
                    m_objBlobLow[ iLoopCount ] = new CogBlobTool();
                    m_objBlobLow[ iLoopCount ].InputImage = new CogImage8Grey( 10, 10 );
                    m_objBlobLow[ iLoopCount ].Run();

                    m_objFindLineToolTop[ iLoopCount ].Run();
                    m_objFindLineToolBottom[ iLoopCount ].Run();
                    m_objIntersectLine[ iLoopCount ].Run();

                    m_objCoverWeld[ iLoopCount ] = new CoverWeldingDll();
                }


                // 라이브용 이미지 멤버
                m_objImageData = new HLDevice.Abstract.CDeviceCameraAbstract.CImageData();
                // 카메라 그랩 콜백
                m_refCamera.HLSetCallbackFunctionGrabImage( SetCallbackGrabImage );
                // 트레이스 메세지 콜백
                m_refCamera.HLSetCallbackTraceMessage( SetCallbackTraceMessage );
                // 그랩 에러 콜백
                m_refCamera.HLSetCallbackGrabError( SetCallbackGrabError );

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

            pDocument.SetUpdateDisplayOriginal( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150.enumDisplayIndex.PMS );
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
                    string strErrorLog = string.Format( "CProcessVisionProcess150Gocator {0} SetCallbackGrabImage Grab Complete false", m_iCameraIndex );
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0 + m_iCameraIndex ), strErrorLog );
                    SetEvent( enumEvent.EVENT_GRAB );
                    break;
                }
                // 설비 시작인 경우 무조건 트리거로 간주
                if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) {
                    SetImageData( objImageData );
                    // 트리거 그랩 신호 왔으므로 이벤트 클리어
                    SetEvent( enumEvent.EVENT_GRAB );
                } else {
                    // 트리거인 경우
                    if( enumCommand.CMD_START_GRAB == m_eCommand ) {
                        SetImageData( objImageData );
                        SetEvent( enumEvent.EVENT_GRAB );
                    }
                    // 라이브인 경우
                    else {
                        // 이미지 라이브
                        //SetLive( objImageData );
                    }
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetImageData( HLDevice.Abstract.CDeviceCameraAbstract.CImageData objImageData )
        {
            var pDocument = CDocument.GetDocument;
            // 검사 결과 구조체
            CInspectionResult.CResult objResult = new CInspectionResult.CResult();
            // 에러 로그
            string strErrorLog = string.Format( "CProcessVisionProcess150Gocator {0} SetPMSImage ", m_iCameraIndex );

            do {
                objResult.objResultCommon.strTriggerTime = DateTime.Now.ToString( "HH:mm:ss.fff" );
                objResult.objResultCommon.iIndexDisplayOriginalImage = objImageData.iMultiGrabImageIndex;
                //                 objResult.objResultCommon.objInputGrabOriginImage[ objImageData.iMultiGrabImageIndex ] = new Cognex.VisionPro.CogImage8Grey( objImageData.bitmapImage ); 
                //                 objResult.objResultCommon.objInputGrabOriginalImageBitmap[ objImageData.iMultiGrabImageIndex ] = objImageData.bitmapImage;

                objResult.objResultCommon.i3DImageWidth = objImageData.objCameraData3D.iWidth;
                objResult.objResultCommon.i3DImageHeight = objImageData.objCameraData3D.iHeight;
                objResult.objResultCommon.i3DResolutionX = objImageData.objCameraData3D.iResolutionX;
                objResult.objResultCommon.i3DResolutionY = objImageData.objCameraData3D.iResolutionY;
                objResult.objResultCommon.i3DResolutionZ = objImageData.objCameraData3D.iResolutionZ;
                objResult.objResultCommon.i3DOffsetZ = objImageData.objCameraData3D.iOffsetZ;
                objResult.objResultCommon.obj3DDataHeightOrigin = objImageData.objCameraData3D.objHeightDataOrigin.Clone() as short[];
                objResult.objResultCommon.obj3DDataIntensityOrigin = objImageData.objCameraData3D.objIntensityDataOrigin.Clone() as byte[];
            } while( false );

            // 검사 결과값 카피해서 도큐먼트 내 올려줌
            pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );
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
                // 카메라 설정 변경
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );

                int iInspectionIndex = pDocument.GetInspectionIndex();
                // 3d 센서 스캔길이 설정해주고
                if( false == m_refCamera.HL3DSetScanLength( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].i3DScanLength ) ) break;
                if( false == m_refCamera.HL3DSetScanWidth( objRecipeParameter.objInspectionParameter[ iInspectionIndex ].d3DScanWidth ) ) break;

                ResetEvent( enumEvent.EVENT_GRAB );
                // 그랩
                if( false == bSimulationMode ) {
                    if( false == m_refCamera.HL3DStart() ) break;
                } else {
                    if( false == m_refCamera.HL3DStart( pDocument.GetInspectionIndex() ) ) break;
                }


                // 비전 Busy상태
                if( false == m_objPLC.HLWriteWordFromPLC( "PC_BUSY", ( short )CDefine.enumVisionStatus.STATUS_BUSY ) )
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 ), "[SEND_PLC][FAIL]Vision_Status_Busy On" );
                else
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 ), "[SEND_PLC]Vision_Status_Busy On" );


                // 카메라 그랩 완료를 기다린다.
                if( false == WaitHandle.WaitAll( m_objWaitHandle[ ( int )enumEvent.EVENT_GRAB ], 15000 ) ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), "GRAB_FAIL_TIME_OUT" );
                    m_bGrabComplete = true;
                    break;
                }

                bReturn = true;
            } while( false );

            if( false == bReturn ) {
                m_refCamera.HL3DStop();
                // 비전 Busy상태
                m_objPLC.HLWriteWordFromPLC( "PC_BUSY", ( short )CDefine.enumVisionStatus.STATUS_IDLE );
            }

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 싱글 프레임 그랩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetStopGrab()
        {
            bool bReturn = false;

            do {
                if( false == m_refCamera.HL3DStop() ) break;

                bReturn = true;
            } while( false );


            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 처리
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetProcessImageData()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );
            CInspectionResult.CResult objResult = pDocument.GetInspectionResultAlign( pDocument.GetInspectionIndex() );
            // 검사인덱스
            int iInspectionIndex = pDocument.GetInspectionIndex();
            do {
                try
                {
                    objResult.objResultCommon.objVidiScore.Clear();
                    short[] sHeightDataOrigin = objResult.objResultCommon.obj3DDataHeightOrigin;        //센서에서 획득한 원본 3D Data(1차원 배열)
                    int[] iHeightDataOrigin = new int[ sHeightDataOrigin.Length ];                      //원본 3D Data를 short에서 int로 변환
                    byte[] byteIntensityOrigin = objResult.objResultCommon.obj3DDataIntensityOrigin;    //센서에서 획득한 원본 밝기 이미지

                    int iWidth = objResult.objResultCommon.i3DImageWidth;
                    int iHeight = objResult.objResultCommon.i3DImageHeight;
                    int iResolution = objResult.objResultCommon.i3DResolutionZ;
                    int iZoffset = objResult.objResultCommon.i3DOffsetZ;

                    //  SetDataToCsv( "d:\\Test3D\\HeightDataOrigin1D.csv", sHeightDataOrigin );
                    int[,] objHeightDataOrigin2dBuf = new int[ iHeight, iWidth ];       //원본 3D Data를 2차원 배열로 변환
                    byte[,] objIntensityDataOrigin2dBuf = new byte[ iHeight, iWidth ];  //원본 밝기 이미지를 2차원 배열로 변환

                    Stopwatch objTact = new Stopwatch();
                    objTact.Start();

                    // 3D Data를 2차원 배열로 변환
                    for( int iLoopHeight = 0; iLoopHeight < iHeight; iLoopHeight++ ) {
                        for( int iLoopWidth = 0; iLoopWidth < iWidth; iLoopWidth++ ) {
                            objHeightDataOrigin2dBuf[ iLoopHeight, iLoopWidth ] = ( sHeightDataOrigin[ ( iLoopHeight * iWidth ) + iLoopWidth ] /*+ iZoffset*/ );
                            iHeightDataOrigin[ ( iLoopHeight * iWidth ) + iLoopWidth ] = ( sHeightDataOrigin[ ( iLoopHeight * iWidth ) + iLoopWidth ] /*+ iZoffset*/ );
                        }
                    }

                    //원본 밝기 이미지를 2차원 배열로 변환
                    for( int iLoopHeight = 0; iLoopHeight < iHeight; iLoopHeight++ ) {
                        for( int iLoopWidth = 0; iLoopWidth < iWidth; iLoopWidth++ ) {
                            objIntensityDataOrigin2dBuf[ iLoopHeight, iLoopWidth ] = byteIntensityOrigin[ ( iLoopHeight * iWidth ) + iLoopWidth ];
                        }
                    }

                    //90도 회전 시키기
                    if( CDefine.enumImageRotation.ROTATION_90 == objRecipeParameter.objInspectionParameter[ iInspectionIndex ].e3DImageRotation ) {
                        iHeight = objResult.objResultCommon.i3DImageWidth;
                        iWidth = objResult.objResultCommon.i3DImageHeight;
                        RotateRight( ref objHeightDataOrigin2dBuf );
                        RotateRight( ref objIntensityDataOrigin2dBuf );
                    } else if( CDefine.enumImageRotation.ROTATION_270 == objRecipeParameter.objInspectionParameter[ iInspectionIndex ].e3DImageRotation ) {
                        iHeight = objResult.objResultCommon.i3DImageWidth;
                        iWidth = objResult.objResultCommon.i3DImageHeight;
                        RotateLeft( ref objHeightDataOrigin2dBuf );
                        RotateLeft( ref objIntensityDataOrigin2dBuf );
                    }
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "1D -> 2D Array Create : " + objTact.ElapsedMilliseconds.ToString() );
                    objTact.Restart();

                    //-32768을 자르기 위해 -32768이 아닌 데이터를 찾아서 거기서 부터 자른다
                    int iIndexWidth = 0;
                    int iIndexHeight = 0;
                    //bool bFind = false;
                    //for (int iLoopHeight = 10; iLoopHeight < iHeight; iLoopHeight++)
                    //{
                    //    for (int iLoopWidth = 200; iLoopWidth < iWidth; iLoopWidth++)
                    //    {
                    //        if ( /*61134*/ -32768 != objHeightDataOrigin2dBuf[iLoopHeight, iLoopWidth])
                    //        {
                    //            iIndexWidth = iLoopWidth;
                    //            iIndexHeight = iLoopHeight;
                    //            bFind = true;
                    //            break;
                    //        }
                    //    }
                    //    if (true == bFind) break;
                    //}
                    int iIndexWidthReverse = 0;
                    int iIndexHeightReverse = 0;
                    //bFind = false;
                    //for (int iLoopHeight = iHeight - 11; iLoopHeight >= 0; iLoopHeight--)
                    //{
                    //    for (int iLoopWidth = iWidth - 201; iLoopWidth >= 0; iLoopWidth--)
                    //    {
                    //        if ( /*61134*/ -32768 != objHeightDataOrigin2dBuf[iLoopHeight, iLoopWidth])
                    //        {

                    //            iIndexWidthReverse = iWidth - iLoopWidth - 1;
                    //            iIndexHeightReverse = iHeight - iLoopHeight - 1;
                    //            bFind = true;
                    //            break;
                    //        }
                    //    }
                    //    if (true == bFind) break;
                    //}


                    int iNewSizeWidth = iWidth - ( iIndexWidth + iIndexWidthReverse );
                    int iNewSizeHeight = iHeight - ( iIndexHeight + iIndexHeightReverse );

                    double[,] objHeightDataOrigin2d = new double[ iNewSizeHeight, iNewSizeWidth ];
                    double[] objHeightDataOrigin1d = new double[ iNewSizeWidth * iNewSizeHeight ];

                    double dZoffset = ( double )( iZoffset ) / 1000;
                    double dAvreage = ( double )iHeightDataOrigin.Average();
                    for( int iLoopHeight = 0; iLoopHeight < iNewSizeHeight; iLoopHeight++ ) {
                        for( int iLoopWidth = 0; iLoopWidth < iNewSizeWidth; iLoopWidth++ ) {

                            if( /*61134*/ -32768 == objHeightDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ] ) {
                          //      objHeightDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ] = ( int )dAvreage;// 0;
                            }

                            ////objHeightDataOrigin2d[iLoopHeight, iLoopWidth] = (double)((objHeightDataOrigin2dBuf[iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth] + iZoffset) * iResolution) / 1000000;
                            ///
                            // 아무가공안한데이터
                            //objHeightDataOrigin2d[ iLoopHeight, iLoopWidth ] = ( ( double )( objHeightDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ] ) ) ;
                            if( /*61134*/ -32768 == objHeightDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ] ) {
                                objHeightDataOrigin2d[ iLoopHeight, iLoopWidth ] = ( double )( objHeightDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ] );
                            } else {
                                objHeightDataOrigin2d[ iLoopHeight, iLoopWidth ] = ( ( double )( objHeightDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ] * iResolution ) / 1000000 ) + dZoffset;
                            }

                            if( 0 != objHeightDataOrigin2d[ iLoopHeight, iLoopWidth ] ) {
                                objHeightDataOrigin1d[ iLoopWidth + ( iLoopHeight * iNewSizeWidth ) ] = objHeightDataOrigin2d[ iLoopHeight, iLoopWidth ];
                            } else {
                                if( 0 != iLoopWidth + ( iLoopHeight * iNewSizeWidth ) )
                                    objHeightDataOrigin1d[ iLoopWidth + ( iLoopHeight * iNewSizeWidth ) ] = objHeightDataOrigin1d[ ( iLoopWidth + ( iLoopHeight * iNewSizeWidth ) ) - 1 ];
                            }
                        }
                    }

                    byte[,] objIntensityDataOrigin2d = new byte[ iNewSizeHeight, iNewSizeWidth ];
                    byte[] objIntensityDataOrigin1d = new byte[ iNewSizeWidth * iNewSizeHeight ];
                    for( int iLoopHeight = 0; iLoopHeight < iNewSizeHeight; iLoopHeight++ ) {
                        for( int iLoopWidth = 0; iLoopWidth < iNewSizeWidth; iLoopWidth++ ) {
                            objIntensityDataOrigin2d[ iLoopHeight, iLoopWidth ] = objIntensityDataOrigin2dBuf[ iLoopHeight + iIndexHeight, iLoopWidth + iIndexWidth ];
                            objIntensityDataOrigin1d[ iLoopWidth + ( iLoopHeight * iNewSizeWidth ) ] = objIntensityDataOrigin2d[ iLoopHeight, iLoopWidth ];
                        }
                    }

                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Find 0 Data and Crop Array : " + objTact.ElapsedMilliseconds.ToString() );
                    objTact.Restart();

                    // 레졸루션을 곱해서 최대최소값을 바꾸고
                    double imax = objHeightDataOrigin1d.Max()/* / iResolution*/;
                    double imin = objHeightDataOrigin1d.Min() /* / iResolution*/;

                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Height Map To Bitmap : " + objTact.ElapsedMilliseconds.ToString() );
                    objTact.Restart();

                    // Bitmap 이미지
                    //objResult.objResultCommon.obj3DImageHeightBitmap = RawBytesToBitmapGrey( bImageData, iNewSizeWidth, iNewSizeHeight );
                    objResult.objResultCommon.obj3DImageIntensityBitmap = RawBytesToBitmapGrey( objIntensityDataOrigin1d, iNewSizeWidth, iNewSizeHeight );

                    objResult.objResultCommon.obj3DDataHeightCrop1d = objHeightDataOrigin1d;
                    objResult.objResultCommon.obj3DDataHeightCrop2d = objHeightDataOrigin2d;
                    objResult.objResultCommon.obj3DDataIntensityCrop = objIntensityDataOrigin1d;
                    objResult.objResultCommon.obj3DDataHeight2dOriginal = objHeightDataOrigin2dBuf;

                    objResult.objResultCommon.obj3DImageHeight = new CogImage8Grey( objResult.objResultCommon.obj3DImageHeightBitmap );
                    objResult.objResultCommon.obj3DImageIntensity = new CogImage8Grey( objResult.objResultCommon.obj3DImageIntensityBitmap );

                    objResult.objResultCommon.obj3DImage[ ( int )CDefine.enum3DImageType.IMAGE_HEIGHT ] = objResult.objResultCommon.obj3DImageHeight;
                    objResult.objResultCommon.obj3DImage[ ( int )CDefine.enum3DImageType.IMAGE_INTENSITY ] = objResult.objResultCommon.obj3DImageIntensity;

                    objResult.objResultCommon.obj3DImageBitmap[ ( int )CDefine.enum3DImageType.IMAGE_HEIGHT ] = objResult.objResultCommon.obj3DImageHeightBitmap;
                    objResult.objResultCommon.obj3DImageBitmap[ ( int )CDefine.enum3DImageType.IMAGE_INTENSITY ] = objResult.objResultCommon.obj3DImageIntensityBitmap;

                    // pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Save Bitmap 3ea : " + objTact.ElapsedMilliseconds.ToString() );
                    objTact.Restart();
                } catch( Exception ex ) {
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, ex.ToString() );
                }



                bReturn = true;
            } while( false );

            if( false == bReturn ) {
            }

            // 파일저장하는부분은 나중에 하자 지금은 테스트니깐 여기에 있음

            // 검사 결과값 카피해서 도큐먼트 내 올려줌
            pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );
            // 디스플레이 결과 업데이트 ( 도큐먼트 내부에서 Begininvoke 호출 )
            pDocument.SetUpdateDisplay3D( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.IMAGE );

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

            CInspectionResult.CResult objResult = new CInspectionResult.CResult();  //검사 결과 수집

            objResult = pDocument.GetInspectionResultAlign( pDocument.GetInspectionIndex() );
            objResult.objResultCommon.obj3DResultHeightData = new List<double[,]>();
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

                // 패턴실행은 무조껀 Intensity이미지로..
                CogImage8Grey objImage = new CogImage8Grey( objResult.objResultCommon.obj3DImage[ ( int )CDefine.enum3DImageType.IMAGE_INTENSITY ] );

                // 패턴실행
                HLDevice.Abstract.CVisionLibraryAbstract.CResultData objPatternData;
                m_refVisionLibrary[ iInspectionIndex ].HLRun( objImage, out objPatternData );


                if( false == objPatternData.bResult ) break;

                objResult.objGraphicsPMAlign.Add( ( ICogRecord )objPatternData.objGraphics[ 0 ] );

                //검사는 선택이미지로 해야하는데 거의 높이일듯
                objImage = objResult.objResultCommon.obj3DImage[ ( int )CDefine.enum3DImageType.IMAGE_INTENSITY ];// ( int )objRecipeParameter.objInspectionParameter[ iInspectionIndex ].e3DImageTypeVIDI ];
                Cognex.VisionPro.CalibFix.CogFixtureTool objFixtureTool = new Cognex.VisionPro.CalibFix.CogFixtureTool();
                objFixtureTool.InputImage = objImage.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey;

                CogTransform2DLinear obj2DLinear = new CogTransform2DLinear();
                obj2DLinear.TranslationX = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose().TranslationX;
                obj2DLinear.TranslationY = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose().TranslationY;
                objFixtureTool.RunParams.UnfixturedFromFixturedTransform = obj2DLinear;
                //objFixtureTool.RunParams.UnfixturedFromFixturedTransform = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose();

                objFixtureTool.Run();
                objPatternData.objCogImage = objFixtureTool.OutputImage as CogImage8Grey;

                objResult.objResultCommon.dPatternPositionX = obj2DLinear.TranslationX;
                objResult.objResultCommon.dPatternPositionY = obj2DLinear.TranslationY;

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
                    m_objVidiParameter[ iLoopCount ].bBusy = true;
                    m_objVidiParameter[ iLoopCount ].iInspectionIndex = iInspectionIndex;
                    m_objVidiParameter[ iLoopCount ].objInputImage = objResult.objResultCommon.objCropImageVidi[ iLoopCount ];
                    m_objVidiParameter[ iLoopCount ].objInputImageBitmap = objResult.objResultCommon.objCropImageBitmapVidi[ iLoopCount ];
                    m_objVidiParameter[ iLoopCount ].strVidiStreamName = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.strStreamName;
                    m_objVidiParameter[ iLoopCount ].strVidiToolName = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.strToolName;

                    m_objVidiParameter[ iLoopCount ].dStartX = dStartX;
                    m_objVidiParameter[ iLoopCount ].dStartY = dStartY;
                    m_objVidiParameter[ iLoopCount ].dEndX = dEndX;
                    m_objVidiParameter[ iLoopCount ].dEndY = dEndY;
                    m_objVidiParameter[ iLoopCount ].dRotation = dRotation;
                    m_objVidiParameter[ iLoopCount ].dPatternPositionX = objPatternData.dPositionX[ 0 ];
                    m_objVidiParameter[ iLoopCount ].dPatternPositionY = objPatternData.dPositionY[ 0 ];
                    m_objVidiParameter[ iLoopCount ].obj3DDataHeightCrop2d = objResult.objResultCommon.obj3DDataHeightCrop2d;



                    // 스레드풀로 비디검사 실행
                    ThreadPool.QueueUserWorkItem( new WaitCallback( ThreadVidiProcess ), m_objVidiParameter[ iLoopCount ] );
                    Thread.Sleep( 10 );
                }

                // 검사는 무조껀 높이데이터로 한다
                objImage = objResult.objResultCommon.obj3DImage[ ( int )CDefine.enum3DImageType.IMAGE_HEIGHT /*( int )objRecipeParameter.objInspectionParameter[ iInspectionIndex ].e3DImageTypeMeasure*/ ];
                //objFixtureTool = new Cognex.VisionPro.CalibFix.CogFixtureTool();
                objFixtureTool.InputImage = objImage;
                objFixtureTool.RunParams.UnfixturedFromFixturedTransform = obj2DLinear;// ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose();
                objFixtureTool.Run();
                objPatternData.objCogImage = objFixtureTool.OutputImage.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey;// as CogImage8Grey;

                CogImage8Grey objImageSecond = new CogImage8Grey( objResult.objResultCommon.obj3DImage[ ( int )CDefine.enum3DImageType.IMAGE_INTENSITY ] );
                Cognex.VisionPro.CalibFix.CogFixtureTool objFixtureToolSecond = new Cognex.VisionPro.CalibFix.CogFixtureTool();
                objFixtureToolSecond.InputImage = objImageSecond;
                objFixtureToolSecond.RunParams.UnfixturedFromFixturedTransform = obj2DLinear;// ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iInspectionIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool.Results[ 0 ].GetPose();
                objFixtureToolSecond.Run();
                objImageSecond = objFixtureToolSecond.OutputImage.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey; //as CogImage8Grey;

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
                        //CogSerializer.SaveObjectToFile( objCogAffineTransformTool, "d:\\measure" + iLoopCount.ToString() + ".vpp" , typeof( System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ), CogSerializationOptionsConstants.All );

                        // 비디 시작 파라미터를 채우고
                        m_objMeasureParameter[ iLoopCount ].Init();
                        m_objMeasureParameter[ iLoopCount ].iInspectionIndex = iInspectionIndex;
                        m_objMeasureParameter[ iLoopCount ].bBusy = true;
                        m_objMeasureParameter[ iLoopCount ].objInputImage = objResult.objResultCommon.objCropImageMeasure[ iLoopCount ];
                        m_objMeasureParameter[ iLoopCount ].objInputImageSecond = objResult.objResultCommon.objCropImageMeasureSecond[ iLoopCount ];
                        m_objMeasureParameter[ iLoopCount ].objInputImageBitmap = objResult.objResultCommon.objCropImageBitmapMeasure[ iLoopCount ];
                        m_objMeasureParameter[ iLoopCount ].objInputImageBitmapSecond = objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ];
                        m_objMeasureParameter[ iLoopCount ].dStartX = dStartX;
                        m_objMeasureParameter[ iLoopCount ].dStartY = dStartY;
                        m_objMeasureParameter[ iLoopCount ].dEndX = dEndX;
                        m_objMeasureParameter[ iLoopCount ].dEndY = dEndY;
                        m_objMeasureParameter[ iLoopCount ].dRotation = dRotation;
                        m_objMeasureParameter[ iLoopCount ].dGradientStartX = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGradientRegion[ iLoopCount ].dStartX;
                        m_objMeasureParameter[ iLoopCount ].dGradientStartY = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGradientRegion[ iLoopCount ].dStartY;
                        m_objMeasureParameter[ iLoopCount ].dGradientEndX = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGradientRegion[ iLoopCount ].dEndX;
                        m_objMeasureParameter[ iLoopCount ].dGradientEndY = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGradientRegion[ iLoopCount ].dEndY;
                        m_objMeasureParameter[ iLoopCount ].dGradientRotation = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objGradientRegion[ iLoopCount ].dRotation;
                        m_objMeasureParameter[ iLoopCount ].dPatternPositionX = objPatternData.dPositionX[ 0 ];
                        m_objMeasureParameter[ iLoopCount ].dPatternPositionY = objPatternData.dPositionY[ 0 ];
                        m_objMeasureParameter[ iLoopCount ].obj3DDataHeightCrop2d = objResult.objResultCommon.obj3DDataHeightCrop2d;

                        // 스레드풀로검사 실행
                        ThreadPool.QueueUserWorkItem( new WaitCallback( ThreadMeasureProcess ), m_objMeasureParameter[ iLoopCount ] );
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
                while( true ) {
                    bool bBusy = false;
                    for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ iInspectionIndex ].iCountSerchRegion; iLoopCount++ ) {
                        if( true == m_objMeasureParameter[ iLoopCount ].bBusy )
                            bBusy = true;
                    }

                    if( false == bBusy ) break;

                    int iTimeOut = 5000;
                    int iTimePeriod = 10;
                    iTimeOut -= iTimePeriod;
                    Thread.Sleep( iTimePeriod );
                    if( 0 >= iTimeOut ) {
                        bMeasureRunStatus = false;
                        break;
                    }
                }

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

                    objResult.objResultCommon.objCropImageBitmapMeasure[ iLoopCount ] = m_objMeasureParameter[ iLoopCount ].objInputImageBitmap;
                    objResult.objResultCommon.obj3DRegionData[ iLoopCount ] = m_objMeasureParameter[ iLoopCount ].objRegionData;
                    objResult.objResultCommon.obj3DResultHeightData.Add( m_objMeasureParameter[ iLoopCount ].objHeightData );
                    objResult.objResultCommon.objMeasureTactTime.Add( m_objMeasureParameter[ iLoopCount ].strTactTime );
                    objResult.objResultCommon.obj3DResultOverlayGraphic.Add( new CogImage8Grey( m_objMeasureParameter[ iLoopCount ].objResultOverlay ) );
                    objResult.objResultCommon.obj3DHeghtThresholdOverCount.Add( m_objMeasureParameter[ iLoopCount ].iHeightThresholdOverCount );
                    objResult.objResultCommon.objMeasureLine.Add( m_objMeasureParameter[ iLoopCount ].objLineData );
                    objResult.objResultCommon.obj3DListHeightOverBlobCountHigh.Add( m_objMeasureParameter[ iLoopCount ].iHeightLimitOverBlobCountHigh );
                    objResult.objResultCommon.obj3DListHeightOverBlobCountLow.Add( m_objMeasureParameter[ iLoopCount ].iHeightLimitOverBlobCountLow );
                    objResult.objResultCommon.obj3DListWeldWidth.Add( m_objMeasureParameter[ iLoopCount ].objLineData.dDistance );
                    for( int iLoopSample = 0; iLoopSample < m_objMeasureParameter[ iLoopCount ].objListSampleHeightData.Count; iLoopSample++ )
                        objResult.objResultCommon.obj3DListSampleHeightData[ iLoopCount ].Add( m_objMeasureParameter[ iLoopCount ].objListSampleHeightData[ iLoopSample ] );
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
            //pDocument.SetUpdateDisplayPMS( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150.enumDisplayIndex.PMS );
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
            CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );
            do {
                // 비디 스트림 이름 셋
                string strStreamName = objParameter.strVidiStreamName;
                //비디객체생성은 1개만 됨
                if( false == m_refVidi[ 0/*objParameter.iVidiIndex*/ ].SetStreamName( strStreamName ) ) {
                    objParameter.dScore = 0;
                    break;
                }


                // 원본이미지의 크롭영역
                double dOriginStartX;
                double dOriginStartY;
                double dOriginEndX;
                double dOriginEndY;
                //90도 회전일경우
                if( 1 < objParameter.dRotation )// 라디안으로 계산 1.5707이 90도이므로 1보다 큰경우
                {
                    // dEndY 가 X떨어진 거리
                    dOriginStartX = objParameter.dStartX - objParameter.dEndY + objParameter.dPatternPositionX;
                    dOriginStartY = objParameter.dStartY + objParameter.dPatternPositionY;
                    dOriginEndX = dOriginStartX + objParameter.dEndY;
                    dOriginEndY = dOriginStartY + objParameter.dEndX;
                } else {
                    dOriginStartX = objParameter.dStartX + objParameter.dPatternPositionX;
                    dOriginStartY = objParameter.dStartY + objParameter.dPatternPositionY;
                    dOriginEndX = dOriginStartX + objParameter.dEndX;
                    dOriginEndY = dOriginStartY + objParameter.dEndY;
                }

                objParameter.objRegionData.dStartX = dOriginStartX;
                objParameter.objRegionData.dStartY = dOriginStartY;
                objParameter.objRegionData.dEndX = dOriginEndX;
                objParameter.objRegionData.dEndY = dOriginEndY;

                if( CDefine.enum3DImageType.IMAGE_HEIGHT == objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].e3DImageTypeVIDI ) {
                    // 높이데이터를 검색영역만큼 생성해서 가지고 있자
                    objParameter.objHeightData = new double[ ( int )( dOriginEndY - dOriginStartY ), ( int )( dOriginEndX - dOriginStartX ) ];
                    // 루프 세로 = StartY 부터 EndY까지
                    // 루프 가로 = StartX 부터 EndX까지
                    double[] objRawHeightData = new double[ ( int )( dOriginEndY - dOriginStartY ) * ( int )( dOriginEndX - dOriginStartX ) ];
                    float[] objRawHeightData1 = new float[ ( int )( dOriginEndY - dOriginStartY ) * ( int )( dOriginEndX - dOriginStartX ) ];
                    int iHeight = 0, iWidth = 0;
                    for( int iLoopHeight = ( int )dOriginStartY; iLoopHeight < ( int )dOriginEndY; iLoopHeight++ ) {
                        iWidth = 0;
                        for( int iLoopWidth = ( int )dOriginStartX; iLoopWidth < ( int )dOriginEndX; iLoopWidth++ ) {
                            if( iHeight < objParameter.objHeightData.GetLength( 0 ) && iWidth < objParameter.objHeightData.GetLength( 1 ) ) {
                                try {
                                    if( 0 > iLoopWidth || iLoopWidth >= objParameter.obj3DDataHeightCrop2d.GetLength( 1 ) || 0 > iLoopHeight || iLoopHeight >= objParameter.obj3DDataHeightCrop2d.GetLength( 0 ) )
                                        objRawHeightData[ ( iHeight * ( int )( dOriginEndX - dOriginStartX ) ) + iWidth ] = 60;
                                    else {
                                        objRawHeightData[ ( iHeight * ( int )( dOriginEndX - dOriginStartX ) ) + iWidth ] = objParameter.obj3DDataHeightCrop2d[ iLoopHeight, iLoopWidth ];
                                        objRawHeightData1[ ( iHeight * ( int )( dOriginEndX - dOriginStartX ) ) + iWidth ] = ( float )objParameter.obj3DDataHeightCrop2d[ iLoopHeight, iLoopWidth ];
                                    }
                                } catch( Exception ) {
                                }
                            }
                            iWidth++;
                        }
                        iHeight++;
                    }



                    try {
                        Mat objMat = new Mat( ( int )( dOriginEndY - dOriginStartY ), ( int )( dOriginEndX - dOriginStartX ), MatType.CV_32FC1, objRawHeightData1 );
                        Mat objMatReturn = new Mat();
                        Cv2.MedianBlur( objMat, objMatReturn, 3 );
                        objMatReturn.GetArray( 0, 0, objRawHeightData1 );
                    } catch( Exception ex ) {
                        Trace.WriteLine( ex.ToString() );
                    }
                    double imax = objRawHeightData1.Max()/* / iResolution*/;
                    double imin = imax;// objRawHeightData1.Min();
                    for( int iLoopCount = 0; iLoopCount < objRawHeightData1.Length; iLoopCount++ ) {
                        if( 0 != objRawHeightData1[ iLoopCount ] && -32768 != objRawHeightData1[ iLoopCount ] ) {
                            if( imin > objRawHeightData1[ iLoopCount ] )
                                imin = objRawHeightData1[ iLoopCount ];
                        }
                        //                     if( 0 != objRawHeightData1[ iLoopCount ] ) {
                        //                             if( 0.3 > Math.Abs( imax - objRawHeightData1[ iLoopCount ] ) )
                        //                                 if( imin > objRawHeightData1[ iLoopCount ] )
                        //                                     imin = objRawHeightData1[ iLoopCount ];
                        //                         }

                    }

                    //if( 0.5 < Math.Abs( imax - imin ) ) imin = imax - 0.5;
                    byte[] bImageData = new byte[ ( int )( dOriginEndY - dOriginStartY ) * ( int )( dOriginEndX - dOriginStartX ) ];

                    for( int iLoopCount = 0; iLoopCount < objRawHeightData1.Length; iLoopCount++ ) {
                        bImageData[ iLoopCount ] = ( byte )( ( 255 / ( imax - imin ) ) * ( ( objRawHeightData1[ iLoopCount ] ) - imin ) );
                    }


                    objParameter.objInputImageBitmap = RawBytesToBitmapGrey( bImageData, ( int )( dOriginEndX - dOriginStartX ), ( int )( dOriginEndY - dOriginStartY ) );
                }
                
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
                        string strVidiResult = dVidiScoreThreshold < objParameter.dScore ? "OK" : "NG";
                        string strResult = string.Format( ",POSITION : {0}, REGION : {1}, VIDI_SCORE : {2:F2}, RESULT : {3}", objParameter.iInspectionIndex + 1, objParameter.iVidiIndex + 1, objParameter.dScore, strVidiResult );
                        //pDocument.SetUpdateLog( CDefine.enumLogType.LOG_RESULT_VIDI, strResult );
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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ThreadMeasureProcess( object obj )
        {
            CMeasureParameter objParameter = obj as CMeasureParameter;
            Stopwatch objStopwatch = new Stopwatch();
            var pDocument = CDocument.GetDocument;
            CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );
            Stopwatch objTact = new Stopwatch();
            objStopwatch.Start();
            objTact.Start();

            do {
                //일단 여기서 높이측정관련 테스트좀 하자
                {
                    // 원본이미지의 크롭영역
                    double dOriginStartX;
                    double dOriginStartY;
                    double dOriginEndX;
                    double dOriginEndY;

                    double dGradientStartX;
                    double dGradientStartY;
                    double dGradientEndX;
                    double dGradientEndY;

                    //90도 회전일경우
                    if( 1 < objParameter.dRotation )// 라디안으로 계산 1.5707이 90도이므로 1보다 큰경우
                    {
                        // dEndY 가 X떨어진 거리
                        dOriginStartX = objParameter.dStartX - objParameter.dEndY + objParameter.dPatternPositionX;
                        dOriginStartY = objParameter.dStartY + objParameter.dPatternPositionY;
                        dOriginEndX = dOriginStartX + objParameter.dEndY;
                        dOriginEndY = dOriginStartY + objParameter.dEndX;
                    } else {
                        dOriginStartX = objParameter.dStartX + objParameter.dPatternPositionX;
                        dOriginStartY = objParameter.dStartY + objParameter.dPatternPositionY;
                        dOriginEndX = dOriginStartX + objParameter.dEndX;
                        dOriginEndY = dOriginStartY + objParameter.dEndY;
                    }

                    //90도 회전일경우
                    if( 1 < objParameter.dRotation )// 라디안으로 계산 1.5707이 90도이므로 1보다 큰경우
                    {
                        // dEndY 가 X떨어진 거리
                        dGradientStartX = objParameter.dGradientStartX - objParameter.dGradientEndY + objParameter.dPatternPositionX;
                        dGradientStartY = objParameter.dGradientStartY + objParameter.dPatternPositionY;
                        dGradientEndX = dGradientStartX + objParameter.dGradientEndY;
                        dGradientEndY = dGradientStartY + objParameter.dGradientEndX;
                    } else {
                        dGradientStartX = objParameter.dGradientStartX + objParameter.dPatternPositionX;
                        dGradientStartY = objParameter.dGradientStartY + objParameter.dPatternPositionY;
                        dGradientEndX = dGradientStartX + objParameter.dEndX;
                        dGradientEndY = dGradientStartY + objParameter.dEndY;
                    }
                    //bool bGradientPositonTop = false;
                    //if( dOriginStartY < dGradientStartY ) bGradientPositonTop = true;

                    objParameter.objRegionData.dStartX = dOriginStartX;
                    objParameter.objRegionData.dStartY = dOriginStartY;
                    objParameter.objRegionData.dEndX = dOriginEndX;
                    objParameter.objRegionData.dEndY = dOriginEndY;


                    // 높이데이터를 검색영역만큼 생성해서 가지고 있자
                    objParameter.objHeightData = new double[ ( int )( dOriginEndY - dOriginStartY ), ( int )( dOriginEndX - dOriginStartX ) ];
                    // 루프 세로 = StartY 부터 EndY까지
                    // 루프 가로 = StartX 부터 EndX까지
                    int iHeight = 0, iWidth = 0;
                    for( int iLoopHeight = ( int )dOriginStartY; iLoopHeight < ( int )dOriginEndY; iLoopHeight++ ) {
                        iWidth = 0;
                        for( int iLoopWidth = ( int )dOriginStartX; iLoopWidth < ( int )dOriginEndX; iLoopWidth++ ) {
                            if( iHeight < objParameter.objHeightData.GetLength( 0 ) && iWidth < objParameter.objHeightData.GetLength( 1 ) ) {

                                try {
                                    if( 0 <= iLoopHeight && 0 <= iLoopWidth && objParameter.obj3DDataHeightCrop2d.GetLength( 0 ) > iLoopHeight && objParameter.obj3DDataHeightCrop2d.GetLength( 1 ) > iLoopWidth )
                                        objParameter.objHeightData[ iHeight, iWidth ] = objParameter.obj3DDataHeightCrop2d[ iLoopHeight, iLoopWidth ];
                                    else {
                                        objParameter.objHeightData[ iHeight, iWidth ] = 0;
                                    }
                                } catch( Exception ) {
                                }
                            }
                            iWidth++;
                        }
                        iHeight++;
                    }
                    // 기울기를 구할 데이터 영역
                    double[,] objHeightGradientData = new double[ ( int )( dOriginEndY - dOriginStartY ), ( int )( dOriginEndX - dOriginStartX ) ];
                    iHeight = 0; iWidth = 0;
                    for( int iLoopHeight = ( int )dOriginStartY; iLoopHeight < ( int )dOriginEndY; iLoopHeight++ ) {
                        iWidth = 0;
                        for( int iLoopWidth = ( int )dOriginStartX; iLoopWidth < ( int )dOriginEndX; iLoopWidth++ ) {

                            iWidth++;
                        }
                        iHeight++;
                    }

                    // 루프 세로 = StartY 부터 EndY까지
                    // 루프 가로 = StartX 부터 EndX까지
                    double[] objRawHeightData = new double[ ( int )( dOriginEndY - dOriginStartY ) * ( int )( dOriginEndX - dOriginStartX ) ];
                    iHeight = 0; iWidth = 0;
                    for( int iLoopHeight = ( int )dOriginStartY; iLoopHeight < ( int )dOriginEndY; iLoopHeight++ ) {
                        iWidth = 0;
                        for( int iLoopWidth = ( int )dOriginStartX; iLoopWidth < ( int )dOriginEndX; iLoopWidth++ ) {
                            if( iHeight < objParameter.objHeightData.GetLength( 0 ) && iWidth < objParameter.objHeightData.GetLength( 1 ) ) {
                                try {
                                    if( 0 > iLoopWidth || iLoopWidth >= objParameter.obj3DDataHeightCrop2d.GetLength( 1 ) || 0 > iLoopHeight || iLoopHeight >= objParameter.obj3DDataHeightCrop2d.GetLength( 0 ) )
                                        objRawHeightData[ ( iHeight * ( int )( dOriginEndX - dOriginStartX ) ) + iWidth ] = 60;
                                    else
                                        objRawHeightData[ ( iHeight * ( int )( dOriginEndX - dOriginStartX ) ) + iWidth ] = objParameter.obj3DDataHeightCrop2d[ iLoopHeight, iLoopWidth ];
                                } catch( Exception ) {
                                    // 자르는 영역이 -좌표가 될때 60으로...

                                }
                            }
                            iWidth++;
                        }
                        iHeight++;
                    }

                    iHeight = 0; iWidth = 0;
                    for( int iLoopHeight = ( int )dGradientStartY; iLoopHeight < ( int )dGradientEndY; iLoopHeight++ ) {
                        iWidth = 0;
                        for( int iLoopWidth = ( int )dGradientStartX; iLoopWidth < ( int )dGradientEndX; iLoopWidth++ ) {
                            if( iHeight < objHeightGradientData.GetLength( 0 ) && iWidth < objHeightGradientData.GetLength( 1 ) ) {
                                try {
                                    if( 0 <= iLoopHeight && 0 <= iLoopWidth && objParameter.obj3DDataHeightCrop2d.GetLength( 0 ) > iLoopHeight && objParameter.obj3DDataHeightCrop2d.GetLength( 1 ) > iLoopWidth )
                                        objHeightGradientData[ iHeight, iWidth ] = objParameter.obj3DDataHeightCrop2d[ iLoopHeight, iLoopWidth ];
                                    else {
                                        objHeightGradientData[ iHeight, iWidth ] = 0;
                                    }
                                } catch( Exception ) {
                                }
                            }
                            iWidth++;
                        }
                        iHeight++;
                    }

                    double imax = objRawHeightData.Max()/* / iResolution*/;
                    double imin = objRawHeightData.Min() /* / iResolution*/;


                    byte[] bImageData = new byte[ ( int )( dOriginEndY - dOriginStartY ) * ( int )( dOriginEndX - dOriginStartX ) ];

                    // ranges[ iLoopCount ] 여기값에다가해상도를 곱하자.
                    // 원본데이터를 가지고 있어야할텐데... 해상도를 곱한값으로 가지고 있어야하나?
                    for( int iLoopCount = 0; iLoopCount < objRawHeightData.Length; iLoopCount++ ) {
                        bImageData[ iLoopCount ] = ( byte )( ( 255 / ( imax - imin ) ) * ( ( objRawHeightData[ iLoopCount ] /* / iResolution*/ ) - imin ) );
                    }


                    objParameter.objInputImageBitmap = RawBytesToBitmapGrey( bImageData, ( int )( dOriginEndX - dOriginStartX ), ( int )( dOriginEndY - dOriginStartY ) );
                    //objParameter.objInputImageBitmap.Save( "d:\\test1.bmp" );

                    string strLog = string.Format( ",POSITION : {0}, REGION : {1}, ThreadMeasure : DataCrop TactTime - {2}ms", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1, objTact.ElapsedMilliseconds.ToString() );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, strLog );
                    objTact.Restart();


                    if( true ) {
                        int[] x1 = new int[ objHeightGradientData.GetLength( 1 ) ];
                        float[] y1 = new float[ objHeightGradientData.GetLength( 1 ) ];
                        iHeight = iWidth = 0;

                        objParameter.objRegionData.obj3DListNGPositionX = new List<int>();
                        objParameter.objRegionData.obj3DListNGPositionY = new List<int>();

                        // 라인들의 기울기와 베이스기준의 평균을 구해서 사용
                        double dTilt = 0;
                        double dBias = 0;
                        List<double> objListTilt = new List<double>();
                        List<double> objListdBias = new List<double>();
                        iHeight = iWidth = 0;
                        for( int iLoopHeight = 0; iLoopHeight < objHeightGradientData.GetLength( 0 ); iLoopHeight++ ) {
                            iWidth = 0;
                            for( int iLoopWidth = 0; iLoopWidth < objHeightGradientData.GetLength( 1 ); iLoopWidth++ ) {
                                x1[ iWidth ] = iWidth;
                                y1[ iWidth ] = ( float )objHeightGradientData[ iHeight, iWidth ];
                                iWidth++;
                            }
                            iHeight++;
                            m_objCoverWeld[ objParameter.iMeasureIndex ].DoCoverWeldingPlaneFit( x1, y1, x1.Length, 0.02, 20 );
                            objListTilt.Add( m_objCoverWeld[ objParameter.iMeasureIndex ].GetCoverWeldingPlaneTilt() );
                            objListdBias.Add( m_objCoverWeld[ objParameter.iMeasureIndex ].GetCoverWeldingPlaneBias() );
                        }

                        dTilt = objListTilt.ToArray().Average();
                        dBias = objListdBias.ToArray().Average();
                        iHeight = iWidth = 0;
                        for( int iLoopHeight = 0; iLoopHeight < objHeightGradientData.GetLength( 0 ); iLoopHeight++ ) {
                            iWidth = 0;
                            for( int iLoopWidth = 0; iLoopWidth < objParameter.objHeightData.GetLength( 1 ); iLoopWidth++ ) {
                                objParameter.objHeightData[ iHeight, iWidth ] = objParameter.objHeightData[ iHeight, iWidth ] - ( ( dTilt * iLoopWidth ) + dBias );
                                //objParameter.objHeightData[ iHeight, iWidth ] = objParameter.objHeightData[ iHeight, iWidth ] - y1[ iWidth ];// ( ( dTilt * iLoopWidth ) + dBias );
                                iWidth++;
                            }
                            iHeight++;
                        }
                    }

                    strLog = string.Format( ",POSITION : {0}, REGION : {1}, ThreadMeasure : REVISION_GRADIENT_TACTTIME - {2}ms", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1, objTact.ElapsedMilliseconds.ToString() );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, strLog );
                    objTact.Restart();


                    int iImgeWidth = objParameter.objHeightData.GetLength( 1 );
                    int iImgeHeight = objParameter.objHeightData.GetLength( 0 );
                    byte[] objOverlayImage = new byte[ iImgeWidth * iImgeHeight ];
                    byte[] objOverlayImageHigh = new byte[ iImgeWidth * iImgeHeight ];
                    byte[] objOverlayImageLow = new byte[ iImgeWidth * iImgeHeight ];
                    double[] objResultData = new double[ iImgeWidth * iImgeHeight ];
                    int iOverCountUpper = 0;
                    int iOverCountLower = 0;


                    for( int iLoopHeight = 0; iLoopHeight < objParameter.objHeightData.GetLength( 0 ); iLoopHeight++ ) {
                        for( int iLoopWidth = 0; iLoopWidth < objParameter.objHeightData.GetLength( 1 ); iLoopWidth++ ) {
                            objResultData[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = objParameter.objHeightData[ iLoopHeight, iLoopWidth ];

                            if( iLoopHeight == objParameter.objHeightData.GetLength( 0 ) / 2 ) {
                                if( 0 != iLoopWidth && 0 == iLoopWidth % 50 ) {
                                    objParameter.objListSampleHeightData.Add( objParameter.objHeightData[ iLoopHeight, iLoopWidth ] );
                                }
                            }

                            if( objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].d3DHeightThresholdMax < objParameter.objHeightData[ iLoopHeight, iLoopWidth ] ) {
                                objParameter.iHeightThresholdOverCount++;
                                iOverCountUpper++;
                                objOverlayImage[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 0;
                                objOverlayImageHigh[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 0;
                                objOverlayImageLow[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 255;
                            } else if( objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].d3DHeightThresholdMin > objParameter.objHeightData[ iLoopHeight, iLoopWidth ] ) {
                                objParameter.iHeightThresholdOverCount++;
                                iOverCountLower++;
                                objOverlayImage[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 100;
                                objOverlayImageHigh[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 255;
                                objOverlayImageLow[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 0;
                            } else {
                                objOverlayImageHigh[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 255;
                                objOverlayImageLow[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 255;
                                objOverlayImage[ ( iLoopHeight * iImgeWidth ) + iLoopWidth ] = 255;
                            }
                        }
                    }

                    string strResult = string.Format( ",POSITION : {0}, REGION : {1}, MAX : {2:F3}, MIN : {3:F3}", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1, objResultData.Max(), objResultData.Min() );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_RESULT_MEASURE_3D, strResult );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_MEASURE_3D_HEIGHT_MIN_MAX, strResult, false );
                    objParameter.objResultOverlay = RawBytesToBitmapGrey( objOverlayImage, iImgeWidth, iImgeHeight );


                    strResult = string.Format( ",POSITION : {0}, REGION : {1}, SAMPLE HEIGHT DATA, ", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1 );
                    for( int iLoopCount = 0; iLoopCount < objParameter.objListSampleHeightData.Count; iLoopCount++ ) {
                        strResult += ( string.Format( "{0:F3},", objParameter.objListSampleHeightData[ iLoopCount ] ) );
                    }

                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_RESULT_MEASURE_3D, strResult );

                    // 높이 초과, 미만인경우 블랍으로 찾는다
                    CogImage8Grey objImageHigh = new CogImage8Grey( RawBytesToBitmapGrey( objOverlayImageHigh, iImgeWidth, iImgeHeight ) );
                    CogImage8Grey objImageLow = new CogImage8Grey( RawBytesToBitmapGrey( objOverlayImageLow, iImgeWidth, iImgeHeight ) );
                    m_objBlobHigh[ objParameter.iMeasureIndex ].InputImage = objImageHigh;
                    m_objBlobLow[ objParameter.iMeasureIndex ].InputImage = objImageLow;
                    m_objBlobHigh[ objParameter.iMeasureIndex ].RunParams.ConnectivityMinPixels = objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].i3DHeightThresholdCount;
                    m_objBlobLow[ objParameter.iMeasureIndex ].RunParams.ConnectivityMinPixels = objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].i3DHeightThresholdCount;
                    m_objBlobHigh[ objParameter.iMeasureIndex ].Run();
                    m_objBlobLow[ objParameter.iMeasureIndex ].Run();


                    if ( null != m_objBlobHigh[ objParameter.iMeasureIndex ].Results && 0 < m_objBlobHigh[ objParameter.iMeasureIndex ].Results.GetBlobs().Count ) {
                        objParameter.iHeightLimitOverBlobCountHigh = m_objBlobHigh[ objParameter.iMeasureIndex ].Results.GetBlobs().Count;
                        objParameter.bHeightLimitOverHigh = true;
                    } else
                        objParameter.bHeightLimitOverHigh = false;

                    if( null != m_objBlobLow[ objParameter.iMeasureIndex ].Results && 0 < m_objBlobLow[ objParameter.iMeasureIndex ].Results.GetBlobs().Count ) {
                        objParameter.iHeightLimitOverBlobCountLow = m_objBlobLow[ objParameter.iMeasureIndex ].Results.GetBlobs().Count;
                        objParameter.bHeightLimitOverLow = true;
                    } else
                        objParameter.bHeightLimitOverLow = false;

                    string strBLobResult = "OK";
                    if( 0 < objParameter.iHeightLimitOverBlobCountHigh || 0 < objParameter.iHeightLimitOverBlobCountLow ) strBLobResult = "NG";

                    double dResolution = pDocument.m_objConfig.GetCameraParameter( ( int )CDefine.enumCamera.CAMERA_1 ).dResolution;
                    strResult = string.Format( ",POSITION : {0}, REGION : {1}, OVER_REGION_SIZE : {2:F3}mm, MAX_OVER_REGION_COUNT : {3}, MIN_OVER_REGION_COUNT : {4}, RESULT : {5}", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1, objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].i3DHeightThresholdCount * dResolution, objParameter.iHeightLimitOverBlobCountHigh, objParameter.iHeightLimitOverBlobCountLow, strBLobResult );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_RESULT_MEASURE_3D, strResult );

                    // 여기서 폭 측정합시다
                    SetFindLineTool( objParameter.iInspectionIndex, objParameter.iMeasureIndex, objParameter.objInputImageSecond.Width, objParameter.objInputImageSecond.Height );
                    m_objFindLineToolTop[ objParameter.iMeasureIndex ].InputImage = objParameter.objInputImageSecond;
                    m_objFindLineToolBottom[ objParameter.iMeasureIndex ].InputImage = objParameter.objInputImageSecond;
                    m_objFindLineToolTop[ objParameter.iMeasureIndex ].Run();
                    m_objFindLineToolBottom[ objParameter.iMeasureIndex ].Run();

                    bool bResultTop = false;
                    bool bResultBottom = false;
                    if( null != m_objFindLineToolTop[ objParameter.iMeasureIndex ].Results && 0 < m_objFindLineToolTop[ objParameter.iMeasureIndex ].Results.Count ) {
                        bResultTop = true;
                    }

                    if( null != m_objFindLineToolBottom[ objParameter.iMeasureIndex ].Results && 0 < m_objFindLineToolBottom[ objParameter.iMeasureIndex ].Results.Count ) {
                        bResultBottom = true;
                    }

                    if( false == bResultTop || false == bResultBottom ) {
                        break;
                    }

                    CogLine objCogLine = new CogLine();
                    objCogLine.SelectedSpaceName = "#";
                    objCogLine.SetXYRotation( objParameter.objInputImageSecond.Width / 2, 0, 90 * ( Math.PI / 180 ) );

                    double dPositionXTop = 0;
                    double dPositionYTop = 0;
                    double dPositionXBottom = 0;
                    double dPositionYBottom = 0;

                    try {
                        m_objIntersectLine[ objParameter.iMeasureIndex ].InputImage = objParameter.objInputImageSecond;
                        m_objIntersectLine[ objParameter.iMeasureIndex ].LineA = m_objFindLineToolTop[ objParameter.iMeasureIndex ].Results.GetLine();
                        m_objIntersectLine[ objParameter.iMeasureIndex ].LineB = objCogLine;
                        m_objIntersectLine[ objParameter.iMeasureIndex ].Run();
                        dPositionXTop = m_objIntersectLine[ objParameter.iMeasureIndex ].X;
                        dPositionYTop = m_objIntersectLine[ objParameter.iMeasureIndex ].Y;

                        m_objIntersectLine[ objParameter.iMeasureIndex ].LineA = m_objFindLineToolBottom[ objParameter.iMeasureIndex ].Results.GetLine();
                        m_objIntersectLine[ objParameter.iMeasureIndex ].LineB = objCogLine;
                        m_objIntersectLine[ objParameter.iMeasureIndex ].Run();
                        dPositionXBottom = m_objIntersectLine[ objParameter.iMeasureIndex ].X;
                        dPositionYBottom = m_objIntersectLine[ objParameter.iMeasureIndex ].Y;
                    } catch( Exception ex ) {
                        pDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "ThreadMeasure : " + ex.ToString() );
                    }


                    CogDistancePointPointTool objDistance = new CogDistancePointPointTool();
                    objDistance.InputImage = objParameter.objInputImageSecond;
                    objParameter.objLineData.dStartX = objDistance.StartX = dPositionXTop;
                    objParameter.objLineData.dStartY = objDistance.StartY = dPositionYTop;
                    objParameter.objLineData.dEndX = objDistance.EndX = dPositionXBottom;
                    objParameter.objLineData.dEndY = objDistance.EndY = dPositionYBottom;
                    objDistance.Run();
                    objParameter.objLineData.dDistance = objDistance.Distance * pDocument.m_objConfig.GetCameraParameter( ( int )CDefine.enumCamera.CAMERA_1 ).dResolution; ;

                    strBLobResult = "OK";
                    if( objParameter.objLineData.dDistance > objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].d3DWeldWidthMax || objParameter.objLineData.dDistance < objRecipeParameter.objInspectionParameter[ objParameter.iInspectionIndex ].d3DWeldWidthMin ) strBLobResult = "NG";
                    strResult = string.Format( ",POSITION : {0}, REGION : {1}, WELD_WIDTH : {2:F3}mm, RESULT : {3}", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1, objParameter.objLineData.dDistance, strBLobResult );

                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_RESULT_MEASURE_3D, strResult );


                    strLog = string.Format( ",POSITION : {0}, REGION : {1}, ThreadMeasure : MEASURE_PROCESS_TACTTIME - {2}ms", objParameter.iInspectionIndex + 1, objParameter.iMeasureIndex + 1, objTact.ElapsedMilliseconds.ToString() );
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, strLog );
                    objTact.Restart();
                }


            } while( false );
            objStopwatch.Stop();
            pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "ThreadMeasure : " + objStopwatch.ElapsedMilliseconds.ToString() );
            objParameter.strTactTime = objStopwatch.ElapsedMilliseconds.ToString();
            objParameter.bBusy = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetFindLineTool( int iInspectionIndex, int iMeasureIndex, int iImageWidth, int iImageHeight )
        {
            var pDocument = CDocument.GetDocument;
            CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );

            CConfig.CFindLineParameter objLineParameter = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objFindLineTop[ iMeasureIndex ];
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.NumCalipers = objLineParameter.iCalipersNumber;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.NumToIgnore = objLineParameter.iIgnoreNumber;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperRunParams.ContrastThreshold = objLineParameter.iThreshold;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperRunParams.FilterHalfSizeInPixels = objLineParameter.iFilter;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperSearchLength = objLineParameter.dSearchLength;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperProjectionLength = 20;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperRunParams.Edge0Polarity = ( CogCaliperPolarityConstants )objLineParameter.ePolaraty;
            if( CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90 == objLineParameter.eSerachDirection )
                m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperSearchDirection = 90 * ( Math.PI / 180 );
            else
                m_objFindLineToolTop[ iMeasureIndex ].RunParams.CaliperSearchDirection = -90 * ( Math.PI / 180 );

            m_objFindLineToolTop[ iMeasureIndex ].RunParams.ExpectedLineSegment.SelectedSpaceName = "#";
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.ExpectedLineSegment.StartX = ( iImageWidth / 2 ) - ( iImageWidth / 6 );
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.ExpectedLineSegment.EndX = ( iImageWidth / 2 ) + ( iImageWidth / 6 );
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.ExpectedLineSegment.StartY = 0;
            m_objFindLineToolTop[ iMeasureIndex ].RunParams.ExpectedLineSegment.EndY = 0;

            objLineParameter = objRecipeParameter.objInspectionParameter[ iInspectionIndex ].objFindLineBottom[ iMeasureIndex ];
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.NumCalipers = objLineParameter.iCalipersNumber;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.NumToIgnore = objLineParameter.iIgnoreNumber;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperRunParams.ContrastThreshold = objLineParameter.iThreshold;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperRunParams.FilterHalfSizeInPixels = objLineParameter.iFilter;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperSearchLength = objLineParameter.dSearchLength;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperProjectionLength = 20;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperRunParams.Edge0Polarity = ( CogCaliperPolarityConstants )objLineParameter.ePolaraty;
            if( CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90 == objLineParameter.eSerachDirection )
                m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperSearchDirection = 90 * ( Math.PI / 180 );
            else
                m_objFindLineToolBottom[ iMeasureIndex ].RunParams.CaliperSearchDirection = -90 * ( Math.PI / 180 );

            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.ExpectedLineSegment.SelectedSpaceName = "#";
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.ExpectedLineSegment.StartX = ( iImageWidth / 2 ) - ( iImageWidth / 6 );
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.ExpectedLineSegment.EndX = ( iImageWidth / 2 ) + ( iImageWidth / 6 );
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.ExpectedLineSegment.StartY = iImageHeight;
            m_objFindLineToolBottom[ iMeasureIndex ].RunParams.ExpectedLineSegment.EndY = iImageHeight;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 상태 감시 쓰레드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadProcess( object state )
        {
            CProcessVisionProcess150Gocator pThis = ( CProcessVisionProcess150Gocator )state;

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
                        case enumCommand.CMD_STOP_GRAB:
                            if( true == pThis.SetStopGrab() ) pThis.m_eStatus = enumStatus.STS_STOP_GRAB;
                            break;
                        case enumCommand.CMD_PROCESS_IMAGE_DATA:
                            if( true == pThis.SetProcessImageData() ) pThis.m_eStatus = enumStatus.STS_PROCESS_IMAGE_DATA;
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