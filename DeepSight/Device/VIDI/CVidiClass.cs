using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using ViDi2;

namespace DeepSight
{
    public class CVidiClass
    {
        public enum EN_ToolType { EN_RED, EN_GREEN, EN_BLUE };
        //bool m_bInit = false;
        string m_strWorkspaceFilePath;
        string m_strWorkspaceName;
        string m_strStreamName;

        //static bool useOptimizedGpuMemory = true;
        List<int> GPUList = new List<int>();
        ViDi2.Runtime.IControl m_Control = null;
        ViDi2.Runtime.IWorkspace m_Workspace;
        ViDi2.Runtime.IStream m_Stream;
        ISample m_Sample = null;

        public void Initialize(string strWorkspaceFilePath, string strWorkspaceName, string strStreamName)
        {
            m_strWorkspaceFilePath = strWorkspaceFilePath;
            m_strWorkspaceName = strWorkspaceName;
            m_strStreamName = strStreamName;
            VidiInit();
        }

        /// <summary>
        /// Gets or sets the main control providing access to the library
        /// </summary>
        public ViDi2.Runtime.IControl Control
        {
            get { return m_Control; }
            set
            {
                m_Control = value;
                //RaisePropertyChanged(nameof(m_Control));
            }
        }

        /// <summary>
        /// Gets or sets the current workspace
        /// </summary>
        public ViDi2.Runtime.IWorkspace Workspace
        {
            get { return m_Workspace; }
            set
            {
                m_Workspace = value;
                Stream = m_Workspace.Streams.First();
               // RaisePropertyChanged(nameof(m_Workspace));
            }
        }

        /// <summary>
        /// Gets or sets the current stream
        /// </summary>
        public ViDi2.Runtime.IStream Stream
        {
            get { return m_Stream; }
            set
            {
                m_Stream = value;
                //sampleViewer.Sample = null;
              //  RaisePropertyChanged(nameof(m_Stream));
            }
        }

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetStreamName(string strName)
        {
            try
            {
                m_strStreamName = strName;
                m_Stream = m_Workspace.Streams[m_strStreamName];
            }
            catch { return false; }
            return true;
        }

        private void VidiInit()
        {
            try
            {
                if (m_Control != null) 
                    m_Control.Dispose();

                // holds the main control
                m_Control = new ViDi2.Runtime.Local.Control();


                // opens a runtime workspace from file
                //m_Workspace = m_Control.Workspaces.Add(m_strWorkspaceName, m_strWorkspaceFilePath);
                if( true == File.Exists( m_strWorkspaceFilePath ) ) {
                    using( var fs = new System.IO.FileStream( m_strWorkspaceFilePath, System.IO.FileMode.Open ) ) {
                        m_Workspace = m_Control.Workspaces.Add( System.IO.Path.GetFileNameWithoutExtension( m_strWorkspaceFilePath ), fs );
                    }

                    SetStreamName( m_strStreamName );
                }
            }
            catch(ViDi2.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine( ex.ToString() );
                //MessageBox.Show("Failed VIDI Initialize!");
            }
        }

        public void VidiDispose()
        {
            // dispose the control to free all resources
            if( null != m_Control )
                m_Control.Dispose();
        }

        public bool VidiFileLoad( string strWorkspaceFilePath )
        {
            bool bReturn = false;
            do {
                m_strWorkspaceFilePath = strWorkspaceFilePath;
                VidiInit();
                bReturn = true;
            } while( false );

            return bReturn;
        }

        public bool Process(Bitmap bmpImg, string strToolName, CDefine.enumVidiType eVidiType)
        {
            bool bReturn = false;

            do
            {
                try
                {
                    IImage img = new FormsImage( bmpImg );
                    m_Sample = Stream.Tools[ strToolName ].Process( img );
                }
                catch ( System.Exception )
                {
                    break;
                }
                bReturn = true;
            } while ( false );

            return bReturn;
            
        }

