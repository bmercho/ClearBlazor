/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record StackPanelDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "StackPanel";
        public string Description {get; set; } = "Arranges children elements into a single line that can be oriented horizontally or vertically.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "StackPanelApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "StackPanel");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", "Defines the orientation of the stack panel. Landscape or portrait\r"),
            new ApiComponentInfo("Spacing", "double", "0", "The spacing between children in the direction defined by Orientation.\r"),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("OnElementMouseEnter", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse enters the component \r"),
            new ApiComponentInfo("OnElementMouseLeave", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse leaves the component \r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
