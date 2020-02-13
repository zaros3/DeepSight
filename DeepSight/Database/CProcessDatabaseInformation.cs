using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
	public class CProcessDatabaseInformation : CDatabaseAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//public property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// SQLite
		public CSQLite m_objSQLite;
		// Information UI Text
		public CManagerTable m_objManagerTableInformationUIText;
		// Information User Message
		public CManagerTable m_objManagerTableInformationUserMessage;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CProcessDatabaseInformation()
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
				CErrorReturn objReturn = m_objSQLite.HLInitialize( string.Format( @"{0}\{1}.db3", objConfig.GetCurrentPath(), objDatabaseParameter.strDatabaseInformation ) );
				if( true == objReturn.m_bError ) break;
				// SQLite Connect
				objReturn = m_objSQLite.HLConnect();
				if( true == objReturn.m_bError ) break;
				// Information UI Text 초기화
				m_objManagerTableInformationUIText = new CManagerTable();
				if( false == m_objManagerTableInformationUIText.HLInitialize(
					m_objSQLite,
					string.Format( @"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationUIText ),
					string.Format( @"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationUIText ) ) ) break;
				// Information User Message 초기화
				m_objManagerTableInformationUserMessage = new CManagerTable();
				if( false == m_objManagerTableInformationUserMessage.HLInitialize(
					m_objSQLite,
					string.Format( @"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationUserMessage ),
					string.Format( @"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationUserMessage ) ) ) break;

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
			// Information UI Text
			if( null != m_objManagerTableInformationUIText ) {
				m_objManagerTableInformationUIText.HLDeinitialize();
			}
			// Information User Message
			if( null != m_objManagerTableInformationUserMessage ) {
				m_objManagerTableInformationUserMessage.HLDeinitialize();
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