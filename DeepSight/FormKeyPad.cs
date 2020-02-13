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
    public partial class FormKeyPad : Form
    {

        /// <summary>
        /// 계산 방법
        /// </summary>
        enum enumCalulation
        {
            CAL_NONE = 0, CAL_PLUS, CAL_MINUS, CAL_DEVISION, CAL_MULTIPLY
        }

        /// <summary>
        /// 키패드
        /// </summary>
        enum enumKeyPad
        {
            KEYPAD_CHARACTER_0 = 0,
            KEYPAD_CHARACTER_1,
            KEYPAD_CHARACTER_2,
            KEYPAD_CHARACTER_3,
            KEYPAD_CHARACTER_4,
            KEYPAD_CHARACTER_5,
            KEYPAD_CHARACTER_6,
            KEYPAD_CHARACTER_7,
            KEYPAD_CHARACTER_8,
            KEYPAD_CHARACTER_9,
            KEYPAD_CHARACTER_HYPHEN,
            KEYPAD_CHARACTER_POINT,
            KEYPAD_CHARACTER_BACKSPACE,
            KEYPAD_CHARACTER_OK,
            KEYPAD_CHARACTER_CANCEL,
            KEYPAD_CHARACTER_A,
            KEYPAD_CHARACTER_B,
            KEYPAD_CHARACTER_C,
            KEYPAD_CHARACTER_D,
            KEYPAD_CHARACTER_E,
            KEYPAD_CHARACTER_F,
            KEYPAD_CHARACTER_CLEAR,
            KEYPAD_CHARACTER_DIVISION,
            KEYPAD_CHARACTER_EQUALS,
            KEYPAD_CHARACTER_PLUS,
            KEYPAD_CHARACTER_HEX,
            KEYPAD_CHARACTER_DEC,
            KEYPAD_CHARACTER_BIN,
            KEYPAD_CHARACTER_MULTIPLY
        };

        /// <summary>
        /// 계산 모드
        /// </summary>
        private enumCalulation m_eCalulation;
        private double _dValue;
        public double m_dValue
        {
            get
            {
                return _dValue;
            }
            set
            {
                _dValue = value;
            }
        }

        /// <summary>
        /// 바뀌기 전 값
        /// </summary>
        private double _dOriginValue;
        public double m_dOriginValue
        {
            get
            {
                return _dOriginValue;
            }
            set
            {
                _dOriginValue = value;
            }
        }

        /// <summary>
        /// 바뀔 값
        /// </summary>
        private string _strInputData;
        public string m_strInputData
        {
            get
            {
                return _strInputData;
            }
            set
            {
                _strInputData = value;
            }
        }

        /// <summary>
        /// 리턴 값
        /// </summary>
        private double _dResultValue;
        public double m_dResultValue
        {
            get
            {
                return _dResultValue;
            }
            set
            {
                _dResultValue = value;
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="dOriginValue"> 현재 변경되기 전의 값을 넣어준다.</param>
        public FormKeyPad( double dOriginValue )
        {
            InitializeComponent();

            m_dOriginValue = dOriginValue;
            // Form 초기화
            Initialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bReturn = false;
            do {
                string strOriginValue;
                strOriginValue = string.Format( "{0:F5}", m_dOriginValue );
                if( 0 != m_dOriginValue ) m_strInputData = string.Format( "{0:F5}", m_dOriginValue );
                else m_strInputData = "0";

                m_eCalulation = enumCalulation.CAL_NONE;

                Reload();

                BtnDisplayOriginValue.Text = strOriginValue;

                //BtnKeyPadTitle.BackColor = Color.Gray;

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

            pFormCommon.SetButtonColor( this.BtnKeyPadTitle, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonBackColor( this.BtnDisPlayKeyValue, pFormCommon.COLOR_WHITE );
            pFormCommon.SetButtonBackColor( this.BtnDisplayOriginValue, pFormCommon.COLOR_WHITE );

            pFormCommon.SetButtonColor( this.BtnKeyPadMinusDotOne, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadPlusDotOne, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadMinusDotFive, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadPlusDotFive, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadMinusOne, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadPlusOne, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadMinusTen, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadPlusTen, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadClear, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar7, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar8, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar9, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar6, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar5, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar4, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar3, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar2, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadChar0, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadPoint, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadEquals, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadOK, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadBackSpace, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadDivision, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadMutiply, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadHyphen, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadPlus, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyPadCancel, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
        }

        /// <summary>
        /// 계산 모드에 따라 버튼 이름 및 활성화.
        /// </summary>
        /// <param name="bCalMode"></param>
        private void Reload( bool bCalMode = false )
        {
            if( true == bCalMode ) {
                BtnKeyPadChar0.Enabled = true;
                BtnKeyPadChar1.Enabled = true;
                BtnKeyPadChar2.Enabled = true;
                BtnKeyPadChar3.Enabled = true;
                BtnKeyPadChar4.Enabled = true;
                BtnKeyPadChar5.Enabled = true;
                BtnKeyPadChar6.Enabled = true;
                BtnKeyPadChar7.Enabled = true;
                BtnKeyPadChar8.Enabled = true;
                BtnKeyPadChar9.Enabled = true;

                BtnKeyPadMinusDotOne.Enabled = false;
                BtnKeyPadPlusDotOne.Enabled = false;
                BtnKeyPadMinusDotFive.Enabled = false;
                BtnKeyPadPlusDotFive.Enabled = false;
                BtnKeyPadMinusOne.Enabled = false;
                BtnKeyPadPlusOne.Enabled = false;

                BtnKeyPadDivision.Enabled = true;
                BtnKeyPadMutiply.Enabled = true;
                BtnKeyPadHyphen.Enabled = true;
                BtnKeyPadPlus.Enabled = true;
                BtnKeyPadEquals.Enabled = true;
                BtnKeyPadPoint.Enabled = true;
            }

            BtnDisPlayKeyValue.Text = m_strInputData;
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadClear_Click( object sender, EventArgs e )
        {
            m_strInputData = "0";
            m_dValue = 0;
            m_eCalulation = enumCalulation.CAL_NONE;
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadBackSpace_Click( object sender, EventArgs e )
        {
            if( 0 == m_strInputData.Length )
                return;
            if( 1 == m_strInputData.Length )
                m_strInputData = "0";
            if( 1 < m_strInputData.Length ) {
                string strConvertText = "";
                strConvertText = m_strInputData.Substring( 0, m_strInputData.Length - 1 );
                m_strInputData = strConvertText;
            }

            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadDivision_Click( object sender, EventArgs e )
        {
            if( "0" == m_strInputData && 0 == m_dValue )
                return;
            m_dValue = Convert.ToDouble( m_strInputData );
            m_eCalulation = enumCalulation.CAL_DEVISION;
            m_strInputData = "0";
        }


        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMutiply_Click( object sender, EventArgs e )
        {
            if( "0" == m_strInputData && 0 == m_dValue )
                return;
            m_dValue = Convert.ToDouble( m_strInputData );
            m_eCalulation = enumCalulation.CAL_MULTIPLY;
            m_strInputData = "0";
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadHyphen_Click( object sender, EventArgs e )
        {
            if( "0" == m_strInputData && 0 == m_dValue ) {
                m_strInputData = "-";
                Reload();
            } else {
                m_dValue = Convert.ToDouble( m_strInputData );
                m_eCalulation = enumCalulation.CAL_MINUS;
                m_strInputData = "0";
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlus_Click( object sender, EventArgs e )
        {
            if( "0" == m_strInputData && 0 == m_dValue )
                return;
            m_dValue = Convert.ToDouble( m_strInputData );
            m_eCalulation = enumCalulation.CAL_PLUS;
            m_strInputData = "0";
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadEquals_Click( object sender, EventArgs e )
        {
            double dValue = 0.0;

            m_dValue = Convert.ToDouble( m_strInputData );

            switch( m_eCalulation ) {
                case enumCalulation.CAL_PLUS:
                    m_dValue = m_dValue + dValue;
                    break;
                case enumCalulation.CAL_MINUS:
                    m_dValue = m_dValue - dValue;
                    break;
                case enumCalulation.CAL_MULTIPLY:
                    m_dValue = m_dValue * dValue;
                    break;
                case enumCalulation.CAL_DEVISION:
                    m_dValue = m_dValue / dValue;
                    break;
                case enumCalulation.CAL_NONE:
                    break;
            }

            m_eCalulation = enumCalulation.CAL_NONE;
            string strBuf;
            strBuf = string.Format( "{0:F5}", m_dValue );

            m_dValue = Convert.ToDouble( m_strInputData );

            Reload();

        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadOK_Click( object sender, EventArgs e )
        {
            double dValue = 0.0;

            m_dValue = Convert.ToDouble( m_strInputData );

            switch( m_eCalulation ) {
                case enumCalulation.CAL_PLUS:
                    m_dValue = m_dValue + dValue;
                    break;
                case enumCalulation.CAL_MINUS:
                    m_dValue = m_dValue - dValue;
                    break;
                case enumCalulation.CAL_MULTIPLY:
                    m_dValue = m_dValue * dValue;
                    break;
                case enumCalulation.CAL_DEVISION:
                    m_dValue = m_dValue / dValue;
                    break;
                case enumCalulation.CAL_NONE:
                    m_dValue = dValue;
                    break;
            }

            m_eCalulation = enumCalulation.CAL_NONE;
            string strBuf;
            strBuf = string.Format( "{0:F5}", m_dValue );

            m_dValue = Convert.ToDouble( m_strInputData );

            Reload();

            m_dResultValue = m_dValue;

            this.DialogResult = DialogResult.OK;

            FormKeyPad.ActiveForm.Close();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPoint_Click( object sender, EventArgs e )
        {
            int iIndex = 0;
            iIndex = m_strInputData.IndexOf( "." );
            if( -1 == iIndex ) {
                m_strInputData += ".";
                Reload();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar0_Click( object sender, EventArgs e )
        {
            bool bEquals = false;
            bEquals = m_strInputData.Equals( "0" );
            if( false == bEquals ) {
                if( m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                    m_strInputData = "";
                }
                m_strInputData += "0";
                Reload();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar1_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "1";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar2_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "2";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar3_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "3";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar4_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "4";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar5_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "5";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar6_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "6";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar7_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "7";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar8_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "8";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar9_Click( object sender, EventArgs e )
        {
            if( true == m_strInputData.Equals( "0" ) || m_strInputData == string.Format( "{0:F5}", m_dOriginValue ) ) {
                m_strInputData = "";
            }
            m_strInputData += "9";
            Reload();
        }

        /// <summary>
        /// -0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusDotOne_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) - 0.1 );
            Reload();
        }

        /// <summary>
        /// +0.1 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusDotOne_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) + 0.1 );
            Reload();
        }

        /// <summary>
        /// -0.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusDotFive_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) - 0.5 );
            Reload();
        }

        /// <summary>
        /// +0.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusDotFive_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) + 0.5 );
            Reload();
        }

        /// <summary>
        /// -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusOne_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) - 1.0 );
            Reload();
        }

        /// <summary>
        /// +1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusOne_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) + 1.0 );
            Reload();
        }

        /// <summary>
        /// -10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusTen_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) - 10.0 );
            Reload();
        }

        /// <summary>
        /// +10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusTen_Click( object sender, EventArgs e )
        {
            m_strInputData = string.Format( "{0:F5}", double.Parse( m_strInputData ) + 10.0 );
            Reload();
        }
        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            FormKeyPad.ActiveForm.Close();
        }

        /// <summary>
        /// enter 키로 인한 값 변경 되지 않도록 함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPad_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {
            if( e.KeyCode == Keys.Enter )
                e.IsInputKey = true;
        }
    }
}
