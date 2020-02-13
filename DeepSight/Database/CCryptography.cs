using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DeepSight
{
	public class CCryptography
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 암호로 사용한 것은 Base64 인코딩 / 디코딩
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public string SetEncoding( string strText )
		{
			string strReturn = null;

			do {
				try {
					byte[] bData = new byte[ strText.Length ];
					bData = System.Text.Encoding.UTF8.GetBytes( strText );
					string strEncodeData = Convert.ToBase64String( bData );
					strReturn = strEncodeData;
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}
			} while( false );

			return strReturn;
		}

		public string SetDecoding( string strText )
		{
			string strReturn = null;

			do {
				try {
					System.Text.UTF8Encoding objEncoding = new System.Text.UTF8Encoding();
					System.Text.Decoder objDecoder = objEncoding.GetDecoder();

					byte[] bDecode = Convert.FromBase64String( strText );
					int iCount = objDecoder.GetCharCount( bDecode, 0, bDecode.Length );
					char[] charDecode = new char[ iCount ];
					objDecoder.GetChars( bDecode, 0, bDecode.Length, charDecode, 0 );
					strReturn = new string( charDecode );
				}
				catch( Exception ex ) {
					Trace.WriteLine( ex.Message );
				}
			} while( false );

			return strReturn;
		}
	}
}