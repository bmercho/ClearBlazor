/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record CardDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Card";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "CardApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Card");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("CardHeader", "RenderFragment?", "null", ""),
            new ApiComponentInfo("CardSection", "RenderFragment?", "null", ""),
            new ApiComponentInfo("CardFooter", "RenderFragment?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
