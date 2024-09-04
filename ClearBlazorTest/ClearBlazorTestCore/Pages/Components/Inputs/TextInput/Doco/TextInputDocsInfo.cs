/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TextInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TextInput";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } =  ("{docInfo.ApiLink.Item1}", "{docInfo.ApiLink.Item2}");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TextInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<string>", "ContainerInputBase<string>Api");
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
