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
        public (string, string) InheritsLink {get; set; } = ("ListBase<TItem>", "ListBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBackground", " IBackgroundApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control. Contains the columns for that table.\r"),
            new ApiComponentInfo("index,", "(int", "(0, Alignment.Start)", "Gets or sets the index of the Items to be initially shown in visible area.\rIt can be shown in the centre, start or end of the visible are.\rIs is zero based.\r"),
            new ApiComponentInfo("ShowHeader", "bool", "true", ""),
            new ApiComponentInfo("ColumnDefs", "string", "", ""),
            new ApiComponentInfo("RowSpacing", "int", "5", ""),
            new ApiComponentInfo("ColumnSpacing", "int", "5", ""),
            new ApiComponentInfo("HorizontalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", ""),
            new ApiComponentInfo("VerticalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", ""),
            new ApiComponentInfo("StickyHeader", "bool", "true", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task GotoIndex(int index, Alignment verticalAlignment)", "async", "", "Goto the given index in the data\r"),
            new ApiComponentInfo("Task GotoStart()", "async", "", "Goto the start of the list\r"),
            new ApiComponentInfo("Task GotoEnd()", "async", "", "Goto the end of the list\r"),
            new ApiComponentInfo("Task<bool> AtEnd()", "async", "", "Returns true if the list has been scrolled to the end. \r"),
            new ApiComponentInfo("Task<bool> AtStart()", "async", "", "Returns true if the list is at the start. \r"),
        };
    }
}
