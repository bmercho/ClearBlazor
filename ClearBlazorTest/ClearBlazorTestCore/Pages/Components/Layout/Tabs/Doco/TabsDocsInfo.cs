/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TabsDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Tabs";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "TabsApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Tabs");
        public (string, string) InheritsLink {get; set; } = ("", "Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IContent", "IContentApi"),
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
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Colour", "Color?", "null", ""),
            new ApiComponentInfo("OnTabChanged", "EventCallback<Tab>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
