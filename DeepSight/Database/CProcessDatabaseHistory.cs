using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
	public class CProcessDatabaseHistory : CDatabaseAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// History Delete
		private CProcessDatabaseHistoryDelete m_objProcessDatabaseHistoryDelete;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//public property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// SQLite
		public CSQLite m_objSQLite;
		// History Align
		public CManagerTable m_objManagerTableHistoryAlign;
		// History Inspection
		public CManagerTable m_objManagerTableHistoryInspection;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CProcessDatabaseHistory()
		{

		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool Initialize( CProcessDatabase objProcessDatabase )
		{
			bool bReturn = false;
			var pDocument = CDocument.GetDocument;
			do {
				// 상위 클래스 이어줌
				base.m_objProcessDatabase = objProcessDatabase;
				CConfig objConfig = pDocument.m_objConfig;
				// 데이터베이스 파라메터
				CConfig.CDatabaseParameter objDatabaseParameter = objConfig.GetDatabaseParameter();
				// SQLite 초기화
				m_objSQLite = new CSQLite();
				CErrorReturn objReturn = m_objSQLite.HLInitialize( string.Format( @"{0}\{1}.db3", objConfig.GetDatabaseHistoryPath(), objDatabaseParameter.strDatabaseHistory ) );
				if( true == objReturn.m_bError ) break;
				// SQLite Connect
				objReturn = m_objSQLite.HLConnect();
				if( true == objReturn.m_bError ) break;
				// History Align 초기화
				m_objManagerTableHistoryAlign = new CManagerTable();
				if( false == m_objManagerTableHistoryAlign.HLInitialize(
					m_objSQLite,
					string.Format( @"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryAlign ), "" ) ) break;
				// 				// History Inspection 초기화
				// 				m_objManagerTableHistoryInspection = new CManagerTable();
				// 				if( false == m_objManagerTableHistoryInspection.HLInitialize(
				// 					m_objSQLite,
				// 					string.Format( @"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryInspection ), "" ) ) break;

				// Process History Delete 초기화
				m_objProcessDatabaseHistoryDelete = new CProcessDatabaseHistoryDelete();
				if( false == m_objProcessDatabaseHistoryDelete.Initialize( m_objProcessDatabase, m_objSQLite ) ) break;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 해제 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void Deinitialize()
		{
			// Process History Delete 해제
			if( null != m_objProcessDatabaseHistoryDelete ) {
				m_objProcessDatabaseHistoryDelete.DeInitialize();
			}
			// History Align
			if( null != m_objManagerTableHistoryAlign ) {
				m_objManagerTableHistoryAlign.HLDeinitialize();
			}
			// History Inspection
			if( null != m_objManagerTableHistoryInspection ) {
				m_objManagerTableHistoryInspection.HLDeinitialize();
			}
			if( null != m_objSQLite ) {
				// SQLite Disconnect
				m_objSQLite.HLDisconnect();
				// SQLite 해제
				m_objSQLite.HLDeInitialize();
			}
		}
	}
}