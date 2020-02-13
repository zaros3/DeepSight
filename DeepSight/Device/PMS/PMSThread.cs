using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PMSProcessing
{
    class PMSThread
    {
        public const int PMS_THREAD_STATE_IDLE              = 1;
        public const int PMS_THREAD_STATE_READY             = 2;
        public const int PMS_THREAD_STATE_READY_TO_PROCESS  = 3;
        public const int PMS_THREAD_STATE_IN_CONVERTING     = 4;
        public const int PMS_THREAD_STATE_DONE              = 5;

        public const int WM_USER                            = 0x0400;
        public const int WM_MESSAGE_PMS_THREAD_DONE         = WM_USER + 10001;

        public bool m_bThreadRun = true;
        int m_nThreadIndex;
        int m_nState;
        object m_pParent;

        PMSMixedModeSharp m_PMSProcess;

        int m_nStartLine = 0;
        int m_nInterv    = 200;

        public PMSThread()
        {
            m_pParent       = null;
            m_nState        = PMS_THREAD_STATE_IDLE;
            m_nThreadIndex  = -1;
            m_PMSProcess    = null;
        }

        public int Run()
        {
            if (IsReady())
            {
                m_nState = PMS_THREAD_STATE_IN_CONVERTING;
               // DeepSight.CDocument.GetDocument.SetUpdateLog( DeepSight.CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "PMSProcessing - Run() : " + m_nThreadIndex.ToString(), false );
                CalculatePMS();
                m_nState = PMS_THREAD_STATE_DONE;
                ((PMSThreadManager)m_pParent).ProcessDone(true, m_nThreadIndex);
            }
            return 0;
        }

        public void Init(object pParent, PMSMixedModeSharp PMSMix, int nThreadIdx, int nStartLine, int nIntv)
        {
            m_pParent      = pParent;
            m_nThreadIndex = nThreadIdx;
            m_nState       = PMS_THREAD_STATE_READY;
            m_nStartLine   = nStartLine;
            m_nInterv      = nIntv;
            m_PMSProcess   = PMSMix;
        }

        unsafe void CalculatePMS()
        {
            m_PMSProcess.ProcessingPMSThread(m_nStartLine, m_nInterv);
        }


        public bool IsReady()
        {
            if (m_nState == PMS_THREAD_STATE_READY)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            m_nState = PMS_THREAD_STATE_READY;
        }
    }
}
