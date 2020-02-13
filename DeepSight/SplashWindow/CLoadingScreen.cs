using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
	public enum TypeOfMessage
	{
		Success = 0,
		Warning,
		Error,
	}

	public static class CLoadingScreen
	{
		static CDialogLoadingWindow m_objDialogLoadingWindow = null;
		static private string m_strProgramName = "DEEPSIGHT Vision Program";
		static private string m_strVersion = "DEEPSIGHT.9.5.19.11.2.3";

		public static void ShowSplashScreen()
		{
			if( null == m_objDialogLoadingWindow ) {
				m_objDialogLoadingWindow = new CDialogLoadingWindow( m_strProgramName, m_strVersion );
				m_objDialogLoadingWindow.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
				m_objDialogLoadingWindow.ShowSplashScreen();
			}
		}

		public static void CloseSplashScreen()
		{
			if( null != m_objDialogLoadingWindow ) {
				m_objDialogLoadingWindow.CloseSplashScreen();
				m_objDialogLoadingWindow = null;
			}
		}

		public static bool IsSplashScreen()
		{
			bool bReturn = false;

			do {
				if( null != m_objDialogLoadingWindow ) {
					bReturn = true;
				}
				else {
					bReturn = false;
				}

			} while( false );

			return bReturn;
		}

		public static void UpdateStatusText( int iIndex, string Text )
		{
			if( null != m_objDialogLoadingWindow ) {
				m_objDialogLoadingWindow.UpdateStatusText( iIndex, Text );
			}
		}

		public static void UpdateStatusTextWithStatus( int iIndex, string Text, TypeOfMessage objTypeOfMessage )
		{
			if( null != m_objDialogLoadingWindow ) {
				m_objDialogLoadingWindow.UpdateStatusTextWithStatus( iIndex, Text, objTypeOfMessage );
			}
		}

		public static int GetPrograssPoint()
		{
            int iPrograssPoint = 0;
            try
            {
                iPrograssPoint = m_objDialogLoadingWindow.GetPrograssPoint();
            }
            catch ( Exception )
            {
                iPrograssPoint = 0;
            }
            

            return iPrograssPoint;
		}
	}
}
