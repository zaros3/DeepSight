using System;
using System.Collections.Generic;
using OpenCvSharp;
using PMSProcessing;
using System.Runtime.InteropServices;
using System.Drawing;
using Cognex.VisionPro;
using System.Diagnostics;

namespace DeepSight
{
    // 하이브제공 소스 그대로 포워딩
    // 콜벡만 따로 처리
    public class CPMSManager : CProcessAbstract
    {
        //PMS
        // 한번에 이사가야할듯...
        static class Constants
        {
            public const bool bOffLine = true;
            public const int nThreshold = 0;
            public const int ImageNum = 4;
        }

        private PMSThreadManager m_PMS = null;// = new PMSProcessManager();
        private PMSMixedModeSharp m_PMSMix = new PMSMixedModeSharp();
        private int m_iThreadNum = 5;
        private enum EN_IMAGE_TYPE { IMAGE_NORM = 0, IMAGE_ALBEDO, IMAGE_P, IMAGE_Q, IMAGE_MAX };
        private Bitmap[] m_bmpResultPMS = new Bitmap[ 4 ];
        private byte[][] m_ppbResultPMS = new byte[ 4 ][];
        //private EN_IMAGE_TYPE m_enSelImage = EN_IMAGE_TYPE.IMAGE_ALBEDO;
        private int m_iStride = 0;

        List<float> m_vTilt = new List<float>();
        List<float> m_vSlant = new List<float>();
        float[] m_pTilt = new float[ Constants.ImageNum ];
        float[] m_pSlant = new float[ Constants.ImageNum ];

        private CDefine.enumInspectionType m_eInspectionType;
        // 여기서 부터는 NTK
        private int m_iCameraIndex;
        // 이벤트
        public delegate void ProcessDoneDelegate();
        public event ProcessDoneDelegate ProcessDoneEvent;

        Mat[] m_objMatImage = new Mat[ 4 ];

