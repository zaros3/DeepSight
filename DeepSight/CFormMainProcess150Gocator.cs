using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace DeepSight
{
	public partial class CFormMainProcess150Gocator : Form, CFormInterface
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// enumeration
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private enum enumAlignDataListColumn
		{
			INDEX,
			X1, Y1, T1,
			X2, Y2, T2,
			X3, Y3, T3,
			X4, Y4, T4,
			ALIGN_DATA_LIST_FINAL,
		};

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 디스플레이 패널
		private Panel[] m_objPanelDisplay;
		// 폼 디스플레이 화면
		private Form[] m_objFormDisplay;
		// 폼 디스플레이 확대 화면
		private CFormDisplayChangeSize m_objFormDisplayChangeSize;
		// 디스플레이 사이즈 ( 원본 )
		private Size[] m_objDisplaySize;
		// 디스플레이 위치 ( 원본 )
		private Point[] m_objDisplayLocation;
		// 리스트 박스
		private ListBox[] m_objListLog;
		// 타입별 카메라 수량
		private int m_iCameraCount;
        // 디스플레이 수량
        private int m_iDisplayIndex;

        public enum enumDisplayIndex { IMAGE, VIDI_1, VIDI_2, VIDI_3, VIDI_4, VIDI_5, VIDI_6, MEASURE_1, MEASURE_2, MEASURE_3, MEASURE_4, MEASURE_5, MEASURE_6, DISPLAY_FINAL };
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 로그 업데이트 델리게이트
        public delegate void DelegateUpdateLog( int iCameraIndex, string strLog );
		// 화면 사이즈 변경 델리게이트
		public delegate void DelegateChangeDisplaySize( int iCameraIndex );
		
        // 검사 결과 ( 디스플레이 & 데이터 갱신 )
		public delegate void DelegateUpdateDisplay( enumDisplayIndex eDisplayIndex, CInspectionResult.CResult objResult );
        public DelegateUpdateDisplay m_delegateUpdateDisplayResultVIDI;
        public DelegateUpdateDisplay m_delegateUpdateDisplayResultMeasure;
        public DelegateUpdateDisplay m_delegateUpdateDisplay3D;

        public delegate void DelegateUpdateResult( int iPosition, bool bResult );
        public DelegateUpdateResult m_delegateUpdateResult;


        // 라이브 결과 ( 디스플레이 )
        public delegate void DelegateUpdateDisplayLive( int iCameraIndex, Cognex.VisionPro.CogImage8Grey objLiveImage );
		public DelegateUpdateDisplayLive m_delegateUpdateDisplayLive;
		// 이미지 저장
		public delegate void DelegateSaveImage( int iCameraIndex, CInspectionResult.CResult objResult );
		public DelegateSaveImage m_delegateSaveImage;
		
        // 얼라인 보정값 갱신
		public delegate void DelegateUpdateAlignLog( int iCameraIndex, int iProductIndex, double dX, double dY, double dT );
		// 유저 권한 레벨에 따른 버튼 상태 변경
		public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
		public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;

        CFormResourceInfo m_objResourceInfo;

        CDialogResult m_objDialogResult;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormMainProcess150Gocator()
		{
			InitializeComponent();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 폼 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CFormMainLoader_Load( object sender, EventArgs e )
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
		private void CFormMainLoader_FormClosed( object sender, FormClosedEventArgs e )
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
				// 폼 초기화
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
			// 폼 디스플레이 해제
			for( int iLoopCount = 0; iLoopCount < m_objFormDisplay.Length; iLoopCount++ ) {
				if( null != m_objFormDisplay[ iLoopCount ] ) {
					( m_objFormDisplay[ iLoopCount ] as CFormDisplay ).DeInitialize();
				}
			}
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
				var pDocument = CDocument.GetDocument;
				m_iCameraCount = ( int )CDefine.enumCamera.CAMERA_FINAL;
                // 검사 결과 ( 디스플레이 & 데이터 갱신 )
                m_delegateUpdateDisplayResultVIDI = new DelegateUpdateDisplay( UpdateDisplayResultVIDI );
                m_delegateUpdateDisplayResultMeasure = new DelegateUpdateDisplay( UpdateDisplayResultMeasure );
                m_delegateUpdateDisplay3D = new DelegateUpdateDisplay( UpdateDisplay3D );

                m_delegateUpdateResult = new DelegateUpdateResult( UpdateResult );
                // 라이브 결과 ( 디스플레이 )
                m_delegateUpdateDisplayLive = new DelegateUpdateDisplayLive( UpdateDisplayLive );
				// 이미지 저장
				m_delegateSaveImage = new DelegateSaveImage( SaveImage );
				// 리스트 로그
				m_objListLog = new ListBox[ m_iCameraCount ];
				m_objListLog[ ( int )CDefine.enumCamera.CAMERA_1 ] = this.listLogInputTray1;

                m_objResourceInfo = new CFormResourceInfo();
                SetFormDockStyle( m_objResourceInfo, panelResourceInfo );

                // 디스플레이 화면 생성
                {
                    m_iDisplayIndex = ( int )enumDisplayIndex.DISPLAY_FINAL;
                    m_objPanelDisplay = new Panel[ m_iDisplayIndex ];
					m_objPanelDisplay[ ( int )enumDisplayIndex.IMAGE ] = this.panelDisplayPMS;

                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_1 ] = this.panelDisplayVidi1;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_2 ] = this.panelDisplayVidi2;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_3 ] = this.panelDisplayVidi3;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_4 ] = this.panelDisplayVidi4;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_5 ] = this.panelDisplayVidi5;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_6 ] = this.panelDisplayVidi6;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_1 ] = this.panelDisplayMeasure1;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_2 ] = this.panelDisplayMeasure2;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_3 ] = this.panelDisplayMeasure3;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_4] = this.panelDisplayMeasure4;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_5 ] = this.panelDisplayMeasure5;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_6 ] = this.panelDisplayMeasure6;

                    // 폼 디스플레이 생성
                    m_objFormDisplay = new Form[ m_iDisplayIndex ];
					m_objDisplaySize = new Size[ m_iDisplayIndex ];
					m_objDisplayLocation = new Point[ m_iDisplayIndex ];

                    string[] strDisplayName = new string[ m_objFormDisplay.Length ];
                    strDisplayName[ ( int )enumDisplayIndex.IMAGE ] = enumDisplayIndex.IMAGE.ToString();

                    strDisplayName[ ( int )enumDisplayIndex.VIDI_1 ] = enumDisplayIndex.VIDI_1.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.VIDI_2 ] = enumDisplayIndex.VIDI_2.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.VIDI_3 ] = enumDisplayIndex.VIDI_3.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.VIDI_4 ] = enumDisplayIndex.VIDI_4.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.VIDI_5 ] = enumDisplayIndex.VIDI_5.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.VIDI_6 ] = enumDisplayIndex.VIDI_6.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_1 ] = enumDisplayIndex.MEASURE_1.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_2 ] = enumDisplayIndex.MEASURE_2.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_3 ] = enumDisplayIndex.MEASURE_3.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_4 ] = enumDisplayIndex.MEASURE_4.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_5 ] = enumDisplayIndex.MEASURE_5.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_6 ] = enumDisplayIndex.MEASURE_6.ToString();

                    

                    for ( int iLoopCount = 0; iLoopCount < m_objFormDisplay.Length; iLoopCount++ ) {
						// 디스플레이 화면 생성 & 초기화
						CFormDisplay objForm = new CFormDisplay();
                        int iImageIndex = 0;
                        if( ( int )enumDisplayIndex.VIDI_1 <= iLoopCount && ( int )enumDisplayIndex.MEASURE_1 > iLoopCount )
                            iImageIndex = iLoopCount - ( int )enumDisplayIndex.VIDI_1;
                        else if( ( int )enumDisplayIndex.MEASURE_1 <= iLoopCount )
                            iImageIndex = iLoopCount - ( int )enumDisplayIndex.MEASURE_1;

                        objForm.Initialize( ( int )CDefine.enumCamera.CAMERA_1, string.Format( "{0}", strDisplayName[ iLoopCount ] ), false, iImageIndex );
                        objForm.SetDisplayIndex( iLoopCount );
                        switch((enumDisplayIndex)iLoopCount)
                        {
                            case enumDisplayIndex.MEASURE_1:
                            case enumDisplayIndex.MEASURE_2:
                            case enumDisplayIndex.MEASURE_3:
                            case enumDisplayIndex.MEASURE_4:
                            case enumDisplayIndex.MEASURE_5:
                            case enumDisplayIndex.MEASURE_6:
                                objForm.UseMeasureInfo3D();
                                break;
                            default:
                                break;
                        }

                        objForm.Visible = true;
						objForm.SetTimer( true );
						// 사이즈 조정
						Panel objPanel = m_objPanelDisplay[ iLoopCount ];
						objForm.SetSize( objPanel.Location.X, objPanel.Location.Y, objPanel.Width, objPanel.Height, true );
						// 디스플레이 사이즈 변경 이벤트 추가
						objForm.EventChangeSizeDisplay += new CFormDisplay.DelegateChangeSizeDisplay( ChangeDisplaySize );
						// 패널에 화면 붙임
						SetFormDockStyle( objForm, objPanel );
						m_objFormDisplay[ iLoopCount ] = objForm;
						// 원본 패널 사이즈 및 위치 저장
						m_objDisplaySize[ iLoopCount ] = m_objPanelDisplay[ iLoopCount ].Size;
						m_objDisplayLocation[ iLoopCount ] = m_objPanelDisplay[ iLoopCount ].Location;
					}
				}

				// 디스플레이 확대 화면 생성
				m_objFormDisplayChangeSize = new CFormDisplayChangeSize();
				
				// 버튼 색상 정의
				SetButtonColor();
				// 초기화 언어 변경
				SetChangeLanguage();
				// 버튼 이벤트 로그 정의
				SetButtonEventChange();
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
		//목적 : 버튼 색상 정의
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonColor()
		{
			var pFormCommon = CFormCommon.GetFormCommon;
			this.BackColor = pFormCommon.COLOR_FORM_VIEW;
			// 버튼
			pFormCommon.SetButtonColor( this.BtnTitleOperation, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnStart, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnStop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnTitleCameraAction, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnTriggerInputTray1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
			pFormCommon.SetButtonColor( this.BtnLiveInputTray1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitleInspectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnInspectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            // 리스트
            for( int iLoopCount = 0; iLoopCount < m_objListLog.Length; iLoopCount++ ) {
				m_objListLog[ iLoopCount ].BackColor = pFormCommon.COLOR_TITLE;
				m_objListLog[ iLoopCount ].ForeColor = pFormCommon.COLOR_WHITE;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 버튼 이벤트 로그 추가
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetButtonEventChange()
		{
			SetButtonEventChange( this.BtnStart );
			SetButtonEventChange( this.BtnStop );
			SetButtonEventChange( this.BtnTriggerInputTray1 );
			SetButtonEventChange( this.BtnLiveInputTray1 );
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
				SetControlChangeLanguage( this.BtnTitleOperation );
				SetControlChangeLanguage( this.BtnStart );
				SetControlChangeLanguage( this.BtnStop );
				SetControlChangeLanguage( this.BtnTitleCameraAction );
				SetControlChangeLanguage( this.BtnTriggerInputTray1 );
				SetControlChangeLanguage( this.BtnLiveInputTray1 );

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

			if( true == bVisible ) {
				var pFormCommon = CFormCommon.GetFormCommon;
				// 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
				m_delegateSetChangeButtonStatus = new DelegateSetChangeButtonStatus( pFormCommon.SetChangeButtonStatus );
				// 페이지 편집 권한에 따라 버튼 상태 변경
				pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_MAIN );
				// 해당 폼을 말단으로 설정
				pDocument.GetMainFrame().SetCurrentForm( this );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetControlEnable()
		{
			var pDocument = CDocument.GetDocument;
			if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) {

			}
			else {

			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 메인 화면 리스트에 로그 표시
		//설명 : 비동기 호출
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetUpdateLog( int iCameraIndex, string strLog )
		{
			this.BeginInvoke( new DelegateUpdateLog( UpdateLog ), new object[] { iCameraIndex, strLog } );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 메인 화면 리스트에 얼라인 로그 표시
		//설명 : 비동기 호출
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetUpdateAlignLog( int iCameraIndex, int iProductIndex, double dX, double dY, double dT )
		{
			this.BeginInvoke( new DelegateUpdateAlignLog( UpdateAlignLog ), new object[] { iCameraIndex, iProductIndex, dX, dY, dT } );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 디스플레이 사이즈 변경
		//설명 : 비동기 호출
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetChangeDisplaySize( int iCameraIndex )
		{
			this.BeginInvoke( new DelegateChangeDisplaySize( ChangeDisplaySize ), new object[] { iCameraIndex } );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void UpdateLog( int iCameraIndex, string strLog )
		{
			do {
				if( iCameraIndex >= m_objListLog.Length ) break;

				ListBox obj = m_objListLog[ iCameraIndex ];
				obj.Items.Insert( 0, strLog );
				if( 60 < obj.Items.Count ) {
					obj.Items.RemoveAt( obj.Items.Count - 1 );
				}
				obj.SetSelected( 0, true );

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void UpdateAlignLog( int iCameraIndex, int iProductIndex, double dX, double dY, double dT )
		{
			var pDocument = CDocument.GetDocument;

			try {
			}
			catch( Exception ex ) {
				pDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "CFormMainLoader UpdateAlignLog " + ex.Message );	
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 디스플레이 사이즈 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void ChangeDisplaySize( int iCameraIndex )
		{
			var pDocument = CDocument.GetDocument;

			do {
                // 인덱스 예외 처리
                if(m_iDisplayIndex <= iCameraIndex) break;
                // 디스플레이 축소
                Panel objPanel = m_objPanelDisplay[ iCameraIndex ];
				if( objPanel.Size == m_objFormDisplayChangeSize.Size ) {
					objPanel.Size = m_objDisplaySize[ iCameraIndex ];
					objPanel.Location = m_objDisplayLocation[ iCameraIndex ];
					// 디스플레이 사이즈 조정
					( m_objFormDisplay[ iCameraIndex ] as CFormDisplay ).SetSize(
						objPanel.Location.X, objPanel.Location.Y,
						objPanel.Width, objPanel.Height, true );
					// 전체 디스플레이 다시 보여줌
					for( int iLoopCount = 0; iLoopCount < m_objPanelDisplay.Length; iLoopCount++ ) {
						m_objPanelDisplay[ iLoopCount ].Show();
					}
				}
				// 디스플레이 확대
				else {
					objPanel.Size = m_objFormDisplayChangeSize.Size;
					//objPanel.Location = m_objFormDisplayChangeSize.Location;
                    objPanel.Location = m_objPanelDisplay[ 0 ].Location;
					// 디스플레이 사이즈 조정
					( m_objFormDisplay[ iCameraIndex ] as CFormDisplay ).SetSize(
						objPanel.Location.X, objPanel.Location.Y,
						objPanel.Width, objPanel.Height, true );
					// 나머지 디스플레이 숨김
					for( int iLoopCount = 0; iLoopCount < m_objPanelDisplay.Length; iLoopCount++ ) {
						if( iCameraIndex == iLoopCount ) continue;
						m_objPanelDisplay[ iLoopCount ].Hide();
					}
				}

			} while( false );
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 검사 트리거 시 해당 델리게이트 호출
        //설명 : 디스플레이에 검사 이미지 올리는 부분은 늦어도 상관 없으므로 비동기 호출
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayResultVIDI( enumDisplayIndex eDisplayIndex, CInspectionResult.CResult objResult )
        {
            CFormDisplay obj = m_objFormDisplay[ ( int )eDisplayIndex ] as CFormDisplay;

            do {
                if( null == obj ) break;

                obj.BeginInvoke( obj.m_objDelegateUpdateDisplayVIDI, objResult );

            } while( false );
        }

        public void UpdateDisplayResultMeasure( enumDisplayIndex eDisplayIndex, CInspectionResult.CResult objResult )
        {
            CFormDisplay obj = m_objFormDisplay[ ( int )eDisplayIndex ] as CFormDisplay;

            do {
                if( null == obj ) break;

                obj.BeginInvoke( obj.m_objDelegateUpdateDisplayMeasure3D, objResult );

            } while( false );
        }

        public void UpdateDisplay3D( enumDisplayIndex eDisplayIndex, CInspectionResult.CResult objResult )
        {
            CFormDisplay obj = m_objFormDisplay[ ( int )eDisplayIndex ] as CFormDisplay;

            do
            {
                if ( null == obj ) break;

                obj.BeginInvoke( obj.m_objDelegateUpdateDisplay3D, objResult );

            } while ( false );
        }

        public void UpdateDisplayPMS( enumDisplayIndex eDisplayIndex, CInspectionResult.CResult objResult )
        {
            CFormDisplay obj = m_objFormDisplay[ ( int )eDisplayIndex ] as CFormDisplay;

            do
            {
                if ( null == obj ) break;

                obj.BeginInvoke( obj.m_objDelegateUpdateDisplayPMSImage, objResult );

            } while ( false );
        }

        public void UpdateResult( int iPosition, bool bResult )
        {
            do {
                if( false == CDocument.GetDocument.m_objConfig.GetSystemParameter().bUseResultDialog ) break;

                if( null == m_objDialogResult ) {
                    m_objDialogResult = new CDialogResult();
                    m_objDialogResult.Show();
                }

                if( false == m_objDialogResult.bShow )
                    m_objDialogResult.Show();

                m_objDialogResult.BeginInvoke( m_objDialogResult.m_delegateUpdateDisplayResult, iPosition, bResult );
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브 시 해당 델리게이트 호출
        //설명 : 동기 호출
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateDisplayLive( int iCameraIndex, Cognex.VisionPro.CogImage8Grey objLiveImage )
		{
			CFormDisplay obj = m_objFormDisplay[ iCameraIndex ] as CFormDisplay;

			do {
				if( null == obj ) break;
				// 라이브는 바로 날림
				obj.UpdateDisplay( objLiveImage );
				
			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 이미지 저장 해당 델리게이트 호출
		//설명 : 비동기 호출
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SaveImage( int iCameraIndex, CInspectionResult.CResult objResult )
		{
			var pDocument = CDocument.GetDocument;
			CFormDisplay obj = m_objFormDisplay[ iCameraIndex ] as CFormDisplay;

			do {
				if( null == obj ) break;

				if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) {
					obj.BeginInvoke( obj.m_objDelegateSaveImage, objResult, false );
				}
				else {
					obj.BeginInvoke( obj.m_objDelegateSaveImage, objResult, true );
				}

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 런 모드 시작
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnStart_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;

			do {
				if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) break;
             

                // 연결 상태 확인
                if (false == pDocument.GetCheckDevice())
                {
                    pDocument.SetUpdateLog(CDefine.enumLogType.LOG_SYSTEM, "Device Disconnect");
                    // 디바이스 연결 상태를 확인하세요
                    pDocument.SetMessage(CDefine.enumAlarmType.ALARM_INFORMATION, 10052);
                    break;
                }
                // 라이브 오프
                for (int iLoopCount = 0; iLoopCount < (int)CDefine.enumCamera.CAMERA_FINAL; iLoopCount++)
                {
                    pDocument.SetLiveMode(iLoopCount, CDefine.enumLiveMode.LIVE_MODE_OFF);
                }


                // 비전레디상태 PLC전송
                // PLC 데이터 초기화
                short[] iWriteData = new short[ ( int )CDefine.enumPCOutIndex.PC_INSPECTION_RESULT_1 ];
                // 생성과 동시에 0초기화되기때문에 오토런상태만 1로 바꿈
                iWriteData[ ( int )CDefine.enumPCOutIndex.PC_AUTO_RUN ] = ( int )CDefine.enumVisionReady.VISION_READY_ON;
                pDocument.m_objProcessMain.m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_VISION_ALIVE.ToString(), iWriteData.Length, iWriteData );

                // 티치모드는 시작시 비활성화
                CConfig.CSystemParameter objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                objSystemParameter.bVidiTeachMode = false;
                pDocument.m_objConfig.SaveSystemParameter(objSystemParameter);
                // 시작
                pDocument.SetRunMode( CDefine.enumRunMode.RUN_MODE_START );


				SetControlEnable();

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 런 모드 정지
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnStop_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
            // 비전레디상태 PLC전송
            pDocument.m_objProcessMain.m_objPLC.HLWriteWordFromPLC( "PC_AUTO_RUN", ( int )CDefine.enumVisionReady.VISION_READY_OFF );
			pDocument.SetRunMode( CDefine.enumRunMode.RUN_MODE_STOP );
			// 라이브 오프
			for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
				pDocument.SetLiveMode( iLoopCount, CDefine.enumLiveMode.LIVE_MODE_OFF );
			}

			SetControlEnable();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 런 모드 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetDisplayRunMode()
		{
			var pDocument = CDocument.GetDocument;
			var pFormCommon = CFormCommon.GetFormCommon;
			// 시작 상태
			if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) {
				pFormCommon.SetButtonBackColor( BtnStart, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnStart, pFormCommon.COLOR_UNACTIVATE );
			}
			// 정지 상태
			if( CDefine.enumRunMode.RUN_MODE_STOP == pDocument.GetRunMode() ) {
				pFormCommon.SetButtonBackColor( BtnStop, pFormCommon.COLOR_ACTIVATE );
			}
			else {
				pFormCommon.SetButtonBackColor( BtnStop, pFormCommon.COLOR_UNACTIVATE );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트리거 & 라이브 표시
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetDisplayTriggerLive()
		{
			var pDocument = CDocument.GetDocument;
			var pFormCommon = CFormCommon.GetFormCommon;
			// 트리거 ( Input Tray )

            if( CDefine.enumTrigger.TRIGGER_ON == pDocument.GetTrigger( ( int )CDefine.enumCamera.CAMERA_1 ) ) {
                pFormCommon.SetButtonBackColor( BtnTriggerInputTray1, pFormCommon.COLOR_ACTIVATE );
            } else {
                pFormCommon.SetButtonBackColor( BtnTriggerInputTray1, pFormCommon.COLOR_UNACTIVATE );
            }

			// 라이브 ( Input Tray )
            if( CDefine.enumLiveMode.LIVE_MODE_ON == pDocument.GetLiveMode( ( int )CDefine.enumCamera.CAMERA_1 ) ) {
                pFormCommon.SetButtonBackColor( BtnLiveInputTray1, pFormCommon.COLOR_ACTIVATE );
            } else {
                pFormCommon.SetButtonBackColor( BtnLiveInputTray1, pFormCommon.COLOR_UNACTIVATE );
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 타이머
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void timer_Tick( object sender, EventArgs e )
		{
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;
            // 런 모드 표시
            SetDisplayRunMode();
			// 트리거 & 라이브 표시
			SetDisplayTriggerLive();
            // 현재검사위치
            pFormCommon.SetButtonText( this.BtnInspectionPosition, ( pDocument.GetInspectionIndex() + 1 ).ToString() );
        }

		

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 트리거 ( Input Tray )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnTriggerInputTray1_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) break;
                if( CDefine.enumTrigger.TRIGGER_OFF == pDocument.GetTrigger( ( int )CDefine.enumCamera.CAMERA_1 ) ) {
                    // 트리거 온
                    pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1, CDefine.enumTrigger.TRIGGER_ON );
                } else {
                    pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1, CDefine.enumTrigger.TRIGGER_OFF );
                    var objProcess = pDocument.m_objProcessMain.m_objProcessVision.m_objProcessVisionManagerProcess150Gocator.m_objProcessVisionProcess150[ ( int )CDefine.enumCamera.CAMERA_1 ] as CProcessVisionProcess150Gocator;
                    objProcess.SetEvent( CProcessVisionProcess150Gocator.enumEvent.EVENT_GRAB  );
                    Thread.Sleep( 100 );
                    pDocument.m_objProcessMain.m_objCamera[ ( int )CDefine.enumCamera.CAMERA_1 ].HL3DStop();
                }

                
            } while( false );
        }

       
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 라이브 ( Input Tray )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLiveInputTray1_Click( object sender, EventArgs e )
        {
            CDialogChart cDialogChart = new CDialogChart();
            cDialogChart.ShowDialog();
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 탭 선택 막음
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void dataGridViewLog_Paint( object sender, PaintEventArgs e )
		{
			//( sender as DataGridView ).ClearSelection();
		}

        private void BtnInspectionPosition_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                if( CDefine.enumRunMode.RUN_MODE_START == pDocument.GetRunMode() ) break;

                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );

                string[] strButtonList = new string[ objRecipeParameter.iCountInspectionPosition ];
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.iCountInspectionPosition; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = "INSPECTION POSITION " + ( iLoopCount + 1 ).ToString();
                }

                CDialogEnumerate objDialog = new CDialogEnumerate( objRecipeParameter.iCountInspectionPosition, strButtonList, pDocument.GetInspectionIndex() );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    //if( true == pDocument.m_objConfig.GetSystemParameter().bVidiTeachMode )
                        pDocument.SetInspectionIndex( objDialog.GetResult() );
                    //pDocument.m_objProcessMain.m_objPLC.HLWriteWordFromPLC( CDefine.enumPLCInputIndex.PLC_INSPECTION_INDEX.ToString(), ( short )( objDialog.GetResult() + 1 ) );
                }


                // 여기서 결과 디스플레이를 뿌리자
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    pDocument.SetUpdateDisplayResultVIDI( objDialog.GetResult(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.VIDI_1 + iLoopCount );
                }
                // 
                // 여기서 결과 디스플레이를 뿌리자
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    pDocument.SetUpdateDisplayResultMeasure( objDialog.GetResult(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.MEASURE_1 + iLoopCount );
                }
                pDocument.SetUpdateDisplay3D( objDialog.GetResult(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.IMAGE );


            } while( false );
        }

        private void BtnTitleCameraAction_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            //pDocument.SetDisplayResultMonitorHistory( "A" );
            
            do {
                if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) break;

                try {
                    //200205.YJ 선택 불편해!
                    using (var dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog { IsFolderPicker = true })
                    {
                        if (Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok == dialog.ShowDialog())
                        {

                            if (false == CLoadingScreen.IsSplashScreen())
                            {
                                // 레시피를 로드 중입니다…
                                pDocument.GetMainFrame().ShowWaitMessage(true, "Load Simulation Image");
                            }

                            pDocument.m_objProcessMain.m_objCamera[(int)CDefine.enumCamera.CAMERA_1].HL3DSetSimulationImage(dialog.FileName);

                            pDocument.GetMainFrame().ShowWaitMessage(false, "");
                        }
                    }
                } catch( Exception ) {
                    break;

                }
                /*
             
                FolderBrowserDialog dialog1 = new FolderBrowserDialog();

                try {
                    if( System.Windows.Forms.DialogResult.OK == dialog1.ShowDialog() ) {

                        if( false == CLoadingScreen.IsSplashScreen() ) {
                            // 레시피를 로드 중입니다…
                            pDocument.GetMainFrame().ShowWaitMessage( true, "Load Simulation Image" );
                        }

                        pDocument.m_objProcessMain.m_objCamera[ ( int )CDefine.enumCamera.CAMERA_1 ].HL3DSetSimulationImage( dialog1.SelectedPath );

                        pDocument.GetMainFrame().ShowWaitMessage( false, "" );
                    }
                } catch( Exception ) {
                    break;

                }    
             */
            } while ( false );
        }
    }
}