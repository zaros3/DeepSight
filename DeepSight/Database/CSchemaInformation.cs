using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSight
{
	/// <summary>
	/// 스키마 정보를 정의
	/// </summary>
	public class CSchemaInformation
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Enumeration
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 스키마 정보
		/// </summary>
		public enum enumSchemaInformation
		{
			SCHEMA_INFORMATION_COLUMN_NAME = 0,
			SCHEMA_INFORMATION_DATA_TYPE,
			SCHEMA_INFORMATION_PK,
			SCHEMA_INFORMATION_AUTOINCREMENT,
			SCHEMA_INFORMATION_NOT_NULL,
			SCHEMA_INFORMATION_FINAL
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// member variable
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 칼럼 이름
		/// </summary>
		private string _strColumnName;
		public string m_strColumnName
		{
			get { return _strColumnName; }
		}
		/// <summary>
		/// 데이터 타입
		/// </summary>
		private string _strDataType;
		public string m_strDataType
		{
			get { return _strDataType; }
		}
		private Type _objDataType;
		public Type m_objDataType
		{
			get { return _objDataType; }
		}
		/// <summary>
		/// pk 유무
		/// </summary>
		private bool _bPk;
		public bool m_bPk
		{
			get { return _bPk; }
		}
		/// <summary>
		/// Auto Increment 유무
		/// </summary>
		private bool _bAutoIncrement;
		public bool m_bAutoIncrement
		{
			get { return _bAutoIncrement; }
		}
		/// <summary>
		/// Not Null 유무
		/// </summary>
		private bool _bNotNull;
		public bool m_bNotNull
		{
			get { return _bNotNull; }
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CSchemaInformation( string strColumnName, string strDataType, bool bPK, bool bAutoIncrement, bool bNotNull )
		{
			// 공백 제거 & 대문자
			_strColumnName = strColumnName.Trim().ToUpper();
			_strDataType = strDataType.Trim().ToUpper();
			_bPk = bPK;
			_bAutoIncrement = bAutoIncrement;
			_bNotNull = bNotNull;
			// Sqlite에서 정의하는 데이터 타입
			// DataType 보고 실제 Type 정해줌
			if( "INTEGER" == _strDataType ||
				"INT" == _strDataType ||
				"TINYINT" == _strDataType ||
				"SMALLINT" == _strDataType ||
				"MEDIUMINT" == _strDataType ||
				"BIGINT" == _strDataType ||
				"UNSIGNED BIG INT" == _strDataType ||
				"INT2" == _strDataType ||
				"INT8" == _strDataType ) {
				_objDataType = typeof( int );
			}
			else if( "REAL" == _strDataType ||
				"DOUBLE" == _strDataType ||
				"DOUBLE PRECISION" == _strDataType ||
				"FLOAT" == _strDataType ) {
				_objDataType = typeof( double );
			}
			else if( true == _strDataType.Contains( "CHARACTER" ) ||
				true == _strDataType.Contains( "VARCHAR" ) ||
				true == _strDataType.Contains( "VARYING CHARACTER" ) ||
				true == _strDataType.Contains( "NCHAR" ) ||
				true == _strDataType.Contains( "NATIVE CHARACTER" ) ||
				true == _strDataType.Contains( "NVARCHAR" ) ||
				"TEXT" == _strDataType ||
				"CLOB" == _strDataType ) {
				_objDataType = typeof( string );
			}
			else {
				_objDataType = typeof( string );
			}
		}
	}
}