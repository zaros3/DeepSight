using System;
using System.Collections.Generic;
using OpenCvSharp;
using PMSProcessing;
using System.Runtime.InteropServices;
using System.Drawing;
using Cognex.VisionPro;
using System.Diagnostics;
using SkinLeadWelding;

namespace DeepSight {
    // 하이브제공 소스 그대로 포워딩
    // 콜벡만 따로 처리
    public class CMeasureManager : CProcessAbstract {
        //PMS
        // 한번에 이사가야할듯...
        public struct structureMeasureParameter {
            //버스바
            public int nLRLowThresh;
            public int nLRContinousLen;
            public int nLRHighThresh;
            public int nTBThresh;
            public int nTBContinousLen;
            //센서
            public int nSensorStdWid;
            public int nSensorStdHgt;
            public int nThresh;

            public CogImage8Grey objImage;
            public Bitmap objImageBitmap;

            public CogImage8Grey objImageSecond;
            public Bitmap objImageBitmapSecond;
        }

        public struct structureMeasureResult {
            public int iWidth;
            public int iHeight;
            public int iStartX;
            public int iStartY;
            public int iEndX;
            public int iEndY;
        }
        // 여기서 부터는 NTK
        private int m_iCameraIndex;

        private BusbarDll m_objMeasure;

        public CMeasureManager()
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

