/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TableDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Table";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } =  ("{docInfo.ApiLink.Item1}", "{docInfo.ApiLink.Item2}");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Table");
        public (string, string) InheritsLink {get; set; } = ("", "Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
