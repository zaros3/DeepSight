using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PMSProcessing
{
    class PMSThreadManager
    {
        PMSThread[] m_ppPMSThread;
        Thread[] m_thdPMS;
        EventWaitHandle[] m_EwhThdEvent;
        bool m_bThreadRun = false;
        int m_nProcessNum;
        int m_nRunningThreadNum;
        PMSMixedModeSharp m_PMSMixed;

        public delegate void ProcessDoneDelegate();
        public event ProcessDoneDelegate ProcessDoneEvent;

        public PMSThreadManager()
        {
            m_ppPMSThread = null;
            m_EwhThdEvent = null;
            m_thdPMS      = null;
        }

        public bool Create(object pParentWnd, PMSMixedModeSharp PMSMix, int nProcessorNum)
        {
            m_nRunningThreadNum = 0;
            m_nProcessNum = nProcessorNum;
            m_ppPMSThread = new PMSThread[nProcessorNum];
            m_thdPMS      = new Thread[nProcessorNum];
            m_EwhThdEvent = new EventWaitHandle[nProcessorNum];
            m_PMSMixed    = (PMSMixedModeSharp)PMSMix;

            for (int i = 0; i < m_nProcessNum; i++)
            {
                m_ppPMSThread[i] = new PMSThread();
                m_EwhThdEvent[i] = new EventWaitHandle(false, EventResetMode.ManualReset, "ThreadEvent." + i.ToString());
                m_thdPMS[i]      = new Thread(ThreadRun);
                m_thdPMS[i].IsBackground = true;
                m_thdPMS[i].Priority     = ThreadPriority.Normal;
                m_bThreadRun             = true;
                m_thdPMS[i].Start(i);
                m_ppPMSThread[i].Init(this, PMSMix, i, 0, 0);
            }
            return true;
        }

        public void Close()
        {
            m_bThreadRun = false;
            for (int i = 0; i < m_nProcessNum; i++)
            {
                if (m_thdPMS[i] != null && m_thdPMS[i].IsAlive)
                {
                    m_ppPMSThread[i].m_bThreadRun = false;
                    m_thdPMS[i].Join(300);
                    m_EwhThdEvent[i].Set();
                }
            }
        }

        public void ThreadRun(object stateIndex)
        {
            int i = Convert.ToInt32(stateIndex);
            while (m_bThreadRun)
            {
                m_EwhThdEvent[i].WaitOne();
                if (false == m_bThreadRun)
                    return;
                m_EwhThdEvent[i].Reset();    //WaitOne이랑 세트
                m_ppPMSThread[i].Run();
                //DeepSight.CDocument.GetDocument.SetUpdateLog( DeepSight.CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "PMSThreadManager : ThreadRun" + i.ToString(), false );
                //               Thread.Sleep(15);
            }
        }

        public void DoStartProcess(int nImgNum, int nImgWid, int nImgHgt)
        {
            if (nImgNum < 3) return;
            if (m_nProcessNum < 1) return;

            int nIntv = nImgHgt / m_nProcessNum;

            for (int i = 0; i < m_nProcessNum - 1; i++)
            {
                m_ppPMSThread[i].Init(this, m_PMSMixed, i, i * nIntv, nIntv);
                m_ppPMSThread[i].Reset();
                m_EwhThdEvent[i].Set();
            }
            //m_ppPMSThread[m_nProcessNum - 1].Init(this, m_PMSMixed, m_nProcessNum - 1, (m_nProcessNum - 1) * nIntv, nImgHgt);
            m_ppPMSThread[ m_nProcessNum - 1 ].Init( this, m_PMSMixed, m_nProcessNum - 1, ( m_nProcessNum - 1 ) * nIntv, nIntv );
            m_ppPMSThread[ m_nProcessNum - 1 ].Reset();
            m_EwhThdEvent[ m_nProcessNum - 1 ].Set();

            m_nRunningThreadNum = m_nProcessNum;
        }

        public void ProcessDone(bool bDone, int nIndex)
        {
            CheckThreadJobDone(nIndex);
            return;
        }

        void CheckThreadJobDone(int nIndex)
        {
            m_ppPMSThread[nIndex].Reset();
            m_nRunningThreadNum--;
           // DeepSight.CDocument.GetDocument.SetUpdateLog( DeepSight.CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "m_nRunningThreadNum : " + m_nRunningThreadNum.ToString(), false );
            try
            {
                if (m_nRunningThreadNum == 0)
                    ProcessDoneEvent();
            }
            catch
            {

            }
        }
    }
}
