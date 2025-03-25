/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ListBoxDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ListBox<TListBox>";
        public string Description {get; set; } = "A control that contains a list of ListBoxItems. \rA ListBoxItem can consist of text, icon or am avatar.\rThe list can be hierarchical.\rIf custom UI is required for list items use the ListView control instead.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ListBoxApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ListBox");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Value", "TListBox?", "null", "The selected value of the list box.\r"),
            new ApiComponentInfo("ValueChanged", "EventCallback<TListBox?>", "", "Event that is raised when the Value changes\r"),
            new ApiComponentInfo("Values", "List<TListBox?>?", "null", "The selected values of the list box. (when MultiSelect is true)\r"),
            new ApiComponentInfo("ValuesChanged", "EventCallback<List<TListBox?>>", "", "Event that is raised when Values changes\r"),
            new ApiComponentInfo("ListData", "List<ListDataItem<TListBox>>?", "null", "Provides the data for list. If not null this is used instead of the ChildContent \r"),
            new ApiComponentInfo("ContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", "Used when ListData is not null to horizontally align the content of items\r"),
            new ApiComponentInfo("Spacing", "double", "0", "The spacing between item\r"),
            new ApiComponentInfo("RowSize", "<a href=SizeApi>Size</a>", "Size.Normal", "The row size for each item\r"),
            new ApiComponentInfo("Clickable", "bool", "true", "Indicates if row items are clickable\r"),
            new ApiComponentInfo("MultiSelect", "bool", "false", "Indicates if \r"),
            new ApiComponentInfo("AllowSelectionToggle", "bool", "false", "Allows a single selection to be toggled off as well as on \r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("OnSelectionChanged", "EventCallback<ListDataItem<TListBox>>", "", "Event raised when the selection is changed\r"),
            new ApiComponentInfo("OnSelectionsChanged", "EventCallback<List<ListDataItem<TListBox>>>", "", "Event raised when selections ar changed\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
