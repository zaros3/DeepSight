using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DeepSight
{
	public partial class CFormConfig : Form, CFormInterface
    {
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 현재 폼
        private CDefine.FormViewConfig m_eCurrentForm = CDefine.FormViewConfig.FORM_VIEW_CONFIG_FINAL;
        // 폼 (화면이 전환되는 폼)
        private struct structureForm
        {
            private Form _objForm;
            public Form m_objForm
            {
                get { return _objForm; }
            }
            private CFormInterface _IForm;
            public CFormInterface m_IForm
            {
                get { return _IForm; }
            }

            public structureForm( CFormInterface form )
            {
                this._IForm = form;
                this._objForm = this._IForm as Form;
            }
        }

        private structureForm[] m_stForm;

        // 폼에 사이즈에 맞춰 버튼을 동적 할당해줌
        // 동적 생성될 버튼
        private Button[] m_btnMenu;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CFormConfig()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CFormConfig_Load( object sender, EventArgs e )
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
        private void CFormConfig_FormClosed( object sender, FormClosedEventArgs e )
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
        private bool Initialize()
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
				var pFormCommon = CFormCommon.GetFormCommon;
                int iMenuCount = ( int )CDefine.FormViewConfig.FORM_VIEW_CONFIG_FINAL;
                // 버튼 여백 & 넓이 설정
                int iWhiteSpace = 2;
                int iButtonWidth = ( this.panelFormMenu.Width / iMenuCount );
                // 버튼 개수가 적어서 base 버튼보다 넓이가 크면 base 버튼 넓이로 고정
                if( iButtonWidth > this.BtnBase.Width ) {
                    iButtonWidth = this.BtnBase.Width;
                }
                string[] strButtonName = new string[ iMenuCount ];
                m_btnMenu = new Button[ iMenuCount ];
                for( int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++ ) {
                    strButtonName[ iLoopButton ] = ( ( CDefine.FormViewConfig )iLoopButton ).ToString();
                }
                // 버튼 동적 생성
				pFormCommon.SetDynamicMenuButton( m_btnMenu, this.panelFormMenu, strButtonName, iButtonWidth, iWhiteSpace, new EventHandler( ButtonMenu_Click ) );
                // 버튼 이름 설정
                for( int iLoopMenu = 0; iLoopMenu < m_btnMenu.Length; iLoopMenu++ ) {
                    m_btnMenu[ iLoopMenu ].Name = string.Format( "BtnConfigMenu[{0}]", iLoopMenu );
                }
                // 폼 생성
                m_stForm = new structureForm[ iMenuCount ];
                // 화면 전환
                SetChangeForm( CDefine.FormViewConfig.FORM_VIEW_CONFIG_OPTION );
                // 타이머 설정
                timer.Interval = 100;
                timer.Enabled = true;

                this.BackColor = pFormCommon.COLOR_FORM_VIEW;
                this.panelFormMenu.BackColor = pFormCommon.COLOR_FORM_VIEW;
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetChangeForm( CDefine.FormViewConfig eForm )
        {
            var pDocument = CDocument.GetDocument;
            do {
                // 현재 폼이 이전 폼과 같으면 건너뜀
                if( m_eCurrentForm == eForm ) break;
                // 현재 폼 Invisible
                if( CDefine.FormViewConfig.FORM_VIEW_CONFIG_FINAL != m_eCurrentForm ) {
                    m_stForm[ ( int )m_eCurrentForm ].m_IForm.SetVisible( false );
                    m_stForm[ ( int )m_eCurrentForm ].m_IForm.SetTimer( false );
                }

                // 생성이 되어 있지 않으면 생성
                if( null == m_stForm[ ( int )eForm ].m_IForm ) {
                    switch( eForm ) {
                        case CDefine.FormViewConfig.FORM_VIEW_CONFIG_OPTION:
                            m_stForm[ ( int )eForm ] = new structureForm( new CFormConfigOption() as CFormInterface );
                            break;
                        case CDefine.FormViewConfig.FORM_VIEW_CONFIG_RECIPE:
                            m_stForm[ ( int )eForm ] = new structureForm( new CFormConfigRecipe() as CFormInterface );
                            break;
                        default:
                            break;
                    }
                    // 패널에 생성된 화면 붙임
                    SetFormDockStyle( m_stForm[ ( int )eForm ].m_objForm, this.panelFormView );
                    m_stForm[ ( int )eForm ].m_IForm.SetVisible( true );
                    m_stForm[ ( int )eForm ].m_IForm.SetTimer( true );
                    m_stForm[ ( int )eForm ].m_IForm.SetChangeLanguage();
                }
                // 생성 되어 있으면 Visible
                else {
                    m_stForm[ ( int )eForm ].m_IForm.SetVisible( true );
                    m_stForm[ ( int )eForm ].m_IForm.SetTimer( true );
                    m_stForm[ ( int )eForm ].m_IForm.SetChangeLanguage();
                }
                
                m_eCurrentForm = eForm;
            } while( false );
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
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetControlChangeLanguage( m_btnMenu[ ( int )CDefine.FormViewConfig.FORM_VIEW_CONFIG_OPTION ] );
                SetControlChangeLanguage( m_btnMenu[ ( int )CDefine.FormViewConfig.FORM_VIEW_CONFIG_RECIPE ] );

                m_stForm[ ( int )m_eCurrentForm ].m_IForm.SetChangeLanguage();

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
            m_stForm[ ( int )m_eCurrentForm ].m_IForm.SetTimer( bTimer );
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
            m_stForm[ ( int )m_eCurrentForm ].m_IForm.SetVisible( bVisible );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼 클릭 이벤트 정의
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ButtonMenu_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            Button objButton = sender as Button;
            try {
                var objRegex = new Regex( @"\[(.+)\]" );
                if( true == objRegex.IsMatch( objButton.Name ) ) {
                    string strText = objRegex.Match( objButton.Name ).Groups[ 1 ].Value;
                    CDefine.FormViewConfig eFormView = ( CDefine.FormViewConfig )Convert.ToInt32( strText );

                    // 버튼 로그 추가
                    string strLog = string.Format( "[{0}] [Form Change : {1} -> {2}]", "ButtonMenu_Click", m_eCurrentForm.ToString(), eFormView.ToString() );
                    pDocument.SetUpdateButtonLog( this, strLog );

                    SetChangeForm( eFormView );
                }
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머.
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer_Tick(object sender, EventArgs e)
        {
			var pFormCommon = CFormCommon.GetFormCommon;
            for (int iLoopMenu = 0; iLoopMenu < (int)CDefine.FormViewConfig.FORM_VIEW_CONFIG_FINAL; iLoopMenu++) {

                if (iLoopMenu == (int)m_eCurrentForm)
					pFormCommon.SetButtonColor( m_btnMenu[ iLoopMenu ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
                else
					pFormCommon.SetButtonColor( m_btnMenu[ iLoopMenu ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }
        }
    }
}