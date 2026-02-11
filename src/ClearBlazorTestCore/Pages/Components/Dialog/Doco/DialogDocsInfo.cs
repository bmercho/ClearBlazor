/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DialogDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Dialog";
        public string Description {get; set; } = "A Dialog control.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "DialogApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Dialog");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
