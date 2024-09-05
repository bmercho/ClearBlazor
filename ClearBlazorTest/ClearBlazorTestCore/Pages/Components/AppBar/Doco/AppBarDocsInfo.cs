/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AppBarDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "AppBar";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "AppBarApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "AppBar");
        public (string, string) InheritsLink {get; set; } = ("DockPanel", "DockPanelApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IColour", "IColourApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Colour", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("DoSomething()", "void", "", "Do some thing\r"),
        };
    }
}
