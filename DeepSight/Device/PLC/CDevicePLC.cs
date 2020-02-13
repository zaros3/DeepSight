using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSight;


namespace HLDevice
{
    public class CDevicePLC
    {
        private HLDevice.Abstract.CDevicePLCAbstract m_objPLC;
        private Dictionary<string, HLDevice.Abstract.CDevicePLCAbstract.CPLCParameter> m_objPLCParameter;
        private HLDevice.Abstract.CDevicePLCAbstract.CInitializeParameter m_objInitializeParameter;

        // PLC 인터페이스
        private CInterfacePLC m_objInterfacePLC;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDevicePLC( HLDevice.Abstract.CDevicePLCAbstract objDeviceAbstractPLC )
        {
            m_objPLC = objDeviceAbstractPLC;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLInitialize( HLDevice.Abstract.CDevicePLCAbstract.CInitializeParameter objInitializeParameter )
        {
            m_objPLCParameter = new Dictionary<string, HLDevice.Abstract.CDevicePLCAbstract.CPLCParameter>( objInitializeParameter.objPLCParameter );
            m_objInitializeParameter = new Abstract.CDevicePLCAbstract.CInitializeParameter();
            m_objInitializeParameter = objInitializeParameter.Clone() as Abstract.CDevicePLCAbstract.CInitializeParameter;
            m_objInitializeParameter.objPLCParameter = new Dictionary<string, HLDevice.Abstract.CDevicePLCAbstract.CPLCParameter>( objInitializeParameter.objPLCParameter );

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

            return m_objPLC.HLInitialize( objInitializeParameter );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void HLDeInitialize()
        {
            m_objPLC.HLDeInitialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 버전 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string HLGetVersion()
        {
            return m_objPLC.HLGetVersion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 버전 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLIsConnected()
        {
            return m_objPLC.HLIsConnected();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 1개의 비트데이터 읽기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLReadBitFromPLC( string strName, ref bool bReadData, bool bRealAddress = false )
        {
            bool bReturn = false;

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadBitFromPLC( strName, ref bReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadBitFromPLC( m_objPLCParameter[ strName ].strAddress, ref bReadData ) ) break;
                        //m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strName ].iInOutIndex ] = bReadData;
                        if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strName ].eCommunicationType )
                            m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strName ].iInOutIndex ] = bReadData;
                        else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                            m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strName ].iInOutIndex ] = bReadData;
                    }
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
        public bool HLReadBitFromPLC( string strName, int iCount, ref bool[] bReadData, bool bRealAddress = false )
        {
            bool bReturn = false;

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadBitFromPLC( strName, iCount, ref bReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadBitFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, ref bReadData ) ) break;
                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                            if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = bReadData[ iLoopCount ];
                            else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = bReadData[ iLoopCount ];
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
        public bool HLReadWordFromPLC( string strName, int iCount, ref short[] sReadData, bool bRealAddress = false )
        {
            bool bReturn = false;

            //1 Word -> 16bit Word를 Bit로 변환하여 데이터 넘김
            int iWordBitSize = 16;
            bool [,]pData = new bool[ iCount, iWordBitSize ];

            CInterfacePLC.CWordToBit[] objData = new CInterfacePLC.CWordToBit[ iCount ];
            for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                objData[ iLoopCount ] = new CInterfacePLC.CWordToBit();
            }

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadWordFromPLC( strName, iCount, ref sReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, ref sReadData, strName ) ) break;

                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                            for( int iLoopBit = 0; iLoopBit < 16; iLoopBit++ ) {
                                int index = iLoopBit / 16;
                                pData[ iLoopCount, iLoopBit ] = ( ( sReadData[ iLoopCount ] >> ( iLoopBit % 16 ) ) & 0x01 ) > 0 ? true : false;

                                if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType ) {
                                    m_objInterfacePLC.objWordToBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ].bWordToBit[ iLoopBit ] = pData[ iLoopCount, iLoopBit ];
                                } else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType ) {
                                    m_objInterfacePLC.objWordToBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ].bWordToBit[ iLoopBit ] = pData[ iLoopCount, iLoopBit ];
                                }
                            }
                        }

                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                            if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sReadData[ iLoopCount ];
                            else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.sInterfacePlcWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sReadData[ iLoopCount ];
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
        //목적 : 1개의 비트데이터 읽기
        //설명 : R영역 데이터 리딩이라서 이부분에 일단 비트읽어오는 부분 추가
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLReadWordFromPLC( string strName, out CInterfacePLC.CWordToBit objWordToBit, bool bRealAddress = false )
        {
            bool bReturn = false;

            int iCount = 1;

            short[] sReadData = new short[ iCount ];
            //1 Word -> 16bit Word를 Bit로 변환하여 데이터 넘김
            int iWordBitSize = 16;
            bool [,]pData = new bool[ iCount, iWordBitSize ];

            CInterfacePLC.CWordToBit objData = new CInterfacePLC.CWordToBit();

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadWordFromPLC( strName, iCount, ref sReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, ref sReadData, strName ) ) break;

                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                            for( int iLoopBit = 0; iLoopBit < iWordBitSize; iLoopBit++ ) {
                                int index = iLoopBit / 16;
                                pData[ iLoopCount, iLoopBit ] = ( ( sReadData[ iLoopCount ] >> ( iLoopBit % 16 ) ) & 0x01 ) > 0 ? true : false;

                                if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType ) {
                                    m_objInterfacePLC.objWordToBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ].bWordToBit[ iLoopBit ] = pData[ iLoopCount, iLoopBit ];
                                } else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType ) {
                                    m_objInterfacePLC.objWordToBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ].bWordToBit[ iLoopBit ] = pData[ iLoopCount, iLoopBit ];
                                }
                            }

                            if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sReadData[ iLoopCount ];
                            else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.sInterfacePlcWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sReadData[ iLoopCount ];
                        }
                    }
                }

                bReturn = true;
            } while( false );

            objWordToBit = objData.Clone() as CInterfacePLC.CWordToBit;
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : R영역 데이터 리딩이라서 이부분에 일단 비트읽어오는 부분 추가
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLReadWordFromPLC( string strName, int iCount, out CInterfacePLC.CWordToBit[] objWordToBit, bool bRealAddress = false )
        {
            bool bReturn = false;
            short[] sReadData = new short[ iCount ];

            //1 Word -> 16bit Word를 Bit로 변환하여 데이터 넘김
            int iWordBitSize = 16;
            bool [,]pData = new bool[ iCount, iWordBitSize ];

            CInterfacePLC.CWordToBit[] objData = new CInterfacePLC.CWordToBit[ iCount ];
            for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                objData[ iLoopCount ] = new CInterfacePLC.CWordToBit();
            }

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadWordFromPLC( strName, iCount, ref sReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, ref sReadData, strName ) ) break;
                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {

                            for( int iLoopBit = 0; iLoopBit < iWordBitSize; iLoopBit++ ) {
                                int index = iLoopBit / 16;
                                pData[ iLoopCount, iLoopBit ] = ( ( sReadData[ iLoopCount ] >> ( iLoopBit % 16 ) ) & 0x01 ) > 0 ? true : false;

                                if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType ) {
                                    m_objInterfacePLC.objWordToBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ].bWordToBit[ iLoopBit ] = pData[ iLoopCount, iLoopBit ];
                                } else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType ) {
                                    m_objInterfacePLC.objWordToBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ].bWordToBit[ iLoopBit ] = pData[ iLoopCount, iLoopBit ];
                                }
                            }

                            if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sReadData[ iLoopCount ];
                            else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.sInterfacePlcWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sReadData[ iLoopCount ];
                        }
                    }
                }

                bReturn = true;
            } while( false );

            objWordToBit = objData.Clone() as CInterfacePLC.CWordToBit[];

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLReadWordASCIIFromPLC( string strName, int iCount, ref string strReadData, bool bRealAddress = false )
        {
            bool bReturn = false;
            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadWordASCIIFromPLC( strName, iCount, ref strReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadWordASCIIFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, ref strReadData, strName ) ) break;
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
        public bool HLReadDoubleWordFromPLC( string strName, ref double dReadData, bool bRealAddress = false )
        {
            bool bReturn = false;

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadDoubleWordFromPLC( strName, ref dReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadDoubleWordFromPLC( m_objPLCParameter[ strName ].strAddress, ref dReadData, strName ) ) break;
                        dReadData = dReadData / m_objInitializeParameter.dMultiple;
                        if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
                            m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strName ].iInOutIndex ] = dReadData;
                        else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                            m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strName ].iInOutIndex ] = dReadData;
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
        public bool HLReadDoubleWordFromPLC( string strName, int iCount, ref double[] dReadData, bool bRealAddress = false )
        {
            bool bReturn = false;

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLReadDoubleWordFromPLC( strName, iCount, ref dReadData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLReadDoubleWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, ref dReadData, strName ) ) break;
                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {

                            dReadData[ iLoopCount ] = dReadData[ iLoopCount ] / m_objInitializeParameter.dMultiple;
                            if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount  ] = dReadData[ iLoopCount ];
                            else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
                                m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = dReadData[ iLoopCount ];
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
        public bool HLWriteBitFromPLC( string strName, bool bWriteData, bool bRealAddress = false )
        {
            bool bReturn = false;

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteBitFromPLC( strName, bWriteData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLWriteBitFromPLC( m_objPLCParameter[ strName ].strAddress, bWriteData, strName ) ) break;
//                         if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strName ].eCommunicationType )
//                             m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strName ].iInOutIndex ] = bWriteData;
//                         else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_OUT == m_objPLCParameter[ strName ].eCommunicationType )
//                             m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strName ].iInOutIndex ] = bWriteData;
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
        public bool HLWriteBitFromPLC( string strName, int iCount, bool[] bWriteData, bool bRealAddress = false )
        {
            bool bReturn = false;

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteBitFromPLC( strName, iCount, bWriteData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLWriteBitFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, bWriteData, strName ) ) break;
//                         for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
//                             if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_IN == m_objPLCParameter[ strName ].eCommunicationType )
//                                 m_objInterfacePLC.bInterfacePlcBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = bWriteData[ iLoopCount ];
//                             else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_BIT_OUT == m_objPLCParameter[ strName ].eCommunicationType )
//                                 m_objInterfacePLC.bInterfacePlcBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = bWriteData[ iLoopCount ];
//                         }
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //         //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //         //생성 : 
        //         //수정 : 
        //         //목적 :
        //         //설명 : 
        //         //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //         public bool HLWriteWordBitFromPLC( string strName, int iCount, bool[] bWriteData, bool bRealAddress = false )
        //         {
        //             bool bReturn = false;
        //             do {
        //                 // 일단 이함수는 사용안하는걸로
        //                 if( true == bRealAddress ) {
        //                     if( false == m_objPLC.HLWriteWordBitFromPLC( strName, iCount, bWriteData ) ) break;
        //                 } else {
        //                     if( false == m_objPLCParameter.ContainsKey( strName ) ) {
        //                         break;
        //                     } else {
        //                         if( false == m_objPLC.HLWriteWordBitFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, bWriteData ) ) break;
        //                     }
        //                 }
        // 
        //                 bReturn = true;
        //             } while( false );
        // 
        //             return bReturn;
        //         }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLWriteWordFromPLC( string strName, bool bData, int iIndex, bool bRealAddress = false )
        {
            bool bReturn = false;
            int iCount = 1;
            short[] sData = new short[ iCount ];

            do {

                if( 16 <= iIndex ) {
                    break;
                }

                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteWordFromPLC( strName, iCount, sData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {

                        // 해당 번지 읽어와서 현재 인덱스의 데이터만 변경한다
                        CInterfacePLC.CWordToBit objWordToBit;

                        if( false == HLReadWordFromPLC( m_objPLCParameter[ strName ].strAddress, out objWordToBit ) ) break;
                        objWordToBit.bWordToBit[ iIndex ] = bData;


                        for( int iLoopCount = 0; iLoopCount < 16; iLoopCount++ ) {
                            int ivalue = ( true == objWordToBit.bWordToBit[ iLoopCount ] ) ? 1 : 0;
                            sData[ 0 ] |= ( short )( ivalue << ( iLoopCount % 16 ) );
                        }

                        if( false == m_objPLC.HLWriteWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, sData, strName ) ) break;

                        // R영역 데이터 리딩이라서 이부분에 일단 비트읽어오는 부분 추가
                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                            if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType ) {
                                m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sData[ 0 ];
                                m_objInterfacePLC.objWordToBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = objWordToBit.Clone() as CInterfacePLC.CWordToBit;
                            } else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType ) {
                                m_objInterfacePLC.sInterfacePlcWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sData[ 0 ];
                                m_objInterfacePLC.objWordToBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = objWordToBit.Clone() as CInterfacePLC.CWordToBit;
                            }
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
        public bool HLWriteWordFromPLC( string strName, CInterfacePLC.CWordToBit objWordToBit, bool bRealAddress = false )
        {
            bool bReturn = false;
            int iCount = 1;
            short[] sData = new short[ iCount ];

            do {

                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteWordFromPLC( strName, iCount, sData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {

                        for( int iLoopCount = 0; iLoopCount < 16; iLoopCount++ ) {
                            int ivalue = ( true == objWordToBit.bWordToBit[ iLoopCount ] ) ? 1 : 0;
                            sData[ 0 ] |= ( short )( ivalue << ( iLoopCount % 16 ) );
                        }

                        if( false == m_objPLC.HLWriteWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, sData, strName ) ) break;
                        // R영역 데이터 리딩이라서 이부분에 일단 비트읽어오는 부분 추가
//                         for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
//                             if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType ) {
//                                 m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sData[ 0 ];
//                                 m_objInterfacePLC.objWordToBitIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = objWordToBit.Clone() as CInterfacePLC.CWordToBit;
//                             } else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType ) {
//                                 m_objInterfacePLC.sInterfacePlcWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sData[ 0 ];
//                                 m_objInterfacePLC.objWordToBitOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = objWordToBit.Clone() as CInterfacePLC.CWordToBit;
//                             }
//                         }
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
        public bool HLWriteWordFromPLC(string strName, short sWriteData, bool bRealAddress = false)
        {
            bool bReturn = false;
            do
            {
                if (true == bRealAddress)
                {
                    if (false == m_objPLC.HLWriteWordFromPLC(strName, sWriteData)) break;
                }
                else
                {
                    if (false == m_objPLCParameter.ContainsKey(strName))
                    {
                        break;
                    }
                    else
                    {
                        if (false == m_objPLC.HLWriteWordFromPLC(m_objPLCParameter[strName].strAddress, sWriteData, strName)) break;
                    }
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
        public bool HLWriteWordFromPLC( string strName, int iCount, short[] sData, bool bRealAddress = false )
        {
            bool bReturn = false;
            int iWordBitSize = 16;

            CInterfacePLC.CWordToBit[] objData = new CInterfacePLC.CWordToBit[ iCount ];
            for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                objData[ iLoopCount ] = new CInterfacePLC.CWordToBit();
            }

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteWordFromPLC( strName, iCount, sData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {

                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {

                            for( int iLoopBit = 0; iLoopBit < iWordBitSize; iLoopBit++ ) {
                                int index = iLoopBit / 16;
                                objData[ iLoopCount ].bWordToBit[ iLoopBit ] = ( ( sData[ iLoopCount ] >> ( iLoopBit % 16 ) ) & 0x01 ) > 0 ? true : false;
                            }
                        }

                        if( false == m_objPLC.HLWriteWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, sData, strName ) ) break;

//                         for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
//                             if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
//                                 m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sData[ iLoopCount ];
//                             else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
//                                 m_objInterfacePLC.sInterfacePlcWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = sData[ iLoopCount ];
//                         }
//                         if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
//                             m_objInterfacePLC.objWordToBitIn = objData.Clone() as CInterfacePLC.CWordToBit[];
//                         else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_WORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
//                             m_objInterfacePLC.objWordToBitOut = objData.Clone() as CInterfacePLC.CWordToBit[];
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
        public bool HLWriteWordFromPLC( string strName, double dWriteData, bool bRealAddress = false )
        {
            bool bReturn = false;
            // 0.00001을 더하는 경우는 0.0012에 10000을곱하면 11.9999999가 나온다. 특정경우인거같은데 확인해봐야함
            dWriteData = ( dWriteData /*+ 0.00001 */) * m_objInitializeParameter.dMultiple;
            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteWordFromPLC( strName, dWriteData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLWriteWordFromPLC( m_objPLCParameter[ strName ].strAddress, dWriteData, strName ) ) break;
//                         if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
//                             m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strName ].iInOutIndex ] = dWriteData;
//                         else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
//                             m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strName ].iInOutIndex ] = dWriteData;
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
        public bool HLWriteWordFromPLC( string strName, int iCount, double[] dData, bool bRealAddress = false )
        {
            bool bReturn = false;

            for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
                // 0.00001을 더하는 경우는 0.0012에 10000을곱하면 11.9999999가 나온다. 특정경우인거같은데 확인해봐야함
                dData[ iLoopCount ] = ( dData[ iLoopCount ] /*+ 0.00001 */) * m_objInitializeParameter.dMultiple;
            }

            do {
                if( true == bRealAddress ) {
                    if( false == m_objPLC.HLWriteWordFromPLC( strName, iCount, dData ) ) break;
                } else {
                    if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                        break;
                    } else {
                        if( false == m_objPLC.HLWriteWordFromPLC( m_objPLCParameter[ strName ].strAddress, iCount, dData, strName ) ) break;
                        for( int iLoopCount = 0; iLoopCount < iCount; iLoopCount++ ) {
//                             if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_IN == m_objPLCParameter[ strName ].eCommunicationType )
//                                 m_objInterfacePLC.dInterfacePlcDWordIn[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = dData[ iLoopCount ];
//                             else if( Abstract.CDevicePLCAbstract.CPLCParameter.enumPLCCommunicationType.TYPE_DWORD_OUT == m_objPLCParameter[ strName ].eCommunicationType )
//                                 m_objInterfacePLC.dInterfacePlcDWordOut[ m_objPLCParameter[ strName ].iInOutIndex + iLoopCount ] = dData[ iLoopCount ];
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
        public bool HLGetPlcAddressIndex( string strName, ref int iAddressIndex )
        {
            bool bReturn = false;

            do {
                if( false == m_objPLCParameter.ContainsKey( strName ) ) {
                    break;
                } else {
                    iAddressIndex = m_objPLCParameter[ strName ].iInOutIndex;
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
        public bool HLConvertWordToBit( short sWrodData, out bool[] pBitData, out int iBitDataSize )
        {
            bool bReturn = false;
            int iWordBitSize = 16;
            bool[] pData = new bool[ iWordBitSize ];
            do {

                for( int iLoopBit = 0; iLoopBit < iWordBitSize; iLoopBit++ ) {
                    int index = iLoopBit / 16;
                    pData[ 0 ] = ( ( sWrodData >> ( iLoopBit % 16 ) ) & 0x01 ) > 0 ? true : false;
                }


                bReturn = true;
            } while( false );

            iBitDataSize = iWordBitSize;
            pBitData = pData;

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 :
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLConvertBitToWord( bool[] pBitData, ref short sData )
        {
            bool bReturn = false;
            int iWordBitSize = 16;

            short[] pData = new short[ 1 ];
            do {


                for( int iLoopCount = 0; iLoopCount < iWordBitSize; iLoopCount++ ) {
                    int ivalue = ( true == pBitData[ iLoopCount ] ) ? 1 : 0;
                    pData[ 0 ] |= ( short )( ivalue << ( iLoopCount % 16 ) );
                }

                bReturn = true;
            } while( false );

            sData = pData[ 0 ];

            return bReturn;
        }

        public CInterfacePLC HLGetInterfacePLC()
        {
            return m_objInterfacePLC;
        }

        public Dictionary<string, HLDevice.Abstract.CDevicePLCAbstract.CPLCParameter> HLGetPLCParameter()
        {
            return m_objPLCParameter;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void MakeError()
        {
            //             HLDeviceDLL.MCProtocol.CDeviceMCProtocol..CMCU32Error objError = m_objFFU.HLGetErrorCode();
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
        public HLDevice.Abstract.CDevicePLCAbstract.CMCProtocolError HLGetErrorCode()
        {
            return m_objPLC.HLGetErrorCode();
        }

    }
}
