/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IBackgroundDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IBackground";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BackgroundColour", "Color?", ""),
        };
    }
}
