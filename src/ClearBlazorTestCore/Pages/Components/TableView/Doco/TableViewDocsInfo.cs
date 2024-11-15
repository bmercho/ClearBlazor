/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TableViewDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TableView<TItem>";
        public string Description {get; set; } = "TableView is a templated table component supporting virtualization and allowing multiple selections.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "TableViewApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TableView");
        public (string, string) InheritsLink {get; set; } = ("ListViewBase<TItem>", "ListViewBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ShowHeader", "bool", "true", "Indicates if the header row is to be shown or not.\r"),
            new ApiComponentInfo("ColumnSpacing", "int", "5", "The spacing between the columns.\r"),
            new ApiComponentInfo("HorizontalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", "Indicates if horizontal grid lines are to be shown.\r"),
            new ApiComponentInfo("VerticalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", "Indicates if vertical grid lines are to be shown.\r"),
            new ApiComponentInfo("StickyHeader", "bool", "true", "Indicates if the header row (if shown) is sticky. ie stays at top while other rows are scrolled.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
