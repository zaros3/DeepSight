﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDevice
{
    public class CDeviceLightController
    {
        HLDevice.Abstract.CDeviceLightControllerAbstract m_objLightController;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CDeviceLightController( HLDevice.Abstract.CDeviceLightControllerAbstract objDeviceAbstractLightController )
        {
            m_objLightController = objDeviceAbstractLightController;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 초기화
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLInitialize( HLDevice.Abstract.CDeviceLightControllerAbstract.CInitializeParameter objInitializeParameter )
        {
            return m_objLightController.HLInitialize( objInitializeParameter );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 해제
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void HLDeInitialize()
        {
            m_objLightController.HLDeInitialize();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 버전 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string HLGetVersion()
        {
            return m_objLightController.HLGetVersion();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 버전 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLIsConnected()
        {
            return m_objLightController.HLIsConnected();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLSetLightIntensity( int iChannel, int iIntensity )
        {
            return m_objLightController.HLSetLightIntensity( iChannel, iIntensity );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLSetLightIntensity( int[] iIntensity )
        {
            return m_objLightController.HLSetLightIntensity( iIntensity );
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLSetLightOff( int iChannel )
        {
            return m_objLightController.HLSetLightOff( iChannel );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool HLSetLightOff()
        {
            return m_objLightController.HLSetLightOff();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 조명 스트로브 펄스 설정
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetPulse( int iChannel, int iPulse )
        {
            return m_objLightController.HLSetPulse( iChannel, iPulse );
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 고정 채널 모드(선택한 채널의 조명만 켠다
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetChannelHold( int iChannel )
        {
            return m_objLightController.HLSetChannelHold( iChannel );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //수정 : 
        //목적 : 자동 채널 모드 입력한 채널의 수까지 돌리고 돌리고(ex >> if iChannel == 3 then 1 2 3 1 2 3 1 2 3)
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool HLSetAutoIndex( int iChannel )
        {
            return m_objLightController.HLSetAutoIndex( iChannel );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 현재 알람 상태 정보를 리턴한다.
        //설명 : 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public HLDevice.Abstract.CDeviceLightControllerAbstract.CLightControllerError HLGetErrorCode()
        {
            return m_objLightController.HLGetErrorCode();
        }
    }
}
