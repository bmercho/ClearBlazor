/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ContainerInputBaseDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ContainerInputBase<TItem>";
        public string Description {get; set; } = "Base class for all container input components   \r";
        public (string, string) ApiLink  {get; set; } = ("API", "ContainerInputBaseApi");
        public (string, string) ExamplesLink {get; set; } = ("", "");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Value", "TItem?", "default", "The value of the input\r"),
            new ApiComponentInfo("ValueChanged", "EventCallback<TItem>", "", "Event callback when the value changes\r"),
            new ApiComponentInfo("Immediate", "bool", "false", "Indicates whether the input is immediate. ie Changes the Value as soon as input is received,\rotherwise, Value is updated when the user presses Enter or the input loses focus.\rDefaults to false.\r"),
            new ApiComponentInfo("InputContainerStyle", "<a href=InputContainerStyleApi>InputContainerStyle</a>", "InputContainerStyle.Outlined", "The fill mode of the text edit\r"),
            new ApiComponentInfo("TextWrapping", "<a href=TextWrapApi>TextWrap</a>", "TextWrap.NoWrap", "The text wrapping of the text edit\r"),
            new ApiComponentInfo("TextTrimming", "<a href=TextTrimmingApi>TextTrimming</a>", "TextTrimming.None", "The text trimming of the text edit\r"),
            new ApiComponentInfo("IsTextSelectionEnabled", "bool", "false", "Indicates whether text selection is enabled\r"),
            new ApiComponentInfo("Placeholder", "string", "", "The placeholder of the input    \r"),
            new ApiComponentInfo("Clearable", "bool", "false", "Indicates whether the input is clearable via a clear button\r"),
            new ApiComponentInfo("DebounceInterval", "int?", "null", "The debounce interval in milliseconds. Defaults to null (effectively 0).\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
