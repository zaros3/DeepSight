using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
	public class CInterfacePLC
	{
		public bool[] bInterfacePlcBitIn;
		public bool[] bInterfacePlcBitOut;
		public short[] sInterfacePlcWordIn;
		public short[] sInterfacePlcWordOut;
		public double[] dInterfacePlcDWordIn;
		public double[] dInterfacePlcDWordOut;
		public CWordToBit[] objWordToBitIn;
		public CWordToBit[] objWordToBitOut;

		public class CWordToBit : ICloneable
		{
			public int iWordToBitSize = 16;
			public bool[] bWordToBit = new bool[ 16 ];
			public string[] strBitName = new string[ 16 ];
			public object Clone()
			{
				CWordToBit objWordToBit = new CWordToBit();
				objWordToBit.iWordToBitSize = this.iWordToBitSize;
				objWordToBit.bWordToBit = this.bWordToBit.Clone() as bool[];
				objWordToBit.strBitName = this.strBitName.Clone() as string[];
				return objWordToBit;
			}
		}

		public class CInitializeParameter : ICloneable
		{
			public int iCountBitIn;
			public int iCountBitOut;
			public int iCountWordIn;
			public int iCountWordOut;
			public int iCountDWordIn;
			public int iCountDWordOut;

			public object Clone()
			{
				CInitializeParameter objInitializeParameter = new CInitializeParameter();
				objInitializeParameter.iCountBitIn = this.iCountBitIn;
				objInitializeParameter.iCountBitOut = this.iCountBitOut;
				objInitializeParameter.iCountWordIn = this.iCountWordIn;
				objInitializeParameter.iCountWordOut = this.iCountWordOut;
				objInitializeParameter.iCountDWordIn = this.iCountDWordIn;
				objInitializeParameter.iCountDWordOut = this.iCountDWordOut;

				return objInitializeParameter;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 생성자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public CInterfacePLC()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소멸자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		~CInterfacePLC()
		{
		}

		public bool Initialize( CInitializeParameter objInitializeParameter )
		{
			bool bReturn = false;

			do {
				bInterfacePlcBitIn = new bool[ objInitializeParameter.iCountBitIn ];
				bInterfacePlcBitOut = new bool[ objInitializeParameter.iCountBitOut ];
				sInterfacePlcWordIn = new short[ objInitializeParameter.iCountWordIn ];
				sInterfacePlcWordOut = new short[ objInitializeParameter.iCountWordOut ];
				dInterfacePlcDWordIn = new double[ objInitializeParameter.iCountDWordIn ];
				dInterfacePlcDWordOut = new double[ objInitializeParameter.iCountDWordOut ];
				objWordToBitIn = new CWordToBit[ objInitializeParameter.iCountWordIn ];
				objWordToBitOut = new CWordToBit[ objInitializeParameter.iCountWordOut ];

				for( int iLoopCount = 0; iLoopCount < objInitializeParameter.iCountWordIn; iLoopCount++ ) {
					objWordToBitIn[ iLoopCount ] = new CWordToBit();
				}
				for( int iLoopCount = 0; iLoopCount < objInitializeParameter.iCountWordOut; iLoopCount++ )
					objWordToBitOut[ iLoopCount ] = new CWordToBit();

				bReturn = true;
			} while( false );

			return bReturn;
		}

	}
}