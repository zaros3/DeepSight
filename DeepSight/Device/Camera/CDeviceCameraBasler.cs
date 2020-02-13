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

namespace HLDevice.Camera
{
	public class CDeviceCameraBaslse : CDeviceCameraAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 카메라 에러
		private CDeviceCameraAbstract.CCameraError m_objError;
		// 초기화 파라매터
		private CDeviceCameraAbstract.CInitializeParameter m_objInitializeParameter;
		// Y축 이미지 반전 유무
		private bool m_bReverseY;
		// 이미지 90도 회전 유무
		private bool m_bRotation90;
		// 이미지 180도 회전 유무
		private bool m_bRotation180;
		// 이미지 270도 회전 유무
		private bool m_bRotation270;
		// 그랩 수집 수량
		private int iGCCollectCount;
		// 이미지 제공 클래스
		public CDeviceCameraImageProviderBasler m_objImageProvider;
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

        private int m_iMultiGrabIndexImageFile;
        // 이미지 데이터
        private List<CDeviceCameraAbstract.CImageData> m_lstImageData;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDeviceCameraBaslse()
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
                m_lstImageData = new List<CDeviceCameraAbstract.CImageData>();

                // 초기화 파라매터 복사
                m_objInitializeParameter = ( CInitializeParameter )objInitializeParameter.Clone();
				// 이미지 제공자 생성
                if( CDeviceCameraAbstract.CInitializeParameter.enumUseCameraType.USE_REAL_CAMERA_TYPE == objInitializeParameter.eUseCameraType ) {
                    m_objImageProvider = new CDeviceCameraImageProviderBasler();
                } else {
                    m_objImageProvider = ( CDeviceCameraImageProviderBasler )objInitializeParameter.objCamera;
                }

                //멀티그랩
                m_iMultiGrabIndex = 0;
                iGCCollectCount = 0;
                m_iMultiGrabIndexImageFile = 0;
                // 변수 초기화
                m_bReverseY = false;
				m_bRotation90 = false;
				m_bRotation180 = false;
				m_bRotation270 = false;

				// 콜백 이벤트 등록
				//Grab Error Event Callback
				m_objImageProvider.GrabErrorEvent += new CDeviceCameraImageProviderBasler.GrabErrorEventHandler( OnGrabErrorEventCallback );
				//Device Open Event Callback
				m_objImageProvider.DeviceOpenedEvent += new CDeviceCameraImageProviderBasler.DeviceOpenedEventHandler( OnDeviceOpenedEventCallback );
				//Device Close Event Callback
				m_objImageProvider.DeviceClosedEvent += new CDeviceCameraImageProviderBasler.DeviceClosedEventHandler( OnDeviceClosedEventCallback );
				//Device Remove Event Callback
				m_objImageProvider.DeviceRemovedEvent += new CDeviceCameraImageProviderBasler.DeviceRemovedEventHandler( OnDeviceRemovedEventCallback );
				//Grab Started Event Callback
				m_objImageProvider.GrabbingStartedEvent += new CDeviceCameraImageProviderBasler.GrabbingStartedEventHandler( OnGrabbingStartedEventCallback );
				//Grab Stopped Event Callback
				m_objImageProvider.GrabbingStoppedEvent += new CDeviceCameraImageProviderBasler.GrabbingStoppedEventHandler( OnGrabbingStoppedEventCallback );
				//Image Ready Event Callback
				m_objImageProvider.ImageReadyEvent += new CDeviceCameraImageProviderBasler.ImageReadyEventHandler( OnImageReadyEventCallback );
				//Image Closing Event Callback
				m_objImageProvider.DeviceClosingEvent += new CDeviceCameraImageProviderBasler.DeviceClosingEventHandler( OnDeviceClosingEventCallback );
				// 카메라 접속
				SetCameraConnect();
				// 접속 스레드 시작
				m_ThreadConnect = new Thread( ThreadConnect );
				m_ThreadConnect.Start( this );

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
			return ( CDeviceCameraImageProviderBasler )m_objImageProvider.Clone();
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
				if( null == m_objImageProvider ) break;

