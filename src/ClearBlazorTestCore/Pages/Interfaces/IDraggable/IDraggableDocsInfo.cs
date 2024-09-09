/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IDraggableDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IDraggable";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("IsDraggable", "bool", ""),
        };
    }
}
