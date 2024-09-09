/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SizeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Size";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("VerySmall", "Size", ""),
            new ApiFieldInfo("Small", "Size", ""),
            new ApiFieldInfo("Normal", "Size", ""),
            new ApiFieldInfo("Large", "Size", ""),
            new ApiFieldInfo("VeryLarge", "Size", ""),
        };
    }
}
