using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSight
{
    public partial class CDialogChangeLogInPassword : Form
    {
        CDefine.enumUserAuthorityLevel m_objUserAuthorityLevel;
        public CDialogChangeLogInPassword( CDefine.enumUserAuthorityLevel eUserAuthorityLevel )
        {
            InitializeComponent();
            m_objUserAuthorityLevel = eUserAuthorityLevel;
            Initialize();
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
              //  m_objUserAuthorityLevel = CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_OPERATOR;
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
                // 폼 중앙에서 생성
                this.CenterToParent();
                // 현재 로그인 된 유저 정보를 얻어서 뿌려줘야 함.
                TextPassword.Text = "";
                // 포커스 ID 버튼으로 이동
                this.ActiveControl = this.TextPassword;
                // 버튼 색상 정의
                SetButtonColor();

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

            pFormCommon.SetButtonColor( this.BtnTitlePassword, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            TextPassword.BackColor = pFormCommon.COLOR_UNACTIVATE;
            TextPassword.ForeColor = pFormCommon.COLOR_WHITE;

            pFormCommon.SetButtonColor( this.BtnOK, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCancel, pFormCommon.COLOR_WHITE, pFormCommon.LAMP_RED_OFF );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 비밀번호 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnOK_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            CDefine.enumLoginResult eLoginResult = CDefine.enumLoginResult.LOGIN_RESULT_FINAL;
            CDefine.enumUserAuthorityLevel eAuthorityLevel = m_objUserAuthorityLevel;

            do {
                string strPassword = TextPassword.Text;
                if( "" == strPassword ) {
                    // 비밀번호가 없습니다.
                    pDocument.SetMessage( CDefine.enumAlarmType.ALARM_INFORMATION, 10007 );
                    break;
                }
                // 암호 변경
				eLoginResult = pDocument.m_objLogin.SetChangePassword( eAuthorityLevel, strPassword );
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            } while( false );

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 취소
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
