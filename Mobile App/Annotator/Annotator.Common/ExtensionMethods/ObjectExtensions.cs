using System;
using System.Collections.Generic;
using System.Text;

namespace Annotator.Common.ExtensionMethods
{
    public static class ObjectExtensions
    {
        public static T GetPropValue<T>(this object @object, string propName)
        {
            var propertyInfo = @object.GetType().GetProperty(propName);
            if (propertyInfo == null)
            {
                return default;
            }

            return (T) propertyInfo.GetValue(@object, null);
        }
    }
}
