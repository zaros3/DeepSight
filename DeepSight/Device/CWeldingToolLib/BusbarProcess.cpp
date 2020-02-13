#include "pch.h"
#include "BusbarProcess.h"

BusbarProcess::BusbarProcess() {
	m_pTopPosX = NULL;
	m_pTopPosY = NULL;
	m_pBtmPosX = NULL;
	m_pBtmPosY = NULL;
}

BusbarProcess::~BusbarProcess() {
	ReleaseMemory();
}

Mat BusbarProcess::LineSTD(Mat matImg, CRect rtROI, int nMaskSize, int nDirection) {
	if (nDirection == STD_HORIZON)
		return LineSTDHorizon(matImg, rtROI, nMaskSize);
	else 
		return LineSTDVertical(matImg, rtROI, nMaskSize);
}

Mat BusbarProcess::LineSTDHorizon(Mat matImg, CRect rtROI, int nMaskSize) {
	Mat matSTD = Mat(rtROI.Height(), rtROI.Width(), CV_32FC1, Scalar(0));

	int nHalfMask = nMaskSize / 2;
	int nMaskLen  = 2 * nHalfMask + 1;
	float fInv = 1.0f / nMaskLen;
	float fInvSq = fInv * fInv;
	LPBYTE pData = matImg.data;
	float* pSqr = (float*)matSTD.data;
	for (int y = rtROI.top, j = 0; y < rtROI.bottom; y++, j++) {
		int nStep = y * matImg.cols;
		int nSqSp = j * matImg.cols;

		int nSum   = 0;
		int nSqSum = 0;
		int nCnt   = 0;
		int nStrtX = 0;
		for (int n = rtROI.left - nHalfMask; n < rtROI.right - nHalfMask; n++) {
			if(rtROI.left + n < 0) continue;

			nCnt++;
			if (nCnt > nMaskLen) break;

			BYTE bVal = *(pData + nStep + n);
			nSum   += bVal;
			nSqSum += bVal * bVal;
			nStrtX = n - nHalfMask + 1;
		}
		float fSTD = fInv * nSqSum - fInvSq * nSum * nSum;
		*(pSqr + nSqSp) = cv::sqrt(fSTD);

		for (int x = nStrtX, i = 1; x < rtROI.right + nHalfMask; x++, i++) {
			if(x > matImg.cols - nHalfMask - 1) break;
			int nPos = nStep + x;
			BYTE bVal0 = *(pData + nPos - nHalfMask - 1);
			BYTE bVal1 = *(pData + nPos + nHalfMask);
			nSum   += (bVal1 - bVal0);
			nSqSum += (bVal1 * bVal1 - bVal0 * bVal0);
			float fSTD = fInv * nSqSum - fInvSq * nSum * nSum;
			*(pSqr + nSqSp + i) = cv::sqrt(fSTD);
		}
	}
	return matSTD;
}

Mat BusbarProcess::LineSTDVertical(Mat matImg, CRect rtROI, int nMaskSize) {
	Mat matSTD = Mat(rtROI.Height(), rtROI.Width(), CV_32FC1, Scalar(0));

	int nHalfMask = nMaskSize / 2;
	int nMaskLen  = 2 * nHalfMask + 1;
	float fInv = 1.0f / nMaskLen;
	float fInvSq = fInv * fInv;
	LPBYTE pData = matImg.data;
	float* pSqr = (float*)matSTD.data;
	for (int x = rtROI.left, i = 0; x < rtROI.right; x++, i++) {
		int nSum   = 0;
		int nSqSum = 0;
		int nCnt   = 0;
		int nStrtY = 0;
		for (int n = rtROI.top - nHalfMask; n < rtROI.bottom - nHalfMask; n++) {
			if (rtROI.top + n < 0) continue;

			nCnt++;
			if (nCnt > nMaskLen) break;

			BYTE bVal = *(pData + x + n * matImg.cols);
			nSum   += bVal;
			nSqSum += bVal * bVal;
			nStrtY = n - nHalfMask + 1;
		}
		float fSTD = fInv * nSqSum - fInvSq * nSum * nSum;
		*(pSqr + i) = cv::sqrt(fSTD);

		for (int y = nStrtY, j = 1; y < rtROI.bottom + nHalfMask; y++, j++) {
			if (y > matImg.rows - nHalfMask - 1) break;
			int nPos = matImg.cols * y + x;
			BYTE bVal0 = *(pData + nPos - (nHalfMask + 1) * matImg.cols);
			BYTE bVal1 = *(pData + nPos + nHalfMask * matImg.cols);
			nSum   += (bVal1 - bVal0);
			nSqSum += (bVal1 * bVal1 - bVal0 * bVal0);
			float fSTD = fInv * nSqSum - fInvSq * nSum * nSum;
			*(pSqr + j * matImg.cols + i) = cv::sqrt(fSTD);
		}
	}
	return matSTD;
}

Mat BusbarProcess::VariationFilter(Mat matImg, CRect rtROI, int nMaskSize) {
	Mat matSTD = Mat(rtROI.Height(), rtROI.Width(), CV_32FC1, Scalar(0));

	int nHalfMask = nMaskSize / 2;
	int nMaskLen = 2 * nHalfMask + 1;
	int nMaskSqLen = nMaskLen * nMaskLen;
	
	float fInv = 1.0f / nMaskSqLen;
	float fInvSq = fInv * fInv;
	LPBYTE pData = matImg.data;
	float* pSqr = (float*)matSTD.data;
	for (int y = rtROI.top, j = 0; y < rtROI.bottom; y++, j++) {
		int nSqSp = j * matImg.cols;
		for (int x = rtROI.left, i = 0; x < rtROI.right; x++, i++) {
			int nSum = 0;
			int nSqSum = 0;
			int nCnt = 0;
			for (int ny = -nHalfMask; ny < nHalfMask + 1; ny++) {
				if (y + ny < 0) continue;
				if (y + ny > matImg.rows - 1) continue;
				int nMaskStep = (y + ny) * matImg.cols;
				for (int nx = -nHalfMask; nx < nHalfMask; nx++) {
					if (x + nx < 0) continue;
					if (x + nx > matImg.cols - 1) continue;

					BYTE bVal = *(pData + nMaskStep + nx + x);
					nSum += bVal;
					nSqSum += bVal * bVal;
					nCnt++;
				}
			}
			float fSTD = 0.0f;
			if (nCnt == nMaskSqLen) fSTD = fInv * nSqSum - fInvSq * nSum * nSum;
			else if (nCnt != 0) fSTD = (float)nSqSum / nCnt - (float)nSum * nSum / (nCnt * nCnt);
			*(pSqr + nSqSp + i) = cv::sqrt(fSTD);
		}
	}
	return matSTD;
}

