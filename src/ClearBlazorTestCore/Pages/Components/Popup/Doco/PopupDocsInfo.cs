/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record PopupDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Popup";
        public string Description {get; set; } = "A popup control that can be used to display additional information.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "PopupApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Popup");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IDisposable", " IDisposableApi"),
            (" IObserver<BrowserSizeInfo>", " IObserver<BrowserSizeInfo>Api"),
            (" IObserver<bool>", " IObserver<bool>Api"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("UseTransition", "bool", "true", "Indicates whether to use a transition when opening or closing the popup.\r"),
            new ApiComponentInfo("Open", "bool", "false", "Indicates whether the popup is open or closed.\r"),
            new ApiComponentInfo("CloseOnOutsideClick", "bool", "true", "Indicates whether the popup should close when the user clicks outside of it.\r"),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", "Indicates whether the popup should allow vertical flipping.\r"),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", "Indicates whether the popup should allow horizontal flipping.\r"),
            new ApiComponentInfo("OpenChanged", "EventCallback<bool>", "", "Event that is raised when the popup is opened or closed.\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of the popup.\r"),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomCentre", "The position of the popup.\r"),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopCentre", "The transform of the popup.\r"),
            new ApiComponentInfo("Delay", "int?", "null", "The delay before the popup is displayed. (in milliseconds)\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task MouseDown()", "async", "", ""),
        };
    }
}
