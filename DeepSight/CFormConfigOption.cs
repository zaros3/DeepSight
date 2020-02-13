using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepSight
{
	public partial class CFormConfigOption : Form, CFormInterface
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 옵션 파라미터
		private CConfig.CSystemParameter m_objSystemParameter;
        private CConfig.CLightControllerParameter m_objLightControllerParameter;
        private CConfig.CPLCInitializeParameter m_objPlcParameter;
        private CConfig.CCameraParameter m_objCameraParameter;
        private CConfig.CRecipeParameter m_objRecipeParameter;
        private CDefine.enumCamera m_eCamera;
        private double m_dCameraResolutionDistance;
        private double m_dCameraResolutionPixel;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 유저 권한 레벨에 따른 버튼 상태 변경
        public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
        public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormConfigOption()
		{
			InitializeComponent();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormConfigOption_Load( object sender, EventArgs e )
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
		private void CFormConfigOption_FormClosed( object sender, FormClosedEventArgs e )
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
			var pDocument = CDocument.GetDocument;

			do {
				// 폼 초기화
				// 초기 메인 스테이지
				m_eCamera = CDefine.enumCamera.CAMERA_1;
                m_objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                m_objLightControllerParameter = pDocument.m_objConfig.GetLightControllerParameter( CDefine.enumLightController.LIGHT_CONTROLLER_MAIN );
                m_objPlcParameter = pDocument.m_objConfig.GetPLCParameter();
                m_objCameraParameter = pDocument.m_objConfig.GetCameraParameter( ( int )m_eCamera );
                m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCamera );
                m_dCameraResolutionDistance = 1;
                m_dCameraResolutionPixel = 100;
                GetCameraResolutionPixel();

                if( false == InitializeForm() ) break;

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
		//목적 : 폼 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool InitializeForm()
		{
			bool bReturn = false;

			do {
				// 버튼 표시
				SetButtonVisible();
				// 버튼 색상 정의
				SetButtonColor();
                //설비 타입에 따라 버튼 숨김 처리 필요
                SetChangeMachineTypeButton();
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
		//목적 : 버튼 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonVisible()
		{
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

			pFormCommon.SetButtonColor( this.BtnTitleOptionView, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnTitleOptionConfig, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnTitleSystemConfig, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnImageSave, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnPassMode, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor(this.BtnVisionTeachMode, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE);
            pFormCommon.SetButtonColor( this.BtnUseResultDialog, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnUseAutoRecipeChange, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.btnTitlePeriodImage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.btnPeriodImage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.btnTitlePeriodReport, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.btnPeriodReport, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnSaveTypeBMP, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnSaveTypeJPG, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleCamera, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnCamera1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleImage, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleDataBase, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleMachineType, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnMachineType60, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMachineType110, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMachineType150, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleMachinePosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnMachinePositionA, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMachinePositionB, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitlePLC, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnAddressPLC, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleLightContoller, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleLightControllerPort, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnLightControllerPort, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleLightControllerBaudrate, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnLightControllerBaudrate, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleRecipePath, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnRecipePath, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor(this.BtnTitleImagePath, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE);
            pFormCommon.SetButtonColor(this.BtnImagePath, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE);

            pFormCommon.SetButtonColor( this.BtnTitleCameraResolution, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleResolution, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleResolutionDistance, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleResolutionPixel, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnCameraResolution, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnResolutionDistance, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnResolutionPixel, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnLoad, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSave, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            if( CDefine.enumMachineType.PROCESS_150 == m_objSystemParameter.eMachineType && CDefine.enumCameraType.CAMERA_3D == m_objSystemParameter.eCameraType ) {
                panel3DSensor.Visible = true;
            } else {
                panel3DSensor.Visible = false;
            }
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
				SetControlChangeLanguage( this.BtnTitleOptionView );
				SetControlChangeLanguage( this.BtnTitleOptionConfig );
				SetControlChangeLanguage( this.BtnTitleSystemConfig );
				SetControlChangeLanguage( this.BtnImageSave );
				SetControlChangeLanguage( this.btnTitlePeriodImage );
				SetControlChangeLanguage( this.btnTitlePeriodReport );
				SetControlChangeLanguage( this.BtnSaveTypeBMP );
				SetControlChangeLanguage( this.BtnSaveTypeJPG );
                SetControlChangeLanguage( this.BtnTitleCamera );
                SetControlChangeLanguage( this.BtnCamera1 );
                SetControlChangeLanguage( this.BtnTitleImage );
                SetControlChangeLanguage( this.BtnTitleDataBase );
                SetControlChangeLanguage( this.BtnTitleMachineType );
                SetControlChangeLanguage( this.BtnMachineType60 );
                SetControlChangeLanguage( this.BtnMachineType110 );
                SetControlChangeLanguage( this.BtnMachineType150 );
                SetControlChangeLanguage( this.BtnTitleMachinePosition );
                SetControlChangeLanguage( this.BtnMachinePositionA );
                SetControlChangeLanguage( this.BtnMachinePositionB );
                SetControlChangeLanguage( this.BtnTitlePLC );
                SetControlChangeLanguage( this.BtnAddressPLC );
                SetControlChangeLanguage( this.BtnTitleLightContoller );
                SetControlChangeLanguage( this.BtnTitleLightControllerPort );
                SetControlChangeLanguage( this.BtnTitleLightControllerBaudrate );
                SetControlChangeLanguage( this.BtnTitleRecipePath );
                SetControlChangeLanguage(this.BtnTitleImagePath);
                SetControlChangeLanguage( this.BtnSave );
                SetControlChangeLanguage( this.BtnLoad );
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
			var pDocument = CDocument.GetDocument;
			this.Visible = bVisible;
			var pFormCommon = CFormCommon.GetFormCommon;
			if( true == bVisible ) {
				// 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
				m_delegateSetChangeButtonStatus = new DelegateSetChangeButtonStatus( pFormCommon.SetChangeButtonStatus );
				// 페이지 편집 권한에 따라 버튼 상태 변경
				pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_CONFIG );
				// 권한에따른 버튼 숨김표시 설정
				SetButtonStatus();
				// 해당 폼을 말단으로 설정
				pDocument.GetMainFrame().SetCurrentForm( this );
				// 옵션 파라미터 갱신
				m_objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                //조명파라미터 갱신
                m_objLightControllerParameter = pDocument.m_objConfig.GetLightControllerParameter( CDefine.enumLightController.LIGHT_CONTROLLER_MAIN );
                // 카메라 파라미터 갱신
                m_objCameraParameter = pDocument.m_objConfig.GetCameraParameter( ( int )m_eCamera );
                m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCamera );

            }
		}

		private void SetButtonStatus()
		{
			var pDocument = CDocument.GetDocument;
			if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER == pDocument.GetUserInformation().m_eAuthorityLevel ) {
			}
			else {
			}
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 선택 카메라 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplayCurrentCamera()
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            switch( m_eCamera ) {
                case CDefine.enumCamera.CAMERA_1:
                    pFormCommon.SetButtonBackColor( this.BtnCamera1, pFormCommon.COLOR_ACTIVATE );
                    break;
             
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 시스템 옵션 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisplaySystemConfig()
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            var pDocument = CDocument.GetDocument;

            // 이미지 삭제기간 표시
            pFormCommon.SetButtonText( this.btnPeriodImage, string.Format( "{0:D}", m_objSystemParameter.iPeriodImage ) );
            // 레포트 삭제기간 표시
            pFormCommon.SetButtonText( this.btnPeriodReport, string.Format( "{0:D}", m_objSystemParameter.iPeriodDatabase ) );

            // 이미지 저장 사용
            SetUseColor( this.BtnImageSave, m_objSystemParameter.bImageSave );

            if ( CDefine.enumSaveImageType.TYPE_BMP == m_objSystemParameter.eSaveImageType )
                SetUseColor( this.BtnSaveTypeBMP, true );
            else
                SetUseColor( this.BtnSaveTypeBMP, false );

            if ( CDefine.enumSaveImageType.TYPE_JPG == m_objSystemParameter.eSaveImageType )
                SetUseColor( this.BtnSaveTypeJPG, true );
            else
                SetUseColor( this.BtnSaveTypeJPG, false );

            if( CDefine.enumMachineType.PROCESS_60 == m_objSystemParameter.eMachineType )
                SetUseColor( this.BtnMachineType60, true );
            else
                SetUseColor( this.BtnMachineType60, false );

            if ( CDefine.enumMachineType.PROCESS_110 == m_objSystemParameter.eMachineType )
                SetUseColor( this.BtnMachineType110, true );
            else
                SetUseColor( this.BtnMachineType110, false );

            if ( CDefine.enumMachineType.PROCESS_150 == m_objSystemParameter.eMachineType )
                SetUseColor( this.BtnMachineType150, true );
            else
                SetUseColor( this.BtnMachineType150, false );

            if ( CDefine.enumMachinePosition.POSITION_A == m_objSystemParameter.eMachinePosition )
                SetUseColor( this.BtnMachinePositionA, true );
            else
                SetUseColor( this.BtnMachinePositionA, false );

            if ( CDefine.enumMachinePosition.POSITION_B == m_objSystemParameter.eMachinePosition )
                SetUseColor( this.BtnMachinePositionB, true );
            else
                SetUseColor( this.BtnMachinePositionB, false );

            if( true == m_objSystemParameter.bPassMode )
                SetUseColor( this.BtnPassMode, true );
            else
                SetUseColor( this.BtnPassMode, false );

            if( true == m_objSystemParameter.bVidiTeachMode )
                SetUseColor( this.BtnVisionTeachMode, true );
            else
                SetUseColor( this.BtnVisionTeachMode, false );

            if (true == m_objSystemParameter.bUseResultDialog)
                SetUseColor(this.BtnUseResultDialog, true);
            else
                SetUseColor(this.BtnUseResultDialog, false);

            if( true == m_objSystemParameter.bUseAutoRecipeChange )
                SetUseColor( this.BtnUseAutoRecipeChange, true );
            else
                SetUseColor( this.BtnUseAutoRecipeChange, false );

            pFormCommon.SetButtonText( this.BtnAddressPLC, string.Format( "{0} : {1}", m_objPlcParameter.strSocketIPAddress, m_objPlcParameter.iSocketPortNumber ) );
            pFormCommon.SetButtonText( this.BtnLightControllerPort, string.Format( "{0}", m_objLightControllerParameter.strSerialPortName ) );
            pFormCommon.SetButtonText( this.BtnLightControllerBaudrate, string.Format( "{0:D}", m_objLightControllerParameter.iSerialPortBaudrate ) );
            pFormCommon.SetButtonText( this.BtnRecipePath, string.Format( "{0}", m_objSystemParameter.strRecipePath ) );
            pFormCommon.SetButtonText(this.BtnImagePath, string.Format("{0}", m_objSystemParameter.strImageSavePath));
            pFormCommon.SetButtonText( this.BtnCameraResolution, string.Format( "{0:F5}", m_objCameraParameter.dResolution ) );
            pFormCommon.SetButtonText( this.BtnResolutionDistance, string.Format( "{0:F3}", m_dCameraResolutionDistance ) );
            pFormCommon.SetButtonText( this.BtnResolutionPixel, string.Format( "{0:F3}", m_dCameraResolutionPixel ) );




        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer_Tick( object sender, EventArgs e )
		{
            SetDisplayCurrentCamera();
            SetDisplaySystemConfig();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Use On Off
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetUseColor( Button objButton, bool bUse )
		{
			var pFormCommon = CFormCommon.GetFormCommon;
			if( true == bUse ) {
				pFormCommon.SetButtonBackColor( objButton, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( objButton, pFormCommon.COLOR_UNACTIVATE );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 저장 유무
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnImageSave_Click( object sender, EventArgs e )
		{
            m_objSystemParameter.bImageSave = !m_objSystemParameter.bImageSave;
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
			pDocument.m_objConfig.SaveSystemParameter( m_objSystemParameter );
            pDocument.m_objConfig.SaveRecipeParameter( ( int )m_eCamera, m_objRecipeParameter );
            CConfig.CDatabaseParameter objDatabaseParameter = pDocument.m_objConfig.GetDatabaseParameter();
			objDatabaseParameter.iDeletePeriodAlign = m_objSystemParameter.iPeriodDatabase;
			pDocument.m_objConfig.SaveDatabaseParameter( objDatabaseParameter );
            pDocument.m_objConfig.SaveCameraParameter( ( int )m_eCamera, m_objCameraParameter );
			// 저장이 완료되었습니다.
			pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10014 );
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
            // 옵션& 시스템 파라미터 로드
            m_objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
            //조명파라미터 갱신
            m_objLightControllerParameter = pDocument.m_objConfig.GetLightControllerParameter( CDefine.enumLightController.LIGHT_CONTROLLER_MAIN );
            // 카메라 파라미터 갱신
            m_objCameraParameter = pDocument.m_objConfig.GetCameraParameter( ( int )m_eCamera );
            //레시피 파라미터 갱신
            m_objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )m_eCamera );
            // 불러오기가 완료되었습니다.
            pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10015 );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 저장 기간
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void btnPeriodImage_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [{1}]", "btnPeriodImage_Click", true );
			pDocument.SetUpdateButtonLog( this, strLog );

			FormKeyPad objKeyPad = new FormKeyPad( m_objSystemParameter.iPeriodImage );
			if( DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objSystemParameter.iPeriodImage = ( int )objKeyPad.m_dResultValue;
				strLog = string.Format( "[{0}] [iPeriodImage : {1:F3}] [{2}]", "btnPeriodImage_Click", m_objSystemParameter.iPeriodImage, false );
			}

			// 버튼 로그 추가
			pDocument.SetUpdateButtonLog( this, strLog );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 레포트 저장 기간
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void btnPeriodReport_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [{1}]", "btnPeriodReport_Click", true );
			pDocument.SetUpdateButtonLog( this, strLog );

			FormKeyPad objKeyPad = new FormKeyPad( m_objSystemParameter.iPeriodDatabase );
			if( DialogResult.OK == objKeyPad.ShowDialog() ) {
                m_objSystemParameter.iPeriodDatabase = ( int )objKeyPad.m_dResultValue;
				strLog = string.Format( "[{0}] [iPeriodReport : {1:F3}] [{2}]", "btnPeriodReport_Click", m_objSystemParameter.iPeriodDatabase, false );
			}

			// 버튼 로그 추가
			pDocument.SetUpdateButtonLog( this, strLog );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 저장 타입
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnSaveTypeBMP_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [{1}]", "BtnSaveTypeBMP_Click", true );
			pDocument.SetUpdateButtonLog( this, strLog );

            m_objSystemParameter.eSaveImageType = CDefine.enumSaveImageType.TYPE_BMP;
		}

		private void BtnSaveTypeJPG_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [{1}]", "BtnSaveTypeJPG_Click", true );
			pDocument.SetUpdateButtonLog( this, strLog );

            m_objSystemParameter.eSaveImageType = CDefine.enumSaveImageType.TYPE_JPG;
		}

	

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCamera1_Click( object sender, EventArgs e )
        {
            m_eCamera = CDefine.enumCamera.CAMERA_1;
            var pDocument = CDocument.GetDocument;

        }

        private void BtnPassMode_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnPassMode_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            m_objSystemParameter.bPassMode = !m_objSystemParameter.bPassMode;

            strLog = string.Format( "[{0}] [{1}]", "PASS MODE", m_objSystemParameter.bPassMode );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        private void BtnLightControllerPort_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                
                FormKeyBoard objKey = new FormKeyBoard( m_objLightControllerParameter.strSerialPortName );
                if( DialogResult.OK != objKey.ShowDialog() )    break;
                if( 4 > objKey.m_strReturnValue.Length )     break;
                if( -1 == objKey.m_strReturnValue.IndexOf( "COM" ) ) break;

                string strPortNumber = objKey.m_strReturnValue.ToUpper();
                // 조명 컨트롤러 생성 & 초기화
                {
                    for( int iLoopCount = 0; iLoopCount < pDocument.m_objProcessMain.m_objLightController.Length; iLoopCount++ ) {
                        pDocument.m_objProcessMain.m_objLightController[ iLoopCount ].HLDeInitialize();
                        HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter objLightControllerInitializeParameter;
                        // 조명 컨트롤러 파라미터 정보 설정
                        objLightControllerInitializeParameter = new HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter();
                        objLightControllerInitializeParameter.eType = ( HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter.enumType )m_objLightControllerParameter.eType;
                        objLightControllerInitializeParameter.strSerialPortName = m_objLightControllerParameter.strSerialPortName = strPortNumber;
                        objLightControllerInitializeParameter.iSerialPortBaudrate = m_objLightControllerParameter.iSerialPortBaudrate;
                        objLightControllerInitializeParameter.iSerialPortDataBits = m_objLightControllerParameter.iSerialPortDataBits;
                        objLightControllerInitializeParameter.eParity = ( HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter.enumSerialPortParity )m_objLightControllerParameter.eParity;
                        objLightControllerInitializeParameter.eStopBits = ( HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter.enumSerialPortStopBits )m_objLightControllerParameter.eStopBits;
                        objLightControllerInitializeParameter.strSocketIPAddress = m_objLightControllerParameter.strSocketIPAddress;
                        objLightControllerInitializeParameter.iSocketPortNumber = m_objLightControllerParameter.iSocketPortNumber;
                        pDocument.m_objConfig.SaveLightControllerParameter( m_objLightControllerParameter, ( CDefine.enumLightController )iLoopCount );

                        // 조명 컨트롤러 객체 초기화
                        pDocument.m_objProcessMain.m_objLightController[ iLoopCount ].HLInitialize( objLightControllerInitializeParameter );
                    }
                }
            } while( false );
            
        }

        private void BtnVisionTeachMode_Click(object sender, EventArgs e)
        {
            var pDocument = CDocument.GetDocument;
            if( CDefine.enumRunMode.RUN_MODE_STOP != pDocument.GetRunMode() ) return;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnVisionTeachMode_Click", true);
            pDocument.SetUpdateButtonLog(this, strLog);

            m_objSystemParameter.bVidiTeachMode = !m_objSystemParameter.bVidiTeachMode;

            strLog = string.Format("[{0}] [{1}]", "Vision Teach Mode", m_objSystemParameter.bVidiTeachMode);
            pDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnCameraResolution_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnCameraResolution_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            
            FormKeyPad objKey = new FormKeyPad( m_objCameraParameter.dResolution );
            if( DialogResult.OK == objKey.ShowDialog() ) {

                strLog = string.Format( "[{0}] : [{1:F5}] -> [{2:F5}]", "Camera Resolution", m_objCameraParameter.dResolution, objKey.m_dResultValue );
                m_objCameraParameter.dResolution = objKey.m_dResultValue;
                pDocument.SetUpdateButtonLog( this, strLog );
                GetCameraResolutionPixel();
            }
        }

        private void BtnResolutionDistance_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnResolutionDistance_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );


            FormKeyPad objKey = new FormKeyPad( m_objCameraParameter.dResolution );
            if( DialogResult.OK == objKey.ShowDialog() ) {

                strLog = string.Format( "[{0}] : [{1:F3}]", "Camera Distance", objKey.m_dResultValue );
                m_dCameraResolutionDistance = objKey.m_dResultValue;
                pDocument.SetUpdateButtonLog( this, strLog );
                GetCameraResolution();
            }
        }

        private void BtnResolutionPixel_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnResolutionPixel_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );


            FormKeyPad objKey = new FormKeyPad( m_objCameraParameter.dResolution );
            if( DialogResult.OK == objKey.ShowDialog() ) {

                strLog = string.Format( "[{0}] : [{1:F3}]", "Camera Pixel", objKey.m_dResultValue );
                m_dCameraResolutionPixel = objKey.m_dResultValue;
                pDocument.SetUpdateButtonLog( this, strLog );
                GetCameraResolution();
            }
        }

        private double GetCameraResolution()
        {
            m_objCameraParameter.dResolution = m_dCameraResolutionDistance / m_dCameraResolutionPixel;
            return m_objCameraParameter.dResolution;
        }

        private double GetCameraResolutionDistance()
        {
            m_dCameraResolutionDistance = m_objCameraParameter.dResolution * m_dCameraResolutionPixel;
            return m_dCameraResolutionDistance;
        }

        private double GetCameraResolutionPixel()
        {
            m_dCameraResolutionPixel = m_dCameraResolutionDistance / m_objCameraParameter.dResolution;
            return 0;
        }

        private void Btn3DThreasholdMax_Click( object sender, EventArgs e )
        {
            //var pDocument = CDocument.GetDocument;
            //// 버튼 로그 추가
            //string strLog = string.Format( "[{0}] [{1}]", "Btn3DThreasholdMax_Click", true );
            //pDocument.SetUpdateButtonLog( this, strLog );

            //FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter.d3DHeightThresholdMax );
            //if( DialogResult.OK == objKeyPad.ShowDialog() ) {
            //    m_objRecipeParameter.d3DHeightThresholdMax = objKeyPad.m_dResultValue;
            //    strLog = string.Format( "[{0}] [d3DHeightThresholdMax : {1:F3}] [{2}]", "Btn3DThreasholdMax_Click", m_objRecipeParameter.d3DHeightThresholdMax, false );
            //}

            //// 버튼 로그 추가
            //pDocument.SetUpdateButtonLog( this, strLog );
        }

        private void Btn3DThreasholdMin_Click( object sender, EventArgs e )
        {
            //var pDocument = CDocument.GetDocument;
            //// 버튼 로그 추가
            //string strLog = string.Format( "[{0}] [{1}]", "Btn3DThreasholdMin_Click", true );
            //pDocument.SetUpdateButtonLog( this, strLog );

            //FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter.d3DHeightThresholdMin );
            //if( DialogResult.OK == objKeyPad.ShowDialog() ) {
            //    m_objRecipeParameter.d3DHeightThresholdMin = objKeyPad.m_dResultValue;
            //    strLog = string.Format( "[{0}] [d3DHeightThresholdMin : {1:F3}] [{2}]", "Btn3DThreasholdMin_Click", m_objRecipeParameter.d3DHeightThresholdMin, false );
            //}

            //// 버튼 로그 추가
            //pDocument.SetUpdateButtonLog( this, strLog );
        }

        private void Btn3DThreasholdCount_Click( object sender, EventArgs e )
        {
            //var pDocument = CDocument.GetDocument;
            //// 버튼 로그 추가
            //string strLog = string.Format( "[{0}] [{1}]", "Btn3DThreasholdCount_Click", true );
            //pDocument.SetUpdateButtonLog( this, strLog );

            //FormKeyPad objKeyPad = new FormKeyPad( m_objRecipeParameter.i3DHeightThresholdCount );
            //if( DialogResult.OK == objKeyPad.ShowDialog() ) {
            //    m_objRecipeParameter.i3DHeightThresholdCount = ( int )objKeyPad.m_dResultValue;
            //    strLog = string.Format( "[{0}] [d3DHeightThresholdCount : {1:F3}] [{2}]", "Btn3DThreasholdCount_Click", m_objRecipeParameter.i3DHeightThresholdCount, false );
            //}

            //// 버튼 로그 추가
            //pDocument.SetUpdateButtonLog( this, strLog );
        }

        private void BtnUseResultDialog_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnUseResultDialog_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            m_objSystemParameter.bUseResultDialog = !m_objSystemParameter.bUseResultDialog;

            strLog = string.Format( "[{0}] [{1}]", "USE RESULT DIALOG", m_objSystemParameter.bPassMode );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        private void BtnUseAutoRecipeChange_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnUseAutoRecipeChange_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            m_objSystemParameter.bUseAutoRecipeChange = !m_objSystemParameter.bUseAutoRecipeChange;

            strLog = string.Format( "[{0}] [{1}]", "USE AUTO RECIPE", m_objSystemParameter.bUseAutoRecipeChange );
            pDocument.SetUpdateButtonLog( this, strLog );
        }
    }
}