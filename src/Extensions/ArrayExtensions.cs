using SerialPersistanceLibrary.Enums;
using System;

namespace SerialPersistanceLibrary.Extensions
{
    /// <summary>
    /// Contains extension methods for the System.Array type.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Refactors an System.Array according to the ArrayRefactorAction passed by the user.
        /// </summary>
        /// <typeparam name="T">Array Content Type Specifier.</typeparam>
        /// <param name="This">System.Array Instance To Be Operated On.</param>
        /// <param name="Value">Type To Be Added or Removed.</param>
        /// <param name="Action">Action To Be Taken.</param>
        public static void Refactor<T>( this T[] This, T Value, ArrayRefactorAction Action )
        {
            if ( This == null )
            {
                throw new ArgumentNullException("Input Array Cannot Be Null");
            }


            lock ( This )
            {
                T[] _TempArray;

                switch ( Action )
                {
                    case ArrayRefactorAction.Add:

                        _TempArray = new T[ This.Length + 1 ];

                        This.CopyTo( _TempArray, 0 );

                        _TempArray[ This.Length ]  = Value;

                        break;

                    case ArrayRefactorAction.Remove:

                        long _NewArrayLength = This.Length - 1;
                        long _RemoveAtIndex = 0;
                        _TempArray = new T[ _NewArrayLength ];

                        foreach( T _ArrayValue in This )
                        {
                            if ( _ArrayValue.Equals( Value ) )
                            {
                                break;
                            }

                            _RemoveAtIndex++;
                        }

                        if ( _RemoveAtIndex == 0 )
                        {
                            Array.Copy( This, 1, _TempArray, 0, _NewArrayLength );
                        }
                        else
                        {
                            Array.Copy( This, 0, _TempArray, 0, _RemoveAtIndex );
                            Array.Copy( This, ( _RemoveAtIndex + 1 ), _TempArray, _RemoveAtIndex, ( _NewArrayLength - _RemoveAtIndex ) );
                        }
                        break;

                    default:
                        throw new NotImplementedException( "Handler for Array Refactor Mode : " + Action.ToString() + " is Unimplemented." );
                }

                #pragma warning disable CS0728 // Possibly incorrect assignment to local which is the argument to a using or lock statement
                This       = _TempArray;
                #pragma warning restore CS0728 // Possibly incorrect assignment to local which is the argument to a using or lock statement

                _TempArray = null;
            }

        }
    }
}
