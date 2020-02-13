using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using System.Diagnostics;

namespace HLDevice.VisionLibrary
{
    public class CVisionLibraryCogFixture : HLDevice.Abstract.CVisionLibraryAbstract
    {
        private HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError m_objError = new HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError();
        private HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter m_objInitializeParameter = new CInitializeParameter();
        
        private int m_iIndex;
        private CogFixtureTool m_objFixtureTool;

        CResultData m_objResultData = new CResultData();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CVisionLibraryCogFixture()
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
                m_iIndex = m_objInitializeParameter.iIndex;
                m_objInitializeParameter = ( CInitializeParameter )objInitializeParameter.Clone();
                m_objFixtureTool = new CogFixtureTool();
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

                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "Fixture{0}.VPP", m_iIndex );
                try
                {
                    m_objFixtureTool = CogSerializer.LoadObjectFromFile( strFileName ) as CogFixtureTool;
                }
                catch ( System.Exception ex )
                {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLLoadRecipe", 5203, ex.Message );
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
                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "Fixture{0}.VPP", m_iIndex );
                try
                {
                    CogSerializer.SaveObjectToFile( m_objFixtureTool, strFileName, typeof( System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ), CogSerializationOptionsConstants.All );
                }
                catch ( System.Exception ex )
                {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLSaveRecipe", 5204, ex.Message );
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
                  
                }
                catch (System.Exception ex)
                {
                    Trace.Write(ex.Message + "-> " + ex.StackTrace);
                    break;
                }

                bReturn = true;
            } while( false );


            objResultData.bResult = bReturn;
            m_objResultData = ( CResultData )objResultData.Clone();
            return bReturn;
        }

        public override bool HLRun( Cognex.VisionPro.CogImage8Grey objCogImage, out HLDevice.Abstract.CVisionLibraryAbstract.CResultData objResultData, bool bUseCalibrationImage = false )
        {
            objResultData = new CResultData();
            bool bReturn = false;
            do {
                try {
                    m_objFixtureTool.InputImage = objCogImage;
                    // 여기에 패턴에서 찾은 좌표를 넣어야하는데..

                    m_objFixtureTool.Run();
                    CogImage8Grey objResultImage = ( CogImage8Grey )m_objFixtureTool.OutputImage;
                    if ( null == objResultImage )
                    {
                        objResultData.objCogImage = objCogImage;
                        break;
                    }
                    else
                    {
                        objResultData.objCogImage = objResultImage;
                    }
                } catch( System.Exception ex ) {
                    Trace.Write( ex.Message + "-> " + ex.StackTrace );
                    break;
                }

                bReturn = true;
            } while( false );


            objResultData.bResult = bReturn;
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
            return this;
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
