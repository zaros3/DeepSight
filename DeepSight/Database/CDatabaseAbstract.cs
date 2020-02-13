﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DeepSight
{
	public class CDatabaseAbstract
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//protected property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 프로세스 종료
		protected bool m_bThreadExit;
		// 쓰레드
		protected Thread m_ThreadProcess;
		// 프로세스 데이터베이스
		protected CProcessDatabase m_objProcessDatabase;
	}
}