        //Red 툴 결과 처리
        public bool GetRedToolResult(string strToolName, out double dScore, out Bitmap bmpOverlay, out Bitmap bmpViewImage)
        {
            dScore = 99999.0;
            bmpOverlay = new Bitmap( 10, 10 );
            bmpViewImage = new Bitmap( 10, 10 );
            try {

                // retrieve the marking for the red tool called 'analyze'
                //'분석'이라는 빨간 색 도구에 대한 표시 검색
                IRedMarking redMarking = m_Sample.Markings[ strToolName ] as IRedMarking;

                // gets the overlay image
                //오버레이 이미지를 가져옵니다.
                //IImage m_OverlayImage = redMarking.OverlayImage(0);
                //IImage m_ViewImage = redMarking.ViewImage(0);
                // -scale
                // cale of the overlay
                // -hide_heat_map
                // hide the heat map from the overlay of a red tool
                // - hide_mask
                // hide the mask from the overlay
                // -hide_regions
                //  hide the contour region of the defect found in a red tool
                // - hide_red_score
                // hide the score of the bottom left corner of a red tool
                // -hide_secondary_classes
                // hide the second class found of a green tool 
                // - hide_matches
                // hide the model matches of a blue tool 
                // - fit_size size of the image usage example: fit_size 640x480, hide_mask,hide_region

                bmpOverlay = redMarking.OverlayImage( 0 ).Bitmap;
                //bmpOverlay = redMarking.OverlayImage(0, "hide_heat_map" ).Bitmap;
                bmpViewImage = redMarking.ViewImage( 0 ).Bitmap;

                if( redMarking != null ) {
                    int countPositive = 0;
                    int countIntermediate = 0;

                    foreach( IRedView view in redMarking.Views ) {
                        if( view.Score > view.Threshold.Upper )
                            countPositive++;
                        else if( view.Score > view.Threshold.Lower )
                            countIntermediate++;
                        if( dScore > view.Score )
                            dScore = view.Score;
                    }
                    bmpOverlay = redMarking.OverlayImage( 0, "hide_heat_map" ).Bitmap;
                } else return false;
            } catch( Exception ) {
                return false;
            }
            return true;
        }

        //Red 툴 결과 영상 얻기
        public void GetRedToolResultImage(string strToolName, out Bitmap bmpOverlay, out Bitmap bmpViewImage)
        {
            // retrieve the marking for the red tool called 'analyze'
            //'분석'이라는 빨간 색 도구에 대한 표시 검색
            IRedMarking redMarking = m_Sample.Markings[strToolName] as IRedMarking;

            // gets the overlay image
            //오버레이 이미지를 가져옵니다.
            bmpOverlay = redMarking.OverlayImage(0).Bitmap;
            bmpViewImage = redMarking.ViewImage(0).Bitmap;
        }

        //Blue 툴 결과 처리
        public bool GetBlueToolResult(string strToolName, out double dMatchScore, out double dFeatureScore)
        {
            dMatchScore = 99999.0;
            dFeatureScore = 99999.0;

            // access the marking for the blue tool called 'localize'
            //'현지화'라는 파란 색 도구의 표시에 액세스
            IBlueMarking blueMarking = m_Sample.Markings[strToolName] as IBlueMarking;

            if (blueMarking != null)
            {
                foreach (IBlueView view in blueMarking.Views)
                {
                    foreach (IMatch match in view.Matches)
                    {
                        if (dMatchScore > match.Score)
                            dMatchScore = match.Score;
                    }
                    foreach (IFeature feature in view.Features)
                    {
                        if (dFeatureScore > feature.Score)
                            dFeatureScore = feature.Score;
                    }
                }
            }
            else return false;

            return true;
        }

        //Green 툴 결과 처리
        public bool GetGreenToolResult(string strToolName, out string strTagName)
        {
            strTagName = "";

            // access the marking for the green tool called 'classify'
            //'분류'라고 하는 녹색 도구의 표시에 액세스
            IGreenMarking greenMarking = m_Sample.Markings[strToolName] as IGreenMarking;

            if (greenMarking != null)
            {
                foreach (IGreenView view in greenMarking.Views)
                {
                    strTagName = view.BestTag.Name;
                }
            }
            else return false;

            return true;
        }

    }
}
