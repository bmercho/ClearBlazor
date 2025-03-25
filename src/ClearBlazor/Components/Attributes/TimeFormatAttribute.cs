
namespace ClearBlazor
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class TimeFormatAttribute : Attribute
    {
        public TimeFormatAttribute(string timeFormat)
        {
            TimeFormat = timeFormat;
        }

        public string TimeFormat { get; }
    }
}
