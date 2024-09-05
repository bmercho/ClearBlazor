/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record WrapPanelDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "WrapPanel";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "WrapPanelApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "WrapPanel");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
            (" IBackground", " IBackgroundApi"),
            (" IBorder", " IBorderApi"),
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
            new ApiComponentInfo("RowSpacing", "double", "0", ""),
            new ApiComponentInfo("ColumnSpacing", "double", "0", ""),
            new ApiComponentInfo("Direction", "<a href=DirectionApi>Direction</a>", "Direction.Row", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
