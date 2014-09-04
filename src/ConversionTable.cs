using System;

namespace SerialPersistanceLibrary
{
    internal static class ConversionTable
    {
        internal static dynamic ConvertFromSerial(Type _Type, dynamic Var)
        {
            switch(_Type.ToString())
            {
                case "System.Windows.Media.SolidColorBrush":
                    return new System.Windows.Media.BrushConverter().ConvertFrom(Var);

                case "System.Windows.Thickness":
                    return new System.Windows.Thickness(Var[0], Var[1], Var[2], Var[3]);

                default:
                    throw new InvalidCastException("No Cast Method Exsists For Type");
            }
        }

        internal static dynamic ConvertToSerial(Type _Type, dynamic Var)
        {
            switch (_Type.ToString())
            {
                case "System.Windows.Media.SolidColorBrush":
                    return Var.Color.ToString();
            
                case "System.Windows.Thickness":
                    return new double[4] { Var.Left, Var.Top, Var.Right, Var.Bottom };

                default:
                    throw new InvalidCastException("No Cast Method Exsists For Type");
            }
        }
    }
}
