/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DirectionDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Direction";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Row", "Direction", ""),
            new ApiFieldInfo("RowReverse", "Direction", ""),
            new ApiFieldInfo("Column", "Direction", ""),
            new ApiFieldInfo("ColumnReverse", "Direction", ""),
        };
    }
}
