/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TabsDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Tabs";
        public string Description {get; set; } = "Allows you to split the interface up into different areas, \reach accessible by clicking on the tab header, usually positioned at the top of the control. \r";
        public (string, string) ApiLink  {get; set; } = ("API", "TabsApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Tabs");
        public (string, string) InheritsLink {get; set; } = ("", "Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
            (" IBorder", " IBorderApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of tab header\r"),
            new ApiComponentInfo("Color", "Color?", "null", "The color of the tab header\r"),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=IBackgroundApi>IBackground</a>\r"),
            new ApiComponentInfo("OnTabChanged", "EventCallback<Tab>", "", "An event that is raised when the Tab is changed.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
