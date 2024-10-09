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
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            ("", "Api"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RenderFragment<TItem>?", "required", "null", "The template for rendering each row.\rThe item is passed to each child for customization of the row\r"),
            new ApiComponentInfo("Items", "IEnumerable<TItem>?", "null", " The items to be displayed in the list. If this is not null DataProvider is used.\r If DataProvider is also not null then Items takes precedence.\r"),
            new ApiComponentInfo("DataProvider", "DataProviderRequestDelegate<TItem>?", "null", "Defines the data provider used to get pages of data from where ever. eg database\rUsed if Items is null.\r"),
            new ApiComponentInfo("SelectedItems", "List<TItem>", "new()", "The currently selected items. (when in Multiselect mode)\r"),
            new ApiComponentInfo("SelectedItem", "TItem?", "default", "The currently selected item. (when in single select mode)\r"),
            new ApiComponentInfo("SelectedItemsChanged", "EventCallback<List<TItem>>", "", "Event that is raised when the SelectedItems is changed.(when in multi select mode)\r"),
            new ApiComponentInfo("SelectedItemChanged", "EventCallback<TItem>", "", "Event that is raised when the SelectedItem is changed.(when in single select mode)\r"),
            new ApiComponentInfo("SelectionMode", "<a href=SelectionModeApi>SelectionMode</a>", "SelectionMode.None", "The selection mode of this control. One of None, Single, SimpleMulti or Multi.\r"),
            new ApiComponentInfo("AllowSelectionToggle", "bool", "false", "If true, when in single selection mode, allows the selection to be toggled.\r"),
            new ApiComponentInfo("HoverHighlight", "bool", "true", "If true highlights the item when it is hovered over.\r"),
            new ApiComponentInfo("ItemHeight", "int?", "null", "The height to be used for each item.\rThis is only used if the VirtualizeMode is Virtualize.\rIn this case it is optional and if not present the height is obtained from the first item.\r"),
            new ApiComponentInfo("index,", "(int", "(0, Alignment.Start)", "Gets or sets the index of the Items to be initially shown in visible area.\rIt can be shown in the centre, start or end of the visible are.\r"),
            new ApiComponentInfo("VirtualizeMode", "<a href=VirtualizeModeApi>VirtualizeMode</a>", "VirtualizeMode.None", "Indicates how a list of items is Virtualized.\r"),
            new ApiComponentInfo("ShowLoadingSpinner", "bool", "false", "Indicates if the spinner is shown when new data is being loaded. Use when getting data externally and \rit takes some time to load the data.\r"),
            new ApiComponentInfo("PageSize", "int", "10", "Approximately the number of rows that will fit in the ScrollViewer.\rAdjust this until this number al least fills a page.\rShould be too large rather that to small.\r"),
            new ApiComponentInfo("OverscrollBehaviour", "<a href=OverscrollBehaviourApi>OverscrollBehaviour</a>", "OverscrollBehaviour.None", "Defines what happens when the boundary of a scrolling area is reached in the vertical direction. \r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task GotoIndex(int index, Alignment verticalAlignment)", "async", "", "Goto the given index in the data\r"),
            new ApiComponentInfo("Task GotoStart()", "async", "", "Goto the start of the list\r"),
            new ApiComponentInfo("Task GotoEnd()", "async", "", "Goto the end of the list\r"),
            new ApiComponentInfo("Task Refresh()", "async", "", "Refresh the list. Call this when items are added to or deleted from the data or if an item has changed.\rWhen VirtualizationMode is None a new object needs to be created with a new Id for \rall items that need re-rendering. This ensures that only the changed items are re-rendered. (otherwise it would be expensive)\rOther Virtualized modes re-render all items, which should not be expensive as they are virtualized.\r"),
            new ApiComponentInfo("Refresh(TItem item)", "void", "", "Refresh an item in the list when it has been updated. (only re-renders the given item)\r"),
            new ApiComponentInfo("Task<bool> AtEnd()", "async", "", "Returns true if the list has been scrolled to the end. \r"),
            new ApiComponentInfo("Task RemoveAllSelections()", "async", "", ""),
            new ApiComponentInfo("Task HandleScrollEvent(ScrollState scrollState)", "async", "", ""),
        };
    }
}
