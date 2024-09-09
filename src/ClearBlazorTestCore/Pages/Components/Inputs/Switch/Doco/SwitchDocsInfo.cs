/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SwitchDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Switch";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "SwitchApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Switch");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Checked", "bool", "null", ""),
            new ApiComponentInfo("CheckedChanged", "EventCallback<bool>", "", ""),
            new ApiComponentInfo("LabelLocation", "<a href=LabelLocationApi>LabelLocation</a>", "LabelLocation.End", ""),
            new ApiComponentInfo("UncheckedColor", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
