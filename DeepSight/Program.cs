using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DeepSight
{
    static class Program
    {
		private static void ApplicationStart()
		{
			// 중복 실행 방지
			bool bDupliacte = false;
			Mutex hMutex = new Mutex( true, "DeepSight", out bDupliacte );

			// 뮤텍스가 생성되면 최초 한번 실행
			if( true == bDupliacte ) {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault( false );
				Application.Run( new CMainFrame() );
			}
			else {
				// 뮤텍스 생성 실패되면 이미 프로세스 상주 중
				MessageBox.Show( "Already DeepSight.exe Running..." );
			}
		}

		private static Assembly ResolveAssembly( object sender, ResolveEventArgs args )
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			string name = args.Name.Substring( 0, args.Name.IndexOf( ',' ) ) + ".dll";
			List<string> resources = thisAssembly.GetManifestResourceNames().Where( s => s.EndsWith( name ) ).ToList();
			if( !resources.Any() ) return null;
			string resourceName = resources.First();
			using( Stream stream = thisAssembly.GetManifestResourceStream( resourceName ) ) {
				if( stream == null ) return null;
				var block = new byte[ stream.Length ];
				stream.Read( block, 0, block.Length );
				return Assembly.Load( block );
			}
		}

		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
            ThreadPool.SetMaxThreads( 300, 300 );
            ThreadPool.SetMinThreads( 200, 200 );
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
			ApplicationStart();
		}
    }
}