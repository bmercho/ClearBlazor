/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record NonVirtualizedTreeDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "NonVirtualizedTree<TItem>";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "NonVirtualizedTreeApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "NonVirtualizedTree");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
            (" IBorder where TItem", " IBorder where TItemApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("Value", "TItem?", "null", ""),
            new ApiComponentInfo("ValueChanged", "EventCallback<TItem?>", "", ""),
            new ApiComponentInfo("Values", "List<TItem?>?", "null", ""),
            new ApiComponentInfo("ValuesChanged", "EventCallback<List<TItem?>>", "", ""),
            new ApiComponentInfo("ListData", "List<TItem>?", "null", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "Color.Transparent", ""),
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
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
