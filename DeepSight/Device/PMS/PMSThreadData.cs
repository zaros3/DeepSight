using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PMSProcessing
{
    class PMSThreadData
    {
        public Mat m_matLight;
        public List<Mat> m_vImg = new List<Mat>();
        public Mat m_matAlbd;
        public Mat m_matNorm;
        public Mat m_matPImg;
        public Mat m_matQImg;
        public int m_nThresh;
        public int m_nInterv;
        //public int m_nStartLine;
        public Rect m_rtROI;


        public PMSThreadData()
        {
            m_nThresh = 30;
            m_rtROI = new Rect(0, 0, 0, 0);
        }

        public void SetPMSThreadData(Size szImgSize, ref Mat matAlbd, ref Mat matNorm, ref Mat matPImg, ref Mat matQImg)
        {
            m_matAlbd = matAlbd;
            m_matNorm = matNorm;
            m_matPImg = matPImg;
            m_matQImg = matQImg;
        }

        public void SetImages(List<Mat> vImg, Rect rtROI, int nInterv)
        {
            m_vImg    = vImg;
            m_rtROI   = rtROI;
            m_nInterv = nInterv;
        }

        public void SetLight(Mat matLight)
        {
            m_matLight = matLight;
        }

        public void SetROI(Rect rtROI)
        {
            m_rtROI = rtROI;
        }

        public Mat GetAlbedoImage()
        {
            return m_matAlbd;
        }

        public Mat GetPImage()
        {
            return m_matPImg;
        }

        public Mat GetQImage()
        {
            return m_matQImg;
        }
    }
}
