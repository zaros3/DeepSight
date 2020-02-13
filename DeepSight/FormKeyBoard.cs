using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DeepSight {
    public partial class FormKeyBoard : Form {

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        
        /// <summary>
        /// Shift 키 눌렀을 때
        /// </summary>
        private bool _bShift;
        public bool m_bShift 
        {
            get {
                return _bShift;
            }
            set {
                _bShift = value;
            }
        }

        /// <summary>
        /// CTRL 키 눌렀을 때
        /// </summary>
        private bool _bCtrl;
        public bool m_bCtrl {
            get {
                return _bCtrl;
            }
            set {
                _bCtrl = value;
            }
        }

        /// <summary>
        /// ALT 키 눌렀을 때
        /// </summary>
        private bool _bAlt;
        public bool m_bAlt {
            get {
                return _bAlt;
            }
            set {
                _bAlt = value;
            }
        }

        /// <summary>
        /// CapLock 키 눌렀을 때
        /// </summary>
        private bool _bCapsLock;
        public bool m_bCapsLock {
            get {
                return _bCapsLock;
            }
            set {
                _bCapsLock = value;
            }
        }

        /// <summary>
        /// Quotes 
        /// </summary>
        private string _bQuotes;
        public string m_bQuotes
        {
            get
            {
                return _bQuotes;
            }
            set
            {
                _bQuotes = value;
            }
        }

        /// <summary>
        /// 키보드 최종 리턴 값
        /// </summary>
        private string _strReturnValue;
        public string m_strReturnValue
        {
            get
            {
                return _strReturnValue;
            }
            set
            {
                _strReturnValue = value;
            }
        }

        public string m_strInputData;
        /// <summary>
        /// 키보드 생성자
        /// </summary>
        public FormKeyBoard( string strData = "" ) {
            m_strInputData = strData;
            InitializeComponent();

        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize() 
        {
            bool bReturn = false;
            do 
            {
                InitializeButton();
                m_bShift = m_bCapsLock = m_bCtrl = m_bAlt = false;
                m_bCapsLock = true;

                m_bQuotes = BtnKeyBoardCharQuotes.Text;
                Reload();
                EditKeyBoardDisplay.Text = m_strInputData;// "";
                EditKeyBoardDisplay.Focus();
                
                bReturn = true;
            } while ( false );
            return bReturn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 버튼 색상 정의
        //설명 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetButtonColor()
        {
            var pFormCommon = CFormCommon.GetFormCommon;
            this.BackColor = pFormCommon.COLOR_FORM_VIEW;

            pFormCommon.SetButtonColor( this.BtnKeyBoardTitle, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

            pFormCommon.SetButtonColor( this.BtnKeyBoardEsc, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyBoardCharAccent, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyBoardTAB, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyBoardCapsLock, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            pFormCommon.SetButtonColor( this.BtnKeyBoardLeftShift, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

        }
        
        /// <summary>
        /// 버튼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeButton()
        {
            bool bReturn = false;
            do
            {

                var pFormCommon = CFormCommon.GetFormCommon;
                this.BackColor = pFormCommon.COLOR_FORM_VIEW;

                pFormCommon.SetButtonColor( this.BtnKeyBoardTitle, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_TITLE );

                pFormCommon.SetButtonColor( this.BtnKeyBoardEsc, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharAccent, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardTAB, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCapsLock, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardLeftShift, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar0, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar1, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar2, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar3, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar4, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar5, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar6, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar7, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar8, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardChar9, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

                pFormCommon.SetButtonColor( this.BtnKeyBoardCharQ, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharW, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharE, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharT, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharY, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharU, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharI, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharO, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharP, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharA, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharS, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharD, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharF, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharG, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharH, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharJ, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharK, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharL, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharZ, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharX, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharC, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharV, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharB, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharN, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharM, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharR, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

                pFormCommon.SetButtonColor( this.BtnKeyBoardCharLeftBracket, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharRightBracket, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharSemicolon, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharQuotes, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharComma, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharPoint, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharSlash, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

                pFormCommon.SetButtonColor( this.BtnKeyBoardRightShift, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardEnter, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardSpace, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardBackSpace, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardEqual, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardClear, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardAlt, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCtrl, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardCharBackSlash, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                pFormCommon.SetButtonColor( this.BtnKeyBoardHyphen, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );

                //BtnKeyBoardTitle.BackColor = SystemColors.Control;
                //BtnKeyBoardRightShift.BackColor = SystemColors.Control;
                //BtnKeyBoardLeftShift.BackColor = SystemColors.Control;
                //BtnKeyBoardCharAccent.BackColor = SystemColors.Control;
                //BtnKeyBoardChar0.BackColor = SystemColors.Control;
                //BtnKeyBoardChar1.BackColor = SystemColors.Control;
                //BtnKeyBoardChar2.BackColor = SystemColors.Control;
                //BtnKeyBoardChar3.BackColor = SystemColors.Control;
                //BtnKeyBoardChar4.BackColor = SystemColors.Control;
                //BtnKeyBoardChar5.BackColor = SystemColors.Control;
                //BtnKeyBoardChar6.BackColor = SystemColors.Control;
                //BtnKeyBoardChar7.BackColor = SystemColors.Control;
                //BtnKeyBoardChar8.BackColor = SystemColors.Control;
                //BtnKeyBoardChar9.BackColor = SystemColors.Control;
                //BtnKeyBoardCharQ.BackColor = SystemColors.Control;
                //BtnKeyBoardCharW.BackColor = SystemColors.Control;
                //BtnKeyBoardCharE.BackColor = SystemColors.Control;
                //BtnKeyBoardCharR.BackColor = SystemColors.Control;
                //BtnKeyBoardCharT.BackColor = SystemColors.Control;
                //BtnKeyBoardCharY.BackColor = SystemColors.Control;
                //BtnKeyBoardCharU.BackColor = SystemColors.Control;
                //BtnKeyBoardCharI.BackColor = SystemColors.Control;
                //BtnKeyBoardCharO.BackColor = SystemColors.Control;
                //BtnKeyBoardCharP.BackColor = SystemColors.Control;
                //BtnKeyBoardCharA.BackColor = SystemColors.Control;
                //BtnKeyBoardCharS.BackColor = SystemColors.Control;
                //BtnKeyBoardCharD.BackColor = SystemColors.Control;
                //BtnKeyBoardCharF.BackColor = SystemColors.Control;
                //BtnKeyBoardCharG.BackColor = SystemColors.Control;
                //BtnKeyBoardCharH.BackColor = SystemColors.Control;
                //BtnKeyBoardCharJ.BackColor = SystemColors.Control;
                //BtnKeyBoardCharK.BackColor = SystemColors.Control;
                //BtnKeyBoardCharL.BackColor = SystemColors.Control;
                //BtnKeyBoardCharZ.BackColor = SystemColors.Control;
                //BtnKeyBoardCharX.BackColor = SystemColors.Control;
                //BtnKeyBoardCharC.BackColor = SystemColors.Control;
                //BtnKeyBoardCharV.BackColor = SystemColors.Control;
                //BtnKeyBoardCharB.BackColor = SystemColors.Control;
                //BtnKeyBoardCharN.BackColor = SystemColors.Control;
                //BtnKeyBoardCharM.BackColor = SystemColors.Control;
                //BtnKeyBoardCharLeftBracket.BackColor = SystemColors.Control;
                //BtnKeyBoardCharRightBracket.BackColor = SystemColors.Control;
                //BtnKeyBoardCharSemicolon.BackColor = SystemColors.Control;
                //BtnKeyBoardCharQuotes.BackColor = SystemColors.Control;
                //BtnKeyBoardCharComma.BackColor = SystemColors.Control;
                //BtnKeyBoardCharPoint.BackColor = SystemColors.Control;
                //BtnKeyBoardCharSlash.BackColor = SystemColors.Control;

                //BtnKeyBoardLeftShift.BackColor = SystemColors.Control;
                //BtnKeyBoardRightShift.BackColor = SystemColors.Control;
                //BtnKeyBoardTAB.BackColor = SystemColors.Control;
                //BtnKeyBoardCapsLock.BackColor = SystemColors.Control;
                //BtnKeyBoardEnter.BackColor = SystemColors.Control;
                //BtnKeyBoardSpace.BackColor = SystemColors.Control;
                //BtnKeyBoardBackSpace.BackColor = SystemColors.Control;
                //BtnKeyBoardEsc.BackColor = SystemColors.Control;
                //BtnKeyBoardEqual.BackColor = SystemColors.Control;
                //BtnKeyBoardClear.BackColor = SystemColors.Control;
                //BtnKeyBoardAlt.BackColor = SystemColors.Control;
                //BtnKeyBoardCtrl.BackColor = SystemColors.Control;
                //BtnKeyBoardCharBackSlash.BackColor = SystemColors.Control;
                //BtnKeyBoardHyphen.BackColor = SystemColors.Control;
                
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Shift / CapLock 키 눌렀을때 다시 표시.
        /// </summary>
        private void Reload() 
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            if (true == m_bShift) {
                BtnKeyBoardRightShift.BackColor = Color.LimeGreen;
                BtnKeyBoardLeftShift.BackColor = Color.LimeGreen;
                BtnKeyBoardCharAccent.Text = "~";
                BtnKeyBoardChar0.Text = ")";                                BtnKeyBoardChar1.Text = "!";
                BtnKeyBoardChar2.Text = "@";                                BtnKeyBoardChar3.Text = "#";
                BtnKeyBoardChar4.Text = "$";                                BtnKeyBoardChar5.Text = "%";
                BtnKeyBoardChar6.Text = "^";                                BtnKeyBoardChar7.Text = "&&";
                BtnKeyBoardChar8.Text = "*";                                BtnKeyBoardChar9.Text = "(";
                BtnKeyBoardHyphen.Text = "_";                               BtnKeyBoardEqual.Text = "+";
                BtnKeyBoardCharQ.Text = BtnKeyBoardCharQ.Text.ToUpper();
                BtnKeyBoardCharW.Text = BtnKeyBoardCharW.Text.ToUpper();
                BtnKeyBoardCharE.Text = BtnKeyBoardCharE.Text.ToUpper();
                BtnKeyBoardCharR.Text = BtnKeyBoardCharR.Text.ToUpper();
                BtnKeyBoardCharT.Text = BtnKeyBoardCharT.Text.ToUpper();
                BtnKeyBoardCharY.Text = BtnKeyBoardCharY.Text.ToUpper();
                BtnKeyBoardCharU.Text = BtnKeyBoardCharU.Text.ToUpper();
                BtnKeyBoardCharI.Text = BtnKeyBoardCharI.Text.ToUpper();
                BtnKeyBoardCharO.Text = BtnKeyBoardCharO.Text.ToUpper();
                BtnKeyBoardCharP.Text = BtnKeyBoardCharP.Text.ToUpper();
                BtnKeyBoardCharA.Text = BtnKeyBoardCharA.Text.ToUpper();
                BtnKeyBoardCharS.Text = BtnKeyBoardCharS.Text.ToUpper();
                BtnKeyBoardCharD.Text = BtnKeyBoardCharD.Text.ToUpper();
                BtnKeyBoardCharF.Text = BtnKeyBoardCharF.Text.ToUpper();
                BtnKeyBoardCharG.Text = BtnKeyBoardCharG.Text.ToUpper();
                BtnKeyBoardCharH.Text = BtnKeyBoardCharH.Text.ToUpper();
                BtnKeyBoardCharJ.Text = BtnKeyBoardCharJ.Text.ToUpper();
                BtnKeyBoardCharK.Text = BtnKeyBoardCharK.Text.ToUpper();
                BtnKeyBoardCharL.Text = BtnKeyBoardCharL.Text.ToUpper();
                BtnKeyBoardCharZ.Text = BtnKeyBoardCharZ.Text.ToUpper();
                BtnKeyBoardCharX.Text = BtnKeyBoardCharX.Text.ToUpper();
                BtnKeyBoardCharC.Text = BtnKeyBoardCharC.Text.ToUpper();
                BtnKeyBoardCharV.Text = BtnKeyBoardCharV.Text.ToUpper();
                BtnKeyBoardCharB.Text = BtnKeyBoardCharB.Text.ToUpper();
                BtnKeyBoardCharN.Text = BtnKeyBoardCharN.Text.ToUpper();
                BtnKeyBoardCharM.Text = BtnKeyBoardCharM.Text.ToUpper();
                BtnKeyBoardCharLeftBracket.Text = "{";                      BtnKeyBoardCharRightBracket.Text = "}";
                BtnKeyBoardCharSemicolon.Text = ":";                        BtnKeyBoardCharQuotes.Text = m_bQuotes;
                BtnKeyBoardCharComma.Text = "<";                            BtnKeyBoardCharPoint.Text = ">";
                BtnKeyBoardCharSlash.Text = "?";                            BtnKeyBoardCharBackSlash.Text = "|";
            } else {
                pFormCommon.SetButtonColor( this.BtnKeyBoardRightShift, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                //BtnKeyBoardRightShift.BackColor = SystemColors.Control;
                pFormCommon.SetButtonColor( this.BtnKeyBoardLeftShift, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
               // BtnKeyBoardLeftShift.BackColor = SystemColors.Control;
                BtnKeyBoardCharAccent.Text = "`";                           BtnKeyBoardChar0.Text = "0";
                BtnKeyBoardChar1.Text = "1";                                BtnKeyBoardChar2.Text = "2";
                BtnKeyBoardChar3.Text = "3";                                BtnKeyBoardChar4.Text = "4";
                BtnKeyBoardChar5.Text = "5";                                BtnKeyBoardChar6.Text = "6";
                BtnKeyBoardChar7.Text = "7";                                BtnKeyBoardChar8.Text = "8";
                BtnKeyBoardChar9.Text = "9";                                BtnKeyBoardHyphen.Text = "-";
                BtnKeyBoardEqual.Text = "=";    
                BtnKeyBoardCharQ.Text = BtnKeyBoardCharQ.Text.ToLower();
                BtnKeyBoardCharW.Text = BtnKeyBoardCharW.Text.ToLower();
                BtnKeyBoardCharE.Text = BtnKeyBoardCharE.Text.ToLower();
                BtnKeyBoardCharR.Text = BtnKeyBoardCharR.Text.ToLower();
                BtnKeyBoardCharT.Text = BtnKeyBoardCharT.Text.ToLower();
                BtnKeyBoardCharY.Text = BtnKeyBoardCharY.Text.ToLower();
                BtnKeyBoardCharU.Text = BtnKeyBoardCharU.Text.ToLower();
                BtnKeyBoardCharI.Text = BtnKeyBoardCharI.Text.ToLower();
                BtnKeyBoardCharO.Text = BtnKeyBoardCharO.Text.ToLower();
                BtnKeyBoardCharP.Text = BtnKeyBoardCharP.Text.ToLower();
                BtnKeyBoardCharA.Text = BtnKeyBoardCharA.Text.ToLower();
                BtnKeyBoardCharS.Text = BtnKeyBoardCharS.Text.ToLower();
                BtnKeyBoardCharD.Text = BtnKeyBoardCharD.Text.ToLower();
                BtnKeyBoardCharF.Text = BtnKeyBoardCharF.Text.ToLower();
                BtnKeyBoardCharG.Text = BtnKeyBoardCharG.Text.ToLower();
                BtnKeyBoardCharH.Text = BtnKeyBoardCharH.Text.ToLower();
                BtnKeyBoardCharJ.Text = BtnKeyBoardCharJ.Text.ToLower();
                BtnKeyBoardCharK.Text = BtnKeyBoardCharK.Text.ToLower();
                BtnKeyBoardCharL.Text = BtnKeyBoardCharL.Text.ToLower();
                BtnKeyBoardCharZ.Text = BtnKeyBoardCharZ.Text.ToLower();
                BtnKeyBoardCharX.Text = BtnKeyBoardCharX.Text.ToLower();
                BtnKeyBoardCharC.Text = BtnKeyBoardCharC.Text.ToLower();
                BtnKeyBoardCharV.Text = BtnKeyBoardCharV.Text.ToLower();
                BtnKeyBoardCharB.Text = BtnKeyBoardCharB.Text.ToLower();
                BtnKeyBoardCharN.Text = BtnKeyBoardCharN.Text.ToLower();
                BtnKeyBoardCharM.Text = BtnKeyBoardCharM.Text.ToLower();
                BtnKeyBoardCharLeftBracket.Text = "[";                      BtnKeyBoardCharRightBracket.Text = "]";
                BtnKeyBoardCharSemicolon.Text = ";";                        BtnKeyBoardCharQuotes.Text = "'";
                BtnKeyBoardCharComma.Text = ",";                            BtnKeyBoardCharPoint.Text = ".";
                BtnKeyBoardCharSlash.Text = "/";                            BtnKeyBoardCharBackSlash.Text = "\\";
            }

            if( true == m_bCapsLock )
            {
                BtnKeyBoardCapsLock.BackColor = Color.LimeGreen;

                BtnKeyBoardCharQ.Text = BtnKeyBoardCharQ.Text.ToUpper();
                BtnKeyBoardCharW.Text = BtnKeyBoardCharW.Text.ToUpper();
                BtnKeyBoardCharE.Text = BtnKeyBoardCharE.Text.ToUpper();
                BtnKeyBoardCharR.Text = BtnKeyBoardCharR.Text.ToUpper();
                BtnKeyBoardCharT.Text = BtnKeyBoardCharT.Text.ToUpper();
                BtnKeyBoardCharY.Text = BtnKeyBoardCharY.Text.ToUpper();
                BtnKeyBoardCharU.Text = BtnKeyBoardCharU.Text.ToUpper();
                BtnKeyBoardCharI.Text = BtnKeyBoardCharI.Text.ToUpper();
                BtnKeyBoardCharO.Text = BtnKeyBoardCharO.Text.ToUpper();
                BtnKeyBoardCharP.Text = BtnKeyBoardCharP.Text.ToUpper();
                BtnKeyBoardCharA.Text = BtnKeyBoardCharA.Text.ToUpper();
                BtnKeyBoardCharS.Text = BtnKeyBoardCharS.Text.ToUpper();
                BtnKeyBoardCharD.Text = BtnKeyBoardCharD.Text.ToUpper();
                BtnKeyBoardCharF.Text = BtnKeyBoardCharF.Text.ToUpper();
                BtnKeyBoardCharG.Text = BtnKeyBoardCharG.Text.ToUpper();
                BtnKeyBoardCharH.Text = BtnKeyBoardCharH.Text.ToUpper();
                BtnKeyBoardCharJ.Text = BtnKeyBoardCharJ.Text.ToUpper();
                BtnKeyBoardCharK.Text = BtnKeyBoardCharK.Text.ToUpper();
                BtnKeyBoardCharL.Text = BtnKeyBoardCharL.Text.ToUpper();
                BtnKeyBoardCharZ.Text = BtnKeyBoardCharZ.Text.ToUpper();
                BtnKeyBoardCharX.Text = BtnKeyBoardCharX.Text.ToUpper();
                BtnKeyBoardCharC.Text = BtnKeyBoardCharC.Text.ToUpper();
                BtnKeyBoardCharV.Text = BtnKeyBoardCharV.Text.ToUpper();
                BtnKeyBoardCharB.Text = BtnKeyBoardCharB.Text.ToUpper();
                BtnKeyBoardCharN.Text = BtnKeyBoardCharN.Text.ToUpper();
                BtnKeyBoardCharM.Text = BtnKeyBoardCharM.Text.ToUpper();
            } 
            else
            {
                if (true == m_bShift)
                {
                    //BtnKeyBoardCapsLock.BackColor = SystemColors.Control;
                    pFormCommon.SetButtonColor( this.BtnKeyBoardCapsLock, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                } 
                else
                {
                    //BtnKeyBoardCapsLock.BackColor = SystemColors.Control;
                    pFormCommon.SetButtonColor( this.BtnKeyBoardCapsLock, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
                    BtnKeyBoardCharQ.Text = BtnKeyBoardCharQ.Text.ToLower();
                    BtnKeyBoardCharW.Text = BtnKeyBoardCharW.Text.ToLower();
                    BtnKeyBoardCharE.Text = BtnKeyBoardCharE.Text.ToLower();
                    BtnKeyBoardCharR.Text = BtnKeyBoardCharR.Text.ToLower();
                    BtnKeyBoardCharT.Text = BtnKeyBoardCharT.Text.ToLower();
                    BtnKeyBoardCharY.Text = BtnKeyBoardCharY.Text.ToLower();
                    BtnKeyBoardCharU.Text = BtnKeyBoardCharU.Text.ToLower();
                    BtnKeyBoardCharI.Text = BtnKeyBoardCharI.Text.ToLower();
                    BtnKeyBoardCharO.Text = BtnKeyBoardCharO.Text.ToLower();
                    BtnKeyBoardCharP.Text = BtnKeyBoardCharP.Text.ToLower();
                    BtnKeyBoardCharA.Text = BtnKeyBoardCharA.Text.ToLower();
                    BtnKeyBoardCharS.Text = BtnKeyBoardCharS.Text.ToLower();
                    BtnKeyBoardCharD.Text = BtnKeyBoardCharD.Text.ToLower();
                    BtnKeyBoardCharF.Text = BtnKeyBoardCharF.Text.ToLower();
                    BtnKeyBoardCharG.Text = BtnKeyBoardCharG.Text.ToLower();
                    BtnKeyBoardCharH.Text = BtnKeyBoardCharH.Text.ToLower();
                    BtnKeyBoardCharJ.Text = BtnKeyBoardCharJ.Text.ToLower();
                    BtnKeyBoardCharK.Text = BtnKeyBoardCharK.Text.ToLower();
                    BtnKeyBoardCharL.Text = BtnKeyBoardCharL.Text.ToLower();
                    BtnKeyBoardCharZ.Text = BtnKeyBoardCharZ.Text.ToLower();
                    BtnKeyBoardCharX.Text = BtnKeyBoardCharX.Text.ToLower();
                    BtnKeyBoardCharC.Text = BtnKeyBoardCharC.Text.ToLower();
                    BtnKeyBoardCharV.Text = BtnKeyBoardCharV.Text.ToLower();
                    BtnKeyBoardCharB.Text = BtnKeyBoardCharB.Text.ToLower();
                    BtnKeyBoardCharN.Text = BtnKeyBoardCharN.Text.ToLower();
                    BtnKeyBoardCharM.Text = BtnKeyBoardCharM.Text.ToLower();
                }
                
            }
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardEsc_Click( object sender, EventArgs e )
        {
            FormKeyPad.ActiveForm.DialogResult = DialogResult.Cancel;
            FormKeyPad.ActiveForm.Close();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardTAB_Click( object sender, EventArgs e )
        {
            string strbuf, strbuff;
            strbuf = EditKeyBoardDisplay.Text; 
            strbuff = string.Format( "{0}{1}", strbuf, "    " );
            EditKeyBoardDisplay.Text = strbuff;

            EditKeyBoardDisplay.Select( 0, strbuff.Length );
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCapsLock_Click( object sender, EventArgs e )
        {
            if( true == m_bCapsLock ) m_bCapsLock = false;
            else                      m_bCapsLock = true;
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardLeftShift_Click( object sender, EventArgs e )
        {
            if (true == m_bShift) {
                m_bShift = false;
                BtnKeyBoardLeftShift.BackColor = SystemColors.Control;
            } else {
                m_bShift = true;
                BtnKeyBoardLeftShift.BackColor = Color.LimeGreen;
            }

            Reload();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        // 생성 : 
        // 인자 : 
        // 리턴 : 
        // 상세 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnKeyBoardCtrl_Click( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            if (true == m_bCtrl)
            {
                m_bCtrl = false;
                //BtnKeyBoardCtrl.BackColor = SystemColors.Control;
                pFormCommon.SetButtonColor( this.BtnKeyBoardCtrl, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            } else
            {
                m_bCtrl = true;
                BtnKeyBoardCtrl.BackColor = Color.LimeGreen;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        // 생성 : 
        // 인자 : 
        // 리턴 : 
        // 상세 : 
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BtnKeyBoardAlt_Click( object sender, EventArgs e )
        {
            var pFormCommon = CFormCommon.GetFormCommon;

            if (true == m_bAlt)
            {
                m_bAlt = false;
                //BtnKeyBoardAlt.BackColor = SystemColors.Control;
                pFormCommon.SetButtonColor( this.BtnKeyBoardAlt, pFormCommon.COLOR_WHITE, pFormCommon.COLOR_UNACTIVATE );
            } else
            {
                m_bAlt = true;
                BtnKeyBoardAlt.BackColor = Color.LimeGreen;
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardSpace_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += " ";
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardClear_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text = "";
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardRightShift_Click( object sender, EventArgs e )
        {
            if (true == m_bShift)
            {
                m_bShift = false;
                BtnKeyBoardRightShift.BackColor = SystemColors.Control;
            } else
            {
                m_bShift = true;
                BtnKeyBoardRightShift.BackColor = Color.LimeGreen;
            }

            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardEnter_Click( object sender, EventArgs e )
        {
            m_strReturnValue = EditKeyBoardDisplay.Text;
            FormKeyPad.ActiveForm.DialogResult = DialogResult.OK;
            FormKeyPad.ActiveForm.Close();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardBackSpace_Click( object sender, EventArgs e )
        {
            string strConvertText = "";

            if (0 == EditKeyBoardDisplay.Text.Length)
                return;
            if (1 <= EditKeyBoardDisplay.Text.Length)
            {
                strConvertText = EditKeyBoardDisplay.Text.Substring( 0, EditKeyBoardDisplay.Text.Length - 1 );
                EditKeyBoardDisplay.Text = strConvertText;
                EditKeyBoardDisplay.Focus();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharAccent_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharAccent.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar1_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar1.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar2_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar2.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar3_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar3.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar4_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar4.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar5_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar5.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar6_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar6.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar7_Click( object sender, EventArgs e )
        {
            if (BtnKeyBoardChar7.Text.Length > 1)
            {
                EditKeyBoardDisplay.Text += BtnKeyBoardChar7.Text.Substring( 1 );
            } 
            else
            {
                EditKeyBoardDisplay.Text += BtnKeyBoardChar7.Text;
            }
            
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar8_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar8.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar9_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar9.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardChar0_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardChar0.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardHyphen_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardHyphen.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardEqual_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardEqual.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharQ_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharQ.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharW_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharW.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharE_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharE.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharR_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharR.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharT_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharT.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharY_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharY.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharU_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharU.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharI_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharI.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharO_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharO.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharP_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharP.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharLeftBracket_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharLeftBracket.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharRightBracket_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharRightBracket.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharBackSlash_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharBackSlash.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharA_Click( object sender, EventArgs e )
        {
            if (true == m_bCtrl)
            {
                m_bCtrl = false;
                BtnKeyBoardCtrl.BackColor = SystemColors.Control;
                EditKeyBoardDisplay.Focus();
                EditKeyBoardDisplay.Select( 0, EditKeyBoardDisplay.Text.Length );
                
            } else
            {
                EditKeyBoardDisplay.Text += BtnKeyBoardCharA.Text;
                EditKeyBoardDisplay.Focus();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharS_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharS.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharD_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharD.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharF_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharF.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharG_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharG.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharH_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharH.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharJ_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharJ.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharK_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharK.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharL_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharL.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharSemicolon_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharSemicolon.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharQuotes_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharQuotes.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharZ_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharZ.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharX_Click( object sender, EventArgs e )
        {
            if (true == m_bCtrl)
            {
                m_bCtrl = false;
                BtnKeyBoardCtrl.BackColor = SystemColors.Control;
                EditKeyBoardDisplay.Focus();
                EditKeyBoardDisplay.Cut();
            } else
            {
                EditKeyBoardDisplay.Text += BtnKeyBoardCharX.Text;
                EditKeyBoardDisplay.Focus();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharC_Click( object sender, EventArgs e )
        {
            if (true == m_bCtrl)
            {
                m_bCtrl = false;
                BtnKeyBoardCtrl.BackColor = SystemColors.Control;
                EditKeyBoardDisplay.Focus();
                EditKeyBoardDisplay.Copy();
            } 
            else
            {
                EditKeyBoardDisplay.Text += BtnKeyBoardCharC.Text;
                EditKeyBoardDisplay.Focus();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharV_Click( object sender, EventArgs e )
        {
            if (true == m_bCtrl)
            {
                m_bCtrl = false;
                BtnKeyBoardCtrl.BackColor = SystemColors.Control;
                EditKeyBoardDisplay.Focus();
                EditKeyBoardDisplay.Paste();
            } else
            {
                EditKeyBoardDisplay.Text += BtnKeyBoardCharV.Text;
                EditKeyBoardDisplay.Focus();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharB_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharB.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharN_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharN.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharM_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharM.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharComma_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharComma.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharPoint_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharPoint.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCharSlash_Click( object sender, EventArgs e )
        {
            EditKeyBoardDisplay.Text += BtnKeyBoardCharSlash.Text;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormKeyBoard_Load( object sender, EventArgs e )
        {
            Initialize();
            EditKeyBoardDisplay.Text = m_strInputData;// "";
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditKeyBoardDisplay_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.KeyCode == Keys.Enter ) {
                m_strReturnValue = EditKeyBoardDisplay.Text;
                FormKeyPad.ActiveForm.DialogResult = DialogResult.OK;
                FormKeyPad.ActiveForm.Close();
            }
//             if( e.Control && e.KeyCode == Keys.A )
//             {
//                 EditKeyBoardDisplay.Focus();
//                 EditKeyBoardDisplay.SelectAll();
//             } 
//             else if ( e.Control && e.KeyCode == Keys.C )
//             {
//                 EditKeyBoardDisplay.Focus();
//                 EditKeyBoardDisplay.Copy();
//             } 
//             else if (e.Control && e.KeyCode == Keys.V)
//             {
//                 EditKeyBoardDisplay.Focus();
//                 EditKeyBoardDisplay.Paste();
//             } 
//             else if (e.Control && e.KeyCode == Keys.X)
//             {
//                 EditKeyBoardDisplay.Focus();
//                 EditKeyBoardDisplay.Cut();
//             } 
//             else if (e.KeyData == Keys.CapsLock)
//             {
//                 if (true == m_bCapsLock)
//                 {
//                     m_bCapsLock = false;
//                     BtnKeyBoardCapsLock.BackColor = SystemColors.Control;
//                 } 
//                 else
//                 {
//                     m_bCapsLock = true;
//                     BtnKeyBoardCapsLock.BackColor = Color.LimeGreen;
//                 }
//                 Reload();
//             } 
//             else if (e.Shift )
//             {
//                 if (true == m_bShift)
//                 {
//                     m_bShift = false;
//                     keybd_event( (byte)0x14, (byte)0x45, 1, 0 );
//                     keybd_event( (byte)0x14, (byte)0x45, 1 | 2, 0 );
//                     BtnKeyBoardLeftShift.BackColor = SystemColors.Control;
//                     BtnKeyBoardRightShift.BackColor = SystemColors.Control;
//                 } else
//                 {
//                     m_bShift = true;
//                     keybd_event( (byte)0x14, (byte)0x45, 1, 1 );
//                     if( e.KeyData == Keys.LShiftKey)
//                         BtnKeyBoardLeftShift.BackColor = Color.LimeGreen;
//                     else
//                         BtnKeyBoardRightShift.BackColor = Color.LimeGreen;
//                 }
//                 Reload();
//             }
        }
    }
}
