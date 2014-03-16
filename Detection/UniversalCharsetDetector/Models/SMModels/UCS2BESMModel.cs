namespace Frost.SharpCharsetDetector.Models.SMModels {

    public class UCS2BeSMModel : SMModel {
        private static readonly int[] UCS_2BECls = {
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 00 - 07 
            BitPackage.Pack4bits(0, 0, 1, 0, 0, 2, 0, 0), // 08 - 0f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 10 - 17 
            BitPackage.Pack4bits(0, 0, 0, 3, 0, 0, 0, 0), // 18 - 1f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 20 - 27 
            BitPackage.Pack4bits(0, 3, 3, 3, 3, 3, 0, 0), // 28 - 2f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 30 - 37 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 38 - 3f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 40 - 47 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 48 - 4f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 50 - 57 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 58 - 5f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 60 - 67 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 68 - 6f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 70 - 77 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 78 - 7f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 80 - 87 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 88 - 8f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 90 - 97 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 98 - 9f 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // a0 - a7 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // a8 - af 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // b0 - b7 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // b8 - bf 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // c0 - c7 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // c8 - cf 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // d0 - d7 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // d8 - df 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // e0 - e7 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // e8 - ef 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // f0 - f7 
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 4, 5) // f8 - ff 
        };

        private static readonly int[] UCS_2BESt = {
            BitPackage.Pack4bits(5, 7, 7, ERROR, 4, 3, ERROR, ERROR), //00-07 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR, ERROR, ITSME, ITSME, ITSME, ITSME), //08-0f 
            BitPackage.Pack4bits(ITSME, ITSME, 6, 6, 6, 6, ERROR, ERROR), //10-17 
            BitPackage.Pack4bits(6, 6, 6, 6, 6, ITSME, 6, 6), //18-1f 
            BitPackage.Pack4bits(6, 6, 6, 6, 5, 7, 7, ERROR), //20-27 
            BitPackage.Pack4bits(5, 8, 6, 6, ERROR, 6, 6, 6), //28-2f 
            BitPackage.Pack4bits(6, 6, 6, 6, ERROR, ERROR, START, START) //30-37 
        };

        private static readonly int[] UCS_2BECharLenTable = {2, 2, 2, 0, 2, 2};

        public UCS2BeSMModel() : base(
            new BitPackage(IndexShift.Shift4BITS, ShiftMask.Mask4BITS, BitShift.Shift4BITS, UnitMask.Mask4BITS, UCS_2BECls),
            6,
            new BitPackage(IndexShift.Shift4BITS, ShiftMask.Mask4BITS, BitShift.Shift4BITS, UnitMask.Mask4BITS, UCS_2BESt),
            UCS_2BECharLenTable,
            "UTF-16BE",
            1201) {
        }
    }

}