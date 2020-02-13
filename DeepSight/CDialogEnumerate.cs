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
    public partial class CDialogEnumerate : Form
    {
        private const int DEF_MAX_BUTTON_COUNT = 30;
        private int m_iCount;
        private int m_iCurrentIndex;
        private string[] m_strList;
        private Size m_objButtonSize;
        private Button[] m_objButton;
        public CDialogEnumerate( int iCount, string[] strList, int iCurrentIndex )
        {
            InitializeComponent();


            if ( DEF_MAX_BUTTON_COUNT < iCount || iCount < strList.Length )
            {
                CDialogEnumerate.ActiveForm.DialogResult = DialogResult.No;
                CDialogEnumerate.ActiveForm.Close();
            }

            m_iCount = iCount;
            m_strList = strList.Clone() as string[];
            m_iCurrentIndex = iCurrentIndex;
            Initialize();

        }

        private bool Initialize()
        {
            bool bReturn = false;

            do
            {
                // 버튼 사이즈
                m_objButtonSize.Width = 380;
                m_objButtonSize.Height = 100;
                
                //다이얼로그 크그에 맞게 생성
                // 10개씩 총 3줄만 보여주자
                // 10개 미만일 경우에는 한줄로 표시
                if( 0 == ( m_iCount / 10 ) )
                    this.Width = 380 * ( ( m_iCount / 10 ) + 1 ) + 6/*마진*/;
                else if( 0 != m_iCount%10 && m_iCount > 10 )
                    this.Width = 380 * ( ( m_iCount / 10 ) + 1 ) + 6/*마진*/;
                else
                    this.Width = 380 * ( ( m_iCount / 10 ) ) + 6/*마진*/;

                if ( 10 <= m_iCount )
                    this.Height = ( 100 * 10 ) + 6/*마진*/;
                else
                    this.Height = ( 100 * m_iCount ) + 6/*마진*/;

                SetButton();


                bReturn = true;
            } while ( false );

            return bReturn;
        }

        private void SetButton()
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            this.BackColor = pFormCommon.COLOR_FORM_VIEW;

            m_objButton = new Button[ m_iCount ];
            for ( int iLoopCount = 0; iLoopCount < m_objButton.Length; iLoopCount++ )
            {
                m_objButton[ iLoopCount ] = new Button();
                this.Controls.Add( m_objButton[ iLoopCount ] );
                m_objButton[ iLoopCount ].Size = m_objButtonSize;
                m_objButton[ iLoopCount ].Location = new Point( 3 + m_objButtonSize.Width * ( iLoopCount / 10 ), 3 + m_objButtonSize.Height * ( iLoopCount % 10 ) );
                m_objButton[ iLoopCount ].MouseClick += CDialogEnumerate_MouseClick;
                pFormCommon.SetButtonText( m_objButton[ iLoopCount ], m_strList[ iLoopCount ] );

                if( iLoopCount == m_iCurrentIndex )
                    pFormCommon.SetButtonColor( m_objButton[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_ACTIVATE );
                else
                    pFormCommon.SetButtonColor( m_objButton[ iLoopCount ], pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            }

            
        }

        private void CDialogEnumerate_MouseClick( object sender, MouseEventArgs e )
        {
            Button btn = ( Button )sender;
            for ( int iLoopCount = 0; iLoopCount < m_objButton.Length; iLoopCount++ )
            {
                if ( btn.Location == m_objButton[ iLoopCount ].Location )
                {
                    m_iCurrentIndex = iLoopCount;
                    break;
                }
            }

            CDialogEnumerate.ActiveForm.DialogResult = DialogResult.OK;
            CDialogEnumerate.ActiveForm.Close();
        }

        public int GetResult()
        {
            return m_iCurrentIndex;
        }
    }
}
