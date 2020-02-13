﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
    // 폼에서 무조건 구현해줘야 하는 부분
    public interface CFormInterface
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 모든 폼에서 언어 변경 함수를 묶어서 관리
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool SetChangeLanguage();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 모든 폼에서 타이머 끄고 키고를 제어
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void SetTimer( bool bTimer );

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 모든 폼에서 Visible 상태를 제어
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void SetVisible( bool bVisible );
    }
}