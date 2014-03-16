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
using System.IO;
using System.Text;
using Frost.SharpCharsetDetector.Probers;

namespace Frost.SharpCharsetDetector {

    internal enum InputState {
        PureASCII = 0,
        EscASCII = 1,
        Highbyte = 2
    };

    public enum LanguageFilter {
        ChineseSimplified = 0x1,
        ChineseTraditional = 0x2,
        Japanese = 0x4,
        Korean = 0x8,
        All = 0x1f,
        NonCjk = 0x10,
        Chinese = ChineseSimplified | ChineseTraditional,
        Cjk = Japanese | Korean | ChineseSimplified | ChineseTraditional
    }

    public class UniversalDetector {
        protected const float SHORTCUT_THRESHOLD = 0.95f;

        private const float MINIMUM_THRESHOLD = 0.20f;
        private const int PROBERS_NUM = 3;

        private readonly CharsetProber[] _charsetProbers = new CharsetProber[PROBERS_NUM];
        private int _winCodePage = -1;
        private CharsetProber _escCharsetProber;
        private bool _gotData;
        private InputState _inputState;
        private byte _lastChar;
        private bool _start;
        //private LanguageFilter _languageFilter;
        //private int _bestGuess;

        //protected UniversalDetector(LanguageFilter languageFilter) : this() {
        //    _bestGuess = -1;
        //    _languageFilter = languageFilter;
        //}

        public UniversalDetector() {
            _start = true;
            _inputState = InputState.PureASCII;
            _lastChar = 0x00;
        }

        /// <summary>The detected charset. It can be null.</summary>
        public string DetectedCharset { get; private set; }

        /// <summary>Gets a value indicating whether the detected encoding is a supported encoding .NET.</summary>
        /// <value>Is <c>true</c> if the detected encoding is a supported encoding in .NET; otherwise, <c>false</c>.</value>
        public bool IsSupportedEncoding {get { return _winCodePage > 0 && _winCodePage < 65535; }}

        public Encoding DetectedEncoding {
            get {
                if (_winCodePage != -1) {
                    return Encoding.GetEncoding(_winCodePage);
                }
                throw new NotSupportedException("The encoding is not supported in .NET.");
            }
        }

        /// <summary>The confidence of the detected charset, if any.</summary>
        public float Confidence { get; private set; }

        /// <summary>Returns true if the detector has found a result and it is sure about it.</summary>
        /// <returns>Returns <c>true</c> if the detector has detected the encoding; otherwise <c>false</c>.</returns>
        public bool IsDone { get; private set; }

