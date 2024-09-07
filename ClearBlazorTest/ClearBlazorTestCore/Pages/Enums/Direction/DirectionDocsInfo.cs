/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DirectionDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Direction";
        public string Description {get; set; } = "Used by WrapPanel to indicates the direction of wrapping\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Row", "Direction", "The items are laid out in rows from left to right and wraps to a new row.  \r"),
            new ApiFieldInfo("RowReverse", "Direction", "The items are laid out in rows from right to left and wraps to a new row.  \r"),
            new ApiFieldInfo("Column", "Direction", "The items are laid out in columns from top to bottom and wraps to a new column.  \r"),
            new ApiFieldInfo("ColumnReverse", "Direction", "The items are laid out in columns from bottom to top and wraps to a new column.  \r"),
        };
    }
}
