/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record GridDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Grid";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "GridApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Grid");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IContent", "IContentApi"),
            ("IBackground", "IBackgroundApi"),
            ("IBoxShadow", "IBoxShadowApi"),
            (" IBorder", " IBorderApi"),
            (" IBackgroundGradient", " IBackgroundGradientApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", ""),
            new ApiComponentInfo("Rows", "string", "*", ""),
            new ApiComponentInfo("ColumnSpacing", "double", "0", ""),
            new ApiComponentInfo("RowSpacing", "double", "0", ""),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
            new ApiComponentInfo("BackgroundGradient1", "string?", "null", ""),
            new ApiComponentInfo("BackgroundGradient2", "string?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
