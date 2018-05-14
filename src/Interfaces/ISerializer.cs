using SerialPersistanceLibrary.Enums;
using System;

namespace SerialPersistanceLibrary.Interfaces
{
    /// <summary>
    /// An interface defining the required properties of a serialization handler.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Retreives a serialized object from disk.
        /// </summary>
        /// <param name="PathToFile">Path To The File Containing The Serialized Object.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <param name="MaxWaitForDiskAccess">How Long To Wait If An Error Occors When Attempting To Access The File.</param>
        /// <returns>The Deserilized Object.</returns>
        dynamic Deserialize( String PathToFile, IOSerializationFormatter IOFormatterEnum, UInt16 MaxWaitForDiskAccess );

        /// <summary>
        /// Retrieves a serialized object from disk.
        /// </summary>
        /// <param name="PathToFile">Path To The File Containing The Serialized Object.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <returns>The Deserilized Object.</returns>
        dynamic Deserialize( String PathToFile, IOSerializationFormatter IOFormatterEnum);

        /// <summary>
        /// Serializes an object to dsik.
        /// </summary>
        /// <param name="ObjectToSerialize">Object To Be Serialized And Stored To Disk.</param>
        /// <param name="PathToFile">Path Indicating Where To Store The Serilized Object Data.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        /// <param name="MaxWaitForDiskAccess">How Long To Wait If An Error Occors When Attempting To Access/Create The File.</param>
        void Serialize( Object ObjectToSerialize, String PathToFile, IOSerializationFormatter IOFormatterEnum, UInt16 MaxWaitForDiskAccess );

        /// <summary>
        /// Serializes an object to dsik.
        /// </summary>
        /// <param name="ObjectToSerialize">Object To Be Serialized And Stored To Disk.</param>
        /// <param name="PathToFile">Path Indicating Where To Store The Serilized Object Data.</param>
        /// <param name="IOFormatterEnum">Base Serialization Class.</param>
        void Serialize( Object ObjectToSerialize, String PathToFile, IOSerializationFormatter IOFormatterEnum);
    }
}
