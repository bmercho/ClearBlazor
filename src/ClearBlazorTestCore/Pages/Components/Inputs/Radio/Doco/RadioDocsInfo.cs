/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record RadioDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Radio<TItem>";
        public string Description {get; set; } = "A radio button input component\r";
        public (string, string) ApiLink  {get; set; } = ("API", "RadioApi");
        public (string, string) ExamplesLink {get; set; } = ("", "");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Checked", "bool", "false", "Indicates whether the radio button is checked   \r"),
            new ApiComponentInfo("Value", "TItem?", "null", "The value of the radio button\r"),
            new ApiComponentInfo("LabelLocation", "<a href=LabelLocationApi>LabelLocation</a>", "LabelLocation.End", "The location of the label\r"),
            new ApiComponentInfo("CheckedIcon", "string", "Icons.Material.Filled.RadioButtonChecked", "The icon to display when the radio button is checked\r"),
            new ApiComponentInfo("UncheckedIcon", "string", "Icons.Material.Filled.RadioButtonUnchecked", "The icon to display when the radio button is unchecked\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
