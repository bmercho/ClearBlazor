/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record CheckBoxDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "CheckBox<TItem>";
        public string Description {get; set; } = "A checkbox component\r";
        public (string, string) ApiLink  {get; set; } = ("API", "CheckBoxApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "CheckBox");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Checked", "TItem?", "null", "The value of the checkbox\r"),
            new ApiComponentInfo("CheckedChanged", "EventCallback<TItem>", "", "Event that is triggered when the checkbox is checked or unchecked\r"),
            new ApiComponentInfo("LabelLocation", "<a href=LabelLocationApi>LabelLocation</a>", "LabelLocation.End", "The location of the label\r"),
            new ApiComponentInfo("CheckedIcon", "string", "Icons.Material.Filled.CheckBox", "The icon to display when the checkbox is checked\r"),
            new ApiComponentInfo("UncheckedIcon", "string", "Icons.Material.Filled.CheckBoxOutlineBlank", "The icon to display when the checkbox is unchecked\r"),
            new ApiComponentInfo("IndeterminateIcon", "string", "Icons.Material.Filled.IndeterminateCheckBox", "The icon to display when the checkbox is in an indeterminate state\r"),
            new ApiComponentInfo("TriState", "bool", "false", "Indicates whether the checkbox can show an indeterminate state\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
