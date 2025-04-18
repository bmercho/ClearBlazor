/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ColorPickerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ColorPicker";
        public string Description {get; set; } = "A control for selecting a color.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ColorPickerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ColorPicker");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBackground", " IBackgroundApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Color", "Color?", "null", "The initial selected color\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "ThemeManager.CurrentColorScheme.SurfaceContainerHighest", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
            new ApiComponentInfo("ColorChanged", "EventCallback<Color>", "", "An event raised when the selected color is changed\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
