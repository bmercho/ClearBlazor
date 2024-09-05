/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SelectDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Select";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "SelectApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Select");
        public (string, string) InheritsLink {get; set; } = ("<TItem> : ContainerInputBase<TItem>", "<TItem> : ContainerInputBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("SelectData", "List<ListDataItem<TItem>>?", "null", ""),
            new ApiComponentInfo("Values", "List<TItem?>?", "null", ""),
            new ApiComponentInfo("ValuesChanged", "EventCallback<List<TItem?>>", "", ""),
            new ApiComponentInfo("MultiSelect", "bool", "false", ""),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomLeft", ""),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopLeft", ""),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", ""),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
