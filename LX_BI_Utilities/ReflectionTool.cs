 
using System.Collections.Generic;
using System.Linq;

namespace LX_BurnInSolution.Utilities
{
    public static class ReflectionTool
    {
        public static  bool ComparePropertyValues(object sourceObject, Dictionary<string, object> dict)
        {
            bool inequality = false;
            var msProps = sourceObject.GetType().GetProperties().ToList();

            foreach (var kvp in dict)
            {
                var msProp = msProps.Find(p => p.Name == kvp.Key);
 
                var sourceVals = Converter.ConvertObjectTo(msProp.GetValue(sourceObject), msProp.PropertyType);
                var editVals = Converter.ConvertObjectTo(kvp.Value, msProp.PropertyType);

                if (sourceVals == null &&
                    editVals == null)
                {

                }
                else
                {
                    if ((sourceVals == null && editVals != null) ||
                        (sourceVals != null && editVals == null))
                    {
                        inequality = true;
                    }
                    else
                    {
                        if (sourceVals.Equals(editVals))
                        {

                        }
                        else
                        {
                            inequality = true;
                        }
                    }
                }
            }
            return inequality;
        }
        public static void SetPropertyValues(object sourceObject, Dictionary<string, object> dict)
        {
            var msProps = sourceObject.GetType().GetProperties().ToList();

            foreach (var kvp in dict)
            {
                var msProp = msProps.Find(p => p.Name == kvp.Key);

                var editVals = Converter.ConvertObjectTo(kvp.Value, msProp.PropertyType);
                msProp.SetValue(sourceObject, editVals);
            }
        }
    }
}