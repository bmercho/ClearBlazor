/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IBorderDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IBorder";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BorderThickness", "string?", ""),
            new ApiFieldInfo("BorderColour", "Color?", ""),
            new ApiFieldInfo("BorderStyle", "BorderStyle?", ""),
            new ApiFieldInfo("CornerRadius", "string?", ""),
        };
    }
}
