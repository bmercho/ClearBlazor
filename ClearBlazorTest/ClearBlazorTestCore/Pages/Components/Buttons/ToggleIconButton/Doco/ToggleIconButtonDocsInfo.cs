/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToggleIconButtonDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ToggleIconButton";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ToggleIconButtonApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ToggleIconButton");
        public (string, string) InheritsLink {get; set; } = ("Button", "ButtonApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("OnToggleChanged", "EventCallback<bool>", "", ""),
            new ApiComponentInfo("ToggledIcon", "string?", "null", ""),
            new ApiComponentInfo("ToggledIconColor", "Color?", "null", ""),
            new ApiComponentInfo("Text", "string", "string.Empty", ""),
            new ApiComponentInfo("ToggledText", "string", "string.Empty", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
