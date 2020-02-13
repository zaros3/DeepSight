﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using System.Diagnostics;

namespace HLDevice.VisionLibrary
{
    public class CVisionLibraryImageProcess : HLDevice.Abstract.CVisionLibraryAbstract
    {
        private HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError m_objError = new HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError();
        private HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter m_objInitializeParameter = new CInitializeParameter();
        
        private int m_iIndex;
        public CogIPOneImageTool m_objImageProcess;

        
        CResultData m_objResultData = new CResultData();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CVisionLibraryImageProcess()
        {
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override string HLGetVersion()
        {
            return "1.0.0.1";
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLInitialize( HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter objInitializeParameter )
        {
            bool bReturn = false;

            do
            {
                m_objInitializeParameter = ( HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter )objInitializeParameter.Clone();

                m_objImageProcess = new CogIPOneImageTool();

                m_iIndex = m_objInitializeParameter.iIndex;
                m_objInitializeParameter = ( CInitializeParameter )objInitializeParameter.Clone();
                HLLoadRecipe( m_objInitializeParameter.strRecipePath, m_objInitializeParameter.strRecipeName );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void HLDeInitialize()
        {
           // m_objVisionLibrary.HLDeInitialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : Load 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLLoadRecipe( string strRecipePath, string strRecipeName )
        {
            bool bReturn = false;

            do {

                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "IPOneImage{0}.VPP", m_iIndex );

                try {
                    m_objImageProcess = CogSerializer.LoadObjectFromFile( strFileName ) as CogIPOneImageTool;
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLLoadRecipe", 5503, ex.Message );
                    break;
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : Save 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSaveRecipe( string strRecipePath, string strRecipeName )
        {
            bool bReturn = false;

            do {
                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "IPOneImage{0}.VPP", m_iIndex );

                try {
                    CogSerializer.SaveObjectToFile( m_objImageProcess, strFileName, typeof( System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ), CogSerializationOptionsConstants.Minimum );
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLSaveRecipe", 5504, ex.Message );
                    break;
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : Run 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLRun( System.Drawing.Bitmap bipmapImage, out CResultData objResultData )
        {
            objResultData = new CResultData();
            bool bReturn = false;
            do {
                try
                {
                    
                    m_objImageProcess.InputImage = new CogImage8Grey( bipmapImage );
                    m_objImageProcess.Run();
                    CogImage8Grey objResultImage = ( CogImage8Grey )m_objImageProcess.OutputImage;
                    objResultData.bitmapResultImage = objResultImage.ToBitmap();
     
                }
                catch (System.Exception ex)
                {
                    Trace.Write(ex.Message + "-> " + ex.StackTrace);
                    MakeErrorMessage( "HLRun", 5505, ex.Message );
                    break;
                }

                bReturn = true;
            } while( false );

            objResultData.bitmapInputImage = null;//bipmapImage;
            objResultData.bResult = bReturn;
            objResultData.eLibrary = CResultData.enumLibrary.IMAGEPROCESS;
            m_objResultData = ( CResultData )objResultData.Clone();
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : Run 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLRun( Cognex.VisionPro.CogImage8Grey objCogImage, out HLDevice.Abstract.CVisionLibraryAbstract.CResultData objResultData, bool bUseCalibrationImage = true )
        {
            objResultData = new CResultData();
            bool bReturn = false;
            do {
                try {

                    m_objImageProcess.InputImage = objCogImage;
                    m_objImageProcess.Run();
                    CogImage8Grey objResultImage = ( CogImage8Grey )m_objImageProcess.OutputImage;
                    objResultData.objCogImage = objResultImage;

                } catch( System.Exception ex ) {
                    Trace.Write( ex.Message + "-> " + ex.StackTrace );
                    MakeErrorMessage( "HLRun", 5505, ex.Message );
                    break;
                }

                bReturn = true;
            } while( false );

            objResultData.bitmapInputImage = null;//objCogImage.ToBitmap();
            objResultData.bResult = bReturn;
            objResultData.eLibrary = CResultData.enumLibrary.IMAGEPROCESS;
            m_objResultData = ( CResultData )objResultData.Clone();
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 결과 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLGetResult( out CResultData objResultData )
        {
            objResultData = ( CResultData )m_objResultData.Clone();

            bool bReturn = false;

            do {


                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 비전라이브러리 참조반환 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object HLGetReferenceLibrary()
        {
            return m_objImageProcess;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError MakeErrorMessage( string strFunctionName, int iReturnCode, string strMessage = "" )
        {
            m_objError.strEventTime = System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" );
            m_objError.strFunctionName = strFunctionName;
            m_objError.iReturnCode = iReturnCode;
            m_objError.strMessage = strMessage;

            // 함수 실행 결과가 성공이 아닐 경우
            if( 0 != m_objError.iReturnCode ) {

            } else {
                m_objError.iReturnCode = 0;
            }

            return m_objError;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 알람 상태 정보를 리턴한다.
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError HLGetErrorCode()
        {
            return ( HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError )m_objError.Clone();
        }
        
    }
}
