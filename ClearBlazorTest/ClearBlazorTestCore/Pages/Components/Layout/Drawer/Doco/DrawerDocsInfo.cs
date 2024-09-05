/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DrawerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Drawer";
        public string Description {get; set; } = "";
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
            new ApiComponentInfo("DrawerLocation", "<a href=DrawerLocationApi>DrawerLocation</a>", "DrawerLocation.Left", ""),
            new ApiComponentInfo("DrawerMode", "<a href=DrawerModeApi>DrawerMode</a>", "DrawerMode.Responsive", ""),
            new ApiComponentInfo("DrawerContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("DrawerBody", "RenderFragment?", "null", ""),
            new ApiComponentInfo("Open", "bool", "false", ""),
            new ApiComponentInfo("OpenChanged", "EventCallback<bool>", "", ""),
            new ApiComponentInfo("OverlayEnabled", "bool", "true", ""),
            new ApiComponentInfo("Colour", "Color?", "null", ""),
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
