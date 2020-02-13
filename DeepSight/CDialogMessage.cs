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
    public partial class CDialogMessage : Form
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // key or string
        public enum enumKeyString { KEY = 0, STRING };
        // 알람 구조체 정보
        private CDefine.structureAlarmInformation m_objAlarmInformation;
        // Key 로 검색하는지 일반 메세지를 받을 지
        private enumKeyString m_eKeyString;
        // 폰트 크기 설정
        private const double m_dFontSizeTitleAlarmType = 48.0;
        private const double m_dFontSizeTitleAlarmEtc = 15.75;
        private const double m_dFontSizeAlarmDescription = 15.0;
        // 폰트 이름 설정
        private const string m_strFontName = "맑은 고딕";
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDialogMessage( CDefine.structureAlarmInformation objAlarmInformation )
        {   
            // 알람 구조체 정보 받음
            m_objAlarmInformation = objAlarmInformation;
            // 알람 구조체에 알람 설명이 없는 경우 키로 검색
            if( "" == m_objAlarmInformation.strAlarmDescription || null == m_objAlarmInformation.strAlarmDescription ) {
                // 키로 검색
                m_eKeyString = enumKeyString.KEY;
            }
            else {
                // 알람 설명 그대로 적어줌
                m_eKeyString = enumKeyString.STRING;
            }
            
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 폼 로드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CDialogMessage_Load( object sender, EventArgs e )
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
        private void CDialogMessage_FormClosed( object sender, FormClosedEventArgs e )
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
            var pDocument = CDocument.GetDocument;
            bool bReturn = false;

            do {
                // 폼 중앙에서 생성
                this.CenterToParent();
                // 폼 가장 위로
                this.TopMost = true;
                // 텍스트 박스's 초기화
                if( false == InitializeTextBox( this.TextBoxTitleAlarmType, m_strFontName, m_dFontSizeTitleAlarmType ) ) break;
                if( false == InitializeTextBox( this.TextBoxTitleAlarmTime, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxTitleAlarmCode, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxTitleAlarmObject, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxTitleAlarmPosition, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxAlarmTime, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxAlarmCode, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxAlarmObject, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxAlarmPosition, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                if( false == InitializeTextBox( this.TextBoxTitleAlarmDescription, m_strFontName, m_dFontSizeTitleAlarmEtc ) ) break;
                // 텍스트 박스 설정 초기화
                if( false == InitializeRichTextBox( this.RichTextBoxAlarmDescriptionKorea ) ) break;
                if( false == InitializeRichTextBox( this.RichTextBoxAlarmDescriptionVietnam ) ) break;

                // 알람 시간
                this.TextBoxAlarmTime.Text = DateTime.Now.ToString( CDatabaseDefine.DEF_DATE_TIME_FORMAT );
                // 알람 코드
                this.TextBoxAlarmCode.Text = string.Format( "{0}", m_objAlarmInformation.iAlarmCode );
                // 알람 오브젝트
                this.TextBoxAlarmObject.Text = m_objAlarmInformation.strAlarmObject;
                // 알람 위치
                this.TextBoxAlarmPosition.Text = m_objAlarmInformation.strAlarmFunction;

                // Key
                if( enumKeyString.KEY == m_eKeyString ) {
                    // 유저 메세지 데이터 테이블에서 언어 Row값 뽑아옴
                    CManagerTable objManagerTable = pDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUserMessage;

                    try {
                        DataTable objDataTable = objManagerTable.HLGetDataTable();
                        DataRow[] objDataRow = objDataTable.Select( string.Format( "{0} = '{1}'", objManagerTable.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumInformationUserMessage.ID ], m_objAlarmInformation.iAlarmCode ) );
                        // 한국어 뿌려줌
                        this.RichTextBoxAlarmDescriptionKorea.Text = objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUserMessage.TEXT_KOREA ].ToString();
                        // 언어에 따른 코드뿌려줌
                        CConfig.CSystemParameter objOptionParameter = pDocument.m_objConfig.GetSystemParameter();
                        this.RichTextBoxAlarmDescriptionVietnam.Text = objDataRow[ 0 ].ItemArray[ ( ( int )objOptionParameter.eLanguage ) + 1 ].ToString();
                        // 로그
                        pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, objDataRow[ 0 ].ItemArray[ ( int )CDatabaseDefine.enumInformationUserMessage.TEXT_KOREA ].ToString() );
                    }
                    catch( Exception ex ) {
                        Trace.WriteLine( ex.Message );
                    }
                }
                // String
                else if( enumKeyString.STRING == m_eKeyString ) {
                    // 입력 받은 문자열 뿌려줌
                    this.RichTextBoxAlarmDescriptionKorea.Text = m_objAlarmInformation.strAlarmDescription;
                    // 로그
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, m_objAlarmInformation.strAlarmDescription );
                }
                // 버튼 색상 정의
                SetButtonColor();
                // 버튼 언어 변경
                SetChangeLanguage();

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
			pFormCommon.SetButtonBackColor( this.BtnYes, pFormCommon.m_colorNormal );
			pFormCommon.SetButtonBackColor( this.BtnNo, pFormCommon.m_colorNormal );
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
                // 알람 타입
                switch( m_objAlarmInformation.eAlarmLevel ) {
                    case CDefine.enumAlarmType.ALARM_INFORMATION:
                        SetTextBoxChangeLanguage( this.TextBoxTitleAlarmType, "TextBoxTitleAlarmTypeInformation" );
                        break;
                    case CDefine.enumAlarmType.ALARM_WARNING:
                        SetTextBoxChangeLanguage( this.TextBoxTitleAlarmType, "TextBoxTitleAlarmTypeWarning" );
                        break;
                    case CDefine.enumAlarmType.ALARM_ALARM:
                        SetTextBoxChangeLanguage( this.TextBoxTitleAlarmType, "TextBoxTitleAlarmTypeAlarm" );
                        break;
                    case CDefine.enumAlarmType.ALARM_INTERLOCK:
                        SetTextBoxChangeLanguage( this.TextBoxTitleAlarmType, "TextBoxTitleAlarmTypeInterlock" );
                        break;
                    case CDefine.enumAlarmType.ALARM_QUESTION:
                        SetTextBoxChangeLanguage( this.TextBoxTitleAlarmType, "TextBoxTitleAlarmTypeQuestion" );
                        break;
                    case CDefine.enumAlarmType.ALARM_MOTOR:
                        SetTextBoxChangeLanguage( this.TextBoxTitleAlarmType, "TextBoxTitleAlarmTypeMotor" );
                        break;
                    default:
                        break;
                }
                // 알람 시간
                SetTextBoxChangeLanguage( this.TextBoxTitleAlarmTime, this.TextBoxTitleAlarmTime.Name );
                // 알람 코드
                SetTextBoxChangeLanguage( this.TextBoxTitleAlarmCode, this.TextBoxTitleAlarmCode.Name );
                // 알람 객체
                SetTextBoxChangeLanguage( this.TextBoxTitleAlarmObject, this.TextBoxTitleAlarmObject.Name );
                // 알람 위치
                SetTextBoxChangeLanguage( this.TextBoxTitleAlarmPosition, this.TextBoxTitleAlarmPosition.Name );
                // 알람 타입에 따라 다른 언어를 불러옴
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                if( CDefine.enumAlarmType.ALARM_QUESTION != m_objAlarmInformation.eAlarmLevel ) {
                    this.BtnYes.Visible = false;
                    SetControlChangeLanguage( this.BtnNo, "BtnOK" );
                }
                else {
                    SetControlChangeLanguage( this.BtnYes, "BtnYes" );
                    SetControlChangeLanguage( this.BtnNo, "BtnNo" );
                }
                
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
        private void SetControlChangeLanguage( Button objButton, string strKey )
        {
            var pDocument = CDocument.GetDocument;
			var pFormCommon = CFormCommon.GetFormCommon;
			pFormCommon.SetButtonText( objButton, pDocument.GetDatabaseUIText( strKey, this.Name ) );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 언어 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetTextBoxChangeLanguage( TextBox objTextBox, string strKey )
        {
            var pDocument = CDocument.GetDocument;
            objTextBox.Text = pDocument.GetDatabaseUIText( strKey, this.Name );
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
        //목적 : 텍스트 박스's 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool InitializeTextBox( TextBox objTextBox, string strFontName, double dSize )
        {
            bool bReturn = false;

            do {
                // 읽기만
                objTextBox.ReadOnly = true;
                // 탭 포커스 이동 x
                objTextBox.TabStop = false;
                // 폰트 변경
                objTextBox.Font = new Font( strFontName, ( float )dSize, objTextBox.Font.Style );
                
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool InitializeRichTextBox( RichTextBox objRich )
        {
            bool bReturn = false;

            do {
				var pFormCommon = CFormCommon.GetFormCommon;
                // 리치 텍스트 박스 기본 스타일 초기화
				if( false == pFormCommon.InitializeRichTextBox( objRich ) ) break;
                // 리치 텍스트 박스 크기 변경
				pFormCommon.SetRichTextBoxFont( objRich, m_strFontName, m_dFontSizeAlarmDescription, objRich.Font.Style );
                
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 포커스 오지마라
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetDisableFocus( Control objControl )
        {
            objControl.Enabled = false;
            objControl.Enabled = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : YES or OK
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnYes_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            if( CDefine.enumAlarmType.ALARM_QUESTION == m_objAlarmInformation.eAlarmLevel ) {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [{1}]", "BtnYes_Click", DialogResult.Yes.ToString() );
                pDocument.SetUpdateButtonLog( this, strLog );

                CDialogMessage.ActiveForm.DialogResult = DialogResult.Yes;
                CDialogMessage.ActiveForm.Close();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : NO or CANCEL or OK
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnNo_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            if( CDefine.enumAlarmType.ALARM_QUESTION == m_objAlarmInformation.eAlarmLevel ) {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [{1}]", "BtnNo_Click", DialogResult.No.ToString() );
                pDocument.SetUpdateButtonLog( this, strLog );

                CDialogMessage.ActiveForm.DialogResult = DialogResult.No;
                CDialogMessage.ActiveForm.Close();
            }
            else {
                // 버튼 로그 추가
                string strLog = string.Format( "[{0}] [{1}]", "BtnNo_Click", DialogResult.OK.ToString() );
                pDocument.SetUpdateButtonLog( this, strLog );

                CDialogMessage.ActiveForm.DialogResult = DialogResult.OK;
                CDialogMessage.ActiveForm.Close();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 포커스
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RichTextBoxKorea_Enter( object sender, EventArgs e )
        {
            RichTextBox objTextBox = sender as RichTextBox;
            SetDisableFocus( objTextBox );
        }

        private void RichTextBoxVietnam_Enter( object sender, EventArgs e )
        {
            RichTextBox objTextBox = sender as RichTextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxTitleAlarmType_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxTitleAlarmTime_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxTitleAlarmCode_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxTitleAlarmObject_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxTitleAlarmPosition_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxAlarmTime_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxAlarmCode_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxAlarmObject_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxAlarmPosition_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }

        private void TextBoxTitleAlarmDescription_Enter( object sender, EventArgs e )
        {
            TextBox objTextBox = sender as TextBox;
            SetDisableFocus( objTextBox );
        }
    }
}