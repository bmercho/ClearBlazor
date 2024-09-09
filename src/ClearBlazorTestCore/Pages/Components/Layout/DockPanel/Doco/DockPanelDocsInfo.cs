/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DockPanelDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "DockPanel";
        public string Description {get; set; } = "A Dock Panel is used to dock child elements in the left, right, top, and bottom positions of the panel. \rThe position of child elements is determined by the Dock property of the respective child elements\rIf a child does not have a Dock property it used the remaining available space of the panel.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "DockPanelApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "DockPanel");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=IBackgroundApi>IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
