
namespace ClearBlazor
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class DateFormatAttribute : Attribute
    {
        public DateFormatAttribute(string dateFormat)
        {
            DateFormat = dateFormat;
        }

        public string DateFormat { get; }
    }
}
