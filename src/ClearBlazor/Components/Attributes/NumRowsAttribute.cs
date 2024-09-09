using System;

namespace ClearBlazor
{
    // Attribute to indicate that the property is to be used as a group header
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class NumRowsAttribute : Attribute
    {
        public NumRowsAttribute(int numRows)
        {
            NumRows = numRows;
        }

        public int NumRows { get; }
    }
}
