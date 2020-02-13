using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DeepSight
{
	public class CCsvFile
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Csv 파일 -> DataTable
		//설명 : 인자 : strPath - 접근할 파일 경로 / 리턴 : DataTable로 반환
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public DataTable GetDataTableFromCsv( string strPath, bool isFirstRowHeader )
		{
			DataTable objDataTable = new DataTable();

			// 헤더 유무
			string strHeader = null;
			if( true == isFirstRowHeader ) {
				strHeader = "Yes";
			}
			else {
				strHeader = "No";
			}
			try {
				// 이름 제외한 폴더 경로
				string strPathOnly = Path.GetDirectoryName( strPath );
				// 파일 이름
				string strFileName = Path.GetFileName( strPath );
				// 전체 얻어옴
				string strQuery = string.Format( "select * from [{0}]", strFileName );
				// CSV 파일 -> DataTable 변환
				using( OleDbConnection objConnection = new OleDbConnection(
					@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPathOnly +
					";Extended Properties=\"Text;HDR=" + strHeader + "\"" ) )
				using( OleDbCommand objCommand = new OleDbCommand( strQuery, objConnection ) )
				using( OleDbDataAdapter objAdapter = new OleDbDataAdapter( objCommand ) ) {
					objDataTable.Locale = CultureInfo.CurrentCulture;
					objAdapter.Fill( objDataTable );
				}
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.Message );
			}

			return objDataTable;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : DataTable -> Csv 파일
		//설명 : 인자 : strPath - 접근할 파일 경로 / objDataTable - 저장할 DataTable
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetDataTableToCsv( string strPath, DataTable objDataTable )
		{
			try {
				FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
				StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.UTF8 );
				// 컬럼 구분자 , 로 나눔
				string strLine = string.Join( ",", objDataTable.Columns.Cast<object>() );
				objStreamWriter.WriteLine( strLine );
				// row 구분자 , 로 나눔
				for( int iLoopRow = 0; iLoopRow < objDataTable.Rows.Count; iLoopRow++ ) {
					strLine = string.Join( ",", objDataTable.Rows[ iLoopRow ].ItemArray.Cast<object>() );
					objStreamWriter.WriteLine( strLine );
				}
				objStreamWriter.Close();
				objFileStream.Close();
			}
			catch( Exception ex ) {
				Trace.WriteLine( ex.Message );
			}
		}
	}
}