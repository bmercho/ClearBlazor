/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DrawerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Drawer";
        public string Description {get; set; } = "A panel docked to a side of the page that slides in and out to shown or hidden.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "DrawerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Drawer");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
            ("IDisposable", "IDisposableApi"),
            (" IObserver<BrowserSizeInfo>", " IObserver<BrowserSizeInfo>Api"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("DrawerLocation", "<a href=DrawerLocationApi>DrawerLocation</a>", "DrawerLocation.Left", "The side that the drawer will reside.\r"),
            new ApiComponentInfo("DrawerMode", "<a href=DrawerModeApi>DrawerMode</a>", "DrawerMode.Responsive", "The drawer mode.\r"),
            new ApiComponentInfo("DrawerContent", "RenderFragment?", "null", "The content of the drawer\r"),
            new ApiComponentInfo("DrawerBody", "RenderFragment?", "null", "The content of the drawer body\r"),
            new ApiComponentInfo("Open", "bool", "false", "Indicates if the drawer is open\r"),
            new ApiComponentInfo("OverlayEnabled", "bool", "true", "Indicates if an overlay will be shown over the container of the drawer\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("OpenChanged", "EventCallback<bool>", "", "Event that is raised when the drawer is opened or closed.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
