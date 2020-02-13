﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HLDevice;
using HLDevice.Camera;
using System.Drawing;
using System.Diagnostics;

namespace DeepSight
{
	public class CProcessVisionManagerProcess150Gocator : CProcessAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 스레드
		private Thread[] m_ThreadManager;
		// 타입 ( Input Tray )
		private Type m_typeProcessVisionProcess150;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//public property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 비전 얼라인 ( Input Tray * 3 + PreAlign * 1 )
		public CProcessAbstract[] m_objProcessVisionProcess150;
		// 매니저 변수
		public struct structureManagerArgs
		{
			// 자기 객체 참조
			public object pThis;
			// 카메라 인덱스
			public int iCameraIndex;

			public structureManagerArgs( object pthis, int cameraIndex )
			{
				this.pThis = pthis;
				this.iCameraIndex = cameraIndex;
			}
		}
		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CProcessVisionManagerProcess150Gocator()
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

                // 비전 얼라인
                m_objProcessVisionProcess150 = new CProcessAbstract[ ( int )CDefine.enumCamera.CAMERA_FINAL ];
				for( int iLoopCount = ( int )CDefine.enumCamera.CAMERA_1; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
                   var obj = new CProcessVisionProcess150Gocator();
                     
					obj.Initialize( iLoopCount );
                    m_objProcessVisionProcess150[ iLoopCount ] = obj;
                    m_typeProcessVisionProcess150 = obj.GetType();
				}
				
				// PLC 객체 참조
				m_objPLC = pDocument.m_objProcessMain.m_objPLC;

