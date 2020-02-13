#pragma once

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace std;
using namespace cv;

class __declspec(dllexport) SensingMeasure
{
public:
	SensingMeasure();
	void DoMeasureSensor(Mat matImg, CSize szSensorStandardSize, int nThresh);
	void DoMeasureSensor(LPBYTE pData, int nImgWid, int nImgHgt, CSize szSensorStandardSize, int nThresh);
	int GetWidth();
	int GetHeight();
	int GetTop();
	int GetLeft();
	int GetRight();
	int GetBottom();

protected:
	Mat XYSobelImage8U(Mat matImg, int nMaskLen);
	Mat SensorDilate(Mat matBin, Mat matMSK, int nKernel);
	Mat DoBlobling(Mat matImg);
	vector<float> VerticalProfile8U(Mat matEdg);
	vector<float> HorizonProfile8U(Mat matEdg);


protected:
	CRect m_rtSensorPos;

};

