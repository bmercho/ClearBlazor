/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SizeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Size";
        public string Description {get; set; } = "Defines the size of a component.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("VerySmall", "Size", "Very small\r"),
            new ApiFieldInfo("Small", "Size", "Small\r"),
            new ApiFieldInfo("Normal", "Size", "Normal\r"),
            new ApiFieldInfo("Large", "Size", "Large\r"),
            new ApiFieldInfo("VeryLarge", "Size", "Very large\r"),
        };
    }
}
