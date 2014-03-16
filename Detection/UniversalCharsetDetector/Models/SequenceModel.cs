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

namespace Frost.SharpCharsetDetector.Models {

    public abstract class SequenceModel {
        ///<summary>[256] table use to find a char's order</summary>
        protected readonly byte[] CharToOrderMap;

        ///<summary>[SAMPLE_SIZE][SAMPLE_SIZE] table to find a 2-char sequence's frequency</summary>
        protected readonly byte[] PrecedenceMatrix;

        public SequenceModel(byte[] charToOrderMap, byte[] precedenceMatrix, float typicalPositiveRatio,bool keepEnglishLetter, string charsetName, int winCodePage) {
            CharToOrderMap = charToOrderMap;
            PrecedenceMatrix = precedenceMatrix;
            TypicalPositiveRatio = typicalPositiveRatio;
            KeepEnglishLetter = keepEnglishLetter;
            CharsetName = charsetName;
            WindowsCodePage = winCodePage;
        }

        ///<summary>freqSeqs / totalSeqs</summary>
        public float TypicalPositiveRatio { get; protected set; }

        ///<summary>not used</summary>
        public bool KeepEnglishLetter { get; protected set; }

        public string CharsetName { get; private set; }

        public int WindowsCodePage { get; private set; }

        public byte GetOrder(byte b) {
            return CharToOrderMap[b];
        }

        public byte GetPrecedence(int pos) {
            return PrecedenceMatrix[pos];
        }
    }

}