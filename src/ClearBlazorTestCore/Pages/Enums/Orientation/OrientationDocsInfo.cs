/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record OrientationDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Orientation";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Portrait", "Orientation", ""),
            new ApiFieldInfo("Landscape", "Orientation", ""),
        };
    }
}