				bReturn = m_objImageProvider.IsOpen;
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
				// 연결 상태 아니면 빠져나감
				if( false == m_objImageProvider.IsOpen ) break;
				if( CInitializeParameter.enumUseCameraType.USE_VIRTUAL_CAMERA_TYPE == m_objInitializeParameter.eUseCameraType ) break;
				if( false == HLSetExposureTime( objCameraConfig.dExposureTime ) ) break;
				if( false == HLSetGain( objCameraConfig.dGain ) ) break;
				if( false == HLSetReverseX( objCameraConfig.bReverseX ) ) break;
				if( false == HLSetReverseY( objCameraConfig.bReverseY ) ) break;
				if( false == HLSetRotation90( objCameraConfig.bRotation90 ) ) break;
				if( false == HLSetRotation180( objCameraConfig.bRotation180 ) ) break;
				if( false == HLSetRotation270( objCameraConfig.bRotation270 ) ) break;
// 				if( false == HLSetWidth( objCameraConfig.iCameraWidth ) ) break;
// 				if( false == HLSetHeight( objCameraConfig.iCameraHeight ) ) break;
// 				if( false == HLSetXOffset( objCameraConfig.iCameraXOffset ) ) break;
// 				if( false == HLSetYOffset( objCameraConfig.iCameraYOffset ) ) break;
				// 디폴트로 설정해놓는 변수
				if( false == HLSetPacketSize( DeepSight.CDefine.DEF_DEFAULT_PACKET_SIZE ) ) break;
				if( false == HLSetHeartBeatTimeOut( DeepSight.CDefine.DEF_DEFAULT_HEARTBEAT_TIMEOUT ) ) break;
				if( false == HLSetFrameRate( objCameraConfig.dFrameRate ) ) break;

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
		//목적 : 게인 값 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override double HLGetGain()
		{
			double dGain = 0.0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 가능한 항목 체크
					bool bDigitalAll = Pylon.DeviceFeatureIsAvailable( m_objImageProvider.GetDeviceHandle(), "EnumEntry_GainSelector_DigitalAll" );
					bool bAll = Pylon.DeviceFeatureIsAvailable( m_objImageProvider.GetDeviceHandle(), "EnumEntry_GainSelector_All" );
					if( true == bDigitalAll ) {
						// DigitalAll 사용 가능한 경우
						Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "GainSelector", "DigitalAll" );
					}
					else if( true == bAll ) {
						// All 사용 가능한 경우
						Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "GainSelector", "All" );
					}
					// 게인 값 확인
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "GainRaw" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							dGain = ( double )GenApi.IntegerGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}
				
			} while( false );

			return dGain;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 게인 값 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetGain( double dGain )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 가능한 항목 체크
					bool bDigitalAll = Pylon.DeviceFeatureIsAvailable( m_objImageProvider.GetDeviceHandle(), "EnumEntry_GainSelector_DigitalAll" );
					bool bAll = Pylon.DeviceFeatureIsAvailable( m_objImageProvider.GetDeviceHandle(), "EnumEntry_GainSelector_All" );
					if( true == bDigitalAll ) {
						// DigitalAll 사용 가능한 경우
						Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "GainSelector", "DigitalAll" );
					}
					else if( true == bAll ) {
						// All 사용 가능한 경우
						Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "GainSelector", "All" );
					}
					// 게인 값 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "GainRaw" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// GainRaw 최소, 최대값 읽어와서 넘어가는 경우 수정
							long iMin = GenApi.IntegerGetMin( objNodeHandle );
							long iMax = GenApi.IntegerGetMax( objNodeHandle );
							if( dGain < ( double )iMin ) {
								dGain = ( double )iMin;
							}
							else if( dGain > ( double )iMax ) {
								dGain = ( double )iMax;
							}
							GenApi.IntegerSetValue( objNodeHandle, ( long )dGain );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 노출 시간 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override double HLGetExposureTime()
		{
			double dExposureTime = 0.0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 노출 시간 반환
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "ExposureTimeAbs" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							dExposureTime = GenApi.FloatGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return dExposureTime;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 노출 시간 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetExposureTime( double dExposureTime )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 노출 시간 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "ExposureTimeAbs" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 노출 시간 최소, 최대값 읽어와서 넘어가는 경우 수정
							double dMin = GenApi.FloatGetMin( objNodeHandle );
							double dMax = GenApi.FloatGetMax( objNodeHandle );
							if( dExposureTime < dMin ) {
								dExposureTime = dMin;
							}
							else if( dExposureTime > dMax ) {
								dExposureTime = dMax;
							}
							GenApi.FloatSetValue( objNodeHandle, dExposureTime );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 픽셀 넓이 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override int HLGetWidth()
		{
			int iWidth = 0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 픽셀 넓이 반환
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "Width" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							iWidth = ( int )GenApi.IntegerGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return iWidth;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 픽셀 넓이 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetWidth( int iWidth )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 픽셀 넓이 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "Width" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 픽셀 넓이 최소, 최대값 읽어와서 넘어가는 경우 수정
							int iMin = ( int )GenApi.IntegerGetMin( objNodeHandle );
							int iMax = ( int )GenApi.IntegerGetMax( objNodeHandle );
							if( iWidth < iMin ) {
								iWidth = iMin;
							}
							else if( iWidth > iMax ) {
								iWidth = iMax;
							}
							GenApi.IntegerSetValue( objNodeHandle, iWidth );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 픽셀 높이 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override int HLGetHeight()
		{
			int iHeight = 0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 픽셀 높이 반환
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "Height" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							iHeight = ( int )GenApi.IntegerGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return iHeight;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 픽셀 높이 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetHeight( int iHeight )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 픽셀 높이 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "Height" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 픽셀 넓이 최소, 최대값 읽어와서 넘어가는 경우 수정
							int iMin = ( int )GenApi.IntegerGetMin( objNodeHandle );
							int iMax = ( int )GenApi.IntegerGetMax( objNodeHandle );
							if( iHeight < iMin ) {
								iHeight = iMin;
							}
							else if( iHeight > iMax ) {
								iHeight = iMax;
							}
							GenApi.IntegerSetValue( objNodeHandle, iHeight );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 X Offset 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override int HLGetXOffset()
		{
			int iOffset = 0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 X Offset 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "OffsetX" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							iOffset = ( int )GenApi.IntegerGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return iOffset;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 X Offset 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetXOffset( int iOffset )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 X Offset 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "OffsetX" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// X Offset 최소, 최대값 읽어와서 넘어가는 경우 수정
							int iMin = ( int )GenApi.IntegerGetMin( objNodeHandle );
							int iMax = ( int )GenApi.IntegerGetMax( objNodeHandle );
							if( iOffset < iMin ) {
								iOffset = iMin;
							}
							else if( iOffset > iMax ) {
								iOffset = iMax;
							}
							GenApi.IntegerSetValue( objNodeHandle, iOffset );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 Y Offset 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override int HLGetYOffset()
		{
			int iOffset = 0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 Y Offset 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "OffsetY" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							iOffset = ( int )GenApi.IntegerGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return iOffset;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 카메라 Y Offset 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetYOffset( int iOffset )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 카메라 Y Offset 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "OffsetY" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// Y Offset 최소, 최대값 읽어와서 넘어가는 경우 수정
							int iMin = ( int )GenApi.IntegerGetMin( objNodeHandle );
							int iMax = ( int )GenApi.IntegerGetMax( objNodeHandle );
							if( iOffset < iMin ) {
								iOffset = iMin;
							}
							else if( iOffset > iMax ) {
								iOffset = iMax;
							}
							GenApi.IntegerSetValue( objNodeHandle, iOffset );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 그랩시 이미지 자를때 옵셋 센터
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetOffsetCenter( bool bCenterX, bool bCenterY )
        {
            bool bReturn = false;

            do {
                try {
                    // 예외 처리
                    if( false == m_objImageProvider.IsOpen ) break;

                    NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "CenterX" );
                    if( true == objNodeHandle.IsValid ) {
                        if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
                            GenApi.BooleanSetValue( objNodeHandle, bCenterX );
                        }
                    }

                    objNodeHandle = m_objImageProvider.GetNodeFromDevice( "CenterY" );
                    if( true == objNodeHandle.IsValid ) {
                        if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
                            GenApi.BooleanSetValue( objNodeHandle, bCenterY );
                        }
                    }

                } catch( System.Exception ex ) {
                    if( null != m_objCallbackTraceMessage ) {
                        m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
                    }
                }

                    bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 프레임 비율 반환
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override double HLGetFrameRate()
		{
			double dFrameRate = 0.0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 취득 프레임 레이트 사용 유무 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "AcquisitionFrameRateEnable" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 사용 적용
							GenApi.BooleanSetValue( objNodeHandle, true );
						}
					}
					// 취득 프레임 레이트 적용
					objNodeHandle = m_objImageProvider.GetNodeFromDevice( "AcquisitionFrameRateAbs" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							dFrameRate = GenApi.FloatGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return dFrameRate;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 프레임 비율 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetFrameRate( double dFrameRate )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 취득 프레임 레이트 사용 유무 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "AcquisitionFrameRateEnable" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 사용 적용
							GenApi.BooleanSetValue( objNodeHandle, true );
						}
					}
					// 취득 프레임 레이트 적용
					objNodeHandle = m_objImageProvider.GetNodeFromDevice( "AcquisitionFrameRateAbs" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 취득 프레임 레이트 최소, 최대값 읽어와서 넘어가는 경우 수정
							double dMin = GenApi.FloatGetMin( objNodeHandle );
							double dMax = GenApi.FloatGetMax( objNodeHandle );
							if( dFrameRate < dMin ) {
								dFrameRate = dMin;
							}
							else if( dFrameRate > dMax ) {
								dFrameRate = dMax;
							}
							GenApi.FloatSetValue( objNodeHandle, dFrameRate );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 하트비트 타임 아웃 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetHeartBeatTimeOut( int iMilliseconds )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 하트비트 타임 아웃 적용
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "HeartbeatTimeout" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							GenApi.IntegerSetValue( objNodeHandle, iMilliseconds );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : X축 이미지 반전
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetReverseX( bool bReverse )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// X축 이미지 반전
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "ReverseX" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							GenApi.BooleanSetValue( objNodeHandle, bReverse );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : Y축 이미지 반전
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetReverseY( bool bReverse )
		{
            bool bReturn = false;

            do {
                try {
                    // 예외 처리
                    if( false == m_objImageProvider.IsOpen ) break;
                    // X축 이미지 반전
                    NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "ReverseY" );
                    if( true == objNodeHandle.IsValid ) {
                        if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
                            GenApi.BooleanSetValue( objNodeHandle, bReverse );
                        } else {
                            m_bReverseY = bReverse;
                        }
                    } else {
                        m_bReverseY = bReverse;
                    }
                } catch( System.Exception ex ) {
                    if( null != m_objCallbackTraceMessage ) {
                        m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
                        // 바슬러에서 Y반전이 안되는 경우는 이미지를 돌려준다 
                        m_bReverseY = bReverse;
                    }
                    break;
                }

                bReturn = true;
            } while( false );

            return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 이미지 90도 회전
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetRotation90( bool bRotation )
		{
			bool bReturn = false;

			do {
				m_bRotation90 = bRotation;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 이미지 180도 회전
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetRotation180( bool bRotation )
		{
			bool bReturn = false;

			do {
				m_bRotation180 = bRotation;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 이미지 270도 회전
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetRotation270( bool bRotation )
		{
			bool bReturn = false;

			do {
				m_bRotation270 = bRotation;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 패킷 사이즈 반환
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override int HLGetPacketSize()
		{
			int iPacketSize = 0;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 패킷 사이즈 설정
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "GevSCPSPacketSize" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							iPacketSize = ( int )GenApi.IntegerGetValue( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

			} while( false );

			return iPacketSize;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 패킷 사이즈 설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetPacketSize( int iPacketSize )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 패킷 사이즈 설정
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "GevSCPSPacketSize" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							// 패킷 사이즈 최소, 최대값 읽어와서 넘어가는 경우 수정
							long iMin = GenApi.IntegerGetMin( objNodeHandle );
							long iMax = GenApi.IntegerGetMax( objNodeHandle );
							if( iPacketSize < ( int )iMin ) {
								iPacketSize = ( int )iMin;
							}
							else if( iPacketSize > ( int )iMax ) {
								iPacketSize = ( int )iMax;
							}
							GenApi.IntegerSetValue( objNodeHandle, ( long )iPacketSize );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 트리거 셀렉터 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetTriggerSelector( CDeviceCameraAbstract.enumTriggerSelector eTriggerSelector )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 가능한 항목 체크
					bool bAcquisitionStart = Pylon.DeviceFeatureIsAvailable( m_objImageProvider.GetDeviceHandle(), "EnumEntry_TriggerSelector_AcquisitionStart" );
					bool bFrameStart = Pylon.DeviceFeatureIsAvailable( m_objImageProvider.GetDeviceHandle(), "EnumEntry_TriggerSelector_FrameStart" );

					if( enumTriggerSelector.ACQUISITION_START == eTriggerSelector ) {
						if( false == bAcquisitionStart ) break;
						// AcquisitionStart 사용 가능한 경우
						Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerSelector", "AcquisitionStart" );
					}
					else if( enumTriggerSelector.FRAME_START == eTriggerSelector ) {
						if( false == bFrameStart ) break;
						// FrameStart 사용 가능한 경우
						Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerSelector", "FrameStart" );
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 트리거 모드 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetTriggerMode( CDeviceCameraAbstract.enumTriggerMode eTriggerMode )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 트리거 모드 설정
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "TriggerMode" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							if( enumTriggerMode.OFF == eTriggerMode ) {
								Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerMode", "Off" );
							}
							else if( enumTriggerMode.ON == eTriggerMode ) {
								Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerMode", "On" );
							}
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 트리거 소스 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetTriggerSource( CDeviceCameraAbstract.enumTriggerSource eTriggerSource )
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 트리거 소스 설정
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "TriggerSource" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							if( enumTriggerSource.SOFTWARE == eTriggerSource ) {
								Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerSource", "Software" );
							}
							else if( enumTriggerSource.LINE1 == eTriggerSource ) {
								Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerSource", "Line1" );
							}
							else if( enumTriggerSource.LINE2 == eTriggerSource ) {
								Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerSource", "Line2" );
							}
							else if( enumTriggerSource.LINE3 == eTriggerSource ) {
								Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "TriggerSource", "Line3" );
							}
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 라인 소스 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetLineSelect( enumLineSelector eLineSelect )
        {
            bool bReturn = false;

            do
            {
                try
                {
                    // 예외 처리
                    if ( false == m_objImageProvider.IsOpen ) break;
                    // 트리거 소스 설정
                    NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "LineSelector" );
                    if ( true == objNodeHandle.IsValid )
                    {
                        if ( true == GenApi.NodeIsAvailable( objNodeHandle ) )
                        {
                            if ( enumLineSelector.LINE1 == eLineSelect )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSelector", "Line1" );
                            }
                            else if ( enumLineSelector.LINE2 == eLineSelect )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSelector", "Line2" );
                            }
                            else if ( enumLineSelector.LINE3 == eLineSelect )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSelector", "Line3" );
                            }
                            else if ( enumLineSelector.LINE4 == eLineSelect )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSelector", "Line4" );
                            }
                        }
                    }
                }
                catch ( System.Exception ex )
                {
                    if ( null != m_objCallbackTraceMessage )
                    {
                        m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
                    }
                    break;
                }

                bReturn = true;
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 라인 소스 변경
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetLineSource( enumLineSource eLineSource )
        {
            bool bReturn = false;

            do
            {
                try
                {
                    // 예외 처리
                    if ( false == m_objImageProvider.IsOpen ) break;
                    // 트리거 소스 설정
                    NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "LineSource" );
                    if ( true == objNodeHandle.IsValid )
                    {
                        if ( true == GenApi.NodeIsAvailable( objNodeHandle ) )
                        {
                            if ( enumLineSource.ACQUISITIONTRIGGERREADY == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "AcquisitionTriggerReady" );
                            }
                            else if ( enumLineSource.ACQUISITIONTRIGGERWAIT == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "AcquisitionTriggerWait" );
                            }
                            else if ( enumLineSource.EXPOSUREACTIVE == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "ExposureActive" );
                            }
                            else if ( enumLineSource.FLASHWINDOW == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "FlashWindow" );
                            }
                            else if ( enumLineSource.FRAMECYCLE == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "FrameCycle" );
                            }
                            else if ( enumLineSource.FRAMETRIGGERWAIT == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "FrameTriggerWait" );
                            }
                            else if ( enumLineSource.FREQUENCYCONVERTER == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "FrequencyConverter" );
                            }
                            else if ( enumLineSource.LINETRIGGERWAIT == eLineSource )
                            {
                                Pylon.DeviceFeatureFromString( m_objImageProvider.GetDeviceHandle(), "LineSource", "LineTriggerWait" );
                            }
                        }
                    }
                }
                catch ( System.Exception ex )
                {
                    if ( null != m_objCallbackTraceMessage )
                    {
                        m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
                    }
                    break;
                }

                bReturn = true;
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : StartSingleGrab
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLStartSingleGrab()
		{
			bool bReturn = false;

			do {
				m_objImageProvider.OneShot();

				bReturn = true;
			} while( false );

			return bReturn;
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : StartSingleGrab
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLStartMultiGrab()
        {
            bool bReturn = false;

            do
            {
                var pDocument = DeepSight.CDocument.GetDocument;
                if( false == pDocument.m_bImageInspectionMode ) {
                    // 멀티그랩 인덱스를 넘겨주기때문에 찍기전 초기화
                    m_iMultiGrabIndex = 0;
                    m_objImageProvider.MultiShot();
                } else {
                    ImageFileMultiGrab();
                }
                

                bReturn = true;
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : SetContinousGrab
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetContinousGrab()
		{
			bool bReturn = false;

			do {
				m_objImageProvider.ContinuousShot();

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : SetSingleGrab
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetSingleGrab()
		{
			bool bReturn = false;

			do {
				m_objImageProvider.Stop();


				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 소프트웨어 트리거
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool HLSetSoftwareTrigger()
		{
			bool bReturn = false;

			do {
				try {
					// 예외 처리
					if( false == m_objImageProvider.IsOpen ) break;
					// 소프트웨어 트리거
					NODE_HANDLE objNodeHandle = m_objImageProvider.GetNodeFromDevice( "TriggerSoftware" );
					if( true == objNodeHandle.IsValid ) {
						if( true == GenApi.NodeIsAvailable( objNodeHandle ) ) {
							GenApi.CommandExecute( objNodeHandle );
						}
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
				}

				bReturn = true;
			} while( false );

			return bReturn;
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
				/* Before using any pylon methods, the pylon runtime must be initialized. */
				Pylon.Initialize();
				/* Enumerate all camera devices. You must call
                PylonEnumerateDevices() before creating a device. */
				uint iDeviceCount = Pylon.EnumerateDevices();
				// 디바이스 없으면 빠져나감
				if( 0 >= iDeviceCount ) break;

				try {
					for( uint iLoopCount = 0; iLoopCount < iDeviceCount; iLoopCount++ ) {
                        if( CInitializeParameter.enumUseCameraType.USE_REAL_CAMERA_TYPE == m_objInitializeParameter.eUseCameraType ) {
                            // 디바이스 정보 핸들 얻어옴
                            PYLON_DEVICE_INFO_HANDLE hDeviceInfoHandle = Pylon.GetDeviceInfoHandle( iLoopCount );
                            // 해당 디바이스에 시리얼 넘버 받아옴
                            string strSerial = Pylon.DeviceInfoGetPropertyValueByName( hDeviceInfoHandle, Pylon.cPylonDeviceInfoSerialNumberKey );
                            
                            // 시리얼 넘버 비교해서 맞는 곳만 들어감
                            if( strSerial == m_objInitializeParameter.strCameraSerialNumber ) {
                                // 이미지 제공자 오픈 상태 아니면 오픈해줌 (내부에서 디바이스 오픈까지 다 함)
                                if( true == Pylon.IsDeviceAccessible( iLoopCount, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream ) ) {
                                    if( false == m_objImageProvider.IsOpen ) {
                                        m_objImageProvider.Open( iLoopCount );
                                        // 카메라 설정 셋팅
                                        HLSetCameraConfig( m_objInitializeParameter.objCameraConfig );
                                    }
                                } else {
                                    break;
                                }

                                bReturn = true;
                                Thread.Sleep( 100 );
                                break;
                            }
                        } else {
                            bReturn = true;
                        }
					}
				}
				catch( System.Exception ex ) {
					if( null != m_objCallbackTraceMessage ) {
						m_objCallbackTraceMessage( ex.Message + "->" + ex.StackTrace );
					}
					break;
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
			m_objImageProvider.Close();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnGrabErrorEventCallback( Exception grabException, string additionalErrorMessage )
		{
			if( null != m_objCallback ) {
				CImageData objData = new CImageData();
				objData.bGrabComplete = false;
				m_objCallback( objData );
			}
			if( null != m_objCallbackTraceMessage ) {
				m_objCallbackTraceMessage( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Grab Error : " + grabException.Message );
			}
			if( null != m_objCallbackGrabError ) {
				m_objCallbackGrabError();
			}
			//Trace.WriteLine( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Grab Error" );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnDeviceOpenedEventCallback()
		{
			if( null != m_objCallbackTraceMessage ) {
				m_objCallbackTraceMessage( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Opened Complete" );
			}
			//Trace.WriteLine( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Opened Complete" );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnDeviceClosedEventCallback()
		{
			if( null != m_objCallbackTraceMessage ) {
				m_objCallbackTraceMessage( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Closed Complete" );
			}
			//Trace.WriteLine( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Closed Complete" );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnDeviceClosingEventCallback()
		{
			if( null != m_objCallbackTraceMessage ) {
				m_objCallbackTraceMessage( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Closing Complete" );
			}
			//Trace.WriteLine( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Closing Complete" );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnDeviceRemovedEventCallback()
		{
			m_objImageProvider.Close();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnGrabbingStartedEventCallback()
		{
			if( null != m_objCallbackTraceMessage ) {
				m_objCallbackTraceMessage( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Started Complete" );
			}
			//Trace.WriteLine( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Started Complete" );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//수정 : 
		//목적 : 
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void OnGrabbingStoppedEventCallback()
		{
			if( null != m_objCallbackTraceMessage ) {
				m_objCallbackTraceMessage( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Stopped Complete" + " ->" + m_objImageProvider.GetLastErrorMessage() );
			}
			//Trace.WriteLine( "Camera " + m_objInitializeParameter.iIndex.ToString() + " Stopped Complete" + " ->" + m_objImageProvider.GetLastErrorMessage() );
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
				CDeviceCameraImageProviderBasler.Image objImage = m_objImageProvider.GetCurrentImage();

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


					// Y축 리버스
					if( true == m_bReverseY ) {
						m_objBitImage.RotateFlip( RotateFlipType.RotateNoneFlipY );
					}
					// 90도 이미지 회전 유무
					if( true == m_bRotation90 ) {
						m_objBitImage.RotateFlip( RotateFlipType.Rotate90FlipNone );
					}
					// 180도 이미지 회전 유무
					if( true == m_bRotation180 ) {
						m_objBitImage.RotateFlip( RotateFlipType.Rotate180FlipNone );
					}
					// 270도 이미지 회전 유무
					if( true == m_bRotation270 ) {
						m_objBitImage.RotateFlip( RotateFlipType.Rotate270FlipNone );
					}

					if( null != m_objCallback ) {
						CImageData objData = new CImageData();
						objData.bGrabComplete = true;
                        objData.bitmapImage = m_objBitImage;// ( Bitmap )m_objBitImage.Clone();
						objData.iImageWidth = m_objBitImage.Width;
						objData.iImageHeight = m_objBitImage.Height;
                        objData.iMultiGrabImageIndex = m_iMultiGrabIndex;
						m_objCallback( objData );
					}
                    m_iMultiGrabIndex++;

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
			m_objImageProvider.ReleaseImage();
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 이미지로딩 검사
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetImageFile( string strPath )
        {
            bool bReturn = false;
            do {
                var pDocument = DeepSight.CDocument.GetDocument;

                try {
                    m_lstImageData.Clear();
                    m_iMultiGrabIndexImageFile = 0;
                    // 프로젝트 bin 폴더
                    // 수정 - 타입 & 카메라별 시뮬레이션 이미지 폴더 따로 관리
                    string strImagePath = strPath;
                    // 폴더 내 파일 목록 불러오기
                    string[] arFileNames = System.IO.Directory.GetFiles( strImagePath + "\\" );
                    // 폴더 내의 파일 수만큼 루프
                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        if( -1 != arFileNames[ iLoopCount ].IndexOf( "Origin_VIDI" ) ) {
                            CImageData objImageData = new CImageData();
                            objImageData.bitmapImage = new Bitmap( arFileNames[ iLoopCount ] );
                            objImageData.bGrabComplete = true;
                            m_lstImageData.Add( objImageData );
                        }
                    }

                    arFileNames = System.IO.Directory.GetFiles( strImagePath + "\\" );
                    // 폴더 내의 파일 수만큼 루프
                    for( int iLoopCount = 0; iLoopCount < arFileNames.Length; iLoopCount++ ) {
                        if( -1 != arFileNames[ iLoopCount ].IndexOf( "Origin_Measure" ) ) {
                            CImageData objImageData = new CImageData();
                            objImageData.bitmapImage = new Bitmap( arFileNames[ iLoopCount ] );
                            objImageData.bGrabComplete = true;
                            m_lstImageData.Add( objImageData );
                        }
                    }

                } catch( System.Exception ex ) {
                    System.Diagnostics.Trace.WriteLine( ex.ToString() );
                    break;
                }
                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : StartSingleGrab
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool ImageFileMultiGrab()
        {
            bool bReturn = false;

            do {

                if( null != m_objCallback ) {
                    if( 0 == m_lstImageData.Count ) break;

                    for( int iLoopCount = 0; iLoopCount < m_iMaxCountMultiGrab; iLoopCount++ ) {
                        CImageData objImageData = m_lstImageData[ m_iMultiGrabIndexImageFile ].Clone() as CImageData;
                        objImageData.iImageWidth = objImageData.bitmapImage.Width;
                        objImageData.iImageHeight = objImageData.bitmapImage.Height;
                        objImageData.iMultiGrabImageIndex = iLoopCount;
                        m_objCallback( ( CImageData )objImageData.Clone() );

                        m_iMultiGrabIndexImageFile++;

                        if( m_iMultiGrabIndexImageFile >= m_lstImageData.Count )
                            m_iMultiGrabIndexImageFile = 0;
                    }
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
			CDeviceCameraBaslse pThis = ( CDeviceCameraBaslse )state;

			while( false == pThis.m_bThreadExit ) {
				// 커넥트 변수가 false면 카메라 커넥트 다시 시도
				if( null != pThis.m_objImageProvider ) {
					if( false == pThis.m_objImageProvider.IsOpen ) {
						pThis.SetCameraConnect();
					}
				}

				Thread.Sleep( 1000 );
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