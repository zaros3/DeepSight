using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 메모리맵 페이지들이 정의 되어 있는 네임스페이스 입니다.
/// </summary>
namespace ENC.MemoryMap.Pages
{
    using VisionDefine;
    using System.IO.MemoryMappedFiles;

    // [메모리맵 페이지 클래스]
    //
    // 1. MMVisionData_PageBase를 상속 받아 개별 페이지를 정의합니다.
    // 2. 프로퍼티를 정의하여 내부 데이터에 접근이 용이하게 합니다.
    //

    /// <summary>
    /// 개별 셀 데이터를 정의한 클래스
    /// </summary>
    sealed class CMMFPagesVisionData : MMVisionData_PageBase
    {
        public enum enumLanguage { LANGUAGE_KOREA = 0, LANGUAGE_CHINA, LANGUAGE_ENGLISH, LANGUAGE_VIETNAM, LANGUAGE_FINAL = 10 };
        /// <summary>
        /// Vision 데이터 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="pageIndex">할당 할 페이지 인덱스</param>
        /// <param name="pageCount">메모리 맵에 모든 페이지 개수</param>
        public CMMFPagesVisionData( MemoryMappedFile memMap, uint pageIndex, uint pageCount )
            : base( memMap, pageIndex, pageCount )
        {
            objMasterPositionX = new CMasterPositionX( this );
            objMasterPositionY = new CMasterPositionY( this );
            objMasterPositionAngle = new CMasterPositionAngle( this );
        }

        /// <summary>
        /// 기본값을 씁니다.
        /// </summary>
        public void DefaultValue()
        {
            // 내부 데이터를 클리어 한다.
            ByteData.Clear();
            BoolData.Clear();
            IntData.Clear();
            FloatData.Clear();
            DoubleData.Clear();
            StringData.Clear();
        }

//        public void SetVisionData( DeepSight.CInspectionResult.CResult objVisionData )
//        {
//            int iBoolDataIndex = 0;
//            int iDoubleDataIndex = 0;
//            int iStringDataIndex = 0;

//            try {
////                 System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
////                 byte[] byteImageData = ( byte[] )converter.ConvertTo( objVisionData.objInputImage, typeof( byte[] ) );
////                 ByteData.Copy( byteImageData );
//                iImageWidth = objVisionData.objInputImage.Width;
//                iImageHeight = objVisionData.objInputImage.Height;

//            } catch( System.Exception ex ) {
//                System.Diagnostics.Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
//            }

//            BoolData[ iBoolDataIndex ] = objVisionData.bResult;
//            StringData[ iStringDataIndex++ ] = objVisionData.strEventTime;
//            StringData[ iStringDataIndex++ ] = objVisionData.strTactTime;
//            for( int iLoopCount = 0; iLoopCount < ( int )DeepSight.CDefine.enumInspectPosition.INSPECT_POSITION_FIANL; iLoopCount++ ) {
//                DoubleData[ iDoubleDataIndex++ ] = objVisionData.dPositionX[ iLoopCount ];
//                DoubleData[ iDoubleDataIndex++ ] = objVisionData.dPositionY[ iLoopCount ];
//                DoubleData[ iDoubleDataIndex++ ] = objVisionData.dPositionAngle[ iLoopCount ];
//                DoubleData[ iDoubleDataIndex++ ] = objVisionData.dScore[ iLoopCount ];
//            }
                
//        }

//        public void GetVisionData( out DeepSight.CInspectionResult.CResult objVisionData )
//        {
//            int iBoolDataIndex = 0;
//            int iDoubleDataIndex = 0;
//            int iStringDataIndex = 0;

//            objVisionData = new DeepSight.CInspectionResult.CResult();

//            try {
////                 if( 0 < iImageWidth && 0 < iImageHeight ) {
////                     byte[] byteImageData = ( byte[] )ByteData.ToBytes( iImageWidth * iImageHeight );
////                     System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
////                     objVisionData.objInputImage = ( System.Drawing.Bitmap )converter.ConvertFrom( byteImageData );
////                 }

//            } catch( System.Exception ex ) {
//                System.Diagnostics.Trace.WriteLine( ex.Message + "->" + ex.StackTrace );
//            }

//            objVisionData.bResult = BoolData[ iBoolDataIndex ];
//            objVisionData.strEventTime = StringData[ iStringDataIndex++ ];
//            objVisionData.strTactTime = StringData[ iStringDataIndex++ ];
//            for( int iLoopCount = 0; iLoopCount < ( int )DeepSight.CDefine.enumInspectPosition.INSPECT_POSITION_FIANL; iLoopCount++ ) {
//                objVisionData.dPositionX[ iLoopCount ] = DoubleData[ iDoubleDataIndex++ ];
//                objVisionData.dPositionY[ iLoopCount ] = DoubleData[ iDoubleDataIndex++ ];
//                objVisionData.dPositionAngle[ iLoopCount ] = DoubleData[ iDoubleDataIndex++ ];
//                objVisionData.dScore[ iLoopCount ] = DoubleData[ iDoubleDataIndex++ ];
//            }
//        }
        #region VisionData 에 접근 할 수 있는 프로퍼티를 정의합니다.

        // 트리거 확인 및 세팅
        public bool bTrigger
        {
            get { return BoolData[ 100 ]; }
            set { BoolData[ 100 ] = value; }
        }

        // 트리거 확인 및 세팅
        public bool bLive
        {
            get { return BoolData[ 101 ]; }
            set { BoolData[ 101 ] = value; }
        }

        // 조명 On/Off
        public bool bLightOn
        {
            get { return BoolData[ 102 ]; }
            set { BoolData[ 102 ] = value; }
        }

