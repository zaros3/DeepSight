using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
	// 검사 결과에 사용되는 구조체만 묶어서 관리
	public class CInspectionResult
	{
		// 공통 검사 결과
		public struct structureResultCommon
		{
            // 검사 결과
            public int iInspectionPosition;
			public CDefine.enumResult eResult;
            public string strCellID;
			// 트리거 시간
			public string strTriggerTime;
			// 택타임
			public string strTactTime;
			// 코그 원본 이미지
			public Cognex.VisionPro.CogImage8Grey objInputGrabImage;
            // PMS에 넣을 원본이미지
            public List<Cognex.VisionPro.CogImage8Grey> objInputGrabOriginImage;
            public List<Bitmap> objInputGrabOriginalImageBitmap;
            //VIDI에 사용되는 원본이미지
            public List<Cognex.VisionPro.CogImage8Grey> objInputGrabOriginImageVIDI;
            public List<Bitmap> objInputGrabOriginalImageBitmapVIDI;
            //검사에 사용되는 원본이미지
            public List<Cognex.VisionPro.CogImage8Grey> objInputGrabOriginImageMeasure;
            public List<Bitmap> objInputGrabOriginalImageBitmapMeasure;


            // PMS 아웃
            public List<Bitmap> objPMSImageBitmap;
            public List<Cognex.VisionPro.CogImage8Grey> objPMSImage;
            // 검사용 PMS이미지가 또 필요하다함
            public List<Bitmap> objPMSImageBitmapMeasure;
            public List<Cognex.VisionPro.CogImage8Grey> objPMSImageMeasure;


            // 비디에 넣을 자른 이미지
            public List<Cognex.VisionPro.CogImage8Grey> objCropImageVidi;
            public List<Bitmap> objCropImageBitmapVidi;

            // 치수측정관련
            public struct structureDataRegionMeasure {
                public double dStartX;
                public double dStartY;
                public double dEndX;
                public double dEndY;
            }

            public struct structureDataLineMeasure {
                public double dStartX;
                public double dStartY;
                public double dEndX;
                public double dEndY;
                public double dDistance;
            }

            public int iMeasureResultCount;
            public List<Cognex.VisionPro.CogImage8Grey> objCropImageMeasure;
            public List<Bitmap> objCropImageBitmapMeasure;
            public List<Cognex.VisionPro.CogImage8Grey> objCropImageMeasureSecond;
            public List<Bitmap> objCropImageBitmapMeasureSecond;
            public List<double> dListMeasureSizeWidth;
            public List<double> dListMeasureSizeHeight;
            public List<structureDataRegionMeasure> objMeasureRegion;
            public List<structureDataLineMeasure> objMeasureLine;
            // 비디 결과 및 이미지
            //비디 동작카운트
            public int iVidiResultCount;
            public List<CDefine.enumResult> objVidiResult;
            public List<Cognex.VisionPro.CogImage8Grey> objVidiResultImage;
            public List<Cognex.VisionPro.CogImage8Grey> objVidiResultOverlayGraphic;
            public List<double> objVidiScore;
            public List<string> objVidiTactTime;
            public List<string> objMeasureTactTime;
            // 디스플레이할때 이미지 인덱스
            public int iIndexDisplayOriginalImage;
            // 코그 그랩 이미지
            public Cognex.VisionPro.CogImage8Grey objOutputGrabImage;


            // 검사결과
            public List<CDefine.enumResult> objMeasureResult;
            public List<CDefine.enumResult> objMeasureResult3DWidth;

            //3D센서 데이터
            public List<int> obj3DListHeightOverBlobCountHigh;
            public List<int> obj3DListHeightOverBlobCountLow;

            public List<double> obj3DListWeldWidth;
            public List<double>[] obj3DListSampleHeightData;

            public struct structureDataRegion3D {
                public double dStartX;
                public double dStartY;
                public double dEndX;
                public double dEndY;

                public List<int> obj3DListNGPositionX;
                public List<int> obj3DListNGPositionY;
            }

            // 높이 해상도
            public int i3DResolutionX;
            public int i3DResolutionY;
            public int i3DResolutionZ;
            public int i3DOffsetZ;
            // 원본데이터
            public int i3DImageWidth;
            public int i3DImageHeight;
            public short[] obj3DDataHeightOrigin;
            public byte[] obj3DDataIntensityOrigin;

            // 코그이미지
            public Cognex.VisionPro.CogImage8Grey obj3DImageHeight;
            public Cognex.VisionPro.CogImage8Grey obj3DImageIntensity;
            public List<Cognex.VisionPro.CogImage8Grey> obj3DImage;
            public List<Bitmap> obj3DImageBitmap;
            // 자른데이터
            // 테두리부분 쓰레기데이터 삭제
            public double[] obj3DDataHeightCrop1d;
            public double[,] obj3DDataHeightCrop2d;
            public int[,] obj3DDataHeight2dOriginal;
            public byte[] obj3DDataIntensityCrop;

            public List<structureDataRegion3D> obj3DRegionData;
            public List<double[,]> obj3DResultHeightData;
            public List<Cognex.VisionPro.CogImage8Grey> obj3DResultOverlayGraphic;
            public List<int> obj3DHeghtThresholdOverCount;
            // 비트맵이미지
            public Bitmap obj3DImageHeightBitmap;
            public Bitmap obj3DImageIntensityBitmap;

            // 패턴좌표
            public double dPatternPositionX;
            public double dPatternPositionY;

            //3D Measure 검사 영역 정보 수집용
            public List<List<Rectangle>>    objMeasureAreaRect;
            public List<List<double>>       objMeasureAreaValue;

            public void Init()
			{
                iInspectionPosition = 0;
                eResult = CDefine.enumResult.RESULT_NG;
                strCellID = "";
                strTriggerTime = "";
				strTactTime = "";
				objInputGrabImage = null;
				objOutputGrabImage = null;
                objVidiResult = new List<CDefine.enumResult>();
                objMeasureResult = new List<CDefine.enumResult>();
                objMeasureResult3DWidth = new List<CDefine.enumResult>();
                objPMSImage = new List<Cognex.VisionPro.CogImage8Grey>();
                objPMSImageMeasure = new List<Cognex.VisionPro.CogImage8Grey>();
                objInputGrabOriginImage = new List<Cognex.VisionPro.CogImage8Grey>();
                objInputGrabOriginalImageBitmap = new List<Bitmap>();
                objInputGrabOriginImageVIDI = new List<Cognex.VisionPro.CogImage8Grey>();
                objInputGrabOriginalImageBitmapVIDI = new List<Bitmap>();
                objInputGrabOriginImageMeasure = new List<Cognex.VisionPro.CogImage8Grey>();
                objInputGrabOriginalImageBitmapMeasure = new List<Bitmap>();
                objPMSImageBitmap = new List<Bitmap>();
                objPMSImageBitmapMeasure = new List<Bitmap>();
                objCropImageVidi = new List<Cognex.VisionPro.CogImage8Grey>();
                objCropImageBitmapVidi = new List<Bitmap>();
                objCropImageMeasure = new List<Cognex.VisionPro.CogImage8Grey>();
                objCropImageBitmapMeasure = new List<Bitmap>();
                objCropImageMeasureSecond = new List<Cognex.VisionPro.CogImage8Grey>();
                objCropImageBitmapMeasureSecond = new List<Bitmap>();
                dListMeasureSizeWidth = new List<double>();
                dListMeasureSizeHeight = new List<double>();
                objMeasureRegion = new List<structureDataRegionMeasure>();
                objMeasureLine = new List<structureDataLineMeasure>();
                objVidiResultImage = new List<Cognex.VisionPro.CogImage8Grey>();
                objVidiResultOverlayGraphic = new List<Cognex.VisionPro.CogImage8Grey>();
                obj3DResultOverlayGraphic = new List<Cognex.VisionPro.CogImage8Grey>();
                objMeasureAreaRect = new List<List<Rectangle>>();
                objMeasureAreaValue = new List<List<double>>();
                objVidiScore = new List<double>();
                objVidiTactTime = new List<string>();
                objMeasureTactTime = new List<string>();

                i3DImageWidth = 0;
                i3DImageHeight = 0;
                i3DResolutionX = 0;
                i3DResolutionY = 0;
                i3DResolutionZ = 0;
                i3DOffsetZ = 0;
                obj3DHeghtThresholdOverCount = new List<int>();
                obj3DImageHeight = new Cognex.VisionPro.CogImage8Grey();
                obj3DImageIntensity = new Cognex.VisionPro.CogImage8Grey();
                obj3DImage = new List<Cognex.VisionPro.CogImage8Grey>();
                obj3DImageBitmap = new List<Bitmap>();
                obj3DImageHeightBitmap = new Bitmap( 10, 10 );
                obj3DImageIntensityBitmap = new Bitmap( 10, 10 );
                obj3DRegionData = new List<structureDataRegion3D>();
                obj3DResultHeightData = new List<double[,]>();
                obj3DListHeightOverBlobCountHigh = new List<int>();
                obj3DListHeightOverBlobCountLow = new List<int>();
                obj3DListWeldWidth = new List<double>();
                obj3DListSampleHeightData = new List<double>[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dPatternPositionX = 0;
                dPatternPositionY = 0;

                for ( int iLoopCount = 0; iLoopCount < 4; iLoopCount++ )
                {
                    objInputGrabOriginImage.Add( new Cognex.VisionPro.CogImage8Grey() );
                    objInputGrabOriginalImageBitmap.Add( new Bitmap( 10, 10 ) );
                    objInputGrabOriginImageVIDI.Add( new Cognex.VisionPro.CogImage8Grey() );
                    objInputGrabOriginalImageBitmapVIDI.Add( new Bitmap( 10, 10 ) );
                    objInputGrabOriginImageMeasure.Add( new Cognex.VisionPro.CogImage8Grey() );
                    objInputGrabOriginalImageBitmapMeasure.Add( new Bitmap( 10, 10 ) );
                    objPMSImage.Add( new Cognex.VisionPro.CogImage8Grey() );
                    objPMSImageBitmap.Add( new Bitmap( 10, 10 ) );
                    objPMSImageMeasure.Add( new Cognex.VisionPro.CogImage8Grey() );
                    objPMSImageBitmapMeasure.Add( new Bitmap( 10, 10 ) );
                    obj3DImage.Add( new Cognex.VisionPro.CogImage8Grey() );
                    obj3DImageBitmap.Add( new Bitmap( 10, 10 ) );
                }

                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    structureDataRegion3D objData = new structureDataRegion3D();
                    objData.obj3DListNGPositionX = new List<int>();
                    objData.obj3DListNGPositionY = new List<int>();
                    obj3DListSampleHeightData[ iLoopCount ] = new List<double>();
                    obj3DRegionData.Add( objData );
                }

                iIndexDisplayOriginalImage = 0;

            }
		}

		
		public class CResult : ICloneable
		{
			// 공통 검사 결과
			public structureResultCommon objResultCommon = new structureResultCommon();
			// 이미지 저장 경로
			public string strSaveImagePath;
			// 검색 영역
			public Cognex.VisionPro.CogRectangle objGraphicsSearchRegion;
			// 패턴 결과 그래픽
			public List<Cognex.VisionPro.ICogRecord> objGraphicsPMAlign;

			public CResult()
			{
				objResultCommon.Init();
                strSaveImagePath = "";
				objGraphicsSearchRegion = new Cognex.VisionPro.CogRectangle();
				objGraphicsPMAlign = new List<Cognex.VisionPro.ICogRecord>();
			}

			public object Clone()
			{
				CResult objResult = new CResult();
				objResult.objResultCommon = this.objResultCommon;
				objResult.strSaveImagePath = this.strSaveImagePath;
				objResult.objGraphicsSearchRegion = this.objGraphicsSearchRegion;
				objResult.objGraphicsPMAlign = this.objGraphicsPMAlign;
				return objResult;
			}
		}

		// 비전 얼라인 보정량 객체
		public class CInspectionHistoryData : ICloneable
		{
            public string strDateTime;
            public string strCellID;
            public int iPosition;
            public int iTactTime;
            public string strResult;
            public string strNgType;

            public double[] dVidiScore;
            public string[] strVidiResult;

            public double[] dMeasureWidth;
            public double[] dMeasureHeight;
            public string[] strMeasureResult;

            public string[] str3dHeightResult;

            public double[] dMeasureStartX;
            public double[] dMeasureStartY;
            public double[] dMeasureEndX;
            public double[] dMeasureEndY;

            public int iMeasureLineFindCount;
            public double dPatternPositionX;
            public double dPatternPositionY;

            public string strImagePath;

            public CInspectionHistoryData()
            {
                strDateTime = "";
                strCellID = "";
                iPosition = 0;
                iTactTime = 0;
                strResult = "";
                strNgType = "";
                dVidiScore = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                strVidiResult = new string[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                dMeasureWidth = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureHeight = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                strMeasureResult = new string[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                dMeasureStartX = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureStartY = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureEndX = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureEndY = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                str3dHeightResult = new string[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    strVidiResult[ iLoopCount ] = "";
                    strMeasureResult[ iLoopCount ] = "";
                    str3dHeightResult[ iLoopCount ] = "";
                }

                iMeasureLineFindCount = 0;
                dPatternPositionX = 0;
                dPatternPositionY = 0;

                strImagePath = "";
        }

            public void Clear()
			{
                strDateTime = "";
                strCellID = "";
                iPosition = 0;
                iTactTime = 0;
                strResult = "";
                strNgType = "";
                dVidiScore = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                strVidiResult = new string[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                dMeasureWidth = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureHeight = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                strMeasureResult = new string[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                dMeasureStartX = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureStartY = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureEndX = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                dMeasureEndY = new double[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                str3dHeightResult = new string[ CDefine.DEF_MAX_COUNT_CROP_REGION ];

                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    strVidiResult[ iLoopCount ] = "";
                    strMeasureResult[ iLoopCount ] = "";
                    str3dHeightResult[ iLoopCount ] = "";
                }

                iMeasureLineFindCount = 0;
                dPatternPositionX = 0;
                dPatternPositionY = 0;

                strImagePath = "";
            }

			public object Clone()
			{
                CInspectionHistoryData objRevisionData = new CInspectionHistoryData();

                objRevisionData.strDateTime = strDateTime;
                objRevisionData.strCellID = strCellID;
                objRevisionData.iPosition = iPosition;
                objRevisionData.iTactTime = iTactTime;
                objRevisionData.strResult = strResult;
                objRevisionData.strNgType = strNgType;

                objRevisionData.dVidiScore = dVidiScore.Clone() as double[];
                objRevisionData.strVidiResult = strVidiResult.Clone() as string[];

                objRevisionData.dMeasureWidth = dMeasureWidth.Clone() as double[];
                objRevisionData.dMeasureHeight = dMeasureHeight.Clone() as double[];
                objRevisionData.strMeasureResult = strMeasureResult.Clone() as string[];

                objRevisionData.dMeasureStartX = dMeasureStartX.Clone() as double[];
                objRevisionData.dMeasureStartY = dMeasureStartY.Clone() as double[];
                objRevisionData.dMeasureEndX = dMeasureEndX.Clone() as double[];
                objRevisionData.dMeasureEndY = dMeasureEndY.Clone() as double[];

                objRevisionData.str3dHeightResult = str3dHeightResult.Clone() as string[];

                objRevisionData.iMeasureLineFindCount = iMeasureLineFindCount;
                objRevisionData.dPatternPositionX = dPatternPositionX;
                objRevisionData.dPatternPositionY = dPatternPositionY;

                objRevisionData.strImagePath = strImagePath;

                return objRevisionData;
			}
		}
	}
}
