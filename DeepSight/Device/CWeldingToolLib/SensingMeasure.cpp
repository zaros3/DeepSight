#include "pch.h"
#include "SensingMeasure.h"

SensingMeasure::SensingMeasure() {

}

void SensingMeasure::DoMeasureSensor(LPBYTE pData, int nImgWid, int nImgHgt, CSize szSensorStandardSize, int nThresh) {
	Mat matImg = Mat(nImgHgt, nImgWid, CV_8UC1, pData);
	DoMeasureSensor(matImg, szSensorStandardSize, nThresh);
}

void SensingMeasure::DoMeasureSensor(Mat matImg, CSize szSensorStandardSize, int nThresh) {
//	cv::blur(matImg, matImg, cv::Size(3, 3));
	
	Mat matEDG = XYSobelImage8U(matImg, 3);

	Mat matBIN;
	threshold(matEDG, matBIN, nThresh, 255, THRESH_BINARY);

	Mat matMSK = Mat(matImg.size(), CV_8UC1, Scalar(0));
	cv::ellipse(matMSK, cv::RotatedRect(cv::Point2f(0.5f * matImg.cols, 0.5f * matImg.rows), cv::Size2f((float)szSensorStandardSize.cx, (float)szSensorStandardSize.cy), 0.0f), Scalar(255), -1);

	Mat matDIL0, matERD0;
	Mat matDIL = SensorDilate(matBIN, matMSK, 9);

	Mat matKNL3 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3), cv::Point(1, 1));
	Mat matKNL5 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5, 5), cv::Point(2, 2));
	cv::erode(matDIL, matERD0, matKNL3);
	cv::dilate(matERD0, matDIL0, matKNL5);

	Mat matBLB = DoBlobling(matDIL0);

	Mat matRst = Mat(matBIN.size(), CV_8UC1, Scalar(0));
	cv::bitwise_and(matBIN, matBLB, matRst);

	vector<float> vHorzProf = HorizonProfile8U(matRst);
	vector<float> vVertProf = VerticalProfile8U(matRst);

	for (int i = 0; i < (int)vHorzProf.size() / 2; i++) 
		if (vHorzProf[i] < 1.0f) m_rtSensorPos.top = i;

	for (int i = (int)vHorzProf.size() - 1; i > (int)vHorzProf.size() / 2; i--)
		if (vHorzProf[i] < 1.0f) m_rtSensorPos.bottom = i;

	for (int i = 0; i < (int)vVertProf.size() / 2; i++)
		if (vVertProf[i] < 1.0f) m_rtSensorPos.left = i;

	for (int i = (int)vVertProf.size() - 1; i > (int)vVertProf.size() / 2; i--)
		if (vVertProf[i] < 1.0f) m_rtSensorPos.right = i;
	
// 	Mat matSHW = Mat(matRst.size(), CV_8UC3);
// 	cv::cvtColor(matImg, matSHW, cv::COLOR_GRAY2BGR);
// 	rectangle(matSHW, cv::Rect(m_rtSensorPos.left, m_rtSensorPos.top, m_rtSensorPos.Width(), m_rtSensorPos.Height()), Scalar(0, 255, 0), 1);
// 
// 	cv::imwrite("E:\\matMSK.bmp", matSHW);
}

Mat SensingMeasure::XYSobelImage8U(Mat matImg, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matEDGX8U, matEDGY8U;
	Mat matEDGX, matEDGY, matEDG;
	cv::Sobel(matImg, matEDGX, CV_16S, 1, 0, nMaskLen);
	cv::Sobel(matImg, matEDGY, CV_16S, 0, 1, nMaskLen);
	cv::convertScaleAbs(matEDGX, matEDGX8U);
	cv::convertScaleAbs(matEDGY, matEDGY8U);
	cv::addWeighted(matEDGX8U, 0.5, matEDGY8U, 0.5, 0, matEDG);
	return matEDG;
}

Mat SensingMeasure::SensorDilate(Mat matBin, Mat matMSK, int nKernel) {
	if (nKernel < 3) nKernel = 3;

	int nHalf = nKernel / 2;
	nKernel = 2 * nHalf + 1;

	Mat matRet = matBin.clone();

	if (matBin.empty() || matMSK.empty()) return matRet;
	if (matBin.cols != matMSK.cols || matBin.rows != matMSK.rows) return matRet;

	LPBYTE pBin = matBin.data;
	LPBYTE pMSK = matMSK.data;
	LPBYTE pRet = matRet.data;
	for (int y = nHalf; y < matBin.rows - nHalf; y++) {
		int nStep = y * matBin.cols;
		for (int x = nHalf; x < matBin.cols - nHalf; x++) {
			int nPos = nStep + x;
			if(*(pMSK + nPos) != 255) continue;
			for (int j = -nHalf; j < nHalf + 1; j++) {
				int nStpM = (y + j) * matBin.cols;
				for (int i = -nHalf; i < nHalf + 1; i++) {
					int nPosM = nStpM + x + i;
					if (*(pBin + nPosM) == 255) {
						*(pRet + nPos) = 255;
						j = nHalf + 1;
						i = nHalf + 1;
					}
				}
			}
		}
	}
	return matRet;
}

Mat SensingMeasure::DoBlobling(Mat matImg) {
	std::vector<std::vector<cv::Point>> contours;
	cv::findContours(matImg.clone(), contours, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);

	Mat matRet = Mat(matImg.size(), CV_8UC1, Scalar(0));
	int nIndex = -1;
	double dMaxArea = 0;
	for (int i = 0; i < (int)contours.size(); i++) {
		double dArea = cv::contourArea(contours[i]);
		if (dMaxArea < dArea) {
			dMaxArea = dArea;
			nIndex = i;
		}
	}
	cv::drawContours(matRet, contours, nIndex, cv::Scalar(255), -1);
	return matRet;
}

vector<float> SensingMeasure::VerticalProfile8U(Mat matEdg) {
	vector<float> vProf(matEdg.cols);
	float fInvLen = 1.0f / matEdg.rows;
	LPBYTE pData = matEdg.data;
	for (int i = 0; i < matEdg.cols; i++) {
		int nSum = 0;
		for (int j = 0; j < matEdg.rows; j++) {
			nSum += *(pData + j * matEdg.cols + i);
		}
		vProf[i] = nSum * fInvLen;
	}
	return vProf;
}

vector<float> SensingMeasure::HorizonProfile8U(Mat matEdg) {
	vector<float> vProf(matEdg.rows);
	float fInvLen = 1.0f / matEdg.cols;
	LPBYTE pData = matEdg.data;
	for (int j = 0; j < matEdg.rows; j++) {
		int nStep = j * matEdg.cols;
		int nSum = 0;
		for (int i = 0; i < matEdg.cols; i++) {
			nSum += *(pData + nStep + i);
		}
		vProf[j] = nSum * fInvLen;
	}
	return vProf;
}

int SensingMeasure::GetWidth() {
	return m_rtSensorPos.Width();
}

int SensingMeasure::GetHeight() {
	return m_rtSensorPos.Height();
}

int SensingMeasure::GetTop() {
	return m_rtSensorPos.top;
}

int SensingMeasure::GetLeft() {
	return m_rtSensorPos.left;
}

int SensingMeasure::GetRight() {
	return m_rtSensorPos.right;
}
int SensingMeasure::GetBottom() {
	return m_rtSensorPos.bottom;
}