using SerialPersistanceLibrary.Enums;
using System;

namespace SerialPersistanceLibrary.Interfaces
{
    /// <summary>
    /// An interface defining the required properties of an encrypting serialization handler.
    /// </summary>
    public interface IEncryptedSerializer : IDisposable
    {
        /// <summary>
        /// Retreives an encrypted serialized object from disk.
        /// </summary>
        /// <param name="PathToFile">Path To The File Containing The Serialized Object.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <param name="Key">Byte Array To Use As Encryption Key.</param>
        /// <param name="MaxWaitForDiskAccess">How Long To Wait If An Error Occors When Attempting To Access The File.</param>
        /// <param name="DiskAccessAttemptInterval">How Long To Wait Before Re-Attempting Disk Access If An Error Occurs When Attempting To Access The File.</param>
        /// <returns>The Deserilized Object.</returns>
        dynamic Deserialize( String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key, TimeSpan MaxWaitForDiskAccess, TimeSpan DiskAccessAttemptInterval );

        /// <summary>
        /// Retreives an encrypted serialized object from disk.
        /// </summary>
        /// <param name="PathToFile">Path To The File Containing The Serialized Object.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <param name="Key">Byte Array To Use As Encryption Key.</param>
        /// <returns>The Deserilized Object.</returns>
        dynamic Deserialize( String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key );

        /// <summary>
        /// Encrypts and serializes an object to dsik.
        /// </summary>
        /// <param name="ObjectToSerialize">Object To Be Serialized And Stored To Disk.</param>
        /// <param name="PathToFile">Path Indicating Where To Store The Serilized Object Data.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <param name="Key">Byte Array To Use As Decryption Key.</param>
        /// <param name="MaxWaitForDiskAccess">How Long To Wait If An Error Occors When Attempting To Access/Create The File.</param>
        /// <param name="DiskAccessAttemptInterval">How Long To Wait Before Re-Attempting Disk Access If An Error Occurs When Attempting To Access/Create The File.</param>
        void Serialize( Object ObjectToSerialize, String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key, TimeSpan MaxWaitForDiskAccess, TimeSpan DiskAccessAttemptInterval );

        /// <summary>
        /// Encrypts and serializes an object to dsik.
        /// </summary>
        /// <param name="ObjectToSerialize">Object To Be Serialized And Stored To Disk.</param>
        /// <param name="PathToFile">Path Indicating Where To Store The Serilized Object Data.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <param name="Key">Byte Array To Use As Decryption Key.</param>
        void Serialize( Object ObjectToSerialize, String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key );
    }
}
