using ChartDirector;
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
    public partial class CDialogChart : Form
    {
        // 3D 그래프 그리기
        private SurfaceChart c;
        private double rotationAngle = 45;
        private double elevationAngle = 30;
        private int lastMouseX = -1;
        private int lastMouseY = -1;
        private bool isDragging = false;

        private enum enumImageType {  IMAGE_ORIGIN, IMAGE_CROP };
        // 검사포지션
        private int m_iPositionInspection;
        // 크롭인덱스
        private int m_iPositionCrop;
        // 표시할 이미지 타입
        private enumImageType m_eImageType;

        public CDialogChart()
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
                m_eImageType = enumImageType.IMAGE_CROP;
                m_iPositionInspection = pDocument.GetInspectionIndex();
                m_iPositionCrop = 0;
                // 폼 초기화
                if( false == InitializeForm() ) break;

                winChartViewer1.updateViewPort( true, false );
                initChartViewer( winChartViewer1 );
                timer1.Start();
                ReloadChart();
                winChartViewer1.MouseUsage = WinChartMouseUsage.ScrollOnDrag;
                bReturn = true;
            } while( false );

            return bReturn;
        }

        private void initChartViewer( WinChartViewer viewer )
        {

            // Enable mouse wheel zooming by setting the zoom ratio to 1.1 per wheel event
            viewer.MouseWheelZoomRatio = 2;// 1.1;
            // Initially set the mouse usage to "Pointer" mode (Drag to Scroll mode)
            //PointerPB.Checked = true;

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
        public bool ReloadChart()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            do {
                drawChart( winChartViewer1 );

                bReturn = true;
            } while( false );

            return bReturn;
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

            pFormCommon.SetButtonColor( this.BtnInspectionPosition, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnViewImageOrigin, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnViewImageCrop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnPositionCrop, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnCancel, pFormCommon.COLOR_WHITE, pFormCommon.LAMP_RED_OFF );
        }

        private void updateControls( WinChartViewer viewer )
        {
        }


        public void drawChart( WinChartViewer viewer )
        {
            var pDocument = CDocument.GetDocument;
            CInspectionResult.CResult objResult = pDocument.GetInspectionResultAlign(m_iPositionInspection );

            do {
                double[,] objData = { { 0, 0 }, { 0, 0 } };

                try {
                    if( enumImageType.IMAGE_ORIGIN == m_eImageType ) {
                        if( null != objResult.objResultCommon.obj3DDataHeightCrop2d )
                            objData = objResult.objResultCommon.obj3DDataHeightCrop2d;
                    } else {
                        if( null != objResult.objResultCommon.obj3DResultHeightData && m_iPositionCrop < objResult.objResultCommon.obj3DResultHeightData.Count )
                            objData = objResult.objResultCommon.obj3DResultHeightData[ m_iPositionCrop ];
                    }



                     double[] xData = new double[ objData.GetLength( 0 ) * objData.GetLength( 1 ) ];
                     double[] yData = new double[ objData.GetLength( 0 ) * objData.GetLength( 1 ) ];
                     double[] zData = new double[ objData.GetLength( 0 ) * objData.GetLength( 1 ) ];

                    int temp = 0;

                    //                     for( int x = 0; x < objData.GetLength( 1 ); x++ ) {
                    //                         for( int y = 0; y < objData.GetLength( 0 ); y++ ) {
                    //                             xData[ temp ] = x;
                    //                             yData[ temp ] = y;
                    //                             zData[ temp ] = objData[ y, x ];
                    //                             temp += 1;
                    //                         }
                    //                     }

                    int iX = 0, iY = 0;
                    for( int x = objData.GetLength( 1 )-1; x >= 0; x-- ) {
                        iY = 0;
                        for( int y = objData.GetLength( 0 )-1; y >= 0; y-- ) {
                        //for( int y = 0; y < objData.GetLength( 0 ); y++ ) {
                            xData[ temp ] = x;
                            yData[ temp ] = y;
                            zData[ temp ] = objData[ iY, x ];
                            if( enumImageType.IMAGE_CROP == m_eImageType ) {
                                if( -1 < objData[ iY, x ] )
                                    zData[ temp ] = objData[ iY, x ];
                                else
                                    zData[ temp ] = -1;
                            }
                            

                            temp += 1;
                            iY++;
                        }
                        iX++;
                    }

                    zData[ 0 ] = 0.5;
                    zData[ 1 ] = -0.5;

                    c = null;
                    // Create a SurfaceChart object of size 720 x 600 pixels
                    c = new SurfaceChart( 1082, 698 );

                    // Set the center of the plot region at (330, 290), 
                    //and set width x depth x height to
                    // 360 x 360 x 270 pixels
                    c.setPlotRegion( 1082 / 2, 698 / 2, 700, 400, 400 );

                    // Set the data to use to plot the chart
                    c.setData( xData, yData, zData );

                    // Spline interpolate data to a 80 x 80 grid for a smooth surface
                    c.setInterpolation( 150, 150 );

                    // Set the view angles
                    c.setViewAngle( elevationAngle, rotationAngle );

                    // Check if draw frame only during rotation
                    if( isDragging && true/*DrawFrameOnRotate.Checked*/ ) {
                        c.setShadingMode( Chart.RectangularFrame );
                    }

                    // Add a color axis (the legend) in which the left center is anchored
                    //at (650, 270). Set the length to 200 pixels and the labels on the
                    //right side.
                    c.setColorAxis( 1000, 270, Chart.Left, 200, Chart.Right );

                    // Set the x, y and z axis titles using 10 points Arial Bold font
                    c.xAxis().setTitle( "X", "Arial Bold", 15 );
                    c.yAxis().setTitle( "Y", "Arial Bold", 15 );

                    // Set axis label font
                    c.xAxis().setLabelStyle( "Arial", 10 );
                    c.yAxis().setLabelStyle( "Arial", 10 );
                    c.zAxis().setLabelStyle( "Arial", 10 );
                    c.colorAxis().setLabelStyle( "Arial", 10 );

                    // Output the chart
                    viewer.Chart = c;
                } catch( Exception ex ) {
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "Draw Chart : " + ex.ToString() );
                }
                
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

        private void winChartViewer1_ViewPortChanged( object sender, ChartDirector.WinViewPortEventArgs e )
        {
            // In addition to updating the chart, we may also need to update other controls that
            // changes based on the view port.
            updateControls( winChartViewer1 );

            // Update the chart if necessary
            if( e.NeedUpdateChart ) {
                ReloadChart();
                //AngleRedrawChart( winChartViewer1 );
            }
        }

        private void winChartViewer1_MouseDown( object sender, MouseEventArgs e )
        {
            if( 0 != ( e.Button & MouseButtons.Left ) ) {
                // Start Drag
                isDragging = true;
                lastMouseX = e.X;
                lastMouseY = e.Y;
                ( sender as WinChartViewer ).updateViewPort( true, false );
            }
        }

        private void winChartViewer1_MouseMove( object sender, MouseEventArgs e )
        {
            if( isDragging ) {
                // The chart is configured to rotate by 90 degrees when the mouse moves from 
                // left to right, which is the plot region width (360 pixels). Similarly, the
                // elevation changes by 90 degrees when the mouse moves from top to buttom,
                // which is the plot region height (270 pixels).
                rotationAngle += ( lastMouseX - e.X ) * 90.0 / 360;
                elevationAngle += ( e.Y - lastMouseY ) * 90.0 / 270;
                lastMouseX = e.X;
                lastMouseY = e.Y;
                ( sender as WinChartViewer ).updateViewPort( true, false );
            }
        }

        private void winChartViewer1_MouseUp( object sender, MouseEventArgs e )
        {
            if( 0 != ( e.Button & MouseButtons.Left ) ) {
                // End Drag
                isDragging = false;
                ( sender as WinChartViewer ).updateViewPort( true, false );
            }
        }

        private void BtnInspectionPosition_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );

                string[] strButtonList = new string[ objRecipeParameter.iCountInspectionPosition ];
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.iCountInspectionPosition; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = "INSPECTION POSITION " + ( iLoopCount + 1 ).ToString();
                }
                CDialogEnumerate objDialog = new CDialogEnumerate( objRecipeParameter.iCountInspectionPosition, strButtonList, m_iPositionInspection );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    m_iPositionInspection = objDialog.GetResult() ;
                    ReloadChart();
                }

            } while( false );
        }

        private void BtnPositionCrop_Click( object sender, EventArgs e )
        {
            var pDocument = CDocument.GetDocument;

            do {
                CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 );

                string[] strButtonList = new string[ objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].iCountSerchRegion ];
                for( int iLoopCount = 0; iLoopCount < objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].iCountSerchRegion; iLoopCount++ ) {
                    strButtonList[ iLoopCount ] = "CROP POSITION " + ( iLoopCount + 1 ).ToString();
                }
                CDialogEnumerate objDialog = new CDialogEnumerate( objRecipeParameter.objInspectionParameter[ m_iPositionInspection ].iCountSerchRegion, strButtonList, m_iPositionCrop );
                if( DialogResult.OK == objDialog.ShowDialog() ) {
                    m_iPositionCrop = objDialog.GetResult();
                    ReloadChart();
                }

            } while( false );
        }

        private void BtnViewImageOrigin_Click( object sender, EventArgs e )
        {
            m_eImageType = enumImageType.IMAGE_ORIGIN;
            ReloadChart();
        }

        private void BtnViewImageCrop_Click( object sender, EventArgs e )
        {
            m_eImageType = enumImageType.IMAGE_CROP;
            ReloadChart();

        }

        private void timer1_Tick( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            switch( m_eImageType ) {
                case enumImageType.IMAGE_ORIGIN:
                    pFormCommon.SetButtonBackColor( this.BtnViewImageOrigin, pFormCommon.COLOR_ACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnViewImageCrop, pFormCommon.COLOR_UNACTIVATE );
                    break;
                case enumImageType.IMAGE_CROP:
                    pFormCommon.SetButtonBackColor( this.BtnViewImageOrigin, pFormCommon.COLOR_UNACTIVATE );
                    pFormCommon.SetButtonBackColor( this.BtnViewImageCrop, pFormCommon.COLOR_ACTIVATE );
                    break;
                default:
                    break;
            }

            pFormCommon.SetButtonText( this.BtnInspectionPosition, "INSPECTION POSITION " + ( m_iPositionInspection + 1 ).ToString() );
            pFormCommon.SetButtonText( this.BtnPositionCrop, "CROP POSITION " + ( m_iPositionCrop + 1 ).ToString() );

        }
    }
}
