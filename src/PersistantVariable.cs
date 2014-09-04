using System;
using System.Runtime.Serialization;

namespace SerialPersistanceLibrary
{
    [Serializable()]
    internal struct PersistantVariable : ISerializable
    {
        internal readonly String ID;
        public   readonly Type   T;
        private  readonly Object value;

        private PersistantVariable(SerializationInfo info, StreamingContext context)
        {
            ID    = info.GetString("ID");
            T     = (Type)info.GetValue("T", typeof(Type));
            value = info.GetValue("value", typeof(Object));
        }
        
        internal PersistantVariable(String ID, Type T, Object Value)
        {
            this.ID = ID;
            this.T  = T;

            if (T.IsSerializable)
            {
                value = Value;
            }
            else
            {
                value = ConversionTable.ConvertToSerial(T, Value);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            return this.ID.Equals(((PersistantVariable)obj).ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID",    ID);
            info.AddValue("T",     T);
            info.AddValue("value", value);
        }

        internal dynamic Value
        {
            get
            {
                if (T.IsSerializable)
                {
                    return value;
                }
                else
                {
                    return ConversionTable.ConvertFromSerial(T, value);
                }
            }
        }
    }
}
