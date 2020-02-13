using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Cognex.VisionPro;
using System.Diagnostics;
using System.Reflection;
using HLDevice;
using System.Collections.Generic;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Display;
using HLDevice.VisionLibrary;
using HLDevice.Abstract;

namespace DeepSight {
    public partial class CFormSetupVision : Form, CFormInterface {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // enum
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 라이브러리 종류
        public enum enumLibrary { PM_ALIGN_MAIN = 0, LIBRARY_FINAL };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 카메라 인덱스
        private CDefine.enumCamera m_eCameraIndex;
        // 이전선택 카메라 인덱스
        private CDefine.enumCamera m_eCameraPreviousIndex;
        // 선택된 검사 포지션
        private int m_iIndexInspectionPosition;

        // 비전 라이브러리 얼라인 객체
        private List<CVisionLibrary> m_refVisionLibrary;
        // 이미지 자르는 툴
        private List<CogAffineTransformTool> m_objCogAffineTransformTool;

        // 검사 결과 구조체
        private CInspectionResult.CResult m_objResult = new CInspectionResult.CResult();

        CConfig.CRecipeParameter[] m_objRecipeParameter;

        // 디스플레이 패널
        private Panel m_objPanelDisplayMain;
        // 폼 디스플레이 화면
        private Form m_objFormDisplayMain;
        // 자른 이미지 보기
        private Panel[] m_objPanelCropImage;
        private Form[] m_objFormDisplayCropImage;

        // 라이브러리 화면
        private Form[] m_objFormLibrary;
        // 마스터 포지션 이미지
        private CogImage8Grey m_objMasterPositionImage;
        // 현재 라이브러리
        private enumLibrary m_eLibrary;
        // 입력된 이미지
        private CogImage8Grey m_objInputCogImage;

        private CDefine.enumInspectionType m_eInspectionType;
        // 오퍼레이션 버튼 선택후 인덱스 선택시 레시피 복사
        private bool m_bCopyRecipe;
        // 조명값 한번에 셋팅하기
        private bool m_bAllSetLightValue;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormSetupVision()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormSetupVision_Load( object sender, EventArgs e )
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
        private void CFormSetupVision_FormClosed( object sender, FormClosedEventArgs e )
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
        public bool Initialize()
        {
            bool bReturn = false;

            do {
                var pDocument = CDocument.GetDocument;
                var pFormCommon = CFormCommon.GetFormCommon;
                // 변수 초기화
                m_eCameraIndex = CDefine.enumCamera.CAMERA_1;
                m_eCameraPreviousIndex = CDefine.enumCamera.CAMERA_1;
                m_iIndexInspectionPosition = 0;
                m_eInspectionType = CDefine.enumInspectionType.TYPE_VIDI;
                m_eLibrary = enumLibrary.PM_ALIGN_MAIN;
                m_bCopyRecipe = false;
                m_bAllSetLightValue = false;
                // 레시피 경로
                string strRecipePath = pDocument.m_objConfig.GetRecipePath();
                // 레시피 이름
                string strRecipeName = pDocument.m_objConfig.GetSystemParameter().strRecipeID;


                // 비전 라이브러리 얼라인 객체
                {
                    m_refVisionLibrary = new List<CVisionLibrary>();
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopCount++ ) {
                        CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 1, string.Format( "Vision Library {0} Initialize Start.", iLoopCount ), TypeOfMessage.Warning );
                        m_refVisionLibrary.Add( new CVisionLibrary( new CVisionLibraryCogPMAlign() ) );

                        CVisionLibraryAbstract.CInitializeParameter objInit = new CVisionLibraryAbstract.CInitializeParameter();
                        objInit.iIndex = iLoopCount; objInit.strRecipeName = strRecipeName; objInit.strRecipePath = strRecipePath;
                        m_refVisionLibrary[ iLoopCount ].HLInitialize( objInit );
                    }
                }

                // 이미지 자르기
                m_objCogAffineTransformTool = new List<CogAffineTransformTool>();

