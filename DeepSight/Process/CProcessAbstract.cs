using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HLDevice;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using Cognex.VisionPro;
using System.IO;
using Cognex.VisionPro.ImageFile;
using System.Diagnostics;

namespace DeepSight {
    public abstract class CProcessAbstract {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //structure
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 자재길이체크 파라미터
        public struct structureMaterialDistanceParameter {
            public enum enumDistanceType { TYPE_WIDTH, TYPE_HEIGHT };
            // 거리타입
            public enumDistanceType eDistanceType;
            // 이미지 가로
            public int iImageWidth;
            // 이미지 세로
            public int iImageHeight;
            // 픽셀좌표
            public double dPositionX1;
            public double dPositionY1;
            public double dPositionX2;
            public double dPositionY2;
            // 카메라간 거리
            public double dCameraDistance;
            // 픽셀 해상도
            public double dResolution;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //protected property
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 프로세스 종료
        protected bool m_bThreadExit;
        // 프로세스 쓰레드
        protected Thread m_ThreadProcess;
        // 알람 구조체
        protected CDefine.structureAlarmInformation m_objAlarmStructure;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public property
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // IO 객체
        public CDeviceIO m_objIO;
        // PLC 객체
        public CDevicePLC m_objPLC;
        // 조명 컨트롤러 객체
        public CDeviceLightController[] m_objLightController;


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CProcessAbstract()
        {
            m_bThreadExit = false;
            m_ThreadProcess = null;

            m_objAlarmStructure = new CDefine.structureAlarmInformation();
            m_objAlarmStructure.strAlarmObject = "CProcessAbstract";
            m_objAlarmStructure.strAlarmFunction = "CProcessAbstract";
            m_objAlarmStructure.strAlarmDescription = "";
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract bool Initialize();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract void DeInitialize();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 두 점간의 거리구하기, 화면중심 대 화면중심이 카메라거리 
        //설명 : 가로 : 1,2번 3,4번 세로 : 1,3번 2,4번 카메라
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual double GetDistancePointPoint( structureMaterialDistanceParameter objMaterialDistanceParameter )
        {
            double dDistance = 0;
            double dCameraDistance = 0;
            double dPositionX1 = 0, dPositionX2 = 0, dPositionY1 = 0, dPositionY2 = 0;

            // 자재 가로 타입 길이 구하기
            if( structureMaterialDistanceParameter.enumDistanceType.TYPE_WIDTH == objMaterialDistanceParameter.eDistanceType ) {
                // 카메라 거리
                dCameraDistance = objMaterialDistanceParameter.dCameraDistance;
                // 왼쪽카메라 포지션
                dPositionX1 = ( objMaterialDistanceParameter.iImageWidth / 2 ) - objMaterialDistanceParameter.dPositionX1;
                dPositionY1 = objMaterialDistanceParameter.dPositionY1;

                // 오른쪽카메라 포지션
                dPositionX2 = ( objMaterialDistanceParameter.iImageWidth / 2 ) - objMaterialDistanceParameter.dPositionX2;
                dPositionY2 = objMaterialDistanceParameter.dPositionY2;

                // 거리구하기
                // root ( ( X1 - X2 )^2 + ( Y1- Y2 )^2 )
                dDistance = dCameraDistance + ( Math.Sqrt( Math.Pow( dPositionX1 - dPositionX2, 2 ) + Math.Pow( dPositionY1 - dPositionY2, 2 ) ) * objMaterialDistanceParameter.dResolution );
            }
            // 자재 세로 타입 길이 구하기
            else if( structureMaterialDistanceParameter.enumDistanceType.TYPE_HEIGHT == objMaterialDistanceParameter.eDistanceType ) {
                // 카메라 거리
                dCameraDistance = objMaterialDistanceParameter.dCameraDistance;
                // 위쪽카메라 포지션
                dPositionX1 = objMaterialDistanceParameter.dPositionX1;
                dPositionY1 = ( objMaterialDistanceParameter.iImageHeight / 2 ) - objMaterialDistanceParameter.dPositionY1;

                // 아래쪽카메라 포지션
                dPositionX2 = objMaterialDistanceParameter.dPositionX2;
                dPositionY2 = ( objMaterialDistanceParameter.iImageHeight / 2 ) - objMaterialDistanceParameter.dPositionY2;

                // 거리구하기
                // root ( ( X1 - X2 )^2 + ( Y1- Y2 )^2 )
                dDistance = dCameraDistance + ( Math.Sqrt( Math.Pow( dPositionX1 - dPositionX2, 2 ) + Math.Pow( dPositionY1 - dPositionY2, 2 ) ) * objMaterialDistanceParameter.dResolution );
            }

            return dDistance;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 문자열 반전
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetReverseString( string strData )
        {
            var pDocument = CDocument.GetDocument;
            string strReturn = "";

            do {
                if( null == strData || "" == strData ) break;
                try {
                    char[] szReverseData = strData.ToCharArray();
                    strReturn = new string( szReverseData );
                } catch( Exception ex ) {
                    pDocument.SetUpdateLog( CDefine.enumLogType.LOG_VISION_EXCEPTION_SYSTEM, "CProcessAbstract.GetReverseString" + ex.ToString() );
                }

            } while( false );

            return strReturn;
        }

        public void BitmapToByteArr( List<Bitmap> imgBit, out List<byte[]> aftByte )
        {
            aftByte = new List<byte[]>();

            for( int i = 0; i < imgBit.Count; i++ ) {
                CogImage8Grey cogImg = new CogImage8Grey( imgBit[ i ] );
                //Shared.g_lstImg.Add( cogImg );
                ICogImage8PixelMemory pmr = cogImg.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, cogImg.Width, cogImg.Height );
                aftByte.Add( new byte[ pmr.Stride * pmr.Height ] );
                IntPtr ptr = pmr.Scan0;
                Marshal.Copy( ptr, aftByte[ i ], 0, pmr.Stride * pmr.Height );
            }
        }

        public void CogImageToByteArr( List<CogImage8Grey> objImage, out List<byte[]> aftByte )
        {
            aftByte = new List<byte[]>();

            for( int i = 0; i < objImage.Count; i++ ) {
                CogImage8Grey cogImg = objImage[ i ];
                //Shared.g_lstImg.Add( cogImg );
                ICogImage8PixelMemory pmr = cogImg.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, cogImg.Width, cogImg.Height );
                aftByte.Add( new byte[ pmr.Stride * pmr.Height ] );
                IntPtr ptr = pmr.Scan0;
                Marshal.Copy( ptr, aftByte[ i ], 0, pmr.Stride * pmr.Height );
            }
        }