            do {

                bReturn = true;
            } while( false );

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
            do {
                m_iCameraIndex = iCameraIndex;

                m_objMeasure = new BusbarDll();


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
        public override void DeInitialize()
        {
            m_bThreadExit = true;
            //m_objMeasure.BusbarReleaseMemory();

        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 4장의 이미지를 PMS통해 합친다. 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetInspectionBusbar( structureMeasureParameter objParameter )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            // 에러 로그
            string strErrorLog = string.Format( "CProcessVisionProcess60 {0} SetInspectionBusbar ", m_iCameraIndex );
            Stopwatch objStopWatch = new Stopwatch();
            objStopWatch.Start();
            do {
               // pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), strErrorLog );

                try {

                    List<byte[]> objImageByte;
                    List<byte[]> objImageByteSecond;

                    CogImageToByteArr( objParameter.objImage, out objImageByte );
                    CogImageToByteArr( objParameter.objImage, out objImageByteSecond );

                    int iWidth = objParameter.objImageBitmap.Width;
                    int iHeight = objParameter.objImageBitmap.Height;

                    CogImage8Grey objCogImage = new CogImage8Grey( objParameter.objImageBitmap );
                    ICogImage8PixelMemory pmr = objCogImage.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, iWidth, iHeight );
                    int iStride = pmr.Stride;

                    iWidth = iStride;

                    Mat objMatImage = new Mat( iHeight, iWidth, MatType.CV_8UC1, objImageByte[ 0 ] );

                    Mat objMatImageSecond = new Mat( iHeight, iWidth, MatType.CV_8UC1, objImageByteSecond[ 0 ] );

                    //m_objMeasure.SetBusbarParameters( objParameter.iThresh, objParameter.iMaskLen, objParameter.iMeasureInterv, objParameter.iCaliperLen, objParameter.iSkipLenFromCenter );

                    //DoMeasureWelding( int nWidth, int nHeight, byte* pPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, byte* pPMS3, int nTBThresh, int nTBContinousLen );
                    unsafe {
                        m_objMeasure.DoBusbarMeasureWelding( iWidth, iHeight, objMatImage.DataPointer, objParameter.nLRLowThresh, objParameter.nLRContinousLen, objParameter.nLRHighThresh, objMatImageSecond.DataPointer, objParameter.nTBThresh, objParameter.nTBContinousLen );
                    }

                } catch( Exception ex ) {
                    Trace.WriteLine( "CMeasureManager.cs -> SetPMSImage ---" + ex.StackTrace );
                    break;
                }
                bReturn = true;


            } while( false );
            //CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Measure SetInspection TactTime : " + objStopWatch.ElapsedMilliseconds.ToString() );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 4장의 이미지를 PMS통해 합친다. 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public structureMeasureResult GetResultBusbar()
        {
            var pDocument = CDocument.GetDocument;
            structureMeasureResult objResult = new structureMeasureResult();

            // 에러 로그
            string strErrorLog = string.Format( "CMeasureManager {0} GetResult ", m_iCameraIndex );
            Stopwatch objStopWatch = new Stopwatch();
            objStopWatch.Start();
            do {
                objResult.iStartX = m_objMeasure.GetBusbarMeasuredLeft();
                objResult.iStartY = m_objMeasure.GetBusbarMeasuredTop();
                objResult.iEndX = m_objMeasure.GetBusbarMeasuredRight();
                objResult.iEndY = m_objMeasure.GetBusbarMeasuredBottom();
                objResult.iWidth = m_objMeasure.GetBusbarMeasuredWidth();
                objResult.iHeight = m_objMeasure.GetBusbarMeasuredHeight();
            } while( false );
            //CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Measure GetResult TactTime : " + objStopWatch.ElapsedMilliseconds.ToString() );

            return objResult;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 4장의 이미지를 PMS통해 합친다. 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetInspectionSensor( structureMeasureParameter objParameter )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            // 에러 로그
            string strErrorLog = string.Format( "CMeasureManager {0} SetInspectionSensor ", m_iCameraIndex );
            Stopwatch objStopWatch = new Stopwatch();
            objStopWatch.Start();
            do {
               // pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + m_iCameraIndex ), strErrorLog );

                try {

                    List<byte[]> objImageByte;
                    List<byte[]> objImageByteSecond;

                    CogImageToByteArr( objParameter.objImage, out objImageByte );
                    CogImageToByteArr( objParameter.objImage, out objImageByteSecond );

                    int iWidth = objParameter.objImageBitmap.Width;
                    int iHeight = objParameter.objImageBitmap.Height;

                    CogImage8Grey objCogImage = new CogImage8Grey( objParameter.objImageBitmap );
                    ICogImage8PixelMemory pmr = objCogImage.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, iWidth, iHeight );
                    int iStride = pmr.Stride;

                    iWidth = iStride;

                    Mat objMatImage = new Mat( iHeight, iWidth, MatType.CV_8UC1, objImageByte[ 0 ] );

                    Mat objMatImageSecond = new Mat( iHeight, iWidth, MatType.CV_8UC1, objImageByteSecond[ 0 ] );

                    unsafe {
                        m_objMeasure.DoSensorMeasure( iWidth, iHeight, objMatImage.DataPointer, objParameter.nSensorStdWid, objParameter.nSensorStdHgt, objParameter.nThresh );
                    }

                } catch( Exception ex ) {
                    Trace.WriteLine( "CMeasureManager.cs -> SetPMSImage ---" + ex.StackTrace );
                    break;
                }
                bReturn = true;


            } while( false );
            //CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Measure SetInspection TactTime : " + objStopWatch.ElapsedMilliseconds.ToString() );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 검사
        //설명 : 4장의 이미지를 PMS통해 합친다. 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public structureMeasureResult GetResultSensor()
        {
            var pDocument = CDocument.GetDocument;
            structureMeasureResult objResult = new structureMeasureResult();

            // 에러 로그
            string strErrorLog = string.Format( "CMeasureManager {0} GetResult ", m_iCameraIndex );
            Stopwatch objStopWatch = new Stopwatch();
            objStopWatch.Start();
            do {
                objResult.iStartX = m_objMeasure.GetSensorMeasuredLeft();
                objResult.iStartY = m_objMeasure.GetSensorMeasuredTop();
                objResult.iEndX = m_objMeasure.GetSensorMeasuredRight();
                objResult.iEndY = m_objMeasure.GetSensorMeasuredBottom();
                objResult.iWidth = m_objMeasure.GetSensorMeasuredWidth();
                objResult.iHeight = m_objMeasure.GetSensorMeasuredHeight();
            } while( false );
            //CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Measure GetResult TactTime : " + objStopWatch.ElapsedMilliseconds.ToString() );

            return objResult;
        }

    }
}
