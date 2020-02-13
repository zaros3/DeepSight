using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PylonC.NET;
using PylonC.NETSupportLibrary;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using HLDevice.Abstract;

using Lmi3d;
using Lmi3d.Zen;
using Lmi3d.GoSdk;
using Lmi3d.Zen.Io;
using Lmi3d.GoSdk.Messages;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using DeepSight;

namespace HLDevice.Camera
{
	public class CDeviceCameraGocator : CDeviceCameraAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 카메라 에러
		private CDeviceCameraAbstract.CCameraError m_objError;
		// 초기화 파라매터
		private CDeviceCameraAbstract.CInitializeParameter m_objInitializeParameter;
		
		// 비트맵 형식으로 들어옴
		private Bitmap m_objBitImage;
		// 스레드
		private bool m_bThreadExit;
		private Thread m_ThreadConnect;
		// 그랩 콜백
		private CallBackFunctionGrabImage m_objCallback;
		// Trace 콜백
		private CallBackTraceMessage m_objCallbackTraceMessage;
		// 그랩 에러 콜백
		private CallBackGrabError m_objCallbackGrabError;
        // 멀티샷의 경우 카운팅 처리
        private int m_iMultiGrabIndex;
        private const int m_iMaxCountMultiGrab = 4;

        private GoSensor m_objCamera;
        private GoSystem m_objCameraSystem;
        private const int DEF_3D_DATA_MULTIPLE = 1000000;
        int iGCCollectCount;

