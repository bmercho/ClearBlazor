/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record WrapPanelDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "WrapPanel";
        public string Description {get; set; } = "The WrapPanel positions children next to the other, horizontally(default) or vertically,\runtil there is no more room, where it will wrap to the next line(or column) and then continue.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "WrapPanelApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "WrapPanel");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RowSpacing", "double", "0", "The spacing between rows.\r"),
            new ApiComponentInfo("ColumnSpacing", "double", "0", "The spacing between columns.\r"),
            new ApiComponentInfo("Direction", "<a href=DirectionApi>Direction</a>", "Direction.Row", "The direction for the wrapping. \r"),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
