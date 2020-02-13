using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AEC
{
	public class AEC256
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 우리 암호화 key
		private static string m_strKey = "hoonlabhoonlabhoonlabhoonlabNTkV";

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 인코딩
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static string AESEncrypt256( string strInput )
		{
			RijndaelManaged aes = new RijndaelManaged();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = Encoding.UTF8.GetBytes( m_strKey );
			aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

			var encrypt = aes.CreateEncryptor( aes.Key, aes.IV );
			byte[] xBuff = null;
			using( var ms = new MemoryStream() ) {
				using( var cs = new CryptoStream( ms, encrypt, CryptoStreamMode.Write ) ) {
					byte[] xXml = Encoding.UTF8.GetBytes( strInput );
					cs.Write( xXml, 0, xXml.Length );
				}

				xBuff = ms.ToArray();
			}

			string Output = Convert.ToBase64String( xBuff );
			return Output;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 디코딩
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static string AESDecrypt256( string strInput )
		{
			RijndaelManaged aes = new RijndaelManaged();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = Encoding.UTF8.GetBytes( m_strKey );
			aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

			var decrypt = aes.CreateDecryptor();
			byte[] xBuff = null;
			using( var ms = new MemoryStream() ) {
				using( var cs = new CryptoStream( ms, decrypt, CryptoStreamMode.Write ) ) {
					byte[] xXml = Convert.FromBase64String( strInput );
					cs.Write( xXml, 0, xXml.Length );
				}

				xBuff = ms.ToArray();
			}

			String Output = Encoding.UTF8.GetString( xBuff );
			return Output;
		}
	}
}