				// 메인 스레드
				m_ThreadManager = new Thread[ ( int )CDefine.enumCamera.CAMERA_FINAL ];
				for( int iLoopCount = 0; iLoopCount < m_ThreadManager.Length; iLoopCount++ ) {
					m_ThreadManager[ iLoopCount ] = new Thread( ThreadManager );
					structureManagerArgs objArgs = new structureManagerArgs( this, iLoopCount );
					m_ThreadManager[ iLoopCount ].Start( objArgs );
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
			// 스레드 종료
			m_bThreadExit = true;
			// 메인 스레드
			for( int iLoopCount = 0; iLoopCount < m_ThreadManager.Length; iLoopCount++ ) {
				m_ThreadManager[ iLoopCount ].Join();
			}
			// 비전 얼라인
			for( int iLoopCount = ( int )CDefine.enumCamera.CAMERA_1; iLoopCount < ( int )CDefine.enumCamera.CAMERA_FINAL; iLoopCount++ ) {
				if( null != m_objProcessVisionProcess150[ iLoopCount ] ) {
                    CProcessVisionProcess150Gocator obj = m_objProcessVisionProcess150[ iLoopCount ] as CProcessVisionProcess150Gocator;
					if( null != obj ) {
						obj.DeInitialize();
					}
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 트리거 감시 ( Input Tray )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool IsInspection( int iCameraIndex )
		{
			bool bReturn = false;
			var pDocument = CDocument.GetDocument;
			//int iBitIndex = 0;
			//bool bStatus = false;

			do {
				// 설비 정지, 메뉴얼 테스트
				if( CDefine.enumRunMode.RUN_MODE_STOP == pDocument.GetRunMode() ) {
					// 트리거 유무
					if( CDefine.enumTrigger.TRIGGER_OFF == pDocument.GetTrigger( iCameraIndex ) ) {
						break;
					}
				}
				// 설비 오토 런 PLC 통신 대기
				else {
                    // PLC로 감시하자
                    // 하기소스는 PLC로 바꾼뒤 삭제예정
                    short[] iReadData = new short[ 1 ];
                    m_objPLC.HLReadWordFromPLC( "PLC_INSPECTION_START", iReadData.Length, ref iReadData );
                    if ( ( int )CDefine.enumTrigger.TRIGGER_OFF == iReadData[ 0 ] ) break;

                    // 트리거 ON
                    pDocument.SetTrigger( iCameraIndex, CDefine.enumTrigger.TRIGGER_ON );
                }

                bReturn = true;
			} while( false );

			return bReturn;
		}

        

      

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 라이브 감시 ( Input Tray )
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool IsLive( int iCameraIndex )
		{
			bool bReturn = false;
			var pDocument = CDocument.GetDocument;

			do {
				// 설비 정지, 메뉴얼 테스트
				if( CDefine.enumRunMode.RUN_MODE_STOP == pDocument.GetRunMode() ) {
					// 라이브 온 유무
					if( CDefine.enumLiveMode.LIVE_MODE_OFF == pDocument.GetLiveMode( iCameraIndex ) ) {
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
        //목적 : CellID 읽기
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string ReadCellID()
        {
            string strID = "";
            // PLC에서 적어주는 길이 10
            //short[] iReadData = new short[10];
            //m_objPLC.HLReadWordFromPLC(CDefine.enumPLCInputIndex.PLC_CELL_ID_1.ToString(), iReadData.Length, ref iReadData);

            //for (int iLoopCount = 0; iLoopCount < iReadData.Length; iLoopCount++)
            //{
            //    byte[] bConvert = BitConverter.GetBytes(iReadData[iLoopCount]);
            //    if (0 != iReadData[iLoopCount])
            //    {
            //        if (BitConverter.IsLittleEndian)
            //        {
            //            Array.Reverse(bConvert);
            //            char c1 = Convert.ToChar(bConvert[1]);
            //            if (c1 == '\0') c1 -= ' ';
            //            char c2 = Convert.ToChar(bConvert[0]);
            //            if (c2 == '\0') c2 -= ' ';
            //            strID = string.Format("{0}{1}{2}", strID, c1, c2);
            //        }
            //        else
            //        {
            //            char c1 = Convert.ToChar(bConvert[0]);
            //            if (c1 == '\0') c1 -= ' ';
            //            char c2 = Convert.ToChar(bConvert[1]);
            //            if (c2 == '\0') c2 -= ' ';
            //            strID = string.Format("{0}{1}{2}", strID, c1, c2);
            //        }
            //    }
            //}

            m_objPLC.HLReadWordASCIIFromPLC(CDefine.enumPLCInputIndex.PLC_CELL_ID_1.ToString(), 10, ref strID);

            return strID;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 레시피 체크
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool DoProcessRecipe( int iPlcRecipeNumber)
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;

            do {
                CConfig.CSystemParameter objSystemParameter = pDocument.m_objConfig.GetSystemParameter();

                // 레시피 자동변경 모드 사용안하거나, 레시피 번호가 같으면 성공
                if( iPlcRecipeNumber == Int32.Parse( objSystemParameter.strRecipeID ) || false == objSystemParameter.bUseAutoRecipeChange ) {
                    // PLC와 레시피번호가 같으면 성공

                    // 현재는 레시피시퀀스가 없음
                    if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_JOB_LOAD_FAILED.ToString(), ( short )CDefine.enumVisionRecipeStatus.RECIPE_COMPLETE ) )
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 ), "[SEND_PLC][FAIL]RECIPE_COMPLETE On" );
                    else
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 ), "[SEND_PLC]RECIPE_COMPLETE On" );
                } else {
                    if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_JOB_LOAD_FAILED.ToString(), ( short )CDefine.enumVisionRecipeStatus.RECIPE_CHANG_FAIL ) )
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 ), "[SEND_PLC][FAIL]RECIPE_COMPLETE On" );
                    else
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 ), "[SEND_PLC]RECIPE_COMPLETE On" );
                }

                bReturn = true;
            } while( false );

            return bReturn;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 검사 시작 
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool DoProcessInspection( int iCameraIndex )
		{
			bool bReturn = false;
			var pDocument = CDocument.GetDocument;
            Stopwatch objTact = new Stopwatch();

            Stopwatch objStopwatch = new Stopwatch();

            string strPlcCameraName = string.Format( "CAMERA_{0}", iCameraIndex + 1 );
            
            pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "DoProcessInspection" );
            
            //PLC객체
            HLDevice.CDevicePLC objPLC = pDocument.m_objProcessMain.m_objPLC;
            // 결과구조체
            CInspectionResult.CResult objResult = new CInspectionResult.CResult();
            CConfig.CRecipeParameter objRecipeParameter = pDocument.m_objConfig.GetRecipeParameter( iCameraIndex );
            int iAlarmCode = ( int )CDefine.enumVisionAlarmType.NONE;

            do {
                // 택타임 측정
                objTact.Start();

                // 셀ID를 읽음
                string strCellID = ReadCellID();
                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Read Cell ID : " + strCellID );

               // string strCellID = "";
                // PLC 데이터 읽기
                short[] iReadData = new short[ ( int )CDefine.enumPLCInputIndex.PLC_FINAL ];
                m_objPLC.HLReadWordFromPLC( CDefine.enumPLCInputIndex.PLC_MODEL_CHANGE.ToString(), iReadData.Length, ref iReadData );

                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Read Recipe ID : " + iReadData[ ( int )CDefine.enumPLCInputIndex.PLC_MODEL_CHANGE ].ToString() );
                // 레시피 체크
                if ( false == DoProcessRecipe( iReadData[ ( int )CDefine.enumPLCInputIndex.PLC_MODEL_CHANGE ] ) )
                {
                    break;
                }

                //검사 위치 Read
                int iCurrentInspectionIndex = iReadData[ ( int )CDefine.enumPLCInputIndex.PLC_INSPECTION_INDEX ]-1;
                
                //읽어온 검사 위치가 설정 수 보다 큰 경우 Error 후 종료
                if( iCurrentInspectionIndex > objRecipeParameter.iCountInspectionPosition ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "INSPECTION POSITION OVER" );
                    iAlarmCode = ( int )CDefine.enumVisionAlarmType.INSPECTION_POSITION_OVER_RANGE;
                    break;
                }

                // 티치모드이면 저장되어있는 포지션을 바로 사용하자
                CConfig.CSystemParameter objSystemParameter = pDocument.m_objConfig.GetSystemParameter();
                if (true == objSystemParameter.bVidiTeachMode)
                {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Teach Mode" );
                    iCurrentInspectionIndex = pDocument.GetInspectionIndex();
                }

                if (0 > iCurrentInspectionIndex) iCurrentInspectionIndex = 0;
                // 검사인덱스를 가지고 있고
                pDocument.SetInspectionIndex( iCurrentInspectionIndex );

                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Inspection Index : " + ( iCurrentInspectionIndex + 1 ).ToString() );

                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Grab Start" );
                // 그랩
                objStopwatch.Start();
                if ( false == DoProcessTrigger( iCameraIndex ) ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Grab Fail" );
                    iAlarmCode = ( int )CDefine.enumVisionAlarmType.GRAB_FAIL;

                    // Busy Off
                    if (false == m_objPLC.HLWriteWordFromPLC(CDefine.enumPCOutIndex.PC_BUSY.ToString(), (short)CDefine.enumVisionStatus.STATUS_IDLE))
                        pDocument.SetUpdateLog((CDefine.enumLogType)((int)CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex), "[SEND_PLC][FAIL]Vision_Status_Busy Off");
                    else
                        pDocument.SetUpdateLog((CDefine.enumLogType)((int)CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex), "[SEND_PLC]Vision_Status_Busy Off");

                    break;
                }
                objStopwatch.Stop();
                CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Grab Sequence TactTime : " + objStopwatch.ElapsedMilliseconds.ToString() );

                // Busy Off
                if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_BUSY.ToString(), ( short )CDefine.enumVisionStatus.STATUS_IDLE ) )
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]Vision_Status_Busy Off" );
                else
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]Vision_Status_Busy Off" );

                // 이미지 만들기
                objStopwatch.Restart();
                CProcessVisionProcess150Gocator obj = m_objProcessVisionProcess150[ iCameraIndex ] as CProcessVisionProcess150Gocator;
                obj.SetCommand( CProcessVisionProcess150Gocator.enumCommand.CMD_PROCESS_IMAGE_DATA );
                
                // Busy Off
                int iTimeOut = 25000;
                int iTimePeriod = 3;
                while( CProcessVisionProcess150Gocator.enumCommand.CMD_IDLE != obj.GetCommand() && iTimeOut > 0 ) {
                    iTimeOut -= iTimePeriod;
                    Thread.Sleep( iTimePeriod );
                }
                objStopwatch.Stop();
                CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Image Data Process Sequence TactTime : " + objStopwatch.ElapsedMilliseconds.ToString() );
                if( 0 >= iTimeOut ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Image Data Process  Time Out" );
                    iAlarmCode = ( int )CDefine.enumVisionAlarmType.PMS_IMAGE_SETTING_FAIL;
                    break;
                }

                if( CProcessVisionProcess150Gocator.enumStatus.STS_ERROR == obj.GetStatus() ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Image Data Process  NG" );
                    iAlarmCode = ( int )CDefine.enumVisionAlarmType.PMS_IMAGE_SETTING_FAIL;
                    break;
                }
                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Set Data Process" );

                objStopwatch.Restart();

                //////////////////////////////////////////////////
                // 이미지 획득 완료 시 검사 진행
                objResult.objResultCommon.iInspectionPosition = iCurrentInspectionIndex;
                //Vidi Start
                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Inspection Start" );
 				objStopwatch.Restart();
                obj.SetCommand( CProcessVisionProcess150Gocator.enumCommand.CMD_START_INSPECTION );
                iTimeOut = 25000;
                iTimePeriod = 3;
                while( CProcessVisionProcess150Gocator.enumCommand.CMD_IDLE != obj.GetCommand() && iTimeOut > 0 ) {
                    iTimeOut -= iTimePeriod;
                    Thread.Sleep( iTimePeriod );
                }
                objStopwatch.Stop();
                CDocument.GetDocument.SetUpdateLog( CDefine.enumLogType.LOG_ETC, "Inspection Sequence TactTime : " + objStopwatch.ElapsedMilliseconds.ToString() );

                if( 0 >= iTimeOut ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Inspection Time Out" );
                    objResult = pDocument.GetInspectionResultAlign( iCurrentInspectionIndex );
                    objResult.objResultCommon.strCellID = strCellID;
                    objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_NG;
                    objResult.objResultCommon.iInspectionPosition = iCurrentInspectionIndex;
                    iAlarmCode = ( int )CDefine.enumVisionAlarmType.VIDI_INSPECTION_FAIL;
                    break;
                }
                if( CProcessVisionProcess150Gocator.enumStatus.STS_ERROR == obj.GetStatus() ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_PROCESS_CAMERA_0 + iCameraIndex ), "Inspection Run NG" );
                    objResult = pDocument.GetInspectionResultAlign( iCurrentInspectionIndex );
                    objResult.objResultCommon.strCellID = strCellID;
                    objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_NG;
                    objResult.objResultCommon.iInspectionPosition = iCurrentInspectionIndex;
                    iAlarmCode = ( int )CDefine.enumVisionAlarmType.VIDI_INSPECTION_FAIL;
                    break;
                }

                // 여기서 결과 도출
                objResult = pDocument.GetInspectionResultAlign( iCurrentInspectionIndex );
                objResult.objResultCommon.strCellID = strCellID;


                // 최종결과는 양품으로 설정하고, 1개라도 NG가 나오면 NG로 바꾸자
                objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_OK;
                // 여기에 결과를 채우자, 일단은 1,0으로 채움
                // 1포지션에 검사결과가 5개씩 있다고 가정
                 objResult.objResultCommon.iInspectionPosition = iCurrentInspectionIndex;

                 short[] iResult = new short[objResult.objResultCommon.iVidiResultCount];// ( int )CDefine.enumResultIndex.RESULT_FINAL ];
                 for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.iVidiResultCount; iLoopCount++ )
                 {
                     // 일단 1보다 작으면 양품으로 테스트하자
                     if ( objRecipeParameter.objInspectionParameter[ iCurrentInspectionIndex ].dVidiScore < objResult.objResultCommon.objVidiScore[ iLoopCount ] ) {
                         objResult.objResultCommon.objVidiResult.Add( CDefine.enumResult.RESULT_OK );
                         iResult[ iLoopCount ] = 1;
                     }
                     else {
                         objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_NG;
                         objResult.objResultCommon.objVidiResult.Add( CDefine.enumResult.RESULT_NG );
                         iAlarmCode = ( int )CDefine.enumVisionAlarmType.VIDI_INSPECTION_FAIL;
                     }
                     string strResult = string.Format( "INDEX : {0}, VIDI_Score : {1:F2}, VIDI_Result : {2}", iLoopCount + 1, objResult.objResultCommon.objVidiScore[ iLoopCount ], CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objVidiResult[ iLoopCount ] ? "OK" : "NG" );
                     pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), strResult );
                 }

                for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.iVidiResultCount; iLoopCount++ ) {
                    // 일단 1보다 작으면 양품으로 테스트하자
                    if( 0 >= objResult.objResultCommon.obj3DListHeightOverBlobCountHigh[ iLoopCount ] && 0 >= objResult.objResultCommon.obj3DListHeightOverBlobCountLow[ iLoopCount ]  ) { 
                        objResult.objResultCommon.objMeasureResult.Add( CDefine.enumResult.RESULT_OK );
                        iResult[ iLoopCount ] = 1;
                    } else {
                        objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_NG;
                        objResult.objResultCommon.objMeasureResult.Add( CDefine.enumResult.RESULT_NG );
                        iAlarmCode = ( int )CDefine.enumVisionAlarmType.HEIGHT_OVER;
                    }

                    string strResult = string.Format( ",POSITION: {0}, REGION: {1}, HEIGHT_MAX_OVER_REGION_COUNT : {2}, HEIGHT_MIN_OVER_REGION_COUNT : {3}, HEIGHT_RESULT : {4}", iCurrentInspectionIndex + 1, iLoopCount + 1, objResult.objResultCommon.obj3DListHeightOverBlobCountHigh[ iLoopCount ], objResult.objResultCommon.obj3DListHeightOverBlobCountLow[ iLoopCount ], CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult[ iLoopCount ] ? "OK" : "NG" );
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), strResult );
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_RESULT_MEASURE_3D_HEIGHT ), strResult, false );
                }

                for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.iVidiResultCount; iLoopCount++ ) {
                    // 일단 1보다 작으면 양품으로 테스트하자
                    if( objRecipeParameter.objInspectionParameter[ iCurrentInspectionIndex ].d3DWeldWidthMax > objResult.objResultCommon.obj3DListWeldWidth[ iLoopCount ] && objRecipeParameter.objInspectionParameter[ iCurrentInspectionIndex ].d3DWeldWidthMin < objResult.objResultCommon.obj3DListWeldWidth[ iLoopCount ] ) {
                        objResult.objResultCommon.objMeasureResult3DWidth.Add( CDefine.enumResult.RESULT_OK );
                        iResult[ iLoopCount ] = 1;
                    } else {
                        objResult.objResultCommon.eResult = CDefine.enumResult.RESULT_NG;
                        objResult.objResultCommon.objMeasureResult3DWidth.Add( CDefine.enumResult.RESULT_NG );
                        iAlarmCode = ( int )CDefine.enumVisionAlarmType.WELD_WIDTH_OVER;
                    }
                    string strResult = string.Format( ",POSITION: {0}, REGION: {1} , WELD WIDTH : {2:F3}, WELD_WIDTH_RESULT : {3}", iCurrentInspectionIndex + 1, iLoopCount + 1, objResult.objResultCommon.obj3DListWeldWidth[ iLoopCount ], CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult3DWidth[ iLoopCount ] ? "OK" : "NG" );
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), strResult );
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_RESULT_MEASURE_3D_WELD_WIDTH  ), strResult, false );
                }


                for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.iVidiResultCount; iLoopCount++ ) {
                    // 일단 1보다 작으면 양품으로 테스트하자
                    string strResult = string.Format( ",POSITION : {0}, REGION : {1}, SAMPLE HEIGHT DATA, ", iCurrentInspectionIndex + 1, iLoopCount + 1 );
                    for( int iLoopSample = 0; iLoopSample < objResult.objResultCommon.obj3DListSampleHeightData[ iLoopCount ].Count; iLoopSample++ ) {
                        strResult += ( string.Format( "{0:F3},", objResult.objResultCommon.obj3DListSampleHeightData[ iLoopCount ][ iLoopSample ] ) );
                    }
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_MEASURE_3D_HEIGHT_SAMPLE ), strResult, false );
                }

                //String strPLCAddressName = string.Format( "PC_INSPECTION_RESULT_{0}", iCurrentInspectionIndex * ( int )CDefine.enumResultIndex.RESULT_FINAL );
                //String strPLCAddressName = string.Format(CDefine.enumPCOutIndex.PC_INSPECTION_RESULT_1.ToString() );

                //if ( false == m_objPLC.HLWriteWordFromPLC( strPLCAddressName, iResult.Length, iResult ) )
                //    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC RESULT][FAIL]"+ strPLCAddressName  );
                //else
                //    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC RESULT]" + strPLCAddressName );



                bReturn = true;
			} while( false );
            bool bInspectResult = false;
            // 에러코드
            if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_INSPECTION_ALARM_CODE.ToString(), ( short )iAlarmCode ) )
                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]Set AlarmCode" );
            else
                pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]Set AlarmCode" );

            if( false == bReturn ) {
                if( true == pDocument.m_objConfig.GetSystemParameter().bPassMode ) {
                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "VISION PASS MODE" );

                    if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_INSPECTION_OK.ToString(), ( short )CDefine.enumComplete.COMPLETE_ON ) )
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]PC_INSPECTION_OK On" );
                    else
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]PC_INSPECTION_OK On" );

                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), "VISION PASS MODE : FINAL_RESULT_OK" );
                    bInspectResult = true;
                } else {
                    if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_INSPECTION_NG.ToString(), ( short )CDefine.enumComplete.COMPLETE_ON ) )
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]PC_INSPECTION_NG On" );
                    else
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]PC_INSPECTION_NG On" );

                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), "FINAL_RESULT_NG" );
                    bInspectResult = false;
                }
            } else {
                if( CDefine.enumResult.RESULT_OK == objResult.objResultCommon.eResult ) {
                    if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_INSPECTION_OK.ToString(), ( short )CDefine.enumComplete.COMPLETE_ON ) )
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]PC_INSPECTION_OK On" );
                    else
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]PC_INSPECTION_OK On" );

                    pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), "FINAL_RESULT_OK" );
                    bInspectResult = true;
                } else {
                    if( true == pDocument.m_objConfig.GetSystemParameter().bPassMode ) {
                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "VISION PASS MODE" );

                        if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_INSPECTION_OK.ToString(), ( short )CDefine.enumComplete.COMPLETE_ON ) )
                            pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]PC_INSPECTION_OK On" );
                        else
                            pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]PC_INSPECTION_OK On" );

                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), "VISION PASS MODE : FINAL_RESULT_OK" );
                        bInspectResult = true;
                    } else {
                        if( false == m_objPLC.HLWriteWordFromPLC( CDefine.enumPCOutIndex.PC_INSPECTION_NG.ToString(), ( short )CDefine.enumComplete.COMPLETE_ON ) )
                            pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC][FAIL]PC_INSPECTION_NG On" );
                        else
                            pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex ), "[SEND_PLC]PC_INSPECTION_NG On" );

                        pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_RESULT_STAGE_0 + iCameraIndex ), "FINAL_RESULT_NG" );
                        bInspectResult = false;
                    }
                }
            }
            pDocument.SetDisplayResult( pDocument.GetInspectionIndex(), bInspectResult );

            if (false == m_objPLC.HLWriteWordFromPLC(CDefine.enumPCOutIndex.PC_COMPLETE.ToString(), (short)CDefine.enumComplete.COMPLETE_ON))
                pDocument.SetUpdateLog((CDefine.enumLogType)((int)CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex), "[SEND_PLC][FAIL]PC_COMPLETE On");
            else
                pDocument.SetUpdateLog((CDefine.enumLogType)((int)CDefine.enumLogType.LOG_VISION_INTERFACE_CAMERA_0 + iCameraIndex), "[SEND_PLC]PC_COMPLETE On");

            // 트리거 OFF
            pDocument.SetTrigger( ( int )CDefine.enumCamera.CAMERA_1 + iCameraIndex, CDefine.enumTrigger.TRIGGER_OFF );

            objTact.Stop();

            CInspectionResult.CInspectionHistoryData objHistory = new CInspectionResult.CInspectionHistoryData();
            objHistory.strCellID = objResult.objResultCommon.strCellID;
            objHistory.iPosition = pDocument.GetInspectionIndex() + 1;
            objHistory.iTactTime = ( int )objTact.ElapsedMilliseconds;
            objHistory.strResult = CDefine.enumResult.RESULT_OK == objResult.objResultCommon.eResult ? "OK" : "NG";
            objHistory.strNgType = ( ( CDefine.enumVisionAlarmType )iAlarmCode ).ToString();

            for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiScore.Count; iLoopCount++ ) {

                objHistory.dVidiScore[ iLoopCount ] = objResult.objResultCommon.objVidiScore[ iLoopCount ];
                if( iLoopCount < objResult.objResultCommon.objVidiResult.Count )
                    objHistory.strVidiResult[ iLoopCount ] = CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objVidiResult[ iLoopCount ] ? "OK" : "NG";
                else
                    objHistory.strVidiResult[ iLoopCount ] = "NG";
            }


            for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiScore.Count; iLoopCount++ ) {
                if( iLoopCount < objResult.objResultCommon.objMeasureResult.Count )
                    objHistory.str3dHeightResult[ iLoopCount ] = CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult[ iLoopCount ] ? "OK" : "NG";
                else
                    objHistory.str3dHeightResult[ iLoopCount ] = "NG";
            }

            //double dResolution = CDocument.GetDocument.m_objConfig.GetCameraParameter( ( int )CDefine.enumCamera.CAMERA_1 ).dResolution;
            for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiScore.Count; iLoopCount++ ) {
                if( iLoopCount >= objResult.objResultCommon.obj3DListWeldWidth.Count ) {
                    objHistory.dMeasureWidth[ iLoopCount ] = 0;
                    objHistory.dMeasureHeight[ iLoopCount ] = 0;
                } else {
                    objHistory.dMeasureWidth[ iLoopCount ] = objResult.objResultCommon.obj3DListWeldWidth[ iLoopCount ];
                }
                if( iLoopCount < objResult.objResultCommon.objMeasureResult3DWidth.Count )
                    objHistory.strMeasureResult[ iLoopCount ] = CDefine.enumResult.RESULT_OK == objResult.objResultCommon.objMeasureResult3DWidth[ iLoopCount ] ? "OK" : "NG";
                else
                    objHistory.strMeasureResult[ iLoopCount ] = "NG";
            }

            // 높이 샘플데이터
            for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiScore.Count; iLoopCount++ ) {
                if( iLoopCount >= objResult.objResultCommon.obj3DListSampleHeightData[ iLoopCount ].Count ) {
                    objHistory.dMeasureHeight[ iLoopCount ] = 0;
                } else {
                    objHistory.dMeasureHeight[ iLoopCount ] = objResult.objResultCommon.obj3DListSampleHeightData[ iLoopCount ].Max();
                }
                    
            }

            for( int iLoopCount = 0; iLoopCount < objResult.objResultCommon.objVidiScore.Count; iLoopCount++ ) {
                if( iLoopCount < objResult.objResultCommon.objMeasureResult.Count ) {
                    objHistory.dMeasureStartX[ iLoopCount ] = objResult.objResultCommon.objMeasureLine[ iLoopCount ].dStartX;
                    objHistory.dMeasureStartY[ iLoopCount ] = objResult.objResultCommon.objMeasureLine[ iLoopCount ].dStartY;
                    objHistory.dMeasureEndX[ iLoopCount ] = objResult.objResultCommon.objMeasureLine[ iLoopCount ].dEndX;
                    objHistory.dMeasureEndY[ iLoopCount ] = objResult.objResultCommon.objMeasureLine[ iLoopCount ].dEndY;
                }
            }

            objHistory.iMeasureLineFindCount = objResult.objResultCommon.objMeasureLine.Count;
            objHistory.dPatternPositionX = objResult.objResultCommon.dPatternPositionX;
            objHistory.dPatternPositionY = objResult.objResultCommon.dPatternPositionY;

            objResult.strSaveImagePath = pDocument.m_objConfig.GetSystemParameter().strImageSavePath;

            if( "" == objResult.objResultCommon.strCellID ) {
                objResult.objResultCommon.strCellID = System.DateTime.Now.ToString( "HH.mm.ss" );
                objHistory.strCellID = objResult.objResultCommon.strCellID;
            }

            string strToday = string.Format( "{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
            string strPosition = "Position_" + ( objResult.objResultCommon.iInspectionPosition + 1 ).ToString();
            string strImagePath = objResult.strSaveImagePath + "\\" + strToday + "\\" + objResult.objResultCommon.eResult.ToString() + "\\" + objResult.objResultCommon.strCellID + "\\" + strPosition + "\\";

            objHistory.strImagePath = strImagePath;
            // 데이터베이스 레포트 업데이트
            pDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryAlign( objHistory );





            objTact.Stop();
            objResult.objResultCommon.strTactTime = objTact.ElapsedMilliseconds.ToString();
          //  pDocument.SetUpdateDisplayPMS( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150.enumDisplayIndex.PMS );

            //// 이미지 저장경로 넣기
            objResult.strSaveImagePath = pDocument.m_objConfig.GetSystemParameter().strImageSavePath;
            pDocument.SetInspectionResultAlign( pDocument.GetInspectionIndex(), ( CInspectionResult.CResult )objResult.Clone() );

            // 여기서 결과 디스플레이를 뿌리자
            for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                pDocument.SetUpdateDisplayResultVIDI( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.VIDI_1 + iLoopCount );
            }
            // 
            // 여기서 결과 디스플레이를 뿌리자
            for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                pDocument.SetUpdateDisplayResultMeasure( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.MEASURE_1 + iLoopCount );
            }
            pDocument.SetUpdateDisplay3D( pDocument.GetInspectionIndex(), ( int )CFormMainProcess150Gocator.enumDisplayIndex.IMAGE );
            // 이미지 저장
            Save3DData( ( CInspectionResult.CResult )objResult.Clone() );
            // 배면 결과만 뿌리기
            pDocument.SetDisplayResultMonitor( pDocument.GetInspectionIndex() );
            // 택타임 측정
            pDocument.SetUpdateLog( ( CDefine.enumLogType )( ( int )CDefine.enumLogType.LOG_VISION_TACT_TIME_CAMERA_0 + iCameraIndex ), objTact.ElapsedMilliseconds.ToString() + "ms" );

			return bReturn;
		}

      
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 트리거 ( Input Tray )
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool DoProcessTrigger( int iCameraIndex )
        {
            bool bReturn = false;
            var pDocument = CDocument.GetDocument;
            int iTimeOut = 5000;
            int iTimePeriod = 3;
            CProcessVisionProcess150Gocator obj = m_objProcessVisionProcess150[ iCameraIndex ] as CProcessVisionProcess150Gocator;
            do {
                while( CProcessVisionProcess150Gocator.enumCommand.CMD_IDLE != obj.GetCommand() && iTimeOut > 0 ) {
                    iTimeOut -= iTimePeriod;
                    Thread.Sleep( iTimePeriod );
                }
                if( 0 >= iTimeOut ) break;

                // 설비 정지 모드인 경우
                if ( CDefine.enumRunMode.RUN_MODE_STOP == pDocument.GetRunMode() )
                {
                    // 트리거 ON
                    obj.SetCommand( CProcessVisionProcess150Gocator.enumCommand.CMD_START_GRAB );

                    iTimeOut = 17000;
                    while ( CProcessVisionProcess150Gocator.enumCommand.CMD_IDLE != obj.GetCommand() && iTimeOut > 0 )
                    {
                        iTimeOut -= iTimePeriod;
                        Thread.Sleep( iTimePeriod );
                    }
                    if( 0 >= iTimeOut ) break;
                }
                    // 설비 자동 모드인 경우
                else {
                    // 정지 트리거 타입
                        // 트리거 ON
                        if( CDefine.enumTrigger.TRIGGER_ON == pDocument.GetTrigger( iCameraIndex ) ) {
                            obj.SetCommand( CProcessVisionProcess150Gocator.enumCommand.CMD_START_GRAB );
                        }
                     
                    iTimeOut = 17000;
                    while( CProcessVisionProcess150Gocator.enumCommand.CMD_IDLE != obj.GetCommand() && iTimeOut > 0 ) {
                        iTimeOut -= iTimePeriod;
                        Thread.Sleep( iTimePeriod );
                    }
                    if( 0 >= iTimeOut ) break;
                }
              

                bReturn = true;
            } while( false );

            if( false == bReturn ) {
                // 트리거 ON
                obj.SetStopGrab();
                //obj.SetCommand( CProcessVisionProcess150Gocator.enumCommand.CMD_STOP_GRAB );
            }

            return bReturn;
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 시퀀스 스레드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private static void ThreadManager( object state )
		{
			structureManagerArgs obj = ( structureManagerArgs )state;
            CProcessVisionManagerProcess150Gocator pThis = ( CProcessVisionManagerProcess150Gocator )obj.pThis;
			int iCameraIndex = obj.iCameraIndex;
			// 트리거는 토글 형식으로 동작하는데.. 라이브는 on, off 형식이라 기본 Live off 함수로 들어감.
			// 이전 값 갖고 변경되는 시점에만 함수 타도록 변경

			while( false == pThis.m_bThreadExit ) {
				// 검사 타입별로 Is, Do 함수 구분시킴
                // 트레이 파트
				if( pThis.m_typeProcessVisionProcess150 == pThis.m_objProcessVisionProcess150[ iCameraIndex ].GetType() ) {
					if( true == pThis.IsInspection( iCameraIndex ) ) {
						pThis.DoProcessInspection( iCameraIndex );
                    }
				}
				Thread.Sleep( 5 );
			}
		}

	}
}