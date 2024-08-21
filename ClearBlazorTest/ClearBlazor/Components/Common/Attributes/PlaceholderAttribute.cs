using System;

namespace ClearBlazor
{
    // Attribute to indicate that the property is to be used as a group header
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class PlaceholderAttribute : Attribute
    {
        public PlaceholderAttribute(string placeholder, bool alwaysShowPlaceholder)
        {
            Placeholder = placeholder;
            AlwaysShowPlaceholder = alwaysShowPlaceholder;
        }

        public string Placeholder { get; }
        public bool AlwaysShowPlaceholder { get; }
    }
}

