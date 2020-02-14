using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using System.Diagnostics;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;

namespace DeepSight {
    public partial class CFormDisplay : Form {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // enumeration
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 화면 라인 ( 수평, 수직 )
        private enum enumDisplayLine { HORIZON_LINE = 0, VERTICAL_LINE, DISPLAY_LINE_FINAL }
        private enum enumHistogramResult { RESULT_MIN, RESULT_MAX, RESULT_MEAN }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 코그 그래픽 라벨 결과 ok, ng
        private CogGraphicLabel m_objLabelResult;
        // 코그 화면 라인
        private CogLine[] m_objCogDisplayLine;
        // 카메라 인덱스
        public int m_iCameraIndex;
        // Image 인덱스
        public int m_iImageIndex;
        // 디스플레이 인덱스
        // 같은카메라로 여러장의 이미지 처리때문에 필요
        public int m_iDisplayIndex;
        // 길이 측정
        private CogGraphicLabel m_objLabelGuage;
        private CogRectangleAffine m_objRectangleGuage;
        private bool m_bUseGuage;

        // 매뉴얼 얼라인
        // 메뉴창
        private ContextMenuStrip m_objPopupMenu = new ContextMenuStrip();
        // 언어
        private CDefine.enumLanguage m_eLanguage;
        // 디스플레이 락
        private object m_objDisplayLock = new object();
        // 이코드는 여기서만 사용합시다
        // IDTool 검색영역
        private CogGraphicLabel[] m_objLabelIndexSerchRegion;
        private CogRectangleAffine[] m_objRectangleSerchRegion;

        private CogGraphicLabel[] m_objLabelIndexGradientRegion;
        private CogRectangleAffine[] m_objRectangleGradientRegion;

        // 티칭용 뷰어
        private bool m_bTeach;
        // 히스토 그램 사용
        private CogHistogramTool m_histTool;
        private bool m_bUseHistogram;
        private CogGraphicLabel m_objLabelHistogram;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 디스플레이 업데이트 델리게이트 생성
        public delegate void DelegateUpdateDisplay( CInspectionResult.CResult objResult );
        public DelegateUpdateDisplay m_objDelegateUpdateDisplayVIDI;
        public DelegateUpdateDisplay m_objDelegateUpdateDisplayMeasure;
        public DelegateUpdateDisplay m_objDelegateUpdateDisplayMeasure3D;
        public DelegateUpdateDisplay m_objDelegateUpdateDisplayOriginalImage;
        public DelegateUpdateDisplay m_objDelegateUpdateDisplayPMSImage;
        public DelegateUpdateDisplay m_objDelegateUpdateDisplay3D;
        public DelegateUpdateDisplay m_objDelegateUpdateDisplay3DResult;

        // 캘리브레이션용 디스플레이 업데이트 델리게이트 생성
        public DelegateUpdateDisplay m_objDelegateUpdateDisplayCalibration;

        // 이미지 저장 델리게이트 생성
        public delegate void DelegateSaveImage( CInspectionResult.CResult objResult, bool bManualSave );
        public DelegateSaveImage m_objDelegateSaveImage;
        // 화면 확대 이벤트
        public delegate void DelegateChangeSizeDisplay( int iCameraIndex );
        public event DelegateChangeSizeDisplay EventChangeSizeDisplay;

        // 티칭용 고정크기 때문에 멤버변수로 하나 가지고 있는다, 셋팅 할때만 적용
        public double dVidiFixedSizeWidth;
        public double dVidiFixedSizeHeight;

        public double dVidiFixedSizeWidthGradient;
        public double dVidiFixedSizeHeightGradient;

