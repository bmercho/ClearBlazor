/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ColourPickerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ColourPicker";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ColourPickerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ColourPicker");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
            new ApiComponentInfo("Colour", "Color?", "null", ""),
            new ApiComponentInfo("ColourChanged", "EventCallback<Color>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
