using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
	public class CDatabaseImplementHistory : CDatabaseImplementAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDatabaseImplementHistory()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 얼라인 히스토리 삽입
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override void SetInsertHistoryAlign( object objData )
		{
			CDatabaseSendMessage.CHistoryAlign obj = objData as CDatabaseSendMessage.CHistoryAlign;

			var pDocument = CDocument.GetDocument;
			string strQuery = null;
			string strDate = DateTime.Now.ToString( CDatabaseDefine.DEF_DATE_TIME_FORMAT );
			CManagerTable objManagerTableAlign = pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlign;

			lock( pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite ) {
				try {
					// 트랜잭션 시작
					var objTransaction = pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
					// Align Data
					strQuery = string.Format( "insert into {0} values (", objManagerTableAlign.HLGetTableName() );
                    strQuery += string.Format( "'{0}_{1}',", strDate, obj.m_objData.strCellID.ToString() ); // inner id
                    strQuery += string.Format( "'{0}',", strDate ); // date 
					strQuery += string.Format( "'{0}',", obj.m_objData.strCellID ); // cell id
                    strQuery += string.Format( "{0:D},", obj.m_objData.iPosition ); // position
                    strQuery += string.Format( "{0:D},", obj.m_objData.iTactTime ); // tact time
                    strQuery += string.Format( "'{0}',", obj.m_objData.strResult ); // Result
                    strQuery += string.Format( "'{0}',", obj.m_objData.strNgType ); // Ng Type
                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        // VIDI 결과
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dVidiScore[ iLoopCount ] );
                        strQuery += string.Format( "'{0}',", obj.m_objData.strVidiResult[ iLoopCount ] );
                    }

                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        //치수측정
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dMeasureWidth[ iLoopCount ] );
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dMeasureHeight[ iLoopCount ] );
                        strQuery += string.Format( "'{0}',", obj.m_objData.strMeasureResult[ iLoopCount ] );
                    }

                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        //치수측정
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.str3dHeightResult[ iLoopCount ] );
                    }

                    for( int iLoopCount = 0; iLoopCount < CDefine.DEF_MAX_COUNT_CROP_REGION; iLoopCount++ ) {
                        //치수측정
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dMeasureStartX[ iLoopCount ] );
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dMeasureStartY[ iLoopCount ] );
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dMeasureEndX[ iLoopCount ] );
                        strQuery += string.Format( "'{0:F2}',", obj.m_objData.dMeasureEndY[ iLoopCount ] );
                    }

                    strQuery += string.Format( "'{0}',", obj.m_objData.iMeasureLineFindCount );
                    strQuery += string.Format( "'{0:F2}',", obj.m_objData.dPatternPositionX );
                    strQuery += string.Format( "'{0:F2}',", obj.m_objData.dPatternPositionY );

                    // 마지막 구문
                    strQuery += string.Format( "'{0}')", obj.m_objData.strImagePath );

                    
					pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute( strQuery );
					// 커밋
					pDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit( objTransaction );
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}
			}
		}
	}
}