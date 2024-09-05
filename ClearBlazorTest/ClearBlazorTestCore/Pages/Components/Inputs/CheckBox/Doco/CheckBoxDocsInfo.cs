/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record CheckBoxDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "CheckBox<TItem>";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "CheckBoxApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "CheckBox");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Checked", "TItem?", "null", ""),
            new ApiComponentInfo("CheckedChanged", "EventCallback<TItem>", "", ""),
            new ApiComponentInfo("LabelLocation", "<a href=LabelLocationApi>LabelLocation</a>", "LabelLocation.End", ""),
            new ApiComponentInfo("CheckedIcon", "string", "Icons.Material.Filled.CheckBox", ""),
            new ApiComponentInfo("UncheckedIcon", "string", "Icons.Material.Filled.CheckBoxOutlineBlank", ""),
            new ApiComponentInfo("IndeterminateIcon", "string", "Icons.Material.Filled.IndeterminateCheckBox", ""),
            new ApiComponentInfo("TriState", "bool", "false", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
