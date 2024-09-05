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
            new ApiComponentInfo("SelectedItems", "List<TItem?>", "new()", ""),
            new ApiComponentInfo("SelectedItemsChanged", "EventCallback<List<TItem?>>", "", ""),
            new ApiComponentInfo("SelectedItem", "TItem?", "default", ""),
            new ApiComponentInfo("SelectedItemChanged", "EventCallback<TItem?>", "", ""),
            new ApiComponentInfo("RowTemplate", "RenderFragment<ItemInfo<TItem>>", "null!", ""),
            new ApiComponentInfo("SelectionMode", "<a href=SelectionModeApi>SelectionMode</a>", "SelectionMode.None", ""),
            new ApiComponentInfo("AllowSelectionToggle", "bool", "false", ""),
            new ApiComponentInfo("HoverHighlight", "bool", "true", ""),
            new ApiComponentInfo("ListViewData", "List<TItem>?", "null", ""),
            new ApiComponentInfo("HorizontalContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", ""),
            new ApiComponentInfo("ItemHeight", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "Color.Transparent", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColor", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("OnSelectionChanged", "EventCallback<TItem>", "", ""),
            new ApiComponentInfo("OnSelectionsChanged", "EventCallback<List<TItem?>>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
