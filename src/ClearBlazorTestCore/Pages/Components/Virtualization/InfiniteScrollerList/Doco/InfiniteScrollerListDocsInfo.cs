/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record InfiniteScrollerListDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "InfiniteScrollerList<TItem>";
        public string Description {get; set; } = "Virtualizes a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.\rUse this component if the item height is variable or if the total number of items is unknown.\rOtherwise use the VirtualizedList component.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "InfiniteScrollerListApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "InfiniteScrollerList");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBackground", " IBackgroundApi"),
            (" IBoxShadow", " IBoxShadowApi"),
            ("IList<TItem>", "IList<TItem>Api"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RenderFragment<(TItem", "required", "null", "The template for rendering each row.\rThe item is passed to each child for customization of the row\r"),
            new ApiComponentInfo("Items", "IEnumerable<TItem>?", "null", " The items to be displayed in the list. If this is null DataProvider is used.\r If DataProvider and Items are not null then Items takes precedence.\r"),
            new ApiComponentInfo("DataProvider", "DataProviderRequestDelegate<TItem>?", "null", "Defines the data provider used to get pages of data from where ever. eg database\rUsed if Items is null.\r"),
            new ApiComponentInfo("ShowLoadingSpinner", "bool", "false", "Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and \rit takes some time to load the data.\r"),
            new ApiComponentInfo("PageSize", "int", "10", "Approximately the number of rows that will fit in the ScrollViewer.\rAdjust this until this number al least fills a page.\rShould be too large rather that to small.\r"),
            new ApiComponentInfo("index,", "(int", "(0, Alignment.Start)", "Gets or sets the index of the Items to be initially shown  in visible area.\rIt can be shown in the centre, start or end of the visible are.\r"),
            new ApiComponentInfo("HorizontalContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", "The horizontal content alignment within the control.\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=IBoxShadowApi>IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=IBackgroundApi>IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task GotoStart()", "async", "", "Goes to the start of the list\r"),
            new ApiComponentInfo("Task<List<(TItem, int)", "async", "", ""),
            new ApiComponentInfo("Task HandleScrollEvent(ScrollState scrollState)", "async", "", ""),
        };
    }
}
