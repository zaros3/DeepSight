using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using System.Diagnostics;

namespace HLDevice.VisionLibrary
{
    public class CVisionLibraryCogFindLineIntersect : HLDevice.Abstract.CVisionLibraryAbstract
    {
        private HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError m_objError = new HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError();
        private HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter m_objInitializeParameter = new CInitializeParameter();
        
        private int m_iIndex;
        public CogFindLineTool[] m_objFindLineTool;
        public CogIntersectLineLineTool m_objIntersectTool;
        private double[] m_dLinePositionX;
        private double[] m_dLinePositionY;
        private double[] m_dLineAngle;
        private double m_dScore;
        private double m_dPositionX;
        private double m_dPositionY;
        private double m_dAngle;

        public CogLine[] m_objResultLine;
        private CogLineSegment[] m_objResultLineSegment;
        private CogCompositeShape[] m_objLineGraphics;

        CResultData m_objResultData = new CResultData();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CVisionLibraryCogFindLineIntersect()
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

                m_objFindLineTool = new CogFindLineTool[ ( int )CResultData.enumLine.LINE_FINAL ];
                m_objResultLine = new CogLine[ ( int )CResultData.enumLine.LINE_FINAL ];
                m_objResultLineSegment = new CogLineSegment[ ( int )CResultData.enumLine.LINE_FINAL ];
                m_objLineGraphics = new CogCompositeShape[ ( int )CResultData.enumLine.LINE_FINAL ];

                m_dLinePositionX = new double[ ( int )CResultData.enumLine.LINE_FINAL ];
                m_dLinePositionY = new double[ ( int )CResultData.enumLine.LINE_FINAL ];
                m_dLineAngle = new double[ ( int )CResultData.enumLine.LINE_FINAL ];


                for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ )
                {
                    m_objFindLineTool[ iLoopCount ] = new CogFindLineTool();
                    m_objResultLine[ iLoopCount ] = new CogLine();
                    m_objResultLineSegment[ iLoopCount ] = new CogLineSegment();
                    m_objLineGraphics[ iLoopCount ] = new CogCompositeShape();
                }

                    m_objIntersectTool = new CogIntersectLineLineTool();

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


