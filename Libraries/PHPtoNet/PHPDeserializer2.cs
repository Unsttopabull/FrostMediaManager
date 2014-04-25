using System.Text;

namespace Frost.PHPtoNET {

    public static class PHPDeserializer2 {

        public static object Deserialize(string serialized, Encoding encoding) {
            using (PHPSerializedStream serializedStream = new PHPSerializedStream(serialized, encoding)) {
                return Deserialize(serializedStream);
            }
        }

        public static object Deserialize(PHPSerializedStream s) {
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

        public static T Deserialize<T>(PHPSerializedStream s) {
            return s.DeserializeElement<T>();
        }

        public static T Deserialize<T>(string serialied, Encoding encoding) {
            using (PHPSerializedStream serializedStream = new PHPSerializedStream(serialied, encoding)) {
                return serializedStream.DeserializeElement<T>();
            }
        }
    }

}