/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1/GPL 2.0/LGPL 2.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Mozilla Universal charset detector code.
 *
 * The Initial Developer of the Original Code is
 * Netscape Communications Corporation.
 * Portions created by the Initial Developer are Copyright (C) 2001
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *          Shy Shalom <shooshX@gmail.com>
 *          Rudi Pettazzi <rudi.pettazzi@gmail.com> (C# port)
 * 
 * Alternatively, the contents of this file may be used under the terms of
 * either the GNU General Public License Version 2 or later (the "GPL"), or
 * the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
 * in which case the provisions of the GPL or the LGPL are applicable instead
 * of those above. If you wish to allow use of your version of this file only
 * under the terms of either the GPL or the LGPL, and not to allow others to
 * use your version of this file under the terms of the MPL, indicate your
 * decision by deleting the provisions above and replace them with the notice
 * and other provisions required by the GPL or the LGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file under
 * the terms of any one of the MPL, the GPL or the LGPL.
 *
 * ***** END LICENSE BLOCK ***** */

using System;

namespace Frost.SharpCharsetDetector.Probers {

    /// <summary>Multi-byte charsets probers</summary>
    public class MBCSGroupProber : CharsetProber {
        private const int PROBERS_NUM = 7;

        private static readonly string[] ProberName = {
            "UTF8",
            "SJIS",
            "EUCJP",
            "GB18030",
            "EUCKR", 
            "Big5",
            "EUCTW"
        };

        private int _activeNum;
        private int _bestGuess;
        private readonly bool[] _isActive = new bool[PROBERS_NUM];
        private readonly CharsetProber[] _probers = new CharsetProber[PROBERS_NUM];

        public MBCSGroupProber() {
            _probers[0] = new UTF8Prober();
            _probers[1] = new SJISProber();
            _probers[2] = new EUCJPProber();
            _probers[3] = new GB18030Prober();
            _probers[4] = new EUCKRProber();
            _probers[5] = new Big5Prober();
            _probers[6] = new EUCTWProber();
            Reset();
        }

        public override string CharsetName {
            get {
                if (_bestGuess == -1) {
                    GetConfidence();
                    if (_bestGuess == -1) {
                        _bestGuess = 0;
                    }
                }
                return _probers[_bestGuess].CharsetName;
            }
        }

        /// <summary>Gets the windows code page number.</summary>
        /// <value>If supported returns a windows code page number of the encoding/charset; otherwise -1.</value>
        public override int WindowsCodePage {
            get {
                if (_bestGuess == -1) {
                    GetConfidence();
                    if (_bestGuess == -1) {
                        _bestGuess = 0;
                    }
                }
                return _probers[_bestGuess].WindowsCodePage;
            }
        }

        public override void Reset() {
            _activeNum = 0;
            for (int i = 0; i < _probers.Length; i++) {
                if (_probers[i] != null) {
                    _probers[i].Reset();
                    _isActive[i] = true;
                    ++_activeNum;
                }
                else {
                    _isActive[i] = false;
                }
            }
            _bestGuess = -1;
            State = ProbingState.Detecting;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len) {
            // do filtering to reduce load to probers
            byte[] highbyteBuf = new byte[len];
            int hptr = 0;
            //assume previous is not ascii, it will do no harm except add some noise
            bool keepNext = true;
            int max = offset + len;

            for (int i = offset; i < max; i++) {
                if ((buf[i] & 0x80) != 0) {
                    highbyteBuf[hptr++] = buf[i];
                    keepNext = true;
                }
                else {
                    //if previous is highbyte, keep this even it is a ASCII
                    if (keepNext) {
                        highbyteBuf[hptr++] = buf[i];
                        keepNext = false;
                    }
                }
            }

            for (int i = 0; i < _probers.Length; i++) {
                if (!_isActive[i]) {
                    continue;
                }
                ProbingState st = _probers[i].HandleData(highbyteBuf, 0, hptr);
                if (st == ProbingState.FoundIt) {
                    _bestGuess = i;
                    State = ProbingState.FoundIt;
                    break;
                }

                if (st == ProbingState.NotMe) {
                    _isActive[i] = false;
                    _activeNum--;
                    if (_activeNum <= 0) {
                        State = ProbingState.NotMe;
                        break;
                    }
                }
            }
            return State;
        }

        public override float GetConfidence() {
            float bestConf = 0.0f;

            if (State == ProbingState.FoundIt) {
                return 0.99f;
            }

            if (State == ProbingState.NotMe) {
                return 0.01f;
            }

            for (int i = 0; i < PROBERS_NUM; i++) {
                if (!_isActive[i]) {
                    continue;
                }
                float cf = _probers[i].GetConfidence();
                if (bestConf < cf) {
                    bestConf = cf;
                    _bestGuess = i;
                }
            }
            return bestConf;
        }

        public override void DumpStatus() {
            GetConfidence();
            for (int i = 0; i < PROBERS_NUM; i++) {
                if (!_isActive[i]) {
                    Console.WriteLine("  MBCS inactive: {0} (confidence is too low).", ProberName[i]);
                }
                else {
                    float cf = _probers[i].GetConfidence();
                    Console.WriteLine("  MBCS {0}: [{1}]", cf, ProberName[i]);
                }
            }
        }
    }

}