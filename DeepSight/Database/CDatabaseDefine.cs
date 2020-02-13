using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
	public class CDatabaseDefine
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Define
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 날짜 시간 포멧
		public const string DEF_DATE_FORMAT = "yyyy-MM-dd";
		public const string DEF_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
		// 오름차순
		public const string DEF_ASC = "ASC";
		// 내림차순
		public const string DEF_DESC = "DESC";

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// enumeration
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Table Information UI Text Schema
		public enum enumInformationUIText { IDX, ID, FORM_NAME, TEXT_KOREA, TEXT_CHINA, TEXT_ENGLISH, TEXT_VIETNAM };
		// Table Information User Message Schema
		public enum enumInformationUserMessage { ID, TEXT_KOREA, TEXT_CHINA, TEXT_ENGLISH, TEXT_VIETNAM };
		// Table History Align Schema
		public enum enumHistoryAlign
		{
            INNER_ID = 0, DATE, CELL_ID, POSITION, TACT_TIME, RESULT, NG_TYPE, 
			VIDI_SCORE_1, VIDI_RESULT_1, VIDI_SCORE_2, VIDI_RESULT_2, VIDI_SCORE_3, VIDI_RESULT_3, VIDI_SCORE_4, VIDI_RESULT_4, VIDI_SCORE_5, VIDI_RESULT_5, VIDI_SCORE_6, VIDI_RESULT_6,
            MEASURE_WIDTH_1, MEASURE_HEIGHT_1, MEASURE_RESULT_1, MEASURE_WIDTH_2, MEASURE_HEIGHT_2, MEASURE_RESULT_2, MEASURE_WIDTH_3, MEASURE_HEIGHT_3, MEASURE_RESULT_3,
            MEASURE_WIDTH_4, MEASURE_HEIGHT_4, MEASURE_RESULT_4, MEASURE_WIDTH_5, MEASURE_HEIGHT_5, MEASURE_RESULT_5, MEASURE_WIDTH_6, MEASURE_HEIGHT_6, MEASURE_RESULT_6,
            HEIGHT_RESULT_1, HEIGHT_RESULT_2, HEIGHT_RESULT_3, HEIGHT_RESULT_4, HEIGHT_RESULT_5, HEIGHT_RESULT_6,
            MEASURE_START_X_1, MEASURE_START_Y_1, MEASURE_END_X_1, MEASURE_END_Y_1, MEASURE_START_X_2, MEASURE_START_Y_2, MEASURE_END_X_2, MEASURE_END_Y_2,
            MEASURE_START_X_3, MEASURE_START_Y_3, MEASURE_END_X_3, MEASURE_END_Y_3, MEASURE_START_X_4, MEASURE_START_Y_4, MEASURE_END_X_4, MEASURE_END_Y_4,
            MEASURE_START_X_5, MEASURE_START_Y_5, MEASURE_END_X_5, MEASURE_END_Y_5, MEASURE_START_X_6, MEASURE_START_Y_6, MEASURE_END_X_6, MEASURE_END_Y_6, 
            MEASURE_LINE_FIND_COUNT, PATTERN_POSITION_X, PATTERN_POSITION_Y,
            IMAGE_PATH
        };
		// 언어 모드
		public enum enumLanguage { LANGUAGE_KOREA = 0, LANGUAGE_CHINA, LANGUAGE_ENGLISH, LANGUAGE_VIETNAM, LANGUAGE_FINAL };
		// 알람 레벨 리스트 (알람 정의에 있는 테이블에 LEVEL 칼럼에서 사용)
		public enum enumAlarmLevelList { LIGHT = 0, HEAVY, ALARM_LEVEL_LIST_FINAL };
		// 알람 유닛 리스트 (알람 정의에 있는 테이블에 UNIT 칼럼에서 사용)
		public enum enumAlarmUnitList
		{
			ALIGN_STAGE = 0, DUAL_IN_TRANSFER, DUAL_OUT_TRANSFER, INSPECTION_STAGE,
			FRONT_EQP, MCR_TRANSFER, NACHI_IN_ROBOT, NACHI_OUT_ROBOT,
			BUFFER_STAGE, OUT_CONVEYOR, REAR_EQP, CAMERA, CIM, ETC, ALARM_UNIT_LIST_FINAL
		}
		// 알람 박스 타입 리스트 (알람 정의에 있는 테이블에 BOX_TYPE 칼럼에서 사용)
		public enum enumAlarmBoxTypeList { SQUARE = 0, ALARM_BOX_TYPE_LIST_FINAL }

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Class Data
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// EC 리스트
		public class CECListData : ICloneable
		{
			public string strECID;
			public string strECItemName;
			public string strECDef;
			public string strECSll;
			public string strECSul;
			public string strECWll;
			public string strECWul;

			public CECListData()
			{
				strECID = "";
				strECItemName = "";
				strECDef = "";
				strECSll = "";
				strECSul = "";
				strECWll = "";
				strECWul = "";
			}

			public object Clone()
			{
				CECListData objECListData = new CECListData();
				objECListData.strECID = this.strECID;
				objECListData.strECItemName = this.strECItemName;
				objECListData.strECDef = this.strECDef;
				objECListData.strECSll = this.strECSll;
				objECListData.strECSul = this.strECSul;
				objECListData.strECWll = this.strECWll;
				objECListData.strECWul = this.strECWul;
				return objECListData;
			}
		}

		// 공유 메모리에 올라오는 알람 자료를 리스트 형태로 갖고 있어야 해당 셀을 클릭했을 때 자료를 보여주기 용이함
		public class CAlarmData : ICloneable
		{
			// 알람 발생 시간
			public string strAlarmDate;
			// 알람 아이디
			public int iAlarmID;
			// 알람 레벨
			public enumAlarmLevelList eAlarmLevel;
			// 알람 해당 유닛
			public enumAlarmUnitList eAlarmUnit;
			// 알람 표시될 박스종류
			public enumAlarmBoxTypeList eAlarmBoxType;
			// 알람 표시할 박스 시작점 X
			public int iAlarmBoxRectangleX;
			// 알람 표시할 박스 시작점 Y
			public int iAlarmBoxRectangleY;
			// 알람 표시할 박스 시작점 으로부터 넓이
			public int iAlarmBoxRectangleWidth;
			// 알람 표시할 박스 시작점 으로부터 높이
			public int iAlarmBoxRectangleHeight;
			// 알람 내용
			public string[] strAlarmText;
			// 알람 내용
			public string[] strAlarmActionText;

			public CAlarmData()
			{
				strAlarmDate = "";
				iAlarmID = 0;
				eAlarmLevel = 0;
				eAlarmUnit = 0;
				eAlarmBoxType = 0;
				iAlarmBoxRectangleX = 0;
				iAlarmBoxRectangleY = 0;
				iAlarmBoxRectangleWidth = 0;
				iAlarmBoxRectangleHeight = 0;
				strAlarmText = new string[ ( int )enumLanguage.LANGUAGE_FINAL ];
				strAlarmActionText = new string[ ( int )enumLanguage.LANGUAGE_FINAL ];
				for( int iLoopLanguage = 0; iLoopLanguage < ( int )enumLanguage.LANGUAGE_FINAL; iLoopLanguage++ ) {
					strAlarmText[ iLoopLanguage ] = "";
					strAlarmActionText[ iLoopLanguage ] = "";
				}
			}

			public object Clone()
			{
				CAlarmData objData = new CAlarmData();

				objData.strAlarmDate = this.strAlarmDate;
				objData.iAlarmID = this.iAlarmID;
				objData.eAlarmLevel = this.eAlarmLevel;
				objData.eAlarmUnit = this.eAlarmUnit;
				objData.eAlarmBoxType = this.eAlarmBoxType;
				objData.iAlarmBoxRectangleX = this.iAlarmBoxRectangleX;
				objData.iAlarmBoxRectangleY = this.iAlarmBoxRectangleY;
				objData.iAlarmBoxRectangleWidth = this.iAlarmBoxRectangleWidth;
				objData.iAlarmBoxRectangleHeight = this.iAlarmBoxRectangleHeight;
				objData.strAlarmText = new string[ ( int )enumLanguage.LANGUAGE_FINAL ];
				objData.strAlarmActionText = new string[ ( int )enumLanguage.LANGUAGE_FINAL ];
				for( int iLoopLanguage = 0; iLoopLanguage < ( int )enumLanguage.LANGUAGE_FINAL; iLoopLanguage++ ) {
					objData.strAlarmText[ iLoopLanguage ] = this.strAlarmText[ iLoopLanguage ];
					objData.strAlarmActionText[ iLoopLanguage ] = this.strAlarmActionText[ iLoopLanguage ];
				}

				return objData;
			}
		}
	}
}