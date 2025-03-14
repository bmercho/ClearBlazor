/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ListBoxDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ListBox<TListBox>";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ListBoxApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ListBox");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Value", "TListBox?", "null", ""),
            new ApiComponentInfo("ValueChanged", "EventCallback<TListBox?>", "", ""),
            new ApiComponentInfo("Values", "List<TListBox?>?", "null", ""),
            new ApiComponentInfo("ValuesChanged", "EventCallback<List<TListBox?>>", "", ""),
            new ApiComponentInfo("ListData", "List<ListDataItem<TListBox>>?", "null", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColor", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("ContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", ""),
            new ApiComponentInfo("Spacing", "double", "0", ""),
            new ApiComponentInfo("RowSize", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Clickable", "bool", "true", ""),
            new ApiComponentInfo("MultiSelect", "bool", "false", ""),
            new ApiComponentInfo("AllowSelectionToggle", "bool", "false", ""),
            new ApiComponentInfo("OnSelectionChanged", "EventCallback<ListDataItem<TListBox>>", "", ""),
            new ApiComponentInfo("OnSelectionsChanged", "EventCallback<List<ListDataItem<TListBox>>>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task<bool> SetSelected(ListBoxItem<TListBox> item)", "async", "", ""),
        };
    }
}
