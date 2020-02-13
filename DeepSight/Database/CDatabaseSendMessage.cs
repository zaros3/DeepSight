﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DeepSight
{
	public class CDatabaseSendMessage
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private CDatabaseImplementAbstract m_objDatabaseImplement;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//public property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region 얼라인 데이터 클래스
		public class CHistoryAlign
		{
			// 검사 데이터
			public CInspectionResult.CInspectionHistoryData m_objData;

			public CHistoryAlign( CInspectionResult.CInspectionHistoryData objData )
			{
				m_objData = ( CInspectionResult.CInspectionHistoryData )objData.Clone();
			}
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDatabaseSendMessage( CDatabaseImplementAbstract objDatabaseImplement )
		{
			m_objDatabaseImplement = objDatabaseImplement;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 얼라인 히스토리 삽입
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetInsertHistoryAlign( CInspectionResult.CInspectionHistoryData objData )
		{
			CHistoryAlign obj = new CHistoryAlign( objData );
			ThreadPool.QueueUserWorkItem( new WaitCallback( m_objDatabaseImplement.SetInsertHistoryAlign ), obj );
		}
	}
}