using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace PMSProcessing {
    public partial class PMSMixedModeSharp {
        [DllImport( "kernel32.dll", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )] public static extern bool AllocConsole();

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif

        public static extern void SetLightInformation( int nLightNum, float[] vTilt, float[] vSlant );

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        unsafe public static extern void AddSourceImage( int nWidth, int nHeight, byte* pImg );

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        public static extern void SetParameters( int nImgWid, int nImgHgt, int nThresh );

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        public static extern bool ProcessNormAndAlbedoThread( int nStartRow, int nInterv );

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        unsafe public static extern IntPtr GetAlbedoImage();

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        unsafe public static extern IntPtr GetNormImage();

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        unsafe public static extern IntPtr GetPImage();

#if DEBUG
        [DllImport( "PhotometricStereo.dll" )]
#else
        [DllImport( "PhotometricStereo.dll" )]
#endif
        unsafe public static extern IntPtr GetQImage();

        public void SetLight( int nLightNum, float[] vTilt, float[] vSlant )
        {
            SetLightInformation( nLightNum, vTilt, vSlant );
        }

        unsafe public void SetMatImages( int nWidth, int nHeight, int nImgNum, Mat[] vImg )
        {
            //DeepSight.CDocument.GetDocument.SetUpdateLog(DeepSight.CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "SetMatImages", false);
            for( int i = 0; i < nImgNum; i++ ) {
                byte* pImg = vImg[ i ].DataPointer;
                AddSourceImage( nWidth, nHeight, pImg );
            }
        }

        //--------------------- 추가 --------------------------------------------------------//
        unsafe public void SetMatListImages( int nWidth, int nHeight, int nImgNum, List<Mat> vImg )
        {
            for( int i = 0; i < nImgNum; i++ ) {
                byte* pImg = vImg[ i ].DataPointer;
                AddSourceImage( nWidth, nHeight, pImg );
            }
        }

        unsafe public void SetPMSParameters( int nWidth, int nHeight, int nThresh )
        {
            //DeepSight.CDocument.GetDocument.SetUpdateLog(DeepSight.CDefine.enumLogType.LOG_VISION_EXCEPTION_CAMERA_0, "SetPMSParameters", false);
            SetParameters( nWidth, nHeight, nThresh );
        }

        unsafe public IntPtr GetAledoImagePMS()
        {
            return GetAlbedoImage();
        }

        unsafe public IntPtr GetNormImagePMS()
        {
            return GetNormImage();
        }

        unsafe public IntPtr GetPImagePMS()
        {
            return GetPImage();
        }

        unsafe public IntPtr GetQImagePMS()
        {
            return GetQImage();
        }

        public bool ProcessingPMSThread( int nStartRow, int nInterv )
        {
            return ProcessNormAndAlbedoThread( nStartRow, nInterv );
        }
    }  
}
