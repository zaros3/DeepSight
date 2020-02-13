using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSight;

namespace HLDevice.PLC
{
    public class CDevicePLCVirtual : HLDevice.Abstract.CDevicePLCAbstract
    {
        HLDevice.Abstract.CDevicePLCAbstract.CMCProtocolError m_objError = new HLDevice.Abstract.CDevicePLCAbstract.CMCProtocolError();
        private Dictionary<string, CPLCParameter> m_objPLCParameter;
        //초기화 파라미터
        private HLDevice.Abstract.CDevicePLCAbstract.CInitializeParameter m_objInitializeParameter = new CInitializeParameter();

        // PLC 인터페이스
        private CInterfacePLC m_objInterfacePLC;
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      public CDevicePLCVirtual()
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
        public override bool HLInitialize( HLDevice.Abstract.CDevicePLCAbstract.CInitializeParameter objInitializeParameter )
        {
            bool bReturn = false;

            do {
                HLDeviceDLL.MCProtocol.CDeviceMCProtocolDefine.CInitializeParameter objParameter = new HLDeviceDLL.MCProtocol.CDeviceMCProtocolDefine.CInitializeParameter();
                objParameter.ePLCProtocolType = ( HLDeviceDLL.MCProtocol.CDeviceMCProtocolDefine.enumPLCProtocolType )objInitializeParameter.ePLCProtocolType;
                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;
                m_objPLCParameter = new Dictionary<string, CPLCParameter>( objInitializeParameter.objPLCParameter );
                m_objInitializeParameter.objPLCParameter = new Dictionary<string, CPLCParameter>( objInitializeParameter.objPLCParameter );
                m_objInitializeParameter.iInputCountAll = objInitializeParameter.iInputCountAll;
                m_objInitializeParameter.iOutputCountAll = objInitializeParameter.iOutputCountAll;
                m_objInitializeParameter.iInputCountBit = objInitializeParameter.iInputCountBit;
                m_objInitializeParameter.iOutputCountBit = objInitializeParameter.iOutputCountBit;
                m_objInitializeParameter.iInputCountWord = objInitializeParameter.iInputCountWord;
                m_objInitializeParameter.iOutputCountWord = objInitializeParameter.iOutputCountWord;
                m_objInitializeParameter.iInputCountDWord = objInitializeParameter.iInputCountDWord;
                m_objInitializeParameter.iOutputCountDWord = objInitializeParameter.iOutputCountDWord;

                //PLC인터페이스 초기화
                m_objInterfacePLC = new CInterfacePLC();
                CInterfacePLC.CInitializeParameter objInitializePLC = new CInterfacePLC.CInitializeParameter();
                objInitializePLC.iCountBitIn = m_objInitializeParameter.iInputCountBit;
                objInitializePLC.iCountBitOut = m_objInitializeParameter.iOutputCountBit;
                objInitializePLC.iCountWordIn = m_objInitializeParameter.iInputCountWord;
                objInitializePLC.iCountWordOut = m_objInitializeParameter.iOutputCountWord;
                objInitializePLC.iCountDWordIn = m_objInitializeParameter.iInputCountDWord;
                objInitializePLC.iCountDWordOut = m_objInitializeParameter.iOutputCountDWord;
                m_objInterfacePLC.Initialize( objInitializePLC );
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
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 접속 상태
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLIsConnected()
        {
            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 1개의 비트데이터 읽기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLReadBitFromPLC( string strName, ref bool bReadData, string strKey = "" )
        {
            bool bReturn = false;
            
            do {
                if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN== m_objPLCParameter[ strKey ].eCommunicationType ) {
                    bReadData = m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strKey ].iInOutIndex ];
                } else {
                    bReadData = m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strKey ].iInOutIndex ];
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 시작주소에서 설정된 사이즈만큼 읽어온다
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLReadBitFromPLC( string strName, int iCount, ref bool[] bReadData, string strKey = "" )
        {
            bool bReturn = false;
            
            do {
                for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                    if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                        bReadData[ iLoopCount ] = m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ];
                    } else {
                        bReadData[ iLoopCount ] = m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ];
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLReadWordFromPLC( string strName, int iCount, ref short[] dReadData, string strKey = "" )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            short[] sReadData = new short[ iCount ];

            do {

                if( false == m_objPLCParameter.ContainsKey( strKey ) ) {
                    break;
                } else {
                    for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                        if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                            sReadData[ iLoopCount ] = m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ];
                        } else {
                            sReadData[ iLoopCount ] = m_objInterfacePLC.sInterfacePlcWordOut [ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ];
                        }
                    }
                }

