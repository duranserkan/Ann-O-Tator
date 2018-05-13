using System;
using System.Collections.Generic;
using System.Text;
using Annotator.Common.ExtensionMethods;

namespace Annotator.Common
{
    public abstract class VerifiableBase
    {
        public abstract void Verify();

        protected void IsNullOrEmpty(string paramName)
        {
            if (string.IsNullOrWhiteSpace(this.GetPropValue<string>(paramName)))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        protected void IsURL(string paramName)
        {
            IsNullOrEmpty(paramName);
            var propValue = this.GetPropValue<string>(paramName);
            new Uri(propValue, UriKind.Absolute);
        }
    }
}
