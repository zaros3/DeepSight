using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DeepSight
{
    class CoverWeldingDll
    {

#if DEBUG
        [DllImport("CWeldingToolLib.dll")]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        unsafe public static extern double DoCoverWeldingFitLinear(int[] pPositions, float[] pHeight, int nDataNum, double dDistanceThresh, double dInlierRate);

#if DEBUG
        [DllImport("CWeldingToolLib.dll")]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern double GetCoverWeldingTilt();

#if DEBUG
        [DllImport("CWeldingToolLib.dll")]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern double GetCoverWeldingBias();


        unsafe public void DoCoverWeldingPlaneFit(int[] pPositions, float[] pHeight, int nDataNum, double dDistanceThresh, double dInlierRate)
        {
            DoCoverWeldingFitLinear(pPositions, pHeight, nDataNum, dDistanceThresh, dInlierRate);
        }

        public double GetCoverWeldingPlaneTilt()
        {
            return GetCoverWeldingTilt();
        }
        public double GetCoverWeldingPlaneBias()
        {
            return GetCoverWeldingBias();
        }
    }
}
