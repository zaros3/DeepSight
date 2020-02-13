using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using System.Diagnostics;

namespace HLDevice.VisionLibrary
{
    public class CVisionLibraryCogFindLine : HLDevice.Abstract.CVisionLibraryAbstract
    {
        private HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError m_objError = new HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError();
        private HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter m_objInitializeParameter = new CInitializeParameter();
        
        private int m_iIndex;
        public CogFindLineTool m_objFindLineTool;

        private double m_dLinePositionX;
        private double m_dLinePositionY;
        private double m_dAngle;
        private double m_dScore;

        private CogLine m_objResultLine;
        private CogLineSegment m_objResultLineSegment;
        private CogCompositeShape m_objGraphics;

        CResultData m_objResultData = new CResultData();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CVisionLibraryCogFindLine()
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
                m_objResultLine = new CogLine();
                m_objResultLineSegment = new CogLineSegment();
                m_objGraphics = new CogCompositeShape();

                m_objFindLineTool = new CogFindLineTool();

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

                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "FindLine{0}.VPP", m_iIndex );

                try {
                        m_objFindLineTool = CogSerializer.LoadObjectFromFile(strFileName) as CogFindLineTool;
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLLoadRecipe", 5303, ex.Message );
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
                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "FindLine{0}.VPP", m_iIndex );

                try {
                    CogSerializer.SaveObjectToFile(m_objFindLineTool, strFileName, typeof(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter), CogSerializationOptionsConstants.All);
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLSaveRecipe", 5304, ex.Message );
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
                    m_dScore = 0;
                    m_objFindLineTool.InputImage = new CogImage8Grey( bipmapImage );
                    m_objFindLineTool.Run();

                    if( 0 < m_objFindLineTool.Results.Count ) {
                        m_objResultLine = m_objFindLineTool.Results.GetLine();
                        m_objResultLineSegment = m_objFindLineTool.Results.GetLineSegment();
                        m_objGraphics = m_objFindLineTool.Results[ 0 ].CreateResultGraphics( CogFindLineResultGraphicConstants.All );

                        m_objResultLine.GetXYRotation( out m_dLinePositionX, out m_dLinePositionY, out m_dAngle );
                        m_dAngle = m_dAngle * ( 180 / Math.PI ); 

                    } else {
                        m_dLinePositionX = 0; m_dLinePositionY = 0; m_dAngle = 0; 
                        MakeErrorMessage( "HLSaveRecipe", 5305, "Line Fail" );
                        break;
                    }

                }
                catch (System.Exception ex)
                {
                    Trace.Write(ex.Message + "-> " + ex.StackTrace);
                    break;
                }

                m_dScore = 1;
                bReturn = true;
            } while( false );

         //   objResultData.objGraphics = new CogCompositeShape();
            objResultData.dLinePositionX[ 0 ] = m_dLinePositionX;    objResultData.dLinePositionY[ 0 ] = m_dLinePositionY;        objResultData.dLineAngle[ 0 ]= m_dAngle;    objResultData.dScore[ 0 ] = m_dScore;
          //  objResultData.objGraphics = m_objGraphics;               objResultData.bitmapInputImage = bipmapImage;
            objResultData.bResult = bReturn;
            m_objResultData = ( CResultData )objResultData.Clone();
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : Run 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLRun( Cognex.VisionPro.CogImage8Grey objCogImage, out HLDevice.Abstract.CVisionLibraryAbstract.CResultData objResultData, bool bUseCalibrationImage = false )
        {
            objResultData = new CResultData();
            bool bReturn = false;
            do {
                try {
                    m_dScore = 0;
                    m_objFindLineTool.InputImage = objCogImage;
                    m_objFindLineTool.Run();

                    if( 0 < m_objFindLineTool.Results.Count ) {
                        m_objResultLine = m_objFindLineTool.Results.GetLine();
                        m_objResultLineSegment = m_objFindLineTool.Results.GetLineSegment();
                        m_objGraphics = m_objFindLineTool.Results[ 0 ].CreateResultGraphics( CogFindLineResultGraphicConstants.All );

                        m_objResultLine.GetXYRotation( out m_dLinePositionX, out m_dLinePositionY, out m_dAngle );
                        m_dAngle = m_dAngle * ( 180 / Math.PI );

                    } else {
                        m_dLinePositionX = 0; m_dLinePositionY = 0; m_dAngle = 0;
                        MakeErrorMessage( "HLSaveRecipe", 5305, "Line Fail" );
                        break;
                    }

                } catch( System.Exception ex ) {
                    Trace.Write( ex.Message + "-> " + ex.StackTrace );
                    break;
                }

                m_dScore = 1;
                bReturn = true;
            } while( false );

      //      objResultData.objGraphics = new CogCompositeShape();
            objResultData.dLinePositionX[ 0 ] = m_dLinePositionX; objResultData.dLinePositionY[ 0 ] = m_dLinePositionY; objResultData.dLineAngle[ 0 ] = m_dAngle; objResultData.dScore[ 0 ] = m_dScore;
          //  objResultData.objGraphics = m_objGraphics; objResultData.bitmapInputImage = objCogImage.ToBitmap();
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
            return m_objFindLineTool;
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
