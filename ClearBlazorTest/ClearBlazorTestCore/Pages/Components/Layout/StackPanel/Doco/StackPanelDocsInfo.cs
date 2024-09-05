/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record StackPanelDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "StackPanel";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "StackPanelApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "StackPanel");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("Spacing", "double", "0", ""),
            new ApiComponentInfo("DropZoneName", "string?", "null", ""),
            new ApiComponentInfo("OnElementMouseEnter", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnElementMouseLeave", "EventCallback<MouseEventArgs>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