        //// Gocator 샘플제공
        // 연결 여부
        bool m_bConnect = false;
        //bool m_bScan = false;
        // Gocator 와의 연결 상태를 지속적으로 체크
        bool unresponsive = false;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDeviceCameraGocator()
		{
			m_bThreadExit = false;
			m_objCallback = null;
			m_objCallbackTraceMessage = null;
			m_objCallbackGrabError = null;
			m_objError = new CDeviceCameraAbstract.CCameraError();
			m_objInitializeParameter = new CInitializeParameter();

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
		public override bool HLInitialize( CDeviceCameraAbstract.CInitializeParameter objInitializeParameter )
		{
			bool bReturn = false;

			do {
				// 초기화 파라매터 복사
				m_objInitializeParameter = ( CInitializeParameter )objInitializeParameter.Clone();

                //m_objCamera = new Lmi3d.GoSdk.GoSensor();
                KApiLib.Construct();
                GoSdkLib.Construct();
                m_objCameraSystem = new GoSystem();
                iGCCollectCount = 0;
                // 카메라 접속
                SetCameraConnect();
                m_objCameraSystem.Stop();
                
//                m_objCameraSystem.EnableData( true );
//                m_objCameraSystem.SetDataHandler( ReceiveData );
                

//                 m_objCamera.Setup.GetSurfaceGeneration().GenerationType = GoSurfaceGenerationType.FixedLength;
//                 m_objCamera.Setup.GetSurfaceGeneration().FixedLengthLength = 150;
//                 m_objCamera.Setup.GetSurfaceGeneration().FixedLengthStartTrigger = GoSurfaceGenerationStartTrigger.Sequential;

                // 접속 스레드 시작
                m_ThreadConnect = new Thread( ThreadConnect );
				m_ThreadConnect.Start( this );

				bReturn = true;
			} while( false );

			return bReturn;
		}

        public void ReceiveData( KObject data )
        {
            // 데이터 들어오면 카메라 시스템 정지
            var pDocument = DeepSight.CDocument.GetDocument;
            
            //if( CDefine.enumTrigger.TRIGGER_OFF == pDocument.GetTrigger( ( int )CDefine.enumCamera.CAMERA_1 ) ) return;
            m_objCameraSystem.Stop();
            CImageData3D objImageData3D = new CImageData3D();
            // 일단 해상도를 임의로 지정하자

            GoDataSet dataSet = ( GoDataSet )data;
            pDocument.SetUpdateLog(CDefine.enumLogType.LOG_PROCESS, "GOCATOR RECEIVE DATA : " + dataSet.Count.ToString() );
            for( UInt32 i = 0; i < dataSet.Count; i++ ) {
                GoDataMsg dataObj = ( GoDataMsg )dataSet.Get( i );
                switch( dataObj.MessageType ) {
                    case GoDataMessageType.Stamp: {
                            GoStampMsg stampMsg = ( GoStampMsg )dataObj;
                            for( UInt32 j = 0; j < stampMsg.Count; j++ ) {
                                GoStamp stamp = stampMsg.Get( j );
                                Console.WriteLine( "Frame Index = {0}", stamp.FrameIndex );
                                Console.WriteLine( "Time Stamp = {0}", stamp.Timestamp );
                                Console.WriteLine( "Encoder Value = {0}", stamp.Encoder );
                            }
                        }
                        break;
                    case GoDataMessageType.UniformSurface: {
                            #pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                            GoSurfaceMsg surfaceMsg = ( GoSurfaceMsg )dataObj;
                            #pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                            objImageData3D.iOffsetZ = surfaceMsg.ZOffset;
                            objImageData3D.iResolutionX = surfaceMsg.XResolution;// * DEF_3D_DATA_MULTIPLE;
                            objImageData3D.iResolutionY = surfaceMsg.YResolution;
                            objImageData3D.iResolutionZ = surfaceMsg.ZResolution;
                            int lWidth = objImageData3D.iWidth = ( int )surfaceMsg.Width;
                            int lHeight = objImageData3D.iHeight = ( int )surfaceMsg.Length;
                            int bufferSize = lWidth * lHeight;
                            IntPtr bufferPointer = surfaceMsg.Data;
                            
                            Console.WriteLine( "Whole Part Height Map received:" );
                            Console.WriteLine( " Buffer width: {0}", lWidth );
                            Console.WriteLine( " Buffer Height: {0}", lHeight );

                            objImageData3D.objHeightDataOrigin = new short[ bufferSize ];

                            Marshal.Copy( bufferPointer, objImageData3D.objHeightDataOrigin, 0, objImageData3D.objHeightDataOrigin.Length );
                           // SetDataToCsv("d:\\test.csv", objImageData3D.objHeightDataOrigin);
                        }
                        break;
                    case GoDataMessageType.SurfaceIntensity: {
                            GoSurfaceIntensityMsg surfaceMsg = ( GoSurfaceIntensityMsg )dataObj;
                            long width = surfaceMsg.Width;
                            long length = surfaceMsg.Length;
                            long bufferSize = width * length;
                            IntPtr bufferPointeri = surfaceMsg.Data;

                            Console.WriteLine( "Whole Part Intensity Image received:" );
                            Console.WriteLine( " Buffer width: {0}", width );
                            Console.WriteLine( " Buffer length: {0}", length );
                            objImageData3D.objIntensityDataOrigin = new byte[ bufferSize ];
                            Marshal.Copy( bufferPointeri, objImageData3D.objIntensityDataOrigin, 0, objImageData3D.objIntensityDataOrigin.Length );

//                             objImageData3D.objBitmapIntensity = CopyDataToBitmap( ( int )length, ( int )width, objImageData3D.objIntensityDataOrigin );
//                             objImageData3D.objBitmapIntensity.Save( "d:\\Intensity.bmp" );
                        }
                        break;
                    default:
                        break;
                }
            }

            if( null != m_objCallback ) {
                CImageData objData = new CImageData();
                objData.bGrabComplete = true;
                objData.bitmapImage = objImageData3D.objBitmapIntensity;
                objData.objCameraData3D = objImageData3D;
                m_objCallback( objData );
            }

            iGCCollectCount++;

            if( 1 < iGCCollectCount ) {
                GC.Collect();
                iGCCollectCount = 0;
            }

        }

        public void SetDataToCsv( string strPath, short[,] objData )
        {
            try {
                FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
                StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );

                for( int iLoopHeight = 0; iLoopHeight < objData.GetLength( 0 ); iLoopHeight++ ) {
                    string strLine = "";
                    for( int iLoopWidth = 0; iLoopWidth < objData.GetLength( 1 ); iLoopWidth++ ) {
                        strLine += objData[ iLoopHeight, iLoopWidth ].ToString();
                        strLine += ",";
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

                string strLine = "";

                for( int iLoopHeight = 0; iLoopHeight < objData.Length; iLoopHeight++ ) {
                    strLine = objData[ iLoopHeight ].ToString();
                    objStreamWriter.WriteLine( strLine );
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
                
                for( int iLoopHeight = 0; iLoopHeight < objData.GetLength( 0 ); iLoopHeight++ ) {
                    string strLine = "";
                    for( int iLoopWidth = 0; iLoopWidth < objData.GetLength( 1 ); iLoopWidth++ ) {
                        strLine += objData[ iLoopHeight, iLoopWidth ].ToString();
                        strLine += ",";
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


        public Bitmap CopyDataToBitmap( int iWidth, int iHeight, byte[] data )
        {
            Bitmap bmp = new Bitmap( iWidth, iHeight, PixelFormat.Format8bppIndexed );
            BitmapData bmpdata = bmp.LockBits(
                                    new Rectangle( 0, 0, iWidth, iHeight ),
                                    ImageLockMode.ReadWrite,
                                        PixelFormat.Format8bppIndexed );
            IntPtr ptr = bmpdata.Scan0;
            Marshal.Copy( data, 0, ptr, iWidth * iHeight );
            bmp.UnlockBits( bmpdata );
            //모노이미지로 변환해준다 사용하지 않을경우 칼라이미지가 깨진채로 사용된다
            ColorPalette Gpal = bmp.Palette;
            for( int i = 0; i < 256; i++ ) {
                Gpal.Entries[ i ] = Color.FromArgb( i, i, i );
            }
            bmp.Palette = Gpal;
            
            return bmp;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void HLDeInitialize()
		{
			// 접속 스레드 종료
			m_bThreadExit = true;
			m_ThreadConnect.Join();
			// 카메라 접속 해제
			SetCameraDisconnect();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 객체 리턴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override object HLGetDeviceObject()
		{
            return m_objCamera;// ( CDeviceCameraImageProviderBasler )m_objImageProvider.Clone();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 접속 상태
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLIsConnected()
		{
			bool bReturn = false;

			do {

                bReturn = !unresponsive;// m_objCamera.IsConnected(); 
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 일괄 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetCameraConfig( CInitializeParameter.structureCameraConfig objCameraConfig )
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
		//목적 : 콜백
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override void HLSetCallbackFunctionGrabImage( CallBackFunctionGrabImage objCallback )
		{
			m_objCallback = objCallback;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : Trace 콜백
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override void HLSetCallbackTraceMessage( CallBackTraceMessage objCallback )
		{
			m_objCallbackTraceMessage = objCallback;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 그랩 에러 콜백
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override void HLSetCallbackGrabError( CallBackGrabError objCallback )
		{
			m_objCallbackGrabError = objCallback;
		}



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : ConnectCamera
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool SetCameraConnect()
		{
			bool bReturn = false;

			do {
                try {
                    KIpAddress ipAddress = KIpAddress.Parse( m_objInitializeParameter.str3DCameraIP );
                    GoDataSet dataSet = new GoDataSet();
                    m_objCamera = m_objCameraSystem .FindSensorByIpAddress( ipAddress );
                    m_objCamera.Connect();

                } catch( Exception ex ) {

                    Trace.WriteLine( "CAMERA GOCATOR : " + ex.ToString() );
                }
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 연결 해제
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SetCameraDisconnect()
		{
            m_objCamera.Disconnect();
            m_objCameraSystem.Stop();
            m_objCamera.Dispose();
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnImageReadyEventCallback()
		{
			try {
                CDeviceCameraImageProviderBasler.Image objImage = null;// m_objImageProvider.GetCurrentImage();

				if( null != objImage ) {
					if( BitmapFactory.IsCompatible( m_objBitImage, objImage.Width, objImage.Height, objImage.Color ) ) {
					}
					else {
						// 비트맵으로 호환 가능한 경우 비트맵 이미지 생성
						BitmapFactory.CreateBitmap( out m_objBitImage, objImage.Width, objImage.Height, false );
					}

					BitmapFactory.UpdateBitmap( m_objBitImage, objImage.Buffer, objImage.Width, objImage.Height, objImage.Color );

                    // 멀티그랩에서 사용될 인덱스
                    // 하이브 소스코드를 참고함
                    if ( m_iMultiGrabIndex >= m_iMaxCountMultiGrab ) m_iMultiGrabIndex = 0;

					if( null != m_objCallback ) {
						CImageData objData = new CImageData();
						objData.bGrabComplete = true;
                        objData.bitmapImage = m_objBitImage;// ( Bitmap )m_objBitImage.Clone();
						objData.iImageWidth = m_objBitImage.Width;
						objData.iImageHeight = m_objBitImage.Height;
                        objData.iMultiGrabImageIndex = m_iMultiGrabIndex;
						m_objCallback( objData );
					}

                    iGCCollectCount++;

					if( 4 < iGCCollectCount ) {
						GC.Collect();
						iGCCollectCount = 0;
					}
				}
			}
			catch( System.Exception ex ) {
				if( null != m_objCallbackTraceMessage ) {
					m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
				}
			}
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DAlign()
        {
            bool bReturn = false;
            do {
                m_objCamera.Align();
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DBackup( string strDestPath )
        {
            bool bReturn = false;
            do {
                m_objCamera.Backup( strDestPath );
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DCancel()
        {
            bool bReturn = false;
            do {
                m_objCamera.Cancel();
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DCanStart()
        {
            bool bReturn = false;
            do {
                m_objCamera.CanStart();
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DStart()
        {
            bool bReturn = false;
            do {
                try {
                    m_objCameraSystem.EnableData( true );
                    m_objCameraSystem.SetDataHandler( ReceiveData );
                    m_objCameraSystem.Start();
                } catch( Exception ex ) {
                    Trace.WriteLine( "HL3DStart : " + ex.ToString() );
                    break;
                }
                
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DStop()
        {
            bool bReturn = false;
            do {
                try {
                    //m_objCamera.Stop();
                    m_objCameraSystem.Stop();
                } catch( Exception ex ) {
                    Trace.WriteLine( "HL3DStop : " + ex.ToString() );
                    break;
                }

                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DReset( bool bWait )
        {
            bool bReturn = false;
            do {
                m_objCamera.Reset( bWait );
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DResetEncoder()
        {
            bool bReturn = false;
            do {
                m_objCamera.ResetEncoder();
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DScheduledStart( long lValue )
        {
            bool bReturn = false;
            do {
                m_objCamera.ScheduledStart( lValue );
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DSnapshot()
        {
            bool bReturn = false;
            do {
                m_objCamera.Snapshot();
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DSaveBitmap( string strDestPath )
        {
            bool bReturn = false;
            do {
                //  

                m_objCamera.ExportBitmap( GoReplayExportSourceType.Intensity, GoDataSource.Top, strDestPath );
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DSaveCSV( string strDestPath )
        {
            bool bReturn = false;
            do {
                m_objCamera.ExportCsv( strDestPath );
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DTrigger()
        {
            bool bReturn = false;
            do {
                m_objCamera.Trigger();
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DWaitForBuddies( ulong uTimeOut )
        {
            bool bReturn = false;
            do {
                m_objCamera.WaitForBuddies( uTimeOut );
                bReturn = true;
            } while( false );
            return bReturn;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DSetScanLength( int iScanLength )
        {
            bool bReturn = false;
            do {
                try {
                m_objCamera.Setup.ScanMode = GoMode.Surface;
                m_objCamera.Setup.GetSurfaceGeneration().GenerationType = GoSurfaceGenerationType.FixedLength;
                m_objCamera.Setup.GetSurfaceGeneration().FixedLengthLength = iScanLength;
                m_objCamera.Setup.GetSurfaceGeneration().FixedLengthStartTrigger = GoSurfaceGenerationStartTrigger.Sequential;
                } catch( Exception ex ) {
                    Trace.WriteLine( "HL3DSetScanLength : " + ex.ToString() );
                    break;
                }



                // 최대영역을 구하기 위해 센서 넓이와 크기를 0으로설정하고
                //m_objCamera.Setup.SetActiveAreaWidth( GoRole.Main, 0 );
               // m_objCamera.Setup.SetActiveAreaX( GoRole.Main, 0 );

                // 최대 센서의 넓이를 가져온다
               // double dWidth = m_objCamera.Setup.GetActiveAreaWidthLimitMax( GoRole.Main );
                // 센서 시작위치를 제일 왼쪽으로 보내고 
               // m_objCamera.Setup.SetActiveAreaX( GoRole.Main, (dWidth/2) * -1 );
                // 센서 넓이를 최대로 설정한다
               // m_objCamera.Setup.SetActiveAreaWidth( GoRole.Main, dWidth );


                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 스캔 폭 설정
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DSetScanWidth( double dWidth )
        {
            bool bReturn = false;
            do {
                try {
                    // 센서 넓이를 최대로 설정한다
                    m_objCamera.Setup.SetActiveAreaWidth( GoRole.Main, dWidth );
                    m_objCamera.Setup.SetActiveAreaX( GoRole.Main, ( dWidth / 2 ) * -1 );
                } catch( Exception ex) {
                    Trace.WriteLine( "" + ex.ToString() );
                    break;
                }
                bReturn = true;
            } while( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 3D 센서( Gocator )
        //설명 : 스캔 폭 설정
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HL3DSetScanMaxWidth()
        {
            bool bReturn = false;
            do {
                try {
                    // 최대영역을 구하기 위해 센서 넓이와 크기를 0으로설정하고
                    m_objCamera.Setup.SetActiveAreaWidth( GoRole.Main, 0 );
                    m_objCamera.Setup.SetActiveAreaX( GoRole.Main, 0 );

                    // 최대 센서의 넓이를 가져온다
                    double dWidth = m_objCamera.Setup.GetActiveAreaWidthLimitMax( GoRole.Main );
                    // 센서 시작위치를 제일 왼쪽으로 보내고 
                    m_objCamera.Setup.SetActiveAreaX(GoRole.Main, (dWidth / 2) * -1);
                    // 센서 넓이를 최대로 설정한다
                    m_objCamera.Setup.SetActiveAreaWidth( GoRole.Main, dWidth );
                } catch( Exception ex) {
                    Trace.WriteLine( "" + ex.ToString() );
                    break;
                }
                bReturn = true;
            } while( false );
            return bReturn;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 카메라 접속 스레드
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ThreadConnect( Object state )
		{
            CDeviceCameraGocator pThis = ( CDeviceCameraGocator )state;

			while( false == pThis.m_bThreadExit ) {
                // 커넥트 변수가 false면 카메라 커넥트 다시 시도
                // 				if( false == pThis.HLIsConnected() ) {
                // 						pThis.SetCameraConnect();
                // 				}

                pThis.GocatorStateCheck();

                Thread.Sleep( 100 );
			}
		}

        private void GocatorStateCheck()
        {
            //Lmi3d.GoSdk.GoState Gostate = GetSensorState();

            if( m_objCamera == null )
                return;

            
            Lmi3d.GoSdk.GoState Gostate = m_objCamera.State;

            switch( Gostate ) {
                case Lmi3d.GoSdk.GoState.Online:
                    // 센서가 감지 되었지만, 연결되진 않음
                    break;
                case Lmi3d.GoSdk.GoState.Offline:
                    // 센서가 감지되지 않고 연결도 안됨

                    // 만약 연결되어있던 중 강제로 전원이 나가면, 기존 버튼 초기화
                    if( m_bConnect == true ) {
                        m_bConnect = false;
                      //  m_bScan = false;
                    }
                    break;
                case Lmi3d.GoSdk.GoState.Resetting:
                    // 센서 연결이 끊어지고 재설정중
                    break;
                case Lmi3d.GoSdk.GoState.Incompatible:
                    // 센서가 연결되었지만 클라이언트와 프로토콜이 호환되지 않음(펌웨어 업그레이드 필요)
                    break;
                case Lmi3d.GoSdk.GoState.Inconsistent:
                    // 센서가 연결되었지만 원격 상태가 변경됨
                    break;
                case Lmi3d.GoSdk.GoState.Unresponsive:
                    // 센서가 연결되었지만, 더이상 감지가 되지 않고 있음
                    unresponsive = true;
                    break;
                case Lmi3d.GoSdk.GoState.Cancelled:
                    // 센서가 연결되었지만 GoSensor_Cancel 기능으로 인해 통신이 중단됨
                    break;
                case Lmi3d.GoSdk.GoState.Incomplete:
                    // 센서가 연결되었지만 필요한 버디 센서가 없음(버디 연결 대기 또는 제거 해야 함)
                    break;
                case Lmi3d.GoSdk.GoState.Busy:
                    // 센서가 연결되었지만, 다른 센서가 현재 제어중(버디로 연결되었을 경우 해당)
                    break;
                case Lmi3d.GoSdk.GoState.Ready:
                    // 센서가 연결됨
                                        // 센서가 연결되어 현재 실행중 
                    if( unresponsive == true ) {
                        // 센서가 끊어졌다가 다시 붙은 최초 시점
                        // 이때 센서 연결수행
                        Thread.Sleep( 1000 );
                        Connect();
                        unresponsive = false;
                    }
                    break;
                case Lmi3d.GoSdk.GoState.Running:
                    // 센서가 연결되어 현재 실행중 
                    if( unresponsive == true ) {
                        // 센서가 끊어졌다가 다시 붙은 최초 시점
                        // 이때 센서 연결수행
                        Thread.Sleep( 1000 );
                        Connect();
                        unresponsive = false;
                    }
                    break;
                case Lmi3d.GoSdk.GoState.Upgrading:
                    // 현재 센서가 업그레이드 중
                    break;
            }
        }

        public void Connect()
        {
            try {
                // Gocator 초기화 및 접속 (GoSDK 예제를 참고)
                KApiLib.Construct();
                GoSdkLib.Construct();
                KIpAddress ipAddress;

                // GoSystem 필수
                m_objCameraSystem = new GoSystem();

                ipAddress = KIpAddress.Parse( m_objInitializeParameter.str3DCameraIP );

                m_objCamera = m_objCameraSystem.FindSensorByIpAddress( ipAddress );
                m_objCamera.Connect();

                m_objCameraSystem.EnableData( true );
                m_bConnect = true;

            } catch( Exception ex ) {
                Trace.WriteLine( "Exception Gocator : " +  ex.ToString() );
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 알람 상태 정보를 리턴한다.
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override CDeviceCameraAbstract.CCameraError HLGetErrorCode()
		{
			return ( CDeviceCameraAbstract.CCameraError )m_objError.Clone();
		}
	}
}