        // 라이브 화면 On/Off
        public bool bShowDialogVisionLive
        {
            get { return BoolData[ 103 ]; }
            set { BoolData[ 103 ] = value; }
        }

        // 제어프로그램에서 마스터 포지션 저장 비트
        public bool bSaveMasterPosition
        {
            get { return BoolData[ 104 ]; }
            set { BoolData[ 104 ] = value; }
        }

        // 로그인 요청 비트
        public bool bLogInRequest
        {
            get { return BoolData[ 105 ]; }
            set { BoolData[ 105 ] = value; }
        }

        // 로그인 응답 비트
        public bool bLogInReply
        {
            get { return BoolData[ 106 ]; }
            set { BoolData[ 106 ] = value; }
        }

        // 카메라 연결상태
        public bool bCameraConnected
        {
            get { return BoolData[ 107 ]; }
            set { BoolData[ 107 ] = value; }
        }

        // 비전프로그램 Heart Beat
        public bool bHeartBeat
        {
            get { return BoolData[ 108 ]; }
            set { BoolData[ 108 ] = value; }
        }

        public bool bHeartBeatPLC
        {
            get { return BoolData[ 109 ]; }
            set { BoolData[ 109 ] = value; }
        }

        public int iErrorCode
        {
            get { return IntData[ 100 ]; }
            set { IntData[ 100 ] = value; }
        }

        public int iImageWidth
        {
            get { return IntData[ 101 ]; }
            set { IntData[ 101 ] = value; }
        }

        public int iImageHeight
        {
            get { return IntData[ 102 ]; }
            set { IntData[ 102 ] = value; }
        }

        // 로그인 결과
        public int iLogInResult
        {
            get { return IntData[ 103 ]; }
            set { IntData[ 103 ] = value; }
        }

        // 권한 레벨
        public int iAuthorityLevel
        {
            get { return IntData[ 104 ]; }
            set { IntData[ 104 ] = value; }
        }

        // 프로그램 종료상태
        public int iProgramExitStatus
        {
            get { return IntData[ 105 ]; }
            set { IntData[ 105 ] = value; }
        }

        // 생산수량
        public int iMaterialCountOK
        {
            get { return IntData[ 106 ]; }
            set { IntData[ 106 ] = value; }
        }

        public int iMaterialCountNG
        {
            get { return IntData[ 107 ]; }
            set { IntData[ 107 ] = value; }
        }

        // 파인드 코너 마스터 포지션 X
        public double dMasterPositionFindCornerX
        {
            get { return DoubleData[ 100 ]; }
            set { DoubleData[ 100 ] = value; }
        }
        // 파인드 코너 마스터 포지션 Y
        public double dMasterPositionFindCornerY
        {
            get { return DoubleData[ 101 ]; }
            set { DoubleData[ 101 ] = value; }
        }
        // 파인드 코너 마스터 포지션 Angle
        public double dMasterPositionFindCornerAngle
        {
            get { return DoubleData[ 102 ]; }
            set { DoubleData[ 102 ] = value; }
        }

        public class CMasterPositionX
        {
            CMMFPagesVisionData m_objVisionData = null;
            public CMasterPositionX( CMMFPagesVisionData objVisionData )
            {
                m_objVisionData = objVisionData;
            }

            public double this[ int iIndex ]
            {
                get { return m_objVisionData.DoubleData[ 104 + iIndex ]; }
                set { m_objVisionData.DoubleData[ 104 + iIndex ] = value; }
            }
        }
        public CMasterPositionX objMasterPositionX;

        public class CMasterPositionY
        {
            CMMFPagesVisionData m_objVisionData = null;
            public CMasterPositionY( CMMFPagesVisionData objVisionData )
            {
                m_objVisionData = objVisionData;
            }

            public double this[ int iIndex ]
            {
                get { return m_objVisionData.DoubleData[ 106 + iIndex ]; }
                set { m_objVisionData.DoubleData[ 106 + iIndex ] = value; }
            }
        }
        public CMasterPositionY objMasterPositionY;

        public class CMasterPositionAngle
        {
            CMMFPagesVisionData m_objVisionData = null;
            public CMasterPositionAngle( CMMFPagesVisionData objVisionData )
            {
                m_objVisionData = objVisionData;
            }

            public double this[ int iIndex ]
            {
                get { return m_objVisionData.DoubleData[ 108 + iIndex ]; }
                set { m_objVisionData.DoubleData[ 108 + iIndex ] = value; }
            }
        }
        public CMasterPositionAngle objMasterPositionAngle;

        public string strMachineRecipeID
        {
            get { return StringData[ 100 ]; }
            set { StringData[ 100 ] = value; }
        }

        public string strMachineRecipeName
        {
            get { return StringData[ 101 ]; }
            set { StringData[ 101 ] = value; }
        }

        public string strVisionRecipePPID
        {
            get { return StringData[ 102 ]; }
            set { StringData[ 102 ] = value; }
        }
        
        public string strVisionRecipeName
        {
            get { return StringData[ 103 ]; }
            set { StringData[ 103 ] = value; }
        }

        // 로그인 아이디
        public string strLoginID
        {
            get { return StringData[ 104 ]; }
            set { StringData[ 104 ] = value; }
        }

        // 로그인 패스워드
        public string strLoginPassword
        {
            get { return StringData[ 105 ]; }
            set { StringData[ 105 ] = value; }
        }

        // 로그인 이름
        public string strLoginName
        {
            get { return StringData[ 106 ]; }
            set { StringData[ 106 ] = value; }
        }

        #endregion
    }
}