        //Case - Measure 검사 결과 화면에 사용자가 원하는 위치의 높이 데이터 확인용
        CogPointMarker m_ptPosHeight = new CogPointMarker();
        double[] m_obj3DDataHeightCrop1d;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormDisplay()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Initialize( int iCameraIndex, string strTitle, bool bTeach = false, int iImageIndex = 0 )
        {
            bool bReturn = false;

            do {
                var pDocument = CDocument.GetDocument;
                var pFormCommon = CFormCommon.GetFormCommon;
                m_bTeach = bTeach;
                m_bUseHistogram = false;
                m_iDisplayIndex = 0;
                // 인덱스 정의
                m_iCameraIndex = iCameraIndex;
                m_iImageIndex = iImageIndex;

                pFormCommon.SetButtonText( this.BtnTitle, strTitle );
                m_eLanguage = CDefine.enumLanguage.LANGUAGE_FINAL;
                // 코그 그래픽 객체 생성 & 정의 ( 해당 부분은 인터렉티브로 둬서 스태틱 클리어를 해도 남도록 구성 )

                // 결과 ok, ng ( Top Right )
                {
                    m_objLabelResult = new CogGraphicLabel();
                    m_objLabelResult.Color = CogColorConstants.DarkRed;
                    m_objLabelResult.Font = new Font( m_objLabelResult.Font.Name, 15.0F, m_objLabelResult.Font.Style, m_objLabelResult.Font.Unit );
                    m_objLabelResult.Visible = false;
                    cogDisplay.InteractiveGraphics.Add( m_objLabelResult, "", false );
                }
                // 화면 라인
                {
                    m_objCogDisplayLine = new CogLine[ ( int )enumDisplayLine.DISPLAY_LINE_FINAL ];
                    for( int iLoopCount = 0; iLoopCount < m_objCogDisplayLine.Length; iLoopCount++ ) {
                        m_objCogDisplayLine[ iLoopCount ] = new CogLine();
                        m_objCogDisplayLine[ iLoopCount ].LineStyle = CogGraphicLineStyleConstants.Dot;
                        m_objCogDisplayLine[ iLoopCount ].Color = CogColorConstants.Green;
                        m_objCogDisplayLine[ iLoopCount ].Visible = false;
                        cogDisplay.InteractiveGraphics.Add( m_objCogDisplayLine[ iLoopCount ], "", false );
                    }
                }
                // 길이 측정
                {
                    // 라벨
                    m_objLabelGuage = new CogGraphicLabel();
                    m_objLabelGuage.Color = CogColorConstants.Orange;
                    m_objLabelGuage.Font = new Font( m_objLabelGuage.Font.Name, 12.0F, m_objLabelGuage.Font.Style, m_objLabelGuage.Font.Unit );
                    m_objLabelGuage.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;
                    m_objLabelGuage.Interactive = false;
                    m_objLabelGuage.Visible = false;
                    cogDisplay.InteractiveGraphics.Add( m_objLabelGuage, "", false );
                    // 사각형
                    m_objRectangleGuage = new CogRectangleAffine();
                    m_objRectangleGuage.Color = CogColorConstants.Orange;
                    m_objRectangleGuage.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                    m_objRectangleGuage.SetCenterLengthsRotationSkew( 300, 300, 300, 300, 0, 0 );
                    m_objRectangleGuage.Interactive = true;
                    m_objRectangleGuage.Visible = false;
                    cogDisplay.InteractiveGraphics.Add( m_objRectangleGuage, "", false );
                    // 사용 유무
                    m_bUseGuage = false;
                }

                // 검색영역 인덱스
                {
                    m_objLabelIndexSerchRegion = new CogGraphicLabel[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                    m_objRectangleSerchRegion = new CogRectangleAffine[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        m_objLabelIndexSerchRegion[ iLoopCount ] = new CogGraphicLabel();
                        m_objLabelIndexSerchRegion[ iLoopCount ].Color = CogColorConstants.Green;
                        m_objLabelIndexSerchRegion[ iLoopCount ].Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        m_objLabelIndexSerchRegion[ iLoopCount ].Visible = false;
                        m_objLabelIndexSerchRegion[ iLoopCount ].Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                        m_objLabelIndexSerchRegion[ iLoopCount ].Font = new Font( "맑은 고딕", ( float )9, System.Drawing.FontStyle.Bold );
                        m_objRectangleSerchRegion[ iLoopCount ] = new CogRectangleAffine();
                        m_objRectangleSerchRegion[ iLoopCount ].Color = CogColorConstants.Green;
                        m_objRectangleSerchRegion[ iLoopCount ].Visible = false;

                        if( true == m_bTeach ) {
                            m_objRectangleSerchRegion[ iLoopCount ].GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                            m_objRectangleSerchRegion[ iLoopCount ].Interactive = true;
                        }

                        cogDisplay.InteractiveGraphics.Add( m_objLabelIndexSerchRegion[ iLoopCount ], "IDReader", false );
                        cogDisplay.InteractiveGraphics.Add( m_objRectangleSerchRegion[ iLoopCount ], "IDReader", false );
                    }
                }

                // 기울기영역 인덱스
                {
                    m_objLabelIndexGradientRegion = new CogGraphicLabel[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                    m_objRectangleGradientRegion = new CogRectangleAffine[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        m_objLabelIndexGradientRegion[ iLoopCount ] = new CogGraphicLabel();
                        m_objLabelIndexGradientRegion[ iLoopCount ].Color = CogColorConstants.Red;
                        m_objLabelIndexGradientRegion[ iLoopCount ].Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        m_objLabelIndexGradientRegion[ iLoopCount ].Visible = false;
                        m_objLabelIndexGradientRegion[ iLoopCount ].Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                        m_objLabelIndexGradientRegion[ iLoopCount ].Font = new Font( "맑은 고딕", ( float )9, System.Drawing.FontStyle.Bold );
                        m_objRectangleGradientRegion[ iLoopCount ] = new CogRectangleAffine();
                        m_objRectangleGradientRegion[ iLoopCount ].Color = CogColorConstants.Red;
                        m_objRectangleGradientRegion[ iLoopCount ].Visible = false;

                        if( true == m_bTeach ) {
                            m_objRectangleGradientRegion[ iLoopCount ].GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                            m_objRectangleGradientRegion[ iLoopCount ].Interactive = true;
                        }

                        cogDisplay.InteractiveGraphics.Add( m_objLabelIndexGradientRegion[ iLoopCount ], "IDReader", false );
                        cogDisplay.InteractiveGraphics.Add( m_objRectangleGradientRegion[ iLoopCount ], "IDReader", false );
                    }
                }

                // 히스토그램 
                {
                    m_histTool = new CogHistogramTool();
                    m_histTool.Region = new CogRectangle();
                    ( ( CogRectangle )m_histTool.Region ).Color = CogColorConstants.Green;
                    ( ( CogRectangle )m_histTool.Region ).SetCenterWidthHeight( 1224, 1024, 200, 200 );
                    ( ( CogRectangle )m_histTool.Region ).Visible = false;
                    AddInteractiveGraphic( m_histTool.Region as CogRectangle, "HistgramRoi", false );

                    m_objLabelHistogram = new CogGraphicLabel();
                    m_objLabelHistogram.Color = CogColorConstants.Green;
                    m_objLabelHistogram.Font = new Font( m_objLabelGuage.Font.Name, 12.0F, m_objLabelGuage.Font.Style, m_objLabelGuage.Font.Unit );
                    m_objLabelHistogram.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                    m_objLabelHistogram.Interactive = false;
                    m_objLabelHistogram.Visible = false;
                    cogDisplay.InteractiveGraphics.Add( m_objLabelHistogram, "", false );
                }

                m_ptPosHeight.Interactive = true;
                m_ptPosHeight.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
                m_ptPosHeight.LineWidthInScreenPixels = 2;
                m_ptPosHeight.Color = CogColorConstants.Red;
                m_ptPosHeight.SelectedSpaceName = "@";
                m_ptPosHeight.Changed += new CogChangedEventHandler(CallbackPosition);

                // 검사 결과 업데이트 델리게이트
                m_objDelegateUpdateDisplayVIDI = new DelegateUpdateDisplay( UpdateDisplayVIDI );
                m_objDelegateUpdateDisplayMeasure = new DelegateUpdateDisplay( UpdateDisplayMeasure );
                m_objDelegateUpdateDisplayMeasure3D = new DelegateUpdateDisplay( UpdateDisplayMeasure3D );
                m_objDelegateUpdateDisplayOriginalImage = new DelegateUpdateDisplay( UpdateDisplayOriginalImage );
                m_objDelegateUpdateDisplayPMSImage = new DelegateUpdateDisplay( UpdateDisplayPMSImage );

                m_objDelegateUpdateDisplay3D = new DelegateUpdateDisplay( UpdateDisplay3D );
                m_objDelegateUpdateDisplay3DResult = new DelegateUpdateDisplay( UpdateDisplay3DResult );

                // 캘리브레이션용 디스플레이 업데이트 델리게이트 생성
                m_objDelegateUpdateDisplayCalibration = new DelegateUpdateDisplay( UpdateDisplayCalibration );
                // 이미지 저장 델리게이트
                m_objDelegateSaveImage = new DelegateSaveImage( SaveImage );
                // 언어변경
                SetChangeLanguage();
                // 버튼 색상 정의
                SetButtonColor();
                // 타미어 시작
                timer.Interval = 100;
                timer.Enabled = true;

                try {
                    // 디스플레이 핏
                    cogDisplay.Fit( true );
                    cogDisplay.Image = new Cognex.VisionPro.CogImage8Grey(
                        pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex ).objCameraConfig.iCameraWidth,
                        pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex ).objCameraConfig.iCameraHeight );
                } catch( Exception ex ) {
                    Trace.WriteLine( "Message : " + ex.Message + "\nStack : " + ex.StackTrace );
                    break;
                }

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
            cogDisplay.InteractiveGraphics.Dispose();
            cogDisplay.Dispose();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 이미지 추출
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CogImage8Grey GetCurrentDisplayImage()
        {
            return ( CogImage8Grey )cogDisplay.Image;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CogDisplay GetCogDisplay()
        {
            return cogDisplay;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetCogDisplay( CogDisplay objCogDisplay )
        {
            cogDisplay = objCogDisplay as CogRecordDisplay;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetTimer( bool bTimer )
        {
            timer.Enabled = bTimer;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 디스플레이 인덱스 셋
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDisplayIndex( int iDisplayIndex )
        {
            m_iDisplayIndex = iDisplayIndex;
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
                var pDocument = CDocument.GetDocument;
                if( m_eLanguage == pDocument.m_objConfig.GetSystemParameter().eLanguage ) {
                    break;
                }
                // 버튼
                SetControlChangeLanguage( this.BtnGuage );
                SetControlChangeLanguage( this.BtnHistogram );
                // 팝업 메뉴 변경
                pDocument.SetCogDisplayPopupMenuChangeLanguage( cogDisplay );

                m_eLanguage = pDocument.m_objConfig.GetSystemParameter().eLanguage;

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
        //목적 : 버튼 색상 정의
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonColor()
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            // 버튼 Fore, Back 색상 정의
            pFormCommon.SetButtonColor( this.BtnGuage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnHistogram, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 택 타임 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayMeasureHeight( CInspectionResult.CResult objResult )
        {
            do {
                if( null == objResult.objResultCommon.objMeasureLine ) break;
                for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objMeasureLine.Count; iLoopCount++ ) {
                    CogGraphicLabel objLabelDistance = new CogGraphicLabel();
                    objLabelDistance.Color = CogColorConstants.Red;
                    objLabelDistance.Font = new Font( objLabelDistance.Font.Name, 12.0F, objLabelDistance.Font.Style, objLabelDistance.Font.Unit );
                    objLabelDistance.Alignment = CogGraphicLabelAlignmentConstants.TopCenter;
                    objLabelDistance.SetXYText( objResult.objResultCommon.objMeasureLine[ iLoopCount ].dStartX, objResult.objResultCommon.objMeasureLine[ iLoopCount ].dEndY, string.Format( "{0:F2}", objResult.objResultCommon.objMeasureLine[ iLoopCount ].dDistance ) );
                    objLabelDistance.Visible = true;
                    cogDisplay.StaticGraphics.Add( objLabelDistance, "CommonGraphics" );
                    CogLineSegment objLine = new CogLineSegment();
                    objLine.Color = CogColorConstants.Red;
                    objLine.LineWidthInScreenPixels = 3;
                    objLine.SetStartEnd( objResult.objResultCommon.objMeasureLine[ iLoopCount ].dStartX, objResult.objResultCommon.objMeasureLine[ iLoopCount ].dStartY, objResult.objResultCommon.objMeasureLine[ iLoopCount ].dEndX, objResult.objResultCommon.objMeasureLine[ iLoopCount ].dEndY );
                    cogDisplay.StaticGraphics.Add( objLine, "CommonGraphics" );
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 트리거 시간 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayTriggerTime( CInspectionResult.CResult objResult )
        {
            CogGraphicLabel objLabelTriggerTime = new CogGraphicLabel();
            objLabelTriggerTime.Color = CogColorConstants.Red;
            objLabelTriggerTime.Font = new Font( objLabelTriggerTime.Font.Name, 12.0F, objLabelTriggerTime.Font.Style, objLabelTriggerTime.Font.Unit );
            objLabelTriggerTime.Visible = false;
            objLabelTriggerTime.SelectedSpaceName = "@";
            cogDisplay.InteractiveGraphics.Add( objLabelTriggerTime, "CommonGraphics", false );

            objLabelTriggerTime.SelectedSpaceName = "@";
            if( CDefine.enumCameraType.CAMERA_3D == CDocument.GetDocument.m_objConfig.GetSystemParameter().eCameraType && CDefine.enumMachineType.PROCESS_150 == CDocument.GetDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                // 그래픽 정렬
                objLabelTriggerTime.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                // x, y, string
                double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                objLabelTriggerTime.SetXYText( dWhiteSpaceX, cogDisplay.Image.Height + dWhiteSpaceY, objResult.objResultCommon.strTriggerTime );
            } else {
                // 그래픽 정렬
                objLabelTriggerTime.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;
                // x, y, string
                double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                objLabelTriggerTime.SetXYText( dWhiteSpaceX, cogDisplay.Image.Height - dWhiteSpaceY, objResult.objResultCommon.strTriggerTime );
            }
            objLabelTriggerTime.Visible = true;

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 택 타임 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayTactTime( CInspectionResult.CResult objResult )
        {
            CogGraphicLabel objLabelTactTime = new CogGraphicLabel();
            objLabelTactTime.Color = CogColorConstants.Green;
            objLabelTactTime.Font = new Font( objLabelTactTime.Font.Name, 12.0F, objLabelTactTime.Font.Style, objLabelTactTime.Font.Unit );
            objLabelTactTime.Visible = false;
            cogDisplay.InteractiveGraphics.Add( objLabelTactTime, "CommonGraphics", false );
            objLabelTactTime.SelectedSpaceName = "@";

            if( CDefine.enumCameraType.CAMERA_3D == CDocument.GetDocument.m_objConfig.GetSystemParameter().eCameraType && CDefine.enumMachineType.PROCESS_150 == CDocument.GetDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                // 그래픽 정렬
                objLabelTactTime.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
                // x, y, string
                double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                objLabelTactTime.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, cogDisplay.Image.Height + dWhiteSpaceY, objResult.objResultCommon.strTactTime + "ms" );
            } else {
                // 그래픽 정렬
                objLabelTactTime.Alignment = CogGraphicLabelAlignmentConstants.BottomRight;
                // x, y, string
                double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                objLabelTactTime.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, cogDisplay.Image.Height - dWhiteSpaceY, objResult.objResultCommon.strTactTime + "ms" );
            }

            objLabelTactTime.Visible = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 택 타임 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayTactTimeVIDI( CInspectionResult.CResult objResult )
        {
            CogGraphicLabel objLabelTactTime = new CogGraphicLabel();
            objLabelTactTime.Color = CogColorConstants.Green;
            objLabelTactTime.Font = new Font( objLabelTactTime.Font.Name, 12.0F, objLabelTactTime.Font.Style, objLabelTactTime.Font.Unit );
            objLabelTactTime.Visible = false;
            cogDisplay.InteractiveGraphics.Add( objLabelTactTime, "CommonGraphics", false );
            // 그래픽 정렬
            objLabelTactTime.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
            // x, y, string
            double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
            double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
            objLabelTactTime.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, cogDisplay.Image.Height - dWhiteSpaceY, objResult.objResultCommon.objVidiTactTime[ m_iImageIndex ] + "ms" );
            objLabelTactTime.SelectedSpaceName = "@";
            objLabelTactTime.Visible = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 택 타임 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayTactTime( string strTactTime )
        {
            CogGraphicLabel objLabelTactTime = new CogGraphicLabel();
            objLabelTactTime.Color = CogColorConstants.Green;
            objLabelTactTime.Font = new Font( objLabelTactTime.Font.Name, 12.0F, objLabelTactTime.Font.Style, objLabelTactTime.Font.Unit );
            objLabelTactTime.Visible = false;
            cogDisplay.InteractiveGraphics.Add( objLabelTactTime, "CommonGraphics", false );
            // 그래픽 정렬
            objLabelTactTime.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
            // x, y, string
            double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
            double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
            objLabelTactTime.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, cogDisplay.Image.Height - dWhiteSpaceY, strTactTime + "ms" );
            objLabelTactTime.SelectedSpaceName = "@";
            objLabelTactTime.Visible = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 결과 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayResult( CInspectionResult.CResult objResult, bool bVisible = true )
        {
            string strInspectionResult = "";
            if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.eResult ) {
                strInspectionResult = "OK";
                m_objLabelResult.Color = CogColorConstants.Green;
            } else if( CDefine.enumResult.RESULT_NG == objResult.objResultCommon.eResult ) {
                if( true == CDocument.GetDocument.m_objConfig.GetSystemParameter().bPassMode ) {
                    strInspectionResult = "OK";
                    m_objLabelResult.Color = CogColorConstants.Green;
                } else {
                    strInspectionResult = "NG";
                    m_objLabelResult.Color = CogColorConstants.DarkRed;
                }
            }
            // 결과 그래픽 정렬
            if( CDefine.enumMachineType.PROCESS_150 == CDocument.GetDocument.m_objConfig.GetSystemParameter().eMachineType )
                m_objLabelResult.Alignment = CogGraphicLabelAlignmentConstants.BottomRight;
            else
                m_objLabelResult.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
            // x, y, string
            // 간격 계산을 이미지에 비율로 계산해서 항상 일정하게 표시되도록 변경
            double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
            double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
            m_objLabelResult.SelectedSpaceName = "@";
            m_objLabelResult.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, dWhiteSpaceY, strInspectionResult );
            m_objLabelResult.Visible = bVisible;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 결과 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayResult( bool bResult )
        {
            string strInspectionResult = "";
            if( true == bResult ) {
                strInspectionResult = "OK";
                m_objLabelResult.Color = CogColorConstants.Green;
            } else {
                strInspectionResult = "NG";
                m_objLabelResult.Color = CogColorConstants.DarkRed;
            }
            m_objLabelResult.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
            // x, y, string
            // 간격 계산을 이미지에 비율로 계산해서 항상 일정하게 표시되도록 변경
            double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
            double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
            m_objLabelResult.SelectedSpaceName = "@";
            m_objLabelResult.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, dWhiteSpaceY, strInspectionResult );
            m_objLabelResult.Visible = true;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetSearchRegion( CConfig.CSerchRegion[] objVidiSerchRegion )
        {
            do {
                var pDocument = CDocument.GetDocument;
                if( null == objVidiSerchRegion ) break;
                // 
                // 수만큼 표시
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    if( iLoopCount < objVidiSerchRegion.Length ) {
                        double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                        dStartX = objVidiSerchRegion[ iLoopCount ].dStartX;
                        dStartY = objVidiSerchRegion[ iLoopCount ].dStartY;
                        dVidiFixedSizeWidth = dEndX = objVidiSerchRegion[ iLoopCount ].dEndX;
                        dVidiFixedSizeHeight = dEndY = objVidiSerchRegion[ iLoopCount ].dEndY;
                        dRotation = objVidiSerchRegion[ iLoopCount ].dRotation;
                        dSkew = objVidiSerchRegion[ iLoopCount ].dSkew;

                        m_objLabelIndexSerchRegion[ iLoopCount ].Visible = true;
                        m_objLabelIndexSerchRegion[ iLoopCount ].SetXYText( dStartX, dStartY, "INDEX : " + ( iLoopCount + 1 ).ToString() );
                        m_objRectangleSerchRegion[ iLoopCount ].Visible = true;
                        m_objRectangleSerchRegion[ iLoopCount ].SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                        m_objRectangleSerchRegion[ iLoopCount ].Color = CogColorConstants.Green;
                    } else {
                        m_objLabelIndexSerchRegion[ iLoopCount ].Visible = false;
                        m_objRectangleSerchRegion[ iLoopCount ].Visible = false;
                    }
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGradientRegion( CConfig.CSerchRegion[] objVidiSerchRegion )
        {
            do {
                var pDocument = CDocument.GetDocument;
                if( null == objVidiSerchRegion ) break;
                // 
                // 수만큼 표시
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    if( iLoopCount < objVidiSerchRegion.Length ) {
                        double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                        dStartX = objVidiSerchRegion[ iLoopCount ].dStartX;
                        dStartY = objVidiSerchRegion[ iLoopCount ].dStartY;
                        dVidiFixedSizeWidthGradient = dEndX = objVidiSerchRegion[ iLoopCount ].dEndX;
                        dVidiFixedSizeHeightGradient = dEndY = objVidiSerchRegion[ iLoopCount ].dEndY;
                        dRotation = objVidiSerchRegion[ iLoopCount ].dRotation;
                        dSkew = objVidiSerchRegion[ iLoopCount ].dSkew;

                        m_objLabelIndexGradientRegion[ iLoopCount ].Visible = true;
                        m_objLabelIndexGradientRegion[ iLoopCount ].SetXYText( dStartX, dStartY, "GRADIENT : " + ( iLoopCount + 1 ).ToString() );
                        m_objRectangleGradientRegion[ iLoopCount ].Visible = true;
                        m_objRectangleGradientRegion[ iLoopCount ].SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                        m_objRectangleGradientRegion[ iLoopCount ].Color = CogColorConstants.Red;
                    } else {
                        m_objLabelIndexGradientRegion[ iLoopCount ].Visible = false;
                        m_objRectangleGradientRegion[ iLoopCount ].Visible = false;
                    }
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetSearchRegion( CConfig.CSerchRegion[] objVidiSearchRegion, CInspectionResult.CResult objResult )
        {
            do {
                var pDocument = CDocument.GetDocument;
                if( null == objVidiSearchRegion ) break;
                // 
                // 수만큼 표시
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    if( iLoopCount < objVidiSearchRegion.Length ) {
                        double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                        dStartX = objVidiSearchRegion[ iLoopCount ].dStartX;
                        dStartY = objVidiSearchRegion[ iLoopCount ].dStartY;
                        dVidiFixedSizeWidth = dEndX = objVidiSearchRegion[ iLoopCount ].dEndX;
                        dVidiFixedSizeHeight = dEndY = objVidiSearchRegion[ iLoopCount ].dEndY;
                        dRotation = objVidiSearchRegion[ iLoopCount ].dRotation;
                        dSkew = objVidiSearchRegion[ iLoopCount ].dSkew;

                        m_objLabelIndexSerchRegion[ iLoopCount ].Visible = true;
                        m_objLabelIndexSerchRegion[ iLoopCount ].SetXYText( dStartX, dStartY, "INDEX : " + ( iLoopCount + 1 ).ToString() );
                        m_objRectangleSerchRegion[ iLoopCount ].Visible = true;
                        m_objRectangleSerchRegion[ iLoopCount ].SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );

                        if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objVidiResult[ iLoopCount ] )
                            m_objRectangleSerchRegion[ iLoopCount ].Color = CogColorConstants.Green;
                        else
                            m_objRectangleSerchRegion[ iLoopCount ].Color = CogColorConstants.Red;
                    } else {
                        m_objLabelIndexSerchRegion[ iLoopCount ].Visible = false;
                        m_objRectangleSerchRegion[ iLoopCount ].Visible = false;
                    }
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetSearchRegionMeasure( CConfig.CSerchRegion[] objMeasureSerchRegion, CInspectionResult.CResult objResult = null )
        {
            do {
                var pDocument = CDocument.GetDocument;
                if( null == objMeasureSerchRegion ) break;
                // 
                // 수만큼 표시
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    if( iLoopCount < objMeasureSerchRegion.Length ) {
                        double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                        dStartX = objMeasureSerchRegion[ iLoopCount ].dStartX;
                        dStartY = objMeasureSerchRegion[ iLoopCount ].dStartY;
                        dVidiFixedSizeWidth = dEndX = objMeasureSerchRegion[ iLoopCount ].dEndX;
                        dVidiFixedSizeHeight = dEndY = objMeasureSerchRegion[ iLoopCount ].dEndY;
                        dRotation = objMeasureSerchRegion[ iLoopCount ].dRotation;
                        dSkew = objMeasureSerchRegion[ iLoopCount ].dSkew;

                        CogGraphicLabel objLabelIndexSerchRegion;
                        CogRectangleAffine objRectangleSerchRegion;

                        objLabelIndexSerchRegion = new CogGraphicLabel();
                        objLabelIndexSerchRegion.Color = CogColorConstants.Green;
                        objLabelIndexSerchRegion.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        objLabelIndexSerchRegion.Font = new Font( "맑은 고딕", ( float )9, System.Drawing.FontStyle.Bold );
                        objRectangleSerchRegion = new CogRectangleAffine();
                        if( null == objResult )
                            objRectangleSerchRegion.Color = CogColorConstants.Green;
                        else {
                            if( CDefine.enumResult.RESULT_NG == objResult.objResultCommon.objMeasureResult[ iLoopCount ] )
                                objRectangleSerchRegion.Color = CogColorConstants.Red;
                            else
                                objRectangleSerchRegion.Color = CogColorConstants.Green;
                        }

                        objLabelIndexSerchRegion.SetXYText( dStartX, dStartY - 60, ( iLoopCount + 1 ).ToString() );
                        objRectangleSerchRegion.SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );


                        objLabelIndexSerchRegion.Visible = true;
                        objRectangleSerchRegion.Visible = true;
                        cogDisplay.InteractiveGraphics.Add( objRectangleSerchRegion, "SUB_PMALIGN_INTERACTIVE", false );
                        cogDisplay.InteractiveGraphics.Add( objLabelIndexSerchRegion, "SUB_PMALIGN_INTERACTIVE", false );

                    } else {
                    }
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetHideSearchRegion()
        {
            do {
                var pDocument = CDocument.GetDocument;
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    m_objLabelIndexSerchRegion[ iLoopCount ].Visible = false;
                    m_objRectangleSerchRegion[ iLoopCount ].Visible = false;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetHideGradientRegion()
        {
            do {
                var pDocument = CDocument.GetDocument;
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    m_objRectangleGradientRegion[ iLoopCount ].Visible = false;
                    m_objLabelIndexGradientRegion[ iLoopCount ].Visible = false;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGraphicsPatternResult( Cognex.VisionPro.ICogRecord objCogRecord )
        {
            do {
                do {
                    if( null == objCogRecord ) break;
                    if( true == objCogRecord.SubRecords.ContainsKey( "InputImage" ) ) {
                        CogGraphicCollection obj = objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "CompositeResultGraphics" ].Content as CogGraphicCollection;

                        for( int iLoopGraphicCollection = 0; iLoopGraphicCollection < obj.Count; iLoopGraphicCollection++ ) {
                            obj[ iLoopGraphicCollection ].SelectedSpaceName = "@";
                            cogDisplay.InteractiveGraphics.Add( ( ICogGraphicInteractive )obj[ iLoopGraphicCollection ], "SUB_PMALIGN_INTERACTIVE", false );
                        }
                    }
                } while( false );
            } while( false );
        }
        

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 화면 중심 라인 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayCenterLine( int iWidth, int iHeight )
        {

            CogLine[] objCogDisplayLine;
            objCogDisplayLine = new CogLine[ ( int )enumDisplayLine.DISPLAY_LINE_FINAL ];

            for( int iLoopCount = 0; iLoopCount < m_objCogDisplayLine.Length; iLoopCount++ ) {
                objCogDisplayLine[ iLoopCount ] = new CogLine();
                objCogDisplayLine[ iLoopCount ].LineStyle = CogGraphicLineStyleConstants.Dot;
                objCogDisplayLine[ iLoopCount ].Color = CogColorConstants.Green;
                objCogDisplayLine[ iLoopCount ].Visible = false;
                cogDisplay.InteractiveGraphics.Add( objCogDisplayLine[ iLoopCount ], "CommonGraphics", false );

            }

            // 수평
            objCogDisplayLine[ ( int )enumDisplayLine.HORIZON_LINE ].SetFromStartXYEndXY( 0.0, ( double )iHeight / 2.0, ( double )iWidth, ( double )iHeight / 2.0 );
            objCogDisplayLine[ ( int )enumDisplayLine.HORIZON_LINE ].Visible = true;
            // 수직
            objCogDisplayLine[ ( int )enumDisplayLine.VERTICAL_LINE ].SetFromStartXYEndXY( ( double )iWidth / 2.0, 0.0, ( double )iWidth / 2.0, ( double )iHeight );
            objCogDisplayLine[ ( int )enumDisplayLine.VERTICAL_LINE ].Visible = true;

            objCogDisplayLine[ ( int )enumDisplayLine.HORIZON_LINE ].SelectedSpaceName = "@";
            objCogDisplayLine[ ( int )enumDisplayLine.VERTICAL_LINE ].SelectedSpaceName = "@";
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayOriginalImage( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            do {
                if( null == objResult.objResultCommon.objInputGrabOriginImage ) break;

                if( objResult.objResultCommon.iIndexDisplayOriginalImage >= objResult.objResultCommon.objInputGrabOriginImage.Count ) break;

                try {
                    // 나중에 함수로 빼자
                    if( -1 != cogDisplay.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                        cogDisplay.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
                    }

                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();
                    cogDisplay.StaticGraphics.Clear();
                    SetHideSearchRegion();
                    cogDisplay.Image = objResult.objResultCommon.objInputGrabOriginImage[ objResult.objResultCommon.iIndexDisplayOriginalImage ];

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    // 공통으로 표시하는 그래픽
                    {
                        // 화면 중심 라인
                        SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }
                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayPMSImage( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            do {
                CConfig.CRecipeParameter objParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );
                int iImageType = ( int )objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].ePMSImageTypeVIDI;
                if( null == objResult.objResultCommon.objInputGrabOriginImage ) break;
                if( null == objResult.objResultCommon.objPMSImage[ iImageType ] ) break;



                try {
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();

                    SetHideSearchRegion();
                    // 나중에 함수로 빼자
                    if( -1 != cogDisplay.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                        cogDisplay.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
                    }



                    if( null != objResult.objResultCommon.objOutputGrabImage )
                        cogDisplay.Image = objResult.objResultCommon.objOutputGrabImage;
                    else
                        cogDisplay.Image = objResult.objResultCommon.objPMSImage[ iImageType ];

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    // 공통으로 표시하는 그래픽
                    {
                        // 화면 중심 라인
                        SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }

                    CConfig.CSerchRegion[] objVidiSearchRegion = new CConfig.CSerchRegion[ objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].iCountSerchRegion ];
                    CConfig.CSerchRegion[] objMeasureSearchRegion = new CConfig.CSerchRegion[ objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].iCountSerchRegion ];
                    for( int iLoopCount = 0; iLoopCount < objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].iCountSerchRegion; iLoopCount++ ) {
                        objVidiSearchRegion[ iLoopCount ] = new CConfig.CSerchRegion();
                        objVidiSearchRegion[ iLoopCount ] = objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].objVidiSerchRegion[ iLoopCount ].Clone() as CConfig.CSerchRegion;

                        objMeasureSearchRegion[ iLoopCount ] = new CConfig.CSerchRegion();
                        objMeasureSearchRegion[ iLoopCount ] = objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].objMeasureSerchRegion[ iLoopCount ].Clone() as CConfig.CSerchRegion;
                    }
                    SetDisplayTriggerTime( objResult );
                    SetDisplayTactTime( objResult );


                    SetDisplayResult( objResult, true );
                    if( null != objResult.objGraphicsPMAlign[ 0 ] )
                        SetSearchRegion( objVidiSearchRegion, objResult );
                    // SetSearchRegionMeasure( objMeasureSearchRegion );
                    SetGraphicsPatternResult( objResult.objGraphicsPMAlign[ 0 ] );

                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayVIDI( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            // 기존 디스플레이 클리어
            cogDisplay.StaticGraphics.Clear();
            CommonGraphicsClear();

            do {
                try {
                    if( null == objResult.objResultCommon.objVidiResultImage ||
                        m_iImageIndex >= objResult.objResultCommon.objVidiResultImage.Count ||
                        null == objResult.objResultCommon.objVidiResultImage[ m_iImageIndex ] ) {
                        cogDisplay.Image = null;
                        break;
                    }

                    cogDisplay.Image = objResult.objResultCommon.objVidiResultImage[ m_iImageIndex ];


                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    // 이부분은 하이브제공소스에서 그냥 가져옴
                    CogMaskGraphic objCogMask = new CogMaskGraphic();
                    objCogMask.Image = objResult.objResultCommon.objVidiResultOverlayGraphic[ m_iImageIndex ];
                    objCogMask.SelectedColor = CogColorConstants.Red;

                    AddStaticGraphic( objCogMask, "STATIC_VIDI_RESULT" );

                    CogGraphicLabel objCogLabel = new CogGraphicLabel();
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                    objCogLabel.Color = CogColorConstants.Black;
                    objCogLabel.Font = new Font( objCogLabel.Font.Name, 10.0F, objCogLabel.Font.Style, objCogLabel.Font.Unit );
                    objCogLabel.SetXYText( 0, 0, "Score : " + objResult.objResultCommon.objVidiScore[ m_iImageIndex ].ToString( "0.00" ) );
                    AddStaticGraphic( objCogLabel, "STATIC_VIDI_RESULT" );

                    string strResult = "";
                    if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objVidiResult[ m_iImageIndex ] || true == pDocument.m_objConfig.GetSystemParameter().bPassMode ) {
                        objCogLabel.Color = CogColorConstants.Green;
                        strResult = "OK";
                    } else {
                        objCogLabel.Color = CogColorConstants.Red;
                        strResult = "NG";
                    }

                    objCogLabel.Font = new Font( objCogLabel.Font.Name, 10.0F, objCogLabel.Font.Style, objCogLabel.Font.Unit );
                    double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                    double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                    objCogLabel.SelectedSpaceName = "@";
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.BottomRight;
                    objCogLabel.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, dWhiteSpaceY, strResult );
                    AddStaticGraphic( objCogLabel, "STATIC_VIDI_RESULT" );


                    // 공통으로 표시하는 그래픽
                    {
                        // 검사 트리거 타임 화면에 표시
                        //SetDisplayTriggerTime( objResult );
                        // 검사 택 타임 화면에 표시
                        SetDisplayTactTimeVIDI( objResult );
                        // 검사 결과 화면에 표시
                        // SetDisplayResult( objResult, false );
                        // 화면 중심 라인
                        //   SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }

                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayMeasure( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            // 기존 디스플레이 클리어
            cogDisplay.StaticGraphics.Clear();
            CommonGraphicsClear();

            do {
                try {
                    if( null == objResult.objResultCommon.objCropImageMeasure ||
                        m_iImageIndex >= objResult.objResultCommon.objCropImageMeasure.Count ||
                        null == objResult.objResultCommon.objCropImageMeasure[ m_iImageIndex ] ) {
                        cogDisplay.Image = null;
                        break;
                    }

                    // objResult.objResultCommon.objCropImageMeasure[ m_iImageIndex ].SelectedSpaceName = "#";
                    cogDisplay.Image = new CogImage8Grey( objResult.objResultCommon.objCropImageBitmapMeasure[ m_iImageIndex ] );
                    cogDisplay.Image.SelectedSpaceName = "@";

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );


                    CogGraphicLabel objCogLabel = new CogGraphicLabel();
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                    objCogLabel.Color = CogColorConstants.Black;
                    objCogLabel.Font = new Font( objCogLabel.Font.Name, 10.0F, objCogLabel.Font.Style, objCogLabel.Font.Unit );
                    string strResult = "";
                    if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult[ m_iImageIndex ] || true == pDocument.m_objConfig.GetSystemParameter().bPassMode ) {
                        objCogLabel.Color = CogColorConstants.Green;
                        strResult = "OK";
                    } else {
                        objCogLabel.Color = CogColorConstants.Red;
                        strResult = "NG";
                    }
                    objCogLabel.Font = new Font( objCogLabel.Font.Name, 10.0F, objCogLabel.Font.Style, objCogLabel.Font.Unit );
                    double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                    double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                    objCogLabel.SelectedSpaceName = "@";
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
                    objCogLabel.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, dWhiteSpaceY, strResult );
                    AddStaticGraphic( objCogLabel, "STATIC_VIDI_RESULT" );


                    // 공통으로 표시하는 그래픽
                    {
                        // 검사 트리거 타임 화면에 표시
                        //SetDisplayTriggerTime( objResult );
                        // 검사 택 타임 화면에 표시
                        //    SetDisplayTactTimeVIDI( objResult );
                        // 검사 결과 화면에 표시
                        SetDisplayResult( objResult, false );
                        // 화면 중심 라인
                        SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }

                    CogRectangle objRectangle = new CogRectangle();
                    objRectangle.SelectedSpaceName = "@";
                    objRectangle.Color = CogColorConstants.Green;
                    objRectangle.SetXYWidthHeight( objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dStartX, objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dStartY, objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dEndX, objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dEndY );
                    AddStaticGraphic( objRectangle, "MeasureResult" );

                    // 라벨
                    CogGraphicLabel objLabel = new CogGraphicLabel();
                    objLabel.Color = CogColorConstants.Orange;
                    objLabel.Font = new Font( m_objLabelGuage.Font.Name, 12.0F, m_objLabelGuage.Font.Style, m_objLabelGuage.Font.Unit );
                    objLabel.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;
                    objLabel.Interactive = false;
                    objLabel.Visible = true;
                    double dResolution = CDocument.GetDocument.m_objConfig.GetCameraParameter( m_iCameraIndex ).dResolution;
                    string strData = string.Format( "W : {0:F2}  H : {1:F2}", objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dEndX * dResolution, objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dEndY * dResolution );
                    objLabel.SetXYText( objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dStartX, objResult.objResultCommon.objMeasureRegion[ m_iImageIndex ].dStartY, strData );
                    AddStaticGraphic( objLabel, "MeasureResult" );

                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayMeasure3D( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            // 기존 디스플레이 클리어
            cogDisplay.StaticGraphics.Clear();
            CommonGraphicsClear();

            do {
                try {
                    if( null == objResult.objResultCommon.objCropImageMeasureSecond ||
                        m_iImageIndex >= objResult.objResultCommon.objCropImageMeasureSecond.Count ||
                        null == objResult.objResultCommon.objCropImageMeasureSecond[ m_iImageIndex ] ) {
                        cogDisplay.Image = null;
                        break;
                    }

                    // objResult.objResultCommon.objCropImageMeasure[ m_iImageIndex ].SelectedSpaceName = "#";
                    cogDisplay.Image = new CogImage8Grey( objResult.objResultCommon.objCropImageBitmapMeasureSecond[ m_iImageIndex ] );


                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    CogMaskGraphic objCogMask = new CogMaskGraphic();
                    objCogMask.Image = objResult.objResultCommon.obj3DResultOverlayGraphic[ m_iImageIndex ];
                    objCogMask.SelectedColor = CogColorConstants.Red;
                    AddStaticGraphic( objCogMask, "STATIC_VIDI_RESULT" );

                    CogGraphicLabel objCogLabel = new CogGraphicLabel();
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;

                    string strResult = "";
                    if( ( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult[ m_iImageIndex ] && CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult3DWidth[ m_iImageIndex ] ) || true == pDocument.m_objConfig.GetSystemParameter().bPassMode ) {
                        objCogLabel.Color = CogColorConstants.Green;
                        strResult = "OK";
                    } else {
                        objCogLabel.Color = CogColorConstants.Red;
                        strResult = "NG";

                    }

                    objCogLabel.Font = new Font( objCogLabel.Font.Name, 10.0F, objCogLabel.Font.Style, objCogLabel.Font.Unit );
                    double dWhiteSpaceX = cogDisplay.Image.Width * 0.04;
                    double dWhiteSpaceY = cogDisplay.Image.Height * 0.04;
                    objCogLabel.SelectedSpaceName = "@";
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.BottomRight;
                    objCogLabel.SetXYText( cogDisplay.Image.Width - dWhiteSpaceX, -dWhiteSpaceY, strResult );
                    AddStaticGraphic( objCogLabel, "STATIC_VIDI_RESULT" );

                    CogGraphicLabel objCogMinMaxAvg = new CogGraphicLabel();
                    objCogMinMaxAvg.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                    string strMinMaxAvg = string.Format("Min:{0:2F}   Max:{1:2F}   Average:{2:2F}", objResult.objResultCommon.obj3DDataHeightCrop1d.Min()
                                                                                                    , objResult.objResultCommon.obj3DDataHeightCrop1d.Max()
                                                                                                    , objResult.objResultCommon.obj3DDataHeightCrop1d.Average());
                    objCogMinMaxAvg.Color = CogColorConstants.Yellow;
                    objCogMinMaxAvg.SelectedSpaceName = "@";
                    objCogMinMaxAvg.SetXYText(0, 0, strMinMaxAvg);
                    AddStaticGraphic(objCogMinMaxAvg, "STATIC_HEIGHT_RESULT" );

                    //사용자 지정 위치 데이터 출력 용 마지막 데이터 킵
                    m_obj3DDataHeightCrop1d = objResult.objResultCommon.obj3DDataHeightCrop1d;
                    cogDisplay.InteractiveGraphics.Remove("User Position");
                    m_ptPosHeight.X = cogDisplay.Image.Width + 1;
                    m_ptPosHeight.Y = cogDisplay.Image.Height + 1;
                    cogDisplay.InteractiveGraphics.Add(m_ptPosHeight, "User Position", false);

                    // 샘플 높이데이터 표시
                    {
                        int iWidth = cogDisplay.Image.Width;
                        int iHeight = cogDisplay.Image.Height;

                        int iSample = 0;
                        for( int iLoopWidth = 0; iLoopWidth < iWidth; iLoopWidth++ ) {
                            if( 0 != iLoopWidth && 0 == iLoopWidth % 50 ) {
                                CogPointMarker objHeightPoint = new CogPointMarker();
                                objHeightPoint.Color = CogColorConstants.Green;
                                objHeightPoint.SizeInScreenPixels = 2;
                                objHeightPoint.LineWidthInScreenPixels = 3;
                                objHeightPoint.Visible = true;
                                objHeightPoint.Interactive = true;
                                objHeightPoint.GraphicDOFEnable = CogPointMarkerDOFConstants.None;
                                objHeightPoint.SelectedColor = CogColorConstants.Green;
                                objHeightPoint.X = iLoopWidth;
                                objHeightPoint.Y = iHeight / 2;
                                if( iSample < objResult.objResultCommon.obj3DListSampleHeightData[ m_iImageIndex ].Count )
                                    objHeightPoint.TipText = string.Format( "{0:F3}", objResult.objResultCommon.obj3DListSampleHeightData[ m_iImageIndex ][ iSample ] );
                                cogDisplay.InteractiveGraphics.Add( objHeightPoint, "CommonGraphics", false );
                                iSample++;
                            }
                        }
                    }

                    // 공통으로 표시하는 그래픽
                    {
                        // 검사 택 타임 화면에 표시
                        SetDisplayTactTime( objResult.objResultCommon.objMeasureTactTime[ m_iImageIndex ] );
                        // 화면 중심 라인
                        //SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }

                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplay3D( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            CConfig.CRecipeParameter objParameter = pDocument.m_objConfig.GetRecipeParameter( m_iCameraIndex );
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            do {

                try {
                    // 나중에 함수로 빼자
                    if( -1 != cogDisplay.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                        cogDisplay.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
                    }

                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();
                    cogDisplay.StaticGraphics.Clear();
                    SetHideSearchRegion();

                    if( null != objResult.objResultCommon.objOutputGrabImage )
                        cogDisplay.Image = objResult.objResultCommon.objOutputGrabImage;
                    else
                        cogDisplay.Image = objResult.objResultCommon.obj3DImageIntensity;

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    // 공통으로 표시하는 그래픽
                    {
                        // 화면 중심 라인
                        SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }

                    CConfig.CSerchRegion[] objVidiSerchRegion = new CConfig.CSerchRegion[ objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].iCountSerchRegion ];
                    CConfig.CSerchRegion[] objMeasureSerchRegion = new CConfig.CSerchRegion[ objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].iCountSerchRegion ];
                    for( int iLoopCount = 0; iLoopCount < objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].iCountSerchRegion; iLoopCount++ ) {
                        objVidiSerchRegion[ iLoopCount ] = new CConfig.CSerchRegion();
                        objVidiSerchRegion[ iLoopCount ] = objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].objVidiSerchRegion[ iLoopCount ].Clone() as CConfig.CSerchRegion;

                        objMeasureSerchRegion[ iLoopCount ] = new CConfig.CSerchRegion();
                        objMeasureSerchRegion[ iLoopCount ] = objParameter.objInspectionParameter[ pDocument.GetInspectionIndex() ].objMeasureSerchRegion[ iLoopCount ].Clone() as CConfig.CSerchRegion;
                    }
                    SetDisplayTriggerTime( objResult );
                    SetDisplayTactTime( objResult );


                    SetDisplayResult( objResult, true );
                    if( null != objResult.objGraphicsPMAlign[ 0 ] )
                        SetSearchRegionMeasure( objMeasureSerchRegion, objResult );
                        //SetSearchRegion( objVidiSerchRegion, objResult );
                    SetGraphicsPatternResult( objResult.objGraphicsPMAlign[ 0 ] );
                    SetDisplayMeasureHeight( objResult );

                    cogDisplay.AutoFitWithGraphics = true;
                    cogDisplay.Fit( true );
                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplay3DResult( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
            SetInspectionAlignGraphicClear();

            do {
                if( null == objResult.objResultCommon.obj3DImageHeight ) break;

                try {
                    // 나중에 함수로 빼자
                    if( -1 != cogDisplay.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                        cogDisplay.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
                    }

                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();
                    cogDisplay.StaticGraphics.Clear();
                    SetHideSearchRegion();


                    cogDisplay.Image = objResult.objResultCommon.obj3DImageHeight;

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    // 공통으로 표시하는 그래픽
                    {
                        // 화면 중심 라인
                        SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }
                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 트리거 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayCalibration( CInspectionResult.CResult objResult )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;

            // 매뉴얼 상태에서 변경될 시, 길이 측정과 매뉴얼 얼라인 비활성화 전환
            {
                m_bUseGuage = false;
                m_objLabelGuage.Visible = false;
                m_objRectangleGuage.Visible = false;
                pFormCommon.SetButtonColor( this.BtnGuage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnHistogram, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }

            do {
                if( null == objResult.objResultCommon.objOutputGrabImage ) break;

                try {
                    //// 기존 디스플레이 클리어
                    //cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();

                    cogDisplay.Image = objResult.objResultCommon.objOutputGrabImage;

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );

                    try {
                        //    cogDisplay.Record = objResult.objToolBlockRecord[ objResult.objToolBlockRecord.Count - 1 ];
                    } catch( Exception ex ) {
                        Trace.WriteLine( ex.StackTrace );
                    }

                    // 공통으로 표시하는 그래픽
                    {
                        // 검사 트리거 타임 화면에 표시
                        SetDisplayTriggerTime( objResult );
                        // 검사 택 타임 화면에 표시
                        SetDisplayTactTime( objResult );
                        // 검사 결과 화면에 표시
                        SetDisplayResult( objResult, false );
                        // 화면 중심 라인
                        SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    }

                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신 ( 라이브 용 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplay( CogImage8Grey objGrabImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;

            do {
                if( null == objGrabImage ) break;

                try {
                    // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
                    SetInspectionAlignGraphicClear();
                    SetHideSearchRegion();
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();

                    // 화면 중심 라인
                    SetDisplayCenterLine( cogDisplay.Image.Width, cogDisplay.Image.Height );
                    // 나중에 함수로 빼자
                    if( -1 != cogDisplay.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                        cogDisplay.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
                    }
                    // 그랩 이미지 표시
                    cogDisplay.Image = objGrabImage;
                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 표시 ( 경로 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplay( object obj )
        {
            CogImageFileTool objImageFileTool = new CogImageFileTool();

            do {
                string strImagePath = obj.ToString();
                if( "" == strImagePath ) break;

                try {
                    if( false == File.Exists( strImagePath ) ) {
                        cogDisplay.Image = null;
                        break;
                    }
                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;
                    // 그래픽 클리어
                    cogDisplay.StaticGraphics.Clear();
                    cogDisplay.InteractiveGraphics.Clear();
                    cogDisplay.Image = objImageFileTool.OutputImage;

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }

            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 얼라인 검사 그래픽 ( 인터렉티브 ) 제거
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetInspectionAlignGraphicClear()
        {
            // 비동기 방식이라 연속해서 들어오면 문제 여지 있음
            lock( m_objDisplayLock ) {
                // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
                if( -1 != cogDisplay.InteractiveGraphics.FindItem( "InspectionAlign", CogDisplayZOrderConstants.Front ) ) {
                    cogDisplay.InteractiveGraphics.Remove( "InspectionAlign" );
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 공통그래픽제거
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CommonGraphicsClear()
        {
            // 비동기 방식이라 연속해서 들어오면 문제 여지 있음
            lock( m_objDisplayLock ) {
                // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
                if( -1 != cogDisplay.InteractiveGraphics.FindItem( "CommonGraphics", CogDisplayZOrderConstants.Front ) ) {
                    cogDisplay.InteractiveGraphics.Remove( "CommonGraphics" );
                }

                // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
                if( -1 != cogDisplay.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                    cogDisplay.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 측정그래픽삭제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void InspectGraphicsClear()
        {
            // 비동기 방식이라 연속해서 들어오면 문제 여지 있음
            lock( m_objDisplayLock ) {
                // InspectionAlign 인터렉티브 그래픽 확인해서 있으면 날림
                if( -1 != cogDisplay.InteractiveGraphics.FindItem( "InspectGraphics", CogDisplayZOrderConstants.Front ) ) {
                    cogDisplay.InteractiveGraphics.Remove( "InspectGraphics" );
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 레포트 이미지 디스플레이 용
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplay( CDefine.structureReportImage objReportImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;

            string[] strPosition1 = new string[ 2 ];
            //string strPosition, strStatus;
            cogDisplay.StaticGraphics.Clear();

            BtnHistogram.BackColor = pFormCommon.COLOR_UNACTIVATE;

            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            do {
                try {
                    objImageFileTool.Operator.Open( objReportImage.strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;
                    cogDisplay.Image = objImageFileTool.OutputImage;

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }

                m_objCogDisplayLine[ ( int )enumDisplayLine.HORIZON_LINE ].SetFromStartXYEndXY( 0, cogDisplay.Image.Height / 2, cogDisplay.Image.Width, cogDisplay.Image.Height / 2 );
                m_objCogDisplayLine[ ( int )enumDisplayLine.VERTICAL_LINE ].SetFromStartXYEndXY( cogDisplay.Image.Width / 2, 0, cogDisplay.Image.Width / 2, cogDisplay.Image.Height );

               

            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplay( string strImagePath )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;

            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            do {
                try {
                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;
                    cogDisplay.Image = objImageFileTool.OutputImage;

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }
            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayHistoryPMS( CDefine.structureReportImage objReportImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            string strImagePath = objReportImage.strImagePath + "\\PMS_1.bmp";
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            do {
                try {

                    SetInspectionAlignGraphicClear();
                    SetHideSearchRegion();
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();
                    m_objLabelResult.Visible = false;

                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;
                    cogDisplay.Image = objImageFileTool.OutputImage;

                    SetDisplayResult( objReportImage.bResult );

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }
            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayHistoryVIDI( CDefine.structureReportImage objReportImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            string strImagePath = objReportImage.strImagePath;
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            do {
                try {

                    SetInspectionAlignGraphicClear();
                    SetHideSearchRegion();
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();

                    // 폴더 내 파일 목록 불러오기
                    string[] arFileNames = System.IO.Directory.GetFiles( strImagePath + "\\" );
                    // 폴더 내의 파일 수만큼 루프
                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        string[] strData = arFileNames[ iLoopCount ].Split( '-' );
                        if( -1 != strData[ 2 ].IndexOf( "CROP_VIDI_" + m_iImageIndex.ToString() ) ) {
                            strImagePath = arFileNames[ iLoopCount ];
                            break;
                        }
                    }
                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;
                    cogDisplay.Image = new CogImage8Grey( ( CogImage8Grey )objImageFileTool.OutputImage );


                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        string[] strData = arFileNames[ iLoopCount ].Split( '-' );
                        if( -1 != strData[ 2 ].IndexOf( "CROP_OVERLAY_VIDI_" + m_iImageIndex.ToString() ) ) {
                            strImagePath = arFileNames[ iLoopCount ];
                            break;
                        }
                    }

                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;

                    CogMaskGraphic objCogMask = new CogMaskGraphic();
                    objCogMask.Image = ( CogImage8Grey )objImageFileTool.OutputImage;
                    objCogMask.SelectedColor = CogColorConstants.Red;

                    AddStaticGraphic( objCogMask, "STATIC_VIDI_RESULT" );

                    CogGraphicLabel objCogLabel = new CogGraphicLabel();
                    objCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                    objCogLabel.Color = CogColorConstants.Black;
                    objCogLabel.Font = new Font( objCogLabel.Font.Name, 10.0F, objCogLabel.Font.Style, objCogLabel.Font.Unit );
                    objCogLabel.SetXYText( 0, 0, "Score : " + objReportImage.strScore );
                    AddStaticGraphic( objCogLabel, "STATIC_VIDI_RESULT" );


                    SetDisplayResult( objReportImage.bResult );

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }
            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayHistoryMeasure( CDefine.structureReportImage objReportImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            string strImagePath = objReportImage.strImagePath;
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();
            bool bImageFail = true;
            do {
                try {

                    SetInspectionAlignGraphicClear();
                    SetHideSearchRegion();
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();

                    // 폴더 내 파일 목록 불러오기
                    string[] arFileNames = System.IO.Directory.GetFiles( strImagePath + "\\" );
                    // 폴더 내의 파일 수만큼 루프
                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        string[] strData = arFileNames[ iLoopCount ].Split( '-' );
                        if( -1 != strData[ 2 ].IndexOf( "CROP_DIMENSION_SECOND_" + m_iImageIndex.ToString() ) ) {
                            strImagePath = arFileNames[ iLoopCount ];
                            break;
                        }
                    }
                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();
                    bImageFail = false;

                    if( null == objImageFileTool.OutputImage ) break;
                    cogDisplay.Image = new CogImage8Grey( ( CogImage8Grey )objImageFileTool.OutputImage );

                    SetDisplayResult( objReportImage.bResult );

                    CogRectangle objRectangle = new CogRectangle();
                    objRectangle.SelectedSpaceName = "@";
                    objRectangle.Color = CogColorConstants.Green;
                    objRectangle.SetXYWidthHeight( objReportImage.dStartX, objReportImage.dStartY, objReportImage.dEndX, objReportImage.dEndY );
                    AddStaticGraphic( objRectangle, "MeasureResult" );

                    // 라벨
                    CogGraphicLabel objLabel = new CogGraphicLabel();
                    objLabel.Color = CogColorConstants.Orange;
                    objLabel.Font = new Font( m_objLabelGuage.Font.Name, 12.0F, m_objLabelGuage.Font.Style, m_objLabelGuage.Font.Unit );
                    objLabel.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;
                    objLabel.Interactive = false;
                    objLabel.Visible = true;
                    double dResolution = CDocument.GetDocument.m_objConfig.GetCameraParameter( m_iCameraIndex ).dResolution;
                    string strDataMeasure = string.Format( "W : {0:F2}  H : {1:F2}", objReportImage.dEndX * dResolution, objReportImage.dEndY * dResolution );
                    objLabel.SetXYText( objReportImage.dStartX, objReportImage.dStartY, strDataMeasure );
                    AddStaticGraphic( objLabel, "MeasureResult" );



                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    if( true == bImageFail )
                        cogDisplay.Image = null;
                    break;
                }
            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayHistory3D( CDefine.structureReportImage objReportImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            string strImagePath = objReportImage.strImagePath + "\\IMAGE_INTENSITY.bmp";
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            do {
                try {

                    SetInspectionAlignGraphicClear();
                    SetHideSearchRegion();
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();
                    m_objLabelResult.Visible = false;

                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;


                    Cognex.VisionPro.CalibFix.CogFixtureTool objFixtureTool = new Cognex.VisionPro.CalibFix.CogFixtureTool();
                    objFixtureTool.InputImage = objImageFileTool.OutputImage.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey;

                    CogTransform2DLinear obj2DLinear = new CogTransform2DLinear();
                    obj2DLinear.TranslationX = objReportImage.dPatternPositionX;
                    obj2DLinear.TranslationY = objReportImage.dPatternPositionY;
                    objFixtureTool.RunParams.UnfixturedFromFixturedTransform = obj2DLinear;
                    objFixtureTool.Run();
                    CogImage8Grey objImage = objFixtureTool.OutputImage as CogImage8Grey;

                    cogDisplay.Image = objImage;

                    SetDisplayResult( objReportImage.bResult );

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }
            } while( false );

            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 택 타임 화면에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDisplayMeasureHeight( CDefine.structureReportImage[] objReportImage )
        {
            do {
                if( null == objReportImage ) break;
                if( 0 >= objReportImage[ 0 ].iFindLineCount ) break;
                for( int iLoopCount = 0; iLoopCount < objReportImage[ 0 ].iFindLineCount; iLoopCount++ ) {
                    CogGraphicLabel objLabelDistance = new CogGraphicLabel();
                    objLabelDistance.Color = CogColorConstants.Red;
                    objLabelDistance.Font = new Font( objLabelDistance.Font.Name, 12.0F, objLabelDistance.Font.Style, objLabelDistance.Font.Unit );
                    objLabelDistance.Alignment = CogGraphicLabelAlignmentConstants.TopCenter;
                    objLabelDistance.SetXYText( objReportImage[ iLoopCount ].dStartX, objReportImage[ iLoopCount ].dEndY, string.Format( "{0:F2}", objReportImage[ iLoopCount ].dLineDistance ) );
                    objLabelDistance.Visible = true;
                    cogDisplay.StaticGraphics.Add( objLabelDistance, "CommonGraphics" );
                    CogLineSegment objLine = new CogLineSegment();
                    objLine.Color = CogColorConstants.Red;
                    objLine.LineWidthInScreenPixels = 3;
                    objLine.SetStartEnd( objReportImage[ iLoopCount ].dStartX, objReportImage[ iLoopCount ].dStartY, objReportImage[ iLoopCount ].dEndX, objReportImage[ iLoopCount ].dEndY );
                    cogDisplay.StaticGraphics.Add( objLine, "CommonGraphics" );
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayHistory3DHeight( CDefine.structureReportImage objReportImage )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            string strImagePath = objReportImage.strImagePath;
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            do {
                try {

                    SetInspectionAlignGraphicClear();
                    SetHideSearchRegion();
                    // 기존 디스플레이 클리어
                    cogDisplay.StaticGraphics.Clear();
                    CommonGraphicsClear();

                    // 폴더 내 파일 목록 불러오기
                    string[] arFileNames = System.IO.Directory.GetFiles( strImagePath + "\\" );
                    // 폴더 내의 파일 수만큼 루프
                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        if( -1 != arFileNames[ iLoopCount ].IndexOf( "CROP_DIMENSION_" + m_iImageIndex.ToString() ) ) {
                            strImagePath = arFileNames[ iLoopCount ];
                            break;
                        }
                    }
                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;
                    cogDisplay.Image = new CogImage8Grey( ( CogImage8Grey )objImageFileTool.OutputImage );


                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        if( -1 != arFileNames[ iLoopCount ].IndexOf( "CROP_DIMENSION_OVERLAY_" + m_iImageIndex.ToString() ) ) {
                            strImagePath = arFileNames[ iLoopCount ];
                            break;
                        }
                    }

                    objImageFileTool.Operator.Open( strImagePath, CogImageFileModeConstants.Read );
                    objImageFileTool.Run();

                    if( null == objImageFileTool.OutputImage ) break;

                    CogMaskGraphic objCogMask = new CogMaskGraphic();
                    objCogMask.Image = ( CogImage8Grey )objImageFileTool.OutputImage;
                    objCogMask.SelectedColor = CogColorConstants.Red;

                    AddStaticGraphic( objCogMask, "STATIC_VIDI_RESULT" );
                    SetDisplayResult( objReportImage.bResult );

                    if( true == m_bUseHistogram )
                        SetHistogram( enumHistogramResult.RESULT_MEAN );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
                    cogDisplay.Image = null;
                    break;
                }
            } while( false );
            objImageFileTool.Operator.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 파일 이미지 로딩
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SaveDisplay( string strImagePath )
        {
            var pDocument = CDocument.GetDocument;
            string strPath = strImagePath;
            do {

                CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();
                try {

                    objImageFileTool.InputImage = cogDisplay.Image;
                    string strDirectory = "";
                    // 폴더생성위해 \\ 구분자로 자름
                    string[] strParse = strPath.Split( '\\' );
                    string strFileName = "";
                    for( int iLoopCount = 0; iLoopCount < strParse.Length - 1; iLoopCount++ ) {
                        // 폴더 위치만 파악
                        strDirectory += strParse[ iLoopCount ] + "\\";
                    }

                    strFileName = strParse[ strParse.Length - 1 ];

                    //폴더 존재 여부 체크
                    if( false == Directory.Exists( strDirectory ) ) {
                        //폴더가 없다면 생성
                        Directory.CreateDirectory( strDirectory );
                    }

                    //objSaveImage.Save(strFileName);
                    objImageFileTool.Operator.Open( strDirectory + strFileName, CogImageFileModeConstants.Write );
                    objImageFileTool.Run();
                    objImageFileTool.Operator.Close();
                } catch( System.Exception ex ) {
                    objImageFileTool.Operator.Close();
                    Trace.WriteLine( "Error SaveImage : " + ex.Message + " ->" + ex.StackTrace );
                }

            } while( false );
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetSize( int iWidth, int iHeight )
        {
            this.Width = iWidth; this.Height = iHeight;
            cogDisplay.Width = iWidth; cogDisplay.Height = iHeight - BtnTitle.Height;
            BtnTitle.Width = iWidth;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetSize( int iPositionX, int iPositionY, int iWidth, int iHeight, bool bUseHitogram = true, bool bTitleVisible = true )
        {
            if( true == bTitleVisible ) {
                this.Location = new Point( iPositionX, iPositionY );//  + 45 );
                this.Width = iWidth; this.Height = iHeight;

                if( true == bUseHitogram ) {
                    // 타이틀은 게이지버튼 크기만큼 빼준다
                    BtnTitle.Width = iWidth - BtnGuage.Width - BtnHistogram.Width;
                    // 게이지 버튼 위치
                    BtnGuage.Location = new Point( iWidth - BtnGuage.Width - BtnHistogram.Width, 1 );
                    // 메뉴얼 얼라인 버튼 위치
                    BtnHistogram.Location = new Point( iWidth - BtnHistogram.Width, 1 );

                    cogDisplay.Location = new Point( 1, BtnTitle.Height );
                    cogDisplay.Width = iWidth - 2;
                    cogDisplay.Height = iHeight - BtnTitle.Height - 1;// -45;
                } else {
                    // 타이틀은 게이지버튼 크기만큼 빼준다
                    BtnTitle.Width = iWidth;// -BtnGuage.Width;
                    BtnHistogram.Visible = false;
                    BtnGuage.Visible = false;
                    // 게이지 버튼 위치
                    BtnGuage.Location = new Point( iWidth - BtnGuage.Width, 1 );
                    // 메뉴얼 얼라인 버튼 위치
                    BtnHistogram.Location = new Point( iWidth - BtnHistogram.Width, 1 );

                    cogDisplay.Location = new Point( 1, BtnTitle.Height );
                    cogDisplay.Width = iWidth - 2;
                    cogDisplay.Height = iHeight - BtnTitle.Height - 1;// -45;
                }

            } else {
                this.Location = new Point( iPositionX, iPositionY );
                this.Width = iWidth; this.Height = iHeight;
                BtnTitle.Visible = false;
                BtnGuage.Visible = false;
                BtnHistogram.Visible = false;
                Point iPoint = new Point( 1, 1 );
                cogDisplay.Location = iPoint;
                cogDisplay.Width = iWidth - 2;
                cogDisplay.Height = iHeight - 2;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 팝업시 컨트롤 활성화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetPopupStyle()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 저장
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SaveImage( CInspectionResult.CResult objResult, bool bManualSave = false )
        {
            var pDocument = CDocument.GetDocument;
            do {

                if( false == pDocument.m_objConfig.GetSystemParameter().bImageSave ) break;
                CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

                try {
                    objImageFileTool.InputImage = objResult.objResultCommon.objOutputGrabImage;
                    string strDirectory;
                    string strFolder = "";
                    string strResultFolder = "";

                    if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.eResult ) {
                        strResultFolder = "OK";
                    } else if( CDefine.enumResult.RESULT_NG == objResult.objResultCommon.eResult ) {
                        strResultFolder = "NG";
                    }

                    // 나중에 폴더정리합시다
                    switch( pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                        case CDefine.enumMachineType.PROCESS_60:
                            strFolder = ( ( CDefine.enumCamera )m_iCameraIndex ).ToString();
                            break;
                        case CDefine.enumMachineType.PROCESS_110:
                            strFolder = ( ( CDefine.enumCamera )m_iCameraIndex ).ToString();
                            break;
                        case CDefine.enumMachineType.PROCESS_150:
                            strFolder = ( ( CDefine.enumCamera )m_iCameraIndex ).ToString();
                            break;
                        default:
                            strFolder = "None";
                            break;
                    }

                    if( false == bManualSave )
                        strDirectory = string.Format( "{0}\\{1:D4}-{2:D2}-{3:D2}\\AUTO\\{4}\\{5}\\", pDocument.m_objConfig.GetSystemParameter().strImageSavePath, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, strFolder, strResultFolder );
                    else
                        strDirectory = string.Format( "{0}\\{1:D4}-{2:D2}-{3:D2}\\MANUAL\\{4}\\", pDocument.m_objConfig.GetSystemParameter().strImageSavePath, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, strFolder );

                    //폴더 존재 여부 체크
                    if( false == Directory.Exists( strDirectory ) ) {
                        //폴더가 없다면 생성
                        Directory.CreateDirectory( strDirectory );
                    }

                    string strSaveType = "";
                    if( CDefine.enumSaveImageType.TYPE_BMP == pDocument.m_objConfig.GetSystemParameter().eSaveImageType )
                        strSaveType = ".bmp";
                    else
                        strSaveType = ".jpg";

                    string strFileName = strDirectory;
                    strFileName += string.Format( "{0:D4}", DateTime.Now.Year );
                    strFileName += "-";
                    strFileName += string.Format( "{0:D2}", DateTime.Now.Month );
                    strFileName += "-";
                    strFileName += string.Format( "{0:D2}", DateTime.Now.Day );
                    strFileName += " ";
                    strFileName += string.Format( "{0:D2}", DateTime.Now.Hour );
                    strFileName += " ";
                    strFileName += string.Format( "{0:D2}", DateTime.Now.Minute );
                    strFileName += " ";
                    strFileName += string.Format( "{0:D2}", DateTime.Now.Second );
                    strFileName += " ";
                    strFileName += string.Format( "{0:D3}", DateTime.Now.Millisecond );
                    strFileName += strSaveType;

                    objImageFileTool.Operator.Open( strFileName, CogImageFileModeConstants.Write );
                    objImageFileTool.Run();
                    objImageFileTool.Operator.Close();
                } catch( System.Exception ex ) {
                    objImageFileTool.Operator.Close();
                    Trace.WriteLine( "Error SaveImage : " + ex.Message + " ->" + ex.StackTrace );
                }
                objImageFileTool.Dispose();
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 저장
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SaveImage( object strImagePath )
        {
            var pDocument = CDocument.GetDocument;
            string strPath = strImagePath as string;
            do {

                if( false == pDocument.m_objConfig.GetSystemParameter().bImageSave ) break;
                CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

                try {

                    objImageFileTool.InputImage = cogDisplay.Image;
                    string strDirectory = "";
                    // 폴더생성위해 \\ 구분자로 자름
                    string[] strParse = strPath.Split( '\\' );
                    string strFileName = "";
                    for( int iLoopCount = 0; iLoopCount < strParse.Length - 1; iLoopCount++ ) {
                        // 폴더 위치만 파악
                        strDirectory += strParse[ iLoopCount ] + "\\";
                    }

                    strFileName = strParse[ strParse.Length - 1 ];

                    //폴더 존재 여부 체크
                    if( false == Directory.Exists( strDirectory ) ) {
                        //폴더가 없다면 생성
                        Directory.CreateDirectory( strDirectory );
                    }

                    //objSaveImage.Save(strFileName);
                    objImageFileTool.Operator.Open( strDirectory + strFileName, CogImageFileModeConstants.Write );
                    objImageFileTool.Run();
                    objImageFileTool.Operator.Close();
                } catch( System.Exception ex ) {
                    objImageFileTool.Operator.Close();
                    Trace.WriteLine( "Error SaveImage : " + ex.Message + " ->" + ex.StackTrace );
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer_Tick( object sender, EventArgs e )
        {
            // 버튼 언어 변경
            SetChangeLanguage();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnGuage_Click( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            if( false == m_bUseGuage ) {

                m_objLabelGuage = new CogGraphicLabel();
                m_objLabelGuage.Color = CogColorConstants.Orange;
                m_objLabelGuage.Font = new Font( m_objLabelGuage.Font.Name, 12.0F, m_objLabelGuage.Font.Style, m_objLabelGuage.Font.Unit );
                m_objLabelGuage.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;
                m_objLabelGuage.Interactive = false;
                m_objLabelGuage.Visible = false;
                cogDisplay.InteractiveGraphics.Add( m_objLabelGuage, "InspectGraphics", false );
                // 사각형
                m_objRectangleGuage = new CogRectangleAffine();
                m_objRectangleGuage.Color = CogColorConstants.Orange;
                m_objRectangleGuage.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                m_objRectangleGuage.SetCenterLengthsRotationSkew( 50, 50, 100, 100, 0, 0 );
                m_objRectangleGuage.Interactive = true;
                m_objRectangleGuage.Visible = false;
                cogDisplay.InteractiveGraphics.Add( m_objRectangleGuage, "InspectGraphics", false );

                m_bUseGuage = true;
                m_objLabelGuage.Visible = true;
                m_objRectangleGuage.Visible = true;

                // 최초 생성시 길이 표시를 위함
                double dOriginX, dOriginY, dLenghX, dLenghY, dLoatation, dSkew;
                m_objRectangleGuage.GetOriginLengthsRotationSkew( out dOriginX, out dOriginY, out dLenghX, out dLenghY, out dLoatation, out dSkew );

                double dResolution = CDocument.GetDocument.m_objConfig.GetCameraParameter( m_iCameraIndex ).dResolution;
                string strData = string.Format( "W : {0:F3}, H : {1:F3}", dLenghX * dResolution, dLenghY * dResolution );
                m_objLabelGuage.SetXYText( dOriginX, dOriginY - 20, strData );
                // 활성화
                pFormCommon.SetButtonColor( this.BtnGuage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
            } else {
                m_bUseGuage = false;
                m_objLabelGuage.Visible = false;
                m_objRectangleGuage.Visible = false;
                InspectGraphicsClear();
                // 비활성화
                pFormCommon.SetButtonColor( this.BtnGuage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 드레그 정지 이벤트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void cogDisplay_DraggingStopped( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            if( true == m_bTeach ) {
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                    // 나중에 모드로 만들어서 적용하자
                    // m_objRectangleSerchRegion[ iLoopCount ].SelectedSpaceName = ".";
                    //cogDisplay.Image.SelectedSpaceName = "@";
                    try {
                        m_objRectangleSerchRegion[ iLoopCount ].GetOriginLengthsRotationSkew( out dStartX, out dStartY, out dEndX, out dEndY, out dRotation, out dSkew );
                        m_objRectangleSerchRegion[ iLoopCount ].SetOriginLengthsRotationSkew( dStartX, dStartY, dVidiFixedSizeWidth, dVidiFixedSizeHeight, dRotation, dSkew );
                        m_objLabelIndexSerchRegion[ iLoopCount ].SetXYText( dStartX, dStartY, "INDEX : " + ( iLoopCount + 1 ).ToString() );

                        double dGradientStartX = dStartX;
                        //                         m_objRectangleGradientRegion[ 0 ].GetOriginLengthsRotationSkew( out dStartX, out dStartY, out dEndX, out dEndY, out dRotation, out dSkew );
                        //                         double dGradientStartY = dStartY;

                        m_objRectangleGradientRegion[ iLoopCount ].GetOriginLengthsRotationSkew( out dStartX, out dStartY, out dEndX, out dEndY, out dRotation, out dSkew );
                        m_objRectangleGradientRegion[ iLoopCount ].SetOriginLengthsRotationSkew( /*dStartX*/dGradientStartX, dStartY, dVidiFixedSizeWidthGradient, dVidiFixedSizeHeightGradient, dRotation, dSkew );
                        //m_objRectangleGradientRegion[ iLoopCount ].SetOriginLengthsRotationSkew( /*dStartX*/dGradientStartX, dGradientStartY/*dStartY*/, dVidiFixedSizeWidthGradient, dVidiFixedSizeHeightGradient, dRotation, dSkew );
                        m_objLabelIndexGradientRegion[ iLoopCount ].SetXYText( /*dStartX*/dGradientStartX, dStartY, "GRADIENT : " + ( iLoopCount + 1 ).ToString() );
                    } catch {
                    }

                }
            }
            if( true == m_bUseGuage ) {
                SetGuage();
            }

            if( true == m_bUseHistogram ) {
                SetHistogram( enumHistogramResult.RESULT_MEAN );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddInteractiveGraphic( ICogGraphicInteractive graphic, string groupName, bool checkForDuplicates )
        {
            graphic.Interactive = true;
            graphic.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
            cogDisplay.InteractiveGraphics.Add( graphic, groupName, checkForDuplicates );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddStaticGraphic( ICogGraphic graphic, string groupName )
        {
            cogDisplay.StaticGraphics.Add( graphic, groupName );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DeletestaticGraphic( string groupName )
        {
            cogDisplay.StaticGraphics.Remove( groupName );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 티칭값 저장을 위해 영역 리턴
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CogRectangleAffine GetRectangleAffine( int iIndex )
        {
            CogRectangleAffine objRectangle = null;
            if( null != m_objRectangleSerchRegion[ iIndex ] )
                objRectangle = m_objRectangleSerchRegion[ iIndex ];

            return objRectangle;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 티칭값 저장을 위해 영역 리턴
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CogRectangleAffine GetRectangleAffineGradient( int iIndex )
        {
            CogRectangleAffine objRectangle = null;
            if( null != m_objRectangleGradientRegion[ iIndex ] )
                objRectangle = m_objRectangleGradientRegion[ iIndex ];

            return objRectangle;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이틀 누르면 화면 커짐
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnTitle_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            if( null != EventChangeSizeDisplay ) {
                EventChangeSizeDisplay( m_iDisplayIndex );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnHistogram_Click( object sender, EventArgs e )
        {
            // 바꼇을때 최초1회 측정
            var pFormCommon = CFormCommon.GetFormCommon;
            if( false == m_bUseHistogram ) {

                SetHistogram( enumHistogramResult.RESULT_MEAN );

                m_bUseHistogram = true;
                m_objLabelHistogram.Visible = true;
                ( ( CogRectangle )m_histTool.Region ).Visible = true;

                // 활성화
                pFormCommon.SetButtonColor( this.BtnHistogram, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
            } else {
                m_bUseHistogram = false;
                m_objLabelHistogram.Visible = false;
                ( ( CogRectangle )m_histTool.Region ).Visible = false;
                // 비활성화
                pFormCommon.SetButtonColor( this.BtnHistogram, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetGuage()
        {
            // 최초 생성시 길이 표시를 위함
            double dOriginX, dOriginY, dLenghX, dLenghY, dLoatation, dSkew;
            m_objRectangleGuage.GetOriginLengthsRotationSkew( out dOriginX, out dOriginY, out dLenghX, out dLenghY, out dLoatation, out dSkew );

            double dResolution = CDocument.GetDocument.m_objConfig.GetCameraParameter( m_iCameraIndex ).dResolution;
            string strData = string.Format( "W : {0:F3}, H : {1:F3}", dLenghX * dResolution, dLenghY * dResolution );
            m_objLabelGuage.SetXYText( dOriginX, dOriginY - 20, strData );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetHistogram( enumHistogramResult eHistogramResult )
        {
            do {
                if( null == cogDisplay.Image ) break;
                m_histTool.InputImage = cogDisplay.Image;
                m_histTool.Run();

                double dStartX, dStartY, dEndX, dEndY;
                ( ( CogRectangle )m_histTool.Region ).GetXYWidthHeight( out dStartX, out dStartY, out dEndX, out dEndY );

                if( null == m_histTool.Result ) {
                    ( ( CogRectangle )m_histTool.Region ).SetXYWidthHeight( 300, 30, 300, 30 );
                    m_histTool.InputImage = cogDisplay.Image;
                    m_histTool.Run();
                    if( null == m_histTool.Result ) {
                        break;
                    }
                }
                switch( eHistogramResult ) {
                    case enumHistogramResult.RESULT_MAX:
                        m_objLabelHistogram.SetXYText( dStartX, dStartY, "MAX : " + m_histTool.Result.Maximum.ToString( "0.0" ) );
                        break;
                    case enumHistogramResult.RESULT_MIN:
                        m_objLabelHistogram.SetXYText( dStartX, dStartY, "MIN : " + m_histTool.Result.Minimum.ToString( "0.0" ) );
                        break;
                    case enumHistogramResult.RESULT_MEAN:
                        m_objLabelHistogram.SetXYText( dStartX, dStartY, "MEAN : " + m_histTool.Result.Mean.ToString( "0.0" ) );
                        break;
                }

            } while( false );
        }

        public void CallbackPosition(object sender, CogChangedEventArgs e)
        {
            try
            {
                cogDisplay.StaticGraphics.Remove("User Position Height");

                CogGraphicLabel coglbl = new CogGraphicLabel();
                double dHeight = m_obj3DDataHeightCrop1d[Convert.ToInt32(m_ptPosHeight.X) + (Convert.ToInt32(m_ptPosHeight.Y) * cogDisplay.Image.Width)];
                coglbl.SelectedSpaceName = "@";
                coglbl.Color = CogColorConstants.Red;
                coglbl.SetXYText(m_ptPosHeight.X, m_ptPosHeight.Y, dHeight.ToString("0.00"));

                cogDisplay.StaticGraphics.Add(coglbl, "User Position Height");
            }
            catch { }
        }
    }
}