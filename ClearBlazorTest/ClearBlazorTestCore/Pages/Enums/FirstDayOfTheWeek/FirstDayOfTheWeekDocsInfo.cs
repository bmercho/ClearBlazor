/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record FirstDayOfTheWeekDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "FirstDayOfTheWeek";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Monday", "FirstDayOfTheWeek", ""),
            new ApiFieldInfo("Sunday", "FirstDayOfTheWeek", ""),
        };
    }
}
