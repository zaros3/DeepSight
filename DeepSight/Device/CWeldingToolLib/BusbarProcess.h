#pragma once

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace std;
using namespace cv;

enum STD_Direction {
	STD_HORIZON = 0,
	STD_VERTICAL
};

enum BUSBAR_SEARCH_DIRECTION {
	BUSBAR_HORIZONTAL_LONG = 0,
	BUSBAR_VERTICAL_LONG,
};

class __declspec(dllexport) BusbarProcess
{
public:
	BusbarProcess();
	~BusbarProcess();
	Mat LineSTD(Mat matImg, CRect rtROI, int nMaskSize = 7, int nDirection = STD_HORIZON);
	Mat VerticalSobelImage(Mat matImg, int nMaskLen);
	Mat VerticalSobelImage(Mat matImg, CRect rtROI, int nMaskLen);
	Mat HorizonSobelImage(Mat matImg, int nMaskLen);
	Mat HorizonSobelImage(Mat matImg, CRect rtROI, int nMaskLen);
	Mat XYSobelImage(Mat matImg, CRect rtROI, int nMaskLen);
	Mat XYSobelImage(Mat matImg, int nMaskLen);
	Mat XYSobelImage8U(Mat matImg, int nMaskLen);

	Mat VerticalEdgeImage(Mat matImg, int nMaskLen);

	vector<float> VerticalProfile32F(Mat matEdg);
	vector<float> VerticalProfile8U(Mat matEdg);
	vector<vector<float>> VerticalProfile16S(Mat matEdg, int nCaliperLen, int nInterv);
	vector<vector<float>> VerticalProfile32F(Mat matEdg, int nCaliperLen, int nInterv);
	vector<float> HorizonProfile32F(Mat matEdg);
	vector<float> HorizonProfile8U(Mat matEdg);
	vector<vector<float>> HorizonProfile(Mat matEdg, int nCaliperLen, int nInterv);
	void SetParameters(int nThresh, int nMaskLen, int nMeasureInterv, int nCaliperLen, int nSkipLenFromCenter);
	vector<vector<cv::Point>> DoMeasureWelding(Mat matImg, CRect rtROI);
	vector<vector<cv::Point>> DoMeasureWelding(Mat matImg);
	vector<vector<cv::Point>> DoMeasureWelding(Mat matImg0, Mat matImg1);

	void DoMeasureWelding(LPBYTE pImage, int nWidth, int nHeight);
	int* GetTopPosX();
	int* GetTopPosY();
	int* GetBottomPosX();
	int* GetBottomPosY();
	int  GetPosLength();
	void ReleaseMemory();
	Mat  Image32Fto8U(Mat matImg);

	//--------------------- Using Differ Image -----------------------//
	CRect FindBusbarPosition(Mat matPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, Mat matPMS3, int nTBMaskLen, int nTBThresh, int nTBContinousLen, int nDirection);
	vector<CPoint> HorizontalPartialFindBusbarPos(Mat matImg, CPoint ptLeftTop, CPoint ptLeftRightPos, int nPartialLen, int nThresh, int nAllowanceIntensity, int nContinousCnt);
	Mat HorizontalDifferImage(Mat matImg, CRect rtROI, int nKernelLen);
	Mat VerticalDifferImage(Mat matImg, CRect rtROI, int nKernelLen);
	Mat HorizontalTriangle(Mat matImg, CRect rtROI, int nMaskLen = 5);
	Mat VariationFilter(Mat matImg, CRect rtROI, int nMaskSize);
	vector<float> MaxProfile(Mat matImg, CRect rtROI, int nMaskLen);
	CPoint SearchEndPos(vector<float> vMaxProf, int nContinousVal, int nLen, int nMinVal);

	//------------------- C# -------------------------------------------//
	void FindBusbarPosition(int nImgWid, int nImgHgt, LPBYTE pPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, LPBYTE pPMS3, int nTBThresh, int nTBContinousLen, int nDirection);
	int GetBusbarHeight();
	int GetBusbarWidth();
	int GetBusbarLeft();
	int GetBusbarRight();
	int GetBusbarTop();
	int GetBusbarBottom();
protected:
	Mat LineSTDHorizon(Mat matImg, CRect rtROI, int nMaskSize);
	Mat LineSTDVertical(Mat matImg, CRect rtROI, int nMaskSize);
	CPoint LocalHorizontalPartialFindBusbarPos(Mat matImg, int nThresh, int nAllowanceIntn, int nContinousCnt);
	cv::Point GetFindEdge(vector<float> vProfile, int nThresh);

	//--------------------- Using Differ Image -----------------------//
	CPoint VerticalFindBusbarPos(Mat matImg,  CPoint ptLeftTop, int nMaskLen, int nThresh);
	CPoint HorizontalFindBusbarPos(Mat matImg, CPoint ptLeftTop, CPoint ptLeftRightPos, int nThresh, int nContinousCnt);

protected:
	int m_nThresh;
	int m_nMaskLen;
	int m_nMeasureInterv;
	int m_nSkipLenFromCenter;
	int m_nCaliperLen;
	int* m_pTopPosX;
	int* m_pTopPosY;
	int* m_pBtmPosX;
	int* m_pBtmPosY;
	int m_nPosNum;
	int m_nBusbarWid;
	int m_nBusBarHeight;
	vector<CPoint> m_vEdgePos;
	vector<int> m_vImagePos;

	//----------------- Using Differ Image -----------------------//
	vector<CPoint> m_vPosLRPos;
	CRect m_rtLTRB;
};

