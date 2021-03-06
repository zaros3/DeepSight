﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using ENC.MemoryMap.Manager;
using AEC;

namespace DeepSight
{
	public class CLogin
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private struct
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private class structureLogInParameter : ICloneable
		{
			private const int iUserAuthorityLevelCount = ( int )CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL;
			public string[] strLoginParameter = new string[ iUserAuthorityLevelCount ];

			public object Clone()
			{
				structureLogInParameter objLogInParameter = new structureLogInParameter();
				objLogInParameter.strLoginParameter = this.strLoginParameter as string[];
				return objLogInParameter;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 로그인 파라미터
		private structureLogInParameter m_objLogInParameter;

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CLogin()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public bool Initialize()
		{
			bool bReturn = false;

			do {
				m_objLogInParameter = new structureLogInParameter();
				LoadUserInformationParameter();
				SaveUserInformationParameter();

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
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 유저 정보 파라미터 로드
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void LoadUserInformationParameter()
		{
			var pDocument = CDocument.GetDocument;

			do {
				string strPath = string.Format( @"{0}\{1}", pDocument.m_objConfig.GetCurrentPath(), CDefine.DEF_USER_INFORMATION );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "SYSTEM";
				string strLogInPassword = "";
				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL; iLoopCount++ ) {
					string strData = objINI.GetString( strSection, string.Format( "strLoginParameter{0}", iLoopCount.ToString() ), "Pok+j8Eyvhqpuxu+j2h8fg==" );
					// 데이터 복호화
					strLogInPassword = AEC256.AESDecrypt256( strData );
					m_objLogInParameter.strLoginParameter[ iLoopCount ] = strLogInPassword;
				}

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 유저 정보 파라미터 저장
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SaveUserInformationParameter()
		{
			var pDocument = CDocument.GetDocument;

			do {
				string strPath = string.Format( @"{0}\{1}", pDocument.m_objConfig.GetCurrentPath(), CDefine.DEF_USER_INFORMATION );
				ClassINI objINI = new ClassINI( strPath );

				string strSection = "SYSTEM";
				string strLogInPassword = "";
				for( int iLoopCount = 0; iLoopCount < ( int )CDefine.enumUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL; iLoopCount++ ) {
					// 데이터 암호화
					strLogInPassword = AEC256.AESEncrypt256( m_objLogInParameter.strLoginParameter[ iLoopCount ] );
					objINI.WriteValue( strSection, string.Format( "strLoginParameter{0}", iLoopCount.ToString() ), strLogInPassword );
				}

			} while( false );
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 로그인 확인
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDefine.enumLoginResult SetLogin( CDefine.enumUserAuthorityLevel eAuthorityLevel, string strPassword )
		{
			CDefine.enumLoginResult objResult = CDefine.enumLoginResult.PASSWORD_FAIL;

			do {
				if( strPassword != m_objLogInParameter.strLoginParameter[ ( int )eAuthorityLevel ] ) break;

				objResult = CDefine.enumLoginResult.SUCCESS;
			} while( false );

			return objResult;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 암호 변경
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CDefine.enumLoginResult SetChangePassword( CDefine.enumUserAuthorityLevel eAuthorityLevel, string strPassword )
		{
			CDefine.enumLoginResult objResult = CDefine.enumLoginResult.PASSWORD_FAIL;

			do {
				// 암호 변경해서 데이터 저장
				m_objLogInParameter.strLoginParameter[ ( int )eAuthorityLevel ] = strPassword;
				SaveUserInformationParameter();

				objResult = CDefine.enumLoginResult.SUCCESS;
			} while( false );

			return objResult;
		}
	}
}