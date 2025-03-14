/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IconLocationDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IconLocation";
        public string Description {get; set; } = "Indicates the location of the Icon within a button.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Start", "IconLocation", "Icon located at the start of button\r"),
            new ApiFieldInfo("End", "IconLocation", "Icon located at the end of button\r"),
        };
    }
}
