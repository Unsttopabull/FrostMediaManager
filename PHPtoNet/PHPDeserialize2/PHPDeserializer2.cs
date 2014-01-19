namespace Frost.PHPtoNET {

    public class PHPDeserializer2 {
        public object Deserialize(PHPSerializedStream s) {
            switch (s.Peek()) {
                case 's':
                    return s.ReadString();
                case 'N':
                    return s.ReadNull();
                case 'i':
                    return s.ReadInteger();
                case 'd':
                    return s.ReadDouble();
                case 'b':
                    return s.ReadBoolean();
                case 'a':
                    return s.ReadArray();
                case 'O':
                    return s.ReadObject();
                default:
                    throw new ParsingException("Uknown type or malformed data detected.");
            }
        }
    }

}