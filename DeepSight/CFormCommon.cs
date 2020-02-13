using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DeepSight
{
    // 폼에서 공통으로 쓸 함수를 처리하기 위해 빼 놓음
    public partial class CFormCommon : Form
    {
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static CFormCommon m_pFormCommon = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 컬러 정의
        public Color m_colorRed = Color.FromArgb( 255, 192, 192 );
        public Color m_colorGreen = Color.FromArgb( 192, 255, 192 );
        public Color m_colorYellow = Color.FromArgb( 255, 255, 192 );
        public Color m_colorNormal = Color.FromArgb( 230, 230, 230 );
        public Color m_colorClick = Color.FromArgb( 180, 180, 180 );
        public Color m_colorLabel = Color.FromArgb( 128, 128, 255 );
        public Color m_colorLabelSub = Color.FromArgb( 192, 192, 255 );
        public Color m_colorLabelData = Color.FromArgb( 255, 255, 255 );
        public Color m_colorOn = Color.FromArgb( 192, 255, 192 );
        public Color m_colorOff = Color.FromArgb( 255, 192, 192 );
        public Color m_objBlue = Color.Blue;
        

        public Color COLOR_FORM_TITLE = Color.FromArgb( 005, 005, 005 );
        public Color COLOR_FORM_MENU = Color.FromArgb( 015, 015, 015 );
        public Color COLOR_FORM_VIEW = Color.FromArgb( 025, 025, 025 );
        public Color COLOR_FORM_DIALOG = Color.FromArgb( 025, 025, 025 );
        public Color COLOR_DIALOG_MONITOR = Color.FromArgb( 025, 025, 025 );
        public Color COLOR_TITLE = Color.FromArgb( 005, 005, 005 );
        public Color COLOR_GRAY = Color.FromArgb( 180, 180, 180 );
        public Color COLOR_TEXT = Color.FromArgb( 025, 025, 040 );
        public Color COLOR_ACTIVATE = Color.FromArgb( 000, 84, 255 );
        public Color COLOR_UNACTIVATE = Color.FromArgb( 50, 50, 50 );
        public Color COLOR_NORMAL = Color.FromArgb( 000, 000, 000 );
        public Color COLOR_STOP = Color.FromArgb( 255, 000, 000 );
        public Color COLOR_BLACK	= Color.FromArgb( 000, 000, 000 );
        public Color COLOR_BLUE = Color.FromArgb( 000, 000, 75 );
        public Color COLOR_GREEN	= Color.FromArgb( 29, 219, 22 );
        public Color COLOR_RED = Color.FromArgb( 255, 000, 000 );
        public Color COLOR_WHITE = Color.FromArgb( 255, 255, 255 );
        public Color COLOR_YELLOW = Color.FromArgb( 255, 228, 000 );
        public Color LAMP_GREEN_ON = Color.FromArgb( 000, 180, 000 );
        public Color LAMP_GREEN_OFF = Color.FromArgb( 000, 100, 000 );

        public Color LAMP_YELLOW_ON	 = Color.FromArgb( 255, 255, 000 );
        public Color LAMP_YELLOW_OFF = Color.FromArgb( 125, 125, 000 );
        public Color LAMP_RED_ON = Color.FromArgb( 255, 000, 000 );
        public Color LAMP_RED_OFF = Color.FromArgb( 100, 000, 000 );
        public Color LAMP_BLUE_ON = Color.FromArgb( 000, 000, 255 );
        public Color LAMP_BLUE_OFF = Color.FromArgb( 000, 000, 75 );



        // 유저 권한 레벨에 따른 버튼 상태 변경
        public delegate void DelegateSetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection );
        public DelegateSetChangeButtonStatus m_delegateSetChangeButtonStatus = null;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 인스턴스를 불러옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static CFormCommon GetFormCommon
		{
			get
			{
				if( null == m_pFormCommon ) {
					m_pFormCommon = new CFormCommon();
				}
				return m_pFormCommon;
			}
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        //설명 : 같은 색인 경우 넘김
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetButtonColor( Button objBtn, Color objForeColor, Color objBackColor )
        {
            if( objForeColor != objBtn.ForeColor ) {
                objBtn.ForeColor = objForeColor;
            }
            if( objBackColor != objBtn.BackColor ) {
                objBtn.BackColor = objBackColor;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        //설명 : 같은 색인 경우 넘김
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetButtonBackColor( Button objBtn, Color objBackColor )
        {
            SetButtonColor( objBtn, objBtn.ForeColor, objBackColor );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼에 색 입힘 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        //설명 : 같은 색인 경우 넘김
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetButtonForeColor( Button objBtn, Color objForeColor )
        {
            SetButtonColor( objBtn, objForeColor, objBtn.BackColor );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼에 문자열 변경 (나중에 Button 클래스가 다른 클래스로 대체될 수 있음)
        //설명 : 같은 문자열인 경우 넘김
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetButtonText( Button objBtn, string strText )
        {
            if( strText != objBtn.Text ) {
                objBtn.Text = strText;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 폰트 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewFont( DataGridView objGridView, string strFontName, double dSize, FontStyle objFontStyle )
        {
            objGridView.Font = new Font( strFontName, ( float )dSize, objFontStyle );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 폰트 크기 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewFont( DataGridView objGridView, double dSize )
        {
            SetGridViewFont( objGridView, objGridView.Font.Name, dSize, objGridView.Font.Style );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 폰트 이름 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewFont( DataGridView objGridView, string strFontName )
        {
            SetGridViewFont( objGridView, strFontName, ( double )objGridView.Font.Size, objGridView.Font.Style );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 폰트 스타일 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewFont( DataGridView objGridView, FontStyle objFontStyle )
        {
            SetGridViewFont( objGridView, objGridView.Font.Name, ( double )objGridView.Font.Size, objFontStyle );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 셀 데이터 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewCellData( DataGridView objGridView, int iColumnIndex, int iRowIndex, int iData )
        {
            try {
                if( iData != Convert.ToInt32( objGridView[ iColumnIndex, iRowIndex ].Value ) ) {
                    objGridView[ iColumnIndex, iRowIndex ].Value = iData;
                }
                // null 값 object value값은 변환 시 0값으로 처리되기에 따로 처리되어야 함.
                else if( null == objGridView[ iColumnIndex, iRowIndex ].Value && 0 == iData ) {
                    objGridView[ iColumnIndex, iRowIndex ].Value = iData;
                }
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.StackTrace );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 셀 데이터 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewCellData( DataGridView objGridView, int iColumnIndex, int iRowIndex, double dData )
        {
            try {
                if( dData != Convert.ToDouble( objGridView[ iColumnIndex, iRowIndex ].Value ) ) {
                    objGridView[ iColumnIndex, iRowIndex ].Value = dData;
                }
                // null 값 object value값은 변환 시 0값으로 처리되기에 따로 처리되어야 함.
                else if( null == objGridView[ iColumnIndex, iRowIndex ].Value && 0.0 == dData ) {
                    objGridView[ iColumnIndex, iRowIndex ].Value = dData;
                }
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.StackTrace );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 셀 데이터 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewCellData( DataGridView objGridView, int iColumnIndex, int iRowIndex, string strData )
        {
            try {
                if( null == objGridView[ iColumnIndex, iRowIndex ].Value ) {
                    objGridView[ iColumnIndex, iRowIndex ].Value = strData;
                }
                else if( strData != objGridView[ iColumnIndex, iRowIndex ].Value.ToString() ) {
                    objGridView[ iColumnIndex, iRowIndex ].Value = strData;
                }
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.StackTrace );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 배경색 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetGridViewCellBackColor( DataGridView objGridView, int iColumnIndex, int iRowIndex, Color objBackColor )
        {
            try {
                objGridView[ iColumnIndex, iRowIndex ].Style.BackColor = objBackColor;
            }
            catch( Exception ex ) {
                Trace.WriteLine( ex.StackTrace );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 기본 스타일 초기화
        //설명 : 그리드 뷰 생성 시 기본 제약 조건으로 사용합니다. 해당 부분이 수정되면 다른 부분에도 영향이 갑니다.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool InitializeGridView( DataGridView objGridView )
        {
            bool bReturn = false;

            do {
                // 더블 버퍼링으로 속성 변경
                SetDoubleBuffered( objGridView, true );
                // 그리드 뷰 배경색
                objGridView.BackgroundColor = Color.White;
                // 그리드 뷰 칼럼 사이즈 조정
                objGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // 그리드 뷰 행, 열 사이즈 유저 조정 막음
                objGridView.AllowUserToResizeRows = false;
                objGridView.AllowUserToResizeColumns = false;
                // 그리드 뷰 행 머리글 없앰
                objGridView.RowHeadersVisible = false;
                // 그리드 뷰 홀수행 색 변경
                objGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                // 첫 행 포커스 해제
                objGridView.ClearSelection();
                // 마지막 행 제거
                objGridView.AllowUserToAddRows = false;

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 그리드 뷰 더블 버퍼링 속성 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDoubleBuffered( DataGridView objGridView, bool bSetting )
        {
            Type objType = objGridView.GetType();
            PropertyInfo objPropertyInfo = objType.GetProperty( "DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic );
            objPropertyInfo.SetValue( objGridView, bSetting, null );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 기본 스타일 초기화
        //설명 : 리치 텍스트 박스 생성 시 기본 제약 조건으로 사용합니다. 해당 부분이 수정되면 다른 부분에도 영향이 갑니다.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool InitializeRichTextBox( RichTextBox objRich )
        {
            bool bReturn = false;

            do {
                // 읽기만 가능
                objRich.ReadOnly = true;
                // 복수 라인
                objRich.Multiline = true;
                // 스크롤바 세로
                objRich.ScrollBars = RichTextBoxScrollBars.Vertical;

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 폰트 변경
        //설명 : Control 단위로 빼줄 걸 그랬나.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetRichTextBoxFont( RichTextBox objRich, string strFontName, double dSize, FontStyle objFontStyle )
        {
            objRich.Font = new Font( strFontName, ( float )dSize, objFontStyle );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 폰트 크기 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetRichTextBoxFont( RichTextBox objRich, double dSize )
        {
            SetRichTextBoxFont( objRich, objRich.Font.Name, dSize, objRich.Font.Style );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 폰트 이름 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetRichTextBoxFont( RichTextBox objRich, string strFontName )
        {
            SetRichTextBoxFont( objRich, strFontName, ( double )objRich.Font.Size, objRich.Font.Style );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 폰트 스타일 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetRichTextBoxFont( RichTextBox objRich, FontStyle objFontStyle )
        {
            SetRichTextBoxFont( objRich, objRich.Font.Name, ( double )objRich.Font.Size, objFontStyle );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 리치 텍스트 박스 텍스트 삽입 ~ 가장 최신이 위로 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetRichText( RichTextBox objRich, Queue<string> queueText )
        {
            // queue 쌓이는 Max 255로
            if( 255 <= queueText.Count ) {
                queueText.Dequeue();
            }

            string strText = "";
            string[] strTextArray = queueText.ToArray();
            for( int iLoopText = strTextArray.Length - 1; iLoopText >= 0; iLoopText-- ) {
                strText += strTextArray[ iLoopText ] + "\n";
            }
            objRich.Text = strText;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 메뉴를 갖는 폼들이 메뉴 버튼을 동적으로 생성하기 위한 함수
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetDynamicMenuButton( Button[] objButton, Control objParent, string[] strButtonName, int iButtonWidth, int iWhiteSpace, EventHandler eventButton )
        {
            for( int iLoopButton = 0; iLoopButton < strButtonName.Length; iLoopButton++ ) {
                objButton[ iLoopButton ] = new Button();
                objButton[ iLoopButton ].Name = iLoopButton.ToString();
                objButton[ iLoopButton ].Text = strButtonName[ iLoopButton ];
                objButton[ iLoopButton ].Parent = objParent;
                objButton[ iLoopButton ].Size = new Size( iButtonWidth - iWhiteSpace, objParent.Height );
                objButton[ iLoopButton ].BackColor = Color.White;
                objButton[ iLoopButton ].FlatStyle = FlatStyle.Flat;
                objButton[ iLoopButton ].Click += eventButton;
                // 버튼 오른쪽으로 열거
                objButton[ iLoopButton ].Location = new Point( iLoopButton * iButtonWidth, 0 );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 유저 권한 레벨에 따라 버튼 상태 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection, CDefine.FormView eFormView )
        {
            do {
                // 현재 폼 내에 생성된 컨트롤 리스트를 뽑음
                Control.ControlCollection objCollection = collection;
                // 그 중에서 Button & ImageButton만 사용할 예정
                List<Control> objButtonList = new List<Control>();
                // Button or ImageButton Type인 놈만 List에 저장
                for( int iLoopCollection = 0; iLoopCollection < objCollection.Count; iLoopCollection++ ) {
                    // Button Type
                    if( objCollection[ iLoopCollection ].GetType().Name == ( new Button() ).GetType().Name ) {
                        objButtonList.Add( objCollection[ iLoopCollection ] as Button );
                    }
                }
                // 현재 유저 정보 받음
                CUserInformation objUserInformation = objDocument.GetUserInformation();
                // 현재 유저 권한 레벨이 폼 Write 레벨보다 낮으면 버튼 막음
                CFormView objFormView = objDocument.GetMainFrame().GetFormView() as CFormView;
                if( null == objFormView ) break;
                if( ( int )objUserInformation.m_eAuthorityLevel < ( int )objDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )eFormView ].eLevelWrite ) {
                    for( int iLoopButton = 0; iLoopButton < objButtonList.Count; iLoopButton++ ) {
                        if( true == objButtonList[ iLoopButton ].Enabled ) {
                            objButtonList[ iLoopButton ].Enabled = false;
                        }
                    }
                }
                // 아니면 버튼 살림
                else {
                    for( int iLoopButton = 0; iLoopButton < objButtonList.Count; iLoopButton++ ) {
                        if( false == objButtonList[ iLoopButton ].Enabled ) {
                            objButtonList[ iLoopButton ].Enabled = true;
                        }
                    }
                }

            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 유저 권한 레벨에 따라 버튼 상태 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetChangeButtonStatus( CDocument objDocument, Control.ControlCollection collection )
        {
            do {
                // 현재 폼 내에 생성된 컨트롤 리스트를 뽑음
                Control.ControlCollection objCollection = collection;
                // 그 중에서 Button & ImageButton만 사용할 예정
                List<Control> objButtonList = new List<Control>();
                // Button or ImageButton Type인 놈만 List에 저장
                for( int iLoopCollection = 0; iLoopCollection < objCollection.Count; iLoopCollection++ ) {
                    // Button Type
                    if( objCollection[ iLoopCollection ].GetType().Name == ( new Button() ).GetType().Name ) {
                        objButtonList.Add( objCollection[ iLoopCollection ] as Button );
                    }
                }
                // 현재 유저 정보 받음
                CUserInformation objUserInformation = objDocument.GetUserInformation();
                // 현재 유저 권한 레벨이 폼 Write 레벨보다 낮으면 버튼 막음
                CFormView objFormView = objDocument.GetMainFrame().GetFormView() as CFormView;
                if( null == objFormView ) break;
                // 처음 생성 시 튕김
                if( CDefine.FormView.FORM_VIEW_FINAL == objFormView.GetCurrentForm() ) break;

                if( ( int )objUserInformation.m_eAuthorityLevel < ( int )objDocument.m_objAuthorityParameter.m_objLevelForm[ ( int )objFormView.GetCurrentForm() ].eLevelWrite ) {
                    for( int iLoopButton = 0; iLoopButton < objButtonList.Count; iLoopButton++ ) {
                        if( true == objButtonList[ iLoopButton ].Enabled ) {
                            objButtonList[ iLoopButton ].Enabled = false;
                        }
                    }
                }
                // 아니면 버튼 살림
                else {
                    for( int iLoopButton = 0; iLoopButton < objButtonList.Count; iLoopButton++ ) {
                        if( false == objButtonList[ iLoopButton ].Enabled ) {
                            objButtonList[ iLoopButton ].Enabled = true;
                        }
                    }
                }

            } while( false );
        }
    }
}