using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.CalibFix;
using System.Diagnostics;

namespace HLDevice.VisionLibrary
{
    public class CVisionLibraryCogPMAlign : HLDevice.Abstract.CVisionLibraryAbstract
    {
        private HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError m_objError = new HLDevice.Abstract.CVisionLibraryAbstract.CVisionLibraryError();
        private HLDevice.Abstract.CVisionLibraryAbstract.CInitializeParameter m_objInitializeParameter = new CInitializeParameter();
        
        private int m_iIndex;
        public CogPMAlignTool m_objPMAlignTool;
        private CogFixtureTool m_objFixtureTool;
        private double[] m_dPositionX = new double[ ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL ];
        private double[] m_dPositionY = new double[ ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL ];
        private double[] m_dScore = new double[ ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL ];
        private double[] m_dAngle = new double[ ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL ];
        
        private ICogRecord[] m_objGraphics;
//         private CogRectangleAffine m_objROI;
//         private CogRectangleAffine m_objTrainRegion;
//         private CogImage8Grey m_objTrainedImage;

        CResultData m_objResultData = new CResultData();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CVisionLibraryCogPMAlign()
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
                m_objGraphics = new ICogRecord[ ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL ];
//                 for( int iLoopCount = 0; iLoopCount < ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL; iLoopCount++ )
//                     m_objGraphics[ iLoopCount ] = new ICogRecord();

                m_objPMAlignTool = new CogPMAlignTool();
                m_objFixtureTool = new CogFixtureTool();

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

                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "PMAlign{0}.VPP", m_iIndex );

                try {
                    m_objPMAlignTool = CogSerializer.LoadObjectFromFile( strFileName ) as CogPMAlignTool;
                    // 처음이 느려서 한번찍어줌
                    //m_objPMAlignTool.Run();
                    //m_objFixtureTool.Run();
                    //                     m_objROI = ( CogRectangleAffine )m_objPMAlignTool.SearchRegion;
                    //                     m_objTrainRegion = ( CogRectangleAffine )m_objPMAlignTool.Pattern.TrainRegion;
                    //                     m_objTrainedImage = ( CogImage8Grey )m_objPMAlignTool.Pattern.TrainImage;
                    // 
                    //                     m_objROI.Color = CogColorConstants.Blue;
                    //                     m_objTrainRegion.Color = CogColorConstants.Red; m_objTrainRegion.LineStyle = CogGraphicLineStyleConstants.Dot;
                } catch( System.Exception ex ) {
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
                string strFileName = strRecipePath + "\\" + strRecipeName + "\\" + string.Format( "PMAlign{0}.VPP", m_iIndex );

                try {
                    CogSerializer.SaveObjectToFile( m_objPMAlignTool, strFileName, typeof( System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ), CogSerializationOptionsConstants.All );
                } catch( System.Exception ex ) {
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
                    if (false == m_objPMAlignTool.Pattern.Trained) break;

                    m_objPMAlignTool.InputImage = new CogImage8Grey( bipmapImage );
                    m_objPMAlignTool.Run();

                    if ( 0 < m_objPMAlignTool.Results.Count )//&& iScoreLimit < m_objPMAlignTool.Results[0].Score * 100 ) // 리미트 설정은 추후
                    {
                        for ( int iLoopCount = 0; iLoopCount < m_objPMAlignTool.Results.Count; iLoopCount++ )
                        {
                            if ( ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL <= iLoopCount ) break;
                            m_dPositionX[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].GetPose().TranslationX; m_dPositionY[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].GetPose().TranslationY; m_dAngle[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].GetPose().Rotation * ( 180 / Math.PI ); m_dScore[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].Score;
                            //m_objGraphics[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].CreateResultGraphics( CogPMAlignResultGraphicConstants.All );
                        }
                    }
                    else
                    {
                        for ( int iLoopCount = 0; iLoopCount < ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL; iLoopCount++ )
                        {
                            //   m_objGraphics = null;
                            m_dPositionX[ iLoopCount ] = m_dPositionY[ iLoopCount ] = -1; m_dAngle[ iLoopCount ] = 0; m_dScore[ iLoopCount ] = 0;
                        }

                        MakeErrorMessage( "HLRun", 5205, "Pattern Fail" );
                        break;
                    }
                }
                catch (System.Exception ex)
                {
                    Trace.Write(ex.Message + "-> " + ex.StackTrace);
                    break;
                }

                bReturn = true;
            } while( false );


            for( int iLoopCount = 0; iLoopCount < m_objPMAlignTool.Results.Count; iLoopCount++ ) {
                if( false == bReturn ) {
                } else {
                    if( ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL <= iLoopCount ) break;
                    objResultData.objGraphics[ iLoopCount ] = new CogCompositeShape();
                    objResultData.objGraphics[ iLoopCount ] = m_objGraphics[ iLoopCount ];
                    objResultData.dPositionX[ iLoopCount ] = m_dPositionX[ iLoopCount ]; objResultData.dPositionY[ iLoopCount ] = m_dPositionY[ iLoopCount ]; objResultData.dPositionAngle[ iLoopCount ] = m_dAngle[ iLoopCount ]; objResultData.dScore[ iLoopCount ] = m_dScore[ iLoopCount ];
                }
            }
            m_objGraphics[ 0 ] = m_objPMAlignTool.CreateLastRunRecord();
            objResultData.bitmapInputImage = null;//bipmapImage;
            objResultData.bResult = bReturn;              objResultData.eLibrary = CResultData.enumLibrary.PMALIGN;
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
                    if( null == objCogImage ) break;
                    if( false == m_objPMAlignTool.Pattern.Trained ) break;
                    objCogImage.SelectedSpaceName = "@";
                    m_objPMAlignTool.InputImage = objCogImage;

