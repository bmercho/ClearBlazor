/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ListViewDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ListView<TItem>";
        public string Description {get; set; } = "Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ListViewApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ListView");
        public (string, string) InheritsLink {get; set; } = ("ListBase<TItem>", "ListBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RenderFragment<TItem>?", "required", "null", "The template for rendering each row.\rThe item is passed to each child for customization of the row\r"),
            new ApiComponentInfo("ItemHeight", "int?", "null", "The height to be used for each item.\rThis is only used if the VirtualizeMode is Virtualize.\rIn this case it is optional and if not present the height is obtained from the first item.\r"),
            new ApiComponentInfo("index,", "(int", "(0, Alignment.Start)", "Gets or sets the index of the Items to be initially shown in visible area.\rIt can be shown in the centre, start or end of the visible are.\r"),
            new ApiComponentInfo("ShowLoadingSpinner", "bool", "false", "Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and \rit takes some time to load the data.\r"),
            new ApiComponentInfo("PageSize", "int", "10", "Approximately the number of rows that will fit in the ScrollViewer.\rAdjust this until this number at least fills a page.\rShould be too large rather that to small.\rNot used if VirtualizationMode is None.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task GotoIndex(int index, Alignment verticalAlignment)", "async", "", "Goto the given index in the data. Not used if VirtualizationMode is InfiniteScroll.\r"),
            new ApiComponentInfo("Task GotoStart()", "async", "", "Goto the start of the list\r"),
            new ApiComponentInfo("Task GotoEnd()", "async", "", "Goto the end of the list. Not used if VirtualizationMode is InfiniteScroll.\r"),
            new ApiComponentInfo("NumPages()", "int", "", "Returns the total number of pages. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("CurrentPageNum()", "int", "", "Return the current page number. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task NextPage()", "async", "", "Loads the next page. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task PrevPage()", "async", "", "Loads the previous page. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task GotoPage(int pageNumber)", "async", "", "Goes to the given page number. Used when VirtualizationMode is Pagination\r"),
            new ApiComponentInfo("Task Refresh()", "async", "", "Refresh the list. Call this when items are added to or deleted from the data or if an item has changed.\rWhen VirtualizationMode is None a new object needs to be created with a new Id for \rall items that need re-rendering. This ensures that only the changed items are re-rendered. \r(otherwise it would be expensive)\rOther Virtualized modes re-render all items, which should not be expensive as they are virtualized.\r"),
            new ApiComponentInfo("Task<bool> AtEnd()", "async", "", "Returns true if the list is at the end. \r"),
            new ApiComponentInfo("Task<bool> AtStart()", "async", "", "Returns true if the list is at the start. \r"),
            new ApiComponentInfo("Task HandleScrollEvent(ScrollState scrollState)", "async", "", ""),
        };
    }
}