        public void CogImageToByteArr( CogImage8Grey objImage, out List<byte[]> aftByte )
        {
            aftByte = new List<byte[]>();

                CogImage8Grey cogImg = objImage;
                ICogImage8PixelMemory pmr = cogImg.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, cogImg.Width, cogImg.Height );
                aftByte.Add( new byte[ pmr.Stride * pmr.Height ] );
                IntPtr ptr = pmr.Scan0;
                Marshal.Copy( ptr, aftByte[ 0 ], 0, pmr.Stride * pmr.Height );
        }

        public Bitmap BytesToBitmapGrey( byte[] imgArr, int nWidht, int nHeight )
        {
            Bitmap bmp = new Bitmap( nWidht, nHeight, PixelFormat.Format8bppIndexed );
            BitmapData data = bmp.LockBits(
                                    new Rectangle( 0, 0, nWidht, nHeight ),
                                    ImageLockMode.ReadWrite,
                                        PixelFormat.Format8bppIndexed );
            IntPtr ptr = data.Scan0;

            Marshal.Copy( imgArr, 0, ptr, nWidht * nHeight );
            bmp.UnlockBits( data );

            //모노이미지로 변환해준다 사용하지 않을경우 칼라이미지가 깨진채로 사용된다
            ColorPalette Gpal = bmp.Palette;
            for( int i = 0; i < 256; i++ ) {
                Gpal.Entries[ i ] = Color.FromArgb( i, i, i );
            }
            bmp.Palette = Gpal;

            return bmp;
        }

        public Bitmap RawBytesToBitmapGrey( byte[] imgArr, int nWidht, int nHeight )
        {
            //             CogImage8Grey cogImg = new CogImage8Grey( nWidht, nHeight );
            //             ICogImage8PixelMemory pmr = cogImg.Get8GreyPixelMemory( CogImageDataModeConstants.Read, 0, 0, cogImg.Width, cogImg.Height );
            //             int iStride = pmr.Stride;

            var output = new Bitmap( nWidht, nHeight, PixelFormat.Format8bppIndexed );
            var rect = new Rectangle( 0, 0, nWidht, nHeight );
            var bmpData = output.LockBits( rect, ImageLockMode.ReadWrite, output.PixelFormat );

            // Row-by-row copy
            var arrRowLength = nWidht * Image.GetPixelFormatSize( output.PixelFormat ) / 8;
            var ptr = bmpData.Scan0;
            for( var i = 0; i < nHeight; i++ ) {
                Marshal.Copy( imgArr, i * arrRowLength, ptr, arrRowLength );
                ptr += bmpData.Stride;
            }

            output.UnlockBits( bmpData );

            //모노이미지로 변환해준다 사용하지 않을경우 칼라이미지가 깨진채로 사용된다
            ColorPalette Gpal = output.Palette;
            for( int i = 0; i < 256; i++ ) {
                Gpal.Entries[ i ] = Color.FromArgb( i, i, i );
            }
            output.Palette = Gpal;

            return output;
        }

