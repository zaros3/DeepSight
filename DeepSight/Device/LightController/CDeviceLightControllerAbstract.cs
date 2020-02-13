﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDevice.Abstract
{
    public abstract class CDeviceLightControllerAbstract
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화 파라미터
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class CInitializeParameter : ICloneable
        {
            // 통신 타입
            public enum enumType { TYPE_SOCKET = 0, TYPE_SERIAL_PORT, TYPE_FINAL };
            // 시리얼포트 Parity
            public enum enumSerialPortParity { PARITY_NONE = 0, PARITY_ODD, PARITY_EVEN, PARITY_MARK, PARITY_SPACE };
            // 시리얼포트 StopBits
            public enum enumSerialPortStopBits { STOP_BITS_NONE = 0, STOP_BITS_ONE, STOP_BITS_TWO, STOP_BITS_ONE_POINT_FIVE };

            public enumType eType;
            public string strSocketIPAddress;
            public int iSocketPortNumber;

          
            public string strSerialPortName;
            public int iSerialPortBaudrate;
            public int iSerialPortDataBits;
            public enumSerialPortParity eParity;
            public enumSerialPortStopBits eStopBits;

            public object Clone()
            {
                CInitializeParameter objInitializeParameter = new CInitializeParameter();
                objInitializeParameter.eType = this.eType;
                objInitializeParameter.strSocketIPAddress = this.strSocketIPAddress;
                objInitializeParameter.iSocketPortNumber = this.iSocketPortNumber;

                objInitializeParameter.strSerialPortName = this.strSerialPortName;
                objInitializeParameter.iSerialPortBaudrate = this.iSerialPortBaudrate;
                objInitializeParameter.iSerialPortDataBits = this.iSerialPortDataBits;
                objInitializeParameter.eParity = this.eParity;
                objInitializeParameter.eStopBits = this.eStopBits;

                return objInitializeParameter;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 알람 발생 확인 클래스
        //설명 : 함수 호출 시 반환 형
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class CLightControllerError : ICloneable
        {
            // 이벤트 발생 시간
            public string strEventTime;
            // 수행된 함수 이름
            public string strFunctionName;
            // 알람 리턴 결과
            public int iReturnCode;
            // 알람 메세지
            public string strMessage;

            public object Clone()
            {
                CLightControllerError objError = new CLightControllerError();
                objError.strEventTime = this.strEventTime;
                objError.strFunctionName = this.strFunctionName;
                objError.iReturnCode = this.iReturnCode;
                objError.strMessage = this.strMessage;

                return objError;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 수신데이터 콜백
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class CReceiveData : ICloneable
        {
            public string strData;
            public byte[] byteReceiveData = new byte[ 4096 ];
            public int iByteLength;

            public void clear()
            {
                strData = "";
                Array.Clear( byteReceiveData, 0, byteReceiveData.Length );
                iByteLength = 0;
            }

            public object Clone()
            {
                CReceiveData objData = new CReceiveData();
                objData.strData = this.strData;
                objData.byteReceiveData = this.byteReceiveData;
                objData.iByteLength = this.iByteLength;
                return objData;
            }
        }

        // 델리게이트 선언
        public delegate void CallBackFuntionReceiveData( CReceiveData objReceiveData );

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화 추상객체
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract bool HLInitialize( CInitializeParameter objInitializeParameter );
                                 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 해제 추상객체
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract void HLDeInitialize();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 버전 추상객체
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract string HLGetVersion();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 접속 상태
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLIsConnected()
        {
            bool bReturn = false;

            do
            {
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 콜백 연결
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void SetCallbackFunction( CallBackFuntionReceiveData objReceiveData )
        {
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetLightIntensity( int iChannel, int iIntensity )
        {
            bool bReturn = false;

            do {


            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetLightIntensity( int[] iIntensity )
        {
            bool bReturn = false;

            do {


            } while( false );

            return bReturn;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetLightOff( int iChannel )
        {
            bool bReturn = false;

            do {

            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetLightOff()
        {
            bool bReturn = false;

            do {

            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 조명 스트로브 펄스 설정
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetPulse( int iChannel, int iPulse )
        {
            bool bReturn = false;

            do
            {
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 고정 채널 모드(선택한 채널의 조명만 켠다
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetChannelHold( int iChannel )
        {
            bool bReturn = false;

            do
            {
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 자동 채널 모드 입력한 채널의 수까지 돌리고 돌리고(ex >> if iChannel == 3 then 1 2 3 1 2 3 1 2 3)
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetAutoIndex( int iChannel )
        {
            bool bReturn = false;

            do
            {
            } while ( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 알람 상태 정보를 리턴한다.
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError HLGetErrorCode()
        {
            HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError objError = new HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError();
            return objError;
        }
    }
}