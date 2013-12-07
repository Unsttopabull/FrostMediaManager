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

using Frost.SharpCharsetDetector.ContextAnalysers;
using Frost.SharpCharsetDetector.DistributionAnalysers;
using Frost.SharpCharsetDetector.Models;
using Frost.SharpCharsetDetector.Models.SMModels;

namespace Frost.SharpCharsetDetector.Probers {

    /// <summary>For S-JIS encoding, observe characteristic:\n
    /// 1, kana character (or hankaku?) often have hight frequency of appereance\n
    /// 2, kana character often exist in group\n
    /// 3, certain combination of kana is never used in japanese language.
    /// </summary>
    public class SJISProber : CharsetProber {
        private readonly CodingStateMachine _codingSM;
        private readonly SJISContextAnalyser _contextAnalyser;
        private readonly SJISDistributionAnalyser _distributionAnalyser;
        private readonly byte[] _lastChar = new byte[2];

        public SJISProber() {
            _codingSM = new CodingStateMachine(new SjisSMModel());
            _distributionAnalyser = new SJISDistributionAnalyser();
            _contextAnalyser = new SJISContextAnalyser();
            Reset();
        }

        public override string CharsetName {
            get { return "Shift-JIS"; }
        }

        /// <summary>Gets the windows code page number.</summary>
        /// <value>If supported returns a windows code page number of the encoding/charset; otherwise -1.</value>
        public override int WindowsCodePage {
            get { return 932; }
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len) {
            int max = offset + len;

            for (int i = offset; i < max; i++) {
                int codingState = _codingSM.NextState(buf[i]);
                if (codingState == SMModel.ERROR) {
                    State = ProbingState.NotMe;
                    break;
                }
                if (codingState == SMModel.ITSME) {
                    State = ProbingState.FoundIt;
                    break;
                }
                if (codingState == SMModel.START) {
                    int charLen = _codingSM.CurrentCharLen;
                    if (i == offset) {
                        _lastChar[1] = buf[offset];
                        _contextAnalyser.HandleOneChar(_lastChar, 2 - charLen, charLen);
                        _distributionAnalyser.HandleOneChar(_lastChar, 0, charLen);
                    }
                    else {
                        _contextAnalyser.HandleOneChar(buf, i + 1 - charLen, charLen);
                        _distributionAnalyser.HandleOneChar(buf, i - 1, charLen);
                    }
                }
            }
            _lastChar[0] = buf[max - 1];
            if (State == ProbingState.Detecting) {
                if (_contextAnalyser.GotEnoughData() && GetConfidence() > SHORTCUT_THRESHOLD) {
                    State = ProbingState.FoundIt;
                }
            }
            return State;
        }

        public override void Reset() {
            _codingSM.Reset();
            State = ProbingState.Detecting;
            _contextAnalyser.Reset();
            _distributionAnalyser.Reset();
        }

        public override float GetConfidence() {
            float contxtCf = _contextAnalyser.GetConfidence();
            float distribCf = _distributionAnalyser.GetConfidence();
            return (contxtCf > distribCf ? contxtCf : distribCf);
        }
    }

}