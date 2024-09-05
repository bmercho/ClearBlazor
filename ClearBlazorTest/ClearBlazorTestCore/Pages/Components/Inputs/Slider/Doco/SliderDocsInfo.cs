/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SliderDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Slider";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "SliderApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Slider");
        public (string, string) InheritsLink {get; set; } = ("<TItem> : InputBase", "<TItem> : InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackgroundGradient where TItem : struct", " IBackgroundGradient where TItem : structApi"),
            (" INumber<TItem>", " INumber<TItem>Api"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Value", "TItem?", "default", ""),
            new ApiComponentInfo("ValueChanged", "EventCallback<TItem>", "", ""),
            new ApiComponentInfo("Min", "TItem", "TItem.Zero", ""),
            new ApiComponentInfo("Max", "TItem", "TItem.CreateTruncating(100)", ""),
            new ApiComponentInfo("Step", "TItem?", "TItem.CreateTruncating(1)", ""),
            new ApiComponentInfo("ContrastTrackBackground", "bool", "false", ""),
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
