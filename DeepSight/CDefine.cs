using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
    public class CDefine
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Define
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const string DEF_CONFIG_INI = "VisionConfig.ini";
        public const string DEF_RECIPE_INI = "VisionRecipe.ini";
		public const string DEF_STAGE_INI = "VisionStage.ini";
        public const string DEF_CAMERA_INI = "VisionCamera.ini";
		public const string DEF_DEVICE_INI = "VisionDevice.ini";
        public const string DEF_USER_INFORMATION = "VisionUserInformation.ini";
        public const string DEF_SIMULATION_IMAGE_PATH = "SimulationImages";

        public const string DEF_IO_DAT = "IO.dat";
        public const string DEF_PLC_DAT = "PLC.dat";
        
        public const double DEF_CAL_MULTIPLE_XY = 10000.0;
        public const double DEF_CAL_MULTIPLE_T = 100000.0;
		// 디폴트 패킷 사이즈
		public const int DEF_DEFAULT_PACKET_SIZE = 8000;
		// 디폴트 하트비트 타임 아웃
		public const int DEF_DEFAULT_HEARTBEAT_TIMEOUT = 2000;

        public const int DEF_MAX_COUNT_INSPECTION_POSITION = 30;
        public const int DEF_MAX_COUNT_CROP_REGION = 6;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // enumeration
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Form View 에서 전환하는 Form 목록
        public enum FormView
        {
			FORM_VIEW_MAIN = 0, FORM_VIEW_SETUP, FORM_VIEW_CONFIG, FORM_VIEW_IO, FORM_VIEW_REPORT, FORM_VIEW_FINAL
        };
        // Form View - Main 에서 전환하는 Form 목록
        public enum FormViewMain
        {
            FORM_VIEW_MAIN_ALIGN = 0, FORM_VIEW_MAIN_FINAL
        };
        // Form View - Setup 에서 전환하는 Form 목록
        public enum FormViewSetup
        {
			FORM_VIEW_SETUP_VISION_ALIGN = 0, FORM_VIEW_SETUP_CAMERA, FORM_VIEW_SETUP_FINAL
        };
        // Form View - Config 에서 전환하는 Form 목록
        public enum FormViewConfig
        {
            FORM_VIEW_CONFIG_OPTION = 0, FORM_VIEW_CONFIG_RECIPE, FORM_VIEW_CONFIG_FINAL
        };
		// Form View - IO 에서 전환하는 Form 목록
		public enum FormViewCommunication
		{
			FORM_VIEW_COMMUNICATION_PLC = 0, FORM_VIEW_COMMUNICATION_FINAL
		};
		// Form View - Report 에서 전환하는 Form 목록
		public enum FormViewReport
		{
			FORM_VIEW_REPORT_ALIGN = 0, FORM_VIEW_REPORT_FINAL
		};
        // Alarm Type
        public enum enumAlarmType { ALARM_INFORMATION = 0, ALARM_WARNING, ALARM_ALARM, ALARM_INTERLOCK, ALARM_QUESTION, ALARM_MOTOR, ALARM_FINAL };
        // Question Reply
        public enum enumQuestion { QUESTION_YES, QUESTION_NO, QUESTION_FINAL };
        // Error Process Type
        public enum enumErrorProcess { ERROR_PROCESS_RETRY, ERROR_PROCESS_CONTINUE, ERROR_PROCESS_STOP, ERROR_PROCESS_FINAL };
        // 설비 운용
        public enum enumRunMode { RUN_MODE_STOP = 0, RUN_MODE_START, RUN_MODE_STOPPING, RUN_MODE_PAUSE, RUN_MODE_SETUP, RUN_MODE_INITIALIZE, RUN_MODE_FINAL };
        // 언어 모드
		public enum enumLanguage { LANGUAGE_KOREA = 0, LANGUAGE_CHINA, LANGUAGE_ENGLISH, LANGUAGE_VIETNAM, LANGUAGE_FINAL };
        // Simulation Mode
        public enum enumSimulationMode { SIMULATION_MODE_OFF = 0, SIMULATION_MODE_ON, SIMULATION_MODE_FINAL };
        // 비전 알람 발생 타입
        public enum enumVisionAlarmType
        {
            NONE = 0,
            GRAB_FAIL,
            PMS_IMAGE_SETTING_FAIL,
            POSITION_FIND_FAIL,
            VIDI_INSPECTION_FAIL,
            DIMENSION_INSPECTION_FAIL,
            HEIGHT_OVER,
            INSPECTION_POSITION_OVER_RANGE,
            WELD_WIDTH_OVER,
            INTERFACE_NG,
        }
        // 설비 종류
		public enum enumMachineType { PROCESS_60 = 0, PROCESS_110, PROCESS_150, MACHINE_TYPE_FINAL };
        // 설비에 PC가 2개 설치됨, A,B로 구분짓고, PLC주소만 틀림
        public enum enumMachinePosition {  POSITION_A, POSITION_B };
        // 공장타입이 다름. PLC로그 남기는것 때문에 추가
        public enum enumFactoryType { FACTORY_A, FACTORY_B };
        // 얼라인제품 수량
        // 카메라
		public enum enumCamera { CAMERA_1 = 0, CAMERA_FINAL };
		// 카메라 ( Loader )
        public enum enumTrigger { TRIGGER_OFF = 0, TRIGGER_ON, TRIGGER_FINAL };
        // 라이브 모드
        public enum enumLiveMode { LIVE_MODE_OFF = 0, LIVE_MODE_ON, LIVE_MODE_FINAL };
		// 조명 채널
		public enum enumLIghtChannel { CHANNEL_1 = 0, CHANNEL_2, CHANNEL_3, CHANNEL_4, CHANNEL_FINAL };
        // 비전 조명 컨트롤러
        public enum enumLightController { LIGHT_CONTROLLER_MAIN = 0, LIGHT_CONTROLLER_FINAL };
        // 설비 프로그램 정상 종료
        public enum enumProgramExitStatus { PROGRAM_EXIT_STATUS_OFF = 0, PROGRAM_EXIT_STATUS_ON };
        // 유저 권한 레벨
        public enum enumUserAuthorityLevel { USER_AUTHORITY_LEVEL_OPERATOR = 0, USER_AUTHORITY_LEVEL_ENGINEER, USER_AUTHORITY_LEVEL_MASTER, USER_AUTHORITY_LEVEL_FINAL };
        // 로그인 결과
        public enum enumLoginResult { TIMEOUT_FAIL = 0, ID_FAIL, PASSWORD_FAIL, SUCCESS, LOGIN_RESULT_FINAL };
		// 저장 이미지 타입
        public enum enumSaveImageType { TYPE_BMP = 0, TYPE_JPG };
		// 검사 결과
        public enum enumResult { RESULT_NG = 0, RESULT_OK, RESULT_FINAL };
        // 검사타입
        public enum enumInspectionType {  TYPE_VIDI, TYPE_MEASURE };
        // 저장이미지 타입
        public enum enumImage { IMAGE_ORIGIN, IMAGE_PMS, IMAGE_VIDI, IMAGE_DIMENSION, IMAGE_FINAL };

        // jht 테스트입니다
        // 툴 블록 Input
        public enum enumToolBlockInput { INPUT_1 = 0, INPUT_2, INPUT_3, INPUT_4, INPUT_5, INPUT_6, TOOL_BLOCK_INPUT_FINAL };
		// 툴 블록 Output
		public enum enumToolBlockOutput { OUTPUT_1 = 0, OUTPUT_2, OUTPUT_3, OUTPUT_4, OUTPUT_5,/* OUTPUT_6, OUTPUT_7, OUTPUT_8, OUTPUT_9, OUTPUT_10, OUTPUT_11, OUTPUT_12, */TOOL_BLOCK_OUTPUT_FINAL };
        
        /////PLC 통신 관련
        // 비전 상태 정의
        public enum enumVisionStatus { STATUS_IDLE = 0, STATUS_BUSY };
        // PLC 상태정의
        public enum enumPLCStatus { REQUEST_IDLE = 0, REQUEST_COMPLETE };
        // COMPLETE
        public enum enumComplete {  COMPLETE_OFF = 0, COMPLETE_ON };
        // 비전 READY 정의
        public enum enumVisionReady { VISION_READY_OFF = 0, VISION_READY_ON };
        // 비전 레시피 상태 정의
        public enum enumVisionRecipe { RECIPE_CHANGE = 0, RECIPE_COMPLETE };
        public enum enumVisionRecipeStatus { RECIPE_COMPLETE, RECIPE_CHANG_FAIL };
        //PLC Input영역 인덱스
        public enum enumPLCInputIndex { PLC_MODEL_CHANGE = 0, PLC_INSPECTION_START, PLC_INSPECTION_INDEX, RESERVE, PLC_CELL_ID_1, PLC_CELL_ID_2, PLC_CELL_ID_10 = 13, PLC_FINAL = 16 };
        public enum enumPCOutIndex { PC_VISION_ALIVE = 0, PC_AUTO_RUN, PC_BUSY, PC_COMPLETE, PC_INSPECTION_OK, PC_INSPECTION_NG, PC_INSPECTION_ALARM_CODE, PC_JOB_LOAD_FAILED = 11, PC_INSPECTION_RESULT_1, PC_INSPECTION_RESULT_88 = 99, PC_FINAL = 100 };
        // 결과인덱스 검사1포지션당 5개의 데이터 사용
        public enum enumResultIndex { RESULT_1 = 0, RESULT_5 = 4, RESULT_FINAL };

        // 멀티그랩 이미지 인덱스
        public enum enumMultiGrabIndex {  GRAB_1 = 0, GRAB_2, GRAB_3, GRAB_4, GRAB_FINAL};
        // 이미지 자르기( 가로, 세로 )
        public enum enumCameraCropType { CROP_NONE, CROP_WIDTH, CROP_HEIGHT };
        // 이미지 합성타입
        public enum enumPMSImageType { PMS_TYPE_NORM = 0, PMS_TYPE_ALBEDO, PMS_TYPE_P, PMS_TYPE_Q, PMS_TYPE_MAX };
        // 3D센서 이미지 타입
        public enum enum3DImageType { IMAGE_HEIGHT = 0, IMAGE_INTENSITY, TYPE_FINAL };
        // 이미지 그랩시 90도 회전여부
        public enum enumImageRotation { ROTATION_0, ROTATION_90, ROTATION_270 };
        // VIDI 툴 타입
        public enum enumVidiType {  RED = 0, GREEN, BLUE, TYPE_FINAL };
        // 카메라 타입
        public enum enumCameraType { CAMERA_AREA, CAMERA_3D };
        // 모니터 갯수
        public enum enumMonitor { MONITOR_1 = 0, MONITOR_2, MONITOR_FINAL };
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LOG 사용을 위한 구조체 및 기타
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const int WM_COPYDATA = 0x004A;
		public const int WM_DESTROY = 0x0010;
        public const string DEF_SMART_LOG = "SmartLog";
        public const string DEF_LOG_PROGRAM = "SMART LOG";
        public struct COPYDATA_STRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

		// 보정량 데이터 구조체
		public struct structureRevisionData
		{
			public double dX;
			public double dY;
			public double dT;
		}

        // 로그 종류
        public enum enumLogType
        {
            LOG_SYSTEM, 
            LOG_PROCESS, 
            LOG_BUTTON_OPERATION, 
            LOG_CONFIG_DATA, 
            LOG_VISION_POSITION_CAMERA_0, 
            LOG_VISION_RESULT_STAGE_0,
            LOG_VISION_TACT_TIME_CAMERA_0,
            LOG_VISION_INTERFACE_CAMERA_0,
            LOG_VISION_PROCESS_CAMERA_0,
            LOG_VISION_EXCEPTION_CAMERA_0,
            LOG_VISION_EXCEPTION_SYSTEM, 
            LOG_ETC,
            LOG_RESULT_VIDI,
            LOG_RESULT_MEASURE,
            LOG_RESULT_MEASURE_3D,
            LOG_RESULT_MEASURE_3D_WELD_WIDTH,
            LOG_RESULT_MEASURE_3D_HEIGHT,
            LOG_MEASURE_3D_HEIGHT_SAMPLE,
            LOG_MEASURE_3D_HEIGHT_MIN_MAX,
            LOG_FINAL
        };

        // 로그 구조체
        [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Unicode )]
        public struct structureLog
        {
            public int iLogType;
            
            [System.Runtime.InteropServices.MarshalAs( System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 4096 )]
            public string strData;
            
            //public char[] szLogDate;
        }
		// 알람 구조체
		public struct structureAlarmInformation
		{
			// Alarm 레벨
			public enumAlarmType eAlarmLevel;
			// Alarm 코드
			public int iAlarmCode;
			// 알람 객체
			public string strAlarmObject;
			// 알람 함수
			public string strAlarmFunction;
			// 알람 설명
			public string strAlarmDescription;
		}
		// 레포트 디스플레이 구조체
		public struct structureReportImage
		{
			// 성공여부
			public bool bResult;
			// 이미지 주소
			public string strImagePath;
            //스코어
            public string strScore;
            // 사각형
            public double dStartX;
            public double dStartY;
            public double dEndX;
            public double dEndY;

            public int iFindLineCount;
            public double dLineDistance;
            public double dPatternPositionX;
            public double dPatternPositionY;

            public int iInspectionIndex;
        }
        // 권한 레벨 구조체
        public struct structureAuthorityLevel
        {
            // 편집 가능 권한
            public CDefine.enumUserAuthorityLevel eLevelWrite;
            // 폼 메인 페이지를 볼 수 있는 권한
            public CDefine.enumUserAuthorityLevel eLevelRead;
        }
        // 권한 파라미터 설정 조작 못하게.
        public class CAuthorityParameter
        {
            // 폼 권한 레벨
            private structureAuthorityLevel[] objLevelForm;
            public structureAuthorityLevel[] m_objLevelForm { get { return objLevelForm; } }
            // 언어 버튼 권한 레벨
            private structureAuthorityLevel objLevelLanguage;
            public structureAuthorityLevel m_objLevelLanguage { get { return objLevelLanguage; } }
            // 종료 권한 레벨
            private structureAuthorityLevel objLevelExit;
            public structureAuthorityLevel m_objLevelExit { get { return objLevelExit; } }

            public CAuthorityParameter()
            {
                // 아무 곳에서도 못 바꾸게 하드 코딩.
                objLevelForm = new structureAuthorityLevel[ ( int )FormView.FORM_VIEW_FINAL ];
                objLevelForm[ ( int )FormView.FORM_VIEW_MAIN ] = new structureAuthorityLevel();
                objLevelForm[ ( int )FormView.FORM_VIEW_MAIN ].eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelForm[ ( int )FormView.FORM_VIEW_MAIN ].eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelForm[ ( int )FormView.FORM_VIEW_SETUP ] = new structureAuthorityLevel();
                objLevelForm[ ( int )FormView.FORM_VIEW_SETUP ].eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
                objLevelForm[ ( int )FormView.FORM_VIEW_SETUP ].eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
                objLevelForm[ ( int )FormView.FORM_VIEW_CONFIG ] = new structureAuthorityLevel();
                objLevelForm[ ( int )FormView.FORM_VIEW_CONFIG ].eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
                objLevelForm[ ( int )FormView.FORM_VIEW_CONFIG ].eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
                objLevelForm[ ( int )FormView.FORM_VIEW_IO ] = new structureAuthorityLevel();
                objLevelForm[ ( int )FormView.FORM_VIEW_IO ].eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
                objLevelForm[ ( int )FormView.FORM_VIEW_IO ].eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelForm[ ( int )FormView.FORM_VIEW_REPORT ] = new structureAuthorityLevel();
                objLevelForm[ ( int )FormView.FORM_VIEW_REPORT ].eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelForm[ ( int )FormView.FORM_VIEW_REPORT ].eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelLanguage = new structureAuthorityLevel();
                objLevelLanguage.eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelLanguage.eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                objLevelExit = new structureAuthorityLevel();
                objLevelExit.eLevelWrite = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
                objLevelExit.eLevelRead = enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
            }
        }
    }
}




