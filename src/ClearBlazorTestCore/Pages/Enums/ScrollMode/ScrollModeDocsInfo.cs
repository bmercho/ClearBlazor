/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ScrollModeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "ScrollMode";
        public string Description {get; set; } = "The scroll bar mode\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Disabled", "ScrollMode", "No scroll bar is shown\r"),
            new ApiFieldInfo("Enabled", "ScrollMode", "Scroll bar will always be shown.\r"),
            new ApiFieldInfo("Auto", "ScrollMode", "Scroll bar will be shown only when container has overflowed.\r"),
        };
    }
}