Mat BusbarProcess::VerticalEdgeImage(Mat matImg, int nMaskLen) {
	if (matImg.empty()) return matImg;

	Mat matVAR = Mat(matImg.size(), CV_32FC1, Scalar(0.0));
	LPBYTE pData = matImg.data;
	float* pVars = (float*)matVAR.data;

	int nHalf = nMaskLen / 2;

	for (int j = 0; j < matImg.rows; j++) {
		int nStep = j * matImg.cols;
		for (int i = nHalf; i < matImg.cols - nHalf - 1; i++) {
			int nPos = nStep + i;
			int nCnt = 0;
			int nSqS = 0;
			int nSum = 0;
			for (int nx = -nHalf; nx < nHalf + 1; nx++) {
				int np = nPos + nx;
				BYTE bVal = *(pData + np);
				nSum += bVal;
				nSqS += bVal * bVal;
				nCnt++;
			}

			if (nCnt == 0) *(pVars + nPos) = 0.0f;
			else *(pVars + nPos) = (float)nSqS / nCnt - ((float)nSum * nSum / (nCnt * nCnt));
		}
	}
	return matVAR;
}

Mat BusbarProcess::VerticalSobelImage(Mat matImg, CRect rtROI, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matROI = matImg(cv::Rect(rtROI.left, rtROI.top, rtROI.Width(), rtROI.Height())).clone();
	Mat matEDG;
	cv::Sobel(matROI, matEDG, CV_16S, 0, 1, nMaskLen);
	return matEDG;
}

Mat BusbarProcess::VerticalSobelImage(Mat matImg, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matEDG;
	cv::Sobel(matImg, matEDG, CV_16S, 0, 1, nMaskLen);
	return matEDG;
}

Mat BusbarProcess::HorizonSobelImage(Mat matImg, CRect rtROI, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matROI = matImg(cv::Rect(rtROI.left, rtROI.top, rtROI.Width(), rtROI.Height())).clone();
	Mat matEDG;
	cv::Sobel(matROI, matEDG, CV_16S, 1, 0, nMaskLen);
	return matEDG;
}

Mat BusbarProcess::HorizonSobelImage(Mat matImg, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matEDG;
	cv::Sobel(matImg, matEDG, CV_16S, 1, 0, nMaskLen);
	return matEDG;
}

Mat BusbarProcess::XYSobelImage(Mat matImg, CRect rtROI, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matROI = matImg(cv::Rect(rtROI.left, rtROI.top, rtROI.Width(), rtROI.Height())).clone();
	Mat matEDG;
	cv::Sobel(matROI, matEDG, CV_16S, 1, 1, nMaskLen);
	return matEDG;
}

Mat BusbarProcess::XYSobelImage(Mat matImg, int nMaskLen) {
	if (nMaskLen < 3) nMaskLen = 3;
	Mat matEDGX, matEDGY, matEDG;
	cv::Sobel(matImg, matEDGX, CV_16S, 1, 0, nMaskLen);
	cv::Sobel(matImg, matEDGY, CV_16S, 0, 1, nMaskLen);
	cv::addWeighted(matEDGX, 0.5, matEDGY, 0.5, 0, matEDG);
	return matEDG;
}

