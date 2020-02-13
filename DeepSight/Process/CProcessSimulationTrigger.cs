using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeepSight
{
	public class CProcessSimulationTrigger : CProcessAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CProcessSimulationTrigger()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소멸자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		~CProcessSimulationTrigger()
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
				// 스레드
				m_ThreadProcess = new Thread( ThreadProcess );
				m_ThreadProcess.Start( this );

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
			m_bThreadExit = true;
			m_ThreadProcess.Join();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소프트웨어 트리거 테스트 & 부하 테스트
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static void ThreadProcess( object state )
		{
			CProcessSimulationTrigger pThis = ( CProcessSimulationTrigger )state;
			//var pDocument = CDocument.GetDocument;
			//// 택타임 5초
			//// 제품 감지 ON - 1 sec - 제품 감지 OFF - 4 sec 사이클 반복
			//int iGlassDetectOnDelay = 1000;
			//int iGlassDetectOffDelay = 4000;
			int iThreadSleepTime = 100;
			//Stopwatch objStopwatch = new Stopwatch();
			
			while( false == pThis.m_bThreadExit ) {
				// 시뮬레이션이 아닌 경우
				//if( CDefine.enumSimulationMode.SIMULATION_ON != pDocument.m_objConfig.GetSystemParameter().eSimulationMode ) {
				//	Thread.Sleep( iGlassDetectOffDelay );
				//	continue;
				//}
				//// 설비 정지인 경우
				//if( CDefine.enumRunMode.START != pDocument.GetRunMode() ) {
				//	// 타이머 도는 중인 경우
				//	if( true == objStopwatch.IsRunning ) {
				//		// IO OFF
				//		pDocument.m_objProcessMain.m_objIO.HLSetDigitalBit( "LEFT_VISION_START", false );
				//		pDocument.m_objProcessMain.m_objIO.HLSetDigitalBit( "RIGHT_VISION_START", false );
				//		// 타이머 OFF
				//		objStopwatch.Stop();
				//		objStopwatch.Reset();
				//	}
				//	Thread.Sleep( iGlassDetectOffDelay );
				//	continue;
				//}
				//// 설비 시작인 경우
				//else if( CDefine.enumRunMode.START == pDocument.GetRunMode() ) {
				//	// 타이머 정지인 경우
				//	if( false == objStopwatch.IsRunning ) {
				//		// 타이머 ON
				//		objStopwatch.Start();
				//	}
				//}

				//bool bGlassDetect = false;
				//pDocument.m_objProcessMain.m_objIO.HLGetDigitalBit( "LEFT_VISION_START", ref bGlassDetect );
				//// IO OFF이면 OFF 시간 확인
				//if( false == bGlassDetect ) {
				//	if( iGlassDetectOffDelay <= objStopwatch.ElapsedMilliseconds ) {
				//		// IO ON
				//		pDocument.m_objProcessMain.m_objIO.HLSetDigitalBit( "LEFT_VISION_START", true );
				//		pDocument.m_objProcessMain.m_objIO.HLSetDigitalBit( "RIGHT_VISION_START", true );
				//		// 리셋
				//		objStopwatch.Stop();
				//		objStopwatch.Reset();
				//		// 타이머 ON
				//		objStopwatch.Start();
				//	}
				//}
				//// IO ON이면 ON 시간 확인
				//else {
				//	if( iGlassDetectOnDelay <= objStopwatch.ElapsedMilliseconds ) {
				//		// IO OFF
				//		pDocument.m_objProcessMain.m_objIO.HLSetDigitalBit( "LEFT_VISION_START", false );
				//		pDocument.m_objProcessMain.m_objIO.HLSetDigitalBit( "RIGHT_VISION_START", false );
				//		// 리셋
				//		objStopwatch.Stop();
				//		objStopwatch.Reset();
				//		// 타이머 ON
				//		objStopwatch.Start();
				//	}	
				//}
				
				Thread.Sleep( iThreadSleepTime );
			}
		}
	}
}