                    if( false == bUseCalibrationImage ) {
                        m_objPMAlignTool.InputImage.CoordinateSpaceTree.RootName = @"#@Checkerboard Calibration";
                        m_objPMAlignTool.InputImage.SelectedSpaceName = @"#@Checkerboard Calibration";
                    }
                    

                    m_objPMAlignTool.Run();

                    if( 0 < m_objPMAlignTool.Results.Count )//&& iScoreLimit < m_objPMAlignTool.Results[0].Score * 100 ) // 리미트 설정은 추후
                    {
                        for( int iLoopCount = 0; iLoopCount < m_objPMAlignTool.Results.Count; iLoopCount++ ) {
                            if( ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL <= iLoopCount ) break;
                            m_dPositionX[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].GetPose().TranslationX; m_dPositionY[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].GetPose().TranslationY; m_dAngle[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].GetPose().Rotation * ( 180 / Math.PI ); m_dScore[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].Score;
                            //m_objGraphics[ iLoopCount ] = m_objPMAlignTool.Results[ iLoopCount ].CreateResultGraphics( CogPMAlignResultGraphicConstants.All );
                        }
                        CogImage8Grey FixtureImage = (CogImage8Grey)m_objPMAlignTool.InputImage;//.CopyBase( CogImageCopyModeConstants.CopyPixels ) as CogImage8Grey;
                        FixtureImage.SelectedSpaceName = "@";

                        m_objFixtureTool.InputImage = FixtureImage;
                        m_objFixtureTool.RunParams.UnfixturedFromFixturedTransform = m_objPMAlignTool.Results[ 0 ].GetPose();
                        m_objFixtureTool.Run();
                        objResultData.objCogImage = m_objFixtureTool.OutputImage as CogImage8Grey;
                        if( null == objResultData.objCogImage )
                        {
                            m_objFixtureTool.InputImage = FixtureImage;
                            m_objFixtureTool.RunParams.UnfixturedFromFixturedTransform = m_objPMAlignTool.Results[ 0 ].GetPose();
                            m_objFixtureTool.Run();
                            objResultData.objCogImage = m_objFixtureTool.OutputImage as CogImage8Grey;
                        }
                        
                    } else {
                        for( int iLoopCount = 0; iLoopCount < ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL; iLoopCount++ ) {
                           // m_objGraphics = null;
                            m_dPositionX[ iLoopCount ] = m_dPositionY[ iLoopCount ] = -1; m_dAngle[ iLoopCount ] = 0; m_dScore[ iLoopCount ] = 0;
                        }
                        objResultData.objCogImage = objCogImage;
                        MakeErrorMessage( "HLRun", 5205, "Pattern Fail" );
                        break;
                    }
                } catch( System.Exception ex ) {
                    Trace.Write( ex.Message + "-> " + ex.StackTrace );
                    break;
                }

                bReturn = true;
            } while( false );

            if( null != m_objPMAlignTool.Results ) {
                for( int iLoopCount = 0; iLoopCount < m_objPMAlignTool.Results.Count; iLoopCount++ ) {
                    if( false == bReturn ) {
                    }
                    else {
                        if( ( int )Abstract.CVisionLibraryAbstract.CResultData.enumInspectPosition.INSPECT_POSITION_FINAL <= iLoopCount ) break;
                        objResultData.objGraphics[ iLoopCount ] = new CogCompositeShape();
                        objResultData.objGraphics[ iLoopCount ] = m_objGraphics[ iLoopCount ];
                        objResultData.dPositionX[ iLoopCount ] = m_dPositionX[ iLoopCount ]; objResultData.dPositionY[ iLoopCount ] = m_dPositionY[ iLoopCount ]; objResultData.dPositionAngle[ iLoopCount ] = m_dAngle[ iLoopCount ]; objResultData.dScore[ iLoopCount ] = m_dScore[ iLoopCount ];
                    }
                }
            }
            if( null != objCogImage )
                objResultData.bitmapInputImage = null;//objCogImage.ToBitmap();
            //m_objGraphics[ 0 ] = m_objPMAlignTool.CreateLastRunRecord();
            objResultData.objGraphics[ 0 ] = m_objPMAlignTool.CreateLastRunRecord();
            objResultData.bResult = bReturn; objResultData.eLibrary = CResultData.enumLibrary.PMALIGN;
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

        public override void SetIdle()
        {
            m_objPMAlignTool.InputImage = new CogImage8Grey();
            m_objPMAlignTool.Run();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 비전라이브러리 참조반환 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object HLGetReferenceLibrary()
        {
            return m_objPMAlignTool;
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
