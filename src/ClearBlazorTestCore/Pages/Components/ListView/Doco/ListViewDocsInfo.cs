/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ListViewDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ListView<TItem>";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ListViewApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ListView");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("SelectedItems", "List<TItem?>", "new()", "The currently selected items. (when in Multiselect mode)\r"),
            new ApiComponentInfo("SelectedItemsChanged", "EventCallback<List<TItem?>>", "", "Event that is raised when the SelectedItems is changed.(when in multi select mode)\r"),
            new ApiComponentInfo("SelectedItem", "TItem?", "default", "The currently selected item. (when in single select mode)\r"),
            new ApiComponentInfo("SelectedItemChanged", "EventCallback<TItem?>", "", "Event that is raised when the SelectedItem is changed.(when in single select mode)\r"),
            new ApiComponentInfo("OnSelectionChanged", "EventCallback<TItem>", "", "Event that is raised when the SelectedItem is changed.(when in single select mode)\r"),
            new ApiComponentInfo("OnSelectionsChanged", "EventCallback<List<TItem?>>", "", "Event that is raised when the SelectedItems is changed.(when in multi select mode)\r"),
            new ApiComponentInfo("RowTemplate", "RenderFragment<ItemInfo<TItem>>", "null!", "The template for rendering each row.\rThe item is passed to each child for customization of the row\r"),
            new ApiComponentInfo("SelectionMode", "<a href=SelectionModeApi>SelectionMode</a>", "SelectionMode.None", "The selection mode of this control. One of None, Single or Multi\r"),
            new ApiComponentInfo("AllowSelectionToggle", "bool", "false", "If true, when in single selection mode, allows the selection to be toggled.\r"),
            new ApiComponentInfo("HoverHighlight", "bool", "true", "If true highlights the item when it is hovered over.\r"),
            new ApiComponentInfo("Items", "IEnumerable<TItem>?", "null", " The items to be displayed in the list. If this is not null DataProvider is used.\r If DataProvider is also not null then Items takes precedence.\r"),
            new ApiComponentInfo("DataProvider", "DataProviderRequestDelegate<TItem>?", "null", "Defines the data provider used to get pages of data from where ever. eg database\rUsed if Items is null.\r"),
            new ApiComponentInfo("HorizontalContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", "The horizontal content alignment within the control.\r"),
            new ApiComponentInfo("ItemHeight", "int?", "null", "The height of each item.\rIf not provided uses the height of the first item.\rIgnored if VariableItemHeight is true.\r"),
            new ApiComponentInfo("VirtualizeMode", "<a href=VirtualizeModeApi>VirtualizeMode</a>", "VirtualizeMode.None", "If true it ignores ItemHeight and internally uses the InfiniteScroller component\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "Color.Transparent", "See <a href=IBackgroundApi>IBackground</a>\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=IBoxShadowApi>IBoxShadow</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
