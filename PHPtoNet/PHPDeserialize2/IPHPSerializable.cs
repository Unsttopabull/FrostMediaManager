namespace Frost.PHPtoNET.PHPDeserialize2 {
    interface IPHPSerializable {
        object FromPHPSerializedString(string serializedData);
    }
}
