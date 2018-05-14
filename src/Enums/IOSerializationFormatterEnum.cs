namespace SerialPersistanceLibrary.Enums
{
    /// <summary>
    /// An enum used to select the base serialization class to be used when calling
    /// an instance of ISerializer/IEncryptedSerializer Deserlialize/Serilize methods.
    /// </summary>
    public enum IOSerializationFormatter
    {
        BinaryFormatter    = 0,
        InternalSerializer = 1,
        XmlSerializer      = 2
    }
}