        /// <summary>Feed a block of bytes to the detector.</summary>
        /// <param name="buf">input buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="len">number of available bytes</param>
        public virtual void Feed(byte[] buf, int offset, int len) {
            if (IsDone) {
                return;
            }

            if (len > 0) {
                _gotData = true;
            }

            // If the data starts with BOM, we know it is UTF
            if (_start) {
                _start = false;
                if (len > 3) {
                    switch (buf[0]) {
                        case 0xEF:
                            if (0xBB == buf[1] && 0xBF == buf[2]) {
                                DetectedCharset = "UTF-8";
                                _winCodePage = 65001;
                            }
                            break;
                        case 0xFE:
                            if (0xFF == buf[1] && 0x00 == buf[2] && 0x00 == buf[3]) {
                                // FE FF 00 00  UCS-4, unusual octet order BOM (3412)
                                DetectedCharset = "X-ISO-10646-UCS-4-3412";
                            }
                            else if (0xFF == buf[1]) {
                                DetectedCharset = "UTF-16BE";
                                _winCodePage = 1201;
                            }
                            break;
                        case 0x00:
                            if (0x00 == buf[1] && 0xFE == buf[2] && 0xFF == buf[3]) {
                                DetectedCharset = "UTF-32BE";
                                _winCodePage = 12001;
                            }
                            else if (0x00 == buf[1] && 0xFF == buf[2] && 0xFE == buf[3]) {
                                // 00 00 FF FE  UCS-4, unusual octet order BOM (2143)
                                DetectedCharset = "X-ISO-10646-UCS-4-2143";
                            }
                            break;
                        case 0xFF:
                            if (0xFE == buf[1] && 0x00 == buf[2] && 0x00 == buf[3]) {
                                DetectedCharset = "UTF-32LE";
                                _winCodePage = 12000;
                            }
                            else if (0xFE == buf[1]) {
                                DetectedCharset = "UTF-16LE";
                                _winCodePage = 1200;
                            }
                            break;
                    } // switch
                }
                if (DetectedCharset != null) {
                    IsDone = true;
                    return;
                }
            }

            for (int i = 0; i < len; i++) {
                // other than 0xa0, if every other character is ascii, the page is ascii
                if ((buf[i] & 0x80) != 0 && buf[i] != 0xA0) {
                    // we got a non-ascii byte (high-byte)
                    if (_inputState != InputState.Highbyte) {
                        _inputState = InputState.Highbyte;

                        // kill EscCharsetProber if it is active
                        _escCharsetProber = null;

                        // start multibyte and singlebyte charset prober
                        if (_charsetProbers[0] == null) {
                            _charsetProbers[0] = new MBCSGroupProber();
                        }
                        if (_charsetProbers[1] == null) {
                            _charsetProbers[1] = new SBCSGroupProber();
                        }
                        if (_charsetProbers[2] == null) {
                            _charsetProbers[2] = new Latin1Prober();
                        }
                    }
                }
                else {
                    if (_inputState == InputState.PureASCII && (buf[i] == 0x1B || (buf[i] == 0x7B && _lastChar == 0x7E))) {
                        // found escape character or HZ "~{"
                        _inputState = InputState.EscASCII;
                    }
                    _lastChar = buf[i];
                }
            }

            ProbingState st;
            switch (_inputState) {
                case InputState.EscASCII:
                    if (_escCharsetProber == null) {
                        _escCharsetProber = new EscCharsetProber();
                    }

                    st = _escCharsetProber.HandleData(buf, offset, len);
                    if (st == ProbingState.FoundIt) {
                        IsDone = true;
                        DetectedCharset = _escCharsetProber.CharsetName;
                        _winCodePage = _escCharsetProber.WindowsCodePage;
                    }
                    break;
                case InputState.Highbyte:
                    for (int i = 0; i < PROBERS_NUM; i++) {
                        if (_charsetProbers[i] != null) {
                            st = _charsetProbers[i].HandleData(buf, offset, len);
//#if DEBUG
//                            _charsetProbers[i].DumpStatus();
//#endif
                            if (st == ProbingState.FoundIt) {
                                IsDone = true;
                                DetectedCharset = _charsetProbers[i].CharsetName;
                                _winCodePage = _charsetProbers[i].WindowsCodePage;
                                return;
                            }
                        }
                    }
                    break;
                    // pure ascii
            }
        }

        /// <summary>Feed a bytes stream to the detector. </summary>
        /// <param name="stream">an input stream</param>
        public void Feed(Stream stream) {
            byte[] buff = new byte[1024];
            int read;
            while ((read = stream.Read(buff, 0, buff.Length)) > 0 && !IsDone) {
                Feed(buff, 0, read);
            }
        }

        /// <summary>Tell the detector that there is no more data and it must take its decision.</summary>
        public virtual void DataEnd() {
            if (!_gotData) {
                // we haven't got any data yet, return immediately 
                // caller program sometimes call DataEnd before anything has 
                // been sent to detector
                return;
            }

            if (DetectedCharset != null) {
                IsDone = true;
                Confidence = 1.0f;
                return;
            }

            if (_inputState == InputState.Highbyte) {
                float maxProberConfidence = 0.0f;
                int maxProber = 0;
                for (int i = 0; i < PROBERS_NUM; i++) {
                    if (_charsetProbers[i] != null) {
                        float proberConfidence = _charsetProbers[i].GetConfidence();
                        if (proberConfidence > maxProberConfidence) {
                            maxProberConfidence = proberConfidence;
                            maxProber = i;
                        }
                    }
                }

                if (maxProberConfidence > MINIMUM_THRESHOLD) {
                    DetectedCharset = _charsetProbers[maxProber].CharsetName;
                    _winCodePage = _charsetProbers[maxProber].WindowsCodePage;

                    Confidence = maxProberConfidence;
                }
            }
            else if (_inputState == InputState.PureASCII) {
                DetectedCharset = "ASCII";
                _winCodePage = 20127;

                Confidence = 1.0f;
            }
        }

        /// <summary>Clear internal state of charset detector. In the original interface this method is protected.</summary>
        public virtual void Reset() {
            IsDone = false;
            DetectedCharset = null;
            Confidence = 0.0f;

            _winCodePage = -1;
            _start = true;
            _gotData = false;

            //_bestGuess = -1;
            _inputState = InputState.PureASCII;
            _lastChar = 0x00;

            if (_escCharsetProber != null) {
                _escCharsetProber.Reset();
            }

            for (int i = 0; i < PROBERS_NUM; i++) {
                if (_charsetProbers[i] != null) {
                    _charsetProbers[i].Reset();
                }
            }
        }
    }

}