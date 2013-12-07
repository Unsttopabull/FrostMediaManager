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

using Frost.SharpCharsetDetector.Models;
using Frost.SharpCharsetDetector.Models.SMModels;

namespace Frost.SharpCharsetDetector.Probers {

    public class EscCharsetProber : CharsetProber {
        private const int CHARSETS_NUM = 4;
        private int _activeSM;
        private readonly CodingStateMachine[] _codingSM;
        private string _detectedCharset;
        private int _winCodePage;

        public EscCharsetProber() {
            _codingSM = new CodingStateMachine[CHARSETS_NUM];
            _codingSM[0] = new CodingStateMachine(new HzSMModel());
            _codingSM[1] = new CodingStateMachine(new ISO2022CnSMModel());
            _codingSM[2] = new CodingStateMachine(new ISO2022JpSMModel());
            _codingSM[3] = new CodingStateMachine(new ISO2022KrSMModel());

            Reset();
        }

        public override void Reset() {
            State = ProbingState.Detecting;
            for (int i = 0; i < CHARSETS_NUM; i++) {
                _codingSM[i].Reset();
            }
            _activeSM = CHARSETS_NUM;
            _detectedCharset = null;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len) {
            int max = offset + len;

            for (int i = offset; i < max && State == ProbingState.Detecting; i++) {
                for (int j = _activeSM - 1; j >= 0; j--) {
                    // byte is feed to all active state machine
                    int codingState = _codingSM[j].NextState(buf[i]);
                    if (codingState == SMModel.ERROR) {
                        // got negative answer for this state machine, make it inactive
                        _activeSM--;
                        if (_activeSM == 0) {
                            State = ProbingState.NotMe;
                            return State;
                        }

                        if (j != _activeSM) {
                            CodingStateMachine t = _codingSM[_activeSM];
                            _codingSM[_activeSM] = _codingSM[j];
                            _codingSM[j] = t;
                        }
                    }
                    else if (codingState == SMModel.ITSME) {
                        State = ProbingState.FoundIt;
                        _detectedCharset = _codingSM[j].ModelName;
                        _winCodePage = _codingSM[j].CodePage;
                        return State;
                    }
                }
            }
            return State;
        }

        public override string CharsetName {
            get { return _detectedCharset; }
        }

        /// <summary>Gets the windows code page number.</summary>
        /// <value>If supported returns a windows code page number of the encoding/charset; otherwise -1.</value>
        public override int WindowsCodePage {
            get { return _winCodePage; }
        }

        public override float GetConfidence() {
            return 0.99f;
        }
    }

}