/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SwitchDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Switch";
        public string Description {get; set; } = "A switch input component\r";
        public (string, string) ApiLink  {get; set; } = ("API", "SwitchApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Switch");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Checked", "bool", "null", "Indicates whether the switch is checked. \r"),
            new ApiComponentInfo("CheckedChanged", "EventCallback<bool>", "", "Represents a callback that is triggered when the checked state changes. \r"),
            new ApiComponentInfo("LabelLocation", "<a href=LabelLocationApi>LabelLocation</a>", "LabelLocation.End", "Specifies the location of the label, defaulting to the end position.\r"),
            new ApiComponentInfo("UncheckedColor", "Color?", "null", "Represents the color used when an option is unchecked. \r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
