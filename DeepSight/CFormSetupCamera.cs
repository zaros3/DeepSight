using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;
using HLDevice;
using System.Reflection;
namespace DeepSight
{
	public partial class CFormSetupCamera : Form, CFormInterface
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 카메라 인덱스
		private CDefine.enumCamera m_eCameraIndex;
		// 레시피 파라미터 ( 카메라 설정 )
		private CConfig.CRecipeParameter m_objRecipeParameter;
		// 카메라 화면
		private CFormDisplay m_objFormDisplay = new CFormDisplay();
		// 유저 권한 레벨에 따른 버튼 상태 변경
		public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
		public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CFormSetupCamera()
		{
			InitializeComponent();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormSetupCamera_Load( object sender, EventArgs e )
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
		private void CFormSetupCamera_FormClosed( object sender, FormClosedEventArgs e )
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
				// 폼 초기화
				if( false == InitializeForm() ) break;
				// 인덱스 정의
				m_eCameraIndex = CDefine.enumCamera.CAMERA_1;
				// 레시피 파라미터 복사
				m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCameraIndex );
				// 버튼 언어 변경
				SetChangeLanguage();
				// 버튼 이벤트 로그 정의
				SetButtonEventChange();
				// 타이머 외부에서 제어
				timer.Interval = 10;
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
            m_objFormDisplay.DeInitialize();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool InitializeForm()
		{
			bool bReturn = false;

			do {
				// 카메라 화면 초기화
				m_objFormDisplay.Initialize( ( int )CDefine.enumCamera.CAMERA_1, "CAMERA" );
				m_objFormDisplay.SetSize( panelVision.Location.X, panelVision.Location.Y, panelVision.Width, panelVision.Height, false );
				SetFormDockStyle( m_objFormDisplay, panelVision );
				// 버튼 색상 정의
				SetButtonColor();
				// 설비 타입에 따라 버튼 숨김 처리 필요
				SetChangeMachineTypeButton();

				bReturn = true;
			} while( false );

			return bReturn;
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
			pFormCommon.SetButtonColor( this.BtnTitleCamera, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnCamera1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnCamera2, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnCamera3, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnCamera4, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleCameraSetting, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnTitleGain, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnGain, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleExposureTime, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnExposureTime, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnLive, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnReverseX, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnReverseY, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnRotation90, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnRotation270, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleLightController, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnTitleLight1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnLight1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleLight2, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnLight2, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnLoad, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnSave, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
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
					break;
				case CDefine.enumMachineType.PROCESS_110:
					break;
				case CDefine.enumMachineType.PROCESS_150:
					break;
			}
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
				SetControlChangeLanguage( this.BtnTitleCamera );
				SetControlChangeLanguage( this.BtnCamera1 );
				SetControlChangeLanguage( this.BtnCamera2 );
				SetControlChangeLanguage( this.BtnCamera3 );
				SetControlChangeLanguage( this.BtnCamera4 );
				SetControlChangeLanguage( this.BtnTitleCameraSetting );
				SetControlChangeLanguage( this.BtnTitleGain );
				SetControlChangeLanguage( this.BtnTitleExposureTime );
				SetControlChangeLanguage( this.BtnLive );
				SetControlChangeLanguage( this.BtnReverseX );
				SetControlChangeLanguage( this.BtnReverseY );
				SetControlChangeLanguage( this.BtnRotation90 );
				SetControlChangeLanguage( this.BtnRotation270 );
				SetControlChangeLanguage( this.BtnTitleLightController );
				SetControlChangeLanguage( this.BtnTitleLight1 );
				SetControlChangeLanguage( this.BtnTitleLight2 );
				SetControlChangeLanguage( this.BtnLoad );
				SetControlChangeLanguage( this.BtnSave );

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
		//목적 : 버튼 이벤트 로그 추가
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonEventChange()
		{
			SetButtonEventChange( this.BtnCamera1 );
			SetButtonEventChange( this.BtnCamera2 );
			SetButtonEventChange( this.BtnCamera3 );
			SetButtonEventChange( this.BtnCamera4 );
			SetButtonEventChange( this.BtnGain );
			SetButtonEventChange( this.BtnExposureTime );
			SetButtonEventChange( this.BtnLive );
			SetButtonEventChange( this.BtnReverseX );
			SetButtonEventChange( this.BtnReverseY );
			SetButtonEventChange( this.BtnRotation90 );
			SetButtonEventChange( this.BtnRotation270 );
			SetButtonEventChange( this.BtnLight1 );
			SetButtonEventChange( this.BtnLight2 );
			SetButtonEventChange( this.BtnLoad );
			SetButtonEventChange( this.BtnSave );
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
			Type ControlType = sender.GetType();
			if( ControlType.Name == "CheckBox" ) {
				pDocument.SetUpdateButtonLog( this, string.Format( "{0} : [ {1} ]", ( sender as CheckBox ).Name, "TRUE" ) );
			}
			else {
				pDocument.SetUpdateButtonLog( this, string.Format( "{0} : [ {1} ]", ( sender as Button ).Name, "TRUE" ) );
			}
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
			Type ControlType = sender.GetType();
			if( ControlType.Name == "CheckBox" ) {
				pDocument.SetUpdateButtonLog( this, string.Format( "{0} : [ {1} ]", ( sender as CheckBox ).Name, "FALSE" ) );
			}
			else {
				pDocument.SetUpdateButtonLog( this, string.Format( "{0} : [ {1} ]", ( sender as Button ).Name, "FALSE" ) );
			}
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
		//목적 : 타이머 유무
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetTimer( bool bTimer )
		{
			var pDocument = CDocument.GetDocument;
			timer.Enabled = bTimer;

			if( true == bTimer ) {
				// 레시피 파라미터 복사
				m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCameraIndex );
			}
			else {
				// 카메라 라이브 종료
				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
					pDocument.SetLiveMode( iLoopCount, CDefine.enumLiveMode.LIVE_MODE_OFF );
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Visible 유무
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetVisible( bool bVisible )
		{
			var pDocument = CDocument.GetDocument;
			this.Visible = bVisible;

			if( true == bVisible ) {
				var pFormCommon = CFormCommon.GetFormCommon;
				// 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
				m_delegateSetChangeButtonStatus = new DelegateSetChangeButtonStatus( pFormCommon.SetChangeButtonStatus );
				// 페이지 편집 권한에 따라 버튼 상태 변경
				pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_SETUP );
				SetButtonStatus();
				// 해당 폼을 말단으로 설정
				pDocument.GetMainFrame().SetCurrentForm( this );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Visible 유무
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonStatus()
		{
			var pDocument = CDocument.GetDocument;
			if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER == pDocument.GetUserInformation().m_eAuthorityLevel ) {
				BtnReverseX.Visible = true;
				BtnReverseY.Visible = true;
				BtnRotation90.Visible = true;
				BtnRotation270.Visible = true;
			}
			else {
				BtnReverseX.Visible = false;
				BtnReverseY.Visible = false;
				BtnRotation90.Visible = false;
				BtnRotation270.Visible = false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void timer_Tick( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			var pFormCommon = CFormCommon.GetFormCommon;
			// 카메라 세팅
			// 카메라 1
			if( CDefine.enumCamera.CAMERA_1 == m_eCameraIndex ) {
				pFormCommon.SetButtonBackColor( BtnCamera1, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnCamera1, pFormCommon.COLOR_UNACTIVATE );
			}
		
			// 라이브
			if( CDefine.enumLiveMode.LIVE_MODE_ON == pDocument.GetLiveMode( ( int )m_eCameraIndex ) ) {
				pFormCommon.SetButtonBackColor( BtnLive, pFormCommon.COLOR_ACTIVATE );
				m_objFormDisplay.UpdateDisplay( pDocument.GetLiveImage( ( int )m_eCameraIndex ) );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnLive, pFormCommon.COLOR_UNACTIVATE );
			}
			// 반전 X
			if( true == m_objRecipeParameter.objCameraConfig.bReverseX ) {
				pFormCommon.SetButtonBackColor( BtnReverseX, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnReverseX, pFormCommon.COLOR_UNACTIVATE );
			}
			// 반전 Y
			if( true == m_objRecipeParameter.objCameraConfig.bReverseY ) {
				pFormCommon.SetButtonBackColor( BtnReverseY, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnReverseY, pFormCommon.COLOR_UNACTIVATE );
			}
			// 90도 회전
			if( true == m_objRecipeParameter.objCameraConfig.bRotation90 ) {
				pFormCommon.SetButtonBackColor( BtnRotation90, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnRotation90, pFormCommon.COLOR_UNACTIVATE );
			}
			// 270도 회전
			if( true == m_objRecipeParameter.objCameraConfig.bRotation270 ) {
				pFormCommon.SetButtonBackColor( BtnRotation270, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnRotation270, pFormCommon.COLOR_UNACTIVATE );
			}
			// 게인
			pFormCommon.SetButtonText( this.BtnGain, string.Format( "{0}", ( int )m_objRecipeParameter.objCameraConfig.dGain ) );
			// 노출 시간
			pFormCommon.SetButtonText( this.BtnExposureTime, string.Format( "{0}", ( int )m_objRecipeParameter.objCameraConfig.dExposureTime ) );
			// 조명 1
			// BtnLight1.Text = 미구현
			// 조명 2
			// BtnLight2.Text = 미구현
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnGain_Click( object sender, EventArgs e )
		{
			FormKeyPad objKeyPad = new FormKeyPad( double.Parse( BtnGain.Text ) );
			if( DialogResult.OK == objKeyPad.ShowDialog() ) {
				m_objRecipeParameter.objCameraConfig.dGain = objKeyPad.m_dValue;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnExposureTime_Click( object sender, EventArgs e )
		{
			FormKeyPad objKeyPad = new FormKeyPad( double.Parse( BtnExposureTime.Text ) );
			if( DialogResult.OK == objKeyPad.ShowDialog() ) {
				m_objRecipeParameter.objCameraConfig.dExposureTime = objKeyPad.m_dValue;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 라이브
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnLive_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			if( CDefine.enumLiveMode.LIVE_MODE_OFF == pDocument.GetLiveMode( ( int )m_eCameraIndex ) ) {
				pDocument.SetLiveMode( ( int )m_eCameraIndex, CDefine.enumLiveMode.LIVE_MODE_ON );
			}
			else {
				pDocument.SetLiveMode( ( int )m_eCameraIndex, CDefine.enumLiveMode.LIVE_MODE_OFF );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnReverseX_Click( object sender, EventArgs e )
		{
			m_objRecipeParameter.objCameraConfig.bReverseX = !m_objRecipeParameter.objCameraConfig.bReverseX;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnReverseY_Click( object sender, EventArgs e )
		{
			m_objRecipeParameter.objCameraConfig.bReverseY = !m_objRecipeParameter.objCameraConfig.bReverseY;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnRotation90_Click( object sender, EventArgs e )
		{
			m_objRecipeParameter.objCameraConfig.bRotation90 = !m_objRecipeParameter.objCameraConfig.bRotation90;

			if( true == m_objRecipeParameter.objCameraConfig.bRotation90 ) {
				m_objRecipeParameter.objCameraConfig.bRotation270 = false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnRotation270_Click( object sender, EventArgs e )
		{
			m_objRecipeParameter.objCameraConfig.bRotation270 = !m_objRecipeParameter.objCameraConfig.bRotation270;

			if( true == m_objRecipeParameter.objCameraConfig.bRotation270 ) {
				m_objRecipeParameter.objCameraConfig.bRotation90 = false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnLight1_Click( object sender, EventArgs e )
		{
			FormKeyPad objKeyPad = new FormKeyPad( double.Parse( BtnLight1.Text ) );
			if( DialogResult.OK == objKeyPad.ShowDialog() ) {
				//if( 0 > ( int )objKeyPad.m_dValue ) objKeyPad.m_dValue = 0;
				//else if( 255 < ( int )objKeyPad.m_dValue ) objKeyPad.m_dValue = 255;
				//m_objLightControllerParameter[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ].iIntensity[ ( int )CDefine.enumCamera.CAMERA_1 + ( ( int )m_eCamera * 2 ) ] = ( int )objKeyPad.m_dValue;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnLight2_Click( object sender, EventArgs e )
		{
			FormKeyPad objKeyPad = new FormKeyPad( double.Parse( BtnLight2.Text ) );
			if( DialogResult.OK == objKeyPad.ShowDialog() ) {
				//if( 0 > ( int )objKeyPad.m_dValue ) objKeyPad.m_dValue = 0;
				//else if( 255 < ( int )objKeyPad.m_dValue ) objKeyPad.m_dValue = 255;

				//pDocument.m_objProcessMain.m_objLightController[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ].HLSetLightOff();
				//System.Threading.Thread.Sleep( 100 );
				//m_objLightControllerParameter[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ].iIntensity[ ( int )CDefine.enumCamera.CAMERA_POL_A_ALIGN_2 + ( ( int )m_eCamera * 2 ) ] = ( int )objKeyPad.m_dValue;
				//pDocument.m_objProcessMain.m_objLightController[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ].HLSetLightIntensity( m_objLightControllerParameter[ ( int )CDefine.enumLightController.LIGHT_CONTROLLER_MAIN ].iIntensity );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnLoad_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCameraIndex );
			// 불러오기가 완료되었습니다.
			pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10015 );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnSave_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// 카메라 설정을 변경할 때는 라이브 오프하고 진행
			CDefine.enumLiveMode ePreviousLiveMode = pDocument.GetLiveMode( ( int )m_eCameraIndex );
			if( CDefine.enumLiveMode.LIVE_MODE_ON == ePreviousLiveMode ) {
				pDocument.SetLiveMode( ( int )m_eCameraIndex, CDefine.enumLiveMode.LIVE_MODE_OFF );
				// 라이브 종료 확인 1 sec -
				int iTimeout = 1000;
				int iSleep = 100;
				while( true ) {
					if( 0 >= iTimeout ) break;
					Application.DoEvents();
					iTimeout -= iSleep;
					System.Threading.Thread.Sleep( iSleep );
				}
			}
			pDocument.m_objConfig.SaveRecipeParameter( ( int )m_eCameraIndex, m_objRecipeParameter );
			// 카메라 파라미터 설정
			pDocument.m_objProcessMain.SetCameraConfig();
			// 시뮬레이션인 경우 무시 ( 실제 값 적용 확인 )
			if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
				m_objRecipeParameter.objCameraConfig.dGain = pDocument.m_objProcessMain.m_objCamera[ ( int )m_eCameraIndex ].HLGetGain();
				m_objRecipeParameter.objCameraConfig.dExposureTime = pDocument.m_objProcessMain.m_objCamera[ ( int )m_eCameraIndex ].HLGetExposureTime();
				pDocument.m_objConfig.SaveRecipeParameter( ( int )m_eCameraIndex, m_objRecipeParameter );
			}
			// 저장이 완료되었습니다.
			pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10014 );
			pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "Vision Recipe Save" );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 카메라 선택
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnCamera1_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;

			do {
				if( CDefine.enumCamera.CAMERA_1 == m_eCameraIndex ) break;
				// 이전 라이브 종료
				pDocument.SetLiveMode( ( int )m_eCameraIndex, CDefine.enumLiveMode.LIVE_MODE_OFF );
				m_eCameraIndex = CDefine.enumCamera.CAMERA_1;
				m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCameraIndex );

			} while( false );
		}

		private void BtnCamera2_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;

			do {


			} while( false );
		}

		private void BtnCamera3_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;

			do {


			} while( false );
		}

		private void BtnCamera4_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;

			do {
				

			} while( false );
		}
	}
}