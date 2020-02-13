using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 메모리맵 페이지 관리 매니져들이 있는 네임스페이스 입니다.
/// </summary>
namespace ENC.MemoryMap.Manager
{
    using VisionDefine;
    using Pages;
    using System.IO.MemoryMappedFiles;

    // [메모리맵 관리 매니져 클래스]
    //
    // 1. 메모리 맵 관리 매니져 클래스는 싱글톤 패턴을 사용하여 작성합니다.
    // 2. 파일 하나당 하나의 관리 클래스를 만들어 사용합니다. 
    // 3. 하나의 메모리 맵에는 다수의 페이지가 존재 할 수 있습니다.
    // 4. 맵 파일을 생성 할 때 기존 값이 존재하지 않으면 초기값을 써줍니다.
    //

    /// <summary>
    /// 싱글톤 패턴의 메모리 맵 데이터 매니져 클래스 입니다.
    /// </summary>
    sealed class CMMFManagerVisionData
    {
        public enum enumPage
        {   
			VISION_POL_A_ALIGN_1,
			VISION_POL_A_ALIGN_2,
            VISION_POL_A_ALIGN_3,
            VISION_POL_A_ALIGN_4,
            VISION_POL_B_ALIGN_1,
            VISION_POL_B_ALIGN_2,
            VISION_POL_B_ALIGN_3,
            VISION_POL_B_ALIGN_4,
            VISION_PANEL_ALIGN_1,
            VISION_PANEL_ALIGN_2,
			VISION_POL_A_INSPECTION_1,
			VISION_POL_A_INSPECTION_2,
			VISION_POL_A_INSPECTION_3,
			VISION_POL_A_INSPECTION_4,
            VISION_POL_B_INSPECTION_1,
            VISION_POL_B_INSPECTION_2,
            VISION_POL_B_INSPECTION_3,
            VISION_POL_B_INSPECTION_4, 
			PAGE_FINAL
        };

        /// <summary>
        /// 메모리 맵 파일
        /// </summary>
        private MemoryMappedFile _memMap;
        /// <summary>
        /// 셀 데이터 페이지
        /// </summary>
        private MMVisionData_PageBase[] _VisionDataPages;
        /// <summary>
        /// 싱글톤 패턴 인스턴스
        /// </summary>
        private static CMMFManagerVisionData _instance = null;
        /// <summary>
        /// 멀티 쓰레드 환경에서 인스턴스를 생성 할 때 중복 생성 되지 않도록 락을 걸어준다.
        /// </summary>
        private static object _instanceCreateLock = new object();

        /// <summary>
        /// 셀 데이터 매니져 생성자
        /// </summary>
        private CMMFManagerVisionData()
        {
            // 기존에 파일이 존재하는지 확인 한다.
            var memMapFileName = "MEMData/MemoryMappedFileVisionData.bin";
            bool firstCreateFile = false;
            if( false == System.IO.File.Exists( memMapFileName ) )
            {
                firstCreateFile = true;
            }

            // 메모리 맵 클래스를 생성합니다.
            _memMap = MMVisionData_PageBase.CreateMemMap( fileName: memMapFileName, mapName: memMapFileName, pageCount: ( uint )enumPage.PAGE_FINAL );
            // 메모리 블럭의 데이터 뷰를 생성합니다.
            _VisionDataPages = new CMMFPagesVisionData[ ( int )enumPage.PAGE_FINAL ];
            for( int iLoopCount = 0; iLoopCount < ( int )enumPage.PAGE_FINAL; iLoopCount++ )
            {
                _VisionDataPages[ iLoopCount ] = new CMMFPagesVisionData( memMap: _memMap, pageIndex: ( uint )iLoopCount, pageCount: ( uint )enumPage.PAGE_FINAL );
            }

            // 파일이 존재 하지 않으면 기본값을 써 넣는다.
            if( true == firstCreateFile )
            {
                DefaultValue();
            }
        }

        /// <summary>
        /// 싱글톤 패턴 인스턴스를 불러온다.
        /// </summary>
        public static CMMFManagerVisionData Instance
        {
            get
            {
                if( null == _instance )
                {
                    // 만약 인스턴스를 새로 생성해야 하는 상황이면 중복 생성을 막기 위해 락을 걸고 다시 확인한 뒤 생성한다.
                    lock( _instanceCreateLock )
                    {
                        if( null == _instance )
                        {
                            _instance = new CMMFManagerVisionData();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 파일을 처음 만들었을 때 기본값을 써준다.
        /// </summary>
        private void DefaultValue()
        {
            for( int iLoopCount = 0; iLoopCount < ( int )enumPage.PAGE_FINAL; iLoopCount++ )
            {
                ( ( CMMFPagesVisionData )_VisionDataPages[ iLoopCount ] ).DefaultValue();
            }
        }

        /// <summary>
        /// 페이지 접근 인덱서
        /// </summary>
        /// <param name="iIndex">페이지 번호</param>
        /// <returns></returns>
        public CMMFPagesVisionData this[ int iIndex ]
        {
            get { return _VisionDataPages[ iIndex ] as CMMFPagesVisionData; }
        }

        /// <summary>
        /// 페이지 접근 인덱서
        /// </summary>
        /// <param name="ePage">enumPage</param>
        /// <returns></returns>
        public CMMFPagesVisionData this[ enumPage ePage ]
        {
            get { return _VisionDataPages[ ( int )ePage ] as CMMFPagesVisionData; }
        }

        #region VisionData 에 접근 할 수 있는 프로퍼티를 정의합니다.


        #endregion
    }
}