                m_objRecipeParameter = new CConfig.CRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_FINAL ];
                for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
                    // 					// 비전 라이브러리 클래스 초기화
                    // 					CVisionLibraryAlign obj = new CVisionLibraryAlign();
                    // 					obj.Initialize();
                    // 					// 레퍼런스 넘겨줌
                    // 					m_refVisionLibrary[ iLoopCount ] = obj;
                    m_objRecipeParameter[ iLoopCount ] = pDocument.m_objConfig.GetRecipeParameter( iLoopCount );
                }

                // 디스플레이 화면 생성
                {
                    m_objPanelDisplayMain = new Panel();
                    m_objPanelDisplayMain = this.panelMainView;
                    // 폼 디스플레이 생성
                    m_objFormDisplayMain = new Form();

                    string strDisplayName = "SETUP";

                    // 디스플레이 화면 생성 & 초기화
                    CFormDisplay objForm = new CFormDisplay();
                    objForm.Initialize( 0, string.Format( "{0}", strDisplayName ), true );
                    objForm.Visible = true;
                    objForm.SetTimer( true );
                    // 사이즈 조정
                    Panel objPanel = m_objPanelDisplayMain;
                    objForm.SetSize( objPanel.Location.X, objPanel.Location.Y, objPanel.Width, objPanel.Height, true );
                    // 패널에 화면 붙임
                    SetFormDockStyle( objForm, objPanel );
                    m_objFormDisplayMain = objForm;
                }

                // 라이브러리 폼 화면 생성
                m_objFormLibrary = new Form[ ( int )enumLibrary.LIBRARY_FINAL ];
                for( int iLoopCount = 0; iLoopCount < m_objFormLibrary.Length; iLoopCount++ ) {
                    Panel objPanel = this.panelSetting;
                    Form objForm = null;
                    switch( ( enumLibrary )iLoopCount ) {
                        case enumLibrary.PM_ALIGN_MAIN:
                            objForm = new CFormSetupVisionSettingSubPMAlign();
                            ( objForm as CFormSetupVisionSettingSubPMAlign ).Initialize( ( m_objFormDisplayMain as CFormDisplay ).GetCogDisplay(), m_objFormDisplayMain as CFormDisplay );
                            ( objForm as CFormSetupVisionSettingSubPMAlign ).EventMasterImage += new CFormSetupVisionSettingSubPMAlign.DelegateMasterImage( GetMasterPositionImage );
                            break;
                    }
                    SetFormDockStyle( objForm, objPanel );
                    m_objFormLibrary[ iLoopCount ] = objForm;
                }

                // 크롭이미지 디스플레이 화면 생성
                {
                    int iMaxCount = 6;// CDefine.DEF_MAX_COUNT_CROP_REGION * 2;
                    m_objPanelCropImage = new Panel[ iMaxCount ];
                    m_objPanelCropImage[ 0 ] = this.panelSubView1;
                    m_objPanelCropImage[ 1 ] = this.panelSubView2;
                    m_objPanelCropImage[ 2 ] = this.panelSubView3;
                    m_objPanelCropImage[ 3 ] = this.panelSubView4;
                    m_objPanelCropImage[ 4 ] = this.panelSubView5;
                    m_objPanelCropImage[ 5 ] = this.panelSubView6;

                    // 폼 디스플레이 생성
                    m_objFormDisplayCropImage = new Form[ iMaxCount ];

                    string[] strDisplayName = new string[ iMaxCount ];
                    strDisplayName[ 0 ] = "Crop 1";
                    strDisplayName[ 1 ] = "Crop 2";
                    strDisplayName[ 2 ] = "Crop 3";
                    strDisplayName[ 3 ] = "Crop 4";
                    strDisplayName[ 4 ] = "Crop 5";
                    strDisplayName[ 5 ] = "Crop 6";

                    for( int iLoopCount = 0; iLoopCount < m_objFormDisplayCropImage.Length; iLoopCount++ ) {
                        // 디스플레이 화면 생성 & 초기화
                        CFormDisplay objForm = new CFormDisplay();
                        objForm.Initialize( ( int )CDefine.enumCamera.CAMERA_1, string.Format( "{0}", strDisplayName[ iLoopCount ] ) );
                        objForm.Visible = true;
                        objForm.SetTimer( true );
                        // 사이즈 조정
                        Panel objPanel = m_objPanelCropImage[ iLoopCount ];
                        objForm.SetSize( objPanel.Location.X, objPanel.Location.Y, objPanel.Width, objPanel.Height, false, false );
                        // 패널에 화면 붙임
                        SetFormDockStyle( objForm, objPanel );
                        m_objFormDisplayCropImage[ iLoopCount ] = objForm;
                    }
                }


                // 더블 버퍼
                SetDoubleBuffered();
                // 초기화할때만 로드레시피 합시다.
                // 넘 오래걸림
                SetLoadRecipe();
                // 설비 타입에 따라 버튼 숨김 처리 필요
                SetChangeMachineTypeButton();
                // 초기화 언어 변경
                SetChangeLanguage();
                // 버튼 이벤트 로그 정의
                SetButtonEventChange();
                // 버튼 색상 정의
                SetButtonColor();

                // 현재 선택된 인덱스의 라이브러리 불러오기
                SetConnectLibraryForm();
                // 비전 라이브러리 폼 갱신
                UpdateLibraryForm();
                SetVisionLibraryForm();

                SetRegion();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

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
            // 폼 디스플레이 해제
            ( m_objFormDisplayMain as CFormDisplay ).DeInitialize();
            // 라이브러리 폼 화면 해제
            for( int iLoopCount = 0; iLoopCount < m_objFormLibrary.Length; iLoopCount++ ) {
                if( null != m_objFormLibrary[ iLoopCount ] ) {
                    switch( ( enumLibrary )iLoopCount ) {
                        case enumLibrary.PM_ALIGN_MAIN:
                            ( m_objFormLibrary[ iLoopCount ] as CFormSetupVisionSettingSubPMAlign ).DeInitialize();
                            break;
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 비전 라이브러리 폼 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetVisionLibraryForm()
        {
            switch( m_eLibrary ) {
                case enumLibrary.PM_ALIGN_MAIN:
                    SetVisionLibraryForm( enumLibrary.PM_ALIGN_MAIN, true );
                    break;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브러리 폼에 툴 이어줌
        //설명 : 링크만 연결시킨 것 ~ 각 라이브러리 폼에서는 셋업 라이브러리를 끌어다가 사용하는 것임.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetConnectLibraryForm()
        {
            var pDocument = CDocument.GetDocument;
            // 라이브러리 폼에 셋업 다이얼로그 라이브러리 이어줌
            // PM 얼라인 폼 ( 메인 )
            {
                // 비전 라이브러리 얼라인 객체

                //pDocument.m_objProcessMain.m_objVisionLibraryPMAlign.CopyTo( m_iIndexInspectionPosition, m_refVisionLibrary, 0, 1 );
                CogPMAlignTool obj = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ m_iIndexInspectionPosition ].HLGetReferenceLibrary() ).m_objPMAlignTool;
                ( m_objFormLibrary[ ( int )enumLibrary.PM_ALIGN_MAIN ] as CFormSetupVisionSettingSubPMAlign ).SetTool( obj );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브러리 폼 디스플레이 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetLibraryFormUpdateDisplay( enumLibrary eLibrary, CogImage8Grey objCogImage )
        {
            do {
                if( null == objCogImage ) break;

                switch( eLibrary ) {
                    case enumLibrary.PM_ALIGN_MAIN:
                        ( m_objFormLibrary[ ( int )eLibrary ] as CFormSetupVisionSettingSubPMAlign ).UpdateDisplay( objCogImage );
                        break;
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 더블 버퍼링
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDoubleBuffered()
        {
            this.SetStyle( ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 설비 타입에 따라 버튼 숨김 처리 필요
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetChangeMachineTypeButton()
        {
            var pDocument = CDocument.GetDocument;
             
            switch( pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                case CDefine.enumMachineType.PROCESS_60:
                    BtnTitleMeasureThreshSensor.Visible = false;
                    BtnTitleMeasureSensorSize.Visible = false;
                    BtnMeasureSensorSizeWidth.Visible = false;
                    BtnMeasureSensorSizeHeight.Visible = false;
                    BtnMeasureThreshSensor.Visible = false;
                    break;
                case CDefine.enumMachineType.PROCESS_110:
                    BtnTitleMeasureThresWidth.Visible = false;
                    BtnTitleMeasureThresHeight.Visible = false;
                    BtnMeasureThresWidth.Visible = false;
                    BtnMeasureThresHeight.Visible = false;
                    break;
                case CDefine.enumMachineType.PROCESS_150:
                    break;
            }
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
            // 배경
            this.BackColor = pFormCommon.COLOR_FORM_VIEW;
            // 버튼 Fore, Back 색상 정의
            pFormCommon.SetButtonColor( this.BtnTitleOperation, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnGrabImage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnLoadImage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnInspection, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnSelectVIDI, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSelectMeasure, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleCountInspectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnCountInspectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSelectInpectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.btnTitleRegionCount, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.btnRegionCount, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleRegionSize, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnRegionSizeWidth, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnRegionSizeHeight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitlePMSImageTypeVIDI, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnVidiStreamName1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnVidiToolName1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnVidiToolType1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleMeasure1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleLightValue, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnLightValue1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnLightValue2, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnLightValue3, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnLightValue4, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCameraExposureTime, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnCameraCropWidth, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCameraCropHeight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCameraCropValue, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleVidiScore, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnVidiScore, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleMeasureThresWidth, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleMeasureThresHeight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleMeasureThreshSensor, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleMeasureSensorSize, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

            pFormCommon.SetButtonColor( this.BtnMeasureThresWidth, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMeasureThresHeight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMeasureSensorSizeWidth, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMeasureSensorSizeHeight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMeasureThreshSensor, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleWidthMax, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleWidthMin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleHeightMax, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleHeightMin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnWidthMax, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnWidthMin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnHeightMax, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnHeightMin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );


            pFormCommon.SetButtonColor( this.BtnLoad, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSave, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼 이벤트 로그 추가
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonEventChange()
        {
            SetButtonEventChange( this.BtnGrabImage );
            SetButtonEventChange( this.BtnLoadImage );
            SetButtonEventChange( this.BtnInspection );
            SetButtonEventChange( this.btnRegionCount );
            SetButtonEventChange( this.BtnCountInspectionPosition );
            SetButtonEventChange( this.BtnPMSImageTypeA );
            SetButtonEventChange( this.BtnPMSImageTypeP );
            SetButtonEventChange( this.BtnPMSImageTypeQ );
            SetButtonEventChange( this.BtnSelectVIDI );
            SetButtonEventChange( this.BtnSelectMeasure );
            SetButtonEventChange( this.BtnTitleOperation );
            SetButtonEventChange( this.BtnCameraCropWidth );
            SetButtonEventChange( this.BtnCameraCropHeight );
            SetButtonEventChange( this.BtnLoad );
            SetButtonEventChange( this.BtnSave );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 패턴 마스터 위치 값 전송 받음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void GetMasterPositionImage( CogImage8Grey objImage )
        {
            do {
                if( null == objImage ) break;
                m_objMasterPositionImage = objImage;
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 선택된 라이브러리 폼 업데이트
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateLibraryForm()
        {
            switch( m_eLibrary ) {
                case enumLibrary.PM_ALIGN_MAIN:
                    ( m_objFormLibrary[ ( int )m_eLibrary ] as CFormSetupVisionSettingSubPMAlign ).GetPMAlign();
                    break;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 비전 라이브러리 폼 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetVisionLibraryForm( enumLibrary eLibrary, bool bUpdate )
        {
            CFormInterface obj = m_objFormLibrary[ ( int )eLibrary ] as CFormInterface;

            do {
                if( null == obj ) break;

                obj.SetVisible( bUpdate );
                obj.SetTimer( bUpdate );

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 기존 버튼 클릭 이벤트 지우고 시작 이벤트 -> 실제 클릭 이벤트 -> 종료 이벤트로 변경해줌
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonEventChange( Control obj )
        {
            // 메서드 정보를 가져옴
            MethodInfo objMethod = this.GetType().GetMethod( "get_Events", BindingFlags.NonPublic | BindingFlags.Instance );
            // 메서드 정보와 인스턴스를 통해 이벤트 핸들러 목록을 가져옴
            EventHandlerList objEventList = objMethod.Invoke( obj, new object[] { } ) as EventHandlerList;
            // 해당 버튼 클래스에 Control Type을 가져옴
            Type objControlType = GetControlType( obj.GetType() );
            // 필드 정보를 가져옴
            FieldInfo objFieldInfo = objControlType.GetField( "EventClick", BindingFlags.NonPublic | BindingFlags.Static );
            // 필드 정보에서 해당 컨트롤을 지원하는 필드값을 가져옴
            object objClickValue = objFieldInfo.GetValue( obj );
            // 등록된 델리게이트 이벤트를 가져옴
            Delegate delegateButtonClick = objEventList[ objClickValue ];
            // 기존 클릭 이벤트를 지우고 시작 이벤트 -> 실 구현 이벤트 -> 종료 이벤트로 변경해줌
            objEventList.RemoveHandler( objClickValue, delegateButtonClick );
            objEventList.AddHandler( objClickValue, new EventHandler( SetButtonStartLog ) );
            objEventList.AddHandler( objClickValue, delegateButtonClick );
            objEventList.AddHandler( objClickValue, new EventHandler( SetButtonEndLog ) );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 시작 버튼 로그
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonStartLog( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            pDocument.SetUpdateButtonLog( this, string.Format( "{0} : [ {1} ]", ( sender as Button ).Name, "TRUE" ) );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 종료 버튼 로그
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonEndLog( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            pDocument.SetUpdateButtonLog( this, string.Format( "{0} : [ {1} ]", ( sender as Button ).Name, "FALSE" ) );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : Control Type 검색
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Type GetControlType( Type objType )
        {
            Type objReturn = null;
            // Type 이름 Control 재귀 함수로 검색
            if( "Control" != objType.Name ) {
                objReturn = GetControlType( objType.BaseType );
            } else {
                objReturn = objType;
            }
            return objReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 스타일 달라붙음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetFormDockStyle( Form objForm, Panel objPanel )
        {
            objForm.Owner = this;
            objForm.TopLevel = false;
            objForm.Dock = DockStyle.Fill;
            objPanel.Controls.Add( objForm );
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
                SetControlChangeLanguage( this.BtnTitleOperation );
                SetControlChangeLanguage( this.BtnGrabImage );
                SetControlChangeLanguage( this.BtnLoadImage );
                SetControlChangeLanguage( this.BtnInspection );
                SetControlChangeLanguage( this.BtnTitleCountInspectionPosition );
                SetControlChangeLanguage( this.btnTitleRegionCount );
                SetControlChangeLanguage( this.BtnLoad );
                SetControlChangeLanguage( this.BtnSave );
                // 서브 폼 언어 변경
                for( int iLoopCount = 0; iLoopCount < m_objFormLibrary.Length; iLoopCount++ ) {
                    CFormInterface objFormInterface = m_objFormLibrary[ iLoopCount ] as CFormInterface;
                    if( null != objFormInterface ) {
                        objFormInterface.SetChangeLanguage();
                    }
                }
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
        //목적 : 타이머 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetTimer( bool bTimer )
        {
            timer.Enabled = bTimer;

            if( true == bTimer ) {
                // 레시피 불러오기
                // 잠시 막아두자
                //SetLoadRecipe();
                // 설정파일은 읽어온다
                var pDocument = CDocument.GetDocument;
                // 레시피 경로
                string strRecipePath = pDocument.m_objConfig.GetRecipePath();
                // 레시피 이름
                string strRecipeName = pDocument.m_objConfig.GetSystemParameter().strRecipeID;
                // 라이브러리 레시피 불러오기
                {
                    for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
                        m_objRecipeParameter[ iLoopCount ] = pDocument.m_objConfig.GetRecipeParameter( iLoopCount );
                    }
                }
                if( true == pDocument.m_bRecipeChange ) {
                    LoadRecipe();
                    pDocument.m_bRecipeChange = false;
                }

                // 현재 선택된 인덱스의 라이브러리 불러오기
                SetConnectLibraryForm();
                // 비전 라이브러리 폼 갱신
                SetVisionLibraryForm();
                // 선택된 라이브러리 폼 업데이트
                UpdateLibraryForm();

                SetRegion();
            } else {
                for( int iLoopCount = 0; iLoopCount < m_objFormLibrary.Length; iLoopCount++ ) {
                    ( m_objFormLibrary[ iLoopCount ] as CFormInterface ).SetTimer( bTimer );
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 레시피 불러오기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetLoadRecipe()
        {
            var pDocument = CDocument.GetDocument;
            // 프로그래스 바 있는 상태에서는 표시하지 않음.
            if( false == CLoadingScreen.IsSplashScreen() ) {
                // 레시피를 로드 중입니다…
                pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10111 ) );
            }
            Application.DoEvents();
            // 택타임
            Stopwatch objTactTime = new Stopwatch();
            objTactTime.Start();

            // 메인 리스트에서 뽑아와서 복사해준다
            // 나중에 업데이트 하면 그부분만 변경.
            //pDocument.m_objProcessMain.m_objVisionLibraryPMAlign.CopyTo( m_iIndexInspectionPosition, m_refVisionLibrary, 0, 1 );
            // 라이브러리 폼에 tool 복사 생성해서 넣어줌
            // 툴 블럭 부분... DeepCopy 하는 거랑 그냥 vpp 로드하는거랑 별 차이 없는 듯..
            //  			for( int iLoopCount = 0; iLoopCount < m_refVisionLibrary.Length; iLoopCount++ ) {
            //  				m_refVisionLibrary[ iLoopCount ].m_objVisionLibraryToolBlock.m_objToolBlock = CogSerializer.DeepCopyObject( pDocument.m_objProcessMain.m_objVisionLibraryAlign[ iLoopCount ].m_objVisionLibraryToolBlock.m_objToolBlock ) as CogToolBlock;
            //                  m_objRecipeParameter[ iLoopCount ] = pDocument.m_objConfig.GetRecipeParameter( iLoopCount );
            //  			}

            // 파일로딩으로 바꿈.
            // 레시피 경로
            string strRecipePath = pDocument.m_objConfig.GetRecipePath();
            // 레시피 이름
            string strRecipeName = pDocument.m_objConfig.GetSystemParameter().strRecipeID;

            // 라이브러리 레시피 불러오기
            {
                //  m_refVisionLibrary[ m_iIndexInspectionPosition ].HLLoadRecipe( strRecipePath, strRecipeName );
                for( int iLoopCount = 0; iLoopCount < m_refVisionLibrary.Count; iLoopCount++ ) {
                    if( null != m_refVisionLibrary[ iLoopCount ] ) {
                        m_refVisionLibrary[ iLoopCount ].HLLoadRecipe( strRecipePath, strRecipeName );
                    }
                }
            }

            // 라이브러리 레시피 불러오기
            {
                for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
                    m_objRecipeParameter[ iLoopCount ] = pDocument.m_objConfig.GetRecipeParameter( iLoopCount );
                }
            }

            objTactTime.Stop();
            Trace.WriteLine( string.Format( "Load Recipe : {0} ms", objTactTime.ElapsedMilliseconds ) );

            pDocument.GetMainFrame().ShowWaitMessage( false, "" );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : Visible 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetVisible( bool bVisible )
        {
            this.Visible = bVisible;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 선택 카메라 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayCurrentCamera()
        {
            // 			var pFormCommon = CFormCommon.GetFormCommon;
            // 			switch( m_eCameraIndex ) {
            // 				case CDefine.enumCamera.CAMERA_1:
            // 					pFormCommon.SetButtonBackColor( this.BtnCamera1, pFormCommon.COLOR_ACTIVATE );
            // 					break;
            // 			}
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer_Tick( object sender, EventArgs e )
        {
            // 카메라 인덱스
            SetDisplayCurrentCamera();
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            if( true == m_bCopyRecipe )
                pFormCommon.SetButtonBackColor( this.BtnTitleOperation, pFormCommon.COLOR_ACTIVATE );
            else
                pFormCommon.SetButtonBackColor( this.BtnTitleOperation, pFormCommon.COLOR_UNACTIVATE );

            if( true == m_bAllSetLightValue )
                pFormCommon.SetButtonBackColor( this.BtnTitleLightValue, pFormCommon.COLOR_ACTIVATE );
            else
                pFormCommon.SetButtonBackColor( this.BtnTitleLightValue, pFormCommon.COLOR_UNACTIVATE );


            pFormCommon.SetButtonText( this.BtnCountInspectionPosition, ( m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition ).ToString() );
            pFormCommon.SetButtonText( this.BtnSelectInpectionPosition, "INSPECTION POSITION " + ( m_iIndexInspectionPosition + 1 ).ToString() );
            pFormCommon.SetButtonText( this.btnRegionCount, ( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion ).ToString() );

            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                pFormCommon.SetButtonText( this.BtnRegionSizeWidth, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth.ToString() );
                pFormCommon.SetButtonText( this.BtnRegionSizeHeight, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight.ToString() );
            } else {
                pFormCommon.SetButtonText( this.BtnRegionSizeWidth, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth.ToString() );
                pFormCommon.SetButtonText( this.BtnRegionSizeHeight, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight.ToString() );
            }


            pFormCommon.SetButtonText( this.BtnVidiStreamName1, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ 0 ].objVidiParameter.strStreamName );

            pFormCommon.SetButtonText( this.BtnVidiToolName1, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ 0 ].objVidiParameter.strToolName );

            pFormCommon.SetButtonText( this.BtnVidiToolType1, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ 0 ].objVidiParameter.eVidiType.ToString() );

            if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                pFormCommon.SetButtonText( this.BtnLightValue1, string.Format( "CH 1\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ] ) );
                pFormCommon.SetButtonText( this.BtnLightValue2, string.Format( "CH 2\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ] ) );
                pFormCommon.SetButtonText( this.BtnLightValue3, string.Format( "CH 3\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ] ) );
                pFormCommon.SetButtonText( this.BtnLightValue4, string.Format( "CH 4\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ] ) );
            } else {
                if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                    pFormCommon.SetButtonText( this.BtnLightValue1, string.Format( "CH 1\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ] ) );
                    pFormCommon.SetButtonText( this.BtnLightValue2, string.Format( "CH 2\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ] ) );
                    pFormCommon.SetButtonText( this.BtnLightValue3, string.Format( "CH 3\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ] ) );
                    pFormCommon.SetButtonText( this.BtnLightValue4, string.Format( "CH 4\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ] ) );
                } else {
                    pFormCommon.SetButtonText( this.BtnLightValue1, string.Format( "CH 1\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 0 ] ) );
                    pFormCommon.SetButtonText( this.BtnLightValue2, string.Format( "CH 2\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 1 ] ) );
                    pFormCommon.SetButtonText( this.BtnLightValue3, string.Format( "CH 3\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 2 ] ) );
                    pFormCommon.SetButtonText( this.BtnLightValue4, string.Format( "CH 4\r\n{0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 3 ] ) );
                }
            }

            pFormCommon.SetButtonText( this.BtnCameraExposureTime, string.Format( "EXPOSURE TIME : {0:F1}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dCameraExpouseTime ) );

            pFormCommon.SetButtonText( this.BtnVidiScore, string.Format( "{0:F2}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiScore ) );

            switch( m_eInspectionType ) {
                case CDefine.enumInspectionType.TYPE_VIDI:
                    pFormCommon.SetButtonBackColor( this.BtnSelectVIDI, pFormCommon.COLOR_ACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnSelectMeasure, pFormCommon.COLOR_UNACTIVATE );
                    break;
                case CDefine.enumInspectionType.TYPE_MEASURE:
                    pFormCommon.SetButtonBackColor( this.BtnSelectVIDI, pFormCommon.COLOR_UNACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnSelectMeasure, pFormCommon.COLOR_ACTIVATE );
                    break;
                default:
                    break;
            }

            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                switch( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeVIDI ) {
                    case CDefine.enumPMSImageType.PMS_TYPE_ALBEDO:
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_ACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_UNACTIVATE );
                        break;
                    case CDefine.enumPMSImageType.PMS_TYPE_P:
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_ACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_UNACTIVATE );
                        break;
                    case CDefine.enumPMSImageType.PMS_TYPE_Q:
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_ACTIVATE );
                        break;
                    default:
                        break;
                }
            } else {
                switch( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeMeasure ) {
                    case CDefine.enumPMSImageType.PMS_TYPE_ALBEDO:
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_ACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_UNACTIVATE );
                        break;
                    case CDefine.enumPMSImageType.PMS_TYPE_P:
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_ACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_UNACTIVATE );
                        break;
                    case CDefine.enumPMSImageType.PMS_TYPE_Q:
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeA, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeP, pFormCommon.COLOR_UNACTIVATE );
                        pFormCommon.SetButtonBackColor( this.BtnPMSImageTypeQ, pFormCommon.COLOR_ACTIVATE );
                        break;
                    default:
                        break;
                }
            }

            switch( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType ) {
                case CDefine.enumCameraCropType.CROP_NONE:
                    pFormCommon.SetButtonBackColor( this.BtnCameraCropWidth, pFormCommon.COLOR_UNACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnCameraCropHeight, pFormCommon.COLOR_UNACTIVATE );
                    break;
                case CDefine.enumCameraCropType.CROP_WIDTH:
                    pFormCommon.SetButtonBackColor( this.BtnCameraCropWidth, pFormCommon.COLOR_ACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnCameraCropHeight, pFormCommon.COLOR_UNACTIVATE );
                    break;
                case CDefine.enumCameraCropType.CROP_HEIGHT:
                    pFormCommon.SetButtonBackColor( this.BtnCameraCropWidth, pFormCommon.COLOR_UNACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnCameraCropHeight, pFormCommon.COLOR_ACTIVATE );
                    break;
                default:
                    break;
            }

            pFormCommon.SetButtonText( this.BtnCameraCropValue, string.Format( "{0}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.iCropSize ) );

            pFormCommon.SetButtonText( this.BtnMeasureThresWidth, string.Format( "{0}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarLRLowThresh ) );
            pFormCommon.SetButtonText( this.BtnMeasureThresHeight, string.Format( "{0}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarTBThresh ) );
            pFormCommon.SetButtonText( this.BtnMeasureSensorSizeWidth, string.Format( "{0}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdWid ) );
            pFormCommon.SetButtonText( this.BtnMeasureSensorSizeHeight, string.Format( "{0}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdHgt ) );
            pFormCommon.SetButtonText( this.BtnMeasureThreshSensor, string.Format( "{0}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorThresh ) );

            pFormCommon.SetButtonText( this.BtnWidthMax, string.Format( "{0:F3}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMax ) );
            pFormCommon.SetButtonText( this.BtnWidthMin, string.Format( "{0:F3}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMin ) );
            pFormCommon.SetButtonText( this.BtnHeightMax, string.Format( "{0:F3}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMax ) );
            pFormCommon.SetButtonText( this.BtnHeightMin, string.Format( "{0:F3}", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMin ) );

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 카메라 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ChangeCameraIndex()
        {
            // 레시피 변경 시기는 해당 폼을 빠져나갔다가 다시 왔을 때랑 불러오기 했을 때로 제한함.
            // 카메라 인덱스 변경해도 이전 카메라에서 변경한 부분은 남겨놓음.
            // 레시피 불러오기
            //SetLoadRecipe();
            // 서브젝트
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 카메라 1 선택
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCamera1_Click( object sender, EventArgs e )
        {
            do {
                if( CDefine.enumCamera.CAMERA_1 == m_eCameraIndex ) break;
                m_eCameraPreviousIndex = m_eCameraIndex;
                m_eCameraIndex = CDefine.enumCamera.CAMERA_1;
                // 카메라 변경
                ChangeCameraIndex();

            } while( false );
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그랩 이미지 ( 도큐먼트에 검사 결과 이미지를 들고 옴 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnGrabImage_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 추후에는 PMS이미지로 교체합시다
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType )
                m_objInputCogImage = pDocument.GetInspectionResultAlign( m_iIndexInspectionPosition ).objResultCommon.objPMSImage[ ( int )m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeVIDI ];
            else
                m_objInputCogImage = pDocument.GetInspectionResultAlign( m_iIndexInspectionPosition ).objResultCommon.objPMSImage[ ( int )m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeMeasure ];

            ClearMainDisplayGraphics();
            do {
                if( null == m_objInputCogImage ) break;
                HideSerchRegion();
                // 입력 이미지 넣어줌
                CFormDisplay obj = m_objFormDisplayMain as CFormDisplay;
                obj.UpdateDisplay( m_objInputCogImage );
                SetLibraryFormUpdateDisplay( enumLibrary.PM_ALIGN_MAIN, m_objInputCogImage );
                try {
                } catch( Exception ex ) {
                    string strErrorLog = "CFormSetupVision- BtnGrabImage_Click : " + ex.Message + " ->" + ex.StackTrace;
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0 + ( int )m_eCameraIndex ), strErrorLog );
                    break;
                }


            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 불러오기 ( 파일에서 불러옴 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLoadImage_Click( object sender, EventArgs e )
        {
            OpenFileDialog objOpenFileDialog = new OpenFileDialog();
            objOpenFileDialog.Filter = "Image (*.jpg, *.bmp, *.TIF ) | *jpg; *.bmp; *.TIF;";
            DialogResult bDialogResult = objOpenFileDialog.ShowDialog();
            ClearMainDisplayGraphics();
            if( System.Windows.Forms.DialogResult.OK == bDialogResult ) {
                var pDocument = CDocument.GetDocument;

                do {
                    try {
                        Bitmap objBitmap = new Bitmap( objOpenFileDialog.FileName );
                        m_objInputCogImage = new CogImage8Grey( objBitmap );
                        SetLibraryFormUpdateDisplay( enumLibrary.PM_ALIGN_MAIN, m_objInputCogImage );
                    } catch( Exception ex ) {
                        pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Message : " + ex.Message + "\nStack : " + ex.StackTrace );
                        break;
                    }

                } while( false );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : vpp 불러오기 ( 파일에서 불러옴 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLoadVpp_Click( object sender, EventArgs e )
        {
            OpenFileDialog objOpenFileDialog = new OpenFileDialog();
            objOpenFileDialog.Filter = "vpp ( *.vpp ) | *.vpp;";
            DialogResult bDialogResult = objOpenFileDialog.ShowDialog();
            var pDocument = CDocument.GetDocument;

            if( System.Windows.Forms.DialogResult.OK == bDialogResult ) {
                do {
                    try {
                        if( false == CLoadingScreen.IsSplashScreen() ) {
                            // 레시피를 로드 중입니다…
                            pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10111 ) );
                        }

                        pDocument.GetMainFrame().ShowWaitMessage( false, "" );
                    } catch( Exception ) {
                        break;
                    }

                } while( false );
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 툴 실행
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnInspection_Click( object sender, EventArgs e )
        {
            InspectionTest();
            //'  InspectionTest();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool InspectionTest()
        {
            bool bresult = false;
            do {
                // 일단 패턴의 그래픽을 지워주고
                ClearMainDisplayGraphics();
                // 혹시나 변경했을 영역을 저장해주고
                SetDataRegion();

                // 패턴에 이미지 넣고 자르고 기타 등등
                CFormDisplay objMain = m_objFormDisplayMain as CFormDisplay;
                if( null == m_objInputCogImage ) break;
                CogImage8Grey objImage = m_objInputCogImage;
                HLDevice.Abstract.CVisionLibraryAbstract.CResultData objResultData;
                // 이상하게 최초한번은 안됨
                m_refVisionLibrary[ m_iIndexInspectionPosition ].HLRun( m_objInputCogImage, out objResultData );
                if( null == objResultData.objCogImage )
                    m_refVisionLibrary[ m_iIndexInspectionPosition ].HLRun( m_objInputCogImage, out objResultData );
                objImage = objResultData.objCogImage;
                objMain.GetCogDisplay().Image = objImage;

                if( 0 >= objResultData.dScore[ 0 ] ) {
                    HideSerchRegion();
                    break;
                }

                if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                    for( int iLoopCount = 0; iLoopCount < m_objCogAffineTransformTool.Count; iLoopCount++ ) {
                        m_objCogAffineTransformTool[ iLoopCount ].InputImage = objImage;
                        m_objCogAffineTransformTool[ iLoopCount ].Run();

                        CFormDisplay obj = m_objFormDisplayCropImage[ iLoopCount ] as CFormDisplay;
                        obj.GetCogDisplay().Image = m_objCogAffineTransformTool[ iLoopCount ].OutputImage;
                    }
                } else {
                    for( int iLoopCount = 0; iLoopCount < m_objCogAffineTransformTool.Count; iLoopCount++ ) {
                        m_objCogAffineTransformTool[ iLoopCount ].InputImage = objImage;
                        m_objCogAffineTransformTool[ iLoopCount ].Run();

                        CFormDisplay obj = m_objFormDisplayCropImage[ iLoopCount + ( CDefine.DEF_MAX_COUNT_CROP_REGION - 3 ) ] as CFormDisplay;
                        obj.GetCogDisplay().Image = m_objCogAffineTransformTool[ iLoopCount ].OutputImage;
                    }
                }


                SetRegion();
                objMain.SetGraphicsPatternResult( ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ m_iIndexInspectionPosition ].HLGetReferenceLibrary() ).m_objPMAlignTool.CreateLastRunRecord() );
                bresult = true;
            } while( false );
            return bresult;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 패턴 이미지를 가져옴
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool RunPMAlign( out CogImage8Grey objOutImage )
        {
            bool bReturn = false;
            objOutImage = new CogImage8Grey();
            do {
                CogPMAlignTool obj = ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ m_iIndexInspectionPosition ].HLGetReferenceLibrary() ).m_objPMAlignTool;

                //                 obj.Run();
                //                 CogImage8Grey FixtureImage = obj.InputImage.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey;
                //                 FixtureImage.SelectedSpaceName = "@";
                // 
                //                 m_objFixtureTool.InputImage = FixtureImage;
                //                 m_objFixtureTool.RunParams.UnfixturedFromFixturedTransform = m_objPMAlignTool.Results[ 0 ].GetPose();
                //                 m_objFixtureTool.Run();
                //                 objOutImage = m_objFixtureTool.OutputImage as CogImage8Grey;
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 불러오기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLoad_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 레시피 불러오기
            SetLoadRecipe();

            // 현재 선택된 인덱스의 라이브러리 불러오기
            SetConnectLibraryForm();

            // 비전 라이브러리 폼 갱신
            UpdateLibraryForm();
            SetVisionLibraryForm();


            SetRegion();
            // 불러오기가 완료되었습니다.
            pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10015 );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void LoadRecipe()
        {
            // 레시피 불러오기
            SetLoadRecipe();

            // 현재 선택된 인덱스의 라이브러리 불러오기
            SetConnectLibraryForm();

            // 비전 라이브러리 폼 갱신
            UpdateLibraryForm();
            SetVisionLibraryForm();


            SetRegion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 저장
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnSave_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            SetDataRegion();

            // 프로그래스 바 있는 상태에서는 표시하지 않음.
            if( false == CLoadingScreen.IsSplashScreen() ) {
                // 레시피를 로드 중입니다…
                pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10114 ) );
            }
            Application.DoEvents();
            //ChangeCameraIndex();

            // 레시피 경로
            string strRecipePath = pDocument.m_objConfig.GetRecipePath();
            // 레시피 이름
            string strRecipeName = pDocument.m_objConfig.GetSystemParameter().strRecipeID;

            m_refVisionLibrary[ m_iIndexInspectionPosition ].HLSaveRecipe( strRecipePath, strRecipeName );


            // 라이트값 한번에 저장하기
            if( true == m_bAllSetLightValue ) {
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopCount++ ) {
                    m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iLoopCount ].objGrabParameter.dLightValue[ 0 ] = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ];
                    m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iLoopCount ].objGrabParameter.dLightValue[ 1 ] = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ];
                    m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iLoopCount ].objGrabParameter.dLightValue[ 2 ] = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ];
                    m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iLoopCount ].objGrabParameter.dLightValue[ 3 ] = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ];


                    m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iLoopCount ].objGrabParameter.eCropType = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType;
                    m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iLoopCount ].objGrabParameter.iCropSize = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.iCropSize;
                }
            }
            m_bAllSetLightValue = false;
            //SetDataRegion();
            for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter.Length; iLoopCount++ ) {
                pDocument.m_objConfig.SaveRecipeParameter( iLoopCount, m_objRecipeParameter[ iLoopCount ] );
            }
            // 메인 프로세스 라이브러리 저장된 vpp 로 갱신
            pDocument.m_objProcessMain.LoadRecipe();


            InspectionTest();

            // 저장이 완료되었습니다.
            pDocument.GetMainFrame().ShowWaitMessage( false, "" );
            pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10014 );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void btnRegionCount_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            CInspectionResult.CResult objResult = new CInspectionResult.CResult();

            string strLog = string.Format( "[{0}] [{1}]", "btnRegionCount_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            string[] strButtonList;
            if( CDefine.enumMachineType.PROCESS_110 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                strButtonList = new string[ ( CDefine.DEF_MAX_COUNT_CROP_REGION - 4 ) ];
            } else {
                strButtonList = new string[ ( CDefine.DEF_MAX_COUNT_CROP_REGION - 3 ) ];
            }
             
            for( int iLoopCount = 0; iLoopCount < strButtonList.Length; iLoopCount++ ) {
                strButtonList[ iLoopCount ] = "CROP COUNT " + ( iLoopCount + 1 ).ToString();
            }

            CDialogEnumerate objDialog = new CDialogEnumerate( strButtonList.Length, strButtonList, m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion - 1 );
            if( DialogResult.OK == objDialog.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion = objDialog.GetResult() + 1;
                SetRegion();
            }

            strLog = string.Format( "[{0}] [iRegionCount : {1}] [{2}]", "btnRegionCount_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion, false );
            // 버튼 로그 추가
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetRegion()
        {
            var pDocument = CDocument.GetDocument;

            m_objCogAffineTransformTool.Clear();
            CConfig.CSerchRegion[] objRegion = new CConfig.CSerchRegion[ m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion ];
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                    // 고정사이즈로 가야하기때문에 사이즈는 여기서 픽스로 설정
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight;

                    objRegion[ iLoopCount ] = new CConfig.CSerchRegion();
                    objRegion[ iLoopCount ] = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].Clone() as CConfig.CSerchRegion;

                    double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                    dStartX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dStartX;
                    dStartY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dStartY;
                    dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndX;
                    dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndY;
                    dRotation = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dRotation;
                    dSkew = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dSkew;
                    // 이미지 자르는 툴을 여기서 설정하고 후다닥 씁시다
                    CogAffineTransformTool objTool = new CogAffineTransformTool();
                    objTool.Region.SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                    m_objCogAffineTransformTool.Add( objTool );
                }
            } else {
                for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                    // 고정사이즈로 가야하기때문에 사이즈는 여기서 픽스로 설정
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight;

                    objRegion[ iLoopCount ] = new CConfig.CSerchRegion();
                    objRegion[ iLoopCount ] = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].Clone() as CConfig.CSerchRegion;

                    double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                    dStartX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dStartX;
                    dStartY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dStartY;
                    dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndX;
                    dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndY;
                    dRotation = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dRotation;
                    dSkew = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dSkew;
                    // 이미지 자르는 툴을 여기서 설정하고 후다닥 씁시다
                    CogAffineTransformTool objTool = new CogAffineTransformTool();
                    objTool.Region.SetOriginLengthsRotationSkew( dStartX, dStartY, dEndX, dEndY, dRotation, dSkew );
                    m_objCogAffineTransformTool.Add( objTool );
                }
            }


            CFormDisplay obj = m_objFormDisplayMain as CFormDisplay;
            obj.SetSearchRegion( objRegion );
            // 티칭한 메인디스플레이를 삭제한다
            ClearMainDisplayGraphics();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDataRegion()
        {
            var pDocument = CDocument.GetDocument;
            CFormDisplay obj = m_objFormDisplayMain as CFormDisplay;
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                    CogRectangleAffine objRectangle = obj.GetRectangleAffine( iLoopCount );
                    double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                    objRectangle.GetOriginLengthsRotationSkew( out dStartX, out dStartY, out dEndX, out dEndY, out dRotation, out dSkew );

                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dStartX = dStartX;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dStartY = dStartY;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndX = dEndX;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndY = dEndY;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dRotation = dRotation;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dSkew = dSkew;
                }
            } else {
                for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                    CogRectangleAffine objRectangle = obj.GetRectangleAffine( iLoopCount );
                    double dStartX, dStartY, dEndX, dEndY, dRotation, dSkew;
                    objRectangle.GetOriginLengthsRotationSkew( out dStartX, out dStartY, out dEndX, out dEndY, out dRotation, out dSkew );

                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dStartX = dStartX;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dStartY = dStartY;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndX = dEndX;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndY = dEndY;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dRotation = dRotation;
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dSkew = dSkew;
                }
            }

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCountInspectionPosition_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", "BtnCountInspectionPosition_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            string[] strButtonList = new string[ CDefine.DEF_MAX_COUNT_INSPECTION_POSITION ];
            for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_INSPECTION_POSITION; iLoopCount++ ) {
                strButtonList[ iLoopCount ] = "INSPECTION POSITION COUNT " + ( iLoopCount + 1 ).ToString();
            }
            CDialogEnumerate objDialog = new CDialogEnumerate( CDefine.DEF_MAX_COUNT_INSPECTION_POSITION, strButtonList, m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition - 1 );
            if( DialogResult.OK == objDialog.ShowDialog() )
                m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition = objDialog.GetResult() + 1;

            strLog = string.Format( "[{0}] [iCountInspectionPosition : {1}] [{2}]", "BtnCountInspectionPosition_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition + 1, false );
            pDocument.SetUpdateButtonLog( this, strLog );

            SetRegion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool CopyRecipe( int iCurrentIndex, int iSelectIndex )
        {
            bool bReturn = false;
            do {
                m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iSelectIndex ] = m_objRecipeParameter[ ( int )CDefine.enumCamera.CAMERA_1 ].objInspectionParameter[ iCurrentIndex ].Clone() as CConfig.CInspectionParameter;
                CogPMAlignTool obj = new CogPMAlignTool( ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iCurrentIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool );
                ( ( HLDevice.VisionLibrary.CVisionLibraryCogPMAlign )m_refVisionLibrary[ iSelectIndex ].HLGetReferenceLibrary() ).m_objPMAlignTool = new CogPMAlignTool( obj );


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
        private void BtnSelectInpectionPosition_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", "BtnSelectInpectionPosition_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            // 혹시 티칭영역을 변경했을수도 있으니 데이터를 삽입
            SetDataRegion();

            string[] strButtonList = new string[ m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition ];
            for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition; iLoopCount++ ) {
                strButtonList[ iLoopCount ] = "INSPECTION POSITION " + ( iLoopCount + 1 ).ToString();
            }
            CDialogEnumerate objDialog = new CDialogEnumerate( m_objRecipeParameter[ ( int )m_eCameraIndex ].iCountInspectionPosition, strButtonList, m_iIndexInspectionPosition );
            if( DialogResult.OK == objDialog.ShowDialog() )

                if( true == m_bCopyRecipe ) {
                    CopyRecipe( m_iIndexInspectionPosition, objDialog.GetResult() );
                    m_bCopyRecipe = false;
                }
            m_iIndexInspectionPosition = objDialog.GetResult();


            // 현재 선택된 인덱스의 라이브러리 불러오기
            SetConnectLibraryForm();
            UpdateLibraryForm();

            SetRegion();


            strLog = string.Format( "[{0}] [INSPECTION POSITION : {1}] [{2}]", "BtnSelectInpectionPosition_Click" + ( ( int )m_eCameraIndex ).ToString(), m_iIndexInspectionPosition + 1, false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnTitleRegionSize_Click( object sender, EventArgs e )
        {
            // 혹시 티칭영역을 변경했을수도 있으니 데이터를 삽입
            SetDataRegion();

            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                double dRotation = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ 0 ].dRotation;
                for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                    if( 0 == dRotation )
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dRotation = 90 * ( Math.PI / 180 );
                    else
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dRotation = 0;
                }
            } else {
                double dRotation = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ 0 ].dRotation;
                for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                    if( 0 == dRotation ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dRotation = 90 * ( Math.PI / 180 );
                    } else {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dRotation = 0;
                    }
                }
            }
            SetRegion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnRegionSizeWidth_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", "BtnRegionSizeWidth_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            // 혹시 티칭영역을 변경했을수도 있으니 데이터를 삽입
            SetDataRegion();
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth );
                if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                    if( 0 >= objKeyPad.m_dValue ) {
                        objKeyPad.m_dValue = 1;
                    }
                    for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth = objKeyPad.m_dValue;
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth;
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight;
                    }
                }

                SetRegion();

                strLog = string.Format( "[{0}] [dVidiFixedSizeWidth : {1}] [{2}]", "BtnRegionSizeWidth_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth.ToString(), false );
                pDocument.SetUpdateButtonLog( this, strLog );
            } else {
                FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth );
                if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                    if( 0 >= objKeyPad.m_dValue ) {
                        objKeyPad.m_dValue = 1;
                    }
                    for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth = objKeyPad.m_dValue;
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth;
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight;
                    }
                }

                SetRegion();

                strLog = string.Format( "[{0}] [dMeasureFixedSizeWidth : {1}] [{2}]", "BtnRegionSizeWidth_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth.ToString(), false );
                pDocument.SetUpdateButtonLog( this, strLog );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnRegionSizeHeight_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", "BtnRegionSizeHeight_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            // 혹시 티칭영역을 변경했을수도 있으니 데이터를 삽입
            SetDataRegion();
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight );
                if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                    if( 0 >= objKeyPad.m_dValue ) {
                        objKeyPad.m_dValue = 1;
                    }
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight = objKeyPad.m_dValue;
                    for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeWidth;
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight;
                    }
                }
                SetRegion();

                strLog = string.Format( "[{0}] [dVidiFixedSizeHeight : {1}] [{2}]", "BtnRegionSizeHeight_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiFixedSizeHeight.ToString(), false );
                pDocument.SetUpdateButtonLog( this, strLog );
            } else {
                FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight );
                if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                    if( 0 >= objKeyPad.m_dValue ) {
                        objKeyPad.m_dValue = 1;
                    }
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight = objKeyPad.m_dValue;
                    for( int iLoopCount = 0; iLoopCount < m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].iCountSerchRegion; iLoopCount++ ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndX = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeWidth;
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureSerchRegion[ iLoopCount ].dEndY = m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight;
                    }
                }
                SetRegion();

                strLog = string.Format( "[{0}] [dMeasureFixedSizeHeight : {1}] [{2}]", "BtnRegionSizeHeight_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dMeasureFixedSizeHeight.ToString(), false );
                pDocument.SetUpdateButtonLog( this, strLog );
            }

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ClearMainDisplayGraphics()
        {
            CFormDisplay obj = m_objFormDisplayMain as CFormDisplay;
            if( -1 != obj.GetCogDisplay().InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) ) {
                obj.GetCogDisplay().InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void HideSerchRegion()
        {
            CFormDisplay obj = m_objFormDisplayMain as CFormDisplay;
            obj.SetHideSearchRegion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnPMSImageTypeA_Click( object sender, EventArgs e )
        {
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType )
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeVIDI = CDefine.enumPMSImageType.PMS_TYPE_ALBEDO;
            else
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeMeasure = CDefine.enumPMSImageType.PMS_TYPE_ALBEDO;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnPMSImageTypeP_Click( object sender, EventArgs e )
        {
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType )
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeVIDI = CDefine.enumPMSImageType.PMS_TYPE_P;
            else
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeMeasure = CDefine.enumPMSImageType.PMS_TYPE_P;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnPMSImageTypeQ_Click( object sender, EventArgs e )
        {
            if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType )
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeVIDI = CDefine.enumPMSImageType.PMS_TYPE_Q;
            else
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].ePMSImageTypeMeasure = CDefine.enumPMSImageType.PMS_TYPE_Q;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼이벤트
        //설명 : 스트림 이름 저장
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetStreamName( object sender, EventArgs e )
        {
            if( CDefine.enumInspectionType.TYPE_MEASURE == m_eInspectionType ) return;
            Button BtnSelected = ( Button )sender;
            string strValue = BtnSelected.Name;

            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", strValue + "_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            try {
                int iIndex = Int32.Parse( strValue.Substring( strValue.Length - 1, 1 ) ) - 1;
                FormKeyBoard objKey = new FormKeyBoard( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.strStreamName );
                if( DialogResult.OK == objKey.ShowDialog() ) {
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.strStreamName = objKey.m_strReturnValue;
                    }
                    //m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.strStreamName = objKey.m_strReturnValue;
                }
                strLog = string.Format( "[{0}] [SetStreamName : {1}] [{2}]", strValue + "_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.strStreamName, false );
                pDocument.SetUpdateButtonLog( this, strLog );
            } catch( Exception ex ) {
                CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "CFormSetupVision - SetStreamName : " + ex.ToString(), false );
            }

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼이벤트
        //설명 :툴 이름 저장
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetSToolName( object sender, EventArgs e )
        {
            if( CDefine.enumInspectionType.TYPE_MEASURE == m_eInspectionType ) return;
            Button BtnSelected = ( Button )sender;
            string strValue = BtnSelected.Name;
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", strValue + "_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            try {
                int iIndex = Int32.Parse( strValue.Substring( strValue.Length - 1, 1 ) ) - 1;
                FormKeyBoard objKey = new FormKeyBoard( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.strToolName );
                if( DialogResult.OK == objKey.ShowDialog() ) {
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ )
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.strToolName = objKey.m_strReturnValue;
                    //m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.strToolName = objKey.m_strReturnValue;
                }
                strLog = string.Format( "[{0}] [SetSToolName : {1}] [{2}]", strValue + "_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.strToolName, false );
                pDocument.SetUpdateButtonLog( this, strLog );
            } catch( Exception ex ) {
                CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "CFormSetupVision - SetSToolName : " + ex.ToString(), false );
            }

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼이벤트
        //설명 : 툴 타입
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetSToolType( object sender, EventArgs e )
        {
            if( CDefine.enumInspectionType.TYPE_MEASURE == m_eInspectionType ) return;
            Button BtnSelected = ( Button )sender;
            string strValue = BtnSelected.Name;
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", strValue + "_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            try {
                int iIndex = Int32.Parse( strValue.Substring( strValue.Length - 1, 1 ) ) - 1;

                string[] strButtonList = new string[ ( int )CDefine.enumVidiType.TYPE_FINAL ];
                for( int iLoopCount = 0; iLoopCount < strButtonList.Length; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = ( ( CDefine.enumVidiType )iLoopCount ).ToString();
                }
                CDialogEnumerate objDialog = new CDialogEnumerate( strButtonList.Length, strButtonList, ( int )m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.eVidiType );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ )
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iLoopCount ].objVidiParameter.eVidiType = ( CDefine.enumVidiType )objDialog.GetResult();
                    //m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.eVidiType = ( CDefine.enumVidiType )objDialog.GetResult();
                }
                strLog = string.Format( "[{0}] [SetSToolType : {1}] [{2}]", strValue + "_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objVidiSerchRegion[ iIndex ].objVidiParameter.eVidiType.ToString(), false );
                pDocument.SetUpdateButtonLog( this, strLog );
            } catch( Exception ex ) {
                CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "CFormSetupVision - SetSToolType : " + ex.ToString(), false );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼이벤트
        //설명 : 조명설정
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLightValue1_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnLightValue1_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ] );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ] = objKeyPad.m_dValue;
                } else {
                    if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ] = objKeyPad.m_dValue;
                    } else {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 0 ] = objKeyPad.m_dValue;
                    }
                }
            }

            strLog = string.Format( "[{0}] [LightValue : {1}] [{2}]", "BtnLightValue1_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 0 ].ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLightValue2_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnLightValue2_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ] );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ] = objKeyPad.m_dValue;
                } else {
                    if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ] = objKeyPad.m_dValue;
                    } else {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 1 ] = objKeyPad.m_dValue;
                    }
                }
            }

            strLog = string.Format( "[{0}] [LightValue : {1}] [{2}]", "BtnLightValue2_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 1 ].ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLightValue3_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnLightValue3_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ] );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ] = objKeyPad.m_dValue;
                } else {
                    if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ] = objKeyPad.m_dValue;
                    } else {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 2 ] = objKeyPad.m_dValue;
                    }
                }
            }

            strLog = string.Format( "[{0}] [LightValue : {1}] [{2}]", "BtnLightValue3_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 2 ].ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLightValue4_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnLightValue4_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ] );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                    m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ] = objKeyPad.m_dValue;
                } else {
                    if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ] = objKeyPad.m_dValue;
                    } else {
                        m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameterMeasure.dLightValue[ 3 ] = objKeyPad.m_dValue;
                    }
                }
            }

            strLog = string.Format( "[{0}] [LightValue : {1}] [{2}]", "BtnLightValue4_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dLightValue[ 3 ].ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCameraExposureTime_Click( object sender, EventArgs e )
        {
            do {
                break;
//                 var pDocument = CDocument.GetDocument;
//                 string strLog = string.Format( "[{0}] [{1}]", "BtnCameraExposureTime_Click", true );
//                 pDocument.SetUpdateButtonLog( this, strLog );
//                 FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dCameraExpouseTime );
//                 if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
//                     m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dCameraExpouseTime = objKeyPad.m_dValue;
//                 }
// 
//                 strLog = string.Format( "[{0}] [CameraExposureTime : {1}] [{2}]", "BtnCameraExposureTime_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.dCameraExpouseTime.ToString(), false );
//                 pDocument.SetUpdateButtonLog( this, strLog );
            } while( false );
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnSelectVIDI_Click( object sender, EventArgs e )
        {
            SetDataRegion();
            m_eInspectionType = CDefine.enumInspectionType.TYPE_VIDI;
            SetRegion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnSelectMeasure_Click( object sender, EventArgs e )
        {
            SetDataRegion();
            m_eInspectionType = CDefine.enumInspectionType.TYPE_MEASURE;
            SetRegion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnVidiScore_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strLog = string.Format( "[{0}] [{1}]", "BtnVidiScore_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiScore );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiScore = objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [VIDI Score : {1}] [{2}]", "BtnVidiScore_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiScore.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnTitleOperation_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            CUserInformation objUserInformation = pDocument.GetUserInformation();
            do {
                // 마스터만 가능하도록
                if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER != objUserInformation.m_eAuthorityLevel )
                    break;

                m_bCopyRecipe = !m_bCopyRecipe;
            } while( false );

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnTitleLightValue_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                break;
//                 CUserInformation objUserInformation = pDocument.GetUserInformation();
//                 // 마스터만 가능하도록
//                 if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER != objUserInformation.m_eAuthorityLevel )
//                     break;
// 
//                 m_bAllSetLightValue = !m_bAllSetLightValue;
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCameraCropWidth_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            if( CDefine.enumCameraCropType.CROP_WIDTH == m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType )
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType = CDefine.enumCameraCropType.CROP_NONE;
            else
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType = CDefine.enumCameraCropType.CROP_WIDTH;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCameraCropHeight_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            if( CDefine.enumCameraCropType.CROP_HEIGHT == m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType )
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType = CDefine.enumCameraCropType.CROP_NONE; 
            else
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.eCropType = CDefine.enumCameraCropType.CROP_HEIGHT;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCameraCropValue_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnCameraCropValue_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].dVidiScore );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.iCropSize = ( int )objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [CROP SIZE : {1}] [{2}]", "BtnCameraCropValue_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objGrabParameter.iCropSize.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMeasureThresWidth_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnMeasureThresWidth_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarLRLowThresh );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarLRLowThresh = ( int )objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [CROP SIZE : {1}] [{2}]", "BtnMeasureThresWidth_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarLRLowThresh.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMeasureThresHeight_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnMeasureThresHeight_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarTBThresh );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarTBThresh = ( int )objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [CROP SIZE : {1}] [{2}]", "BtnMeasureThresHeight_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iBusbarTBThresh.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMeasureSensorSizeWidth_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnMeasureSensorSizeWidth_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdWid );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdWid = ( int )objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [CROP SIZE : {1}] [{2}]", "BtnMeasureSensorSizeWidth_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdWid.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMeasureSensorSizeHeight_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnMeasureSensorSizeWidth_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdHgt );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdHgt = ( int )objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [CROP SIZE : {1}] [{2}]", "BtnMeasureSensorSizeWidth_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorStdHgt.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMeasureThreshSensor_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            string strLog = string.Format( "[{0}] [{1}]", "BtnMeasureThreshSensor_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorThresh );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorThresh = ( int )objKeyPad.m_dValue;
            }

            strLog = string.Format( "[{0}] [CROP SIZE : {1}] [{2}]", "BtnMeasureThreshSensor_Click" + ( ( int )m_eCameraIndex ).ToString(), m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.iSensorThresh.ToString(), false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnWidthMax_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnWidthMax_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMax );
            if( DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMax = objKeyPad.m_dResultValue;
                strLog = string.Format( "[{0}] [dSizeWidthMax : {1:F3}] [{2}]", "BtnWidthMax_Click", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMax, false );
            }

            // 버튼 로그 추가
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnWidthMin_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnWidthMin_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMin );
            if( DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMin = objKeyPad.m_dResultValue;
                strLog = string.Format( "[{0}] [dSizeWidthMin : {1:F3}] [{2}]", "BtnWidthMin_Click", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeWidthMin, false );
            }

            // 버튼 로그 추가
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnHeightMax_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnHeightMax_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMax );
            if( DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMax = objKeyPad.m_dResultValue;
                strLog = string.Format( "[{0}] [dSizeHeightMax : {1:F3}] [{2}]", "BtnHeightMax_Click", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMax, false );
            }

            // 버튼 로그 추가
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnHeightMin_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnHeightMin_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMin );
            if( DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMin = objKeyPad.m_dResultValue;
                strLog = string.Format( "[{0}] [dSizeHeightMin : {1:F3}] [{2}]", "BtnHeightMin_Click", m_objRecipeParameter[ ( int )m_eCameraIndex ].objInspectionParameter[ m_iIndexInspectionPosition ].objMeasureParameter.dSizeHeightMin, false );
            }

            // 버튼 로그 추가
            pDocument.SetUpdateButtonLog( this, strLog );
        }
    }
}