        public Bitmap BytesToBitmapColor( byte[] imgArr, int nWidht, int nHeight )
        {
            Bitmap output = new Bitmap( nWidht, nHeight, PixelFormat.Format24bppRgb );
            output.SetResolution( nWidht, nHeight );
            Rectangle rect = new Rectangle( 0, 0, nWidht, nHeight );
            BitmapData bmpData = output.LockBits( rect, ImageLockMode.ReadWrite, output.PixelFormat );
            int bpp = Image.GetPixelFormatSize( output.PixelFormat ) / 8;
            byte[] newArr = new byte[ nWidht * nHeight * bpp ];
            var arrRowLength = nWidht * bpp;
            var ptr = bmpData.Scan0;
            var k = 0;
            for( var y = 0; y < nHeight; y++ ) {
                for( var x = 0; x < nWidht; x++ ) {
                    newArr[ k ] = imgArr[ y * nWidht + x ];
                    newArr[ k + 1 ] = imgArr[ y * nWidht + x ];
                    newArr[ k + 2 ] = imgArr[ y * nWidht + x ];
                    k += bpp;
                }
            }

            for( var i = 0; i < nHeight; i++ ) {
                Marshal.Copy( newArr, i * arrRowLength, ptr, arrRowLength );
                ptr += bmpData.Stride;
            }
            output.UnlockBits( bmpData );
            return output;
        }

        unsafe public byte[] PtrToArray( IntPtr pData, int nLen )
        {
            if( pData == null ) return null;
            byte[] arr = new byte[ nLen ];
            Marshal.Copy( pData, arr, 0, nLen );
            return arr;
        }

        public void SetDataToCsv( string strPath, short[,] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );

                for( int iLoopHeight = 0; iLoopHeight < objData.GetLength( 0 ); iLoopHeight++ ) {
                    string strLine = "";
                    for( int iLoopWidth = 0; iLoopWidth < objData.GetLength( 1 ); iLoopWidth++ ) {
                        //   strLine += string.Format( "{0},", objData[ iLoopHeight, iLoopWidth ].ToString() );
                        objStreamWriter.Write( objData[ iLoopHeight, iLoopWidth ] );
                        objStreamWriter.Write( "," );
                    }
                    objStreamWriter.WriteLine( strLine );
                }
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        public void SetDataToCsv( string strPath, double[,] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );

                for( int iLoopHeight = 0; iLoopHeight < objData.GetLength( 0 ); iLoopHeight++ ) {
                    string strLine = "";
                    for( int iLoopWidth = 0; iLoopWidth < objData.GetLength( 1 ); iLoopWidth++ ) {
                        // strLine += string.Format( "{0},", objData[ iLoopHeight, iLoopWidth ].ToString() );
                        objStreamWriter.Write( objData[ iLoopHeight, iLoopWidth ] );
                        objStreamWriter.Write( "," );
                    }
                    objStreamWriter.WriteLine( strLine );
                }
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        public void SetDataToCsv( string strPath, short[] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );

                for( int iLoopHeight = 0; iLoopHeight < objData.Length; iLoopHeight++ ) {
                    objStreamWriter.WriteLine( objData[ iLoopHeight ] );
                    //strLine = objData[ iLoopHeight ].ToString();
                    //objStreamWriter.WriteLine( strLine );
                }
//                objStreamWriter.WriteLine( strLine );
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        public void SetDataToCsv( string strPath, byte[] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );

                //Stopwatch st = new Stopwatch();
                //st.Start();
                for( int iLoopHeight = 0; iLoopHeight < objData.Length; iLoopHeight++ ) {
                    //strLine = objData[ iLoopHeight ].ToString();
                    //objStreamWriter.WriteLine( strLine );
                    objStreamWriter.WriteLine( objData[ iLoopHeight ] );
                }
                //Trace.WriteLine( st.ElapsedMilliseconds.ToString() );
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        public void SetDataToCsv( string strPath, string[] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.OpenOrCreate, FileAccess.Write );
                objFileStream.Seek( 0, SeekOrigin.End );

                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );
                for( int iLoopHeight = 0; iLoopHeight < objData.Length; iLoopHeight++ ) {
                    objStreamWriter.WriteLine( objData[ iLoopHeight ] );
                }
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }
        public void SetDataToCsv( string strPath, int[,] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );
             //   Stopwatch st = new Stopwatch();
             //   st.Start();
                for( int iLoopHeight = 0; iLoopHeight < objData.GetLength( 0 ); iLoopHeight++ ) {
                    string strLine = "";
                    for( int iLoopWidth = 0; iLoopWidth < objData.GetLength( 1 ); iLoopWidth++ ) {
                        //strLine += string.Format( "{0},", objData[ iLoopHeight, iLoopWidth ].ToString() );
                        objStreamWriter.Write( objData[ iLoopHeight, iLoopWidth ] );
                        objStreamWriter.Write( "," );
                    }
                    objStreamWriter.WriteLine( strLine );
                }
                //Trace.WriteLine( st.ElapsedMilliseconds.ToString() );
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        public void SetDataToCsv( string strPath, byte[,] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );

                for( int iLoopHeight = 0; iLoopHeight < objData.GetLength( 0 ); iLoopHeight++ ) {
                    string strLine = "";
                    for( int iLoopWidth = 0; iLoopWidth < objData.GetLength( 1 ); iLoopWidth++ ) {
                        //   strLine += string.Format( "{0},", objData[ iLoopHeight, iLoopWidth ].ToString() );
                        objStreamWriter.Write( objData[ iLoopHeight, iLoopWidth ] );
                        objStreamWriter.Write( "," );
                    }
                    objStreamWriter.WriteLine( strLine );
                }
                objStreamWriter.Close();
                objFileStream.Close();
            } catch( Exception ex ) {
                Trace.WriteLine( ex.Message );
            }
        }

        public int[,] ConvolveMedianValueFilter( int[,] sourceArray, int windowWidth, int windowHeight )
        {
            int sourceWidth = sourceArray.GetUpperBound( 1 ) + 1;
            int sourceHeight = sourceArray.GetUpperBound( 0 ) + 1;
            int[,] resultArray = new int[ sourceHeight, sourceWidth ];
            int xPadding = windowWidth / 2;
            int yPadding = windowHeight / 2;
            int windowSize = windowHeight * windowWidth;
            int[] targetArray = new int[ windowSize ];
            int index;

            for( int y = 0; y < sourceHeight - 2 * yPadding; y++ ) {
                for( int x = 0; x < sourceWidth - 2 * xPadding; x++ ) {
                    index = 0;
                    for( int r = 0; r < windowHeight; r++ ) {
                        for( int c = 0; c < windowWidth; c++ ) {
                            targetArray[ index++ ] = sourceArray[ y + r, x + c ];
                        }
                    }
                    resultArray[ y + yPadding, x + xPadding ] = GetMedianValue( targetArray, windowSize );
                }
            }
            for( int y = 0; y < yPadding; y++ ) {
                for( int x = xPadding; x < sourceWidth - xPadding; x++ ) {
                    resultArray[ y, x ] = resultArray[ yPadding, x ];
                    resultArray[ sourceHeight - 1 - y, x ] = resultArray[ sourceHeight - 1 - yPadding, x ];
                }
            }
            for( int x = 0; x < xPadding; x++ ) {
                for( int y = 0; y < sourceHeight; y++ ) {
                    resultArray[ y, x ] = resultArray[ y, xPadding ];
                    resultArray[ y, sourceWidth - 1 - x ] = resultArray[ y, sourceWidth - 1 - xPadding ];
                }
            }
            return resultArray;

        }

        private int GetMedianValue( int[] targetArray, int targetLength )
        {
            int value;
            for( int i = 0; i < targetLength - 1; i++ ) {
                for( int j = i + 1; j < targetLength; j++ ) {
                    if( targetArray[ i ] > targetArray[ j ] ) {
                        value = targetArray[ i ];
                        targetArray[ i ] = targetArray[ j ];
                        targetArray[ j ] = value;
                    }
                }
            }
            return ( targetArray[ targetLength / 2 ] );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : IO 비트 결과 확인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual bool WaitDigitalBit( HLDevice.CDeviceIO objIO, string strAddressName, bool bStatus, int iTimeOut = 1000, int iSleepPeriod = 5 )
        {
            bool bReturn = false;
            bool bResult = false;
            var pDocument = CDocument.GetDocument;
            do {
                if( null == objIO )
                    break;

                if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                    // IO 비트 리드
                    if( true == objIO.HLGetDigitalBit( strAddressName, ref bResult ) ) {
                        while( 0 < iTimeOut && bResult != bStatus ) {
                            Thread.Sleep( iSleepPeriod ); iTimeOut -= iSleepPeriod;
                            // IO 비트 리드
                            objIO.HLGetDigitalBit( strAddressName, ref bResult );
                        }
                    } else {
                        break;
                    }

                    // Timeout Check
                    if( 0 >= iTimeOut ) {
                        break;
                    }
                }
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 비트 결과 확인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual bool WaitPLCInterfaceBit( HLDevice.CDevicePLC objPLC, string strAddressName, bool bStatus, int iTimeOut = 1000, int iSleepPeriod = 5 )
        {
            bool bReturn = false;
            bool bResult = false;
            var pDocument = CDocument.GetDocument;
            do {
                if( null == objPLC )
                    break;
                if( null == pDocument )
                    break;

                // 시뮬레이션 모드일 경우 Pass
                if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                    // 워드 인덱스 Read

                    objPLC.HLReadBitFromPLC( strAddressName, ref bResult );

                    while( 0 < iTimeOut && bResult != bStatus ) {
                        Thread.Sleep( iSleepPeriod ); iTimeOut -= iSleepPeriod;
                        objPLC.HLReadBitFromPLC( strAddressName, ref bResult );
                    }

                    // Timeout Check
                    if( 0 >= iTimeOut ) {
                        break;
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 비트 결과 확인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual bool WaitPLCInterfaceBit( HLDevice.CDevicePLC objPLC, string strAddressName, int iBitIndex, bool bStatus, int iTimeOut = 1000, int iSleepPeriod = 5 )
        {
            bool bReturn = false;
            bool bResult = false;
            var pDocument = CDocument.GetDocument;
            do {
                if( null == objPLC )
                    break;
                if( null == pDocument )
                    break;

                // 시뮬레이션 모드일 경우 Pass
                if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                    // 워드 인덱스 Read

                    short[] pReadData = new short[ 1 ];
                    objPLC.HLReadWordFromPLC( strAddressName, 1, ref pReadData );

                    int iWordIndex = objPLC.HLGetPLCParameter()[ strAddressName ].iInOutIndex;
                    bResult = objPLC.HLGetInterfacePLC().objWordToBitIn[ iWordIndex ].bWordToBit[ iBitIndex ];
                    while( 0 < iTimeOut && bResult != bStatus ) {
                        Thread.Sleep( iSleepPeriod ); iTimeOut -= iSleepPeriod;
                        objPLC.HLReadWordFromPLC( strAddressName, 1, ref pReadData );
                        bResult = objPLC.HLGetInterfacePLC().objWordToBitIn[ iWordIndex ].bWordToBit[ iBitIndex ];
                    }

                    // Timeout Check
                    if( 0 >= iTimeOut ) {
                        break;
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : PLC 결과 확인
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual bool WaitPLCInterfaceValue( HLDevice.CDevicePLC objPLC, string strAddressName, double dStatus, int iTimeOut = 1000, int iSleepPeriod = 5 )
        {
            bool bReturn = false;
            double dResult = 0;
            var pDocument = CDocument.GetDocument;
            do {
                if( null == objPLC )
                    break;
                if( null == pDocument )
                    break;

                // 시뮬레이션 모드일 경우 Pass
                if( CDefine.enumSimulationMode.SIMULATION_MODE_OFF == pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
                    // 워드 인덱스 Read

                    objPLC.HLReadDoubleWordFromPLC( strAddressName, ref dResult );

                    while( 0 < iTimeOut && dResult != dStatus ) {
                        Thread.Sleep( iSleepPeriod ); iTimeOut -= iSleepPeriod;
                        objPLC.HLReadDoubleWordFromPLC( strAddressName, ref dResult );
                    }

                    // Timeout Check
                    if( 0 >= iTimeOut ) {
                        break;
                    }
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 저장
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual bool SaveImage( CInspectionResult.CResult objResult )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            do {
                if( true == pDocument.m_objConfig.GetSystemParameter().bImageSave )
                    ThreadPool.QueueUserWorkItem( new WaitCallback( ThreadSaveImage ), objResult );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        private void ThreadSaveImage( object objInspectionResult )
        {
            CInspectionResult.CResult objResult = objInspectionResult as CInspectionResult.CResult;
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            var pDocument = CDocument.GetDocument;
            do {
                try {

                    string strCellID = "";
                    if( "" == objResult.objResultCommon.strCellID ) {
                        strCellID = System.DateTime.Now.ToString( "HH.mm.ss" );
                    } else {
                        strCellID = objResult.objResultCommon.strCellID;
                    }
                    string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
                    

                    string strImageType = CDefine.enumSaveImageType.TYPE_BMP == pDocument.m_objConfig.GetSystemParameter().eSaveImageType ? ".bmp" : ".jpg";

                    string strPosition = "Position_" + ( objResult.objResultCommon.iInspectionPosition + 1 ).ToString();
                    // 메인저장경로\오늘날짜\제품ID\이미지들...
                    string strImagePath = objResult.strSaveImagePath + "\\" + strToday + "\\" + objResult.objResultCommon.eResult.ToString() + "\\" + strCellID + "\\" + strPosition + "\\";
                    //폴더 존재 여부 체크
                    if( false == Directory.Exists( strImagePath ) ) {
                        //폴더가 없다면 생성
                        Directory.CreateDirectory( strImagePath );
                    }

                    string strTime = System.DateTime.Now.ToString( "HHmmss" );

                    // Origin 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objInputGrabOriginalImageBitmapVIDI.Count; iLoopCount++ ) {
                        string strFileName = "Origin_VIDI_" + strTime + "_" + iLoopCount.ToString() + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objInputGrabOriginalImageBitmapVIDI[ iLoopCount ];
                            // 이미지 엔코더 타입 jpeg 변경
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            // 엔코더 품질 설정 변수 생성
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            // 이미지 압축률 50% 설정
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objInputGrabOriginImageVIDI[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    // Origin 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objInputGrabOriginalImageBitmapMeasure.Count; iLoopCount++ ) {
                        string strFileName = "Origin_Measure_" + strTime + "_" + iLoopCount.ToString() + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objInputGrabOriginalImageBitmapMeasure[ iLoopCount ];
                            // 이미지 엔코더 타입 jpeg 변경
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            // 엔코더 품질 설정 변수 생성
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            // 이미지 압축률 50% 설정
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objInputGrabOriginImageMeasure[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }


                    // PMS 이미지 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objPMSImageBitmap.Count; iLoopCount++ ) {
                        string strFileName = "PMS_" + iLoopCount.ToString() + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objPMSImageBitmap[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objPMSImage[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objPMSImageBitmapMeasure.Count; iLoopCount++ ) {
                        string strFileName = "PMS_Measure_" + iLoopCount.ToString() + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objPMSImageBitmap[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objPMSImageMeasure[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    // VIDI 이미지는 파일명이 달라야 트레인할수 있기때문에 파일명을 수정 마지막에 시간붙이기
                    string strVidiName = pDocument.m_objConfig.GetRecipeParameter( ( int )CDefine.enumCamera.CAMERA_1 ).objInspectionParameter[ objResult.objResultCommon.iInspectionPosition ].objVidiSerchRegion[ 0 ].objVidiParameter.strStreamName;
                    // VIDI 이미지 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objCropImageBitmapVidi.Count; iLoopCount++ ) {
                        string strFileName = strVidiName + "_CROP_VIDI_" + iLoopCount.ToString() + "-" +strTime + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapVidi[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objCropImageVidi[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiResultOverlayGraphic.Count; iLoopCount++ ) {
                        string strFileName = "CROP_OVERLAY_VIDI_" + iLoopCount.ToString() + "-" + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapVidi[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objVidiResultOverlayGraphic[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    // 치수측정이미지 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objCropImageBitmapVidi.Count; iLoopCount++ ) {
                        string strFileName = "CROP_DIMENSION_" + iLoopCount.ToString() + "-" + strTime + strImageType;
                        string strFileNameSecond = "CROP_DIMENSION_SECOND_" + iLoopCount.ToString() + "-" + strTime + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapMeasure[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );

                            objbmp = objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ];
                            objEncoder = GetEncoder( ImageFormat.Jpeg );
                            myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileNameSecond, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objCropImageMeasure[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();

                            objImage = new CogImage8Grey( objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ] );
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileNameSecond, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }
                } catch( System.Exception ex ) {
                    System.Diagnostics.Trace.WriteLine( "Error SaveImage : " + ex.Message + " ->" + ex.StackTrace );
                    break;
                }
            } while( false );
            objImageFileTool.Operator.Close();
        }
        private ImageCodecInfo GetEncoder( ImageFormat format )
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach( ImageCodecInfo codec in codecs ) {
                if( codec.FormatID == format.Guid ) {
                    return codec;
                }
            }
            return null;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지 저장
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual bool Save3DData( CInspectionResult.CResult objResult )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            do {
                if( true == pDocument.m_objConfig.GetSystemParameter().bImageSave )
                    ThreadPool.QueueUserWorkItem( new WaitCallback( ThreadSave3DData ), objResult );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        private void ThreadSave3DData( object objInspectionResult )
        {
            CInspectionResult.CResult objResult = objInspectionResult as CInspectionResult.CResult;
            CogImageFileTool objImageFileTool; objImageFileTool = new CogImageFileTool();

            var pDocument = CDocument.GetDocument;
            do {
                try {

                    string strCellID = "";
                    if( "" == objResult.objResultCommon.strCellID ) {
                        strCellID = System.DateTime.Now.ToString( "HH.mm.ss" );
                    } else {
                        strCellID = objResult.objResultCommon.strCellID;
                    }
                    string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );

                    string strImageType = CDefine.enumSaveImageType.TYPE_BMP == pDocument.m_objConfig.GetSystemParameter().eSaveImageType ? ".bmp" : ".jpg";

                    string strPosition = "Position_" + ( objResult.objResultCommon.iInspectionPosition + 1 ).ToString();
                    // 메인저장경로\오늘날짜\제품ID\이미지들...
                    string strImagePath = objResult.strSaveImagePath + "\\" + strToday + "\\" + objResult.objResultCommon.eResult.ToString() + "\\" + strCellID + "\\" + strPosition + "\\";
                    //폴더 존재 여부 체크
                    if( false == Directory.Exists( strImagePath ) ) {
                        //폴더가 없다면 생성
                        Directory.CreateDirectory( strImagePath );
                    }

                    for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enum3DImageType.TYPE_FINAL; iLoopCount++ ) {
                        string strFileName = ( ( CDefine.enum3DImageType )iLoopCount ).ToString() + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.obj3DImageBitmap[ iLoopCount ];
                            // 이미지 엔코더 타입 jpeg 변경
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            // 엔코더 품질 설정 변수 생성
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            // 이미지 압축률 50% 설정
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.obj3DImage[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }
                    

                    // VIDI 이미지는 파일명이 달라야 트레인할수 있기때문에 파일명을 수정 마지막에 시간붙이기
                    string strTime = System.DateTime.Now.ToString( "HHmmss" );
                    // VIDI 이미지 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objCropImageBitmapVidi.Count; iLoopCount++ ) {
                        string strFileName = "CROP_VIDI_" + iLoopCount.ToString() + "-" + strTime + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapVidi[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            //CogImage8Grey objImage = objResult.objResultCommon.objCropImageVidi[ iLoopCount ];
                            CogImage8Grey objImage = new CogImage8Grey( objResult.objResultCommon.objVidiResultImage[ iLoopCount ] );
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiResultOverlayGraphic.Count; iLoopCount++ ) {
                        string strFileName = "CROP_OVERLAY_VIDI_" + iLoopCount.ToString() + "-" + strImageType;
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapVidi[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = objResult.objResultCommon.objVidiResultOverlayGraphic[ iLoopCount ];
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    // 치수측정이미지 저장
                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objCropImageBitmapMeasureSecond.Count; iLoopCount++ ) {
                        string strFileName = "CROP_DIMENSION_" + iLoopCount.ToString() + strImageType;
                        // 비트맵 객체 생성.
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = new CogImage8Grey( objResult.objResultCommon.objCropImageBitmapMeasureSecond[ iLoopCount ] );
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.obj3DResultOverlayGraphic.Count; iLoopCount++ ) {
                        string strFileName = "CROP_DIMENSION_OVERLAY_" + iLoopCount.ToString() + strImageType;
                        // 비트맵 객체 생성.
                        if( ".jpg" == strImageType ) {
                            Bitmap objbmp = objResult.objResultCommon.objCropImageBitmapMeasure[ iLoopCount ];
                            System.Drawing.Imaging.ImageCodecInfo objEncoder = GetEncoder( ImageFormat.Jpeg );
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters objEncoderParameter = new EncoderParameters();
                            objEncoderParameter.Param[ 0 ] = new EncoderParameter( myEncoder, 50L );
                            objbmp.Save( strImagePath + strFileName, objEncoder, objEncoderParameter );
                        } else {
                            CogImage8Grey objImage = new CogImage8Grey( objResult.objResultCommon.obj3DResultOverlayGraphic[ iLoopCount ] );
                            objImageFileTool.InputImage = objImage;
                            objImageFileTool.Operator.Open( strImagePath + strFileName, CogImageFileModeConstants.Write );
                            objImageFileTool.Run();
                        }
                    }

                    // CSV 저장을 해야되는데
                    {
                        string strFileName = "Height2D.csv";
                        //SetDataToCsv( strImagePath + strFileName, objResult.objResultCommon.obj3DDataHeightCrop2d );
                        strFileName = "Height_Original.csv";
                        //SetDataToCsv( strImagePath + strFileName, objResult.objResultCommon.obj3DDataHeight2dOriginal );

                        //if( true == pDocument.m_objConfig.GetSystemParameter().bVidiTeachMode ) {
                        strFileName = "Height," + strPosition.ToUpper() + "," + objResult.objResultCommon.i3DImageWidth.ToString() + "," + objResult.objResultCommon.i3DImageHeight.ToString() + "," + ".csv";
                        SetDataToCsv( strImagePath + strFileName, objResult.objResultCommon.obj3DDataHeightOrigin );

                        string strRawDataPath = objResult.strSaveImagePath + "\\" + "RawData\\";
                        //폴더 존재 여부 체크
                        if( false == Directory.Exists( strRawDataPath ) ) {
                            //폴더가 없다면 생성
                            Directory.CreateDirectory( strRawDataPath );
                        }
                        File.Copy( strImagePath + strFileName, strRawDataPath + strFileName, true );


                        strFileName = "Intesnisy," + strPosition.ToUpper() + "," + objResult.objResultCommon.i3DImageWidth.ToString() + "," + objResult.objResultCommon.i3DImageHeight.ToString() + "," + ".csv";
                        SetDataToCsv( strImagePath + strFileName, objResult.objResultCommon.obj3DDataIntensityOrigin );
                        File.Copy( strImagePath + strFileName, strRawDataPath + strFileName, true );
                        //}
                        for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objCropImageBitmapVidi.Count; iLoopCount++ ) {
                            strFileName = "HEIGHT_RESULT_" + iLoopCount.ToString() + ".csv";
                            SetDataToCsv( strImagePath + strFileName, objResult.objResultCommon.obj3DResultHeightData[ iLoopCount ] );
                        }

                    }


                } catch( System.Exception ex ) {
                    System.Diagnostics.Trace.WriteLine( "Error SaveImage : " + ex.Message + " ->" + ex.StackTrace );
                    break;
                }
            } while( false );
            objImageFileTool.Operator.Close();
        }

        public void RotateRight( ref int[,] matrix )
        {
            int lengthY = matrix.GetLength( 0 );
            int lengthX = matrix.GetLength( 1 );
            int[,] result = new int[ lengthX, lengthY ];
            for( int y = 0; y < lengthY; y++ )
                for( int x = 0; x < lengthX; x++ )
                    result[ x, y ] = matrix[ lengthY - 1 - y, x ];

            matrix = result;
        }

        public void RotateRight( ref double[,] matrix )
        {
            int lengthY = matrix.GetLength( 0 );
            int lengthX = matrix.GetLength( 1 );
            double[,] result = new double[ lengthX, lengthY ];
            for( int y = 0; y < lengthY; y++ )
                for( int x = 0; x < lengthX; x++ )
                    result[ x, y ] = matrix[ lengthY - 1 - y, x ];

            matrix = result;
        }

        public void RotateRight( ref byte[,] matrix )
        {
            int lengthY = matrix.GetLength( 0 );
            int lengthX = matrix.GetLength( 1 );
            byte[,] result = new byte[ lengthX, lengthY ];
            for( int y = 0; y < lengthY; y++ )
                for( int x = 0; x < lengthX; x++ )
                    result[ x, y ] = matrix[ lengthY - 1 - y, x ];

            matrix = result;
        }

        public void RotateLeft( ref double[,] matrix )
        {
            int lengthY = matrix.GetLength( 0 );
            int lengthX = matrix.GetLength( 1 );
            double[,] result = new double[ lengthX, lengthY ];
            for( int y = 0; y < lengthY; y++ )
                for( int x = 0; x < lengthX; x++ )
                    result[ x, y ] = matrix[ y, lengthX - 1 - x ];
            matrix = result;
        }

        public void RotateLeft( ref int[,] matrix )
        {
            int lengthY = matrix.GetLength( 0 );
            int lengthX = matrix.GetLength( 1 );
            int[,] result = new int[ lengthX, lengthY ];
            for( int y = 0; y < lengthY; y++ )
                for( int x = 0; x < lengthX; x++ )
                    result[ x, y ] = matrix[ y, lengthX - 1 - x ];
            matrix = result;
        }

        public void RotateLeft( ref byte[,] matrix )
        {
            int lengthY = matrix.GetLength( 0 );
            int lengthX = matrix.GetLength( 1 );
            byte[,] result = new byte[ lengthX, lengthY ];
            for( int y = 0; y < lengthY; y++ )
                for( int x = 0; x < lengthX; x++ )
                    result[ x, y ] = matrix[ y, lengthX - 1 - x ];
            matrix = result;
        }


    }
}