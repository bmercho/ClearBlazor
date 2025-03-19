/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SliderDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Slider<TItem>";
        public string Description {get; set; } = "A slider component that allows users to select a value within a specified range using a thumb control.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "SliderApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Slider");
        public (string, string) InheritsLink {get; set; } = ("InputBase where TItem", "InputBase where TItemApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Value", "TItem?", "default", "Represents an optional value of type TItem. It can be set to null or a default value.\r"),
            new ApiComponentInfo("ValueChanged", "EventCallback<TItem>", "", "Represents a callback that is invoked when the value changes. It allows for handling updates to the value in\ra component.\r"),
            new ApiComponentInfo("Min", "TItem", "TItem.Zero", "Defines a minimum value of type TItem, initialized to TItem.Zero.\r"),
            new ApiComponentInfo("Max", "TItem", "TItem.CreateTruncating(100)", "Defines a maximum value of type TItem, initialized to a truncating value of 100. \r"),
            new ApiComponentInfo("Step", "TItem?", "TItem.CreateTruncating(1)", "Represents a step value of type TItem, initialized to a truncating value of 1. It can be nullable.\r"),
            new ApiComponentInfo("ContrastTrackBackground", "bool", "false", "Indicates whether to track the background contrast. Defaults to false.\r"),
            new ApiComponentInfo("ShowTickMarks", "bool", "false", ""),
            new ApiComponentInfo("ShowTickMarkLabels", "bool", "false", ""),
            new ApiComponentInfo("ShowValueLabels", "bool", "false", ""),
            new ApiComponentInfo("TickMarkLabelFormat", "string?", "null", ""),
            new ApiComponentInfo("TickMarkLabels", "List<string>?", "null", ""),
            new ApiComponentInfo("BackgroundGradient1", "string?", "null", ""),
            new ApiComponentInfo("BackgroundGradient2", "string?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("text, string tickMargin, string textMargin)", "TickData(string", "", ""),
        };
    }
}
