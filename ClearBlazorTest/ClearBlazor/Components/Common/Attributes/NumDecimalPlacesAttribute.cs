using System;

namespace ClearBlazor
{
    // Attribute to indicate that the property is to be used as a group header
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class NumDecimalPlacesAttribute : Attribute
    {
        public NumDecimalPlacesAttribute(int numDecimalPlaces)
        {
            NumDecimalPlaces = numDecimalPlaces;
        }

        public int NumDecimalPlaces { get; }
    }
}
