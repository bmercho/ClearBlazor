/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record UnderlineDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Underline";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Default", "Underline", ""),
            new ApiFieldInfo("Always", "Underline", ""),
            new ApiFieldInfo("None", "Underline", ""),
        };
    }
}
