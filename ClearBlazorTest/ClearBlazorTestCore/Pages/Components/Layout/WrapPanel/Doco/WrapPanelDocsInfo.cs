/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record WrapPanelDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "WrapPanel";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } =  ("{docInfo.ApiLink.Item1}", "{docInfo.ApiLink.Item2}");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "WrapPanel");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContent"),
            (" IBackground", " IBackground"),
            (" IBorder", " IBorder"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
