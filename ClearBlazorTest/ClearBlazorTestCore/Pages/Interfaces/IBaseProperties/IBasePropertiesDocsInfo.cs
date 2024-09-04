/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IBasePropertiesDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IBaseProperties";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Width", "double", ""),
            new ApiFieldInfo("Height", "double", ""),
            new ApiFieldInfo("MinWidth", "double", ""),
            new ApiFieldInfo("MinHeight", "double", ""),
            new ApiFieldInfo("MaxWidth", "double", ""),
            new ApiFieldInfo("MaxHeight", "double", ""),
            new ApiFieldInfo("Margin", "string", ""),
            new ApiFieldInfo("Padding", "string", ""),
            new ApiFieldInfo("HorizontalAlignment", "Alignment?", ""),
            new ApiFieldInfo("VerticalAlignment", "Alignment?", ""),
            new ApiFieldInfo("Row", "int", ""),
            new ApiFieldInfo("Column", "int", ""),
            new ApiFieldInfo("RowSpan", "int", ""),
            new ApiFieldInfo("ColumnSpan", "int", ""),
        };
    }
}
