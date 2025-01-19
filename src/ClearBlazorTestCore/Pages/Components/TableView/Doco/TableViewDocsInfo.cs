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
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control. Contains the columns for that table.\r"),
            new ApiComponentInfo("RowHeight", "int", "30", "The height to be used for each row.\rThis is only used if the VirtualizeMode is Virtualize.\r"),
            new ApiComponentInfo("index,", "(int", "null", "Gets or sets the index of the Items to be initially shown in visible area.\rIt can be shown in the centre, start or end of the visible are.\rNot available when VirtualizationMode is InfiniteScroll\r"),
            new ApiComponentInfo("RowSpacing", "int", "5", "The spacing between the rows.\r"),
            new ApiComponentInfo("ShowLoadingSpinner", "bool", "false", "Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and \rit takes some time to load the data.\r"),
            new ApiComponentInfo("PageSize", "int", "10", "Approximately the number of rows that will fit in the ScrollViewer.\rAdjust this until this number at least fills a page.\rShould be too large rather that to small.\rNot used if VirtualizationMode is None or Virtualized.\r"),
            new ApiComponentInfo("ShowHeader", "bool", "true", "Indicates if the header row is to be shown or not.\r"),
            new ApiComponentInfo("ColumnSpacing", "int", "5", "The spacing between the columns.\r"),
            new ApiComponentInfo("HorizontalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", "Indicates if horizontal grid lines are to be shown.\r"),
            new ApiComponentInfo("VerticalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", "Indicates if vertical grid lines are to be shown.\r"),
            new ApiComponentInfo("StickyHeader", "bool", "true", "Indicates if the header row (if shown) is sticky. ie stays at top while other rows are scrolled.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task GotoIndex(int index, Alignment verticalAlignment)", "async", "", "Goto the given index in the data. Not available if VirtualizationMode is InfiniteScroll.\r"),
            new ApiComponentInfo("Task GotoStart()", "async", "", "Goto the start of the list\r"),
            new ApiComponentInfo("Task GotoEnd()", "async", "", "Goto the end of the list. Not available if VirtualizationMode is InfiniteScroll.\r"),
            new ApiComponentInfo("NumPages()", "int", "", "Returns the total number of pages. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("CurrentPageNum()", "int", "", "Return the current page number. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task NextPage()", "async", "", "Loads the next page. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task Scroll(int value)", "async", "", ""),
            new ApiComponentInfo("Task PrevPage()", "async", "", "Loads the previous page. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task GotoPage(int pageNumber)", "async", "", "Goes to the given page number. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task Refresh()", "async", "", "Refresh the list. Call this when items are added to or deleted from the data or if an item has changed.\rWhen VirtualizationMode is None a new object needs to be created with a new Id for \rall items that need re-rendering. This ensures that only the changed items are re-rendered. \r(otherwise it would be expensive)\rOther Virtualized modes re-render all items, which should not be expensive as they are virtualized.\r"),
            new ApiComponentInfo("Task RefreshAll()", "async", "", "Fully refreshes the list \r"),
            new ApiComponentInfo("Task ResetComponent()", "async", "", "Resets the component to its initial state. Mainly used for testing.\r"),
            new ApiComponentInfo("Task<bool> AtEnd()", "async", "", "Returns true if the list is at the end. \r"),
            new ApiComponentInfo("Task<bool> AtStart()", "async", "", "Returns true if the list is at the start. \r"),
            new ApiComponentInfo("Task HandleScrollEvent(ScrollState scrollState)", "async", "", ""),
        };
    }
}
