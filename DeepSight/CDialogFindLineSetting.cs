using ChartDirector;
using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSight
{
    public partial class CDialogFindLineSetting : Form
    {

        // 크롭인덱스
        private int m_iPositionCrop;
        private int m_iPositionInspection;

        // 메인 코그 디스플레이
        private CogDisplay m_objCogDisplayMain;
        // 파인드 라인 툴
        private CogFindLineTool m_objFindLineToolTop;
        private CogFindLineTool m_objFindLineToolBottom;
        private bool m_bSaveAll;
        public CConfig.CRecipeParameter m_objRecipeParameter;

        List<CogImage8Grey> m_objListImage;

        public CDialogFindLineSetting( int iInspectionPosition, List<CogImage8Grey> objListImage, CConfig.CRecipeParameter objParameter )
        {
            m_objRecipeParameter = objParameter;
            m_iPositionInspection = iInspectionPosition;
            m_objListImage = objListImage;
            InitializeComponent();
            Initialize();
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
            var pDocument = CDocument.GetDocument;
            do {
                InitializeForm();
                timer1.Start();
                m_objCogDisplayMain = cogDisplayRunImage;
               // m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );
                m_objFindLineToolTop = new CogFindLineTool();
                m_objFindLineToolBottom = new CogFindLineTool();
                m_iPositionCrop = 0;
                Reload();
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
        private void DeInitialize()
        {
            cogDisplayRunImage.InteractiveGraphics.Dispose();
            cogDisplayRunImage.Dispose();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool InitializeForm()
        {
            bool bReturn = false;

            do {
                // 폼 중앙에서 생성
                this.CenterToParent();
                SetButtonColor();
                bReturn = true;
            } while( false );

            return bReturn;
        }

        private void Reload()
        {
            var pDocument = CDocument.GetDocument;
            try {
                m_objFindLineToolTop.InputImage = m_objListImage[ m_iPositionCrop ];

                CConfig.CFindLineParameter objParameter = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ];
                m_objFindLineToolTop.RunParams.NumCalipers = objParameter.iCalipersNumber;
                m_objFindLineToolTop.RunParams.NumToIgnore = objParameter.iIgnoreNumber;
                m_objFindLineToolTop.RunParams.CaliperRunParams.ContrastThreshold = objParameter.iThreshold;
                m_objFindLineToolTop.RunParams.CaliperRunParams.FilterHalfSizeInPixels = objParameter.iFilter;
                m_objFindLineToolTop.RunParams.CaliperSearchLength = objParameter.dSearchLength;
                m_objFindLineToolTop.RunParams.CaliperProjectionLength = 20;
                m_objFindLineToolTop.RunParams.CaliperRunParams.Edge0Polarity = (CogCaliperPolarityConstants)objParameter.ePolaraty;
                if( CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90 == objParameter.eSerachDirection )
                    m_objFindLineToolTop.RunParams.CaliperSearchDirection = 90 * ( Math.PI / 180 );
                else
                    m_objFindLineToolTop.RunParams.CaliperSearchDirection = -90 * ( Math.PI / 180 );

                m_objFindLineToolTop.RunParams.ExpectedLineSegment.SelectedSpaceName = "#";
                m_objFindLineToolTop.RunParams.ExpectedLineSegment.StartX = ( m_objListImage[ m_iPositionCrop ].Width / 2 ) - ( m_objListImage[ m_iPositionCrop ].Width / 6 );
                m_objFindLineToolTop.RunParams.ExpectedLineSegment.EndX = ( m_objListImage[ m_iPositionCrop ].Width / 2 ) + ( m_objListImage[ m_iPositionCrop ].Width / 6 );
                m_objFindLineToolTop.RunParams.ExpectedLineSegment.StartY = 0;
                m_objFindLineToolTop.RunParams.ExpectedLineSegment.EndY = 0;
                
                objParameter = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ];
                m_objFindLineToolBottom.InputImage = m_objListImage[ m_iPositionCrop ];
                m_objFindLineToolBottom.RunParams.NumCalipers = objParameter.iCalipersNumber;
                m_objFindLineToolBottom.RunParams.NumToIgnore = objParameter.iIgnoreNumber;
                m_objFindLineToolBottom.RunParams.CaliperRunParams.ContrastThreshold = objParameter.iThreshold;
                m_objFindLineToolBottom.RunParams.CaliperRunParams.FilterHalfSizeInPixels = objParameter.iFilter;
                m_objFindLineToolBottom.RunParams.CaliperSearchLength = objParameter.dSearchLength;
                m_objFindLineToolBottom.RunParams.CaliperProjectionLength = 20;
                m_objFindLineToolBottom.RunParams.CaliperRunParams.Edge0Polarity = ( CogCaliperPolarityConstants )objParameter.ePolaraty;
                if( CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90 == objParameter.eSerachDirection )
                    m_objFindLineToolBottom.RunParams.CaliperSearchDirection = 90 * ( Math.PI / 180 );
                else
                    m_objFindLineToolBottom.RunParams.CaliperSearchDirection = -90 * ( Math.PI / 180 );

                m_objFindLineToolBottom.RunParams.ExpectedLineSegment.SelectedSpaceName = "#";
                m_objFindLineToolBottom.RunParams.ExpectedLineSegment.StartX = ( m_objListImage[ m_iPositionCrop ].Width / 2 ) - ( m_objListImage[ m_iPositionCrop ].Width / 6 );
                m_objFindLineToolBottom.RunParams.ExpectedLineSegment.EndX = ( m_objListImage[ m_iPositionCrop ].Width / 2 ) + ( m_objListImage[ m_iPositionCrop ].Width / 6 );
                m_objFindLineToolBottom.RunParams.ExpectedLineSegment.StartY = m_objListImage[ m_iPositionCrop ].Height;
                m_objFindLineToolBottom.RunParams.ExpectedLineSegment.EndY = m_objListImage[ m_iPositionCrop ].Height;

                GetFindLineTop();
                GetFindLineBottom();
            } catch( Exception ex ) {
                pDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "CDialogFindLineSetting - Reload : " + ex.ToString() );
            }
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 툴 내 파인드 라인 그래픽 -> 현재 디스플레이에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GetFindLineTop()
        {
            do {
                // 디스플레이 초기화
                m_objCogDisplayMain.Image = null;
                // 이미지 유무 체크해서 메인 디스플레이에 이미지를 가져옴
                SetMainImage();
                // 디스플레이 클리어
                m_objCogDisplayMain.StaticGraphics.Clear();
                m_objCogDisplayMain.InteractiveGraphics.Clear();
                // 이미지 유무 체크
                if( null == m_objCogDisplayMain.Image ) break;
                // 극성 방향
                CogLineSegment objLineSegment;
                // 캘리퍼 사각형's
                CogGraphicCollection objRegions;
                m_objFindLineToolTop.CurrentRecordEnable = CogFindLineCurrentRecordConstants.All;
                ICogRecord objCogRecord = m_objFindLineToolTop.CreateCurrentRecord();
                objLineSegment = ( CogLineSegment )objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "ExpectedShapeSegment" ].Content;
                objRegions = ( CogGraphicCollection )objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "CaliperRegions" ].Content;
                // 캘리퍼 선
                objLineSegment.Color = CogColorConstants.Green;
                //m_objCogDisplayMain.InteractiveGraphics.Add( ( ICogGraphicInteractive )objLineSegment, "", false );
                m_objCogDisplayMain.StaticGraphics.Add( ( ICogGraphicInteractive )objLineSegment, "" );
                // 캘리퍼 상자
                if( null != objRegions ) {
                    for( int iLoopCount = 0; iLoopCount < objRegions.Count; iLoopCount++ ) {
                        objRegions[ iLoopCount ].Color = CogColorConstants.Green;
                        //m_objCogDisplayMain.InteractiveGraphics.Add( ( ICogGraphicInteractive )objRegions[ iLoopCount ], "", false );
                        m_objCogDisplayMain.StaticGraphics.Add( ( ICogGraphicInteractive )objRegions[ iLoopCount ], "" );
                    }
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 툴 내 파인드 라인 그래픽 -> 현재 디스플레이에 표시
        //설명 : 소스코드야 미안하다
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GetFindLineBottom()
        {
            do {
//                 // 디스플레이 초기화
//                 m_objCogDisplayMain.Image = null;
//                 // 이미지 유무 체크해서 메인 디스플레이에 이미지를 가져옴
//                 SetMainImage();
//                 // 디스플레이 클리어
//                 m_objCogDisplayMain.StaticGraphics.Clear();
//                 m_objCogDisplayMain.InteractiveGraphics.Clear();
                // 이미지 유무 체크
                if( null == m_objCogDisplayMain.Image ) break;
                // 극성 방향
                CogLineSegment objLineSegment;
                // 캘리퍼 사각형's
                CogGraphicCollection objRegions;
                m_objFindLineToolBottom.CurrentRecordEnable = CogFindLineCurrentRecordConstants.All;
                ICogRecord objCogRecord = m_objFindLineToolBottom.CreateCurrentRecord();
                objLineSegment = ( CogLineSegment )objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "ExpectedShapeSegment" ].Content;
                objRegions = ( CogGraphicCollection )objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "CaliperRegions" ].Content;
                // 캘리퍼 선
                objLineSegment.Color = CogColorConstants.Green;
                //objLineSegment.Interactive = false;
                //m_objCogDisplayMain.InteractiveGraphics.Add( ( ICogGraphicInteractive )objLineSegment, "", false );
                m_objCogDisplayMain.StaticGraphics.Add( ( ICogGraphicInteractive )objLineSegment, "" );

                // 캘리퍼 상자
                if( null != objRegions ) {
                    for( int iLoopCount = 0; iLoopCount < objRegions.Count; iLoopCount++ ) {
                        objRegions[ iLoopCount ].Color = CogColorConstants.Green;
                        //m_objCogDisplayMain.InteractiveGraphics.Add( ( ICogGraphicInteractive )objRegions[ iLoopCount ], "", false );
                        m_objCogDisplayMain.StaticGraphics.Add( ( ICogGraphicInteractive )objRegions[ iLoopCount ], "" );
                    }
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 유무 체크해서 메인 디스플레이에 이미지를 가져옴
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetMainImage()
        {
            do {
                if( null == m_objFindLineToolTop.InputImage ) break;
                // 이미지 null 체크 말고도.. 이미지 할당 유무 체크해야 함.
                if( false == m_objFindLineToolTop.InputImage.Allocated ) break;

                m_objCogDisplayMain.Image = m_objFindLineToolTop.InputImage;

            } while( false );
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

            //pFormCommon.SetButtonColor( this.BtnPositionCrop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleFindLineSettingTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleNumberTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnNumberTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleIgnoreNumberTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnIgnoreNumberTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleSearchLengthTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSearchLengthTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleContrastThresholdTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnContrastThresholdTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleFilterHalfSizePixelsTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnFilterHalfSizePixelsTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitlePolarityTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnPolarityTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSearchDirectionTop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleFindLineSettingBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleNumberBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnNumberBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleIgnoreNumberBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnIgnoreNumberBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleSearchLengthBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSearchLengthBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleContrastThresholdBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnContrastThresholdBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleFilterHalfSizePixelsBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnFilterHalfSizePixelsBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitlePolarityBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnPolarityBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSearchDirectionBottom, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleCropPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnCropPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnRun, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSave, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCancel, pFormCommon.COLOR_WHITE, pFormCommon.LAMP_RED_OFF );
        }

      
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 취소
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void timer1_Tick( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            pFormCommon.SetButtonText( this.BtnCropPosition, "CROP POSITION " + ( m_iPositionCrop + 1 ).ToString() );
            CConfig.CFindLineParameter objParameter = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ];
            pFormCommon.SetButtonText( this.BtnNumberTop, objParameter.iCalipersNumber.ToString() );
            pFormCommon.SetButtonText( this.BtnIgnoreNumberTop, objParameter.iIgnoreNumber.ToString() );
            pFormCommon.SetButtonText( this.BtnSearchLengthTop, objParameter.dSearchLength.ToString() );
            pFormCommon.SetButtonText( this.BtnContrastThresholdTop, objParameter.iThreshold.ToString() );
            pFormCommon.SetButtonText( this.BtnFilterHalfSizePixelsTop, objParameter.iFilter.ToString() );
            pFormCommon.SetButtonText( this.BtnPolarityTop, string.Format( "{0}", objParameter.ePolaraty.ToString() ) );

            objParameter = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ];
            pFormCommon.SetButtonText( this.BtnNumberBottom, objParameter.iCalipersNumber.ToString() );
            pFormCommon.SetButtonText( this.BtnIgnoreNumberBottom, objParameter.iIgnoreNumber.ToString() );
            pFormCommon.SetButtonText( this.BtnSearchLengthBottom, objParameter.dSearchLength.ToString() );
            pFormCommon.SetButtonText( this.BtnContrastThresholdBottom, objParameter.iThreshold.ToString() );
            pFormCommon.SetButtonText( this.BtnFilterHalfSizePixelsBottom, objParameter.iFilter.ToString() );
            pFormCommon.SetButtonText( this.BtnPolarityBottom, string.Format( "{0}", objParameter.ePolaraty.ToString() ) );

            if( true == m_bSaveAll )
                pFormCommon.SetButtonBackColor( this.BtnTitleCropPosition, pFormCommon.COLOR_ACTIVATE );
            else
                pFormCommon.SetButtonBackColor( this.BtnTitleCropPosition, pFormCommon.COLOR_UNACTIVATE );
        }

        private void BtnCropPosition_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );

                string[] strButtonList = new string[ objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].iCountSerchRegion ];
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].iCountSerchRegion; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = "CROP POSITION " + ( iLoopCount + 1 ).ToString();
                }
                CDialogEnumerate objDialog = new CDialogEnumerate( objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].iCountSerchRegion, strButtonList, m_iPositionCrop );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    m_iPositionCrop = objDialog.GetResult();
                    Reload();
                }

            } while( false );
        }

        private void BtnNumberTop_Click( object sender, EventArgs e )
        {
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iCalipersNumber );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( 30 <= ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 30;
                } else if( 2 > ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 2;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iCalipersNumber = ( int )objKeyPad.m_dResultValue;
                Reload();
            }
        }

        private void BtnIgnoreNumberTop_Click( object sender, EventArgs e )
        {
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iIgnoreNumber );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iCalipersNumber <= ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iCalipersNumber -1;
                } else if( 0 > ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 0;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iIgnoreNumber = ( int )objKeyPad.m_dResultValue;
                Reload();
            }
        }

        private void BtnContrastThresholdTop_Click( object sender, EventArgs e )
        {
            int iValue = 0;
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iThreshold );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( 1 > ( int )objKeyPad.m_dResultValue ) {
                    iValue  = 1;
                } else {
                    iValue = ( int )objKeyPad.m_dResultValue;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iThreshold = iValue;
                Reload();
            }
        }

        private void BtnFilterHalfSizePixelsTop_Click( object sender, EventArgs e )
        {
            int iValue = 0;
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iFilter );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( 1 > ( int )objKeyPad.m_dResultValue ) {
                    iValue = 1;
                } else {
                    iValue = ( int )objKeyPad.m_dResultValue;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].iFilter = iValue;
                Reload();
            }
        }
        private void BtnPolarityTop_Click( object sender, EventArgs e )
        {
            switch( m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].ePolaraty ) {
                case CConfig.CFindLineParameter.enumPolaraty.DarkToLight:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].ePolaraty = CConfig.CFindLineParameter.enumPolaraty.DontCare;
                    break;
                case CConfig.CFindLineParameter.enumPolaraty.DontCare:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].ePolaraty = CConfig.CFindLineParameter.enumPolaraty.LightToDark;
                    break;
                case CConfig.CFindLineParameter.enumPolaraty.LightToDark:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].ePolaraty = CConfig.CFindLineParameter.enumPolaraty.DarkToLight;
                    break;
            }
            Reload();
        }
        private void BtnSearchDirectionTop_Click( object sender, EventArgs e )
        {
            switch( m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].eSerachDirection ) {
                case CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].eSerachDirection = CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_180;
                    break;
                case CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_180:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].eSerachDirection = CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90;
                    break;
            }
            Reload();
        }

        private void BtnNumberBottom_Click( object sender, EventArgs e )
        {
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iCalipersNumber );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( 30 <= ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 30;
                } else if( 2 > ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 2;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iCalipersNumber = ( int )objKeyPad.m_dResultValue;
                Reload();
            }
        }

        private void BtnIgnoreNumberBottom_Click( object sender, EventArgs e )
        {
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iIgnoreNumber );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iCalipersNumber <= ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iCalipersNumber - 1;
                } else if( 0 > ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 0;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iIgnoreNumber = ( int )objKeyPad.m_dResultValue;
                Reload();
            }
        }

        private void BtnContrastThresholdBottom_Click( object sender, EventArgs e )
        {
            int iValue = 0;
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iThreshold );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( 1 > ( int )objKeyPad.m_dResultValue ) {
                    iValue = 1;
                } else {
                    iValue = ( int )objKeyPad.m_dResultValue;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iThreshold = iValue;
                Reload();
            }
            
        }

        private void BtnFilterHalfSizePixelsBottom_Click( object sender, EventArgs e )
        {
            int iValue = 0;
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iFilter );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                // 예외 처리 값은 2 > Value
                if( 1 > ( int )objKeyPad.m_dResultValue ) {
                    iValue = 1;
                } else {
                    iValue = ( int )objKeyPad.m_dResultValue;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].iFilter = iValue;
                Reload();
            }
        }
        private void BtnPolarityBottom_Click( object sender, EventArgs e )
        {
            switch( m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].ePolaraty ) {
                case CConfig.CFindLineParameter.enumPolaraty.DarkToLight:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].ePolaraty = CConfig.CFindLineParameter.enumPolaraty.DontCare;
                    break;
                case CConfig.CFindLineParameter.enumPolaraty.DontCare:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].ePolaraty = CConfig.CFindLineParameter.enumPolaraty.LightToDark;
                    break;
                case CConfig.CFindLineParameter.enumPolaraty.LightToDark:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].ePolaraty = CConfig.CFindLineParameter.enumPolaraty.DarkToLight;
                    break;
            }
            Reload();
        }
        private void BtnSearchDirectionBottom_Click( object sender, EventArgs e )
        {
            switch( m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].eSerachDirection ) {
                case CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].eSerachDirection = CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_180;
                    break;
                case CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_180:
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].eSerachDirection = CConfig.CFindLineParameter.enumSerarchDirection.DIRECTION_90;
                    break;
            }
            Reload();
        }

        private void BtnTitleFindLineSettingTop_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                CUserInformation objUserInformation = pDocument.GetUserInformation();
                // 마스터만 가능하도록
                if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER != objUserInformation.m_eAuthorityLevel ) break;

                // ocx 넣기 전에 이미지 변경해줌
                CDialogCogFindLine obj = new CDialogCogFindLine( m_objFindLineToolTop );
                obj.ShowDialog();

            } while( false );
            Reload();
        }

        private void BtnTitleFindLineSettingBottom_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                CUserInformation objUserInformation = pDocument.GetUserInformation();
                // 마스터만 가능하도록
                if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER != objUserInformation.m_eAuthorityLevel ) break;

                // ocx 넣기 전에 이미지 변경해줌
                CDialogCogFindLine obj = new CDialogCogFindLine( m_objFindLineToolBottom );
                obj.ShowDialog();

            } while( false );
            Reload();
        }

        private void BtnSearchLengthTop_Click( object sender, EventArgs e )
        {
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].dSearchLength );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                if( 10 > ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 10;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].dSearchLength = objKeyPad.m_dResultValue;
                Reload();
            }
        }

        private void BtnSearchLengthBottom_Click( object sender, EventArgs e )
        {
            FormKeyPad objKeyPad = new FormKeyPad( ( double )m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].dSearchLength );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                if( 10 > ( int )objKeyPad.m_dResultValue ) {
                    objKeyPad.m_dResultValue = 10;
                }
                m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].dSearchLength = objKeyPad.m_dResultValue;
                Reload();
            }
        }

        private void BtnRun_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                try {

                    m_objFindLineToolTop.Run();
                    m_objFindLineToolBottom.Run();

                    m_objCogDisplayMain.InteractiveGraphics.Clear();
                    m_objCogDisplayMain.StaticGraphics.Clear();

                    bool bResultTop = false;
                    bool bResultBottom = false;
                    if( null != m_objFindLineToolTop.Results && 0 < m_objFindLineToolTop.Results.Count ) {
                        for( int iLoopCount = 0; iLoopCount < m_objFindLineToolTop.Results.Count; iLoopCount++ ) {
                            m_objCogDisplayMain.StaticGraphics.Add( m_objFindLineToolTop.Results[ iLoopCount ].CreateResultGraphics( CogFindLineResultGraphicConstants.DataPoint | CogFindLineResultGraphicConstants.TipText ), "" );
                            m_objCogDisplayMain.StaticGraphics.Add( m_objFindLineToolTop.Results.GetLine(), "" );
                        }
                        bResultTop = true;
                    }

                    if( null != m_objFindLineToolBottom.Results && 0 < m_objFindLineToolBottom.Results.Count ) {
                        for( int iLoopCount = 0; iLoopCount < m_objFindLineToolBottom.Results.Count; iLoopCount++ ) {
                            m_objCogDisplayMain.StaticGraphics.Add( m_objFindLineToolBottom.Results[ iLoopCount ].CreateResultGraphics( CogFindLineResultGraphicConstants.DataPoint | CogFindLineResultGraphicConstants.TipText ), "" );
                            m_objCogDisplayMain.StaticGraphics.Add( m_objFindLineToolBottom.Results.GetLine(), "" );
                        }
                        bResultBottom = true;
                    }

                    if( false == bResultTop || false == bResultBottom )
                        break;

                    CogLine objCogLine = new CogLine();
                    objCogLine.SelectedSpaceName = "#";
                    objCogLine.SetXYRotation( m_objListImage[ m_iPositionCrop ].Width / 2, 0, 90 * ( Math.PI / 180 ) );
                    m_objCogDisplayMain.StaticGraphics.Add( objCogLine, "" );

                    double dPositionXTop = 0;
                    double dPositionYTop = 0;
                    double dPositionXBottom = 0;
                    double dPositionYBottom = 0;

                    CogIntersectLineLineTool objIntersectLine = new CogIntersectLineLineTool();
                    objIntersectLine.InputImage = m_objListImage[ m_iPositionCrop ];
                    objIntersectLine.LineA = m_objFindLineToolTop.Results.GetLine();
                    objIntersectLine.LineB = objCogLine;
                    objIntersectLine.Run();
                    dPositionXTop = objIntersectLine.X;
                    dPositionYTop = objIntersectLine.Y;

                    objIntersectLine.LineA = m_objFindLineToolBottom.Results.GetLine();
                    objIntersectLine.LineB = objCogLine;
                    objIntersectLine.Run();
                    dPositionXBottom = objIntersectLine.X;
                    dPositionYBottom = objIntersectLine.Y;

                    CogDistancePointPointTool objDistance = new CogDistancePointPointTool();
                    objDistance.InputImage = m_objListImage[ m_iPositionCrop ];
                    objDistance.StartX = dPositionXTop;
                    objDistance.StartY = dPositionYTop;
                    objDistance.EndX = dPositionXBottom;
                    objDistance.EndY = dPositionYBottom;
                    objDistance.Run();

                    CogGraphicLabel objLabel = new CogGraphicLabel();
                    objLabel.SelectedSpaceName = "#";
                    objLabel.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                    double dDistance = objDistance.Distance * pDocument.m_objConfig.GetCameraParameter( ( int )CDefine.enumCamera.CAMERA_1 ).dResolution;
                    objLabel.SetXYText( m_objListImage[ m_iPositionCrop ].Width / 2, 0, string.Format( "DISTANCE : {0:F2}", dDistance ) );
                    m_objCogDisplayMain.StaticGraphics.Add( objLabel, "" );
                } catch( Exception ex ) {
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "CDialogFindLineSetting - Reload : " + ex.ToString() );
                }
            } while( false );
            
        }

        private void BtnSave_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            if( true == m_bSaveAll ) {
                CConfig.CFindLineParameter objParameterTop = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ m_iPositionCrop ].Clone() as CConfig.CFindLineParameter;
                CConfig.CFindLineParameter objParameterBottom = m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ m_iPositionCrop ].Clone() as CConfig.CFindLineParameter;

                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineTop[ iLoopCount ] = objParameterTop.Clone() as CConfig.CFindLineParameter;
                    m_objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].objFindLineBottom[ iLoopCount ] = objParameterBottom.Clone() as CConfig.CFindLineParameter;
                }
            }
            //pDocument.m_objConfig.SaveRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1, m_objRecipeParameter );
            m_bSaveAll = false;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void BtnTitleCropPosition_Click( object sender, EventArgs e )
        {
            m_bSaveAll = !m_bSaveAll;
        }
    }
}
