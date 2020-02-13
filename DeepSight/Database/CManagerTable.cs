using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;

namespace DeepSight
{
	public class CManagerTable
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Enumeration
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 스키마 정보
		public enum enumSchemaInfo { SCHEMA_INFO_INDEX = 0, SCHEMA_INFO_NAME = 1, SCHEMA_INFO_TYPE = 2, SCHEMA_INFO_NOT_NULL = 3, SCHEMA_INFO_DEFAULT = 4, SCHEMA_INFO_PK = 5 };

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// member variable
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// SQLite
		private CSQLite m_objSQLite;
		// 테이블 이름
		private string m_strTableName;
		// 테이블 스키마 이름 문자열 배열
		private string[] m_strTableSchemaName;
		// 테이블 스키마 타입 문자열 배열
		private string[] m_strTableSchemaType;
		// pk 인덱스
		private int m_iPkIndex = 0;
		// 테이블에 전체 데이터 테이블
		private DataTable m_objDataTable;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool HLInitialize( CSQLite objSQLite, string strTableFullPath, string strRecordFullPath )
		{
			bool bReturn = false;

			do {
				try {
					// SQLite 이어줌
					m_objSQLite = objSQLite;
					// Txt 파일 클래스
					CTxtFile objTxtFile = new CTxtFile();
					// 이름 제외한 폴더 경로
					string strTablePathOnly = Path.GetDirectoryName( strTableFullPath );
					// 파일 이름 (확장자를 포함)
					string strTableExtendName = Path.GetFileName( strTableFullPath );
					// 확장자를 제거한 파일 이름
					m_strTableName = Path.GetFileNameWithoutExtension( strTableFullPath );
					// 확장자
					string strTableExtendOnly = Path.GetExtension( strTableFullPath );
					// 테이블이 이미 생성되어 있으면 생성 쿼리문 건너뜀
					bool bExistence = new bool();
					if( false == HLGetTableExistence( m_strTableName, ref bExistence ) ) break;
					if( false == bExistence ) {
						// 테이블 생성
						DataTable objDataTable = new DataTable();
						// .csv 파일은 읽지 않는 걸로 수정
						if( ".txt" == strTableExtendOnly.ToLower() ) {
							objDataTable = objTxtFile.GetDataTableFromTxt( string.Format( @"{0}\{1}", strTablePathOnly, strTableExtendName ), true );
						}
						else {
							string strThrowLog = string.Format( "CManagerTable HLInitialize There is no {0} file.", strTableExtendName );
							throw new System.ArgumentException( strThrowLog );
						}
						if( false == HLSetTableCreate( m_strTableName, objDataTable ) ) break;
					}
					// 테이블 스키마 정보 얻음
					DataTable objSchemaInfo = new DataTable();
					if( false == HLGetTableInformation( m_strTableName, ref objSchemaInfo ) ) break;
					// 테이블 스키마 에트리뷰트 이름, 타입 가져옴
					DataRow[] objSchemaInfoRow = objSchemaInfo.Select();
					m_strTableSchemaName = new string[ objSchemaInfoRow.Length ];
					m_strTableSchemaType = new string[ objSchemaInfoRow.Length ];
					for( int iLoopRow = 0; iLoopRow < objSchemaInfoRow.Length; iLoopRow++ ) {
						m_strTableSchemaName[ iLoopRow ] = objSchemaInfoRow[ iLoopRow ].ItemArray[ ( int )enumSchemaInfo.SCHEMA_INFO_NAME ].ToString();
						m_strTableSchemaType[ iLoopRow ] = objSchemaInfoRow[ iLoopRow ].ItemArray[ ( int )enumSchemaInfo.SCHEMA_INFO_TYPE ].ToString();
						// pk에 해당하는 row값 저장
						if( "1" == objSchemaInfoRow[ iLoopRow ].ItemArray[ ( int )enumSchemaInfo.SCHEMA_INFO_PK ].ToString() ) {
							m_iPkIndex = iLoopRow;
						}
					}
					// 레코드 파일이 있으면 레코드 INSERT 쿼리문 실행
					if( null != strRecordFullPath && "" != strRecordFullPath ) {
						// 이름 제외한 폴더 경로
						string strRecordPathOnly = Path.GetDirectoryName( strRecordFullPath );
						// 파일 이름 (확장자를 포함)
						string strRecordExtendName = Path.GetFileName( strRecordFullPath );
						// 확장자를 제거한 파일 이름
						string strRecordName = Path.GetFileNameWithoutExtension( strRecordFullPath );
						// 확장자
						string strRecordExtendOnly = Path.GetExtension( strRecordFullPath );
						// 레코드 데이터 밀어넣기 전에 테이블에 데이터 삭제
						if( false == HLSetTableDataDelete( m_strTableName ) ) break;
						// 테이블에 레코드 삽입
						DataTable objDataTable = new DataTable();
						// .csv 파일은 읽지 않는 걸로 수정
						if( ".txt" == strRecordExtendOnly.ToLower() ) {
							objDataTable = objTxtFile.GetDataTableFromTxt( string.Format( @"{0}\{1}", strRecordPathOnly, strRecordExtendName ), true );
						}
						else {
							string strThrowLog = string.Format( "CManagerTable HLInitialize There is no {0} file.", strTableExtendName );
							throw new System.ArgumentException( strThrowLog );
						}
						if( false == HLSetTableDataInsert( m_strTableName, objDataTable ) ) break;
					}
					// 해당 테이블에 전체 Select 결과를 저장
					string strQuery = string.Format( "select * from {0}", m_strTableName );
					m_objDataTable = new DataTable();
					m_objSQLite.HLReload( strQuery, ref m_objDataTable );
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
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
		public void HLDeinitialize()
		{

		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 테이블 생성 - 파일에 테이블 생성 쿼리를 읽어서 실행
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLSetTableCreate( string strTableName, DataTable objDataTable )
		{
			bool bReturn = false;

			do {
				// 데이터베이스 테이블 생성
				CSchemaInformation[] objSchemaInformation = HLGetSchemaInformation( objDataTable );
				if( false == HLSetTableCreate( strTableName, ref objSchemaInformation ) ) break;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 테이블 데이터 삭제
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLSetTableDataDelete( string strTableName )
		{
			bool bReturn = false;

			do {
				string strQuery = string.Format( "delete from {0}", strTableName );
				CErrorReturn objReturn = m_objSQLite.HLExecute( strQuery );
				if( true == objReturn.m_bError ) break;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 테이블 데이터 삽입
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLSetTableDataInsert( string strTableName, DataTable objDataTable )
		{
			bool bReturn = false;

			do {
				try {
					string strQuery = "";
					// 트랜잭션 시작
					SQLiteTransaction objTransaction = m_objSQLite.HLBeginTransaction();
					// INSERT 해야 하는 데이터 테이블 레코드 수만큼
					for( int iLoopRow = 0; iLoopRow < objDataTable.Rows.Count; iLoopRow++ ) {
						string strValues = "";
						DataRow objDataRow = objDataTable.Rows[ iLoopRow ];
						strQuery = string.Format( "insert into {0} values", strTableName );
						// 테이블에 정의된 스키마 타입 개수만큼
						for( int iLoopSchema = 0; iLoopSchema < m_strTableSchemaType.Length; iLoopSchema++ ) {
							// 타입 검사
							if( "INTEGER" == m_strTableSchemaType[ iLoopSchema ] ) {
								strValues += string.Format( "{0}", Convert.ToInt32( objDataRow[ iLoopSchema ] ) );
							}
							else if( true == m_strTableSchemaType[ iLoopSchema ].Contains( "VARCHAR" ) ) {
								strValues += string.Format( "'{0}'", objDataRow[ iLoopSchema ].ToString() );
							}
							else if( "REAL" == m_strTableSchemaType[ iLoopSchema ] || "DOUBLE" == m_strTableSchemaType[ iLoopSchema ] ) {
								strValues += string.Format( "{0}", Convert.ToDouble( objDataRow[ iLoopSchema ] ) );
							}

							if( iLoopSchema != m_strTableSchemaType.Length - 1 ) {
								strValues += ",";
							}
						}
						// INSERT 쿼리문
						strQuery = string.Format( "{0} ({1})", strQuery, strValues );
						// 쿼리문 수행
						CErrorReturn objReturn = m_objSQLite.HLExecute( strQuery );
						if( true == objReturn.m_bError ) {
							Trace.WriteLine( objReturn.m_strErrorMessage );
						}
					}
					// 문제 없으면 트랜잭션 커밋
					m_objSQLite.HLCommit( objTransaction );
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Get Table Information
		//설명 : 테이블 스키마 정보 받음
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLGetTableInformation( string strTableName, ref DataTable objDataTable )
		{
			bool bReturn = false;

			do {
				string strQuery = string.Format( "pragma table_info({0})", strTableName );
				CErrorReturn objReturn = m_objSQLite.HLReload( strQuery, ref objDataTable );
				if( true == objReturn.m_bError ) break;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Get Table Existence
		//설명 : 테이블 존재 유무
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLGetTableExistence( string strTableName, ref bool bExistence )
		{
			bool bReturn = false;

			do {
				string strQuery = string.Format( "select count(*) from sqlite_master where name = '{0}'", strTableName );
				DataTable objDataTable = new DataTable();
				CErrorReturn objReturn = m_objSQLite.HLReload( strQuery, ref objDataTable );
				if( true == objReturn.m_bError ) break;
				// 테이블 존재 유무 결과 받음
				bExistence = Convert.ToBoolean( objDataTable.Rows[ 0 ][ 0 ] );

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 테이블 생성
		//설명 : strTableName : 생성 테이블 이름 / strCreate : 생성 쿼리
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLSetTableCreate( string strTableName, string strCreate )
		{
			bool bReturn = false;

			do {
				string strQuery = null;
				// 데이터베이스에서 delete 명령으로 값을 삭제할 때 실제 항목을 삭제하기 위해 테이블 생성 전에 해당 쿼리를 날려줌
				strQuery = "pragma auto_vacuum = 1";
				CErrorReturn objReturn = m_objSQLite.HLExecute( strQuery );
				if( true == objReturn.m_bError ) break;
				strQuery = string.Format( "create table if not exists {0} ( {1} )", strTableName, strCreate );
				objReturn = m_objSQLite.HLExecute( strQuery );
				if( true == objReturn.m_bError ) break;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 
		//설명 : 테이블 생성 (데이터를 받아서 쿼리문 생성)
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private bool HLSetTableCreate( string strTableName, ref CSchemaInformation[] objSchemaInformation )
		{
			bool bReturn = false;

			do {
				string strCreate = null;
				string strPK = null;
				string strAutoIncrement = null;
				string strNotNull = null;

				for( int iLoopSchema = 0; iLoopSchema < objSchemaInformation.Length; iLoopSchema++ ) {
					// PK 키 설정 유무
					if( true == objSchemaInformation[ iLoopSchema ].m_bPk ) {
						strPK = "PRIMARY KEY ";
					}
					else {
						strPK = "";
					}
					// AutoIncrement 설정 유무
					if( true == objSchemaInformation[ iLoopSchema ].m_bAutoIncrement ) {
						strAutoIncrement = "AUTOINCREMENT ";
					}
					else {
						strAutoIncrement = "";
					}
					// Not Null 설정 유무
					if( true == objSchemaInformation[ iLoopSchema ].m_bNotNull ) {
						strNotNull = "NOT NULL ";
					}
					else {
						strNotNull = "";
					}

					strCreate += string.Format( "{0} {1} {2}{3}{4}",
						objSchemaInformation[ iLoopSchema ].m_strColumnName,
						objSchemaInformation[ iLoopSchema ].m_strDataType,
						strPK,
						strAutoIncrement,
						strNotNull );

					if( iLoopSchema != objSchemaInformation.Length - 1 ) {
						strCreate += ", ";
					}
				}
				// 생성
				if( false == HLSetTableCreate( strTableName, strCreate ) ) break;

				bReturn = true;
			} while( false );

			return bReturn;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 파일에 접근해서 스키마 정보를 얻어옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private CSchemaInformation[] HLGetSchemaInformation( DataTable objDataTable )
		{
			CSchemaInformation[] objSchemaInformation = null;

			try {
				objSchemaInformation = new CSchemaInformation[ objDataTable.Rows.Count ];
				// Row 값 Schema Information 자료에 넣는다.
				for( int iLoopRow = 0; iLoopRow < objDataTable.Rows.Count; iLoopRow++ ) {
					DataRow objDataRow = objDataTable.Rows[ iLoopRow ];
					objSchemaInformation[ iLoopRow ] = new CSchemaInformation(
						objDataRow.ItemArray[ ( int )CSchemaInformation.enumSchemaInformation.SCHEMA_INFORMATION_COLUMN_NAME ].ToString(),
						objDataRow.ItemArray[ ( int )CSchemaInformation.enumSchemaInformation.SCHEMA_INFORMATION_DATA_TYPE ].ToString(),
						Convert.ToBoolean( Convert.ToInt32( objDataRow.ItemArray[ ( int )CSchemaInformation.enumSchemaInformation.SCHEMA_INFORMATION_PK ] ) ),
						Convert.ToBoolean( Convert.ToInt32( objDataRow.ItemArray[ ( int )CSchemaInformation.enumSchemaInformation.SCHEMA_INFORMATION_AUTOINCREMENT ] ) ),
						Convert.ToBoolean( Convert.ToInt32( objDataRow.ItemArray[ ( int )CSchemaInformation.enumSchemaInformation.SCHEMA_INFORMATION_NOT_NULL ] ) ) );
				}
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.Message );
			}

			return objSchemaInformation;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 외부에서 해당 객체 테이블 이름을 얻어옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string HLGetTableName()
		{
			return m_strTableName;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 외부에서 해당 객체 테이블 스키마 칼럼's 이름을 얻어옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string[] HLGetTableSchemaName()
		{
			return m_strTableSchemaName;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 외부에서 해당 객체 테이블 스키마 칼럼's 타입을 얻어옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string[] HLGetTableSchemaType()
		{
			return m_strTableSchemaType;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 외부에서 해당 객체 데이터 테이블을 얻어옴
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public DataTable HLGetDataTable()
		{
			return m_objDataTable;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 외부에서 해당 객체 데이터 테이블을 설정함
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void HLSetDataTable( DataTable objDataTable )
		{
			m_objDataTable = objDataTable;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 외부에서 해당 객체 데이터 테이블에 pk를 재설정
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public int HLGetPrimaryKey()
		{
			return m_iPkIndex;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 데이터 테이블을 select * from 으로 갱신함
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void HLSetDataTableUpdate()
		{
			string strQuery = string.Format( "select * from {0}", m_strTableName );
			m_objSQLite.HLReload( strQuery, ref m_objDataTable );
		}
	}
}