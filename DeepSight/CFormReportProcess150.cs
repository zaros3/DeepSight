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
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DeepSight
{
    public partial class CFormReportProcess150 : Form, CFormInterface
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 검색할 양불 타입
		private enum enumResultType
		{
			NG = 0,
			OK,
			ALL,
			RESULT_TYPE_FINAL
		}

		// 현재 양불 타입
		private enumResultType m_eResultType;
        
		// 해당 날짜로 검색된 데이터를 갖고 있음
		private DataTable m_objDataTable;
		private DataRow[] m_objDataRow;
        // 이미지 뷰어
        // 디스플레이 패널
        private Panel[] m_objPanelDisplay;
        // 폼 디스플레이 화면
        private Form[] m_objFormDisplay;
        // 디스플레이 수량
        private int m_iDisplayIndex;
        public enum enumDisplayIndex { PMS, VIDI_1, VIDI_2, VIDI_3, VIDI_4, VIDI_5, VIDI_6, MEASURE_1, MEASURE_2, MEASURE_3, MEASURE_4, MEASURE_5, MEASURE_6, DISPLAY_FINAL };

        // 키보드 사용 이벤트
        private ManualResetEvent m_eventDownStatus;
        // 키보드 사용 시 디스플레이 업데이트 쓰레드
        private Thread m_ThreadUpdateDisplay;
        // 프로세스 종료
        private bool m_bThreadExit;
        //선택된 ROW
        private int m_iRowIndex;
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
		public CFormReportProcess150()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormSetupVisionOptical_Load( object sender, EventArgs e )
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
        private void CFormSetupVisionOptical_FormClosed( object sender, FormClosedEventArgs e )
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


                // 디스플레이 화면 생성
                {
                    m_iDisplayIndex = ( int )enumDisplayIndex.DISPLAY_FINAL;
                    m_objPanelDisplay = new Panel[ m_iDisplayIndex ];
                    m_objPanelDisplay[ ( int )enumDisplayIndex.PMS ] = this.panelDisplayPMS;

                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_1 ] = this.panelDisplayVidi1;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_2 ] = this.panelDisplayVidi2;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_3 ] = this.panelDisplayVidi3;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_4 ] = this.panelDisplayVidi4;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_5 ] = this.panelDisplayVidi5;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.VIDI_6 ] = this.panelDisplayVidi6;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_1 ] = this.panelDisplayMeasure1;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_2 ] = this.panelDisplayMeasure2;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_3 ] = this.panelDisplayMeasure3;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_4 ] = this.panelDisplayMeasure4;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_5 ] = this.panelDisplayMeasure5;
                    m_objPanelDisplay[ ( int )enumDisplayIndex.MEASURE_6 ] = this.panelDisplayMeasure6;

                    // 폼 디스플레이 생성
                    m_objFormDisplay = new Form[ m_iDisplayIndex ];

                    string[] strDisplayName = new string[ m_objFormDisplay.Length ];
                    strDisplayName[ ( int )enumDisplayIndex.PMS ] = enumDisplayIndex.PMS.ToString();
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

                    for( int iLoopCount = 0; iLoopCount < m_objFormDisplay.Length; iLoopCount++ ) {
                        // 디스플레이 화면 생성 & 초기화
                        CFormDisplay objForm = new CFormDisplay();
                        int iImageIndex = 0;
                        if( ( int )enumDisplayIndex.VIDI_1 > iLoopCount && ( int )enumDisplayIndex.PMS < iLoopCount )
                            iImageIndex = iLoopCount;
                        else if( ( int )enumDisplayIndex.VIDI_1 <= iLoopCount && ( int )enumDisplayIndex.MEASURE_1 > iLoopCount )
                            iImageIndex = iLoopCount - ( int )enumDisplayIndex.VIDI_1;
                        else if( ( int )enumDisplayIndex.MEASURE_1 <= iLoopCount )
                            iImageIndex = iLoopCount - ( int )enumDisplayIndex.MEASURE_1;


                        objForm.Initialize( ( int )CDefine.enumCamera.CAMERA_1, string.Format( "{0}", strDisplayName[ iLoopCount ] ), false, iImageIndex );
                        objForm.SetDisplayIndex( iLoopCount );

                        objForm.Visible = true;
                        objForm.SetTimer( true );
                        // 사이즈 조정
                        Panel objPanel = m_objPanelDisplay[ iLoopCount ];
                        objForm.SetSize( objPanel.Location.X, objPanel.Location.Y, objPanel.Width, objPanel.Height, false, false );
                        // 패널에 화면 붙임
                        SetFormDockStyle( objForm, objPanel );
                        m_objFormDisplay[ iLoopCount ] = objForm;
                    }
                }
                m_iRowIndex = -1;
                m_bThreadExit = false;
                m_eventDownStatus = new ManualResetEvent( true );
                // 키보드 디스플레이 쓰레드
                m_ThreadUpdateDisplay = new Thread( ThreadUpdateDisplay );
                m_ThreadUpdateDisplay.Start( this );

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

            m_objFormDisplay = null;
            m_bThreadExit = true;
            m_eventDownStatus.Set();

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
                CDefine.enumMachineType eMachineType = pDocument.m_objConfig.GetSystemParameter().eMachineType;