                string[] strFileName = new string[ ( int )CResultData.enumLine.LINE_FINAL ];
                strFileName[ ( int )CResultData.enumLine.LINE_VERTICAL ] = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "FindLineVertical{0}.VPP", m_iIndex );
                strFileName[ ( int )CResultData.enumLine.LINE_HORIZON ] = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "FindLineHorizon{0}.VPP", m_iIndex );

                try {
                    for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ ) {
                        m_objFindLineTool[ iLoopCount ] = CogSerializer.LoadObjectFromFile( strFileName[ iLoopCount ] ) as CogFindLineTool;
                    }
                        
                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLLoadRecipe", 5403, ex.Message );
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
                string[] strFileName = new string[ ( int )CResultData.enumLine.LINE_FINAL ];
                strFileName[ ( int )CResultData.enumLine.LINE_VERTICAL ] = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "FindLineVertical{0}.VPP", m_iIndex );
                strFileName[ ( int )CResultData.enumLine.LINE_HORIZON ] = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "FindLineHorizon{0}.VPP", m_iIndex );

                try {
                    for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ ) {
                        CogSerializer.SaveObjectToFile( m_objFindLineTool[ iLoopCount ], strFileName[ iLoopCount ], typeof( System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ), CogSerializationOptionsConstants.All );
                    }

                } catch( System.Exception ex ) {
                    Trace.WriteLine( ex.Message + " -> " + ex.StackTrace );
                    MakeErrorMessage( "HLLoadRecipe", 5404, ex.Message );
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
                    //이미지 결과 그래픽 생성
                    objResultData.objLineGraphics = new CogCompositeShape[ 2 ];

                    for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ ) {
                        m_objFindLineTool[ iLoopCount ].InputImage = new CogImage8Grey( bipmapImage );
                        m_objFindLineTool[ iLoopCount ].Run();

                        if( 0 < m_objFindLineTool[ iLoopCount ].Results.Count ) {
                            m_objResultLine[ iLoopCount ] = m_objFindLineTool[ iLoopCount ].Results.GetLine();
                            m_objResultLineSegment[ iLoopCount ] = m_objFindLineTool[ iLoopCount ].Results.GetLineSegment();
                            m_objLineGraphics[ iLoopCount ] = m_objFindLineTool[ iLoopCount ].Results[ 0 ].CreateResultGraphics( CogFindLineResultGraphicConstants.All );

                            objResultData.objLineGraphics[ iLoopCount ] = new CogCompositeShape();
                            objResultData.objLineGraphics[ iLoopCount ] = m_objLineGraphics[ iLoopCount ];
                            m_objResultLine[ iLoopCount ].GetXYRotation( out m_dLinePositionX[ iLoopCount ], out m_dLinePositionY[ iLoopCount ], out m_dLineAngle[ iLoopCount ] );
                            m_dLineAngle[ iLoopCount ] = m_dLineAngle[ iLoopCount ] * ( 180 / Math.PI );

                        } else {
                            m_dLinePositionX[ iLoopCount ] = 0; m_dLinePositionY[ iLoopCount ] = 0; m_dLineAngle[ iLoopCount ] = 0;
                            MakeErrorMessage( "HLSaveRecipe", 5405, "Line Fail" );
                            break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Trace.Write(ex.Message + "-> " + ex.StackTrace);
                    break;
                }

                m_objIntersectTool.InputImage = new CogImage8Grey( bipmapImage );
                m_objIntersectTool.LineA = m_objResultLine[ ( int )CResultData.enumLine.LINE_VERTICAL ]; m_objIntersectTool.LineB = m_objResultLine[ ( int )CResultData.enumLine.LINE_HORIZON ];
                m_objIntersectTool.Run();

                if( true == m_objIntersectTool.Intersects ) {
                    m_dAngle = Math.Abs( m_objIntersectTool.Angle * ( 180 / Math.PI ) );
                    
                    if( ( 85 < m_dAngle || 95 > m_dAngle ) || ( 265 < m_dAngle || 275 > m_dAngle ) )    {
                        m_dPositionX = m_objIntersectTool.X; m_dPositionY = m_objIntersectTool.Y;
                    } else {
                        m_dPositionX = -1; m_dPositionY = -1;
                        MakeErrorMessage( "HLSaveRecipe", 5406, "IntersectTool Angle Range Out" );
                        break;
                    }
                } else {
                    m_dPositionX = -1; m_dPositionY = -1;
                    MakeErrorMessage( "HLSaveRecipe", 5407, "IntersectTool Fail" );
                    break;
                }

                m_dScore = 1;
                bReturn = true;
            } while( false );
            for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ ) {
                objResultData.dLinePositionX[ iLoopCount ] = m_dLinePositionX[ iLoopCount ];    objResultData.dLinePositionY[ 0 ] = m_dLinePositionY[ iLoopCount ];        objResultData.dLineAngle[ 0 ]= m_dLineAngle[ iLoopCount ];    
            }
            objResultData.dPositionX[ 0 ] = m_dPositionX; objResultData.dPositionY[ 0 ] = m_dPositionY; objResultData.dScore[ 0 ] = m_dScore;
      //      objResultData.bitmapInputImage = bipmapImage;
            objResultData.bResult = bReturn; objResultData.eLibrary = CResultData.enumLibrary.FINDLINE_INTERSECT;
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
                    //이미지 결과 그래픽 생성
                    objResultData.objLineGraphics = new CogCompositeShape[ 2 ];

                    for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ ) {
                        m_objFindLineTool[ iLoopCount ].InputImage = objCogImage;
                        m_objFindLineTool[ iLoopCount ].Run();

                        if( 0 < m_objFindLineTool[ iLoopCount ].Results.Count ) {
                            m_objResultLine[ iLoopCount ] = m_objFindLineTool[ iLoopCount ].Results.GetLine();
                            m_objResultLineSegment[ iLoopCount ] = m_objFindLineTool[ iLoopCount ].Results.GetLineSegment();
                            m_objLineGraphics[ iLoopCount ] = m_objFindLineTool[ iLoopCount ].Results[ 0 ].CreateResultGraphics( CogFindLineResultGraphicConstants.All );

                            objResultData.objLineGraphics[ iLoopCount ] = new CogCompositeShape();
                            objResultData.objLineGraphics[ iLoopCount ] = m_objLineGraphics[ iLoopCount ];
                            m_objResultLine[ iLoopCount ].GetXYRotation( out m_dLinePositionX[ iLoopCount ], out m_dLinePositionY[ iLoopCount ], out m_dLineAngle[ iLoopCount ] );
                            m_dLineAngle[ iLoopCount ] = m_dLineAngle[ iLoopCount ] * ( 180 / Math.PI );

                        } else {
                            m_dLinePositionX[ iLoopCount ] = 0; m_dLinePositionY[ iLoopCount ] = 0; m_dLineAngle[ iLoopCount ] = 0;
                            MakeErrorMessage( "HLSaveRecipe", 5405, "Line Fail" );
                            break;
                        }
                    }
                } catch( System.Exception ex ) {
                    Trace.Write( ex.Message + "-> " + ex.StackTrace );
                    break;
                }

                m_objIntersectTool.InputImage = objCogImage;
                m_objIntersectTool.LineA = m_objResultLine[ ( int )CResultData.enumLine.LINE_VERTICAL ]; m_objIntersectTool.LineB = m_objResultLine[ ( int )CResultData.enumLine.LINE_HORIZON ];
                m_objIntersectTool.Run();

                if( true == m_objIntersectTool.Intersects ) {
                    m_dAngle = Math.Abs( m_objIntersectTool.Angle * ( 180 / Math.PI ) );

                    if( ( 85 < m_dAngle || 95 > m_dAngle ) || ( 265 < m_dAngle || 275 > m_dAngle ) ) {
                        m_dPositionX = m_objIntersectTool.X; m_dPositionY = m_objIntersectTool.Y;
                    } else {
                        m_dPositionX = -1; m_dPositionY = -1;
                        MakeErrorMessage( "HLSaveRecipe", 5406, "IntersectTool Angle Range Out" );
                        break;
                    }
                } else {
                    m_dPositionX = -1; m_dPositionY = -1;
                    MakeErrorMessage( "HLSaveRecipe", 5407, "IntersectTool Fail" );
                    break;
                }

                m_dScore = 1;
                bReturn = true;
            } while( false );
            for( int iLoopCount = 0; iLoopCount < ( int )CResultData.enumLine.LINE_FINAL; iLoopCount++ ) {
                objResultData.dLinePositionX[ iLoopCount ] = m_dLinePositionX[ iLoopCount ]; objResultData.dLinePositionY[ 0 ] = m_dLinePositionY[ iLoopCount ]; objResultData.dLineAngle[ 0 ] = m_dLineAngle[ iLoopCount ]; 
            }
            objResultData.dPositionX[ 0 ] = m_dPositionX; objResultData.dPositionY[ 0 ] = m_dPositionY; objResultData.dScore[ 0 ] = m_dScore;
//            objResultData.bitmapInputImage = objCogImage.ToBitmap();
            objResultData.bResult = bReturn; objResultData.eLibrary = CResultData.enumLibrary.FINDLINE_INTERSECT;
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