        public CPMSManager()
        {
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool Initialize()
        {
            bool bReturn = false;

            do
            {

                bReturn = true;
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Initialize( int iCameraIndex )
        {
            bool bReturn = false;
            do
            {
                m_iCameraIndex = iCameraIndex;
                m_eInspectionType = CDefine.enumInspectionType.TYPE_VIDI;
                //PMS초기화 일단 있는그대로 가져온다
                {
                    m_PMS = new PMSThreadManager();
                    m_PMS.ProcessDoneEvent += new PMSThreadManager.ProcessDoneDelegate( CallbackPMSDone );
                    m_PMS.Create( this, m_PMSMix, m_iThreadNum );

                    m_vTilt.Clear();
                    m_vTilt.Add( 270 );
                    m_vTilt.Add( 0 );
                    m_vTilt.Add( 90 );
                    m_vTilt.Add( 180 );

                    m_vSlant.Clear();
                    m_vSlant.Add( 60 );
                    m_vSlant.Add( 60 );
                    m_vSlant.Add( 60 );
                    m_vSlant.Add( 60 );
                }

                SetLightInfo();

                bReturn = true;
            } while ( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void DeInitialize()
        {
            m_bThreadExit = true;
            m_PMS.Close();

        }

        public void SetInspectionType( CDefine.enumInspectionType eInspectionType )
        {
            m_eInspectionType = eInspectionType;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 4장의 이미지를 PMS통해 합친다. 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetPMSImage()
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            // 검사 결과 구조체
            CInspectionResult.CResult objResult = new CInspectionResult.CResult();
            // 에러 로그
            string strErrorLog = string.Format( "CProcessVisionProcess60 {0} SetPMSImage ", m_iCameraIndex );
            Stopwatch objStopWatch = new Stopwatch();
            objStopWatch.Start();
            do
            {
                //pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), strErrorLog );
                objResult = pDocument.GetInspectionResultAlign( pDocument.GetInspectionIndex() );

                try {
                    List<byte[]> objImageByte;

                    CogImageToByteArr( objResult.objResultCommon.objInputGrabOriginImage, out objImageByte );

                    int[] iImageSize = new int[ objImageByte.Count ];
                    for( int i = 0; i < objImageByte.Count; i++ ) { 
                        iImageSize[ i ] = objImageByte[ i ].Length;
                    }

                    bool bImageSizeSame = true;
                    for( int i = 0; i < objImageByte.Count; i++ ) {
                        if( iImageSize[ 0 ] != iImageSize[ i ] ) {
                            bImageSizeSame = false;
                            CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0, "Set PMSImage Setting Fail" );
                            break;
                        }
                    }

                    if( false == bImageSizeSame ) break;

                    int iWidth = objResult.objResultCommon.objInputGrabOriginalImageBitmap[ 0 ].Width;
                    int iHeight = objResult.objResultCommon.objInputGrabOriginalImageBitmap[ 0 ].Height;




                    CogImage8Grey objCogImage = new CogImage8Grey( objResult.objResultCommon.objInputGrabOriginImage[ 0 ] );
                    ICogImage8PixelMemory pmr = objCogImage.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, iWidth, iHeight );
                    m_iStride = pmr.Stride;

                    iWidth = m_iStride;


                    for (int i = 0; i < objImageByte.Count; i++)
                    {
                        if (null != m_objMatImage[i])
                            m_objMatImage[i].Release();
                        m_objMatImage[i] = new Mat(iHeight, iWidth, MatType.CV_8UC1, objImageByte[i]);
                    }

                    m_PMSMix.SetPMSParameters( m_objMatImage[ 0 ].Cols, m_objMatImage[ 0 ].Rows, Constants.nThreshold );
                    m_PMSMix.SetMatImages( m_objMatImage[ 0 ].Cols, m_objMatImage[ 0 ].Rows, Constants.ImageNum, m_objMatImage );
                    m_PMS.DoStartProcess( Constants.ImageNum, m_objMatImage[ 0 ].Cols, m_objMatImage[ 0 ].Rows );
                }
                catch ( Exception ex )
                {
                    Trace.WriteLine( "CPMSManager.cs -> SetPMSImage ---" + ex.StackTrace );
                    CDocument.GetDocument.SetUpdateLog(CDefine.enumLogType.LOG_ETC, "CPMSManager.cs -> SetPMSImage ---" + ex.ToString());
                    break;
                }
                bReturn = true;


            } while ( false );
            //CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Set PMSImage Process TactTime : " + objStopWatch.ElapsedMilliseconds.ToString() );
            pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetLightInfo()
        {
            m_vTilt.Clear();
            m_vTilt.Add( 270 );
            m_vTilt.Add( 0 );
            m_vTilt.Add( 90 );
            m_vTilt.Add( 180 );

            m_vSlant.Clear();
            m_vSlant.Add( 60 );
            m_vSlant.Add( 60 );
            m_vSlant.Add( 60 );
            m_vSlant.Add( 60 );

            for ( int i = 0; i < m_vTilt.Count; i++ )
            {
                m_pTilt[ i ] = m_vTilt[ i ];
                m_pSlant[ i ] = m_vSlant[ i ];
            }

            m_PMSMix.SetLight( Constants.ImageNum, m_pTilt, m_pSlant );
        }

        public void CallbackPMSDone()
        {
            Stopwatch objStopWatch = new Stopwatch();
            objStopWatch.Start();
            try
            {
                // PMS 이미지 완료되면, Inspect를 호출해서 비디 검사를 태웁시다
                var pDocument = CDocument.GetDocument;
                CInspectionResult.CResult objResult = new CInspectionResult.CResult();
                objResult = pDocument.GetInspectionResultAlign( pDocument.GetInspectionIndex() );

                //int Width = objResult.objResultCommon.objInputGrabOriginalImageBitmap[ 0 ].Width;
                int Width = m_iStride;
                int Height = objResult.objResultCommon.objInputGrabOriginalImageBitmap[ 0 ].Height;
                int iIndex;

                if( CDefine.enumInspectionType.TYPE_VIDI == m_eInspectionType ) {
                    iIndex = Convert.ToInt32( EN_IMAGE_TYPE.IMAGE_ALBEDO );
                    m_ppbResultPMS[ iIndex ] = new byte[ Width * Height ];
                    IntPtr pAlbedo = m_PMSMix.GetAledoImagePMS();
                    Marshal.Copy( pAlbedo, m_ppbResultPMS[ iIndex ], 0, Width * Height );
                    objResult.objResultCommon.objPMSImageBitmap[ iIndex ] = BytesToBitmapGrey( m_ppbResultPMS[ iIndex ], Width, Height );
                    objResult.objResultCommon.objPMSImage[ iIndex ] = new CogImage8Grey( objResult.objResultCommon.objPMSImageBitmap[ iIndex ] );

                    iIndex = Convert.ToInt32( EN_IMAGE_TYPE.IMAGE_Q );
                    m_ppbResultPMS[ iIndex ] = new byte[ Width * Height ];
                    IntPtr pQ = m_PMSMix.GetQImagePMS();
                    Marshal.Copy( pQ, m_ppbResultPMS[ iIndex ], 0, Width * Height );
                    objResult.objResultCommon.objPMSImageBitmap[ iIndex ] = BytesToBitmapGrey( m_ppbResultPMS[ iIndex ], Width, Height );
                    objResult.objResultCommon.objPMSImage[ iIndex ] = new CogImage8Grey( objResult.objResultCommon.objPMSImageBitmap[ iIndex ] );

                    iIndex = Convert.ToInt32( EN_IMAGE_TYPE.IMAGE_P );
                    m_ppbResultPMS[ iIndex ] = new byte[ Width * Height ];
                    IntPtr pP = m_PMSMix.GetPImagePMS();
                    Marshal.Copy( pP, m_ppbResultPMS[ iIndex ], 0, Width * Height );
                    objResult.objResultCommon.objPMSImageBitmap[ iIndex ] = BytesToBitmapGrey( m_ppbResultPMS[ iIndex ], Width, Height );
                    objResult.objResultCommon.objPMSImage[ iIndex ] = new CogImage8Grey( objResult.objResultCommon.objPMSImageBitmap[ iIndex ] );

                    
                } else {
                    iIndex = Convert.ToInt32( EN_IMAGE_TYPE.IMAGE_ALBEDO );
                    m_ppbResultPMS[ iIndex ] = new byte[ Width * Height ];
                    IntPtr pAlbedo = m_PMSMix.GetAledoImagePMS();
                    Marshal.Copy( pAlbedo, m_ppbResultPMS[ iIndex ], 0, Width * Height );
                    objResult.objResultCommon.objPMSImageBitmapMeasure[ iIndex ] = BytesToBitmapGrey( m_ppbResultPMS[ iIndex ], Width, Height );
                    objResult.objResultCommon.objPMSImageMeasure[ iIndex ] = new CogImage8Grey( objResult.objResultCommon.objPMSImageBitmapMeasure[ iIndex ] );

                    iIndex = Convert.ToInt32( EN_IMAGE_TYPE.IMAGE_Q );
                    m_ppbResultPMS[ iIndex ] = new byte[ Width * Height ];
                    IntPtr pQ = m_PMSMix.GetQImagePMS();
                    Marshal.Copy( pQ, m_ppbResultPMS[ iIndex ], 0, Width * Height );
                    objResult.objResultCommon.objPMSImageBitmapMeasure[ iIndex ] = BytesToBitmapGrey( m_ppbResultPMS[ iIndex ], Width, Height );
                    objResult.objResultCommon.objPMSImageMeasure[ iIndex ] = new CogImage8Grey( objResult.objResultCommon.objPMSImageBitmapMeasure[ iIndex ] );

                    iIndex = Convert.ToInt32( EN_IMAGE_TYPE.IMAGE_P );
                    m_ppbResultPMS[ iIndex ] = new byte[ Width * Height ];
                    IntPtr pP = m_PMSMix.GetPImagePMS();
                    Marshal.Copy( pP, m_ppbResultPMS[ iIndex ], 0, Width * Height );
                    objResult.objResultCommon.objPMSImageBitmapMeasure[ iIndex ] = BytesToBitmapGrey( m_ppbResultPMS[ iIndex ], Width, Height );
                    objResult.objResultCommon.objPMSImageMeasure[ iIndex ] = new CogImage8Grey( objResult.objResultCommon.objPMSImageBitmapMeasure[ iIndex ] );
                }

                pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );
                ProcessDoneEvent();
            }
            catch { }

            //CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "PMS CallBack Image Process TactTime : " + objStopWatch.ElapsedMilliseconds.ToString() );
        }
    }
}
