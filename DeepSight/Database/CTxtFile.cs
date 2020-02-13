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
	public class CTxtFile
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : Txt 파일 -> DataTable
		//설명 : 인자 : strPath - 접근할 파일 경로 / 리턴 : DataTable로 반환
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public DataTable GetDataTableFromTxt( string strPath, bool isFirstRowHeader )
		{
			DataTable objDataTable = new DataTable();

			try {
				// 공백, 탭 문자 제거
				char[] chRemove = { ' ', '\t' };
				string[] strLines = File.ReadAllLines( strPath, Encoding.Unicode );
				string[] strCols = strLines[ 0 ].Split( ',' );
				// 헤더가 포함되어 있으면 칼럼을 삽입
				if( true == isFirstRowHeader ) {
					for( int iLoopColumn = 0; iLoopColumn < strCols.Length; iLoopColumn++ ) {
						objDataTable.Columns.Add( new DataColumn( strCols[ iLoopColumn ].Trim( chRemove ) ) );
					}
				}
				else {
					for( int iLoopColumn = 0; iLoopColumn < strCols.Length; iLoopColumn++ ) {
						objDataTable.Columns.Add( new DataColumn( "Column" + iLoopColumn.ToString() ) );
					}
				}
				// 레코드 파일을 삽입
				for( int iLoopLine = 1; iLoopLine < strLines.Length; iLoopLine++ ) {
					string[] strRecord = strLines[ iLoopLine ].Split( ',' );

					DataRow objDataRow = objDataTable.NewRow();
					for( int iLoopRow = 0; iLoopRow < strRecord.Length; iLoopRow++ ) {
						objDataRow[ iLoopRow ] = strRecord[ iLoopRow ].Trim( chRemove );
					}
					objDataTable.Rows.Add( objDataRow );
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
		//목적 : DataTable -> Txt 파일
		//설명 : 인자 : strPath - 접근할 파일 경로 / objDataTable - 저장할 DataTable
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public void SetDataTableToTxt( string strPath, DataTable objDataTable )
		{
			try {
				FileStream objFileStream = new FileStream( strPath, FileMode.Create, FileAccess.Write );
				StreamWriter objStreamWriter = new StreamWriter( objFileStream, Encoding.Unicode );
				// 컬럼 구분자 \t,\t 로 나눔
				string strLine = string.Join( "\t,\t", objDataTable.Columns.Cast<object>() );
				objStreamWriter.WriteLine( strLine );
				// row 구분자 \t,\t 로 나눔
				for( int iLoopRow = 0; iLoopRow < objDataTable.Rows.Count; iLoopRow++ ) {
					strLine = string.Join( "\t,\t", objDataTable.Rows[ iLoopRow ].ItemArray.Cast<object>() );
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