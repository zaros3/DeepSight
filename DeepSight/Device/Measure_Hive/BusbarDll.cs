using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SkinLeadWelding
{
    public class BusbarDll
    {

        // #if DEBUG
        //         [DllImport("CWeldingToolLibD.dll")]
        // #else
        //         [DllImport("CWeldingToolLib.dll")]
        // #endif
        //         public static extern void SetParameters(int nThresh, int nMaskLen, int nMeasureInterv, int nCaliperLen, int nSkipLenFromCenter);

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        unsafe public static extern void DoMeasureWelding(int nWidth, int nHeight, byte* pPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, byte* pPMS3, int nTBThresh, int nTBContinousLen);

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetBusbarWidth();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetBusbarHeight();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetBusbarLeft();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetBusbarRight();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetBusbarTop();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetBusbarBottom();

        unsafe public void DoBusbarMeasureWelding(int nWidth, int nHeight, byte* pPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, byte* pPMS3, int nTBThresh, int nTBContinousLen)
        {
            DoMeasureWelding(nWidth, nHeight, pPMS1, nLRLowThresh, nLRContinousLen, nLRHighThresh, pPMS3, nTBThresh, nTBContinousLen);
        }

        public int GetBusbarMeasuredWidth()
        {
            return GetBusbarWidth();
        }
        public int GetBusbarMeasuredHeight()
        {
            return GetBusbarHeight();
        }

        public int GetBusbarMeasuredLeft()
        {
            return GetBusbarLeft();
        }

        public int GetBusbarMeasuredRight()
        {
            return GetBusbarRight();
        }

        public int GetBusbarMeasuredTop()
        {
            return GetBusbarTop();
        }

        public int GetBusbarMeasuredBottom()
        {
            return GetBusbarBottom();
        }

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        unsafe public static extern void DoMeasureSensor(byte* pData, int nImgWid, int nImgHgt, int nSensorStdWid, int nSensorStdHgt, int nThresh);

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetSensorWidth();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetSensorHeight();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetSensorTop();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetSensorLeft();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetSensorRight();

#if DEBUG
        [DllImport( "CWeldingToolLib.dll" )]
#else
        [DllImport("CWeldingToolLib.dll")]
#endif
        public static extern int GetSensorBottom();

        unsafe public void DoSensorMeasure(int nWidth, int nHeight, byte* pImg, int nSensorStdWid, int nSensorStdHgt, int nThresh)
        {
            DoMeasureSensor(pImg, nWidth, nHeight, nSensorStdWid, nSensorStdHgt, nThresh);
        }

        public int GetSensorMeasuredWidth()
        {
            return GetSensorWidth();
        }
        public int GetSensorMeasuredHeight()
        {
            return GetSensorHeight();
        }

        public int GetSensorMeasuredLeft()
        {
            return GetSensorLeft();
        }

        public int GetSensorMeasuredRight()
        {
            return GetSensorRight();
        }

        public int GetSensorMeasuredTop()
        {
            return GetSensorTop();
        }

        public int GetSensorMeasuredBottom()
        {
            return GetSensorBottom();
        }
    }
}
