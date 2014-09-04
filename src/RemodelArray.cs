using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPersistanceLibrary
{
    internal static class RemodelArray
    {
        internal static void Remodel<T>(ref T[] Array, T Value, RemodelArrayMode ResizeMode)
        {
            if (Array == null)
            {
                throw new ArgumentNullException("Input Array Cannot Be Null");
            }

            lock(Array)
            {
                if (ResizeMode.Equals(RemodelArrayMode.Add))
                {
                    long arrayLength = Array.Length;
                    T[]  tempArray   = new T[arrayLength + 1];

                    System.Array.Copy(Array, tempArray, arrayLength);

                    tempArray[arrayLength] = Value;
                    Array                  = tempArray;
                    tempArray              = null;
                }

                if (ResizeMode.Equals(RemodelArrayMode.Remove))
                {
                    long arrayLength = Array.Length;
                    long offset      = 0;
                    T[]  tempArray   = new T[arrayLength - 1];

                    for (int i = 0; i < arrayLength; i++)
                    {
                        if (Array[i].Equals(Value))
                        {
                            Array[i] = default(T);
                            offset   = (offset - 1);
                            continue;
                        }

                        tempArray[i + offset] = Array[i];
                    }

                    Array     = tempArray;
                    tempArray = null;
                }
            }
        }
    }
}
