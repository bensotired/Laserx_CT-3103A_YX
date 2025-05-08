using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Motion.Base
{
    internal class MtrPosSpeedConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            object result;
            if (destType == typeof(string) && value is MtrPosSpeed)
            {
                MtrPosSpeed mtrPosSpeed = (MtrPosSpeed)value;
                result = string.Concat(new object[]
                {
                    mtrPosSpeed.Pos,
                    ", ",
                    mtrPosSpeed.StartVel,
                    ", ",
                    mtrPosSpeed.MaxVel
                });
            }
            else
            {
                result = base.ConvertTo(context, culture, value, destType);
            }
            return result;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string text = (string)value;
                string[] array = text.Split(new char[]
                {
                    ','
                });
                try
                {
                    double pos;
                    double.TryParse(array[0], out pos);
                    int startVel;
                    int.TryParse(array[1], out startVel);
                    int maxVel;
                    int.TryParse(array[2], out maxVel);
                    return new MtrPosSpeed
                    {
                        Pos = pos,
                        StartVel = startVel,
                        MaxVel = maxVel
                    };
                }
                catch
                {
                    throw new InvalidCastException("Cannot convert the string '" + value.ToString() + "' into a MtrPosSpeed");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(value);
        }
    }
}
