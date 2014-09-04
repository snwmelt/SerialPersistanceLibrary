using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace SerialPersistanceLibrary
{
    public sealed class Serializer
    {
        public static dynamic Deserialize(BaseSerializer BaseSerializer, int MaxWaitForDiskAccess, object _object, string PathToFile)
        {
            if (isSeconds(MaxWaitForDiskAccess))
            {
                using (FileStream FStream = openFileStream(MaxWaitForDiskAccess, FileMode.Open, PathToFile))
                {
                    switch (BaseSerializer)
                    {
                        case BaseSerializer.BinaryFormatter:
                            BinaryFormatter BFormatter = new BinaryFormatter();
                            return BFormatter.Deserialize(FStream);


                        case BaseSerializer.XmlSerializer:
                            if (_object == null)
                            {
                                throw new ArgumentException("Cannot Pass Object Of Type Null To BaseSerializer XmlSerializer");
                            }

                            XmlSerializer XSerializer = new XmlSerializer(_object.GetType());
                            return XSerializer.Deserialize(FStream);
                    }
                }
            }

            return null;
        }

        private static bool isSeconds(int MaxWaitForDiskAccess)
        {
            if (MaxWaitForDiskAccess < 0 || 60 < MaxWaitForDiskAccess)
                throw new ArgumentException("Input Out Of Bounds");

            return true;
        }

        private static FileStream openFileStream(int MaxWaitForDiskAccess, FileMode mode, string pathToFile)
        {
            try
            {
                return new FileStream(pathToFile, mode);
            }
            catch (IOException ioe)
            {
                Debug.WriteLine(ioe);

                DateTime maxWait = (DateTime.Now + TimeSpan.FromSeconds(MaxWaitForDiskAccess));

                while (true)
                {
                    if (DateTime.Now.Second.Equals(maxWait.Second)) break;

                    try
                    {
                        return new FileStream(pathToFile, mode);
                    }
                    catch { }
                }

                throw ioe;
            }
        }

        public static void Serialize(BaseSerializer BaseSerializer, string FileToWrite, int MaxWaitForDiskAccess, object _object)
        {
            if (_object == null)
            {
                throw new ArgumentException("Cannot Parse Object Of Type Null");
            }

            if (isSeconds(MaxWaitForDiskAccess))
            {
                using (FileStream FStream = openFileStream(MaxWaitForDiskAccess, FileMode.OpenOrCreate, FileToWrite))
                {
                    switch (BaseSerializer)
                    {
                        case BaseSerializer.BinaryFormatter:
                            BinaryFormatter BFormatter = new BinaryFormatter();
                            BFormatter.Serialize(FStream, _object);
                            break;

                        case BaseSerializer.XmlSerializer:
                            XmlSerializer XSerializer = new XmlSerializer(_object.GetType());
                            XSerializer.Serialize(Console.Out, _object);
                            break;
                    }
                }
            }
        }
    }
}
