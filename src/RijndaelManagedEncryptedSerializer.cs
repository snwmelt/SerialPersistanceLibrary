using SerialPersistanceLibrary.Enums;
using SerialPersistanceLibrary.Extensions;
using SerialPersistanceLibrary.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;

namespace SerialPersistanceLibrary
{
    public sealed class RijndaelManagedEncryptedSerializer : IEncryptedSerializer
    {
        #region Private Variables

        private TimeSpan _AccessAttemptInterval = TimeSpan.FromMilliseconds( 500 );
        private TimeSpan _MaxWaitForDiskAccess = TimeSpan.FromMilliseconds( 2000 );
        private Byte[]   _InitializationVector;

        #endregion


        private CryptoStream _AttemptOpenCryptoStream( String PathToFile, Byte[] Key, CryptoStreamMode WriteMode )
        {
            try
            {
                return _OpenCryptoStream( PathToFile, Key, WriteMode );
            }
            catch ( IOException IOE0 )
            {
                IOException[] IOExceptions = new IOException[] { IOE0 };
                DateTime LastIOExceptionTime = DateTime.Now;
                DateTime GiveUpNow = LastIOExceptionTime + _MaxWaitForDiskAccess;

                for ( ; ; )
                {
                    if ( DateTime.Now.Second >= GiveUpNow.Second )
                    {
                        throw new AggregateException( IOExceptions );
                    }

                    if ( DateTime.Now.Millisecond >= ( LastIOExceptionTime + _AccessAttemptInterval ).Millisecond )
                    {
                        try
                        {
                            return _OpenCryptoStream( PathToFile, Key, WriteMode );
                        }
                        catch ( IOException IOEN )
                        {
                            IOExceptions.Refactor( IOEN, ArrayRefactorAction.Add );

                            LastIOExceptionTime += _AccessAttemptInterval;
                        }
                    }

                    Thread.Sleep( _AccessAttemptInterval - TimeSpan.FromMilliseconds( 1 ) );
                }
            }
        }

        public dynamic Deserialize( String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key, TimeSpan MaxWaitForDiskAccess, TimeSpan DiskAccessAttemptInterval )
        {
            _MaxWaitForDiskAccess  = MaxWaitForDiskAccess;
            _AccessAttemptInterval = DiskAccessAttemptInterval;

            return Deserialize( PathToFile, IOFormatterEnum, Key );
        }

        public dynamic Deserialize( String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key )
        {
            using ( CryptoStream _CryptoStream = _AttemptOpenCryptoStream( PathToFile, Key, CryptoStreamMode.Read ) )
            {
                switch ( IOFormatterEnum )
                {
                    case IOSerializationFormatter.BinaryFormatter:
                        return ( new BinaryFormatter( ) ).Deserialize( _CryptoStream );

                    default:
                        throw new NotImplementedException( "IOSerializationFormatter : " + IOFormatterEnum.ToString( ) + " Unsupported." );
                }
            }
        }

        public void Dispose( )
        {
            _ZeroInitializationVector();
        }
        
        private CryptoStream _OpenCryptoStream( String PathToFile, Byte[] Key, CryptoStreamMode WriteMode )
        {
            RijndaelManaged _RijndaelManaged = new RijndaelManaged( );

            switch ( WriteMode )
            {
                case CryptoStreamMode.Read:
                    BinaryReader _BinaryReader = new BinaryReader( new FileStream( PathToFile, FileMode.Open ) );

                    _ZeroInitializationVector( );

                    _InitializationVector = new Byte[ 16 ];

                    _BinaryReader.Read( _InitializationVector, 0, 16 );

                    return new CryptoStream( _BinaryReader.BaseStream, _RijndaelManaged.CreateDecryptor( Key, _InitializationVector ), WriteMode );
                    
                    
                case CryptoStreamMode.Write:
                    _ZeroInitializationVector( );

                    _InitializationVector = new Byte[ 16 ];

                    Array.Copy( _RijndaelManaged.IV, _InitializationVector, 16 );

                    BinaryWriter _BinaryWriter = new BinaryWriter( new FileStream( PathToFile, FileMode.OpenOrCreate ) );
                    
                    _BinaryWriter.Write( _InitializationVector, 0, 16 );

                    return new CryptoStream( _BinaryWriter.BaseStream, _RijndaelManaged.CreateEncryptor( Key, _InitializationVector ), WriteMode );
                    
                    
                default:
                    throw new NotImplementedException( "CryptoStreamMode : " + WriteMode.ToString( ) + " Unsupported." );
            }
        }

        public void Serialize( Object ObjectToSerialize, String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key, TimeSpan MaxWaitForDiskAccess, TimeSpan DiskAccessAttemptInterval )
        {
            _MaxWaitForDiskAccess  = MaxWaitForDiskAccess;
            _AccessAttemptInterval = DiskAccessAttemptInterval;

            Serialize( ObjectToSerialize, PathToFile, IOFormatterEnum, Key );
        }

        public void Serialize( Object ObjectToSerialize, String PathToFile, IOSerializationFormatter IOFormatterEnum, Byte[] Key )
        {
            if ( ObjectToSerialize is null )
            {
                throw new ArgumentNullException( "Object to be Serialized Cannot be of Type Null." ); 
            }
            
            using ( CryptoStream CryptoStream = _AttemptOpenCryptoStream( PathToFile, Key, CryptoStreamMode.Write ) )
            {
                switch ( IOFormatterEnum )
                {
                    case IOSerializationFormatter.BinaryFormatter:
                        ( new BinaryFormatter( ) ).Serialize( CryptoStream, ObjectToSerialize );
                        break;

                    default:
                        throw new NotImplementedException( "IOSerializationFormatter : " + IOFormatterEnum.ToString( ) + " Unsupported." );
                }
            }
        }

        private void _ZeroInitializationVector( )
        {
            if ( !( _InitializationVector is null ) )
            {
                Array.Clear( _InitializationVector, 0, 16 );
                _InitializationVector = null;
            }
        }
    }
}