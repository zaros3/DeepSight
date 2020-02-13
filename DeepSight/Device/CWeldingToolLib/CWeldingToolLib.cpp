// CWeldingToolLib.cpp : DLL의 초기화 루틴을 정의합니다.
//

#include "pch.h"
#include "framework.h"
#include "CWeldingToolLib.h"
#include "BusbarProcess.h"
#include "SensingMeasure.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//
//TODO: 이 DLL이 MFC DLL에 대해 동적으로 링크되어 있는 경우
//		MFC로 호출되는 이 DLL에서 내보내지는 모든 함수의
//		시작 부분에 AFX_MANAGE_STATE 매크로가
//		들어 있어야 합니다.
//
//		예:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// 일반적인 함수 본문은 여기에 옵니다.
//		}
//
//		이 매크로는 MFC로 호출하기 전에
//		각 함수에 반드시 들어 있어야 합니다.
//		즉, 매크로는 함수의 첫 번째 문이어야 하며
//		개체 변수의 생성자가 MFC DLL로
//		호출할 수 있으므로 개체 변수가 선언되기 전에
//		나와야 합니다.
//
//		자세한 내용은
//		MFC Technical Note 33 및 58을 참조하십시오.
//

// CCWeldingToolLibApp

BEGIN_MESSAGE_MAP(CCWeldingToolLibApp, CWinApp)
END_MESSAGE_MAP()


// CCWeldingToolLibApp 생성

CCWeldingToolLibApp::CCWeldingToolLibApp()
{
	// TODO: 여기에 생성 코드를 추가합니다.
	// InitInstance에 모든 중요한 초기화 작업을 배치합니다.
}


// 유일한 CCWeldingToolLibApp 개체입니다.

CCWeldingToolLibApp theApp;
BusbarProcess m_BusbarPro;
SensingMeasure m_Sensing;

// CCWeldingToolLibApp 초기화
BOOL CCWeldingToolLibApp::InitInstance()
{
	CWinApp::InitInstance();

	return TRUE;
}

// extern "C" __declspec(dllexport) void __stdcall SetParameters(int nThresh, int nMaskLen, int nMeasureInterv, int nCaliperLen, int nSkipLenFromCenter) {
// 	m_BusbarPro.SetParameters(nThresh, nMaskLen, nMeasureInterv, nCaliperLen, nSkipLenFromCenter);
// }
// 
// extern "C" __declspec(dllexport) int* __stdcall GetTopPosX() {
// 	return m_BusbarPro.GetTopPosX();
// }
// 
// extern "C" __declspec(dllexport) int* __stdcall GetTopPosY() {
// 	return m_BusbarPro.GetTopPosY();
// }
// 
// extern "C" __declspec(dllexport) int* __stdcall GetBottomPosX() {
// 	return m_BusbarPro.GetBottomPosX();
// }
// 
// extern "C" __declspec(dllexport) int* __stdcall GetBottomPosY() {
// 	return m_BusbarPro.GetBottomPosY();
// }
//
// extern "C" __declspec(dllexport) void __stdcall ReleaseMemory() {
// 	m_BusbarPro.ReleaseMemory();
// }

extern "C" __declspec(dllexport) void __stdcall DoMeasureWelding(int nWidth, int nHeight, byte * pPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, byte * pPMS3, int nTBThresh, int nTBContinousLen) {
	m_BusbarPro.FindBusbarPosition(nWidth, nHeight, pPMS1, nLRLowThresh, nLRContinousLen, nLRHighThresh, pPMS3, nTBThresh, nTBContinousLen, BUSBAR_HORIZONTAL_LONG);
}

extern "C" __declspec(dllexport) int __stdcall GetBusbarWidth() {
	return m_BusbarPro.GetBusbarWidth();
}


extern "C" __declspec(dllexport) int __stdcall GetBusbarHeight() {
	return m_BusbarPro.GetBusbarHeight();
}

extern "C" __declspec(dllexport) int __stdcall GetBusbarLeft() {
	return m_BusbarPro.GetBusbarLeft();
}

extern "C" __declspec(dllexport) int __stdcall GetBusbarRight() {
	return m_BusbarPro.GetBusbarRight();
}

extern "C" __declspec(dllexport) int __stdcall GetBusbarTop() {
	return  m_BusbarPro.GetBusbarTop();
}

extern "C" __declspec(dllexport) int __stdcall GetBusbarBottom() {
	return  m_BusbarPro.GetBusbarBottom();
}

extern "C" __declspec(dllexport) void __stdcall DoMeasureSensor(LPBYTE pData, int nImgWid, int nImgHgt, int nSensorStdWid, int nSensorStdHgt, int nThresh) {
	CSize szStdSize = CSize(nSensorStdWid, nSensorStdHgt);
	m_Sensing.DoMeasureSensor(pData, nImgWid, nImgHgt, szStdSize, nThresh);
}

extern "C" __declspec(dllexport) int __stdcall GetSensorWidth() {
	return m_Sensing.GetWidth();
}

extern "C" __declspec(dllexport) int __stdcall GetSensorHeight() {
	return m_Sensing.GetHeight();
}

extern "C" __declspec(dllexport) int __stdcall GetSensorTop() {
	return m_Sensing.GetTop();
}

extern "C" __declspec(dllexport) int __stdcall GetSensorLeft() {
	return m_Sensing.GetLeft();
}

extern "C" __declspec(dllexport) int __stdcall GetSensorRight() {
	return m_Sensing.GetRight();
}

extern "C" __declspec(dllexport) int __stdcall GetSensorBottom() {
	return m_Sensing.GetBottom();
}