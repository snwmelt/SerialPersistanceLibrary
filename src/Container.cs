using System;
using System.IO;
using System.Runtime.Serialization;

namespace SerialPersistanceLibrary
{
    [Serializable()]
    public class Container : ISerializable
    {
        private String               fileName;
        private PersistantVariable[] pvs;

        private Container(SerializationInfo info, StreamingContext context)
        {
            pvs = (PersistantVariable[])info.GetValue("persistantVariables", typeof(PersistantVariable[]));
        }

        public Container(Object Obj) : this(((Obj.ToString()).GetHashCode()).ToString()) { }

        public Container(String FileName) : this()
        {
            fileName = FileName;
        }

        public Container()
        {
            pvs = new PersistantVariable[0];
        }

        public void Add<T>(String ID, T Variable)
        {
            RemodelArray.Remodel(ref pvs, (new PersistantVariable(ID, typeof(T), Variable)), RemodelArrayMode.Add);
        }

        private static string baseDirectory;

        public static String BaseDirectory
        {
            set
            {
                baseDirectory = value;
            }

            get
            {
                if (String.IsNullOrEmpty(baseDirectory))
                    return defaultBaseDirectory;

                return baseDirectory;
            }
        }

        private static readonly String defaultBaseDirectory = @"../SerialPersistanceDirectory/";
        
        public static bool Exists(String FileName)
        {
            return File.Exists(FileName);
        }
        
        public static bool Exists(Object Obj)
        {
            return Exists(BaseDirectory + getHashString(Obj));
        }

        private static String getHashString(Object obj)
        {
            return ((obj.ToString()).GetHashCode()).ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("persistantVariables", pvs);
        }

        public static Container Load(Object Obj, int MaxWaitForDiskAccess)
        {
            return Load(getHashString(Obj), MaxWaitForDiskAccess);
        }

        public static Container Load(String FileName, int MaxWaitForDiskAccess)
        {
            if (!File.Exists(FileName))
            {
                if (!File.Exists(BaseDirectory + FileName))
                {
                    throw new FileNotFoundException(FileName);
                }

                FileName = (BaseDirectory + FileName);
            }

            Container container = Serializer.Deserialize(BaseSerializer.BinaryFormatter, MaxWaitForDiskAccess, null, FileName);
            container.fileName  = FileName;

            return container;
        }

        public void Remove(String ID)
        {
            RemodelArray.Remodel(ref pvs, (new PersistantVariable(ID, null, null)), RemodelArrayMode.Remove);
        }

        private long index(String iD)
        {
            for (int i = 0; i < pvs.Length; i++)
            {
                if (pvs[i].ID.Equals(iD))
                {
                    return i;
                }
            }

            return -1;
        }

        public dynamic Return(String ID)
        {
            long index = this.index(ID);

            if (!index.Equals(-1))
                return pvs[index].Value;

            return null;
        }

        public void Save(String FileName, int MaxWaitForDiskAccess)
        {
            if (String.IsNullOrEmpty(FileName))
            {
                throw new ArgumentNullException("FileName Cannot Be null or Empty");
            }

            lock (this)
            {
                Serializer.Serialize(BaseSerializer.BinaryFormatter, FileName, MaxWaitForDiskAccess, this);
            }
        }

        public void Save(int MaxWaitForDiskAccess)
        {
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }

            Save((BaseDirectory + fileName), MaxWaitForDiskAccess);
        }

        public void Update<T>(String ID, T Variable)
        {
            long index = this.index(ID);

            if (index.Equals(-1))
            {
                throw new Exception("ID Is Not Contained In This Container : " + ID);
            }

            if (!(pvs[index].T).Equals(typeof(T)))
            {
                throw new InvalidCastException("Input Variable Is Not Of Type : " + pvs[index].T);
            }

            pvs[index] = new PersistantVariable(ID, typeof(T), Variable);
        }
    }
}
