/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record UniformGridDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "UniformGrid";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "UniformGridApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "UniformGrid");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
            ("IBorder", "IBorderApi"),
            ("IBoxShadow", "IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColor", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
            new ApiComponentInfo("NumRows", "int?", "null", ""),
            new ApiComponentInfo("NumColumns", "int?", "null", ""),
            new ApiComponentInfo("RowSpacing", "int?", "null", ""),
            new ApiComponentInfo("ColumnSpacing", "int?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
