/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToolbarTrayDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ToolbarTray";
        public string Description {get; set; } = "A control that holds a number of toolbars, that can be reordered and placed on new lines.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ToolbarTrayApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ToolbarTray");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
