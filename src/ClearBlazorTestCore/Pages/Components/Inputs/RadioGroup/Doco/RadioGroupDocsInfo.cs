/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record RadioGroupDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "RadioGroup<TItem>";
        public string Description {get; set; } = "A radio group input component\r";
        public (string, string) ApiLink  {get; set; } = ("API", "RadioGroupApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "RadioGroup");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Required", "bool", "false", "When used in a form, indicates if the field is required\r"),
            new ApiComponentInfo("Value", "TItem?", "null", "The value of the radio group\r"),
            new ApiComponentInfo("RadioGroupData", "List<RadioGroupDataItem<TItem>>", "new List<RadioGroupDataItem<TItem>>()", "The data to be displayed in the radio group\r"),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Landscape", "The orientation of the radio group\r"),
            new ApiComponentInfo("Spacing", "int", "0", "The spacing between the radio buttons\r"),
            new ApiComponentInfo("ValueChanged", "EventCallback<TItem>", "", "Event that is triggered when the value of the radio group changes\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