// 
//                 if( CDefine.enumMachineType.MACHINE_2_POL == eMachineType ) {
//                     BtnStageA.Text = "POL LEFT";
//                     BtnStageB.Text = "POL RIGHT";
//                 } else if( CDefine.enumMachineType.MACHINE_SAD == eMachineType ) {
//                     
//                 } else if( CDefine.enumMachineType.MACHINE_1_PANEL_2_POL == eMachineType ) {
//                     
//                 }

				// 검색 양불 타입 초기화
				m_eResultType = enumResultType.ALL;
				// 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
				if( false == InitializeDatabase() ) break;
				// Alarm 리스트 그리드 뷰 초기화
				if( false == InitializeGridView( this.GridViewAlignList ) ) break;
				// From Date 초기화
				if( false == InitializeDateTime( this.DateTimeFrom ) ) break;
				// To Date 초기화
				if( false == InitializeDateTime( this.DateTimeTo ) ) break;
                // 버튼 색상 정의
                SetButtonColor();

                bReturn = true;
            } while( false );

            return bReturn;
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 셀의 값을 한 페이지에서만 보여지도록 처리하기 위해..
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void GridViewAlignList_CellValueNeeded( object sender, DataGridViewCellValueEventArgs e )
		{
			do {
				try {
					if( 0 == m_objDataRow.Length ) break;
					if( m_objDataRow.Length <= e.RowIndex ) break;
					e.Value = m_objDataRow[ e.RowIndex ].ItemArray[ e.ColumnIndex ];
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}
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
            pFormCommon.SetButtonColor( this.BtnResultTypeAll, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnResultTypeNg, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnResultTypeOk, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSaveToCsv, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSelectAsc, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSelectDesc, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnChart, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSelectPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
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
				SetControlChangeLanguage( this.BtnSelectAsc );
				SetControlChangeLanguage( this.BtnSelectDesc );
				SetControlChangeLanguage( this.BtnSaveToCsv );
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
            if( true == bTimer ) {
                timer.Enabled = true;
            }
            else {
                timer.Enabled = false;
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
				pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_REPORT );
                // 해당 폼을 말단으로 설정
                pDocument.GetMainFrame().SetCurrentForm( this );
				// 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
				InitializeDatabase();
            }
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터베이스 현재 날짜로 초기화 & 데이터베이스 연동
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool InitializeDatabase()
		{
			bool bReturn = false;

			do {
				// 날짜 선택 컨트롤을 현재 날짜로 설정
				this.DateTimeFrom.Value = DateTime.Today;
				this.DateTimeTo.Value = DateTime.Today;
				// Align History 데이터베이스 연동
				SetAlignHistoryConnect( this.DateTimeFrom.Value, this.DateTimeTo.Value );
				// DB 리스트 그리드 뷰 초기화
				InitializeGridView( this.GridViewAlignList );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 그리드 뷰 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool InitializeGridView( DataGridView objGridView )
		{
			bool bReturn = false;
			var pFormCommon = CFormCommon.GetFormCommon;

			do {
				// 그리드 뷰 행 열 값 날려줌
				objGridView.Rows.Clear();
				objGridView.Columns.Clear();
				// 그리드 뷰 기본 스타일 초기화
				if( false == pFormCommon.InitializeGridView( objGridView ) ) break;
				// 그리드 뷰 칼럼 사이즈 (성능상 문제가 있어서 사이즈는 고정으로 픽스)
				objGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				// 그리드 뷰 ReadOnly
				objGridView.ReadOnly = true;
				// 그리드 뷰 다중 선택 x
				objGridView.MultiSelect = false;
				// 가상 모드로 사용해서 빠른 처리
				objGridView.VirtualMode = true;
				// 그리드 뷰 선택 모드 (행 전체 선택)
				objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				// 그리드 뷰 칼럼 추가
				for( int iLoopColumn = 0; iLoopColumn < m_objDataTable.Columns.Count; iLoopColumn++ ) {
					objGridView.Columns.Add( m_objDataTable.Columns[ iLoopColumn ].ToString(), m_objDataTable.Columns[ iLoopColumn ].ToString() );
				}
				// 그리드 뷰 칼럼 정렬 x
				for( int iLoopColumn = 0; iLoopColumn < objGridView.Columns.Count; iLoopColumn++ ) {
					objGridView.Columns[ iLoopColumn ].SortMode = DataGridViewColumnSortMode.NotSortable;
				}
                // INNER_ID, DATE 사이즈 조정
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.INNER_ID ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.DATE ].Width = 170;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.CELL_ID ].Width = 170;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ].Width = 90;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.TACT_TIME ].Width = 90;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.RESULT ].Width = 70;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.NG_TYPE ].Width = 180;

                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_6 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_6 ].Width = 150;


                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_6 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_6 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_6 ].Width = 150;

                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_6 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_HEIGHT_6 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_6 ].Width = 150;

                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.HEIGHT_RESULT_1 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.HEIGHT_RESULT_2 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.HEIGHT_RESULT_3 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.HEIGHT_RESULT_4 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.HEIGHT_RESULT_5 ].Width = 150;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.HEIGHT_RESULT_6 ].Width = 150;

                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_1 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_1 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_1 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_1 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_2 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_2 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_2 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_2 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_3 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_3 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_3 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_3 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_4 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_4 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_4 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_4 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_5 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_5 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_5 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_5 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_6 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_6 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_6 ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_6 ].Width = 0;

                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_LINE_FIND_COUNT ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.PATTERN_POSITION_X ].Width = 0;
                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.PATTERN_POSITION_Y ].Width = 0;

                objGridView.Columns[ ( int )CDatabaseDefine.enumHistoryAlign.IMAGE_PATH ].Width = 0;


                // 전체 행 수를 잡아줌
                objGridView.RowCount = m_objDataRow.Length;
				// 그리드 뷰 크기 변경
				pFormCommon.SetGridViewFont( objGridView, 9.0 );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이트 타임 피커 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool InitializeDateTime( DateTimePicker objDateTime )
		{
			bool bReturn = false;

			do {
				// 커스텀 포멧 설정
				objDateTime.CustomFormat = CDatabaseDefine.DEF_DATE_FORMAT;
				objDateTime.Format = DateTimePickerFormat.Custom;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 얼라인 이력 데이터베이스 연동
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetAlignHistoryConnect( DateTime objFrom, DateTime objTo )
		{
			// 디폴트 내림차순
			SetAlignHistoryConnect( objFrom, objTo, CDatabaseDefine.DEF_DESC );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 얼라인 이력 데이터베이스 연동
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetAlignHistoryConnect( DateTime objFrom, DateTime objTo, string strOrder )
		{
			string strQuery = null;
			var pDocument = CDocument.GetDocument;
			// History Align
			CManagerTable objManagerTableHistoryAlign = pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlign;
			
			try {
				strQuery = string.Format( "select * from {0} ", objManagerTableHistoryAlign.HLGetTableName() );
				// WHERE (기간)
				strQuery += string.Format( "where {0} between datetime('{1}') and datetime('{2}')",
					objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.DATE ],
					string.Format( "{0} 00:00:00", objFrom.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) ),
					string.Format( "{0} 23:59:59", objTo.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) ) );
				// WHERE (얼라인 결과 조건)
				if( enumResultType.ALL != m_eResultType ) {
					strQuery += string.Format( " and {0} = '{1}'",
					objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.RESULT ],
					m_eResultType.ToString() );
				}

//                 if( CDefine.enumAlignStage.STAGE_FINAL != m_eStage ) {
//                     strQuery += string.Format( " and {0} = '{1}'",
//                     objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.STAGE ],
//                     m_eStage.ToString() );
//                 }

				m_objDataTable = new DataTable();
				pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload( strQuery, ref m_objDataTable );
				m_objDataRow = m_objDataTable.Select( "", objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.DATE ] + " " + strOrder );
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.Message );
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
			var pFormCommon = CFormCommon.GetFormCommon;

			// NG
			if( enumResultType.NG == m_eResultType ) {
				pFormCommon.SetButtonBackColor( this.BtnResultTypeNg, pFormCommon.COLOR_ACTIVATE );
				pFormCommon.SetButtonBackColor( this.BtnResultTypeOk, pFormCommon.COLOR_UNACTIVATE );
				pFormCommon.SetButtonBackColor( this.BtnResultTypeAll, pFormCommon.COLOR_UNACTIVATE );
			}
			// OK
			else if( enumResultType.OK == m_eResultType ) {
				pFormCommon.SetButtonBackColor( this.BtnResultTypeNg, pFormCommon.COLOR_UNACTIVATE );
				pFormCommon.SetButtonBackColor( this.BtnResultTypeOk, pFormCommon.COLOR_ACTIVATE );
				pFormCommon.SetButtonBackColor( this.BtnResultTypeAll, pFormCommon.COLOR_UNACTIVATE );
			}
			// ALL
			else if( enumResultType.ALL == m_eResultType ) {
				pFormCommon.SetButtonBackColor( this.BtnResultTypeNg, pFormCommon.COLOR_UNACTIVATE );
				pFormCommon.SetButtonBackColor( this.BtnResultTypeOk, pFormCommon.COLOR_UNACTIVATE );
				pFormCommon.SetButtonBackColor( this.BtnResultTypeAll, pFormCommon.COLOR_ACTIVATE );
			}


        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : From 날짜 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void DateTimeFrom_ValueChanged( object sender, EventArgs e )
		{
			// Form 날짜는 To 날짜보다 이후 출력이 되면 안됨
			// To 날짜도 갱신되도록 함
			DateTimePicker objFrom = this.DateTimeFrom;
			DateTimePicker objTo = this.DateTimeTo;
			// From 날짜가 To 날짜보다 더 크면 To 날짜 갱신
			if( objFrom.Value > objTo.Value ) {
				objTo.Value = objFrom.Value;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : To 날짜 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void DateTimeTo_ValueChanged( object sender, EventArgs e )
		{
			// To 날짜는 From 날짜보다 이전 출력이 되면 안됨
			// From 날짜도 갱신되도록 함
			DateTimePicker objFrom = this.DateTimeFrom;
			DateTimePicker objTo = this.DateTimeTo;
			// To 날짜가 From 날짜보다 적으면 From 날짜 갱신
			if( objTo.Value < objFrom.Value ) {
				objFrom.Value = objTo.Value;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : NG 타입만 필터링
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnResultTypeNg_Click( object sender, EventArgs e )
		{
			m_eResultType = enumResultType.NG;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : OK 타입만 필터링
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnResultTypeOk_Click( object sender, EventArgs e )
		{
			m_eResultType = enumResultType.OK;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 전체 타입
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnResultTypeAll_Click( object sender, EventArgs e )
		{
			m_eResultType = enumResultType.ALL;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : SAVE TO CSV FILE
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnSaveToCsv_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// DataTable 받아서 Csv로 저장
			CCsvFile objCsvFile = new CCsvFile();
			// 현재 경로 폴더 확인 & 생성
			string strPath = string.Format( @"{0}\Database", pDocument.m_objConfig.GetLogPath() );
			DirectoryInfo objDirectory = new DirectoryInfo( strPath );
			if( false == objDirectory.Exists ) {
				objDirectory.Create();
			}
			// 현재 경로에 테이블 이름.csv 로 파일 생성
			strPath = string.Format( @"{0}\RECORD_{1}_{2}.csv", strPath, "CELL_LOG_ITEM", string.Format( "{0:yyyyMMdd_HHmm}", DateTime.Now ) );
			objCsvFile.SetDataTableToCsv( strPath, m_objDataTable );
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [Path : {1}]", "BtnSaveToCsv_Click", strPath );
			pDocument.SetUpdateButtonLog( this, strLog );
			MessageBox.Show( string.Format( "Path : {0}", strPath ) );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : SELECT (ASC)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnSelectAsc_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// Align History 데이터베이스 연동
			SetAlignHistoryConnect( this.DateTimeFrom.Value, this.DateTimeTo.Value, CDatabaseDefine.DEF_ASC );
			// DB 리스트 그리드 뷰 초기화
			InitializeGridView( this.GridViewAlignList );
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [From : {1}] [To : {2}]", "BtnSelectAsc_Click", this.DateTimeFrom.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ), this.DateTimeTo.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) );
			pDocument.SetUpdateButtonLog( this, strLog );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : SELECT (DESC)
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void BtnSelectDesc_Click( object sender, EventArgs e )
		{
			var pDocument = CDocument.GetDocument;
			// Align History 데이터베이스 연동
			SetAlignHistoryConnect( this.DateTimeFrom.Value, this.DateTimeTo.Value, CDatabaseDefine.DEF_DESC );
			// DB 리스트 그리드 뷰 초기화
			InitializeGridView( this.GridViewAlignList );
			// 버튼 로그 추가
			string strLog = string.Format( "[{0}] [From : {1}] [To : {2}]", "BtnSelectDesc_Click", this.DateTimeFrom.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ), this.DateTimeTo.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) );
			pDocument.SetUpdateButtonLog( this, strLog );
		}

        private void GridViewAlignList_CellClick( object sender, DataGridViewCellEventArgs e )
        {
            m_iRowIndex = e.RowIndex;
            UpdateDisplay( e.RowIndex );
        }

        private void GridViewAlignList_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {

        }

        private void GridViewAlignList_CellEnter( object sender, DataGridViewCellEventArgs e )
        {
            
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateDisplay( int iRowIndex )
        {
            try {
                CDefine.structureReportImage objReportImage = new CDefine.structureReportImage();
                if( -1 == iRowIndex ) return;
                if( "OK" == m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.RESULT ].ToString() )
                    objReportImage.bResult = true;
                else
                    objReportImage.bResult = false;

                objReportImage.strImagePath = m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.IMAGE_PATH ].ToString();
                objReportImage.dPatternPositionX = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.PATTERN_POSITION_X ].ToString() );
                objReportImage.dPatternPositionY = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.PATTERN_POSITION_Y ].ToString() );
                ( m_objFormDisplay[ ( int )enumDisplayIndex.PMS ] as CFormDisplay ).UpdateDisplayHistory3D( objReportImage );

                // 높이그랙픽 그리기..
                CDefine.structureReportImage[] objReportImageLineResult = new CDefine.structureReportImage[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    objReportImageLineResult[ iLoopCount ] = new CDefine.structureReportImage();
                    objReportImageLineResult[ iLoopCount ].iFindLineCount = Int32.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_LINE_FIND_COUNT ].ToString() );

                    objReportImageLineResult[ iLoopCount ].dStartX = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_1 + ( iLoopCount * 4 ) ].ToString() );
                    objReportImageLineResult[ iLoopCount ].dStartY = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_1 + ( iLoopCount * 4 ) ].ToString() );
                    objReportImageLineResult[ iLoopCount ].dEndX = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_1 + ( iLoopCount * 4 ) ].ToString() );
                    objReportImageLineResult[ iLoopCount ].dEndY = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_1 + ( iLoopCount * 4 ) ].ToString() );

                    objReportImageLineResult[ iLoopCount ].dLineDistance = double.Parse( m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_1 + ( iLoopCount * 3 ) ].ToString() );
                }
                ( m_objFormDisplay[ ( int )enumDisplayIndex.PMS ] as CFormDisplay ).SetDisplayMeasureHeight( objReportImageLineResult );


                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    objReportImage.strScore = m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_SCORE_1 + ( iLoopCount * 2 ) ].ToString();
                    objReportImage.bResult = "OK" == m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.VIDI_RESULT_1 + ( iLoopCount * 2 ) ].ToString() ? true : false;
                    ( m_objFormDisplay[ ( int )enumDisplayIndex.VIDI_1 + iLoopCount ] as CFormDisplay ).UpdateDisplayHistoryVIDI( objReportImage );
                }

                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    objReportImage.bResult = "OK" == m_objDataRow[ iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_1 + ( iLoopCount * 3 ) ].ToString() ? true : false;
                    ( m_objFormDisplay[ ( int )enumDisplayIndex.MEASURE_1 + iLoopCount ] as CFormDisplay ).UpdateDisplayHistory3DHeight( objReportImage );
                }
            } catch( Exception ex ) {
                Trace.WriteLine( ex.ToString() );
            }
        }

        private void GridViewAlignList_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ) {
                m_eventDownStatus.Reset();
            }
        }

        private void GridViewAlignList_KeyUp( object sender, KeyEventArgs e )
        {
            m_eventDownStatus.Set();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 업데이트 디스플레이 쓰레드 ( 키보드 사용 시 )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadUpdateDisplay( object state )
        {
            CFormReportProcess150 pThis = ( CFormReportProcess150 )state;
            // 스레드 주기
            int iThreadPeriod = 100;
            while( false == pThis.m_bThreadExit ) {
                pThis.m_eventDownStatus.WaitOne();
                if( null != pThis.GridViewAlignList.CurrentCell ) {
                    if( -1 != pThis.GridViewAlignList.CurrentCell.RowIndex ) {
                        pThis.m_iRowIndex = pThis.GridViewAlignList.CurrentCell.RowIndex;
                        pThis.UpdateDisplay( pThis.GridViewAlignList.CurrentCell.RowIndex );
                    }
                }

                pThis.m_eventDownStatus.Reset();
                Thread.Sleep( iThreadPeriod );
            }
        }

        private void BtnChart_Click( object sender, EventArgs e )
        {
            CDefine.structureReportImage objReportImage = new CDefine.structureReportImage();
            if( -1 == m_iRowIndex ) return;
            if (0 == m_objDataRow.Length) return;
            objReportImage.strImagePath = m_objDataRow[ m_iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.IMAGE_PATH ].ToString();
            objReportImage.iInspectionIndex = Int32.Parse( m_objDataRow[ m_iRowIndex ].ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ].ToString() );

            CDialogChartHistory objChart = new CDialogChartHistory( objReportImage );
            objChart.ShowDialog();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : SELECT (POSITION DESC)
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnSelectPosition_Click( object sender, EventArgs e )
        {
            //포지션으로 검색
            string strQuery = null;
            var pDocument = CDocument.GetDocument;

            do {
                // History Align
                CManagerTable objManagerTableHistoryAlign = pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlign;
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );

                int iPosition = 0;
                if( 0 >= objRecipeParameter.iCountInspectionPosition ) break;
                string[] strButtonList = new string[ objRecipeParameter.iCountInspectionPosition ];
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.iCountInspectionPosition; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = "POSITION " + ( iLoopCount + 1 ).ToString();
                }
                CDialogEnumerate objDialog = new CDialogEnumerate( objRecipeParameter.iCountInspectionPosition, strButtonList, 0 );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    iPosition = objDialog.GetResult() + 1;
                } else {
                    break;
                }


                try {
                    strQuery = string.Format( "select * from {0} ", objManagerTableHistoryAlign.HLGetTableName() );
                    // WHERE (기간)
                    strQuery += string.Format( "where {0} between datetime('{1}') and datetime('{2}')",
                                    objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.DATE ],
                                    string.Format( "{0} 00:00:00", this.DateTimeFrom.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) ),
                                    string.Format( "{0} 23:59:59", this.DateTimeTo.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) ) );
                    strQuery += string.Format( "and {0} = '{1}'",
                                                          objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ],
                                                          iPosition.ToString() );
                    // WHERE (얼라인 결과 조건)
                    if( enumResultType.ALL != m_eResultType ) {
                        strQuery += string.Format( " and {0} = '{1}'",
                        objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.RESULT ],
                        m_eResultType.ToString() );
                    }


                    m_objDataTable = new DataTable();
                    pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload( strQuery, ref m_objDataTable );
                    m_objDataRow = m_objDataTable.Select( "", objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.DATE ] + " " + CDatabaseDefine.DEF_DESC );
                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message );
                }


                // DB 리스트 그리드 뷰 초기화
                InitializeGridView( this.GridViewAlignList );
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [From : {1}] [To : {2}]", "BtnSelectAsc_Click", this.DateTimeFrom.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ), this.DateTimeTo.Value.ToString( CDatabaseDefine.DEF_DATE_FORMAT ) );
                pDocument.SetUpdateButtonLog( this, strLog );
            } while( false );
        }
            
    }
}