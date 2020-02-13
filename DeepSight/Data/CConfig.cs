using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DeepSight
{
	public class CConfig
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//structure
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 시스템 파라미터 ( Config )
		public class CSystemParameter : ICloneable
		{
			// 아이템 폴더 경로
			public string strItemPath;
			// 레시피 폴더 경로
			public string strRecipePath;
			// 레시피 아이디
			public string strRecipeID;
			// 설비 타입
			public CDefine.enumMachineType eMachineType;
            // 공장 타입
            public CDefine.enumFactoryType eFactoryType;
            // 150공정 카메라가 고케이터인 경우
            public CDefine.enumCameraType eCameraType;
            // 설비별 포지션
            public CDefine.enumMachinePosition eMachinePosition;
            // 포지션에따른 PLC주소 옵셋
            public int iOffsetAddressPLC;
			// 시뮬레이션 모드
			public CDefine.enumSimulationMode eSimulationMode;
            //PLC시뮬레이션모드
            public CDefine.enumSimulationMode eSimulationModePLC;
            // 개발피씨 티칭모드( 비디 이미지 취득위해 )
            public bool bVidiTeachMode;
			// 언어 설정
			public CDefine.enumLanguage eLanguage;
			// 이미지 저장 여부
			public bool bImageSave;
			// 이미지 저장 날짜
			public int iPeriodImage;
			// 데이터베이스 저장 날짜
			public int iPeriodDatabase;
			// 이미지 저장 경로
			public string strImageSavePath;
			// 이미지 저장 타입
			public CDefine.enumSaveImageType eSaveImageType;
            // 패스모드
            public bool bPassMode;
            // 결과 다이얼로그 표시
            public bool bUseResultDialog;
            // 비전 자동레시피 변경여부
            public bool bUseAutoRecipeChange;
            //시뮬레이션이미지폴더
            public string strSimulationImagePath;

            // 타이틀 표시
            public string strTitleProcess60;
            public string strTitleProcess110;
            public string strTitleProcess150;
            public CSystemParameter()
			{
				strItemPath = "";
				strRecipePath = "";
                strSimulationImagePath = "";
                strRecipeID = "";
                strTitleProcess60 = "";
                strTitleProcess110 = "";
                strTitleProcess150 = "";
                bPassMode = false;
                bUseResultDialog = false;
                bUseAutoRecipeChange = false;
                eMachineType = CDefine.enumMachineType.PROCESS_60;
                eFactoryType = CDefine.enumFactoryType.FACTORY_A;
                eCameraType = CDefine.enumCameraType.CAMERA_AREA;
                eMachinePosition = CDefine.enumMachinePosition.POSITION_A;
                iOffsetAddressPLC = 200;
                eSimulationMode = 0;
                eSimulationModePLC = 0;
                bVidiTeachMode = false;
                eLanguage = 0;
				bImageSave = false;
				iPeriodImage = 0;
				iPeriodDatabase = 0;
				strImageSavePath = "";
				eSaveImageType = CDefine.enumSaveImageType.TYPE_BMP;
			}

			public object Clone()
			{
				CSystemParameter objSystemParameter = new CSystemParameter();
				objSystemParameter.strItemPath = this.strItemPath;
				objSystemParameter.strRecipePath = this.strRecipePath;
                objSystemParameter.strSimulationImagePath = this.strSimulationImagePath;
                objSystemParameter.strRecipeID = this.strRecipeID;
                objSystemParameter.strTitleProcess60 = this.strTitleProcess60;
                objSystemParameter.strTitleProcess110 = this.strTitleProcess110;
                objSystemParameter.strTitleProcess150 = this.strTitleProcess150;
                objSystemParameter.bPassMode = this.bPassMode;
                objSystemParameter.bUseResultDialog = this.bUseResultDialog;
                objSystemParameter.bUseAutoRecipeChange = this.bUseAutoRecipeChange;
                objSystemParameter.eMachineType = this.eMachineType;
                objSystemParameter.eFactoryType = this.eFactoryType;
                objSystemParameter.eCameraType = this.eCameraType;
                objSystemParameter.eMachinePosition = this.eMachinePosition;
                objSystemParameter.iOffsetAddressPLC = this.iOffsetAddressPLC;
                objSystemParameter.eSimulationMode = this.eSimulationMode;
                objSystemParameter.eSimulationModePLC = this.eSimulationModePLC;
                objSystemParameter.bVidiTeachMode = this.bVidiTeachMode;
                objSystemParameter.eLanguage = this.eLanguage;
				objSystemParameter.bImageSave = this.bImageSave;
				objSystemParameter.iPeriodImage = this.iPeriodImage;
				objSystemParameter.iPeriodDatabase = this.iPeriodDatabase;
				objSystemParameter.strImageSavePath = this.strImageSavePath;
				objSystemParameter.eSaveImageType = this.eSaveImageType;
				return objSystemParameter;
			}
		};

		// 데이터베이스 파라미터 ( Config )
		public class CDatabaseParameter : ICloneable
		{
			// Database
			// Database History
			public string strDatabaseHistory;
			// Database Information
			public string strDatabaseInformation;
			// Table
			// Table Information UI Text
			public string strTableInformationUIText;
			// Table Information User Message
			public string strTableInformationUserMessage;
			// Table History Align
			public string strTableHistoryAlign;
			// Record
			// Record Information UI Text
			public string strRecordInformationUIText;
			// Record Information User Message
			public string strRecordInformationUserMessage;
			// Delete
			// Delete Period Align
			public int iDeletePeriodAlign;

			public CDatabaseParameter()
			{
				// Database
				strDatabaseHistory = "";
				strDatabaseInformation = "";
				// Table
				strTableInformationUIText = "";
				strTableInformationUserMessage = "";
				strTableHistoryAlign = "";
				// Record
				strRecordInformationUIText = "";
				strRecordInformationUserMessage = "";
				// Delete
				iDeletePeriodAlign = 0;
			}

			public object Clone()
			{
				CDatabaseParameter objDatabaseParameter = new CDatabaseParameter();
				// Database
				objDatabaseParameter.strDatabaseHistory = this.strDatabaseHistory;
				objDatabaseParameter.strDatabaseInformation = this.strDatabaseInformation;
				// Table
				objDatabaseParameter.strTableInformationUIText = this.strTableInformationUIText;
				objDatabaseParameter.strTableInformationUserMessage = this.strTableInformationUserMessage;
				objDatabaseParameter.strTableHistoryAlign = this.strTableHistoryAlign;
				// Record
				objDatabaseParameter.strRecordInformationUIText = this.strRecordInformationUIText;
				objDatabaseParameter.strRecordInformationUserMessage = this.strRecordInformationUserMessage;
				// Delete
				objDatabaseParameter.iDeletePeriodAlign = this.iDeletePeriodAlign;
				return objDatabaseParameter;
			}
		}

		// 레시피 정보 ( Recipe )
		public class CRecipeInformation : ICloneable
		{
			// 레시피 아이디
			public string strRecipeID;
			// 레시피 이름
			public string strRecipeName;

			public CRecipeInformation()
			{
				strRecipeID = "";
				strRecipeName = "";
			}

			public object Clone()
			{
				CRecipeInformation objRecipeInformation = new CRecipeInformation();
				objRecipeInformation.strRecipeID = this.strRecipeID;
				objRecipeInformation.strRecipeName = this.strRecipeName;
				return objRecipeInformation;
			}
		}


		// 카메라 설정
		public struct structureCameraConfig : ICloneable
		{
			// X 반전
			public bool bReverseX;
			// Y 반전
			public bool bReverseY;
			// 90 도 회전
			public bool bRotation90;
			// 180 도 회전
			public bool bRotation180;
			// 270 되 회전
			public bool bRotation270;
			// 게인
			public double dGain;
			// 노출
			public double dExposureTime;
			// 카메라 가로 픽셀
			public int iCameraWidth;
			// 카메라 세로 픽셀
			public int iCameraHeight;
			// 카메라 X Offset
			public int iCameraXOffset;
			// 카메라 Y Offset
			public int iCameraYOffset;
			// FrameRate
			public double dFrameRate;

			public object Clone()
			{
				structureCameraConfig objCameraConfig = new structureCameraConfig();
				objCameraConfig.bReverseX = this.bReverseX;
				objCameraConfig.bReverseY = this.bReverseY;
				objCameraConfig.bRotation90 = this.bRotation90;
				objCameraConfig.bRotation180 = this.bRotation180;
				objCameraConfig.bRotation270 = this.bRotation270;
				objCameraConfig.dGain = this.dGain;
				objCameraConfig.dExposureTime = this.dExposureTime;
				objCameraConfig.iCameraWidth = this.iCameraWidth;
				objCameraConfig.iCameraHeight = this.iCameraHeight;
				objCameraConfig.iCameraXOffset = this.iCameraXOffset;
				objCameraConfig.iCameraYOffset = this.iCameraYOffset;
				objCameraConfig.dFrameRate = this.dFrameRate;
				return objCameraConfig;
			}
		}

        // ID리더 검색영역
        // 아씨 오타..ㅡㅡ 고칠수가 없네
        public class CSerchRegion : ICloneable
        {
            public double dStartX;
            public double dStartY;
            public double dEndX;
            public double dEndY;
            public double dRotation;
            public double dSkew;

            public CVidiParameter objVidiParameter;

            public CSerchRegion()
            {
                dStartX = 0; dStartY = 0; dEndX = 0; dEndY = 0; dRotation = 0; dSkew = 0;
                objVidiParameter = new CVidiParameter();
            }
            public object Clone()
            {
                CSerchRegion obj = new CSerchRegion();
                obj.objVidiParameter = this.objVidiParameter.Clone() as CVidiParameter;
                obj.dStartX = this.dStartX;
                obj.dStartY = this.dStartY;
                obj.dEndX = this.dEndX;
                obj.dEndY = this.dEndY;
                obj.dRotation = this.dRotation;
                obj.dSkew = this.dSkew;
                return obj;
            }
        }

        // 비디파라미터
        // 각 검사 영역마다 싹다 쓰기때문에 여러개 생성
        public class CVidiParameter : ICloneable
        {
            // 워크스페이스 이름 및 경로는 하나만 필요하기때문에 이건 RecipeParmater에 대표로 따로 생성해서 연결만 시켜줌
            public string strWorkSpaceFilePath;
            public string strWorkSpaceName;

            public string strStreamName;
            public string strToolName;
            public CDefine.enumVidiType eVidiType;
            public CVidiParameter()
            {
                strWorkSpaceFilePath = "";
                strWorkSpaceName = "";
                strStreamName = "";
                strToolName = "";
                eVidiType = CDefine.enumVidiType.RED;
            }
            public object Clone()
            {
                CVidiParameter obj = new CVidiParameter();
                obj.strWorkSpaceFilePath = this.strWorkSpaceFilePath;
                obj.strWorkSpaceName = this.strWorkSpaceName;
                obj.strStreamName = this.strStreamName;
                obj.strToolName = this.strToolName;
                obj.eVidiType = this.eVidiType;
                return obj;
            }
        }

        public class CGrabParameter : ICloneable
        {
            public int iCropSize;
            public CDefine.enumCameraCropType eCropType;
            public double dCameraExpouseTime;
            public double[] dLightValue;
            
            public CGrabParameter()
            {
                iCropSize = 0;
                eCropType = CDefine.enumCameraCropType.CROP_NONE;
                dCameraExpouseTime = 10000;
                dLightValue = new double[ ( int )CDefine.enumMultiGrabIndex.GRAB_FINAL ];
                for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumMultiGrabIndex.GRAB_FINAL; iLoopCount++ )
                {
                    dLightValue[ iLoopCount ] = 10;
                }
            }

            public object Clone()
            {
                CGrabParameter obj = new CGrabParameter();
                obj.iCropSize = iCropSize;
                obj.eCropType = eCropType;
                obj.dCameraExpouseTime = dCameraExpouseTime;
                obj.dLightValue = dLightValue.Clone() as double[];
                return obj;
            }
        }

        public class CMeasureParameter : ICloneable {
            public int iBusbarLRLowThresh;
            public int iBusbarLRContinousLen;
            public int iBusbarLRHighThresh;
            public int iBusbarTBThresh;
            public int iBusbarTBContinousLen;
            //센서
            public int iSensorStdWid;
            public int iSensorStdHgt;
            public int iSensorThresh;

            public double dSizeWidthMax;
            public double dSizeWidthMin;
            public double dSizeHeightMax;
            public double dSizeHeightMin;
            public CMeasureParameter()
            {
                iBusbarLRLowThresh = 0;
                iBusbarLRContinousLen = 0;
                iBusbarLRHighThresh = 0;
                iBusbarTBThresh = 0;
                iBusbarTBContinousLen = 0;

                iSensorStdWid = 0;
                iSensorStdHgt = 0;
                iSensorThresh = 0;

                dSizeWidthMax = 0;
                dSizeWidthMin = 0;
                dSizeHeightMax = 0;
                dSizeHeightMin = 0;
            }

            public object Clone()
            {
                CMeasureParameter obj = new CMeasureParameter();
                obj.iBusbarLRLowThresh = iBusbarLRLowThresh;
                obj.iBusbarLRContinousLen = iBusbarLRContinousLen;
                obj.iBusbarLRHighThresh = iBusbarLRHighThresh;
                obj.iBusbarTBThresh = iBusbarTBThresh;
                obj.iBusbarTBContinousLen = iBusbarTBContinousLen;

                obj.iSensorStdWid = iSensorStdWid;
                obj.iSensorStdHgt = iSensorStdHgt;
                obj.iSensorThresh = iSensorThresh;

                obj.dSizeWidthMax = dSizeWidthMax;
                obj.dSizeWidthMin = dSizeWidthMin;
                obj.dSizeHeightMax = dSizeHeightMax;
                obj.dSizeHeightMin = dSizeHeightMin;

                return obj;
            }
        }

        public class CFindLineParameter : ICloneable
        {
            public enum enumPolaraty { DarkToLight = 1, LightToDark, DontCare };
            public enum enumSerarchDirection{ DIRECTION_90 = 0, DIRECTION_180 };
            public enumPolaraty ePolaraty;
            public enumSerarchDirection eSerachDirection;
            public int iCalipersNumber;
            public int iIgnoreNumber;
            public int iThreshold;
            public int iFilter;
            public double dSearchLength;

            public CFindLineParameter()
            {
                ePolaraty = enumPolaraty.LightToDark;
                eSerachDirection = enumSerarchDirection.DIRECTION_90;
                iCalipersNumber = 10;
                iIgnoreNumber = 3;
                iThreshold = 10;
                iFilter = 2;
                dSearchLength = 150;
            }

            public object Clone()
            {
                CFindLineParameter obj = new CFindLineParameter();
                obj.ePolaraty = ePolaraty;
                obj.eSerachDirection = eSerachDirection;
                obj.iCalipersNumber = iCalipersNumber;
                obj.iIgnoreNumber = iIgnoreNumber;
                obj.iThreshold = iThreshold;
                obj.iFilter = iFilter;
                obj.dSearchLength = dSearchLength;
                return obj;
            }
        }
        /// <summary>
        /// 검사파리미터
        /// 검사포인트 -> 크롭(n개)
        /// </summary>
        public class CInspectionParameter : ICloneable
        {
            // 검사 이미지 타입
            
            public CDefine.enumPMSImageType ePMSImageTypeVIDI;
            public CDefine.enumPMSImageType ePMSImageTypeMeasure;
            // 검색영역 갯수
            public int iCountSerchRegion;
            // VIDI 스코어 셋팅
            public double dVidiScore;
            // 비디검색영역
            public CSerchRegion[] objVidiSerchRegion;
            // 치수측정 검색영역
            public CSerchRegion[] objMeasureSerchRegion;
            // 3D센서에서 사용할 기울기 영역
            public CSerchRegion[] objGradientRegion;
            // 3D센서에서 폭측정에 사용되는 라인파라미터
            public CFindLineParameter[] objFindLineTop;
            public CFindLineParameter[] objFindLineBottom;
            // 치수측정에 사용되는 파라미터
            public CMeasureParameter objMeasureParameter;
            //3D 관련은 그냥 쫙 넣어버리자 여기에.....
            public CDefine.enum3DImageType e3DImageTypeVIDI;
            public CDefine.enum3DImageType e3DImageTypeMeasure;
            public int i3DScanLength;
            public double d3DScanWidth;
            public CDefine.enumImageRotation e3DImageRotation;

            //3D 센서 상하한값 
            public double d3DHeightThresholdMax;
            public double d3DHeightThresholdMin;
            public int i3DHeightThresholdCount;
            public double d3DWeldWidthMax;
            public double d3DWeldWidthMin;
            //public CSerchRegion[] objMeasureSerchRegion;

            // 조명파라미터
            public CGrabParameter objGrabParameter;
            public CGrabParameter objGrabParameterMeasure;
            // 비디관련 INI를 하나로 통합하고 싶었으나 분리 되어야함
            // 비디 워크스페이스 이름은 최상위 1개
            // 비디 스트림은 각 검사포지션 마다
            // 비디 툴 이름은 검사포지션->크롭되는 검사영역으로 분리
            // 비디 스트림
            // 검색영역사이즈는 고정이 될수 있음
            public double dVidiFixedSizeWidth;
            public double dVidiFixedSizeHeight;
            public double dMeasureFixedSizeWidth;
            public double dMeasureFixedSizeHeight;
            
            public CInspectionParameter()
            {
                e3DImageTypeVIDI = CDefine.enum3DImageType.IMAGE_HEIGHT;
                e3DImageTypeMeasure = CDefine.enum3DImageType.IMAGE_HEIGHT;
                e3DImageRotation = CDefine.enumImageRotation.ROTATION_0;
                ePMSImageTypeVIDI = CDefine.enumPMSImageType.PMS_TYPE_ALBEDO;
                ePMSImageTypeMeasure = CDefine.enumPMSImageType.PMS_TYPE_ALBEDO;
                objGrabParameter = new CGrabParameter();
                objGrabParameterMeasure = new CGrabParameter();
                objMeasureParameter = new CMeasureParameter();
                i3DScanLength = 0;
                d3DScanWidth = 0;
                d3DHeightThresholdMax = 1;
                d3DHeightThresholdMin = 0;
                i3DHeightThresholdCount = 100;
                d3DWeldWidthMax = 2.5;
                d3DWeldWidthMin = 1.5;
                iCountSerchRegion = 3;
                dVidiScore = 1;
                dVidiFixedSizeWidth = 600;
                dVidiFixedSizeHeight = 300;
                dMeasureFixedSizeWidth = 600;
                dMeasureFixedSizeHeight = 300;
                objVidiSerchRegion = new CSerchRegion[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                objMeasureSerchRegion = new CSerchRegion[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                objGradientRegion = new CSerchRegion[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                objFindLineTop = new CFindLineParameter[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                objFindLineBottom = new CFindLineParameter[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                
                for ( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ )
                {
                    objVidiSerchRegion[ iLoopCount ] = new CSerchRegion();
                    objMeasureSerchRegion[ iLoopCount ] = new CSerchRegion();
                    objGradientRegion[ iLoopCount ] = new CSerchRegion();
                    objFindLineTop[ iLoopCount ] = new CFindLineParameter();
                    objFindLineBottom[ iLoopCount ] = new CFindLineParameter();
                }
            }
            public object Clone()
            {
                CInspectionParameter obj = new CInspectionParameter();
                obj.e3DImageTypeVIDI = this.e3DImageTypeVIDI;
                obj.e3DImageTypeMeasure = this.e3DImageTypeMeasure;
                obj.e3DImageRotation = this.e3DImageRotation;
                obj.i3DScanLength = this.i3DScanLength;
                obj.d3DScanWidth = this.d3DScanWidth;
                obj.d3DHeightThresholdMax = this.d3DHeightThresholdMax;
                obj.d3DHeightThresholdMin = this.d3DHeightThresholdMin;
                obj.i3DHeightThresholdCount = this.i3DHeightThresholdCount;
                obj.d3DWeldWidthMax = this.d3DWeldWidthMax;
                obj.d3DWeldWidthMin = this.d3DWeldWidthMin;
                obj.ePMSImageTypeVIDI = this.ePMSImageTypeVIDI;
                obj.ePMSImageTypeMeasure = this.ePMSImageTypeMeasure;
                obj.objGrabParameter = this.objGrabParameter.Clone() as CGrabParameter;
                obj.objGrabParameterMeasure = this.objGrabParameterMeasure.Clone() as CGrabParameter;
                obj.objMeasureParameter= this.objMeasureParameter.Clone() as CMeasureParameter;
                obj.iCountSerchRegion = this.iCountSerchRegion;
                obj.dVidiScore = this.dVidiScore;
                obj.dVidiFixedSizeWidth = this.dVidiFixedSizeWidth;
                obj.dVidiFixedSizeHeight = this.dVidiFixedSizeHeight;
                obj.dMeasureFixedSizeWidth = this.dMeasureFixedSizeWidth;
                obj.dMeasureFixedSizeHeight = this.dMeasureFixedSizeHeight;
                
                for ( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ )
                {
                    obj.objVidiSerchRegion[ iLoopCount ] = this.objVidiSerchRegion[ iLoopCount ].Clone() as CSerchRegion;
                    obj.objMeasureSerchRegion[ iLoopCount ] = this.objMeasureSerchRegion[ iLoopCount ].Clone() as CSerchRegion;
                    obj.objGradientRegion[ iLoopCount ] = this.objGradientRegion[ iLoopCount ].Clone() as CSerchRegion;
                    obj.objFindLineTop[ iLoopCount ] = this.objFindLineTop[ iLoopCount ].Clone() as CFindLineParameter;
                    obj.objFindLineBottom[ iLoopCount ] = this.objFindLineBottom[ iLoopCount ].Clone() as CFindLineParameter;
                }
                
                return obj;
            }
        }

        // 레시피 파라미터 ( Recipe )
        public class CRecipeParameter : ICloneable
		{
			// 카메라 설정
			public structureCameraConfig objCameraConfig;
            // 검사 포인트 수
            public int iCountInspectionPosition;
            // 검사파라미터
            public CInspectionParameter[] objInspectionParameter;
            // 비디 워크스페이스 이름 및 경로
            public string strVidiWorkSpaceFilePath;
            public string strVidiWorkSpaceName;

            public CRecipeParameter()
			{
				objCameraConfig = new structureCameraConfig();
                strVidiWorkSpaceFilePath = "";
                strVidiWorkSpaceName = "";
                iCountInspectionPosition = 3;

                objInspectionParameter = new CInspectionParameter[ CDefine.DEF_MAX_COUNT_INSPECTION_POSITION ];
                for ( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopCount++ )
                {
                    objInspectionParameter[ iLoopCount ] = new CInspectionParameter();
                }
            }

			public object Clone()
			{
				CRecipeParameter objRecipeParameter = new CRecipeParameter();
				objRecipeParameter.objCameraConfig = ( structureCameraConfig )this.objCameraConfig.Clone();
                objRecipeParameter.strVidiWorkSpaceFilePath = this.strVidiWorkSpaceFilePath;
                objRecipeParameter.strVidiWorkSpaceName = this.strVidiWorkSpaceName;
                objRecipeParameter.iCountInspectionPosition = this.iCountInspectionPosition;

                for ( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopCount++ )
                {
                    objRecipeParameter.objInspectionParameter[ iLoopCount ] = this.objInspectionParameter[ iLoopCount ].Clone() as CInspectionParameter;
                }
                return objRecipeParameter;
			}
		}

		// 카메라 파라미터 ( Camera )
		public class CCameraParameter : ICloneable
		{
			// 사용 카메라 타입
			public enum enumUseCameraType { USE_REAL_CAMERA_TYPE = 0, USE_VIRTUAL_CAMERA_TYPE };
			// 카메라 타입 ( 실제 카메라, 가상 카메라 )
			public enumUseCameraType eUseCameraType;
			// 카메라 인덱스
			public int iCameraIndex;
			// 카메라 시리얼 번호
			public string strCameraSerialNumber;
            // IP주소
            public string strIPAddress;
			// 카메라 픽셀 해상도
			public double dResolution;
			
			public CCameraParameter()
			{
				eUseCameraType = enumUseCameraType.USE_REAL_CAMERA_TYPE;
				iCameraIndex = 0;
				strCameraSerialNumber = "";
                strIPAddress = "";
				dResolution = 0.0;
			}

			public object Clone()
			{
				CCameraParameter objCameraParameter = new CCameraParameter();
				objCameraParameter.eUseCameraType = this.eUseCameraType;
				objCameraParameter.iCameraIndex = this.iCameraIndex;
				objCameraParameter.strCameraSerialNumber = this.strCameraSerialNumber;
                objCameraParameter.strIPAddress = this.strIPAddress;
                objCameraParameter.dResolution = this.dResolution;
				return objCameraParameter;
			}
		}

		// IO 초기화 파라미터 ( Device )
		public class CIOInitializeParameter : ICloneable
		{
			// IO 타입 정의
			public enum enumIOType { IO_TYPE_DI = 0, IO_TYPE_DO, IO_TYPE_AI, IO_TYPE_AO, IO_TYPE_FINAL };
			// IO 파라미터 정의
			public class CIOParameter : ICloneable
			{
				public enumIOType eIOType;
				public string strAddress;
				public string strIOName;
				public string strIndex;
				public int iDeviceCardID;

				public object Clone()
				{
					CIOParameter objIOParameter = new CIOParameter();
					objIOParameter.eIOType = this.eIOType;
					objIOParameter.strAddress = this.strAddress;
					objIOParameter.strIOName = this.strIOName;
					objIOParameter.strIndex = this.strIndex;
					objIOParameter.iDeviceCardID = this.iDeviceCardID;
					return objIOParameter;
				}
			}
			public HLDevice.Abstract.CDeviceIOAbstract.enumDeviceCardType eDeviceCardType;
			// 카드 번호
			public int iCountDeviceCard;
			// Input IO 모듈 갯수
			public int iInputModuleCount;
			// Output IO 모듈 갯수
			public int iOutPutModuleCount;
			// Analog Input 모듈 갯수
			public int iAnalogInputModuleCount;
			// Analog Ouput 모듈 갯수
			public int iAnalogOutputModuleCount;
			// 개벌 IO 정보 map
			public Dictionary<string, CIOParameter> objIOParameter;

			// PLC UI접근
			public List<string> objListIONameInput;
			public List<string> objListIONameOutput;
			public List<string> objListIOAddressInput;
			public List<string> objListIOAddressOutput;

			public CIOInitializeParameter()
			{
				objIOParameter = new Dictionary<string, CIOInitializeParameter.CIOParameter>();
				objListIONameInput = new List<string>();
				objListIONameOutput = new List<string>();
				objListIOAddressInput = new List<string>();
				objListIOAddressOutput = new List<string>();
			}

			public object Clone()
			{
				CIOInitializeParameter objInitializeParameter = new CIOInitializeParameter();
				objInitializeParameter.eDeviceCardType = this.eDeviceCardType;
				objInitializeParameter.iCountDeviceCard = this.iCountDeviceCard;
				objInitializeParameter.iInputModuleCount = this.iInputModuleCount;
				objInitializeParameter.iOutPutModuleCount = this.iOutPutModuleCount;
				objInitializeParameter.iAnalogInputModuleCount = this.iAnalogInputModuleCount;
				objInitializeParameter.iAnalogOutputModuleCount = this.iAnalogOutputModuleCount;
				objInitializeParameter.objListIONameInput = new List<string>( this.objListIONameInput );
				objInitializeParameter.objListIONameOutput = new List<string>( this.objListIONameOutput );
				objInitializeParameter.objListIOAddressInput = new List<string>( this.objListIOAddressInput );
				objInitializeParameter.objListIOAddressOutput = new List<string>( this.objListIOAddressOutput );
				objInitializeParameter.objIOParameter = new Dictionary<string, CIOParameter>( objIOParameter );
				return objInitializeParameter;
			}
		}

		// PLC 초기화 파라미터 ( Device )
		public class CPLCInitializeParameter : ICloneable
		{
			public class CPLCParameter : ICloneable
			{
				public enum enumPLCCommunicationType { TYPE_BIT_IN, TYPE_WORD_IN, TYPE_DWORD_IN, TYPE_BIT_OUT, TYPE_WORD_OUT, TYPE_DWORD_OUT };
				public enumPLCCommunicationType eCommunicationType;
				public string strAddress;
				public string strPLCName;
				public string strIndex;
				public int iInOutIndex;

				public object Clone()
				{
					CPLCParameter objPLCParameter = new CPLCParameter();
					objPLCParameter.eCommunicationType = this.eCommunicationType;
					objPLCParameter.strAddress = this.strAddress;
					objPLCParameter.strPLCName = this.strPLCName;
					objPLCParameter.strIndex = this.strIndex;
					objPLCParameter.iInOutIndex = this.iInOutIndex;
					return objPLCParameter;
				}
			}

			public enum enumPLCProtocolType { TYPE_BINARY, TYPE_ASCII, TYPE_FINAL };
            public enum enumPLCType { TYPE_PLC_Q, TYPE_PLC_R, TYPE_FINAL };
            // PLC 연결 타입
            public enumPLCProtocolType ePLCProtocolType;
            public enumPLCType ePLCType;
            // 주소
            public string strSocketIPAddress;
			// 포트
			public int iSocketPortNumber;
			// PLC 접근 주소 객체
			public Dictionary<string, CPLCParameter> objPLCParameter;
			// PLC IN, OUT Count ALL 채널
			public int iOutputCountAll;
			public int iInputCountAll;

			// PLC IN, OUT Count Bit 채널
			public int iOutputCountBit;
			public int iInputCountBit;

			// PLC IN, OUT Count DWord 채널
			public int iOutputCountWord;
			public int iInputCountWord;

			// PLC IN, OUT Count DWord 채널
			public int iOutputCountDWord;
			public int iInputCountDWord;

			// PLC UI접근
			public List<string> strPLCInput;
			public List<string> strPLCOutput;
			// 더블워드 Read/Write시 상수값
			public double dMultiple;

			public CPLCInitializeParameter()
			{
				objPLCParameter = new Dictionary<string, CPLCInitializeParameter.CPLCParameter>();
				strPLCInput = new List<string>();
				strPLCOutput = new List<string>();
			}

			public object Clone()
			{
				CPLCInitializeParameter objInitializeParameter = new CPLCInitializeParameter();
				objInitializeParameter.strSocketIPAddress = this.strSocketIPAddress;
				objInitializeParameter.iSocketPortNumber = this.iSocketPortNumber;
				objInitializeParameter.ePLCProtocolType = this.ePLCProtocolType;
                objInitializeParameter.ePLCType = this.ePLCType;
                objInitializeParameter.objPLCParameter = new Dictionary<string, CPLCParameter>( this.objPLCParameter );
				objInitializeParameter.strPLCInput = new List<string>( this.strPLCInput );
				objInitializeParameter.strPLCOutput = new List<string>( this.strPLCOutput );
				objInitializeParameter.iInputCountAll = this.iInputCountAll;
				objInitializeParameter.iOutputCountAll = this.iInputCountAll;
				objInitializeParameter.iInputCountBit = this.iInputCountBit;
				objInitializeParameter.iOutputCountBit = this.iOutputCountBit;
				objInitializeParameter.iInputCountWord = this.iInputCountWord;
				objInitializeParameter.iOutputCountWord = this.iOutputCountWord;
				objInitializeParameter.iInputCountDWord = this.iInputCountDWord;
				objInitializeParameter.iOutputCountDWord = this.iOutputCountDWord;
				objInitializeParameter.dMultiple = this.dMultiple;
				return objInitializeParameter;
			}
		}

       

		// 조명 컨트롤러 파라미터 ( Device )
		public class CLightControllerParameter : ICloneable
		{
			// 통신 타입
			public enum enumType { TYPE_SOCKET = 0, TYPE_SERIAL, TYPE_FINAL };
			// 시리얼포트 Parity
			public enum enumSerialPortParity { PARITY_NONE = 0, PARITY_ODD, PARITY_EVEN, PARITY_MARK, PARITY_SPACE };
			// 시리얼포트 StopBits
			public enum enumSerialPortStopBits { STOP_BITS_NONE = 0, STOP_BITS_ONE, STOP_BITS_TWO, STOP_BITS_ONE_POINT_FIVE };
			
			public enumType eType;
			public string strSocketIPAddress;
			public int iSocketPortNumber;

			public string strSerialPortName;
			public int iSerialPortBaudrate;
			public int iSerialPortDataBits;
			public enumSerialPortParity eParity;
			public enumSerialPortStopBits eStopBits;

			public CLightControllerParameter()
			{
				eType = enumType.TYPE_SOCKET;
				strSocketIPAddress = "";
				iSocketPortNumber = 0;
				strSerialPortName = "";
				iSerialPortBaudrate = 0;
				iSerialPortDataBits = 0;
				eParity = enumSerialPortParity.PARITY_NONE;
				eStopBits = enumSerialPortStopBits.STOP_BITS_NONE;
			}
			
			public object Clone()
			{
				CLightControllerParameter objInitializeParameter = new CLightControllerParameter();
				objInitializeParameter.eType = this.eType;
				objInitializeParameter.strSocketIPAddress = this.strSocketIPAddress;
				objInitializeParameter.iSocketPortNumber = this.iSocketPortNumber;

				objInitializeParameter.strSerialPortName = this.strSerialPortName;
				objInitializeParameter.iSerialPortBaudrate = this.iSerialPortBaudrate;
				objInitializeParameter.iSerialPortDataBits = this.iSerialPortDataBits;
				objInitializeParameter.eParity = this.eParity;
				objInitializeParameter.eStopBits = this.eStopBits;
				return objInitializeParameter;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 시스템 파일 경로 ( ex. .\DeepSight\ ~ \Debug )
		// 현재 .exe 파일 있는 경로
		private string m_strCurrentPath;
		// 아이템 파일 경로 ( ex. D:\DeepSight\Item )
		// Item folder [ Database.db3 & VisionCamera.ini & VisionDevice.ini ]
		private string m_strItemPath;
		// 레시피 파일 경로 ( ex. D:\DeepSight_Recipe )
		// Recipe folder [ vpp & VisionEasyAlign.ini & VisionRecipe.ini & VisionStage.ini ]
		private string m_strRecipePath;
		// 시스템 파라미터 선언
		private CSystemParameter m_objSystemParameter;
		// 데이터베이스 파라미터 선언
		private CDatabaseParameter m_objDatabaseParameter;
		// 레시피 정보 선언
		private CRecipeInformation m_objRecipeInformation;
		// 레시피 파라미터 선언
		private CRecipeParameter[] m_objRecipeParameter;
		// 카메라 파라미터 선언
		private CCameraParameter[] m_objCameraParameter;
		// IO 초기화 파라미터 선언
		private CIOInitializeParameter m_objIOInitializeParameter;
		// PLC 초기화 파라미터 선언
		private CPLCInitializeParameter m_objPLCInitializeParameter;
		// 조명 컨트롤러 파라미터 선언
		private CLightControllerParameter[] m_objLightControllerParameter;
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CConfig()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool Initialize()
		{
			bool bReturn = false;

			do {
				// 프로젝트 bin 폴더
				m_strCurrentPath = System.IO.Directory.GetCurrentDirectory();
				// 시스템 파라미터 객체 생성
				m_objSystemParameter = new CSystemParameter();
				// 시스템 파라미터 로드
				LoadSystemParameter();
				// 아이템 폴더 경로
				m_strItemPath = m_objSystemParameter.strItemPath;
				if( false == Directory.Exists( m_strItemPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( m_strItemPath );
				}
				// 레시피 폴더 경로
				m_strRecipePath = m_objSystemParameter.strRecipePath;
				if( false == Directory.Exists( m_strRecipePath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( m_strRecipePath );
				}
				// 데이터베이스 파라미터 객체 생성
				m_objDatabaseParameter = new CDatabaseParameter();
				// 레시피 정보 객체 생성
				m_objRecipeInformation = new CRecipeInformation();
				// 레시피 파라미터 객체 생성 ( 카메라 수만큼 )
				m_objRecipeParameter = new CRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_FINAL ];
				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
					m_objRecipeParameter[ iLoopCount ] = new CRecipeParameter();
				}
				
				// 카메라 파라미터 객체 생성 ( 카메라 수만큼 )
				m_objCameraParameter = new CCameraParameter[ ( int )CDefine.enumCamera.CAMERA_FINAL ];
				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
					m_objCameraParameter[ iLoopCount ] = new CCameraParameter();
				}
				// IO 초기화 파라미터 객체 생성
				m_objIOInitializeParameter = new CIOInitializeParameter();
				// PLC 초기화 파라미터 객체 생성
				m_objPLCInitializeParameter = new CPLCInitializeParameter();
				// 라이트 컨트롤러 객체 생성 ( 조명 컨트롤러 수만큼 )
				m_objLightControllerParameter = new CLightControllerParameter[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_FINAL ];
				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumLightController.LIGHT_CONTROLLER_FINAL; iLoopCount++ ) {
					m_objLightControllerParameter[ iLoopCount ] = new CLightControllerParameter();
				}

                
                LoadParameters();
                SaveParameters();
                
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
		//목적 : 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool LoadParameters()
		{
			bool bReturn = false;

			do {
                // 시스템 파라미터 로드
                LoadSystemParameter();
                
                // 데이터베이스 파라미터 로드
                LoadDatabaseParameter();

                // 레시지 정보 로드
                LoadRecipeInformation();

                // 레시피 파라미터 로드
                LoadRecipeParameter();

                // 카메라 파라미터 로드
                LoadCameraParameter();

                // IO Map 파라미터 로드
                LoadIOParameter();

                // PLC Map 파라미터 로드
                LoadPLCParameter();

                // 라이트 파라미터 로드
                LoadLightControllerParameter();
                CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, string.Format( "LoadParameters - Loading" ), TypeOfMessage.Success );
                bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveParameters()
		{
			bool bReturn = false;

			do {
                // 시스템 파라미터 저장
                SaveSystemParameter();
                // 데이터베이스 파라미터 저장
                SaveDatabaseParameter();
				// 레시피 정보 저장
				SaveRecipeInformation();
				// 레시피 파라미터 저장
				SaveRecipeParameter();
				// 카메라 파라미터 저장
				SaveCameraParameter();
				// IO 파라미터 저장
				SaveIOParameter();
				// PLC 파라미터 저장
				SavePLCParameter();
				// 라이트 파라미터 저장
				SaveLightControllerParameter();
                CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, string.Format( "LoadParameters - Complete" ), TypeOfMessage.Success );
                bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 시스템 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadSystemParameter()
		{
			bool bReturn = false;
			var pFormCommon = CFormCommon.GetFormCommon;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "SYSTEM";
				var varParameter = m_objSystemParameter;

				varParameter.strItemPath = objINI.GetString( strSection, "strItemPath", @"D:\DeepSight\Item" );
				varParameter.strRecipePath = objINI.GetString( strSection, "strRecipePath", @"D:\DeepSight_Recipe" );
                varParameter.strSimulationImagePath = objINI.GetString( strSection, "strSimulationImagePath", @"D:\DeepSight" );
                varParameter.strRecipeID = objINI.GetString( strSection, "strRecipeID", "0" );
                varParameter.strTitleProcess60 = objINI.GetString( strSection, "strTitleProcess60", "BUSBAR INSPECTION" );
                varParameter.strTitleProcess110 = objINI.GetString( strSection, "strTitleProcess110", "SENSOR INSPECTION" );
                varParameter.strTitleProcess150 = objINI.GetString( strSection, "strTitleProcess150", "CASE INSPECTION" );
                varParameter.bPassMode = objINI.GetBool( strSection, "bPassMode", false );
                varParameter.bUseResultDialog = objINI.GetBool( strSection, "bUseResultDialog", false );
                varParameter.bUseAutoRecipeChange = objINI.GetBool( strSection, "bUseAutoRecipeChange", false );
                varParameter.eMachineType = ( CDefine.enumMachineType )objINI.GetInt32( strSection, "eMachineType", ( int )CDefine.enumMachineType.PROCESS_60 );
                varParameter.eFactoryType = ( CDefine.enumFactoryType )objINI.GetInt32( strSection, "eFactoryType", ( int )CDefine.enumFactoryType.FACTORY_A );
                varParameter.eCameraType = ( CDefine.enumCameraType )objINI.GetInt32( strSection, "eCameraType", ( int )CDefine.enumCameraType.CAMERA_AREA );
                varParameter.eMachinePosition = ( CDefine.enumMachinePosition )objINI.GetInt32( strSection, "eMachinePosition", ( int )CDefine.enumMachinePosition.POSITION_A );
                varParameter.iOffsetAddressPLC = objINI.GetInt32( strSection, "iOffsetAddressPLC", 200 );
                varParameter.eSimulationMode = ( CDefine.enumSimulationMode )objINI.GetInt32( strSection, "eSimulationMode", ( int )CDefine.enumSimulationMode.SIMULATION_MODE_OFF );
                varParameter.eSimulationModePLC = ( CDefine.enumSimulationMode )objINI.GetInt32( strSection, "eSimulationModePLC", ( int )CDefine.enumSimulationMode.SIMULATION_MODE_OFF );
                varParameter.bVidiTeachMode = objINI.GetBool( strSection, "bVidiTeachMode", false );
                varParameter.eLanguage = ( CDefine.enumLanguage )objINI.GetInt32( strSection, "eLanguage", ( int )CDefine.enumLanguage.LANGUAGE_KOREA );
				varParameter.bImageSave = objINI.GetBool( strSection, "bImageSave", false );
				varParameter.iPeriodImage = objINI.GetInt32( strSection, "iPeriodImage", 5 );
				varParameter.iPeriodDatabase = objINI.GetInt32( strSection, "iPeriodDatabase", 10 );
				varParameter.strImageSavePath = objINI.GetString( strSection, "strImageSavePath", @"D:\IMAGE" );
				varParameter.eSaveImageType = ( CDefine.enumSaveImageType )objINI.GetInt32( strSection, "eSaveImageType", ( int )CDefine.enumSaveImageType.TYPE_BMP );
                
				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 시스템 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveSystemParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "SYSTEM";
				var varParameter = m_objSystemParameter;

				CConfigValue objConfigValue = new CConfigValue( objINI, varParameter, strSection );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strItemPath ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipePath ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.strSimulationImagePath ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipeID ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.strTitleProcess60 ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.strTitleProcess110 ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.strTitleProcess150 ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eMachineType ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eFactoryType ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eCameraType ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eMachinePosition ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.iOffsetAddressPLC ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eSimulationMode ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eSimulationModePLC ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.bVidiTeachMode ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.bUseResultDialog ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.bUseAutoRecipeChange ) );
                objConfigValue.SaveValue( GetVariableName( () => varParameter.eLanguage ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.bImageSave ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.iPeriodImage ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.iPeriodDatabase ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strImageSavePath ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.eSaveImageType ) );
				
				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 시스템 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveSystemParameter( CSystemParameter objSystemParameter )
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "SYSTEM";
				var varParameter = objSystemParameter;

				CConfigValue objConfigValue = new CConfigValue( objINI, varParameter, strSection );
				if( m_objSystemParameter.strItemPath != objSystemParameter.strItemPath ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.strItemPath ) );
				}
				if( m_objSystemParameter.strRecipePath != objSystemParameter.strRecipePath ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipePath ) );
				}
                if( m_objSystemParameter.strSimulationImagePath != objSystemParameter.strSimulationImagePath ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.strSimulationImagePath ) );
                }
                if( m_objSystemParameter.strRecipeID != objSystemParameter.strRecipeID ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipeID ) );
				}
                if( m_objSystemParameter.strTitleProcess60 != objSystemParameter.strTitleProcess60 ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.strTitleProcess60 ) );
                }
                if( m_objSystemParameter.strTitleProcess110 != objSystemParameter.strTitleProcess110 ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.strTitleProcess110 ) );
                }
                if( m_objSystemParameter.strTitleProcess150 != objSystemParameter.strTitleProcess150 ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.strTitleProcess150 ) );
                }
                if( m_objSystemParameter.eMachineType != objSystemParameter.eMachineType ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.eMachineType ) );
				}
                if( m_objSystemParameter.eFactoryType != objSystemParameter.eFactoryType ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.eFactoryType ) );
                }
                if( m_objSystemParameter.eCameraType != objSystemParameter.eCameraType ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.eCameraType ) );
                }
                if ( m_objSystemParameter.eMachinePosition != objSystemParameter.eMachinePosition )
                {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.eMachinePosition ) );
                }
                if ( m_objSystemParameter.iOffsetAddressPLC != objSystemParameter.iOffsetAddressPLC )
                {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.iOffsetAddressPLC ) );
                }
                if ( m_objSystemParameter.eSimulationMode != objSystemParameter.eSimulationMode ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.eSimulationMode ) );
				}
                if( m_objSystemParameter.eSimulationModePLC != objSystemParameter.eSimulationModePLC ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.eSimulationModePLC ) );
                }
                if( m_objSystemParameter.bVidiTeachMode != objSystemParameter.bVidiTeachMode ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.bVidiTeachMode ) );
                }
                if( m_objSystemParameter.bUseResultDialog != objSystemParameter.bUseResultDialog ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.bUseResultDialog ) );
                }
                if( m_objSystemParameter.bUseAutoRecipeChange != objSystemParameter.bUseAutoRecipeChange ) {
                    objConfigValue.SaveValue( GetVariableName( () => varParameter.bUseAutoRecipeChange ) );
                }
                if( m_objSystemParameter.eLanguage != objSystemParameter.eLanguage ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.eLanguage ) );
				}
				if( m_objSystemParameter.bImageSave != objSystemParameter.bImageSave ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.bImageSave ) );
				}
				if( m_objSystemParameter.iPeriodImage != objSystemParameter.iPeriodImage ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.iPeriodImage ) );
				}
				if( m_objSystemParameter.iPeriodDatabase != objSystemParameter.iPeriodDatabase ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.iPeriodDatabase ) );
				}
				if( m_objSystemParameter.strImageSavePath != objSystemParameter.strImageSavePath ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.strImageSavePath ) );
				}
				if( m_objSystemParameter.eSaveImageType != objSystemParameter.eSaveImageType ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.eSaveImageType ) );
				}
				
				m_objSystemParameter = ( CSystemParameter )objSystemParameter.Clone();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadDatabaseParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "DATABASE";
				var varParameter = m_objDatabaseParameter;

				// Database
				m_objDatabaseParameter.strDatabaseHistory = objINI.GetString( strSection, "strDatabaseHistory", "DATABASE_HISTORY" );
				m_objDatabaseParameter.strDatabaseInformation = objINI.GetString( strSection, "strDatabaseInformation", "DATABASE_INFORMATION" );
				// Table
				m_objDatabaseParameter.strTableInformationUIText = objINI.GetString( strSection, "strTableInformationUIText", "TABLE_INFORMATION_UI_TEXT" );
				m_objDatabaseParameter.strTableInformationUserMessage = objINI.GetString( strSection, "strTableInformationUserMessage", "TABLE_INFORMATION_USER_MESSAGE" );
				m_objDatabaseParameter.strTableHistoryAlign = objINI.GetString( strSection, "strTableHistoryAlign", "TABLE_HISTORY_ALIGN" );
				// Record
				m_objDatabaseParameter.strRecordInformationUIText = objINI.GetString( strSection, "strRecordInformationUIText", "RECORD_INFORMATION_UI_TEXT" );
				m_objDatabaseParameter.strRecordInformationUserMessage = objINI.GetString( strSection, "strRecordInformationUserMessage", "RECORD_INFORMATION_USER_MESSAGE" );
				// Delete
				m_objDatabaseParameter.iDeletePeriodAlign = objINI.GetInt32( strSection, "iDeletePeriodAlign", 5 );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveDatabaseParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "DATABASE";
				var varParameter = m_objDatabaseParameter;

				CConfigValue objConfigValue = new CConfigValue( objINI, varParameter, strSection );
				// Database
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strDatabaseHistory ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strDatabaseInformation ) );
				// Table
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strTableInformationUIText ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strTableInformationUserMessage ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strTableHistoryAlign ) );
				// Record
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecordInformationUIText ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecordInformationUserMessage ) );
				// Delete
				objConfigValue.SaveValue( GetVariableName( () => varParameter.iDeletePeriodAlign ) );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveDatabaseParameter( CDatabaseParameter objDatabaseParameter )
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "DATABASE";
				var varParameter = objDatabaseParameter;

				CConfigValue objConfigValue = new CConfigValue( objINI, varParameter, strSection );
				// Database
				if( m_objDatabaseParameter.strDatabaseHistory != objDatabaseParameter.strDatabaseHistory ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strDatabaseHistory ) );
				}
				if( m_objDatabaseParameter.strDatabaseInformation != objDatabaseParameter.strDatabaseInformation ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strDatabaseInformation ) );
				}
				// Table
				if( m_objDatabaseParameter.strTableInformationUIText != objDatabaseParameter.strTableInformationUIText ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strTableInformationUIText ) );
				}
				if( m_objDatabaseParameter.strTableInformationUserMessage != objDatabaseParameter.strTableInformationUserMessage ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strTableInformationUserMessage ) );
				}
				if( m_objDatabaseParameter.strTableHistoryAlign != objDatabaseParameter.strTableHistoryAlign ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strTableHistoryAlign ) );
				}
				// Record
				if( m_objDatabaseParameter.strRecordInformationUIText != objDatabaseParameter.strRecordInformationUIText ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strRecordInformationUIText ) );
				}
				if( m_objDatabaseParameter.strRecordInformationUserMessage != objDatabaseParameter.strRecordInformationUserMessage ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.strRecordInformationUserMessage ) );
				}
				// Delete
				if( m_objDatabaseParameter.iDeletePeriodAlign != objDatabaseParameter.iDeletePeriodAlign ) {
					objConfigValue.SaveValue( GetVariableName( () => objDatabaseParameter.iDeletePeriodAlign ) );
				}

				m_objDatabaseParameter = objDatabaseParameter.Clone() as CDatabaseParameter;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 정보 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadRecipeInformation()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID );
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}\{2}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID, CDefine.DEF_RECIPE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "RECIPE";
				var varParameter = m_objRecipeInformation;

				varParameter.strRecipeID = objINI.GetString( strSection, "strRecipeID", m_objSystemParameter.strRecipeID );
				varParameter.strRecipeName = objINI.GetString( strSection, "strRecipeName", "1" );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 정보 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveRecipeInformation()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID );
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}\{2}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID, CDefine.DEF_RECIPE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "RECIPE";
				var varParameter = m_objRecipeInformation;

				CConfigValue objConfigValue = new CConfigValue( objINI, varParameter, strSection );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipeID ) );
				objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipeName ) );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 정보 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveRecipeInformation( CRecipeInformation objRecipeInformation )
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID );
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}\{2}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID, CDefine.DEF_RECIPE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "RECIPE";
				var varParameter = objRecipeInformation;

				CConfigValue objConfigValue = new CConfigValue( objINI, varParameter, strSection );
				if( m_objRecipeInformation.strRecipeID != objRecipeInformation.strRecipeID ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipeID ) );
				}
				if( m_objRecipeInformation.strRecipeName != objRecipeInformation.strRecipeName ) {
					objConfigValue.SaveValue( GetVariableName( () => varParameter.strRecipeName ) );
				}

				m_objRecipeInformation = ( CRecipeInformation )objRecipeInformation.Clone();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadRecipeParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID );
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}\{2}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID, CDefine.DEF_RECIPE_INI );
				ClassINI objINI = new ClassINI( strPath );

				var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;

				for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter.Length; iLoopCount++ ) {
					string strSection = ( ( CDefine.enumCamera )iLoopCount ).ToString();
					var varParameter = m_objRecipeParameter[ iLoopCount ];

					// 카메라 설정
					varParameter.objCameraConfig.bReverseX = objINI.GetBool( strSection, "objCameraConfig.bReverseX", false );
					varParameter.objCameraConfig.bReverseY = objINI.GetBool( strSection, "objCameraConfig.bReverseY", false );
					varParameter.objCameraConfig.bRotation90 = objINI.GetBool( strSection, "objCameraConfig.bRotation90", false );
					varParameter.objCameraConfig.bRotation180 = objINI.GetBool( strSection, "objCameraConfig.bRotation180", false );
					varParameter.objCameraConfig.bRotation270 = objINI.GetBool( strSection, "objCameraConfig.bRotation270", false );
					varParameter.objCameraConfig.dGain = objINI.GetDouble( strSection, "objCameraConfig.dGain", 30.0 );
					varParameter.objCameraConfig.dExposureTime = objINI.GetDouble( strSection, "objCameraConfig.dExposureTime", 10000.0 );
					varParameter.objCameraConfig.iCameraWidth = objINI.GetInt32( strSection, "objCameraConfig.iCameraWidth", 4024 );
					varParameter.objCameraConfig.iCameraHeight = objINI.GetInt32( strSection, "objCameraConfig.iCameraHeight", 3036 );
					varParameter.objCameraConfig.iCameraXOffset = objINI.GetInt32( strSection, "objCameraConfig.iCameraXOffset", 0 );
					varParameter.objCameraConfig.iCameraYOffset = objINI.GetInt32( strSection, "objCameraConfig.iCameraYOffset", 0 );
					varParameter.objCameraConfig.dFrameRate = objINI.GetDouble( strSection, "objCameraConfig.dFrameRate", 20.0 );

                    // 비디파라미터
                    varParameter.strVidiWorkSpaceFilePath = objINI.GetString( strSection, "strVidiWorkSpaceFilePath", "d:\\DeepSight_Recipe\\Vidi.vrws" );
                    varParameter.strVidiWorkSpaceName = objINI.GetString( strSection, "strVidiWorkSpaceName", "Workspace" );
                        

                    varParameter.iCountInspectionPosition = objINI.GetInt32( strSection, "iCountInspectionPosition", 3 );


                    for ( int iLoopInspection = 0; iLoopInspection < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopInspection++ )
                    {
                        string strInspection = string.Format( "InspectionIndex_{0}_", iLoopInspection );
                        varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeVIDI = ( CDefine.enumPMSImageType )objINI.GetInt32( strSection, strInspection + "ePMSImageTypeVIDI", 1 );
                        varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeMeasure = ( CDefine.enumPMSImageType )objINI.GetInt32( strSection, strInspection + "ePMSImageTypeMeasure", 1 );
                        varParameter.objInspectionParameter[ iLoopInspection ].e3DImageTypeVIDI = ( CDefine.enum3DImageType )objINI.GetInt32( strSection, strInspection + "e3DImageTypeVIDI", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].e3DImageRotation = ( CDefine.enumImageRotation )objINI.GetInt32( strSection, strInspection + "e3DImageRotation", 1 );
                        varParameter.objInspectionParameter[ iLoopInspection ].i3DScanLength = objINI.GetInt32( strSection, strInspection + "i3DScanLength", 100 );
                        varParameter.objInspectionParameter[ iLoopInspection ].d3DScanWidth = objINI.GetDouble( strSection, strInspection + "d3DScanWidth", 20 );
                        varParameter.objInspectionParameter[iLoopInspection].d3DHeightThresholdMax = objINI.GetDouble(strSection, strInspection + "d3DHeightThresholdMax", 1);
                        varParameter.objInspectionParameter[iLoopInspection].d3DHeightThresholdMin = objINI.GetDouble(strSection, strInspection + "d3DHeightThresholdMin", -2);
                        varParameter.objInspectionParameter[iLoopInspection].i3DHeightThresholdCount = objINI.GetInt32(strSection, strInspection + "i3DHeightThresholdCount", 1000);

                        varParameter.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMax = objINI.GetDouble( strSection, strInspection + "d3DWeldWidthMax", 2.5 );
                        varParameter.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMin = objINI.GetDouble( strSection, strInspection + "d3DWeldWidthMin", 1.5 );

                        varParameter.objInspectionParameter[ iLoopInspection ].e3DImageTypeMeasure = ( CDefine.enum3DImageType )objINI.GetInt32( strSection, strInspection + "e3DImageTypeMeasure", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize = objINI.GetInt32( strSection, strInspection + "iCropSize", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.eCropType = ( CDefine.enumCameraCropType )objINI.GetInt32( strSection, strInspection + "eCropType", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dCameraExpouseTime = objINI.GetDouble( strSection, strInspection + "dCameraExpouseTime", 10000 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 0 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_0", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 1 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_1", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 2 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_2", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 3 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_3", 10 );

                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.iCropSize = objINI.GetInt32( strSection, strInspection + "iCropSize_Measure", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.eCropType = ( CDefine.enumCameraCropType )objINI.GetInt32( strSection, strInspection + "eCropType_Measure", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dCameraExpouseTime = objINI.GetDouble( strSection, strInspection + "dCameraExpouseTime_Measure", 10000 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 0 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_0_Measure", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 1 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_1_Measure", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 2 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_2_Measure", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 3 ] = objINI.GetDouble( strSection, strInspection + "dLightValue_3_Measure", 10 );

                        varParameter.objInspectionParameter[ iLoopInspection ].iCountSerchRegion = objINI.GetInt32( strSection, strInspection + "iCountSerchRegion", 3 );
                        varParameter.objInspectionParameter[ iLoopInspection ].dVidiScore = objINI.GetDouble( strSection, strInspection + "dVidiScore", 1 );
                        varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeWidth = objINI.GetDouble( strSection, strInspection + "dVidiFixedSizeWidth", 600 );
                        varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeHeight = objINI.GetDouble( strSection, strInspection + "dVidiFixedSizeHeight", 300 );
                        varParameter.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeWidth = objINI.GetDouble( strSection, strInspection + "dMeasureFixedSizeWidth", 600 );
                        varParameter.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeHeight = objINI.GetDouble( strSection, strInspection + "dMeasureFixedSizeHeight", 300 );

                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRLowThresh = objINI.GetInt32( strSection, strInspection + "iBusbarLRLowThresh", 12 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRContinousLen = objINI.GetInt32( strSection, strInspection + "iBusbarLRContinousLen", 50 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRHighThresh = objINI.GetInt32( strSection, strInspection + "iBusbarLRHighThresh", 10 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBThresh = objINI.GetInt32( strSection, strInspection + "iBusbarTBThresh", 50 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBContinousLen = objINI.GetInt32( strSection, strInspection + "iBusbarTBContinousLen", 5 );

                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdWid = objINI.GetInt32( strSection, strInspection + "iSensorStdWid", 370 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdHgt = objINI.GetInt32( strSection, strInspection + "iSensorStdHgt", 225 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorThresh = objINI.GetInt32( strSection, strInspection + "iSensorThresh", 70 );

                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMax = objINI.GetInt32( strSection, strInspection + "dSizeWidthMax", 150 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMin = objINI.GetInt32( strSection, strInspection + "dSizeWidthMin", 0 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMax = objINI.GetInt32( strSection, strInspection + "dSizeHeightMax", 150 );
                        varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMin = objINI.GetInt32( strSection, strInspection + "dSizeHeightMin", 0 );


                        for ( int iLoopSearchCount = 0; iLoopSearchCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopSearchCount++ )
                        {
                            string strName = "dStartX_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartX = objINI.GetDouble( strSection, strInspection + strName, 30.0 );
                            strName = "dStartY_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartY = objINI.GetDouble( strSection, strInspection + strName, 30.0 );
                            strName = "dEndX_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndX = objINI.GetDouble( strSection, strInspection + strName, 200.0 );
                            strName = "dEndY_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndY = objINI.GetDouble( strSection, strInspection + strName, 200.0 );
                            strName = "dRotation_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dRotation = objINI.GetDouble( strSection, strInspection + strName, 0 );
                            strName = "dSkew_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dSkew = objINI.GetDouble( strSection, strInspection + strName, 0 );

                            strName = "MeasureSerchRegion_dStartX_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartX = objINI.GetDouble( strSection, strInspection + strName, 30.0 );
                            strName = "MeasureSerchRegion_dStartY_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartY = objINI.GetDouble( strSection, strInspection + strName, 30.0 );
                            strName = "MeasureSerchRegion_dEndX_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndX = objINI.GetDouble( strSection, strInspection + strName, 200.0 );
                            strName = "MeasureSerchRegion_dEndY_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndY = objINI.GetDouble( strSection, strInspection + strName, 200.0 );
                            strName = "MeasureSerchRegion_dRotation_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dRotation = objINI.GetDouble( strSection, strInspection + strName, 0 );
                            strName = "MeasureSerchRegion_dSkew_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dSkew = objINI.GetDouble( strSection, strInspection + strName, 0 );

                            strName = "GradientRegion_dStartX_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartX = objINI.GetDouble( strSection, strInspection + strName, 30.0 );
                            strName = "GradientRegion_dStartY_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartY = objINI.GetDouble( strSection, strInspection + strName, 30.0 );
                            strName = "GradientRegion_dEndX_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndX = objINI.GetDouble( strSection, strInspection + strName, 200.0 );
                            strName = "GradientRegion_dEndY_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndY = objINI.GetDouble( strSection, strInspection + strName, 200.0 );
                            strName = "GradientRegion_dRotation_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dRotation = objINI.GetDouble( strSection, strInspection + strName, 0 );
                            strName = "GradientRegion_dSkew_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dSkew = objINI.GetDouble( strSection, strInspection + strName, 0 );

                            strName = "objFindLineTop_ePolaraty_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].ePolaraty = ( CFindLineParameter.enumPolaraty )objINI.GetInt32( strSection, strInspection + strName, 1 );
                            strName = "objFindLineTop_eSerachDirection_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].eSerachDirection = ( CFindLineParameter.enumSerarchDirection )objINI.GetInt32( strSection, strInspection + strName, 0 );
                            strName = "objFindLineTop_iCalipersNumber_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iCalipersNumber = objINI.GetInt32( strSection, strInspection + strName, 20 );
                            strName = "objFindLineTop_iIgnoreNumber_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iIgnoreNumber = objINI.GetInt32( strSection, strInspection + strName, 10 );
                            strName = "objFindLineTop_iThreshold_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iThreshold = objINI.GetInt32( strSection, strInspection + strName, 10 );
                            strName = "objFindLineTop_iFilter_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iFilter = objINI.GetInt32( strSection, strInspection + strName, 2 );
                            strName = "objFindLineTop_dSearchLength_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].dSearchLength = objINI.GetDouble( strSection, strInspection + strName, 150 );

                            strName = "objFindLineBottom_ePolaraty_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].ePolaraty = ( CFindLineParameter.enumPolaraty )objINI.GetInt32( strSection, strInspection + strName, 1 );
                            strName = "objFindLineBottom_eSerachDirection_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].eSerachDirection = ( CFindLineParameter.enumSerarchDirection )objINI.GetInt32( strSection, strInspection + strName, 1 );
                            strName = "objFindLineBottom_iCalipersNumber_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iCalipersNumber = objINI.GetInt32( strSection, strInspection + strName, 20 );
                            strName = "objFindLineBottom_iIgnoreNumber_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iIgnoreNumber = objINI.GetInt32( strSection, strInspection + strName, 10 );
                            strName = "objFindLineBottom_iThreshold_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iThreshold = objINI.GetInt32( strSection, strInspection + strName, 10 );
                            strName = "objFindLineBottom_iFilter_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iFilter = objINI.GetInt32( strSection, strInspection + strName, 2 );
                            strName = "objFindLineBottom_dSearchLength_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].dSearchLength = objINI.GetDouble( strSection, strInspection + strName, 150 );

                            // 워크스페이스 경로와패스는 레시피파라미터에서 따로 읽었기때문에 값만 가지고 있자
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strWorkSpaceFilePath = varParameter.strVidiWorkSpaceFilePath;
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strWorkSpaceName = varParameter.strVidiWorkSpaceName;
                            // 스트림 및 툴이름, 툴타입은 로딩
                            strName = "strStreamName_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strStreamName = objINI.GetString( strSection, strInspection + strName, "AL" );
                            strName = "strToolName_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strToolName = objINI.GetString( strSection, strInspection + strName, "Analyze" );
                            strName = "eVidiType_" + iLoopSearchCount.ToString();
                            varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.eVidiType = (CDefine.enumVidiType)objINI.GetInt32( strSection, strInspection + strName, 0 );
                        }
                    }
                }
				
				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveRecipeParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID );
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}\{2}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID, CDefine.DEF_RECIPE_INI );
				ClassINI objINI = new ClassINI( strPath );

				var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;

                for ( int iLoopCount = 0; iLoopCount < m_objRecipeParameter.Length; iLoopCount++ )
                {
                    string strSection = ( ( CDefine.enumCamera )iLoopCount ).ToString();
                    var varParameter = m_objRecipeParameter[ iLoopCount ];

                    // 카메라 설정
                    objINI.WriteValue( strSection, "objCameraConfig.bReverseX", varParameter.objCameraConfig.bReverseX );
                    objINI.WriteValue( strSection, "objCameraConfig.bReverseY", varParameter.objCameraConfig.bReverseY );
                    objINI.WriteValue( strSection, "objCameraConfig.bRotation90", varParameter.objCameraConfig.bRotation90 );
                    objINI.WriteValue( strSection, "objCameraConfig.bRotation180", varParameter.objCameraConfig.bRotation180 );
                    objINI.WriteValue( strSection, "objCameraConfig.bRotation270", varParameter.objCameraConfig.bRotation270 );
                    objINI.WriteValue( strSection, "objCameraConfig.dGain", varParameter.objCameraConfig.dGain );
                    objINI.WriteValue( strSection, "objCameraConfig.dExposureTime", varParameter.objCameraConfig.dExposureTime );
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraWidth", varParameter.objCameraConfig.iCameraWidth );
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraHeight", varParameter.objCameraConfig.iCameraHeight );
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraXOffset", varParameter.objCameraConfig.iCameraXOffset );
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraYOffset", varParameter.objCameraConfig.iCameraYOffset );
                    objINI.WriteValue( strSection, "objCameraConfig.dFrameRate", varParameter.objCameraConfig.dFrameRate );

                    // 비디파라미터
                    objINI.WriteValue( strSection, "strVidiWorkSpaceFilePath", varParameter.strVidiWorkSpaceFilePath );
                    objINI.WriteValue( strSection, "strVidiWorkSpaceName", varParameter.strVidiWorkSpaceName );

                    objINI.WriteValue( strSection, "iCountInspectionPosition", varParameter.iCountInspectionPosition );

                    // 로딩시간이 너무오래걸려 이부부은 그냥 넘어감, 여기는 프로그램 실행시에만 저장됨
                    //                     for ( int iLoopInspection = 0; iLoopInspection < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopInspection++ )
                    //                     {
                    //                         string strInspection = string.Format( "InspectionIndex_{0}_", iLoopInspection );
                    //                         objINI.WriteValue( strSection, strInspection + "ePMSImageTypeVIDI", ( int )varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeVIDI );
                    //                         objINI.WriteValue( strSection, strInspection + "dCameraExpouseTime", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dCameraExpouseTime );
                    //                         objINI.WriteValue( strSection, strInspection + "dLightValue_0", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 0 ] );
                    //                         objINI.WriteValue( strSection, strInspection + "dLightValue_1", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 1 ] );
                    //                         objINI.WriteValue( strSection, strInspection + "dLightValue_2", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 2 ] );
                    //                         objINI.WriteValue( strSection, strInspection + "dLightValue_3", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 3 ] );
                    //                         objINI.WriteValue( strSection, strInspection + "iCountSerchRegion", varParameter.objInspectionParameter[ iLoopInspection ].iCountSerchRegion );
                    //                         objINI.WriteValue( strSection, strInspection + "dVidiFixedSizeWidth", varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeWidth );
                    //                         objINI.WriteValue( strSection, strInspection + "dVidiFixedSizeHeight", varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeHeight );
                    //                         for ( int iLoopSearchCount = 0; iLoopSearchCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopSearchCount++ )
                    //                         {
                    //                             string strName = "dStartX_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartX );
                    //                             strName = "dStartY_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartY );
                    //                             strName = "dEndX_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndX );
                    //                             strName = "dEndY_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndY );
                    //                             strName = "dRotation_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dRotation );
                    //                             strName = "dSkew_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dSkew );
                    // 
                    //                             // 워크스페이스 경로와패스는 레시피파라미터에서 따로 읽었기때문에 값만 가지고 있자
                    //                             varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strWorkSpaceFilePath = varParameter.strVidiWorkSpaceFilePath;
                    //                             varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strWorkSpaceName = varParameter.strVidiWorkSpaceName;
                    //                             // 스트림 및 툴이름, 툴타입은 로딩
                    //                             strName = "strStreamName_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strStreamName );
                    //                             strName = "strToolName_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strToolName );
                    //                             strName = "eVidiType_" + iLoopSearchCount.ToString();
                    //                             objINI.WriteValue( strSection, strInspection + strName, ( int )varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.eVidiType );
                    //                         }
                    //                     }

                }
                bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveRecipeParameter( int iCameraIndex, CRecipeParameter objRecipeParameter )
		{
			bool bReturn = false;

            do {
                string strPath = string.Format( @"{0}\{1}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID );
                if ( false == Directory.Exists( strPath ) ) {
                    // 폴더 생성
                    Directory.CreateDirectory( strPath );
                }
                strPath = string.Format( @"{0}\{1}\{2}", m_objSystemParameter.strRecipePath, m_objSystemParameter.strRecipeID, CDefine.DEF_RECIPE_INI );
                ClassINI objINI = new ClassINI( strPath );

                var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;

                string strSection = ( ( CDefine.enumCamera )iCameraIndex ).ToString();
                // 원본 대상
                var varParameterOrigin = m_objRecipeParameter[ iCameraIndex ];
                // 적용 대상
                var varParameter = objRecipeParameter;


                // 카메라 설정
                if ( varParameterOrigin.objCameraConfig.bReverseX != varParameter.objCameraConfig.bReverseX ) {
                    objINI.WriteValue( strSection, "objCameraConfig.bReverseX", varParameter.objCameraConfig.bReverseX );
                }
                if ( varParameterOrigin.objCameraConfig.bReverseY != varParameter.objCameraConfig.bReverseY ) {
                    objINI.WriteValue( strSection, "objCameraConfig.bReverseY", varParameter.objCameraConfig.bReverseY );
                }
                if ( varParameterOrigin.objCameraConfig.bRotation90 != varParameter.objCameraConfig.bRotation90 ) {
                    objINI.WriteValue( strSection, "objCameraConfig.bRotation90", varParameter.objCameraConfig.bRotation90 );
                }
                if ( varParameterOrigin.objCameraConfig.bRotation180 != varParameter.objCameraConfig.bRotation180 ) {
                    objINI.WriteValue( strSection, "objCameraConfig.bRotation180", varParameter.objCameraConfig.bRotation180 );
                }
                if ( varParameterOrigin.objCameraConfig.bRotation270 != varParameter.objCameraConfig.bRotation270 ) {
                    objINI.WriteValue( strSection, "objCameraConfig.bRotation270", varParameter.objCameraConfig.bRotation270 );
                }
                if ( varParameterOrigin.objCameraConfig.dGain != varParameter.objCameraConfig.dGain ) {
                    objINI.WriteValue( strSection, "objCameraConfig.dGain", varParameter.objCameraConfig.dGain );
                }
                if ( varParameterOrigin.objCameraConfig.dExposureTime != varParameter.objCameraConfig.dExposureTime )
                {
                    objINI.WriteValue( strSection, "objCameraConfig.dExposureTime", varParameter.objCameraConfig.dExposureTime );
                }
                if ( varParameterOrigin.objCameraConfig.iCameraWidth != varParameter.objCameraConfig.iCameraWidth )
                {
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraWidth", varParameter.objCameraConfig.iCameraWidth );
                }
                if ( varParameterOrigin.objCameraConfig.iCameraHeight != varParameter.objCameraConfig.iCameraHeight )
                {
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraHeight", varParameter.objCameraConfig.iCameraHeight );
                }
                if ( varParameterOrigin.objCameraConfig.iCameraXOffset != varParameter.objCameraConfig.iCameraXOffset )
                {
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraXOffset", varParameter.objCameraConfig.iCameraXOffset );
                }
                if ( varParameterOrigin.objCameraConfig.iCameraYOffset != varParameter.objCameraConfig.iCameraYOffset )
                {
                    objINI.WriteValue( strSection, "objCameraConfig.iCameraYOffset", varParameter.objCameraConfig.iCameraYOffset );
                }
                if ( varParameterOrigin.objCameraConfig.dFrameRate != varParameter.objCameraConfig.dFrameRate )
                {
                    objINI.WriteValue( strSection, "objCameraConfig.dFrameRate", varParameter.objCameraConfig.dFrameRate );
                }

                if ( varParameterOrigin.strVidiWorkSpaceFilePath != varParameter.strVidiWorkSpaceFilePath )
                    objINI.WriteValue( strSection, "strVidiWorkSpaceFilePath", varParameter.strVidiWorkSpaceFilePath );

                if ( varParameterOrigin.strVidiWorkSpaceName != varParameter.strVidiWorkSpaceName )
                    objINI.WriteValue( strSection, "strVidiWorkSpaceName", varParameter.strVidiWorkSpaceName );

                if ( varParameterOrigin.iCountInspectionPosition != varParameter.iCountInspectionPosition )
                    objINI.WriteValue( strSection, "iCountInspectionPosition", varParameter.iCountInspectionPosition );

                for ( int iLoopInspection = 0; iLoopInspection < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopInspection++ )
                {
                    string strInspection = string.Format( "InspectionIndex_{0}_", iLoopInspection );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].ePMSImageTypeVIDI != varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeVIDI )
                        objINI.WriteValue( strSection, strInspection + "ePMSImageTypeVIDI", ( int )varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeVIDI );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].ePMSImageTypeMeasure != varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeMeasure )
                        objINI.WriteValue( strSection, strInspection + "ePMSImageTypeMeasure", ( int )varParameter.objInspectionParameter[ iLoopInspection ].ePMSImageTypeMeasure );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].e3DImageTypeVIDI != varParameter.objInspectionParameter[ iLoopInspection ].e3DImageTypeVIDI )
                        objINI.WriteValue( strSection, strInspection + "e3DImageTypeVIDI", ( int )varParameter.objInspectionParameter[ iLoopInspection ].e3DImageTypeVIDI );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].e3DImageRotation != varParameter.objInspectionParameter[ iLoopInspection ].e3DImageRotation )
                        objINI.WriteValue( strSection, strInspection + "e3DImageRotation", ( int )varParameter.objInspectionParameter[ iLoopInspection ].e3DImageRotation );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].i3DScanLength != varParameter.objInspectionParameter[ iLoopInspection ].i3DScanLength )
                        objINI.WriteValue( strSection, strInspection + "i3DScanLength", varParameter.objInspectionParameter[ iLoopInspection ].i3DScanLength );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].d3DScanWidth != varParameter.objInspectionParameter[ iLoopInspection ].d3DScanWidth )
                        objINI.WriteValue( strSection, strInspection + "d3DScanWidth", varParameter.objInspectionParameter[ iLoopInspection ].d3DScanWidth );

                    if (varParameterOrigin.objInspectionParameter[iLoopInspection].d3DHeightThresholdMax != varParameter.objInspectionParameter[iLoopInspection].d3DHeightThresholdMax)
                        objINI.WriteValue(strSection, strInspection + "d3DHeightThresholdMax", varParameter.objInspectionParameter[iLoopInspection].d3DHeightThresholdMax);

                    if (varParameterOrigin.objInspectionParameter[iLoopInspection].d3DHeightThresholdMin != varParameter.objInspectionParameter[iLoopInspection].d3DHeightThresholdMin)
                        objINI.WriteValue(strSection, strInspection + "d3DHeightThresholdMin", varParameter.objInspectionParameter[iLoopInspection].d3DHeightThresholdMin);

                    if (varParameterOrigin.objInspectionParameter[iLoopInspection].i3DHeightThresholdCount != varParameter.objInspectionParameter[iLoopInspection].i3DHeightThresholdCount)
                        objINI.WriteValue(strSection, strInspection + "i3DHeightThresholdCount", varParameter.objInspectionParameter[iLoopInspection].i3DHeightThresholdCount);

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMax != varParameter.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMax )
                        objINI.WriteValue( strSection, strInspection + "d3DWeldWidthMax", varParameter.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMax );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMin != varParameter.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMin )
                        objINI.WriteValue( strSection, strInspection + "d3DWeldWidthMin", varParameter.objInspectionParameter[ iLoopInspection ].d3DWeldWidthMin );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].e3DImageTypeMeasure != varParameter.objInspectionParameter[ iLoopInspection ].e3DImageTypeMeasure )
                        objINI.WriteValue( strSection, strInspection + "e3DImageTypeMeasure", ( int )varParameter.objInspectionParameter[ iLoopInspection ].e3DImageTypeMeasure );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize )
                        objINI.WriteValue( strSection, strInspection + "iCropSize", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.eCropType != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.eCropType )
                        objINI.WriteValue( strSection, strInspection + "eCropType", ( int )varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.eCropType );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.dCameraExpouseTime != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dCameraExpouseTime )
                        objINI.WriteValue( strSection, strInspection + "dCameraExpouseTime", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dCameraExpouseTime );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 0 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 0 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_0", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 0 ] );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 1 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 1 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_1", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 1 ] );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 2 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 2 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_2", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 2 ] );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 3 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 3 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_3", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.dLightValue[ 3 ] );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize )
                        objINI.WriteValue( strSection, strInspection + "iCropSize", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameter.iCropSize );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.eCropType != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.eCropType )
                        objINI.WriteValue( strSection, strInspection + "eCropType_Measure", ( int )varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.eCropType );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dCameraExpouseTime != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dCameraExpouseTime )
                        objINI.WriteValue( strSection, strInspection + "dCameraExpouseTime_Measure", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dCameraExpouseTime );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 0 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 0 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_0_Measure", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 0 ] );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 1 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 1 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_1_Measure", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 1 ] );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 2 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 2 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_2_Measure", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 2 ] );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 3 ] != varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 3 ] )
                        objINI.WriteValue( strSection, strInspection + "dLightValue_3_Measure", varParameter.objInspectionParameter[ iLoopInspection ].objGrabParameterMeasure.dLightValue[ 3 ] );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].iCountSerchRegion != varParameter.objInspectionParameter[ iLoopInspection ].iCountSerchRegion )
                        objINI.WriteValue( strSection, strInspection + "iCountSerchRegion", varParameter.objInspectionParameter[ iLoopInspection ].iCountSerchRegion );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].dVidiScore != varParameter.objInspectionParameter[ iLoopInspection ].dVidiScore )
                        objINI.WriteValue( strSection, strInspection + "dVidiScore", varParameter.objInspectionParameter[ iLoopInspection ].dVidiScore );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeWidth != varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeWidth )
                        objINI.WriteValue( strSection, strInspection + "dVidiFixedSizeWidth", varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeWidth );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeHeight != varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeHeight )
                        objINI.WriteValue( strSection, strInspection + "dVidiFixedSizeHeight", varParameter.objInspectionParameter[ iLoopInspection ].dVidiFixedSizeHeight );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeWidth != varParameter.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeWidth )
                        objINI.WriteValue( strSection, strInspection + "dMeasureFixedSizeWidth", varParameter.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeWidth );

                    if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeHeight != varParameter.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeHeight )
                        objINI.WriteValue( strSection, strInspection + "dMeasureFixedSizeHeight", varParameter.objInspectionParameter[ iLoopInspection ].dMeasureFixedSizeHeight );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRLowThresh != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRLowThresh )
                        objINI.WriteValue( strSection, strInspection + "iBusbarLRLowThresh", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRLowThresh );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRContinousLen != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRContinousLen )
                        objINI.WriteValue( strSection, strInspection + "iBusbarLRContinousLen", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRContinousLen );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRHighThresh != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRHighThresh )
                        objINI.WriteValue( strSection, strInspection + "iBusbarLRHighThresh", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarLRHighThresh );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBThresh != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBThresh )
                        objINI.WriteValue( strSection, strInspection + "iBusbarTBThresh", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBThresh );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBContinousLen != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBContinousLen )
                        objINI.WriteValue( strSection, strInspection + "iBusbarTBContinousLen", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iBusbarTBContinousLen );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdWid != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdWid )
                        objINI.WriteValue( strSection, strInspection + "iSensorStdWid", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdWid );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdHgt != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdHgt )
                        objINI.WriteValue( strSection, strInspection + "iSensorStdHgt", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorStdHgt );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorThresh != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorThresh )
                        objINI.WriteValue( strSection, strInspection + "iSensorThresh", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.iSensorThresh );

                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMax != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMax )
                        objINI.WriteValue( strSection, strInspection + "dSizeWidthMax", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMax );
                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMin != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMin )
                        objINI.WriteValue( strSection, strInspection + "dSizeWidthMin", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeWidthMin );
                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMax != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMax )
                        objINI.WriteValue( strSection, strInspection + "dSizeHeightMax", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMax );
                    if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMin != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMin )
                        objINI.WriteValue( strSection, strInspection + "dSizeHeightMin", varParameter.objInspectionParameter[ iLoopInspection ].objMeasureParameter.dSizeHeightMin );


                    for ( int iLoopSearchCount = 0; iLoopSearchCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopSearchCount++ )
                    {
                        string strName = "dStartX_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartX != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartX )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartX );

                        strName = "dStartY_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartY != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartY )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dStartY );

                        strName = "dEndX_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndX != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndX )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndX );

                        strName = "dEndY_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndY != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndY )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dEndY );

                        strName = "dRotation_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dRotation != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dRotation )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dRotation );

                        strName = "dSkew_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dSkew != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dSkew )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].dSkew );

                        strName = "MeasureSerchRegion_dStartX_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartX != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartX )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartX );

                        strName = "MeasureSerchRegion_dStartY_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartY != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartY )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dStartY );

                        strName = "MeasureSerchRegion_dEndX_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndX != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndX )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndX );

                        strName = "MeasureSerchRegion_dEndY_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndY != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndY )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dEndY );

                        strName = "MeasureSerchRegion_dRotation_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dRotation != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dRotation )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dRotation );

                        strName = "MeasureSerchRegion_dSkew_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dSkew != varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dSkew )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objMeasureSerchRegion[ iLoopSearchCount ].dSkew );

                        strName = "GradientRegion_dStartX_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartX != varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartX )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartX );

                        strName = "GradientRegion_dStartY_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartY != varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartY )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dStartY );

                        strName = "GradientRegion_dEndX_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndX != varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndX )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndX );

                        strName = "GradientRegion_dEndY_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndY != varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndY )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dEndY );

                        strName = "GradientRegion_dRotation_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dRotation != varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dRotation )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dRotation );

                        strName = "GradientRegion_dSkew_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dSkew != varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dSkew )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objGradientRegion[ iLoopSearchCount ].dSkew );

                        strName = "objFindLineTop_ePolaraty_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].ePolaraty != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].ePolaraty )
                            objINI.WriteValue( strSection, strInspection + strName, (int)varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].ePolaraty );
                        strName = "objFindLineTop_eSerachDirection_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].eSerachDirection != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].eSerachDirection )
                            objINI.WriteValue( strSection, strInspection + strName, ( int )varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].eSerachDirection );
                        strName = "objFindLineTop_iCalipersNumber_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iCalipersNumber != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iCalipersNumber )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iCalipersNumber );
                        strName = "objFindLineTop_iIgnoreNumber_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iIgnoreNumber != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iIgnoreNumber )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iIgnoreNumber );
                        strName = "objFindLineTop_iThreshold_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iThreshold != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iThreshold )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iThreshold );
                        strName = "objFindLineTop_iFilter_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iFilter != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iFilter )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].iFilter );
                        strName = "objFindLineTop_dSearchLength_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].dSearchLength != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].dSearchLength )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineTop[ iLoopSearchCount ].dSearchLength );

                        strName = "objFindLineBottom_ePolaraty_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].ePolaraty != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].ePolaraty )
                            objINI.WriteValue( strSection, strInspection + strName, ( int )varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].ePolaraty );
                        strName = "objFindLineBottom_eSerachDirection_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].eSerachDirection != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].eSerachDirection )
                            objINI.WriteValue( strSection, strInspection + strName, ( int )varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].eSerachDirection );
                        strName = "objFindLineBottom_iCalipersNumber_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iCalipersNumber != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iCalipersNumber )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iCalipersNumber );
                        strName = "objFindLineBottom_iIgnoreNumber_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iIgnoreNumber != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iIgnoreNumber )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iIgnoreNumber );
                        strName = "objFindLineBottom_iThreshold_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iThreshold != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iThreshold )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iThreshold );
                        strName = "objFindLineBottom_iFilter_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iFilter != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iFilter )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].iFilter );
                        strName = "objFindLineBottom_dSearchLength_" + iLoopSearchCount.ToString();
                        if( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].dSearchLength != varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].dSearchLength )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objFindLineBottom[ iLoopSearchCount ].dSearchLength );


                        // 워크스페이스 경로와패스는 레시피파라미터에서 따로 읽었기때문에 값만 가지고 있자
                        varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strWorkSpaceFilePath = varParameter.strVidiWorkSpaceFilePath;
                        varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strWorkSpaceName = varParameter.strVidiWorkSpaceName;
                        // 스트림 및 툴이름, 툴타입은 로딩
                        strName = "strStreamName_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strStreamName != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strStreamName )
                            objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strStreamName );

                        strName = "strToolName_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strToolName != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strToolName )
                        objINI.WriteValue( strSection, strInspection + strName, varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.strToolName );

                        strName = "eVidiType_" + iLoopSearchCount.ToString();
                        if ( varParameterOrigin.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.eVidiType != varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.eVidiType )
                            objINI.WriteValue( strSection, strInspection + strName, ( int )varParameter.objInspectionParameter[ iLoopInspection ].objVidiSerchRegion[ iLoopSearchCount ].objVidiParameter.eVidiType );
                    }
                }

                m_objRecipeParameter[ iCameraIndex ] = ( CRecipeParameter )objRecipeParameter.Clone();
                bReturn = true;
			} while( false );

			return bReturn;
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadCameraParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_CAMERA_INI );
				ClassINI objINI = new ClassINI( strPath );

				for( int iLoopCount = 0; iLoopCount < m_objCameraParameter.Length; iLoopCount++ ) {
					string strSection = ( ( CDefine.enumCamera )iLoopCount ).ToString();
					var varParameter = m_objCameraParameter[ iLoopCount ];

					varParameter.eUseCameraType = ( CCameraParameter.enumUseCameraType )objINI.GetInt32( strSection, "eUseCameraType", ( int )CCameraParameter.enumUseCameraType.USE_REAL_CAMERA_TYPE );
					varParameter.iCameraIndex = objINI.GetInt32( strSection, "iCameraIndex", iLoopCount );
					varParameter.strCameraSerialNumber = objINI.GetString( strSection, "strCameraSerialNumber", "123" );
                    varParameter.strIPAddress = objINI.GetString( strSection, "strIPAddress", "192.168.1.10" );
                    varParameter.dResolution = objINI.GetDouble( strSection, "dResolution", 1 );
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveCameraParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_CAMERA_INI );
				ClassINI objINI = new ClassINI( strPath );

				for( int iLoopCount = 0; iLoopCount < m_objCameraParameter.Length; iLoopCount++ ) {
					string strSection = ( ( CDefine.enumCamera )iLoopCount ).ToString();
					var varParameter = m_objCameraParameter[ iLoopCount ];

					objINI.WriteValue( strSection, "eUseCameraType", ( int )varParameter.eUseCameraType );
					objINI.WriteValue( strSection, "iCameraIndex", varParameter.iCameraIndex );
					objINI.WriteValue( strSection, "strCameraSerialNumber", varParameter.strCameraSerialNumber );
                    objINI.WriteValue( strSection, "strIPAddress", varParameter.strIPAddress );
                    objINI.WriteValue( strSection, "dResolution", varParameter.dResolution );
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveCameraParameter( int iCameraIndex, CCameraParameter objCameraParameter )
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_CAMERA_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = ( ( CDefine.enumCamera )iCameraIndex ).ToString();
				// 원본 대상
				var varParameterOrigin = m_objCameraParameter[ iCameraIndex ];
				// 적용 대상
				var varParameter = objCameraParameter;

				if( varParameterOrigin.eUseCameraType != varParameter.eUseCameraType ) {
					objINI.WriteValue( strSection, "eUseCameraType", ( int )varParameter.eUseCameraType );
				}
				if( varParameterOrigin.iCameraIndex != varParameter.iCameraIndex ) {
					objINI.WriteValue( strSection, "iCameraIndex", varParameter.iCameraIndex );
				}
				if( varParameterOrigin.strCameraSerialNumber != varParameter.strCameraSerialNumber ) {
					objINI.WriteValue( strSection, "strCameraSerialNumber", varParameter.strCameraSerialNumber );
				}
                if( varParameterOrigin.strIPAddress != varParameter.strIPAddress ) {
                    objINI.WriteValue( strSection, "strIPAddress", varParameter.strIPAddress );
                }
                if( varParameterOrigin.dResolution != varParameter.dResolution ) {
					objINI.WriteValue( strSection, "dResolution", varParameter.dResolution );
				}
				
				m_objCameraParameter[ iCameraIndex ] = ( CCameraParameter )objCameraParameter.Clone();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : IO 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadIOParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "IO";
				var varParameter = m_objIOInitializeParameter;

				varParameter.eDeviceCardType = ( HLDevice.Abstract.CDeviceIOAbstract.enumDeviceCardType )objINI.GetInt32( strSection, "eDeviceCardType", ( int )HLDevice.Abstract.CDeviceIOAbstract.enumDeviceCardType.PCI_7230 );
				varParameter.iCountDeviceCard = objINI.GetInt32( strSection, "iCountDeviceCard", 2 );
				varParameter.iInputModuleCount = objINI.GetInt32( strSection, "iInputModuleCount", 1 );
				varParameter.iOutPutModuleCount = objINI.GetInt32( strSection, "iOutPutModuleCount", 1 );
				varParameter.iAnalogInputModuleCount = objINI.GetInt32( strSection, "iAnalogInputModuleCount", 0 );
				varParameter.iAnalogOutputModuleCount = objINI.GetInt32( strSection, "iAnalogOutputModuleCount", 0 );

				// IO 맵 로드
				LoadIOMap();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : IO 맵 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool LoadIOMap()
		{
			bool bReturn = false;

			do {
				// IO MapData 정보 로드
				string strPath = GetINIPath();
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}", GetINIPath(), CDefine.DEF_IO_DAT );
				if( false == File.Exists( strPath ) ) {
					StreamWriter obj = File.CreateText( strPath );
					obj.Dispose();
				}
				FileStream fs = File.Open( strPath, FileMode.Open );
				StreamReader sr = new StreamReader( fs, Encoding.Default );

				while( !sr.EndOfStream ) {
					string strData = sr.ReadLine();
					strData.Trim();
					strData = strData.Replace( "\t", "" );
					string[] strIOParameter = strData.Split( ',' );

					CIOInitializeParameter.CIOParameter IOParameter = new CIOInitializeParameter.CIOParameter();
					IOParameter.strIndex = strIOParameter[ 0 ];
					IOParameter.strAddress = strIOParameter[ 1 ];
					IOParameter.strIOName = strIOParameter[ 2 ];
					IOParameter.eIOType = ( CIOInitializeParameter.enumIOType )Convert.ToInt32( strIOParameter[ 3 ] );
					IOParameter.iDeviceCardID = Convert.ToInt32( strIOParameter[ 4 ] );
					m_objIOInitializeParameter.objIOParameter.Add( IOParameter.strIOName, IOParameter );

					if( CIOInitializeParameter.enumIOType.IO_TYPE_DI == IOParameter.eIOType || CIOInitializeParameter.enumIOType.IO_TYPE_AI == IOParameter.eIOType ) {
						m_objIOInitializeParameter.objListIONameInput.Add( IOParameter.strIOName );
						m_objIOInitializeParameter.objListIOAddressInput.Add( IOParameter.strIndex );
					}
					else if( CIOInitializeParameter.enumIOType.IO_TYPE_DO == IOParameter.eIOType || CIOInitializeParameter.enumIOType.IO_TYPE_AO == IOParameter.eIOType ) {
						m_objIOInitializeParameter.objListIONameOutput.Add( IOParameter.strIOName );
						m_objIOInitializeParameter.objListIOAddressOutput.Add( IOParameter.strIndex );
					}
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : IO 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveIOParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "IO";
				var varParameter = m_objIOInitializeParameter;

				objINI.WriteValue( strSection, "eDeviceCardType", ( int )varParameter.eDeviceCardType );
				objINI.WriteValue( strSection, "iCountDeviceCard", varParameter.iCountDeviceCard );
				objINI.WriteValue( strSection, "iInputModuleCount", varParameter.iInputModuleCount );
				objINI.WriteValue( strSection, "iOutPutModuleCount", varParameter.iOutPutModuleCount );
				objINI.WriteValue( strSection, "iAnalogInputModuleCount", varParameter.iAnalogInputModuleCount );
				objINI.WriteValue( strSection, "iAnalogOutputModuleCount", varParameter.iAnalogOutputModuleCount );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : PLC 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadPLCParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "PLC";
				var varParameter = m_objPLCInitializeParameter;

				varParameter.ePLCProtocolType = ( CPLCInitializeParameter.enumPLCProtocolType )objINI.GetInt32( strSection, "enumPLCProtocolType", 0 );
                varParameter.ePLCType = (CPLCInitializeParameter.enumPLCType)objINI.GetInt32(strSection, "enumPLCProtocolType", 1);
                varParameter.iSocketPortNumber = objINI.GetInt32( strSection, "iSocketPortNumber", 3000 );
				varParameter.strSocketIPAddress = objINI.GetString( strSection, "strSocketIPAddress", "127.0.0.1" );
				varParameter.dMultiple = objINI.GetDouble( strSection, "dMultiple", 10000 );

				// PLC 맵 로드
				LoadPLCMap();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : PLC 맵 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool LoadPLCMap()
		{
			bool bReturn = false;

			do {
				// PLC MapData 정보 로드
				string strPath = GetINIPath();
				if( false == Directory.Exists( strPath ) ) {
					// 폴더 생성
					Directory.CreateDirectory( strPath );
				}
				strPath = string.Format( @"{0}\{1}", GetINIPath(), CDefine.DEF_PLC_DAT );
				if( false == File.Exists( strPath ) ) {
					StreamWriter obj = File.CreateText( strPath );
					obj.Dispose();
				}
				FileStream fs = File.Open( strPath, FileMode.Open );
				StreamReader sr = new StreamReader( fs, Encoding.Default );

				int iInputCountAll = 0;
				int iOutputCountAll = 0;
				int iInputCountBit = 0;
				int iInputCountWord = 0;
				int iInputCountDWord = 0;
				int iOutputCountBit = 0;
				int iOutputCountWord = 0;
				int iOutputCountDWord = 0;

				while( !sr.EndOfStream ) {
					string strData = sr.ReadLine();
					strData.Trim();
					strData = strData.Replace( "\t", "" );
					string[] strPLCParameter = strData.Split( ',' );

					CPLCInitializeParameter.CPLCParameter PLCParameter = new CPLCInitializeParameter.CPLCParameter();
                    // 설비 A,B 중 B설비는 PLC어드레스가 200만큼 뒤에있음
                    // 주소 파싱할때만 문자열을 숫자로 바꿨다가 다시 문자열로 바꿉시다
                    string strAddressType = strPLCParameter[ 0 ].Substring( 0, 1 );
                    string strAddress = strPLCParameter[ 0 ].Substring( 1, strPLCParameter[ 0 ].Length-1 );
                    if( CDefine.enumMachinePosition.POSITION_B == m_objSystemParameter.eMachinePosition )
                        strAddress = ( Int32.Parse( strAddress ) + m_objSystemParameter.iOffsetAddressPLC ).ToString();

                    PLCParameter.strAddress = strAddressType + strAddress;
                    //PLCParameter.strAddress = strPLCParameter[ 0 ];
                    PLCParameter.strIndex = strPLCParameter[ 1 ];
					PLCParameter.iInOutIndex = Convert.ToInt32( strPLCParameter[ 1 ] );

                    // 이름은 어떻하지?ㅡㅡ
                    // 안쓰는곳은 RESERVE로 사용하기로 하고 RESERVE일 경우 실 주소를 표기하자
                    if ( 0 > strPLCParameter[ 2 ].IndexOf( "RESERVE" ) )
					    PLCParameter.strPLCName = strPLCParameter[ 2 ];
                    else
                        PLCParameter.strPLCName = PLCParameter.strAddress;

                    if ( "BIT_IN" == strPLCParameter[ 3 ] ) {
						PLCParameter.eCommunicationType = CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN;
						m_objPLCInitializeParameter.strPLCInput.Add( PLCParameter.strPLCName );
						iInputCountAll++; iInputCountBit++;
					}
					else if( "WORD_IN" == strPLCParameter[ 3 ] ) {
						PLCParameter.eCommunicationType = CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN;
						m_objPLCInitializeParameter.strPLCInput.Add( PLCParameter.strPLCName );
						iInputCountAll++; iInputCountWord++;
					}
					else if( "DWORD_IN" == strPLCParameter[ 3 ] ) {
						PLCParameter.eCommunicationType = CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN;
						m_objPLCInitializeParameter.strPLCInput.Add( PLCParameter.strPLCName );
						iInputCountAll++; iInputCountDWord++;
					}
					else if( "BIT_OUT" == strPLCParameter[ 3 ] ) {
						PLCParameter.eCommunicationType = CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_OUT;
						m_objPLCInitializeParameter.strPLCOutput.Add( PLCParameter.strPLCName );
						iOutputCountAll++; iOutputCountBit++;
					}
					else if( "WORD_OUT" == strPLCParameter[ 3 ] ) {
						PLCParameter.eCommunicationType = CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT;
						m_objPLCInitializeParameter.strPLCOutput.Add( PLCParameter.strPLCName );
						iOutputCountAll++; iOutputCountWord++;
					}
					else if( "DWORD_OUT" == strPLCParameter[ 3 ] ) {
						PLCParameter.eCommunicationType = CPLCInitializeParameter.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_OUT;
						m_objPLCInitializeParameter.strPLCOutput.Add( PLCParameter.strPLCName );
						iOutputCountAll++; iOutputCountDWord++;
					}
					m_objPLCInitializeParameter.objPLCParameter.Add( PLCParameter.strPLCName, PLCParameter );
				}
				m_objPLCInitializeParameter.iInputCountAll = iInputCountAll;
				m_objPLCInitializeParameter.iOutputCountAll = iOutputCountAll;
				m_objPLCInitializeParameter.iInputCountBit = iInputCountBit;
				m_objPLCInitializeParameter.iInputCountWord = iInputCountWord;
				m_objPLCInitializeParameter.iInputCountDWord = iInputCountDWord;
				m_objPLCInitializeParameter.iOutputCountBit = iOutputCountBit;
				m_objPLCInitializeParameter.iOutputCountWord = iOutputCountWord;
				m_objPLCInitializeParameter.iOutputCountDWord = iOutputCountDWord;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : PLC 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SavePLCParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "PLC";
				var varParameter = m_objPLCInitializeParameter;

				objINI.WriteValue( strSection, "enumPLCProtocolType", ( int )varParameter.ePLCProtocolType );
                objINI.WriteValue(strSection, "enumPLCType", (int)varParameter.ePLCType);
                objINI.WriteValue( strSection, "iSocketPortNumber", varParameter.iSocketPortNumber );
				objINI.WriteValue( strSection, "strSocketIPAddress", varParameter.strSocketIPAddress );
				objINI.WriteValue( strSection, "dMultiple", varParameter.dMultiple );

				bReturn = true;
			} while( false );

			return bReturn;
		}

      
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 조명 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool LoadLightControllerParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumLightController.LIGHT_CONTROLLER_FINAL; iLoopCount++ ) {
					string strSection = ( ( CDefine.enumLightController )iLoopCount ).ToString();
					var varParameter = m_objLightControllerParameter[ iLoopCount ];

					varParameter.eType = ( CLightControllerParameter.enumType )objINI.GetInt32( strSection, "eType", 1 );
					varParameter.strSocketIPAddress = objINI.GetString( strSection, "strSocketIPAddress", "" );
					varParameter.iSocketPortNumber = objINI.GetInt32( strSection, "iSocketPortNumber", 0 );
					varParameter.strSerialPortName = objINI.GetString( strSection, "strSerialPortName", "COM26" );
					varParameter.iSerialPortBaudrate = objINI.GetInt32( strSection, "iSerialPortBaudrate", 9600 );
					varParameter.iSerialPortDataBits = objINI.GetInt32( strSection, "iSerialPortDataBits", 8 );
					varParameter.eParity = ( CLightControllerParameter.enumSerialPortParity )objINI.GetInt32( strSection, "eParity", 0 );
					varParameter.eStopBits = ( CLightControllerParameter.enumSerialPortStopBits )objINI.GetInt32( strSection, "eStopBits", 1 );
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 조명 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SaveLightControllerParameter()
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumLightController.LIGHT_CONTROLLER_FINAL; iLoopCount++ ) {
					string strSection = ( ( CDefine.enumLightController )iLoopCount ).ToString();
					var varParameter = m_objLightControllerParameter[ iLoopCount ];

					objINI.WriteValue( strSection, "eType", ( int )varParameter.eType );
					objINI.WriteValue( strSection, "strSocketIPAddress", varParameter.strSocketIPAddress );
					objINI.WriteValue( strSection, "iSocketPortNumber", varParameter.iSocketPortNumber );
					objINI.WriteValue( strSection, "strSerialPortName", varParameter.strSerialPortName );
					objINI.WriteValue( strSection, "iSerialPortBaudrate", varParameter.iSerialPortBaudrate );
					objINI.WriteValue( strSection, "iSerialPortDataBits", varParameter.iSerialPortDataBits );
					objINI.WriteValue( strSection, "eParity", ( int )varParameter.eParity );
					objINI.WriteValue( strSection, "eStopBits", ( int )varParameter.eStopBits );
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 조명 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool SaveLightControllerParameter( CLightControllerParameter objLightControllerParameter, CDefine.enumLightController eLightController )
		{
			bool bReturn = false;

			do {
				string strPath = string.Format( @"{0}\{1}", m_strItemPath, CDefine.DEF_DEVICE_INI );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = eLightController.ToString();
				// 원본 대상
				var varParameterOrigin = m_objLightControllerParameter[ ( int )eLightController ];
				// 적용 대상
				var varParameter = objLightControllerParameter;

				if( varParameterOrigin.eType != varParameter.eType ) {
					objINI.WriteValue( strSection, "eType", ( int )varParameter.eType );
				}
				if( varParameterOrigin.strSocketIPAddress != varParameter.strSocketIPAddress ) {
					objINI.WriteValue( strSection, "strSocketIPAddress", varParameter.strSocketIPAddress );
				}
				if( varParameterOrigin.iSocketPortNumber != varParameter.iSocketPortNumber ) {
					objINI.WriteValue( strSection, "iSocketPortNumber", varParameter.iSocketPortNumber );
				}
				if( varParameterOrigin.strSerialPortName != varParameter.strSerialPortName ) {
					objINI.WriteValue( strSection, "strSerialPortName", varParameter.strSerialPortName );
				}
				if( varParameterOrigin.iSerialPortBaudrate != varParameter.iSerialPortBaudrate ) {
					objINI.WriteValue( strSection, "iSerialPortBaudrate", varParameter.iSerialPortBaudrate );
				}
				if( varParameterOrigin.iSerialPortDataBits != varParameter.iSerialPortDataBits ) {
					objINI.WriteValue( strSection, "iSerialPortDataBits", varParameter.iSerialPortDataBits );
				}
				if( varParameterOrigin.eParity != varParameter.eParity ) {
					objINI.WriteValue( strSection, "eParity", ( int )varParameter.eParity );
				}
				if( varParameterOrigin.eStopBits != varParameter.eStopBits ) {
					objINI.WriteValue( strSection, "eStopBits", ( int )varParameter.eStopBits );
				}	

				m_objLightControllerParameter[ ( int )eLightController ] = ( CLightControllerParameter )objLightControllerParameter.Clone();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 시스템 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CSystemParameter GetSystemParameter()
		{
			return ( CSystemParameter )m_objSystemParameter.Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDatabaseParameter GetDatabaseParameter()
		{
			return ( CDatabaseParameter )m_objDatabaseParameter.Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 정보 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CRecipeInformation GetRecipeInformation()
		{
			return ( CRecipeInformation )m_objRecipeInformation.Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CRecipeParameter GetRecipeParameter( int iCameraIndex )
		{
			return ( CRecipeParameter )m_objRecipeParameter[ iCameraIndex ].Clone();
		}
	
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CCameraParameter GetCameraParameter( int iCameraIndex )
		{
			return ( CCameraParameter )m_objCameraParameter[ iCameraIndex ].Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : IO 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CIOInitializeParameter GetIOParameter()
		{
			return ( CIOInitializeParameter )m_objIOInitializeParameter.Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : PLC 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CPLCInitializeParameter GetPLCParameter()
		{
			return ( CPLCInitializeParameter )m_objPLCInitializeParameter.Clone();
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 조명 파라미터 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CLightControllerParameter GetLightControllerParameter( CDefine.enumLightController eLightController )
		{
			return ( CLightControllerParameter )m_objLightControllerParameter[ ( int )eLightController ].Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 프로그램 파일 경로 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetCurrentPath()
		{
			return m_strCurrentPath;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 아이템 경로 리턴 (아이템 폴더 위치까지 경로 리턴)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetItemPath()
		{
			return m_strItemPath;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 경로 리턴 (레시피 폴더 위치까지 경로 리턴)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetRecipePath()
		{
			return m_strRecipePath;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그 경로 리턴 (로그 폴더 위치까지 경로 리턴)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetLogPath()
		{
			return string.Format( @"{0}\LOG", m_strItemPath );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : INI 경로 리턴 (INI 폴더 위치까지 경로 리턴)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetINIPath()
		{
			return string.Format( @"{0}\Device\{1}\Dat", m_strCurrentPath, m_objSystemParameter.eMachineType.ToString() );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 테이블 파일 경로 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetDatabaseTablePath()
		{
			return string.Format( @"{0}\DatabaseTable", m_strCurrentPath );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 레코드 파일 경로 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetDatabaseRecordPath()
		{
			return string.Format( @"{0}\DatabaseRecord", m_strCurrentPath );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 이력 파일 경로 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetDatabaseHistoryPath()
		{
			return string.Format( @"{0}\Database", m_strItemPath );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 변수 명 string으로 받음
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string GetVariableName<T>( System.Linq.Expressions.Expression<Func<T>> expression )
		{
			// 바깥에서 람다 사용법
			// var pCommon = Common.CFormCommon.GetFormCommon;
			// string strName = "";
			// string stringTest = "";
			// strName = pCommon.GetVariableName( () => stringTest );
			var body = expression.Body as System.Linq.Expressions.MemberExpression;
			return body.Member.Name;
		}
	}
}