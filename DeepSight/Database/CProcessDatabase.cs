﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
	public class CProcessDatabase
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//public property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 데이터베이스 이력 클래스
		public CProcessDatabaseHistory m_objProcessDatabaseHistory;
		// 데이터베이스 정보 클래스
		public CProcessDatabaseInformation m_objProcessDatabaseInformation;
		// 데이터베이스 쿼리 메세지
		public CDatabaseSendMessage m_objDatabaseSendMessage;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CProcessDatabase()
		{

		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool Initialize()
		{
			bool bReturn = false;

			do {
				// 데이터베이스 이력 초기화
				m_objProcessDatabaseHistory = new CProcessDatabaseHistory();
				if( false == m_objProcessDatabaseHistory.Initialize( this ) ) break;
				// 데이터베이스 정보 초기화
				m_objProcessDatabaseInformation = new CProcessDatabaseInformation();
				if( false == m_objProcessDatabaseInformation.Initialize( this ) ) break;
				// 데이터베이스 히스토리 메세지
				CDatabaseImplementHistory objDatabaseImplementHistory = new CDatabaseImplementHistory();
				m_objDatabaseSendMessage = new CDatabaseSendMessage( objDatabaseImplementHistory );

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
		public void DeInitialize()
		{
			// 데이터베이스 이력 해제
			m_objProcessDatabaseHistory.Deinitialize();
			// 데이터베이스 정보 해제
			m_objProcessDatabaseInformation.Deinitialize();
		}
	}
}