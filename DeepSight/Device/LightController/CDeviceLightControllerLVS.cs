﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDevice.LightController
{
    public class CDeviceLightControllerLVS : HLDevice.Abstract.CDeviceLightControllerAbstract
    {
        HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError m_objError = new HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError();
        HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVS m_objLightController = new HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVS();

        private CallBackFuntionReceiveData m_objCallback = null;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDeviceLightControllerLVS()
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
            return m_objLightController.HLGetVersion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLInitialize( HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter objInitializeParameter )
        {
            bool bReturn = false;

            do
            {
                HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVSDefine.CInitializeParameter objParameter = new HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVSDefine.CInitializeParameter();
                objParameter.eType = ( HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVSDefine.CInitializeParameter.enumType )objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = ( HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVSDefine.CInitializeParameter.enumSerialPortParity )objInitializeParameter.eParity;
                objParameter.eStopBits = ( HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVSDefine.CInitializeParameter.enumSerialPortStopBits )objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;

                if( false == m_objLightController.HLInitialize( objParameter ) ) 
                {
                    MakeError();
                    break;
                }
                
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
            m_objLightController.HLDeInitialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 접속 상태
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLIsConnected()
        {
            return m_objLightController.HLIsConnected();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 콜백 연결
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void SetCallbackFunction( HLDevice.Abstract.CDeviceLightControllerAbstract.CallBackFuntionReceiveData objReceiveData )
        {
            m_objCallback = objReceiveData;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetLightIntensity( int iChannel, int iIntensity )
        {
            bool bReturn = false;

            do {

                if( false == m_objLightController.HLSetLightIntensity( iChannel, iIntensity ) ) {
                    MakeError();
                    break;
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
        public override bool HLSetLightIntensity( int[] iIntensity )
        {
            bool bReturn = false;

            do {
                if( false == m_objLightController.HLSetLightIntensity( iIntensity ) ) {
                    MakeError();
                    break;
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
        public override bool HLSetLightOff( int iChannel )
        {
            bool bReturn = false;

            do {
                if( false == m_objLightController.HLSetLightOff( iChannel ) ) {
                    MakeError();
                    break;
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
        public override bool HLSetLightOff()
        {
            bool bReturn = false;

            do {
                if( false == m_objLightController.HLSetLightOff() ) {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 조명 스트로브 펄스 설정
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetPulse(int iChannel, int iPulse)
        {
            bool bReturn = false;
            do
            {
                //시작인덱스가 1부터
                iChannel += 1;
                if (false == m_objLightController.HLSetPulse(iChannel, iPulse ))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 고정 채널 모드(선택한 채널의 조명만 켠다
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetChannelHold(int iChannel)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objLightController.HLSetChannelHold(iChannel))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 자동 채널 모드 입력한 채널의 수까지 돌리고 돌리고(ex >> if iChannel == 3 then 1 2 3 1 2 3 1 2 3)
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool HLSetAutoIndex(int iChannel)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objLightController.HLSetAutoIndex(iChannel))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

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
            HLDeviceDLL.LightController.LVS.CDeviceLightControllerLVSDefine.CLVSError objError = m_objLightController.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 알람 상태 정보를 리턴한다.
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError HLGetErrorCode()
        {
            return ( HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError )m_objError.Clone();
        }
        
    }
}