                dReadData = sReadData;
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLReadWordASCIIFromPLC( string strName, int iCount, ref string strReadData, string strKey = "" )
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
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLReadDoubleWordFromPLC( string strName, ref double dReadData, string strKey = "" )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                if( false == m_objPLCParameter.ContainsKey( strKey ) ) {
                    break;
                } else {
                    if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                        dReadData = m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strKey ].iInOutIndex ];
                    } else {
                        dReadData = m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strKey ].iInOutIndex ];
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLReadDoubleWordFromPLC( string strName, int iCount, ref double[] dReadData, string strKey = "" )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {

                if( false == m_objPLCParameter.ContainsKey( strKey ) ) {
                    break;
                } else {
                    for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                        if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                            dReadData[ iLoopCount ] = m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ];
                        } else {
                            dReadData[ iLoopCount ] = m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ];
                        }
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteBitFromPLC( string strName, bool bWriteData, string strKey = "" )
        {
            bool bReturn = false;
            do {

                if( false == m_objPLCParameter.ContainsKey( strKey ) ) {
                    break;
                } else {
                    if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                        m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strKey ].iInOutIndex ] = bWriteData;
                    } else {
                        m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strKey ].iInOutIndex ] = bWriteData;
                    }
                } while( false ) ;

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteBitFromPLC( string strName, int iCount, bool[] bWriteData, string strKey = "" )
        {
            bool bReturn = false;
            do {
                for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                    if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                        m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ] = bWriteData[ iLoopCount ];
                    } else {
                        m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ] = bWriteData[ iLoopCount ];
                    }
                }
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteWordBitFromPLC( string strName, int iCount, bool[] bWriteData, string strKey = "" )
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
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteWordFromPLC(string strName, short sData, string strKey = "")
        {
            bool bReturn = false;
            do
            {
                    if (Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[strKey].eCommunicationType || Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[strKey].eCommunicationType)
                    {

                        m_objInterfacePLC.sInterfacePlcWordIn[m_objPLCParameter[strKey].iInOutIndex ] = sData;
                    }
                    else
                    {
                        m_objInterfacePLC.sInterfacePlcWordOut[m_objPLCParameter[strKey].iInOutIndex ] = sData;
                    }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteWordFromPLC( string strName, int iCount, short[] sData, string strKey = "" )
        {
            bool bReturn = false;
            do {
                for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                    if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strKey ].eCommunicationType || Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[strKey].eCommunicationType ) {

                        m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ] = sData[ iLoopCount ];
                    } else {
                        m_objInterfacePLC.sInterfacePlcWordOut[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ] = sData[ iLoopCount ];
                    }
                }
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteWordFromPLC( string strName, double dWriteData, string strKey = "" )
        {
            bool bReturn = false;
            do {
                
                if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {
                    m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strKey ].iInOutIndex ] = dWriteData;
                } else {
                    m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strKey ].iInOutIndex ] = dWriteData;
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLWriteWordFromPLC( string strName, int iCount, double[] dData, string strKey = "" )
        {
            bool bReturn = false;
            do {
                for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                    if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strKey ].eCommunicationType ) {

                        m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ] = dData[ iLoopCount ];
                    } else {
                        m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strKey ].iInOutIndex + iLoopCount ] = dData[ iLoopCount ];
                    }
                }
                bReturn = true;
            } while( false );

            return bReturn;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void MakeError()
        {
            //             HLDeviceDLL.MCProtocol.CDeviceMCProtocol..CMCU32Error objError = m_objPLC.HLGetErrorCode();
            //             m_objError.iReturnCode = objError.iReturnCode;
            //             m_objError.strEventTime = objError.strEventTime;
            //             m_objError.strFunctionName = objError.strFunctionName;
            //             m_objError.strMessage = objError.strMessage;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 알람 상태 정보를 리턴한다.
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override HLDevice.Abstract.CDevicePLCAbstract.CMCProtocolError HLGetErrorCode()
        {
            return ( HLDevice.Abstract.CDevicePLCAbstract.CMCProtocolError )m_objError.Clone();
        }

    }
}