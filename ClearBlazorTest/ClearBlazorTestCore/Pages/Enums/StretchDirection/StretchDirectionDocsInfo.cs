/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record StretchDirectionDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "StretchDirection";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("UpOnly", "StretchDirection", ""),
            new ApiFieldInfo("DownOnly", "StretchDirection", ""),
            new ApiFieldInfo("Both", "StretchDirection", ""),
        };
    }
}
