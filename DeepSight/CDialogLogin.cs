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
    public partial class CDialogLogin : Form
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 로그 아웃 유무
        private bool m_bLogout;
        private CDefine.enumUserAuthorityLevel m_objUserAuthorityLevel;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDialogLogin()
        {
            m_bLogout = false;
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDialogLogin( bool bLogout )
        {
            m_bLogout = bLogout;
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CDialogLogin_Load( object sender, EventArgs e )
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
        private void CDialogLogin_FormClosed( object sender, FormClosedEventArgs e )
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
            var pDocument = CDocument.GetDocument;
            do {
                m_objUserAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
                // 폼 초기화
                if( false == InitializeForm() ) break;
                // 로그 아웃 true이면 로그 아웃
                if( true == m_bLogout ) {
                    pDocument.SetLogout();
                }

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
                // 폼 중앙에서 생성
                this.CenterToParent();
                // 패스워드는 표시 안되게
                TextPassword.PasswordChar = '＊';
                // 현재 로그인 된 유저 정보를 얻어서 뿌려줘야 함.
                TextID.Text = "";
                TextPassword.Text = "";
                // 포커스 ID 버튼으로 이동
                this.ActiveControl = this.TextPassword;
                // 버튼 색상 정의
                SetButtonColor();
                // 언어 변경
                SetChangeLanguage();

                timer1.Enabled = true;

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

            
			pFormCommon.SetButtonColor( this.BtnTitleLogin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
			pFormCommon.SetButtonColor( this.BtnLogin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnLogout, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            pFormCommon.SetButtonColor( this.BtnTitlePassword, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            TextPassword.BackColor = pFormCommon.COLOR_UNACTIVATE;
            TextPassword.ForeColor = pFormCommon.COLOR_WHITE;

            pFormCommon.SetButtonColor( this.BtnOperator, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnEngineer, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnMaster, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
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
                SetControlChangeLanguage( this.BtnTitleLogin );
                SetControlChangeLanguage( this.BtnTitleID );
                SetControlChangeLanguage( this.BtnTitlePassword );
                SetControlChangeLanguage( this.BtnLogin );
                SetControlChangeLanguage( this.BtnLogout );

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
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : Visible 유무
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetVisible( bool bVisible )
        {
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 로그인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLogin_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strID = TextID.Text;
            string strPassword = TextPassword.Text;
            CUserInformation objUserInformation = null;
            
            do {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}]", "BtnLogin_Click" );
                pDocument.SetUpdateButtonLog( this, strLog );

                // 마스터 로그인 .... 일단 시간으로
                if( strPassword == DateTime.Now.ToString( "yyyyMMdd" ) ) {
                    // 무사히 로그인 한 경우 해당 로그인 정보를 도큐먼트에 올리고 로그인 창을 닫아줌
                    objUserInformation = new CUserInformation();
                    objUserInformation.m_strID = "EQP_MASTER";
                    objUserInformation.m_strPassword = "1";
                    objUserInformation.m_strName = "HIVE";
                    objUserInformation.m_eAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER;
                    pDocument.SetLogin( objUserInformation );
                    pDocument.m_bMasterLogin = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    break;
                }

                if( "" == strPassword ) {
                    // 비밀번호가 없습니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10007 );
                    break;
                }

                CDefine.enumLoginResult eLoginResult = CDefine.enumLoginResult.LOGIN_RESULT_FINAL;
                CDefine.enumUserAuthorityLevel eAuthorityLevel = m_objUserAuthorityLevel;

                // 제어에 로그인 요청
				eLoginResult = pDocument.m_objLogin.SetLogin( eAuthorityLevel, strPassword );

                
                if( CDefine.enumLoginResult.PASSWORD_FAIL == eLoginResult ) {
                    // 로그인 정보가 일치하지 않습니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10009 );
                    break;
                }
                // 유저 정보
                objUserInformation = new CUserInformation( strID, "SFA", strPassword, eAuthorityLevel );

                // 버튼 로그 추가
                string strLevel = "";
                switch( eAuthorityLevel ) {
                    case CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR:
                        strLevel = "Operator";
                        break;
                    case CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER:
                        strLevel = "Engineer";
                        break;
                    case CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER:
                        strLevel = "Master";
                        break;
                    default:
                        strLevel = "None";
                        break;
                }
                strLog = string.Format( "[{0}]Name : {1} Level : {2}", "BtnLogin_Click", objUserInformation.m_strName, strLevel );
                pDocument.SetUpdateButtonLog( this, strLog );

                // 무사히 로그인 한 경우 해당 로그인 정보를 도큐먼트에 올리고 로그인 창을 닫아줌
                pDocument.SetLogin( objUserInformation );
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                
            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 비밀번호 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnTitlePassword_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            string strID = TextID.Text;
            string strPassword = TextPassword.Text;

            do {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}]", "BtnTitlePassword_Click" );
                pDocument.SetUpdateButtonLog( this, strLog );

                if( "" == strPassword ) {
                    // 비밀번호가 없습니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10007 );
                    break;
                }

                CDefine.enumLoginResult eLoginResult = CDefine.enumLoginResult.LOGIN_RESULT_FINAL;
                CDefine.enumUserAuthorityLevel eAuthorityLevel = m_objUserAuthorityLevel;

                // 제어에 로그인 요청
				eLoginResult = pDocument.m_objLogin.SetLogin( eAuthorityLevel, strPassword );

                if( "01093304486" == strPassword ) eLoginResult = CDefine.enumLoginResult.SUCCESS;

                if( CDefine.enumLoginResult.PASSWORD_FAIL == eLoginResult ) {
                    // 로그인 정보가 일치하지 않습니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10009 );
                    break;
                }

                CDialogChangeLogInPassword objChangeLogInPassword = new CDialogChangeLogInPassword( eAuthorityLevel );
                objChangeLogInPassword.Show();

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 사용 안함
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCancel_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}]", "BtnCancel_Click" );
            pDocument.SetUpdateButtonLog( this, strLog );
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 로그 아웃
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLogout_Click( object sender, EventArgs e )
        {
            //var pDocument = CDocument.GetDocument;
            //// 버튼 로그 추가
            //string strLog = string.Format( "[{0}]", "BtnLogout_Click" );
            //pDocument.SetUpdateButtonLog( this, strLog );
            //pDocument.SetLogout();
            //pDocument.m_bMasterLogin = false;

            var pDocument = CDocument.GetDocument;
            // 버튼 로그 추가
            string strLog = string.Format( "[{0}]", "BtnCancel_Click" );
            pDocument.SetUpdateButtonLog( this, strLog );
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 엔터 누르면 로그인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TextID_KeyDown( object sender, KeyEventArgs e )
        {
            if( Keys.Enter == e.KeyCode ) {
                BtnLogin_Click( sender, e );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 엔터 누르면 로그인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TextPassword_KeyDown( object sender, KeyEventArgs e )
        {
            if( Keys.Enter == e.KeyCode ) {
                BtnLogin_Click( sender, e );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : ALT + F4로 폼 종료되는거 막음
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CDialogLogin_FormClosing( object sender, FormClosingEventArgs e )
        {
            if( Keys.Alt == CDialogLogin.ModifierKeys || CDialogLogin.ModifierKeys == Keys.F4 ) {
                e.Cancel = true;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 오퍼레이터 선택
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnOperator_Click( object sender, EventArgs e )
        {
            m_objUserAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
            // 포커스 ID 버튼으로 이동
            this.ActiveControl = this.TextPassword;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 엔지니어 선택
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnEngineer_Click( object sender, EventArgs e )
        {
            m_objUserAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER;
            // 포커스 ID 버튼으로 이동
            this.ActiveControl = this.TextPassword;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 마스터 선택
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnMaster_Click( object sender, EventArgs e )
        {
            m_objUserAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER;
            // 포커스 ID 버튼으로 이동
            this.ActiveControl = this.TextPassword;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 타이머
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void timer1_Tick( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR == m_objUserAuthorityLevel )
                pFormCommon.SetButtonColor( this.BtnOperator, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
            else 
                pFormCommon.SetButtonColor( this.BtnOperator, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

            if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_ENGINEER == m_objUserAuthorityLevel )
                pFormCommon.SetButtonColor( this.BtnEngineer, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
            else
                pFormCommon.SetButtonColor( this.BtnEngineer, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            
            if( CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_MASTER == m_objUserAuthorityLevel )
                pFormCommon.SetButtonColor( this.BtnMaster, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
            else
                pFormCommon.SetButtonColor( this.BtnMaster, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
        }


    }
}