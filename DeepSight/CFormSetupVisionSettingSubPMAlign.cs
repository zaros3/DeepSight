using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.PMAlign;
using HLVision.VisionLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSight
{
	public partial class CFormSetupVisionSettingSubPMAlign : Form, CFormInterface
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 메인 코그 디스플레이
		private CogDisplay m_objCogDisplayMain;
		// PM 얼라인 툴
		private CogPMAlignTool m_objPMAlignTool;
		// 검색 영역 설정
		private CogRectangle m_objCogSearchRegion;
		// 패턴 영역 설정
		private CogRectangleAffine m_objCogPatternRegion;
		// 트레인 원점
		private CogCoordinateAxes m_objCogTrainOrigin;
        // 픽스쳐툴
        private CogFixtureTool m_objFixtureTool;
        // 폼디스플레이 객체
        private CFormDisplay m_objFormDisplay;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public delegate void DelegateMasterImage( CogImage8Grey objImage );
        public event DelegateMasterImage EventMasterImage;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CFormSetupVisionSettingSubPMAlign()
		{
			InitializeComponent();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormSetupVisionSettingSubPMAlign_Load( object sender, EventArgs e )
		{
			// Initialize 함수보다 시점 늦음
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 종료
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormSetupVisionSettingSubPMAlign_FormClosed( object sender, FormClosedEventArgs e )
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
        public bool Initialize( CogDisplay objCogDisplay, CFormDisplay objDisplay = null )
		{
			bool bReturn = false;

			do {
                // 메인 코그 디스플레이 연결
                m_objCogDisplayMain = objCogDisplay;
                m_objFormDisplay = objDisplay;
                // 초기화 언어 변경
                SetChangeLanguage();
				// 버튼 이벤트 로그 정의
				SetButtonEventChange();
				// 버튼 색상 정의
				SetButtonColor();
                // 픽스쳐툴 사용
                m_objFixtureTool = new CogFixtureTool();
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
			cogDisplayTrainImage.InteractiveGraphics.Dispose();
			cogDisplayTrainImage.Dispose();
			cogDisplayRunImage.InteractiveGraphics.Dispose();
			cogDisplayRunImage.Dispose();
		}

        public void SetMainDisplayConnect( CogDisplay objCogDisplay )
        {
            // 메인 코그 디스플레이 연결
            m_objCogDisplayMain = objCogDisplay;
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
			pFormCommon.SetButtonColor( this.BtnTitlePatternSetting, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnTrainImageGrab, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnPatternOriginCenter, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnMaskImage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTrain, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTrainStatus, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleScore, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnScore, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnRun, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 버튼 이벤트 로그 추가
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonEventChange()
		{
			SetButtonEventChange( this.BtnTrainImageGrab );
			SetButtonEventChange( this.BtnPatternOriginCenter );
			SetButtonEventChange( this.BtnMaskImage );
			SetButtonEventChange( this.BtnTrain );
			SetButtonEventChange( this.BtnScore );
			SetButtonEventChange( this.BtnRun );
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
			}
			else {
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
			objForm.Visible = true;
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
				SetControlChangeLanguage( this.BtnTitlePatternSetting );
				SetControlChangeLanguage( this.BtnTrainImageGrab );
				SetControlChangeLanguage( this.BtnPatternOriginCenter );
				SetControlChangeLanguage( this.BtnMaskImage );
				SetControlChangeLanguage( this.BtnTrain );
				SetControlChangeLanguage( this.BtnTitleScore );
				SetControlChangeLanguage( this.BtnRun );
				// 팝업 메뉴 변경
				var pDocument = CDocument.GetDocument;
				pDocument.SetCogDisplayPopupMenuChangeLanguage( cogDisplayRunImage );

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

			if( true == bVisible ) {
				// 해당 폼을 말단으로 설정
				var pDocument = CDocument.GetDocument;
				pDocument.GetMainFrame().SetCurrentForm( this );

            }
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레시피 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 		public void SetSaveRecipe( HLVision.VisionLibrary.CVisionLibraryAbstract.CInitializeParameter objInitialize )
// 		{
// // 			CVisionLibraryCogPMAlign obj = new CVisionLibraryCogPMAlign();
// // 			obj.HLInitialize( objInitialize );
// // 			obj.m_objPMAlignTool = this.m_objPMAlignTool;
// // 			obj.HLSaveRecipe( objInitialize.strRecipePath, objInitialize.strRecipeName );
// 		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 툴 적용 ( 외부 -> 내부 )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetTool( CogPMAlignTool objPMAlignTool )
		{
			m_objPMAlignTool = objPMAlignTool;
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 툴 반환 ( 내부 -> 외부 )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CogPMAlignTool GetTool()
		{
			return m_objPMAlignTool;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 유무 체크해서 메인 디스플레이에 이미지를 가져옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetMainImage()
		{
			do
            {
                if ( null == m_objPMAlignTool ) break;
                if ( null == m_objPMAlignTool.Pattern.TrainImage )
                {
                    // 이미지 null 체크 말고도.. 이미지 할당 유무 체크해야 함.
                    if ( null == m_objPMAlignTool.InputImage ) break;
                    if ( false == m_objPMAlignTool.InputImage.Allocated ) break;
                    // 픽스쳐툴 실행해서 아웃풋 이미지로 표시하자

                    m_objCogDisplayMain.Image = m_objPMAlignTool.InputImage;// m_objPMAlignTool.InputImage;
                    break;
                } else
                {
                    // 이미지 null 체크 말고도.. 이미지 할당 유무 체크해야 함.
                    if ( false == m_objPMAlignTool.Pattern.TrainImage.Allocated ) break;
                    if( true == m_objPMAlignTool.Pattern.Trained )
                    {
                        m_objPMAlignTool.InputImage = m_objPMAlignTool.Pattern.TrainImage;
                        CogImage8Grey objResultImage;
                        RunPMAlign( out objResultImage );
                        if( null == objResultImage )
                            RunPMAlign( out objResultImage );
                        m_objCogDisplayMain.Image = objResultImage;
                    }
                    else
                    {
                        m_objCogDisplayMain.Image = m_objPMAlignTool.InputImage;// m_objPMAlignTool.InputImage;
                    }
                }
			} while( false );
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
            do
            {
                m_objPMAlignTool.Run();
                CogImage8Grey FixtureImage = m_objPMAlignTool.InputImage.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey;
                FixtureImage.SelectedSpaceName = "@";

                m_objFixtureTool.InputImage = FixtureImage;
                if ( null == m_objPMAlignTool.Results || 0 >= m_objPMAlignTool.Results .Count ) break;

                m_objFixtureTool.RunParams.UnfixturedFromFixturedTransform = m_objPMAlignTool.Results[ 0 ].GetPose();
                m_objFixtureTool.Run();
                objOutImage = m_objFixtureTool.OutputImage as CogImage8Grey;
                bReturn = true;
            } while ( false );

            return bReturn;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 패턴 이미지를 가져옴
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetTrainedPatternImage()
		{
			do
            {
                if ( null == m_objPMAlignTool ) break;
                if ( null == m_objPMAlignTool.Pattern ) break;
				// 트레인 유무 예외 처리
				if( false == m_objPMAlignTool.Pattern.Trained ) break;

				cogDisplayTrainImage.Image = m_objPMAlignTool.Pattern.GetTrainedPatternImage();

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 검색 영역 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetSearchRegion()
		{
			do {
                // 메인 디스플레이에 이미지 있는 경우에만
                if ( null == m_objCogDisplayMain.Image ) break;

                CogRectangle obj = m_objPMAlignTool.SearchRegion as CogRectangle;
				// 기존에 검색 영역이 있는 경우 연결
				if( null != obj ) {
					obj.Interactive = true;
					obj.GraphicDOFEnable = CogRectangleDOFConstants.All;
					obj.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
					obj.Color = CogColorConstants.Cyan;
					m_objCogSearchRegion = new CogRectangle( obj );
                    m_objPMAlignTool.SearchRegion = m_objCogSearchRegion;
                }
				// 없는 경우 새로 생성
				else {
					m_objCogSearchRegion = new CogRectangle();
					m_objCogSearchRegion.Interactive = true;
					m_objCogSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.All;
					m_objCogSearchRegion.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
					m_objCogSearchRegion.SetXYWidthHeight( 100, 100, m_objCogDisplayMain.Width - 200, m_objCogDisplayMain.Height - 200 );
					m_objCogSearchRegion.Color = CogColorConstants.Cyan;
					// 검색 영역 넣어줌
					m_objPMAlignTool.SearchRegion = m_objCogSearchRegion;
				}
				m_objCogDisplayMain.InteractiveGraphics.Add( m_objCogSearchRegion, "SUB_PMALIGN_INTERACTIVE", false );

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 패턴 영역 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetPatternRegion()
		{
			do {
				// 메인 디스플레이에 이미지 있는 경우에만
				if( null == m_objCogDisplayMain.Image ) break;

				CogRectangleAffine obj = m_objPMAlignTool.Pattern.TrainRegion as CogRectangleAffine;
				// 기존에 패턴 영역이 있는 경우 연결
				if( null != obj ) {
					obj.Interactive = true;
					obj.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
					obj.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
					obj.Color = CogColorConstants.Green;
					// 패턴 영역 이동 시 이벤트 ( 영역 이동시키면 트레인 취소 )
					obj.DraggingStopped += new CogDraggingStoppedEventHandler( EventTrainRegionMove );
					m_objCogPatternRegion = new CogRectangleAffine( obj );
				}
				// 없는 경우 새로 생성
				else {
					m_objCogPatternRegion = new CogRectangleAffine();
					m_objCogPatternRegion.Interactive = true;
					m_objCogPatternRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
					m_objCogPatternRegion.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
					m_objCogPatternRegion.SetCenterLengthsRotationSkew( m_objCogDisplayMain.Width / 2, m_objCogDisplayMain.Height / 2, 50, 50, 0, 0 );
					m_objCogPatternRegion.Color = CogColorConstants.Green;
					// 패턴 영역 이동 시 이벤트 ( 영역 이동시키면 트레인 취소 )
					m_objCogPatternRegion.DraggingStopped += new CogDraggingStoppedEventHandler( EventTrainRegionMove );
					// 패턴 영역 넣어줌
					m_objPMAlignTool.Pattern.TrainRegion = m_objCogPatternRegion;
				}
				m_objCogDisplayMain.InteractiveGraphics.Add( m_objCogPatternRegion, "SUB_PMALIGN_INTERACTIVE", false );

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트레인 원점
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetTrainOrigin()
		{
			do {
				// 메인 디스플레이에 이미지 있는 경우에만
				if( null == m_objCogDisplayMain.Image ) break;

				// 현재 툴에 레코드 받아옴
				m_objPMAlignTool.CurrentRecordEnable = CogPMAlignCurrentRecordConstants.All;
				ICogRecord objCogRecord = m_objPMAlignTool.CreateCurrentRecord();
				CogCoordinateAxes obj = objCogRecord.SubRecords[ "TrainImage" ].SubRecords[ "PatternOrigin" ].Content as CogCoordinateAxes;
				// 기존에 트레인 원점 있는 경우 연결
				if( null != obj ) {
					obj.Interactive = true;
					obj.GraphicDOFEnable = CogCoordinateAxesDOFConstants.All;
					obj.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
					obj.XAxisEndPointAdornment = CogCoordinateAxesEndPointAdornmentConstants.Arrow;
					obj.YAxisEndPointAdornment = CogCoordinateAxesEndPointAdornmentConstants.Arrow;
					obj.Color = CogColorConstants.Green;
					obj.XAxisLabel.Color = CogColorConstants.Green;
					obj.YAxisLabel.Color = CogColorConstants.Green;
					m_objCogTrainOrigin = obj;
				}
				// 없는 경우 새로 생성
				else {
					m_objCogTrainOrigin = new CogCoordinateAxes();
					m_objCogTrainOrigin.Interactive = true;
					m_objCogTrainOrigin.GraphicDOFEnable = CogCoordinateAxesDOFConstants.All;
					m_objCogTrainOrigin.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
					m_objCogTrainOrigin.OriginX = m_objCogPatternRegion.CenterX;
					m_objCogTrainOrigin.OriginY = m_objCogPatternRegion.CenterY;
					m_objCogTrainOrigin.XAxisEndPointAdornment = CogCoordinateAxesEndPointAdornmentConstants.Arrow;
					m_objCogTrainOrigin.YAxisEndPointAdornment = CogCoordinateAxesEndPointAdornmentConstants.Arrow;
					m_objCogTrainOrigin.Color = CogColorConstants.Green;
					m_objCogTrainOrigin.XAxisLabel.Color = CogColorConstants.Green;
					m_objCogTrainOrigin.YAxisLabel.Color = CogColorConstants.Green;
					// 트레인 원점 설정
					m_objPMAlignTool.Pattern.Origin.TranslationX = m_objCogTrainOrigin.OriginX;
					m_objPMAlignTool.Pattern.Origin.TranslationY = m_objCogTrainOrigin.OriginY;
				}
				cogDisplayTrainImage.StaticGraphics.Add( m_objCogTrainOrigin, "SUB_PMALIGN_STATIC" );
				m_objCogDisplayMain.InteractiveGraphics.Add( m_objCogTrainOrigin, "SUB_PMALIGN_INTERACTIVE", false );

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 패턴 그래픽 추가
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetPatternGraphics()
		{
			do {
				// 메인 디스플레이에 이미지 있는 경우에만
				if( null == m_objCogDisplayMain.Image ) break;
				if( null == m_objPMAlignTool.Pattern ) break;
				// 트레인 유무 예외 처리
				if( false == m_objPMAlignTool.Pattern.Trained ) break;

				cogDisplayTrainImage.StaticGraphics.AddList( m_objPMAlignTool.Pattern.CreateGraphicsCoarse( CogColorConstants.Green ), "COARSE" );
				cogDisplayTrainImage.StaticGraphics.AddList( m_objPMAlignTool.Pattern.CreateGraphicsFine( CogColorConstants.Yellow ), "FINE" );
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 마스킹된 이미지 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetMaskImage()
		{
			do {
				// 메인 디스플레이에 이미지 있는 경우에만
				if( null == m_objCogDisplayMain.Image ) break;
				// 예외 확인
				if( null == m_objPMAlignTool.Pattern ) break;
				// 트레인 유무 예외 처리
				if( false == m_objPMAlignTool.Pattern.Trained ) break;
				// 트레인 패턴 디스플레이에 표시할 마스크 이미지
				CogMaskGraphic objTrainPatternMask = new CogMaskGraphic();
				objTrainPatternMask.Image = m_objPMAlignTool.Pattern.GetTrainedPatternImageMask();
				if( null != objTrainPatternMask.Image ) {
					objTrainPatternMask.Color = CogColorConstants.Grey;
					cogDisplayTrainImage.StaticGraphics.Add( ( ICogGraphicInteractive )objTrainPatternMask, "" );
				}
				// 트레인 디스플레이에 표시할 마스크 이미지
// 				CogMaskGraphic objTrainMask = new CogMaskGraphic();
// 				objTrainMask.Image = m_objPMAlignTool.Pattern.TrainImageMask;
// 				if( null != objTrainMask.Image ) {
// 					objTrainMask.Color = CogColorConstants.Grey;
// 					m_objCogDisplayMain.InteractiveGraphics.Add( ( ICogGraphicInteractive )objTrainMask, "SUB_PMALIGN_INTERACTIVE", false );
// 				}

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 툴 내 패턴 그래픽 -> 현재 디스플레이에 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void GetPMAlign()
		{
			do {
				// 디스플레이 초기화
				m_objCogDisplayMain.Image = null;
				// 트레인 이미지는 변경될 때마다 날려줌
				cogDisplayTrainImage.Image = null;
				// 이미지 유무 체크해서 메인 디스플레이에 이미지를 가져옴
				SetMainImage();
                // 디스플레이 클리어
                ClearMainDisplayGraphics();
// 				m_objCogDisplayMain.InteractiveGraphics.Clear();
// 				m_objCogDisplayMain.StaticGraphics.Clear();
                // 트레인 패턴 디스플레이 클리어
                cogDisplayTrainImage.StaticGraphics.Clear();
				// 패턴 이미지를 가져옴
				SetTrainedPatternImage();
				// 검색 영역 설정
				SetSearchRegion();
				// 패턴 영역 설정
				SetPatternRegion();
				// 트레인 원점
				SetTrainOrigin();
				// 패턴 그래픽 추가
				SetPatternGraphics();
				// 마스킹된 이미지 표시
				SetMaskImage();
                if( false == m_objPMAlignTool.Pattern.Trained )
                    if( null != m_objFormDisplay ) m_objFormDisplay.SetHideSearchRegion();
            } while( false );
		}
        private void ClearMainDisplayGraphics()
        {
            if ( -1 != m_objCogDisplayMain.InteractiveGraphics.FindItem( "SUB_PMALIGN_INTERACTIVE", CogDisplayZOrderConstants.Front ) )
            {
                m_objCogDisplayMain.InteractiveGraphics.Remove( "SUB_PMALIGN_INTERACTIVE" );
            }
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트레인 영역 드래그 이벤트
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void EventTrainRegionMove( object sender, CogDraggingEventArgs e )
		{
			do {
				// 트레인 디스플레이 이미지 확인
				if( null == cogDisplayTrainImage.Image ) break;
				// PM 얼라인 툴 예외 처리
				if( false == m_objPMAlignTool.Pattern.Trained ) break;
				// 기존 디스플레이 클리어
				cogDisplayTrainImage.StaticGraphics.Clear();
				cogDisplayTrainImage.InteractiveGraphics.Clear();
				// 트레인 상태 날림
				m_objPMAlignTool.Pattern.Untrain();
				// 트레인 디스플레이 이미지 없앰
				cogDisplayTrainImage.Image = null;

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 실행 이미지 디스플레이 이미지 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void UpdateDisplay( CogImage8Grey objImage )
		{
			do {
				// 이미지 확인
				if( null == objImage ) break;
				// 기존 디스플레이 클리어
				cogDisplayRunImage.StaticGraphics.Clear();
				cogDisplayRunImage.Image = objImage;
				
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
			var pFormCommon = CFormCommon.GetFormCommon;

			do {
				if( null == m_objPMAlignTool.Pattern ) break;
				// 트레인 유무
				if( true == m_objPMAlignTool.Pattern.Trained ) {
					pFormCommon.SetButtonBackColor( this.BtnTrainStatus, pFormCommon.LAMP_GREEN_ON );
				}
				else {
					pFormCommon.SetButtonBackColor( this.BtnTrainStatus, pFormCommon.LAMP_RED_OFF );
				}
				// 스코어
				pFormCommon.SetButtonText( this.BtnScore, ( m_objPMAlignTool.RunParams.AcceptThreshold * 100 ).ToString() );

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트레인 이미지 그랩
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnTrainImageGrab_Click( object sender, EventArgs e )
		{
			do {
				if( null == cogDisplayRunImage.Image ) break;

				m_objCogDisplayMain.Image = cogDisplayRunImage.Image;
				m_objPMAlignTool.InputImage = cogDisplayRunImage.Image as CogImage8Grey;
                // 새로운 트레인 이미지 그랩하고 트레인 상태 날려줌.
                m_objPMAlignTool.Pattern.Untrain();
                m_objPMAlignTool.Pattern.TrainImageMask = null;
                if ( null != m_objFormDisplay ) m_objFormDisplay.SetHideSearchRegion();
				GetPMAlign();

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 설정된 영역 내 센터로 위치 이동
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnPatternOriginCenter_Click( object sender, EventArgs e )
		{
            do
            {
                if ( null == m_objCogTrainOrigin ) break;
                m_objCogTrainOrigin.SetOriginCornerXCornerY(
                    m_objCogPatternRegion.CenterX, m_objCogPatternRegion.CenterY,
                    m_objCogPatternRegion.CenterX + 1, m_objCogPatternRegion.CenterY,
                    m_objCogPatternRegion.CenterX, m_objCogPatternRegion.CenterY + 1 );
            } while ( false );
            
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트레인
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool SetTrain()
		{
			bool bReturn = false;
			
			do {
				var pDocument = CDocument.GetDocument;
                if ( null == m_objPMAlignTool ) break;
                try {
                    m_objPMAlignTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;

                    m_objPMAlignTool.Pattern.TrainImage = m_objCogDisplayMain.Image;
                    m_objPMAlignTool.Pattern.TrainImage.SelectedSpaceName = "@";

                    m_objPMAlignTool.Pattern.Origin.TranslationX = m_objCogTrainOrigin.OriginX;
					m_objPMAlignTool.Pattern.Origin.TranslationY = m_objCogTrainOrigin.OriginY;
					m_objPMAlignTool.Pattern.TrainRegion = new CogRectangleAffine( m_objCogPatternRegion );
                    m_objPMAlignTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                    if( CDefine.enumMachineType.PROCESS_150 != pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                        m_objPMAlignTool.RunParams.ZoneAngle.High = 10 * ( Math.PI / 180 );
                        m_objPMAlignTool.RunParams.ZoneAngle.Low = -10 * ( Math.PI / 180 );
                    } else {
                        m_objPMAlignTool.RunParams.ZoneAngle.High = 0;
                        m_objPMAlignTool.RunParams.ZoneAngle.Low = 0;
                    }
					
					m_objPMAlignTool.RunParams.ScoreUsingClutter = false;
					m_objPMAlignTool.Pattern.Train();
				}
				catch( Exception ex ) {
					pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "CFormSetupVisionSettingSubPMAlign SetTrain " + ex.Message );
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 패턴 영역 선택 유무 확인
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool IsSelectPatternRegion()
		{
			bool bReturn = false;

			do {
				if( null == m_objCogPatternRegion ) break;

				bReturn = m_objCogPatternRegion.Selected;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 패턴 영역 변경 이벤트
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetChangePatternRegion( int iKeyValue, bool bShift )
		{
			// 패턴 트레인 상태 날려버림
			EventTrainRegionMove( null, null );
			// 넓이, 높이 변경
			if( true == bShift ) {
				// 키보드 ← : 넓이 줄임
				if( 37 == iKeyValue ) {
					m_objCogPatternRegion.SideXLength -= 1.0;
				}
				// 키보드 ↑ : 높이 늘림
				else if( 38 == iKeyValue ) {
					m_objCogPatternRegion.SideYLength += 1.0;
				}
				// 키보드 → : 넓이 늘림
				else if( 39 == iKeyValue ) {
					m_objCogPatternRegion.SideXLength += 1.0;
				}
				// 키보드 ↓ : 높이 줄임
				else if( 40 == iKeyValue ) {
					m_objCogPatternRegion.SideYLength -= 1.0;
				}
			}
			// 위치 변경
			else {
				// 키보드 ← : X --
				if( 37 == iKeyValue ) {
					m_objCogPatternRegion.CenterX -= 1.0;
				}
				// 키보드 ↑ : Y --
				else if( 38 == iKeyValue ) {
					m_objCogPatternRegion.CenterY -= 1.0;
				}
				// 키보드 → : X ++
				else if( 39 == iKeyValue ) {
					m_objCogPatternRegion.CenterX += 1.0;
				}
				// 키보드 ↓ : Y ++
				else if( 40 == iKeyValue ) {
					m_objCogPatternRegion.CenterY += 1.0;
				}
			}
			// 패턴 중심 이동
			//BtnPatternOriginCenter_Click( null, null );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 마스크
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnMaskImage_Click( object sender, EventArgs e )
		{
			CDialogCogPMAlignImageMask objDialogMask = new CDialogCogPMAlignImageMask( ( CogImage8Grey )m_objCogDisplayMain.Image, ( CogImage8Grey )m_objPMAlignTool.Pattern.TrainImageMask );
			if( DialogResult.OK == objDialogMask.ShowDialog() ) {
				// 이미지 마스킹 작업 완료 후 패턴을 언트레인 해준다 보정 작업 후 반드시 다시 티칭해야함
				m_objPMAlignTool.Pattern.Untrain();
				m_objPMAlignTool.Pattern.TrainImageMask = objDialogMask.GetMaskImage();
				// 트레인
				SetTrain();
				// 디스플레이 표시
				GetPMAlign();
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트레인 수행 ( 패턴, 영상캘리브레이션 )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnTrain_Click( object sender, EventArgs e )
		{
			bool bReturn = false;
			var pDocument = CDocument.GetDocument;

			do {
				if( null == m_objCogDisplayMain.Image ) break;
				// 트레인
				if( false == SetTrain() ) break;

				bReturn = true;
			} while( false );

			if( true == bReturn ) {
				// 트레인 성공 하였습니다.
				pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10048 );
			}
			else {
				// 트레인에 실패하였습니다.
				pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10049 );
			}
			// 디스플레이 표시
			GetPMAlign();

            // 완료 후, 그래픽 삭제
            ClearMainDisplayGraphics();

        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 비전 스코어 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnScore_Click( object sender, EventArgs e )
		{
			do {
				FormKeyPad objKeyPad = new FormKeyPad( 0.0 );
				if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
					if( 0 < objKeyPad.m_dResultValue && 100 > objKeyPad.m_dResultValue ) {
						m_objPMAlignTool.RunParams.AcceptThreshold = objKeyPad.m_dResultValue / 100.0;
					}
				}
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 패턴 검색
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnRun_Click( object sender, EventArgs e )
		{
			do {
				var pDocument = CDocument.GetDocument;
				var pFormCommon = CFormCommon.GetFormCommon;
				// 이미지 있는 경우에만
				if( null == cogDisplayRunImage.Image ) break;

				try {
                    // PM 얼라인 런
                    CVisionLibraryCogPMAlign objVisionLibrary = new CVisionLibraryCogPMAlign();
                    objVisionLibrary.m_objPMAlignTool = m_objPMAlignTool;
					CVisionLibraryCogPMAlign.stInputData objInput = new CVisionLibraryCogPMAlign.stInputData();
					CVisionLibraryCogPMAlign.stOutputData objOutput = new CVisionLibraryCogPMAlign.stOutputData();
					objInput.objCogImage = cogDisplayRunImage.Image as CogImage8Grey;
					if( false == objVisionLibrary.HLRun( objInput, out objOutput ) ) {
						pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "CFormSetupVisionSettingSubPMAlign BtnRun_Click HLRun Fail" );
						break;
					}
					// 그래픽 클리어
					cogDisplayRunImage.StaticGraphics.Clear();
					// 아예 못 찾으면 빨강
					if( 0 == objOutput.iPatternCount ) {
						pFormCommon.SetButtonBackColor( this.BtnRun, pFormCommon.LAMP_RED_OFF );
					}
					else {
						pFormCommon.SetButtonBackColor( this.BtnRun, pFormCommon.LAMP_GREEN_ON );
					}
					// 패턴 결과 이미지에 표시
					m_objPMAlignTool.LastRunRecordEnable = CogPMAlignLastRunRecordConstants.ResultsCoordinateAxes | CogPMAlignLastRunRecordConstants.ResultsMatchRegion | CogPMAlignLastRunRecordConstants.ResultsOrigin;
					ICogRecord objCogRecord = m_objPMAlignTool.CreateLastRunRecord();
					CogGraphicCollection objResult = ( CogGraphicCollection )objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "CompositeResultGraphics" ].Content;
					for( int iLoopCount = 0; iLoopCount < objResult.Count; iLoopCount++ ) {
						cogDisplayRunImage.StaticGraphics.Add( ( ICogGraphicInteractive )objResult[ iLoopCount ], "" );
					}
					// Result Display
					CogGraphicLabel objLabelSearchPosition = new CogGraphicLabel();
					objLabelSearchPosition.Color = CogColorConstants.Green;
					// 그래픽 정렬
					objLabelSearchPosition.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
					// x, y, string
					objLabelSearchPosition.SelectedSpaceName = "@";
					// 간격 계산을 이미지에 비율로 계산해서 항상 일정하게 표시되도록 변경
					double dWhiteSpaceX = cogDisplayRunImage.Image.Width * 0.01;
					double dWhiteSpaceY = cogDisplayRunImage.Image.Height * 0.01;
					double dSpace = 120.0;
					string strSearchPosition = string.Format( "{0:F3}, {1:F3}, {2:F3}, {3:F0}",
						objOutput.dListTranslationX[ 0 ], // X
						objOutput.dListTranslationY[ 0 ], // Y
						objOutput.dListRotation[ 0 ], // T
						objOutput.dListScore[ 0 ] * 100.0 ); // Score
					objLabelSearchPosition.SetXYText( dWhiteSpaceX, dWhiteSpaceY + ( dSpace ), strSearchPosition );
					objLabelSearchPosition.Visible = true;

					cogDisplayRunImage.StaticGraphics.Add( objLabelSearchPosition, "" );

                    // 메인 폼으로 마스터 포지션 이벤트 실행. 사용할 곳에서만 등록 해준다 ( ex 프리얼라인 )
                    if( null != EventMasterImage ) {
                        CogImage8Grey objImage = cogDisplayRunImage.Image as CogImage8Grey;
                        EventMasterImage( objImage );
                    }
					objCogRecord = m_objPMAlignTool.CreateCurrentRecord();
					CogRectangle objRectangle = objCogRecord.SubRecords[ "InputImage" ].SubRecords[ "SearchRegion" ].Content as CogRectangle;
					cogDisplayRunImage.StaticGraphics.Add( ( ICogGraphicInteractive )objRectangle, "" );
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.StackTrace );
				}
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : ocx
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnTitlePatternSetting_Click( object sender, EventArgs e )
		{
            var pDocument = CDocument.GetDocument;

            do
            {
                CUserInformation objUserInformation = pDocument.GetUserInformation();
                // 마스터만 가능하도록
                if ( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER != objUserInformation.m_eAuthorityLevel ) break;

                // ocx 넣기 전에 이미지 변경해줌
                ICogImage objOriginImage = m_objPMAlignTool.InputImage;
                m_objPMAlignTool.InputImage = cogDisplayRunImage.Image;
                CDialogCogPMAlign obj = new CDialogCogPMAlign( m_objPMAlignTool );
                obj.ShowDialog();
                // 빠져나오면서 원본 이미지 집어넣음
                m_objPMAlignTool.InputImage = objOriginImage;

            } while ( false );

            // 디스플레이 표시
            GetPMAlign();

            // 완료 후, 그래픽 삭제
            ClearMainDisplayGraphics();
        }
	}
}