/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IDropZoneDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IDropZone";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("IsDroppable", "bool", ""),
            new ApiFieldInfo("DropZoneName", "string", ""),
        };
    }
}
