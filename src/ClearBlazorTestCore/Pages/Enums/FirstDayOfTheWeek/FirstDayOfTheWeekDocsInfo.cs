/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record FirstDayOfTheWeekDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "FirstDayOfTheWeek";
        public string Description {get; set; } = "Used by the DatePicker to indicate what is considered the first day of the week.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Monday", "FirstDayOfTheWeek", "Monday is considered the first day of the week.\r"),
            new ApiFieldInfo("Sunday", "FirstDayOfTheWeek", "Sunday is considered the first day of the week.\r"),
        };
    }
}
