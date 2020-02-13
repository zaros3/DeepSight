using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
	public class CProcessVision : CProcessAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 설비 타입별 매니저 구성
		public CProcessVisionManagerProcess60 m_objProcessVisionManagerProcess60;
        public CProcessVisionManagerProcess110 m_objProcessVisionManagerProcess110;
        public CProcessVisionManagerProcess150 m_objProcessVisionManagerProcess150;
        public CProcessVisionManagerProcess150Gocator m_objProcessVisionManagerProcess150Gocator;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CProcessVision()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소멸자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		~CProcessVision()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool Initialize()
		{
			bool bReturn = false;

			do {
				var pDocument = CDocument.GetDocument;
                pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessVision Initialize Start" );

                pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessVision - Manager Initialize" );

                if( CDefine.enumMachineType.PROCESS_60 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
					// 프로그래스 바 : CProcessVisionManagerLoader Initialize Start.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint(), "CProcessVisionManagerProcess60 Initialize Start.", TypeOfMessage.Warning );
                    m_objProcessVisionManagerProcess60 = new CProcessVisionManagerProcess60();
					if( false == m_objProcessVisionManagerProcess60.Initialize() ) {
						pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessVisionManagerProcess60 Initialize Fail");
						break;
					}
					// 프로그래스 바 : CProcessVisionManagerLoader Initialize Completed.
					CLoadingScreen.UpdateStatusTextWithStatus( CLoadingScreen.GetPrograssPoint() + 3, "CProcessVisionManagerLoader Initialize Completed.", TypeOfMessage.Success );
				}
				else if( CDefine.enumMachineType.PROCESS_110 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                    // 프로그래스 바 : CProcessVisionManagerLoader Initialize Start.
                    CLoadingScreen.UpdateStatusTextWithStatus(CLoadingScreen.GetPrograssPoint(), "CProcessVisionManagerProcess110 Initialize Start.", TypeOfMessage.Warning);
                    m_objProcessVisionManagerProcess110 = new CProcessVisionManagerProcess110();
                    if (false == m_objProcessVisionManagerProcess110.Initialize())
                    {
                        pDocument.SetUpdateLog(CDefine.enumLogType.LOG_SYSTEM, "CProcessVisionManagerProcess110 Initialize Fail");
                        break;
                    }
                    // 프로그래스 바 : CProcessVisionManagerLoader Initialize Completed.
                    CLoadingScreen.UpdateStatusTextWithStatus(CLoadingScreen.GetPrograssPoint() + 3, "CProcessVisionManagerLoader Initialize Completed.", TypeOfMessage.Success);
                }
				else if( CDefine.enumMachineType.PROCESS_150 == pDocument.m_objConfig.GetSystemParameter().eMachineType ) {
                    var objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                    // 프로그래스 바 : CProcessVisionManagerLoader Initialize Start.
                    CLoadingScreen.UpdateStatusTextWithStatus(CLoadingScreen.GetPrograssPoint(), "CProcessVisionManagerProcess150 Initialize Start.", TypeOfMessage.Warning);

                    if( CDefine.enumCameraType.CAMERA_3D != objSystemParameter.eCameraType ) {
                        m_objProcessVisionManagerProcess150 = new CProcessVisionManagerProcess150();
                        if( false == m_objProcessVisionManagerProcess150.Initialize() ) {
                            pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessVisionManagerProcess150 Initialize Fail" );
                            break;
                        }
                    } else {
                        m_objProcessVisionManagerProcess150Gocator = new CProcessVisionManagerProcess150Gocator();
                        if( false == m_objProcessVisionManagerProcess150Gocator.Initialize() ) {
                            pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessVisionManagerProcess150Gocator Initialize Fail" );
                            break;
                        }
                    }

                    
                    // 프로그래스 바 : CProcessVisionManagerLoader Initialize Completed.
                    CLoadingScreen.UpdateStatusTextWithStatus(CLoadingScreen.GetPrograssPoint() + 3, "CProcessVisionManagerLoader Initialize Completed.", TypeOfMessage.Success);
                }

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 해제
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override void DeInitialize()
		{
			if( null != m_objProcessVisionManagerProcess60) {
                m_objProcessVisionManagerProcess60.DeInitialize();
			}
            if (null != m_objProcessVisionManagerProcess110)
            {
                m_objProcessVisionManagerProcess110.DeInitialize();
            }
            if (null != m_objProcessVisionManagerProcess150)
            {
                m_objProcessVisionManagerProcess150.DeInitialize();
            }
            if( null != m_objProcessVisionManagerProcess150Gocator ) {
                m_objProcessVisionManagerProcess150Gocator.DeInitialize();
            }
        }
    }
}