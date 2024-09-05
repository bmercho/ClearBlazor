/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TextTrimmingDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "TextTrimming";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("None", "TextTrimming", ""),
            new ApiFieldInfo("Ellipsis", "TextTrimming", "Due to CSS limitations this only works if TextWrapping/>\ris set to TextWrapping.NoWrap/>.\r"),
        };
    }
}
