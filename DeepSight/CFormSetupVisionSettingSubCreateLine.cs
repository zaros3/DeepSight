﻿using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSight
{
	public partial class CFormSetupVisionSettingSubCreateLine : Form, CFormInterface
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 메인 코그 디스플레이
		private CogDisplay m_objCogDisplayMain;
		// 라인 생성 툴
		private CogCreateLineTool m_objCreateLineTool;
		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CFormSetupVisionSettingSubCreateLine()
		{
			InitializeComponent();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormSetupVisionSettingSubCreateLine_Load( object sender, EventArgs e )
		{
			// Initialize 함수보다 시점 늦음
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 종료
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormSetupVisionSettingSubCreateLine_FormClosed( object sender, FormClosedEventArgs e )
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
		public bool Initialize( CogDisplay objCogDisplay )
		{
			bool bReturn = false;

			do {
				// 메인 코그 디스플레이 연결
				m_objCogDisplayMain = objCogDisplay;
				// 초기화 언어 변경
				SetChangeLanguage();
				// 버튼 이벤트 로그 정의
				SetButtonEventChange();
				// 버튼 색상 정의
				SetButtonColor();
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
			cogDisplayRunImage.InteractiveGraphics.Dispose();
			cogDisplayRunImage.Dispose();
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
			pFormCommon.SetButtonColor( this.BtnTitleCreateLineSetting, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnImageGrab, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleLineDegree, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnLineDegree, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 버튼 이벤트 로그 추가
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonEventChange()
		{
			SetButtonEventChange( this.BtnImageGrab );
			SetButtonEventChange( this.BtnLineDegree );
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
				SetControlChangeLanguage( this.BtnTitleCreateLineSetting );
				SetControlChangeLanguage( this.BtnImageGrab );
				SetControlChangeLanguage( this.BtnTitleLineDegree );
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
// 			CVisionLibraryCogCreateLine obj = new CVisionLibraryCogCreateLine();
// 			obj.HLInitialize( objInitialize );
// 			obj.m_objCreateLineTool = this.m_objCreateLineTool;
// 			obj.HLSaveRecipe( objInitialize.strRecipePath, objInitialize.strRecipeName );
// 		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 툴 적용 ( 외부 -> 내부 )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetTool( CogCreateLineTool objCreateLineTool )
		{
			m_objCreateLineTool = objCreateLineTool;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 툴 반환 ( 내부 -> 외부 )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CogCreateLineTool GetTool()
		{
			return m_objCreateLineTool;
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
				if( null == m_objCreateLineTool.InputImage ) break;
				// 이미지 null 체크 말고도.. 이미지 할당 유무 체크해야 함.
				if( false == m_objCreateLineTool.InputImage.Allocated ) break;

				m_objCogDisplayMain.Image = m_objCreateLineTool.InputImage;

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
				if( null == m_objCreateLineTool ) break;
				// 라인 각도
				pFormCommon.SetButtonText( this.BtnLineDegree, string.Format( "{0:F0}", m_objCreateLineTool.Line.Rotation * ( 180 / Math.PI ) ) );
				
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 그랩
		//설명 : 실행 디스플레이에 이미지를 메인 디스플레이에 복사함
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnImageGrab_Click( object sender, EventArgs e )
		{
			do {
				if( null == cogDisplayRunImage.Image ) break;

				m_objCogDisplayMain.Image = cogDisplayRunImage.Image;
				m_objCreateLineTool.InputImage = cogDisplayRunImage.Image as CogImage8Grey;
				GetLine();

			} while( false );
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 툴 내 라인 그래픽 -> 현재 디스플레이에 표시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GetLine()
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
                // 라인 생성
                SetCreateLine();

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라인 설정
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetCreateLine()
        {
            do {
                // 메인 디스플레이에 이미지 있는 경우에만
                if( null == m_objCogDisplayMain.Image ) break;

                CogLine obj = m_objCreateLineTool.Line;
                // 없는 경우 새로 생성
                if( null == obj ) {
                    obj = new CogLine();
                    obj.Interactive = true;
                    obj.Color = CogColorConstants.Green;
                }
                m_objCogDisplayMain.InteractiveGraphics.Add( obj, "", false );

            } while( false );
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 각도 입력
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLineDegree_Click( object sender, EventArgs e )
        {
			// rad -> deg
            FormKeyPad objKeyPad = new FormKeyPad( ( double )( m_objCreateLineTool.Line.Rotation * ( 180 / Math.PI ) ) );
            if( System.Windows.Forms.DialogResult.OK == objKeyPad.ShowDialog() ) {
				// deg -> rad
                m_objCreateLineTool.Line.Rotation = objKeyPad.m_dResultValue* ( Math.PI / 180 );
                // 라인 생성 그래픽 받아옴
                GetLine();
            }
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : ocx
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnTitleCreateLineSetting_Click( object sender, EventArgs e )
		{
            //var pDocument = CDocument.GetDocument;

            //do {
            //    CUserInformation objUserInformation = pDocument.GetUserInformation();
            //    // 마스터만 가능하도록
            //    if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER != objUserInformation.m_eAuthorityLevel ) break;

            //    // ocx 넣기 전에 이미지 변경해줌
            //    ICogImage objOriginImage = m_objCreateLineTool.InputImage;
            //    m_objCreateLineTool.InputImage = cogDisplayRunImage.Image;
            //    CDialogCogCreateLine obj = new CDialogCogCreateLine( m_objCreateLineTool );
            //    obj.ShowDialog();
            //    // 빠져나오면서 원본 이미지 집어넣음
            //    m_objCreateLineTool.InputImage = objOriginImage;

            //} while( false );
		}
	}
}