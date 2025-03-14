/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToggleIconButtonDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ToggleIconButton";
        public string Description {get; set; } = "A button that has an on and off state.\rWhen toggled shows the ToggledIcon and ToggledText otherwise shows th Icon and Text\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ToggleIconButtonApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ToggleIconButton");
        public (string, string) InheritsLink {get; set; } = ("Button", "ButtonApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("OnToggleChanged", "EventCallback<bool>", "", "An event that is raised when the button is toggled\r"),
            new ApiComponentInfo("ToggledIcon", "string?", "null", "The toggled icon. Icon is used for the un-toggled icon.\r"),
            new ApiComponentInfo("ToggledIconColor", "Color?", "null", "The toggled icon color. Color is used for the un-toggled icon color.\r"),
            new ApiComponentInfo("Text", "string", "string.Empty", "The un-toggled text\r"),
            new ApiComponentInfo("ToggledText", "string", "string.Empty", "The toggled text\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task<bool> Toggle()", "async", "", "Toggles the button.\r"),
        };
    }
}
