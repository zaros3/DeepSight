using ChartDirector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSight
{
    public partial class CDialogResultMonitor : Form
    {

        // 디스플레이 패널
        private Panel[] m_objPanelDisplayLeft;
        // 폼 디스플레이 화면
        private Form[] m_objFormDisplayLeft;

        // 디스플레이 패널
        private Panel[] m_objPanelDisplayRight;
        // 폼 디스플레이 화면
        private Form[] m_objFormDisplayRight;

        // 디스플레이 수량
        private int m_iDisplayIndex;
        public enum enumDisplayIndex { OIRGINAL, MEASURE_1, MEASURE_2, MEASURE_3, MEASURE_4, MEASURE_5, MEASURE_6, DISPLAY_FINAL };

        public delegate void DelegateUpdateDisplay( int iPosition );
        public DelegateUpdateDisplay m_delegateUpdateDisplayResult;

        public delegate void DelegateUpdateDisplayHistory(string strCellID );
        public DelegateUpdateDisplayHistory m_delegateUpdateDisplayResultHistory;

        public CDialogResultMonitor()
        {
            InitializeComponent();
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
                InitializeForm();
                m_delegateUpdateDisplayResult = new DelegateUpdateDisplay( UpdateDisplayResult );
                m_delegateUpdateDisplayResultHistory = new DelegateUpdateDisplayHistory( UpdateDisplayResultHistory );

                // 디스플레이 화면 생성
                {
                    m_iDisplayIndex = ( int )enumDisplayIndex.DISPLAY_FINAL;
                    m_objPanelDisplayLeft = new Panel[ m_iDisplayIndex ];
                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.OIRGINAL ] = this.panelDisplayPosition22;

                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.MEASURE_1 ] = this.panelDisplayMeasure11;
                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.MEASURE_2 ] = this.panelDisplayMeasure12;
                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.MEASURE_3 ] = this.panelDisplayMeasure13;
                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.MEASURE_4 ] = this.panelDisplayMeasure14;
                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.MEASURE_5 ] = this.panelDisplayMeasure15;
                    m_objPanelDisplayLeft[ ( int )enumDisplayIndex.MEASURE_6 ] = this.panelDisplayMeasure16;

                    // 폼 디스플레이 생성
                    m_objFormDisplayLeft = new Form[ m_iDisplayIndex ];

                    string[] strDisplayName = new string[ m_objFormDisplayLeft.Length ];
                    strDisplayName[ ( int )enumDisplayIndex.OIRGINAL ] = enumDisplayIndex.OIRGINAL.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_1 ] = enumDisplayIndex.MEASURE_1.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_2 ] = enumDisplayIndex.MEASURE_2.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_3 ] = enumDisplayIndex.MEASURE_3.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_4 ] = enumDisplayIndex.MEASURE_4.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_5 ] = enumDisplayIndex.MEASURE_5.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_6 ] = enumDisplayIndex.MEASURE_6.ToString();

                    for( int iLoopCount = 0; iLoopCount < m_objFormDisplayLeft.Length; iLoopCount++ ) {
                        // 디스플레이 화면 생성 & 초기화
                        CFormDisplay objForm = new CFormDisplay();
                        int iImageIndex = 0;
                        if( ( int )enumDisplayIndex.OIRGINAL == iLoopCount )
                            iImageIndex = iLoopCount;
                        else if( ( int )enumDisplayIndex.MEASURE_1 <= iLoopCount )
                            iImageIndex = iLoopCount - ( int )enumDisplayIndex.MEASURE_1;


                        objForm.Initialize( ( int )CDefine.enumCamera.CAMERA_1, string.Format( "{0}", strDisplayName[ iLoopCount ] ), false, iImageIndex );
                        objForm.UseMeasureInfo3D();

                        objForm.Visible = true;
                        objForm.SetTimer( true );
                        // 사이즈 조정
                        Panel objPanel = m_objPanelDisplayLeft[ iLoopCount ];
                        objForm.SetSize( objPanel.Location.X, objPanel.Location.Y, objPanel.Width, objPanel.Height, false, false );
                        // 패널에 화면 붙임
                        SetFormDockStyle( objForm, objPanel );
                        m_objFormDisplayLeft[ iLoopCount ] = objForm;
                    }
                }

                // 디스플레이 화면 생성
                {
                    m_iDisplayIndex = ( int )enumDisplayIndex.DISPLAY_FINAL;
                    m_objPanelDisplayRight = new Panel[ m_iDisplayIndex ];
                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.OIRGINAL ] = this.panelDisplayPosition24;

                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.MEASURE_1 ] = this.panelDisplayMeasure21;
                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.MEASURE_2 ] = this.panelDisplayMeasure22;
                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.MEASURE_3 ] = this.panelDisplayMeasure23;
                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.MEASURE_4 ] = this.panelDisplayMeasure24;
                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.MEASURE_5 ] = this.panelDisplayMeasure25;
                    m_objPanelDisplayRight[ ( int )enumDisplayIndex.MEASURE_6 ] = this.panelDisplayMeasure26;

                    // 폼 디스플레이 생성
                    m_objFormDisplayRight = new Form[ m_iDisplayIndex ];

                    string[] strDisplayName = new string[ m_objFormDisplayRight.Length ];
                    strDisplayName[ ( int )enumDisplayIndex.OIRGINAL ] = enumDisplayIndex.OIRGINAL.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_1 ] = enumDisplayIndex.MEASURE_1.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_2 ] = enumDisplayIndex.MEASURE_2.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_3 ] = enumDisplayIndex.MEASURE_3.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_4 ] = enumDisplayIndex.MEASURE_4.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_5 ] = enumDisplayIndex.MEASURE_5.ToString();
                    strDisplayName[ ( int )enumDisplayIndex.MEASURE_6 ] = enumDisplayIndex.MEASURE_6.ToString();

                    for( int iLoopCount = 0; iLoopCount < m_objFormDisplayRight.Length; iLoopCount++ ) {
                        // 디스플레이 화면 생성 & 초기화
                        CFormDisplay objForm = new CFormDisplay();
                        int iImageIndex = 0;
                        if( ( int )enumDisplayIndex.OIRGINAL == iLoopCount )
                            iImageIndex = iLoopCount;
                        else if( ( int )enumDisplayIndex.MEASURE_1 <= iLoopCount )
                            iImageIndex = iLoopCount - ( int )enumDisplayIndex.MEASURE_1;


                        objForm.Initialize( ( int )CDefine.enumCamera.CAMERA_1, string.Format( "{0}", strDisplayName[ iLoopCount ] ), false, iImageIndex );
                        objForm.SetDisplayIndex( iLoopCount );

                        objForm.Visible = true;
                        objForm.SetTimer( true );
                        // 사이즈 조정
                        Panel objPanel = m_objPanelDisplayRight[ iLoopCount ];
                        objForm.SetSize( objPanel.Location.X, objPanel.Location.Y, objPanel.Width, objPanel.Height, false, false );
                        // 패널에 화면 붙임
                        SetFormDockStyle( objForm, objPanel );
                        m_objFormDisplayRight[ iLoopCount ] = objForm;
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
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
                Screen[] screens = Screen.AllScreens;
                // 모니터 2개 한정으로
                if( ( int )CDefine.enumMonitor.MONITOR_FINAL == screens.Length ) {
                    Rectangle objRect = new Rectangle();
                    // 서브 모니터는 보조 모니터에 x, y값을 가져오자
                    for( int iLoopCount = 0; iLoopCount < screens.Length; iLoopCount++ ) {
                        if( false == screens[ iLoopCount ].Primary ) {
                            objRect = screens[ iLoopCount ].Bounds;
                            break;
                        }
                    }
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = new Point( objRect.X, objRect.Y );
                } else {
                    this.TopMost = false;
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = new Point( 0, 0 );
                }


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

            pFormCommon.SetButtonColor( this.BtnTitleCaseLeft, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleCaseRight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );
            pFormCommon.SetButtonColor( this.BtnTitleSeparator, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_WHITE );
        }

        public void UpdateDisplayResult( int iPosition )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;

            do {

                CFormDisplay[] objFormDisplay = new CFormDisplay[ m_objFormDisplayLeft.Length ];

                bool bInit = true;
                for( int iLoopCount = 0; iLoopCount < m_objFormDisplayLeft.Length; iLoopCount++ ) {
                    if( 21 == iPosition )
                        objFormDisplay[ iLoopCount ] = m_objFormDisplayLeft[ iLoopCount ] as CFormDisplay;
                    else if( 23 == iPosition )
                        objFormDisplay[ iLoopCount ] = m_objFormDisplayRight[ iLoopCount ] as CFormDisplay;
                    else {
                        bInit = false;
                        break;
                    }
                }

                if( false == bInit )
                    break;


                CInspectionResult.CResult objResult = pDocument.GetInspectionResultAlign( iPosition );

                if( null == objFormDisplay ) break;

                objFormDisplay[ ( int )enumDisplayIndex.OIRGINAL ].BeginInvoke( objFormDisplay[ ( int )enumDisplayIndex.OIRGINAL ].m_objDelegateUpdateDisplay3D, objResult );

                // 여기서 결과 디스플레이를 뿌리자
                for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                    //                     if( 21 == iPosition )
                    //                         obj = m_objFormDisplayLeft[ ( int )enumDisplayIndex.MEASURE_1 + iLoopCount ] as CFormDisplay;
                    //                     else if( 23 == iPosition )
                    //                         obj = m_objFormDisplayRight[ ( int )enumDisplayIndex.MEASURE_1 + iLoopCount ] as CFormDisplay;


                    //obj.BeginInvoke( obj.m_objDelegateUpdateDisplayMeasure3D, objResult );
                    objFormDisplay[ ( int )enumDisplayIndex.MEASURE_1 + iLoopCount ].BeginInvoke( objFormDisplay[ ( int )enumDisplayIndex.MEASURE_1 + iLoopCount ].m_objDelegateUpdateDisplayMeasure3D, objResult );
                }

                if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.eResult ) {
                    if( 21 == iPosition )
                        pFormCommon.SetButtonColor( this.BtnTitleCaseLeft, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_GREEN );
                    else
                        pFormCommon.SetButtonColor( this.BtnTitleCaseRight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_GREEN );
                } else {
                    if( 21 == iPosition )
                        pFormCommon.SetButtonColor( this.BtnTitleCaseLeft, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_RED );
                    else
                        pFormCommon.SetButtonColor( this.BtnTitleCaseRight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_RED );
                }

            } while( false );
        }

        public void UpdateDisplayResultHistory( string strCellID )
        {
            var pDocument = CDocument.GetDocument;
            var pFormCommon = CFormCommon.GetFormCommon;

            do {
                string strQuery = null;
                // History Align
                CManagerTable objManagerTableHistoryAlign = pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlign;

                try {
                    strQuery = string.Format( "select * from {0} ", objManagerTableHistoryAlign.HLGetTableName() );
                    strQuery += string.Format( "where {0} = '{1}'",
                                                          objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.CELL_ID ],
                                                          strCellID );


                    DataTable objDataTable = new DataTable();
                    pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload( strQuery, ref objDataTable );
                    DataRow[] objDataRow = objDataTable.Select( "", objManagerTableHistoryAlign.HLGetTableSchemaName()[ ( int )CDatabaseDefine.enumHistoryAlign.CELL_ID ] + " " + CDatabaseDefine.DEF_ASC );

                    for( int iLoopCount = 0; iLoopCount < objDataRow.Length; iLoopCount++ ) {
                        UpdateDisplay( objDataRow[ iLoopCount ] );
                    }
                        



                } catch( Exception ex ) {
                    Trace.WriteLine( ex.Message );
                }


            } while( false );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 갱신
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateDisplay( DataRow objDataRow )
        {
            do {
                try {
                    CDefine.structureReportImage objReportImage = new CDefine.structureReportImage();
                    if( "OK" == objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.RESULT ].ToString() )
                        objReportImage.bResult = true;
                    else
                        objReportImage.bResult = false;

                    CFormDisplay[] objFormDisplay = new CFormDisplay[ m_objFormDisplayLeft.Length ];
                    bool bInit = true;
                    for( int iLoopCount = 0; iLoopCount < m_objFormDisplayLeft.Length; iLoopCount++ ) {
                        if( "21" == objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ].ToString() )
                            objFormDisplay[ iLoopCount ] = m_objFormDisplayLeft[ iLoopCount ] as CFormDisplay;
                        else if( "23" == objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ].ToString() )
                            objFormDisplay[ iLoopCount ] = m_objFormDisplayRight[ iLoopCount ] as CFormDisplay;
                        else {
                            bInit = false;
                            break;
                        }
                    }

                    if( false == bInit )
                        break;

                    var pFormCommon = CFormCommon.GetFormCommon;
                    if( true == objReportImage.bResult ) {
                        if( "21" == objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ].ToString() )
                            pFormCommon.SetButtonColor( this.BtnTitleCaseLeft, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_GREEN );
                        else
                            pFormCommon.SetButtonColor( this.BtnTitleCaseRight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_GREEN );
                    } else {
                        if( "23" == objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.POSITION ].ToString() )
                            pFormCommon.SetButtonColor( this.BtnTitleCaseLeft, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_RED );
                        else
                            pFormCommon.SetButtonColor( this.BtnTitleCaseRight, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_RED );
                    }


                    objReportImage.strImagePath = objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.IMAGE_PATH ].ToString();

                    objReportImage.dPatternPositionX = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.PATTERN_POSITION_X ].ToString() );
                    objReportImage.dPatternPositionY = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.PATTERN_POSITION_Y ].ToString() );
                    ( objFormDisplay[ ( int )enumDisplayIndex.OIRGINAL ] as CFormDisplay ).UpdateDisplayHistory3D( objReportImage );

                    // 높이그랙픽 그리기..
                    CDefine.structureReportImage[] objReportImageLineResult = new CDefine.structureReportImage[ CDefine.DEF_MAX_COUNT_CROP_REGION ];
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        objReportImageLineResult[ iLoopCount ] = new CDefine.structureReportImage();
                        objReportImageLineResult[ iLoopCount ].iFindLineCount = Int32.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_LINE_FIND_COUNT ].ToString() );

                        objReportImageLineResult[ iLoopCount ].dStartX = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_X_1 + ( iLoopCount * 4 ) ].ToString() );
                        objReportImageLineResult[ iLoopCount ].dStartY = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_START_Y_1 + ( iLoopCount * 4 ) ].ToString() );
                        objReportImageLineResult[ iLoopCount ].dEndX = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_X_1 + ( iLoopCount * 4 ) ].ToString() );
                        objReportImageLineResult[ iLoopCount ].dEndY = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_END_Y_1 + ( iLoopCount * 4 ) ].ToString() );

                        objReportImageLineResult[ iLoopCount ].dLineDistance = double.Parse( objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_WIDTH_1 + ( iLoopCount * 3 ) ].ToString() );
                    }
                ( objFormDisplay[ ( int )enumDisplayIndex.OIRGINAL ] as CFormDisplay ).SetDisplayMeasureHeight( objReportImageLineResult );

                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        objReportImage.bResult = "OK" == objDataRow.ItemArray[ ( int )CDatabaseDefine.enumHistoryAlign.MEASURE_RESULT_1 + ( iLoopCount * 3 ) ].ToString() ? true : false;
                        ( objFormDisplay[ ( int )enumDisplayIndex.MEASURE_1 + iLoopCount ] as CFormDisplay ).UpdateDisplayHistory3DHeight( objReportImage );
                    }
                } catch( Exception ex ) {
                    Trace.WriteLine( ex.ToString() );
                }
            } while( false );
        }







        private void timer1_Tick( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;
        }

        private void CDialogResultMonitor_FormClosing( object sender, FormClosingEventArgs e )
        {
            var pDocument = CDocument.GetDocument;
            if( false == pDocument.m_bProgramExit )
                e.Cancel = true;
            if( Keys.Alt == CDialogLogin.ModifierKeys || CDialogLogin.ModifierKeys == Keys.F4 ) {
                e.Cancel = true;
            }
        }
    }
}
