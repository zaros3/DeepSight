using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepSight
{
    public partial class CFormConfigRecipe : Form, CFormInterface
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 선택된 알람 리스트 행
        private int m_iSelectedRow = 0;
        // 모델 파라미터 리스트
		private List<CConfig.CRecipeInformation> m_objRecipeInformationList;
        // 모델 리스트 칼럼 정의
        private enum enumModelListColumn
        {
            PPID = 0,
            NAME,
            MODEL_LIST_COLUMN_FINAL
        };

		// 유저 권한 레벨에 따른 버튼 상태 변경
		public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
		public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormConfigRecipe()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormConfigRecipe_Load( object sender, EventArgs e )
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
        private void CFormConfigRecipe_FormClosed( object sender, FormClosedEventArgs e )
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
                // 모델 리스트 그리드 뷰 초기화
                string[] strModelList = { enumModelListColumn.PPID.ToString(), enumModelListColumn.NAME.ToString() };
                if( false == InitializeGridView( this.GridViewRecipeList, strModelList ) ) break;
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
        //목적 : 버튼 색상 정의
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonColor()
        {
			var pFormCommon = CFormCommon.GetFormCommon;

            this.BackColor = pFormCommon.COLOR_FORM_VIEW;

            pFormCommon.SetButtonColor( this.BtnTitleRecipeList, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleSelectedPPID, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnSelectedPPID, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnSave, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnLoad, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnDelete, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCreate, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnTitleCreatePPID, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleCreateName, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnCreatePPID, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCreateName, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
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
                SetControlChangeLanguage( this.BtnTitleRecipeList );
                SetControlChangeLanguage( this.BtnTitleSelectedPPID );
                SetControlChangeLanguage( this.BtnSave );
                SetControlChangeLanguage( this.BtnLoad );
                SetControlChangeLanguage( this.BtnDelete );
                SetControlChangeLanguage( this.BtnCreate );
                SetControlChangeLanguage( this.BtnTitleCreatePPID );
                SetControlChangeLanguage( this.BtnTitleCreateName );

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
				pFormCommon.SetChangeButtonStatus( pDocument, this.Controls, CDefine.FormView.FORM_VIEW_SETUP );
                // 해당 폼을 말단으로 설정
                pDocument.GetMainFrame().SetCurrentForm( this );
                // 모델 정보 리스트 갱신
                m_objRecipeInformationList = pDocument.m_objRecipe.GetRecipeInformationList();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool InitializeGridView( DataGridView objGridView, string[] strColumnName )
        {
            bool bReturn = false;

            do {
				var pFormCommon = CFormCommon.GetFormCommon;
                // 그리드 뷰 기본 스타일 초기화
				if( false == pFormCommon.InitializeGridView( objGridView ) ) break;
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // 그리드 뷰 칼럼 추가
                for( int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++ ) {
                    objGridView.Columns.Add( string.Format( "{0}", iLoopColumn ), strColumnName[ iLoopColumn ] );
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[ iLoopColumn ].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                // 그리드 뷰 크기 변경
				pFormCommon.SetGridViewFont( objGridView, 12.0 );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 모델 목록 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetModelListGridView( List<CConfig.CRecipeInformation> objRecipeList )
        {
            var pDocument = CDocument.GetDocument;
            bool bCompare = true;
            DataGridView objGridView = this.GridViewRecipeList;

            do {
                // 현재 그리드 뷰에 레시피 리스트와 비교 (길이 다르면 바로 갱신)
                try {
					if( objRecipeList.Count == objGridView.RowCount ) {
						for( int iLoopRecipeList = 0; iLoopRecipeList < objRecipeList.Count; iLoopRecipeList++ ) {
							if( objRecipeList[ iLoopRecipeList ].strRecipeID != objGridView[ ( int )enumModelListColumn.PPID, iLoopRecipeList ].Value.ToString()
								|| objRecipeList[ iLoopRecipeList ].strRecipeName != objGridView[ ( int )enumModelListColumn.NAME, iLoopRecipeList ].Value.ToString() ) {
                                bCompare = false;
                                break;
                            }
                        }
                    }
                    else {
                        bCompare = false;
                    }
                }
                catch( Exception ex ) {
                    Trace.WriteLine( ex.StackTrace );
                }

                // 데이터 같으면 다시 쓸 필요 없음
                if( true == bCompare ) break;

                // 갱신하는 부분
                lock( objGridView ) {
                    objGridView.Rows.Clear();

					for( int iLoopRecipeList = 0; iLoopRecipeList < objRecipeList.Count; iLoopRecipeList++ ) {
                        string[] strRowData = new string[ ( int )enumModelListColumn.MODEL_LIST_COLUMN_FINAL ];
						strRowData[ ( int )enumModelListColumn.PPID ] = objRecipeList[ iLoopRecipeList ].strRecipeID;
						strRowData[ ( int )enumModelListColumn.NAME ] = objRecipeList[ iLoopRecipeList ].strRecipeName;
                        objGridView.Rows.Add( strRowData );
                        // 갱신할 경우 현재 적용된 PPID를 리스트에서 선택되도록 한다.
						if( objRecipeList[ iLoopRecipeList ].strRecipeID == pDocument.m_objConfig.GetSystemParameter().strRecipeID )
							m_iSelectedRow = iLoopRecipeList;
                    }
                    try {
						if( m_iSelectedRow >= objRecipeList.Count ) {
                            m_iSelectedRow = 0;
                        }
                        objGridView[ 0, m_iSelectedRow ].Selected = true;
                    }
                    catch( Exception ex ) {
                        Trace.WriteLine( ex.StackTrace );
                    }
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 모델 데이터 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetModelListData( int iRow )
        {
            var pDocument = CDocument.GetDocument;
            try {
                // 현재 PPID와 선택된 PPID가 일치하는 경우에만 버튼 활성화
				if( m_objRecipeInformationList[ iRow ].strRecipeID == pDocument.m_objConfig.GetSystemParameter().strRecipeID ) {
                    // 수정 버튼 활성화
                    SetEditButtonEnable( true );
                    // 삭제 버튼 비활성화
                    this.BtnDelete.Enabled = false;
                    this.BtnSave.Enabled = true;
                }
                else {
                    // 수정 버튼 비활성화
                    SetEditButtonEnable( false );
                    this.BtnDelete.Enabled = true;
                    this.BtnSave.Enabled = false;
                }
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 수정 버튼 활성화 & 비활성화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetEditButtonEnable( bool bEnable )
        {
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
            // 현재 PPID 갱신
			pFormCommon.SetButtonText( this.BtnSelectedPPID, pDocument.m_objConfig.GetSystemParameter().strRecipeID );
            // 모델 리스트 갱신
            SetModelListGridView( m_objRecipeInformationList );
            // 현재 유저 정보 받음
            CUserInformation objUserInformation = pDocument.GetUserInformation();
            // 현재 유저 권한 레벨이 폼 Write 레벨보다 낮으면 버튼 막음
            if( ( int )objUserInformation.m_eAuthorityLevel >= ( int )pDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )CDefine.FormView.FORM_VIEW_CONFIG ].eLevelWrite ) {
//                 this.BtnDelete.Enabled = true;
//                 this.BtnSave.Enabled = true;
//                 this.BtnLoad.Enabled = true;
                // 모델 리스트 데이터 갱신
                SetModelListData( m_iSelectedRow );
            }
            else {
                this.BtnDelete.Enabled = false;
                this.BtnSave.Enabled = false;
                this.BtnLoad.Enabled = false;
                // 수정 버튼 비활성화
                SetEditButtonEnable( false );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 레시피 저장
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnSave_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // SSH - 메시지 추가. - 레시피 저장
                // 레시피를 저장하시겠습니까?
                if( System.Windows.Forms.DialogResult.Yes != pDocument.SetMessage( CDefine.enumAlarmType.ALARM_QUESTION, 10044 ) ) break;

				// 레시피를 저장 중입니다…
				pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10114 ) );

				CConfig.CRecipeInformation objRecipeInformation = ( CConfig.CRecipeInformation )m_objRecipeInformationList[ m_iSelectedRow ].Clone();
				objRecipeInformation.strRecipeName = BtnCreateName.Text;
                // 레시피 파라미터 저장
				pDocument.m_objConfig.SaveRecipeInformation( objRecipeInformation );
                // 레시피 파라미터 리스트 갱신
                m_objRecipeInformationList = pDocument.m_objRecipe.GetRecipeInformationList();
                //pDocument.m_objConfig.LoadModelParameter();
            } while( false );

            // 버튼 로그 추가
            string strLog = string.Format( "[{0}]", "BtnSave_Click" );
            pDocument.SetUpdateButtonLog( this, strLog );

			pDocument.GetMainFrame().ShowWaitMessage( false, "" );
        }

        private void BtnLoad_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // SSH - 메시지 추가. - 레시피 불러오기
                // 레시피를 불러오시겠습니까?
                if( System.Windows.Forms.DialogResult.Yes != pDocument.SetMessage( CDefine.enumAlarmType.ALARM_QUESTION, 10045 ) ) break;

				// 레시피를 로드 중입니다…
				pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10111 ) );

                // 시스템 파라미터 현재 PPID 변경
                CConfig.CSystemParameter objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                // 버튼 로그 추가
				string strLog = string.Format( "[{0}] [PPID : {1} -> {2}]", "BtnLoad_Click", objSystemParameter.strRecipeID, m_objRecipeInformationList[ m_iSelectedRow ].strRecipeID );
                pDocument.SetUpdateButtonLog( this, strLog );
                // 현재 적용된 PPID 변경. 
				objSystemParameter.strRecipeID = m_objRecipeInformationList[ m_iSelectedRow ].strRecipeID;
                var objMMFVisionData = ENC.MemoryMap.Manager.CMMFManagerVisionData.Instance;
				objMMFVisionData[ 0 ].strMachineRecipeID = m_objRecipeInformationList[ m_iSelectedRow ].strRecipeID;
                pDocument.m_objConfig.SaveSystemParameter( objSystemParameter );
				pDocument.m_objConfig.LoadRecipeInformation();
                // 레시피 파라미터 로드
                pDocument.m_objConfig.LoadRecipeParameter();
                // 레시피 파라미터 리스트 갱신
				m_objRecipeInformationList = pDocument.m_objRecipe.GetRecipeInformationList();

                pDocument.m_objConfig.LoadLightControllerParameter();
                pDocument.m_objConfig.LoadRecipeParameter();
                pDocument.m_objConfig.LoadCameraParameter();
                pDocument.m_objProcessMain.LoadRecipe();
                pDocument.m_objProcessMain.SetCameraConfig();
                pDocument.m_bRecipeChange = true;
            } while( false );

			pDocument.GetMainFrame().ShowWaitMessage( false, "" );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 레시피 삭제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnDelete_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // SSH - 메시지 추가. -  레시피 삭제
                // 레시피를 삭제하시겠습니까?
                if( System.Windows.Forms.DialogResult.Yes != pDocument.SetMessage( CDefine.enumAlarmType.ALARM_QUESTION, 10046 ) ) break;

				// 레시피를 삭제 중입니다…
				pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10113 ) );

                // 레시피 삭제
				string strRecipeID = m_objRecipeInformationList[ m_iSelectedRow ].strRecipeID;
                string strPath = string.Format( @"{0}\{1}", pDocument.m_objConfig.GetRecipePath(), strRecipeID );

                pDocument.m_objRecipe.SetDirectoryDelete( strPath );
                // 모델 파라미터 리스트 갱신
				m_objRecipeInformationList = pDocument.m_objRecipe.GetRecipeInformationList();
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Delete PPID : {1}]", "BtnDelete_Click", strRecipeID );
                pDocument.SetUpdateButtonLog( this, strLog );
                
            } while( false );

			pDocument.GetMainFrame().ShowWaitMessage( false, "" );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 레시피 생성
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCreate_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            CRecipe objRecipe = pDocument.m_objRecipe;
            CConfig objConfig = pDocument.m_objConfig;
            string strRecipeID = this.BtnCreatePPID.Text;
            string strName = this.BtnCreateName.Text;

            // 레시피 생성
            do {
                // SSH - 메시지 추가. -  레시피 생성
                // 레시피를 생성하시겠습니까?
                if( System.Windows.Forms.DialogResult.Yes != pDocument.SetMessage( CDefine.enumAlarmType.ALARM_QUESTION, 10047 ) ) break;

                // 레시피 아이디가 존재하지 않는 경우
                if( "" == strRecipeID ) {
                    // 레시피 아이디가 없습니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10010 );
                    break;
                }
                // 레시피 아이디가 중복되는 경우
                if( true == pDocument.m_objRecipe.GetRecipeIDOverlap( strRecipeID ) ) {
                    // 레시피 아이디가 중복됩니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10011 );
                    break;
                }

				// 레시피를 생성 중입니다…
				pDocument.GetMainFrame().ShowWaitMessage( true, pDocument.GetDatabaseUserMessage( 10112 ) );

                string strExistFilePath = string.Format( @"{0}\{1}", objConfig.GetRecipePath(), objConfig.GetSystemParameter().strRecipeID );
                string strNewFilePath = string.Format( @"{0}\{1}", objConfig.GetRecipePath(), strRecipeID );
                // 폴더 복사
				objRecipe.SetDirectoryCopy( strExistFilePath, strNewFilePath );
				objRecipe.SetRecipeIDMatch( strRecipeID, strName );
                // 레시피 파라미터 리스트 갱신
				m_objRecipeInformationList = objRecipe.GetRecipeInformationList();

                BtnCreateName.Text = "";
                BtnCreatePPID.Text = "";

                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [Create PPID : {1}]", "BtnCreate_Click", strRecipeID );
                pDocument.SetUpdateButtonLog( this, strLog );

            } while( false );

			pDocument.GetMainFrame().ShowWaitMessage( false, "" );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성 PPID 입력 받음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCreatePPID_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            Button objButton = sender as Button;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnCreatePPID_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            pDocument.SetUpdateButtonLog( this, strLog );
            FormKeyPad objKeyboard = new FormKeyPad( 0 );
            if( System.Windows.Forms.DialogResult.OK == objKeyboard.ShowDialog() ) {
                if( 0 > objKeyboard.m_dResultValue ) objKeyboard.m_dResultValue = 0;
                objButton.Text = ( ( int )objKeyboard.m_dResultValue ).ToString();
            }
            // 버튼 로그 추가
            strLog = string.Format( "[{0}] [Create PPID : {1}] [{2}]", "BtnCreatePPID_Click", objButton.Text, false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성 레시피 이름 입력 받음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCreateName_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            Button objButton = sender as Button;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnCreateName_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyBoard objKeyboard = new FormKeyBoard();
            if( System.Windows.Forms.DialogResult.OK == objKeyboard.ShowDialog() ) {
                objButton.Text = objKeyboard.m_strReturnValue.ToUpper();
            }
            // 버튼 로그 추가
            strLog = string.Format( "[{0}] [Create Name : {1}] [{2}]", "BtnCreateName_Click", objButton.Text, false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이름 입력 받음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnName_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}] [{1}]", "BtnName_Click", true );
            pDocument.SetUpdateButtonLog( this, strLog );

            FormKeyBoard objKeyboard = new FormKeyBoard();
            if( System.Windows.Forms.DialogResult.OK == objKeyboard.ShowDialog() ) {
				m_objRecipeInformationList[ m_iSelectedRow ].strRecipeName = objKeyboard.m_strReturnValue;
            }
            // 버튼 로그 추가
			strLog = string.Format( "[{0}] [Name : {1}] [{2}]", "BtnName_Click", m_objRecipeInformationList[ m_iSelectedRow ].strRecipeName, false );
            pDocument.SetUpdateButtonLog( this, strLog );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 셀 변경 시
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void GridViewRecipeList_CellClick( object sender, DataGridViewCellEventArgs e )
        {
            DataGridView objGridView = sender as DataGridView;

            try {
                m_iSelectedRow = objGridView.CurrentCell.RowIndex;
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }
    }
}