/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record RadioDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Radio";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "RadioApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Radio");
        public (string, string) InheritsLink {get; set; } = ("<TItem> : InputBase", "<TItem> : InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Checked", "bool", "false", ""),
            new ApiComponentInfo("Value", "TItem?", "null", ""),
            new ApiComponentInfo("LabelLocation", "<a href=LabelLocationApi>LabelLocation</a>", "LabelLocation.End", ""),
            new ApiComponentInfo("CheckedIcon", "string", "Icons.Material.Filled.RadioButtonChecked", ""),
            new ApiComponentInfo("UncheckedIcon", "string", "Icons.Material.Filled.RadioButtonUnchecked", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Check()", "void", "", ""),
            new ApiComponentInfo("Uncheck()", "void", "", ""),
        };
    }
}
