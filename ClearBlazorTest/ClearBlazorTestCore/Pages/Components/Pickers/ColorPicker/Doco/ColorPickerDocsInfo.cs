/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ColorPickerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ColorPicker";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ColorPickerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ColorPicker");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
            new ApiComponentInfo("Color", "Color?", "null", ""),
            new ApiComponentInfo("ColorChanged", "EventCallback<Color>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
