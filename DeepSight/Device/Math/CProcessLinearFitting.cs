using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
	public class CProcessLinearFitting : CProcessAbstract
	{
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public property
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const int DEF_LINE_PARAMS_NUM = 2;
        public class CSLine 
        {
            private double a, b;
            public CSLine( double dA, double dB )
            {
                a = dA;
                b = dB;
            }

            public double Angle()
            {
                return Math.Atan2( a, 1.0 );
            }
        }

        public struct structurePoint 
        {
            public double dX;
            public double dY;
        }

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//private property
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 생성자
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CProcessLinearFitting()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 소멸자
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		~CProcessLinearFitting()
		{
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//생성 : 
		//추가 : 
		//목적 : 초기화 함수
		//설명 : 
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public override bool Initialize()
		{
			bool bReturn = false;

			do {
				var pDocument = CDocument.GetDocument;
                pDocument.SetUpdateLog( CDefine.enumLogType.LOG_SYSTEM, "CProcessLinearFitting Initialize Start" );


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
		public override void DeInitialize()
		{
        }

     /*   public double RansacLineFitting( List<Point2f> objListPoint, ref CSLine objLineModel, double dDistanceThreshold )
        {
            double dReturn = 0;
            do {
                if( null == objListPoint ) break;
                int iDataNum = objListPoint.Count;
                if( iDataNum < DEF_LINE_PARAMS_NUM ) break;

                List<structurePoint> objSample = new List<structurePoint>();
                List<structurePoint> objInLidr = new List<structurePoint>();

                int nInLierNum = 0;
                CSLine EstimateModel;
                double dMaxCost = 0.0;
                double dInlierRate = 0.8; //80% Inlier
                int nMaxIter = ( int )( 1.0 + Math.Log( 1.0 - 0.99 ) / Math.Log( 1.0 - Math.Pow( dInlierRate, DEF_LINE_PARAMS_NUM ) ) );

                for( int i = 0; i < nMaxIter; i++ ) {
                    // 1. hypothesis
                    // 원본 데이터에서 임의로 N개의 Sample data를 고른다.
                    GetSample( vSample, LINE_PARAMS_NUM, vData );
                    //모델 파라메터 예측
                    ComputeModelParams( vSample, LINE_PARAMS_NUM, EstimateModel );

                    // 2. Verification
                    // 원본 데이터가 예측된 모델에 잘 맞는지 검사.
                    double dCost = ModelVerification( vInLier, &nInLierNum, EstimateModel, vData, dDistanceThresh );

                    // 만일 예측된 모델이 잘 맞는다면, 이 모델에 대한 유효한 데이터로 새로운 모델을 구한다.
                    if( dMaxCost < dCost ) {
                        dMaxCost = dCost;
                        ComputeModelParams( vInLier, nInLierNum, model );
                    }
                }



            } while( false );

            return dReturn;
        }

        void GetSample( ref List<Point2f> objSamples, int nSampleNum, List<Point2f> objData )
        {
            do {
                int nDataNum = objData.Count;
                if( nDataNum < nSampleNum ) break;

                Random rand = new Random();
                int nCnt = 0;
                objSamples.clear();
                // 데이터에서 중복되지 않게 N개의 무작위 Sample을 채취한다.
                for( int i = 0; i < nDataNum; i++ ) {
                    int n = rand.Next() % nDataNum;
                    if( !FindInSamples( vSamples, nCnt, &vData[ n ] ) ) {
                        vSamples.push_back( vData[ n ] );
                        nCnt++;
                        if( nCnt == nSampleNum ) break;
                    }
                }

            } while( false );

        }

        public bool FindInSamples( List<Point2f> objSamples, int nCurSampleNo, Point2f objData )
        {
             bool bReturn = false;
                do {
                    for( int i = 0; i < nCurSampleNo; ++i ) {
                        if( objSamples[ i ].X == objData.X && objSamples[ i ].Y == objData.Y ) {
                            bReturn = true;
                            break;
                        }
                    }

                } while( false );

            return bReturn;
        }

        public bool ComputeModelParams( List<Point2f> objSamples, int nInLierNum, ref CSLine Model)
        {
            bool bReturn = false;
            do {
                if( nInLierNum < DEF_LINE_PARAMS_NUM || objSamples.Count < DEF_LINE_PARAMS_NUM ) break;

                // Linear Equation : ax + by + c = z

                Mat A = new Mat( DEF_LINE_PARAMS_NUM, DEF_LINE_PARAMS_NUM, MatType.CV_64FC1, Scalar.All( 0.0 ) );
                Mat B = new Mat( DEF_LINE_PARAMS_NUM, 1, MatType.CV_64FC1, Scalar.All( 0.0 ) );

                IntPtr pA = A.Data;
                IntPtr pB = B.Data;
                for( int i = 0; i < nInLierNum; i++ ) {
                    double x = objSamples[ i ].X;
                    double y = objSamples[ i ].Y;

                    //Marshal.Copy( pA, 데이터형식?, 0, size );
                    double a = A.At<double>( i );
                    A.Set<double>( i, x * x );
                    pA += ( x * x );
                    ( pA + 1 ) += x;
                    ( pA + 2 ) += x;
                    ( pA + 3 ) += 1.0;

                    *( pB ) += x * y;
                    *( pB + 1 ) += y;
                }

                // AX=B 형태의 해를 least squares solution 구하기 위해
                // Moore-Penrose pseudo-inverse를 이용한다.;
                Mat X = A.Inv() * B;
                double* pX = ( double* )X.data;

                Model.a = *pX;
                Model.b = *( pX + 1 );

                bReturn = true;
            } while( false );

            return bReturn;
        }

        double ComputeDistance( ref CSLine sModel, ref List<structurePoint> p )
        {
            float&x = p.x;
            float&y = p.y;
            

            double e = fabs( sModel.a * x + sModel.b - y );
            return sqrt( e );
        }

        double LinearFitting::ModelVerification( vector<cv::Point2f>& vInLiers, int* nInLierNum, sLine& EstimatedModel, vector<cv::Point2f> vData, double dDistanceThresh )
        {
            int nCnt = 0;
            double dCost = 0.0;
            for( size_t i = 0; i < vData.size(); i++ ) {
                double dDistance = ComputeDistance( EstimatedModel, vData[ i ] );
                if( dDistance < dDistanceThresh ) {
                    dCost += 1.0;
                    vInLiers[ nCnt ] = vData[ i ];
                    nCnt++;
                }
            }
            *nInLierNum = nCnt;
            return dCost;
        }*/









    }
}