Mat BusbarProcess::XYSobelImage8U(Mat matImg, int nMaskLen) {
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

vector<float> BusbarProcess::VerticalProfile32F(Mat matEdg) {
	vector<float> vProf(matEdg.cols);
	float fInvLen = 1.0f / matEdg.rows;
	float* pData = (float*)matEdg.data;
	for (int i = 0; i < matEdg.cols; i++) {
		float fSum = 0.0f;
		for (int j = 0; j < matEdg.rows; j++) {
			fSum += *(pData + j * matEdg.cols + i);
		}
		vProf[i] = fSum * fInvLen;
	}
	return vProf;
}

vector<float> BusbarProcess::VerticalProfile8U(Mat matEdg) {
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

vector<vector<float>> BusbarProcess::VerticalProfile16S(Mat matEdg, int nCaliperLen, int nInterv) {
	vector<vector<float>> vProf;
	if (nCaliperLen < 3) {
		int nNum = (matEdg.rows - 1) / nInterv + 1;
		short* pData = (short*)matEdg.data;
		for (int ny = 0; ny < nNum; ny++) {
			int nStep = matEdg.cols * ny * nInterv;
			vector<float> vProfIntv;
			for (int nx = 0; nx < matEdg.cols; nx++) {
				int nPos = nx + nStep;
				float fVal = (float)(*(pData + nPos));
				vProfIntv.push_back(fVal);
			}
			vProf.push_back(vProfIntv);
		}
	}
	else {
		int nSrchHalf = nCaliperLen / 2;
		int nNum = (matEdg.rows - nSrchHalf) / nInterv + 1;
		float fInvLen = 1.0f / (2 * nSrchHalf + 1);
		short* pData = (short*)matEdg.data;
		for (int j = 0; j < nNum; j++) {
			int ny = j * nInterv + nSrchHalf;
			vector<float> vProfIntv;
			for (int i = 0; i < matEdg.cols; i++) {
				float fSum = 0.0f;
				for (int n = -nSrchHalf; n < nSrchHalf + 1; n++) {
					int nPosY = ny + n;
					if (nPosY < 0) continue;
					if (nPosY > matEdg.rows - 1) continue;
					fSum += *(pData + nPosY * matEdg.cols + i);
				}
				vProfIntv.push_back(fSum * fInvLen);
			}
			vProf.push_back(vProfIntv);
		}
	}
	return vProf;
}

vector<vector<float>> BusbarProcess::VerticalProfile32F(Mat matEdg, int nCaliperLen, int nInterv) {
	vector<vector<float>> vProf;
	if (nCaliperLen < 3) {
		int nNum = (matEdg.rows - 1) / nInterv + 1;
		float* pData = (float*)matEdg.data;
		for (int ny = 0; ny < nNum; ny++) {
			int nStep = matEdg.cols * ny * nInterv;
				vector<float> vProfIntv;
			for (int nx = 0; nx < matEdg.cols; nx++) {
				int nPos = nx + nStep;
				float fVal = (float)(*(pData + nPos));
				vProfIntv.push_back(fVal);
			}
			vProf.push_back(vProfIntv);
		}
	}
	else {
		int nSrchHalf = nCaliperLen / 2;
		int nNum = (matEdg.rows - nSrchHalf) / nInterv + 1;
		float fInvLen = 1.0f / (2 * nSrchHalf + 1);
		float* pData = (float*)matEdg.data;
		for (int j = 0; j < nNum; j++) {
			int ny = j * nInterv + nSrchHalf;
			vector<float> vProfIntv;
			for (int i = 0; i < matEdg.cols; i++) {
				float fSum = 0.0f;
				for (int n = -nSrchHalf; n < nSrchHalf + 1; n++) {
					int nPosY = ny + n;
					if (nPosY < 0) continue;
					if (nPosY > matEdg.rows - 1) continue;
					fSum += *(pData + nPosY * matEdg.cols + i);
				}
				vProfIntv.push_back(fSum * fInvLen);
			}
			vProf.push_back(vProfIntv);
		}
	}
	return vProf;
}

vector<float> BusbarProcess::HorizonProfile32F(Mat matEdg) {
	vector<float> vProf(matEdg.rows);
	float fInvLen = 1.0f / matEdg.cols;
	float* pData = (float*)matEdg.data;
	for (int j = 0; j < matEdg.rows; j++) {
		int nStep = j * matEdg.cols;
		float fSum = 0.0f;
		for (int i = 0; i < matEdg.cols; i++) {
			fSum += *(pData + nStep + i);
		}
		vProf[j] = fSum * fInvLen;
	}
	return vProf;
}

vector<float> BusbarProcess::HorizonProfile8U(Mat matEdg) {
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

vector<vector<float>> BusbarProcess::HorizonProfile(Mat matEdg, int nCaliperLen, int nInterv) {
	vector<vector<float>> vProf;
	if (nCaliperLen < 3) {
		int nNum = (matEdg.cols - 1) / nInterv + 1;
		float* pData = (float*)matEdg.data;
		for (int i = 0; i < nNum; i++) {
			vector<float> vProfIntv;
			for (int j = 0; j < matEdg.rows; j++) {
				int nPos = j * matEdg.cols + i * nInterv;
				float fVal = (float)(*(pData + nPos));
				vProfIntv.push_back(fVal);
			}
			vProf.push_back(vProfIntv);
		}
	}
	else {
		int nSrchHalf = nCaliperLen / 2;
		int nNum = (matEdg.cols - nSrchHalf) / nInterv + 1;
		float fInvLen = 1.0f / (2 * nSrchHalf + 1);
		float* pData = (float*)matEdg.data;
		for (int i = 0; i < nNum; i++) {
			vector<float> vProfIntv;
			for (int j = 0; j < matEdg.rows; j++) {
				int nStep = j * matEdg.cols;
				int nxpos = i * nInterv + nSrchHalf;
				float fSum = 0.0f;
				for (int n = -nSrchHalf; n < nSrchHalf + 1; n++) {
					int nx = nxpos + n;
					if (nx < 0) continue;
					if (nx > matEdg.cols - 1) continue;
					fSum += *(pData + nStep + nx);
				}
				vProfIntv.push_back(fSum * fInvLen);
			}
			vProf.push_back(vProfIntv);
		}
	}
	return vProf;
}

//Rotation and Shift 후에 사용
vector<vector<cv::Point>> BusbarProcess::DoMeasureWelding(Mat matImg, CRect rtROI) {
	vector<vector<cv::Point>> vWeldingCont;
	if (matImg.empty()) return vWeldingCont;

	Mat matROI = matImg(cv::Rect(rtROI.left, rtROI.top, rtROI.Width(), rtROI.Height())).clone();
//	cv::medianBlur(matROI, matROI, 3);

	Mat matEDG = VerticalSobelImage(matROI, m_nMaskLen);
	vector<vector<float>> vTotalProf = VerticalProfile16S(matEDG, m_nCaliperLen, m_nMeasureInterv);

	for (int n = 0; n < static_cast<int>(vTotalProf.size()); n++) {
		vector<float> vProf = vTotalProf[n];
		for (int i = 0; i < static_cast<int>(vProf.size()); i++) {
			float fVal = vProf[i];
		}
	}
	return vWeldingCont;
}

vector<vector<cv::Point>> BusbarProcess::DoMeasureWelding(Mat matImg0, Mat matImg1) {
	vector<vector<cv::Point>> vWeldingCont;
	if (matImg0.empty()) return vWeldingCont;
	if (matImg1.empty()) return vWeldingCont;

//	Mat matEDGX = VerticalEdgeImage(matImg0, m_nMaskLen);

	Mat matEDG0 = XYSobelImage(matImg0, m_nMaskLen);
	Mat matEDG1 = XYSobelImage(matImg1, m_nMaskLen);

	Mat matMax;

	cv::max(matEDG0, matEDG1, matMax);
	   
	vector<vector<float>> vTotalProf = VerticalProfile16S(matEDG0, m_nCaliperLen, m_nMeasureInterv);

	for (int n = 0; n < static_cast<int>(vTotalProf.size()); n++) {
		vector<float> vProf = vTotalProf[n];
		for (int i = 0; i < static_cast<int>(vProf.size()); i++) {
			float fVal = vProf[i];
		}
	}
	return vWeldingCont;
}



vector<vector<cv::Point>> BusbarProcess::DoMeasureWelding(Mat matImg) {
	vector<vector<cv::Point>> vWeldingCont;
	if (matImg.empty()) return vWeldingCont;

	Mat matEDG = XYSobelImage(matImg, m_nMaskLen);

	imwrite("E:\\matEDG.bmp", matEDG);

	vector<vector<float>> vTotalProf = VerticalProfile16S(matEDG, m_nCaliperLen, m_nMeasureInterv);

	for (int n = 0; n < static_cast<int>(vTotalProf.size()); n++) {
		vector<float> vProf = vTotalProf[n];
		for (int i = 0; i < static_cast<int>(vProf.size()); i++) {
			float fVal = vProf[i];
		}
	}
	return vWeldingCont;
}

void BusbarProcess::DoMeasureWelding(LPBYTE pImage, int nWidth, int nHeight) {
	if (pImage == NULL) return;

	Mat matImg = Mat(nHeight, nWidth, CV_8UC1, pImage);

	Mat matEDG = XYSobelImage(matImg, m_nMaskLen);
	vector<vector<float>> vTotalProf = VerticalProfile16S(matEDG, m_nCaliperLen, m_nMeasureInterv);

	int nNum = static_cast<int>(vTotalProf.size());

	if (m_pTopPosX == NULL) {
		m_pTopPosX = new int[nNum];
		m_pTopPosY = new int[nNum];
		m_pBtmPosX = new int[nNum];
		m_pBtmPosY = new int[nNum];
	}
	else if (nNum != m_nPosNum) {
		delete[] m_pTopPosX;
		delete[] m_pTopPosY;
		delete[] m_pBtmPosX;
		delete[] m_pBtmPosY;
		m_pTopPosX = new int[nNum];
		m_pTopPosY = new int[nNum];
		m_pBtmPosX = new int[nNum];
		m_pBtmPosY = new int[nNum];
		m_nPosNum = nNum;
	}

	for (int n = 0; n < static_cast<int>(vTotalProf.size()); n++) {
		vector<float> vProf = vTotalProf[n];
		cv::Point ptPos = GetFindEdge(vProf, m_nThresh);
		m_pTopPosX[n] = ptPos.x;
		m_pTopPosY[n] = ptPos.y;
	}
}

cv::Point BusbarProcess::GetFindEdge(vector<float> vProfile, int nThresh) {
	cv::Point ptPos = cv::Point(-1, -1);

	int nCnt0 = 0;
	int nCnt1 = 0;
	int nLen = static_cast<int>(vProfile.size() / 2);
	int nHalfLen = nLen / 2;
	for (int i = 0; i < nHalfLen - m_nSkipLenFromCenter; i++) {
		int nBackId = nLen - i - 1;
		float fVal0 = vProfile[i];
		float fVal1 = vProfile[nBackId];

		if (fVal0 < nThresh) nCnt0++;
		else if (nCnt0 > 2) {
			ptPos.x = i;
			nCnt0 = 0;
		}

		if (fVal1 < nThresh) nCnt1++;
		else if (nCnt1 > 2) {
			ptPos.y = nBackId;
			nCnt1 = 0;
		}
	}
	return ptPos;
}

int* BusbarProcess::GetTopPosX() {
	return m_pTopPosX;
}

int* BusbarProcess::GetTopPosY() {
	return m_pTopPosY;
}

int* BusbarProcess::GetBottomPosX() {
	return m_pBtmPosY;
}

int* BusbarProcess::GetBottomPosY() {
	return m_pBtmPosY;
}

int BusbarProcess::GetPosLength() {
	return m_nPosNum;
}

void BusbarProcess::ReleaseMemory() {
	if (m_pTopPosX != NULL) delete[] m_pTopPosX;
	if (m_pTopPosY != NULL) delete[] m_pTopPosY;
	if (m_pBtmPosX != NULL) delete[] m_pBtmPosX;
	if (m_pBtmPosY != NULL) delete[] m_pBtmPosY;
}

Mat BusbarProcess::Image32Fto8U(Mat matImg) {
	Mat mat8U = Mat(matImg.size(), CV_8UC1, Scalar(0));

	float fMin = 0xffffff;
	float fMax = 0.0f;
	float* pP = (float*)matImg.data;
	for (int i = 0; i < matImg.cols * matImg.rows; i++) {
		float fVal = *(pP + i);
		if (fVal < fMin) fMin = fVal;
		if (fVal > fMax) fMax = fVal;
	}

	float dRate = 255.0f / (fMax - fMin);
	LPBYTE pRet = mat8U.data;
	for (int i = 0; i < mat8U.cols * mat8U.rows; i++) {
		float fVal = *(pP + i);
		*(pRet + i) = (BYTE)(dRate * (fVal - fMin));
	}
	return mat8U;
}

//--------------------------- Differ Image ------------------------------------------------------------//
void BusbarProcess::SetParameters(int nThresh, int nMaskLen, int nMeasureInterv, int nCaliperLen, int nSkipLenFromCenter) {
	m_nThresh = nThresh;
	m_nMaskLen = nMaskLen;
	m_nMeasureInterv = nMeasureInterv;
	m_nCaliperLen = nCaliperLen;
	m_nSkipLenFromCenter = nSkipLenFromCenter;
	m_nPosNum = 0;
	if (m_pTopPosX != NULL) {
		delete[] m_pTopPosX;
		delete[] m_pTopPosY;
		delete[] m_pBtmPosX;
		delete[] m_pBtmPosY;
	}
	m_pTopPosX = NULL;
	m_pTopPosY = NULL;
	m_pBtmPosX = NULL;
	m_pBtmPosY = NULL;
}


Mat BusbarProcess::HorizontalDifferImage(Mat matImg, CRect rtROI, int nKernelLen) {
	if (matImg.empty()) return Mat(0, 0, CV_32FC1);
	if (rtROI.Width() == 0 || rtROI.Height() == 0) rtROI = CRect(nKernelLen, 0, matImg.cols - nKernelLen, matImg.rows);
	if (rtROI.left < nKernelLen) rtROI.left = nKernelLen;
	if (rtROI.right < matImg.cols - nKernelLen) rtROI.right = matImg.cols - 2 * nKernelLen;

	Mat matRet = Mat(rtROI.Height(), rtROI.Width(), CV_8UC1, Scalar(0));

	int nHalf = nKernelLen / 2;
	float fInv = 1.0f / nKernelLen;
	LPBYTE pData = matImg.data;
	LPBYTE pRetn = matRet.data;
	for (int i = rtROI.left, x = 0; i < rtROI.right; i++, x++) {
		for (int j = rtROI.top, y = 0; j < rtROI.bottom; j++, y++) {
			int nSum = 0;
			int nPos = j * matImg.cols + i;
			int nCur = y * rtROI.Width() + x;
			for (int n = -nHalf; n < nHalf + 1; n++) {
				nSum += *(pData + nPos + n);
			}
			*(pRetn + nCur) = (int)(abs(nSum * fInv - *(pData + nPos + nKernelLen)) + 0.5); //nKernelLen 만큼의 평균 - 현재위치 + KernelLen위치에서의 이미지와의 차
		}
	}
	return matRet;
}

Mat BusbarProcess::VerticalDifferImage(Mat matImg, CRect rtROI, int nKernelLen) {
	if (matImg.empty()) return Mat(0, 0, CV_32FC1);
	if (rtROI.Width() == 0 || rtROI.Height() == 0) rtROI = CRect(0, nKernelLen, matImg.cols, matImg.rows - nKernelLen);
	if (rtROI.top < nKernelLen) rtROI.top = nKernelLen;
	if (rtROI.bottom < matImg.rows - nKernelLen) rtROI.bottom = matImg.rows - 2 * nKernelLen;

	Mat matRet = Mat(rtROI.Height(), rtROI.Width(), CV_8UC1, Scalar(0));

	int nHalf = nKernelLen / 2;
	float fInv = 1.0f / nKernelLen;
	LPBYTE pData = matImg.data;
	LPBYTE pRetn = matRet.data;
	for (int j = rtROI.top, y = 0; j < rtROI.bottom; j++, y++) {
		int nStep = j * matImg.cols;
		for (int i = rtROI.left, x = 0; i < rtROI.right; i++, x++) {
			int nSum = 0;
			int nPos = nStep + i;
			int nCur = y * rtROI.Width() + x;
			for (int n = -nHalf; n < nHalf + 1; n++) {
				nSum += *(pData + nPos + n * matImg.cols);
			}
			*(pRetn + nCur) = (int)(abs(nSum * fInv - *(pData + nPos + nKernelLen * matImg.cols)) + 0.5);
		}
	}
	return matRet;
}

void BusbarProcess::FindBusbarPosition(int nImgWid, int nImgHgt, LPBYTE pPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, LPBYTE pPMS3, int nTBThresh, int nTBContinousLen, int nDirection) {
	m_rtLTRB = CRect(0, 0, 0, 0);

	Mat matPMS1 = Mat(nImgHgt, nImgWid, CV_8UC1, pPMS1);
	Mat matPMS3 = Mat(nImgHgt, nImgWid, CV_8UC1, pPMS3);

	m_rtLTRB = FindBusbarPosition(matPMS1, nLRLowThresh, nLRContinousLen, nLRHighThresh, matPMS3, 50, nTBThresh, nTBContinousLen, nDirection);
}

CRect BusbarProcess::FindBusbarPosition(Mat matPMS1, int nLRLowThresh, int nLRContinousLen, int nLRHighThresh, Mat matPMS3, int nTBMaskLen, int nTBThresh, int nTBContinousLen, int nDirection) {
	CRect rtEndPos = CRect(0, 0, 0, 0);
	if (matPMS1.empty()) return rtEndPos;
	if (matPMS3.empty()) return rtEndPos;

	m_vPosLRPos.clear();
	
// 	int nContinousVal = 7;
// 	int nEndPosContLen = 50;
// 	int nEndPosMinVal = 10;
	int nPartialLen = 100;
	int nTBHighThresh = nTBThresh + 20;
 	int nMaxProfKernelLen = 40;
	CPoint ptPos = CPoint(-1, -1);
	int nSobelKernel = 3;

	CRect rtROI = CRect(0, 0, matPMS1.cols, matPMS1.rows);
	Mat matEDG, matROI, matTRI, matSTD;
	CPoint ptEndPos = CPoint(-1, -1);

	vector<float> vMaxProf;

	switch (nDirection) {
	case BUSBAR_VERTICAL_LONG:
		matROI = VerticalDifferImage(matPMS1, rtROI, nTBMaskLen);
//		ptPos  = VerticalFindBusbarPos(matROI, rtROI.TopLeft(), nMaskLen, nThresh);
		break;
	case BUSBAR_HORIZONTAL_LONG:
		//Left and Right Position
		matSTD   = LineSTD(matPMS1, rtROI);
		vMaxProf = MaxProfile(matSTD, rtROI, nMaxProfKernelLen);
		ptEndPos = SearchEndPos(vMaxProf, nLRLowThresh, nLRContinousLen, nLRHighThresh);
		if (ptEndPos.x == -1 || ptEndPos.y == -1) break;

		//Top and Bottom Position
		matSTD      = LineSTD(matPMS3, rtROI);
		ptPos       = HorizontalFindBusbarPos(matSTD, CPoint(0, 0), ptEndPos, nTBThresh, nTBContinousLen);
		m_vPosLRPos = HorizontalPartialFindBusbarPos(matSTD, CPoint(0, 0), ptEndPos, nPartialLen, nTBThresh, nTBHighThresh, nTBContinousLen);
		break;
	default:
		//matTRI = HorizontalTriangle(matImg, CRect(0, 0, matImg.cols, matImg.rows));
		//Left and Right Position
		matSTD = LineSTD(matPMS1, rtROI);
		vMaxProf = MaxProfile(matSTD, rtROI, nMaxProfKernelLen);
		ptEndPos = SearchEndPos(vMaxProf, nLRLowThresh, nLRContinousLen, nLRHighThresh);
		if (ptEndPos.x == -1 || ptEndPos.y == -1) break;

		//Top and Bottom Position
		matEDG = XYSobelImage8U(matPMS3, nSobelKernel);
		matROI = HorizontalDifferImage(matEDG, rtROI, nTBMaskLen);
		ptPos = HorizontalFindBusbarPos(matROI, rtROI.TopLeft(), ptEndPos, nTBThresh, nTBContinousLen);
		break;
	}

	rtEndPos.left   = ptEndPos.x;
	rtEndPos.right  = ptEndPos.y;
	rtEndPos.top    = ptPos.x;
	rtEndPos.bottom = ptPos.y;

	m_nBusbarWid = ptEndPos.y - ptEndPos.x;

	return rtEndPos;
}

int BusbarProcess::GetBusbarHeight() {
	if (m_vPosLRPos.size() == 0) return 0;

	vector<int> vHeight;
	for (int i = 0; i < m_vPosLRPos.size(); i++) {
		int nHgt = abs(m_vPosLRPos[i].x - m_vPosLRPos[i].y);
		vHeight.push_back(nHgt);
	}

	std::sort(vHeight.begin(), vHeight.end());

	return vHeight[m_vPosLRPos.size() / 2];
}

int BusbarProcess::GetBusbarWidth() {
	return m_nBusbarWid;
}

int BusbarProcess::GetBusbarLeft() {
	return m_rtLTRB.left;
}

int BusbarProcess::GetBusbarRight() {
	return m_rtLTRB.right;
}

int BusbarProcess::GetBusbarTop() {
	return m_rtLTRB.top;
}

int BusbarProcess::GetBusbarBottom() {
	return m_rtLTRB.bottom;
}

CPoint BusbarProcess::VerticalFindBusbarPos(Mat matImg, CPoint ptLeftTop, int nMaskLen, int nThresh) {
	return CPoint(0,0);
}

// vector<CPoint> BusbarProcess::VerticalFindBusbarPos(Mat matImg, CPoint ptLeftTop, int nMaskLen, int nThresh, int nAllowDif) {
// 	vector<CPoint> vPos(matImg.rows);
// 
// 	int nHalfPos = matImg.cols / 2;
// 	LPBYTE pData = matImg.data;
// 	for (int j = 0; j < matImg.rows; j++) {
// 		int nStep = j * matImg.cols;
// 		vPos[j] = CPoint(nHalfPos, nHalfPos);
// 		for (int i = 0; i < nHalfPos; i++) {
// 			if (*(pData + nStep + i) > nThresh) {
// 				vPos[j].x = i;
// 				break;
// 			}	
// 		}
// 
// 		for (int i = matImg.cols - 1; i > nHalfPos; i--) {
// 			if (*(pData + nStep + i) > nThresh) {
// 				vPos[j].y = i;
// 				break;
// 			}
// 		}
// 	}
// 
// 	return vPos;
// }

// matImg         : 버스바 한개만 잘라낸 이미지
// ptLeftTop      : 전체이미지에서의 좌표
// ptLeftRightPos : matImg내에서의 좌우 버스바의 위치
// nThresh        : 버스바로 인정되는 평균 프로파일의 밝기의 Threshold
// nContiousCnt   : 버스바로 인정되는 평균 프로파일가 Threshold 이상것이 연속해야할 최소길이
// return         : 버스바의 상하의 위치 반납
CPoint BusbarProcess::HorizontalFindBusbarPos(Mat matImg, CPoint ptLeftTop, CPoint ptLeftRightPos, int nThresh, int nContinousCnt) {
	CPoint ptPos = CPoint(-1, -1);

	int nBusbarLen = ptLeftRightPos.y - ptLeftRightPos.x;
	int nHalfLenX = nBusbarLen / 2;
	int nHalfHlfX = ptLeftRightPos.x + nBusbarLen / 4;
	int nHalfPosY = matImg.rows / 2;
	int nHalfHlfY = matImg.rows / 4;

	Mat matROI = matImg(cv::Rect(nHalfHlfX, 0, nHalfLenX, matImg.rows)).clone();

	int nCnt = 0;
	vector<float> vProf = HorizonProfile8U(matROI);
	for (int i = 0; i < (int)vProf.size() - nHalfHlfY; i++) {
		if (vProf[i] > nThresh) {
			nCnt++;
		}
		else {
			ptPos.x = i;
			nCnt = 0;
		}

		if (nCnt > nContinousCnt) {
			break;
		}
	}

	nCnt = 0;
	for (int i = (int)vProf.size() - 1; i > nHalfHlfY; i--) {
		if (vProf[i] > nThresh) {
			nCnt++;
		}
		else {
			ptPos.y = i;
			nCnt = 0;
		}

		if (nCnt > nContinousCnt) {
			break;
		}
	}

	return ptPos;
}


// matImg         : 버스바 한개만 잘라낸 이미지
// ptLeftTop      : 전체이미지에서의 좌표
// ptLeftRightPos : matImg내에서의 좌우 버스바의 위치
// nPartialLen    : matImg 를 nPartialLen만큼 잘라서 처리할 길이
// nThresh        : 버스바로 인정되는 평균 프로파일의 밝기의 Threshold
// nAllowanceIntn : 버스바로 인정되는 평균 프로파일의 밝기의 Threshold 이상이며 nContiousCnt이하인 것중에서 밝기가 nAllowanceIntn이면 nContiousCnt이하라도 그위치를 버스바로 인정
// nContiousCnt   : 버스바로 인정되는 평균 프로파일가 Threshold 이상것이 연속해야할 최소길이
// return         : 버스바의 상하의 위치 반납
vector<CPoint> BusbarProcess::HorizontalPartialFindBusbarPos(Mat matImg, CPoint ptLeftTop, CPoint ptLeftRightPos,  int nPartialLen, int nThresh, int nAllowanceIntn, int nContinousCnt) {
	int nBusbarLen = ptLeftRightPos.y - ptLeftRightPos.x;
	int nHalfLenX = nBusbarLen / 2;
	int nHalfHlfX = ptLeftRightPos.x + nBusbarLen / 4;
	int nHalfPosY = matImg.rows / 2;
	int nHalfHlfY = matImg.rows / 4;

	Mat matROI = matImg(cv::Rect(nHalfHlfX, 0, nHalfLenX, matImg.rows)).clone();

	Mat mat8U;
	matROI.convertTo(mat8U, CV_8UC1);

	Mat matBin;
	threshold(mat8U, matBin, 0, 255, THRESH_BINARY | THRESH_OTSU);

	CPoint ptPos(-1, -1);
	int nCnt = 0;
	vector<float> vProf = HorizonProfile8U(matBin);
	for (int i = 0; i < (int)vProf.size() - nHalfHlfY; i++) {
		if (vProf[i] > nThresh) {
			nCnt++;
		}
		else {
			nCnt = 0;
		}

		if (nCnt > nContinousCnt) {
			ptPos.x = i - nCnt;
			break;
		}
	}

	nCnt = 0;
	for (int i = (int)vProf.size() - 1; i > nHalfHlfY; i--) {
		if (vProf[i] > nThresh) {
			nCnt++;
		}
		else {
			nCnt = 0;
		}

		if (nCnt > nContinousCnt) {
			ptPos.y = i + nCnt;
			break;
		}
	}

	Mat mat8UAll;
	matImg.convertTo(mat8UAll, CV_8UC1);
	threshold(mat8UAll, matBin, 0, 255, THRESH_BINARY | THRESH_OTSU);

	int nSearchLen = 10;
	int nTop = ptPos.x - nSearchLen;
	int nBtm = ptPos.y + nSearchLen;
	nTop = nTop < 0 ? 0 : nTop;
	nBtm = nBtm > matBin.rows ? matBin.rows : nBtm;
	int nPartialNum = nBusbarLen / nPartialLen;
	vector<CPoint> vPos(nPartialNum);
	for (int i = 0; i < nPartialNum; i++) {
		CPoint ptLRPos = CPoint(-1, -1);
		Mat matPTL = matBin(cv::Rect(ptLeftRightPos.x + i * nPartialLen, nTop, nPartialLen, nBtm - nTop)).clone();
		ptLRPos = LocalHorizontalPartialFindBusbarPos(matPTL, nThresh, nAllowanceIntn, nContinousCnt);
		vPos[i] = ptLRPos + CPoint(nTop, nTop);
	}
	return vPos;
}

CPoint BusbarProcess::LocalHorizontalPartialFindBusbarPos(Mat matImg, int nThresh, int nAllowanceIntn, int nContinousCnt) {
	CPoint ptPos = CPoint(-1, -1);

	int nHalfPosY = matImg.rows / 2;
	int nHalfHlfY = matImg.rows / 4;

	BOOL bFlag = FALSE;
	int nCnt = 0;
	vector<float> vProf = HorizonProfile8U(matImg);
	for (int i = 0; i < (int)vProf.size() - nHalfHlfY; i++) {
		float fVal = vProf[i];
		if (fVal > nThresh) {
			nCnt++;
			bFlag = TRUE;
		}
		else {
			nCnt = 0;
		}

		if (nCnt > nContinousCnt) {
			ptPos.x = i - nCnt;
			break;
		}
		else if (fVal > nAllowanceIntn && bFlag) {
			ptPos.x = i - nCnt;
			break;
		}
	}

	nCnt = 0;
	bFlag = FALSE;
	for (int i = (int)vProf.size() - 1; i > nHalfHlfY; i--) {
		float fVal = vProf[i];
		if (fVal > nThresh) {
			nCnt++;
			bFlag = TRUE;
		}
		else {
			nCnt = 0;
		}

		if (nCnt > nContinousCnt) {
			ptPos.y = i + nCnt;
			break;
		}
		else if (fVal > nAllowanceIntn&& bFlag) {
			ptPos.y = i + nCnt;
			break;
		}
	}

	return ptPos;
}


// vector<CPoint> BusbarProcess::HorizontalFindBusbarPos(Mat matImg, CPoint ptLeftTop, int nMaskLen, int nThresh, int nAllowDif) {
// 	vector<CPoint> vPos(matImg.cols);
// 
// 	int nHalfPosX = matImg.cols / 2;
// 	int nHalfHlfX = matImg.cols / 4;
// 	int nHalfPosY = matImg.rows / 2;
// 	int nHalfHlfY = matImg.rows / 4;
// 
// 	int * pHistX = new int[nHalfPosY];
// 	int * pHistY = new int[nHalfPosY];
// 	memset(pHistX, 0, sizeof(int) * nHalfPosY);
// 	memset(pHistY, 0, sizeof(int) * nHalfPosY);
// 
// 	int nGroupLen = 50;
// 	int nGroupNum = (int)(0.5 * matImg.cols / nGroupLen + 1);
// 	for (int n = 0; n < nGroupNum; n++) {
// 		Mat matROI = matImg(cv::Rect(n * nGroupLen + nHalfHlfX, 0, nGroupLen, matImg.rows)).clone();
// 		vector<float> vROIProf = HorizonProfile8U(matROI);
// 		for (int k = 0; k < nHalfPosY - m_nSkipLenFromCenter; k++)
// 			if (vROIProf[k + 1] >= nThresh && vROIProf[k] < nThresh) pHistX[k]++;
// 
// 		for (int k = nHalfPosY + m_nSkipLenFromCenter, m = m_nSkipLenFromCenter; k < matImg.rows - 1; k++, m++)
// 			if (vROIProf[k] >= nThresh && vROIProf[k + 1] < nThresh) pHistY[m]++;
// 	}
// 
// 	int nMaxCntX = 0;
// 	int nMaxPosX = nHalfPosY;
// 	int nMaxCntY = 0;
// 	int nMaxPosY = nHalfPosY;
// 
// 	for (int i = 1; i < nHalfPosY - 1; i++) {
// 		int nSumX = pHistX[i - 1] + pHistX[i] + pHistX[i + 1];
// 		if (nMaxCntX < nSumX) {
// 			nMaxCntX = nSumX;
// 			nMaxPosX = i;
// 		}
// 		int nSumY = pHistY[i - 1] + pHistY[i] + pHistY[i + 1];
// 		if (nMaxCntY < nSumY) {
// 			nMaxCntY = nSumY;
// 			nMaxPosY = i + nHalfPosY;
// 		}
// 	}
// 
// 	int nMinX = nMaxPosX - nAllowDif;
// 	int nMaxX = nMaxPosX + nAllowDif;
// 	int nMinY = nMaxPosY - nAllowDif;
// 	int nMaxY = nMaxPosY + nAllowDif;
// 	int nSearchLen = 25;
// 	int nSearchNum = (int)(0.5 * matImg.cols / nSearchLen);
// 	for (int n = 0; n < nSearchNum; n++) {
// 		Mat matROI = matImg(cv::Rect(n * nSearchLen + nHalfHlfX, 0, nSearchLen, matImg.rows)).clone();
// 		vector<float> vROIProf = HorizonProfile8U(matROI);
// 		for (int k = nMinX; k < nMaxX; k++)
// 			if (vROIProf[k + 1] >= nThresh && vROIProf[k] < nThresh) pHistX[k]++;
// 
// 		for (int k = nMinY, m = m_nSkipLenFromCenter; k < nMaxY; k++, m++)
// 			if (vROIProf[k] >= nThresh && vROIProf[k + 1] < nThresh) pHistY[m]++;
// 	}
// 	delete[] pHistX;
// 	delete[] pHistY;
// 
// 	return vPos;
// }

Mat BusbarProcess::HorizontalTriangle(Mat matImg, CRect rtROI, int nMaskLen) {
	if (rtROI.left < nMaskLen) rtROI.left = nMaskLen;
	if (matImg.cols - rtROI.right < nMaskLen)  rtROI.right = matImg.cols - nMaskLen;

	Mat matRet = Mat(rtROI.Height(), rtROI.Width(), CV_8UC1, Scalar(0));

	LPBYTE pData = matImg.data;
	LPBYTE pRet  = matRet.data;
	for (int j = rtROI.top, y = 0; j < rtROI.bottom; j++, y++) {
		int nStep = j * matImg.cols;
		int nStpR = y * matRet.cols;
		for (int i = rtROI.left, x = 0; i < rtROI.right; i++, x++) {
			int nPos = nStep + i;
			int nSum0 = 0;
			int nSum1 = 0;
			for (int k = 0; k < nMaskLen; k++) {
				nSum0 += *(pData + nPos - k);
				nSum1 += *(pData + nPos + k);
			}
			*(pRet + nStpR + i) = nSum0 - nSum1 + 126;
		}
	}
	return matRet;
}

vector<float> BusbarProcess::MaxProfile(Mat matImg, CRect rtROI, int nMaskLen) {
	vector<float> vProfMax(rtROI.Width());
	vector<float> vProfCur(rtROI.Width());

	if (rtROI.Width() == 0 || rtROI.Height() == 0 || nMaskLen == 0) {
		vProfMax.clear();
		return vProfMax;
	}

	Mat matROI = matImg(cv::Rect(rtROI.left, rtROI.top, rtROI.Width(), rtROI.Height())).clone();

	float * pData = (float *)matImg.data;
	for (int j = rtROI.top; j < rtROI.bottom - nMaskLen; j++) {
		for (int n = 0; n < rtROI.Width(); n++) vProfCur[n] = 0.0f;
		for (int k = 0; k < nMaskLen; k++) {
			int nStep = (j + k) * matImg.cols;
			for (int i = rtROI.left, x = 0; i < rtROI.right; i++, x++) {
				int nPos = nStep + i;
				vProfCur[x] += *(pData + nPos);
			}
		}
		for (int n = 0; n < rtROI.Width(); n++) vProfMax[n] = max(vProfMax[n], vProfCur[n]);
	}

	float fInv = 1.0f / nMaskLen;
	for (int n = 0; n < rtROI.Width(); n++) vProfMax[n] *= fInv;
	return vProfMax;
}

CPoint BusbarProcess::SearchEndPos(vector<float> vMaxProf, int nContinousVal, int nLen, int nMinVal) {
	CPoint ptEnd = CPoint(-1, -1);

	int nMinContinousCnt = 5;

	int nCnt1 = 0;
	for (int i = 0; i < (int)vMaxProf.size() / 2; i++) {
		float fVal = vMaxProf[i];
		if (fVal > nContinousVal) {
			nCnt1++;
		}
		else {
//			if (nCnt0 > nMinContinousCnt)
			nCnt1 = 0;
		}
		
		if (nCnt1 > nLen) {
			ptEnd.x = i - nCnt1;
			break;
		}
	}

	if (ptEnd.x == -1) return ptEnd;

	nCnt1 = 0;
	for (int i = (int)vMaxProf.size() - 1; i > (int)vMaxProf.size() / 2; i--) {
		float fVal = vMaxProf[i];
		if (fVal > nContinousVal) {
			nCnt1++;
		}
		else {
//			if(nCnt0 > nMinContinousCnt)
			nCnt1 = 0;
		}

		if (nCnt1 > nLen) {
			ptEnd.y = i + nCnt1;
			break;
		}
	}

	if (ptEnd.y == -1) return ptEnd;

	for (int i = ptEnd.x; i < ptEnd.x + nLen; i++) {
		float fVal = vMaxProf[i];
		if (fVal > nMinVal) {
			ptEnd.x = i;
			break;
		}
	}

	for (int i = ptEnd.y; i > ptEnd.y - nLen; i--) {
		float fVal = vMaxProf[i];
		if (fVal > nMinVal) {
			ptEnd.y = i;
			break;
		}
	}

	return ptEnd;
}