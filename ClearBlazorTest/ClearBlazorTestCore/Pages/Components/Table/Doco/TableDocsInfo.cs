/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TableDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Table";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "TableApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Table");
        public (string, string) InheritsLink {get; set; } = ("<TItem> : ClearComponentBase", "<TItem> : ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBackground", " IBackgroundApi"),
            ("IBoxShadow", "IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("Items", "List<TItem>", "new List<TItem>()", ""),
            new ApiComponentInfo("ColumnDefs", "string", "", ""),
            new ApiComponentInfo("RowSpacing", "int", "5", ""),
            new ApiComponentInfo("ColumnSpacing", "int", "5", ""),
            new ApiComponentInfo("HorizontalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", ""),
            new ApiComponentInfo("VerticalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
