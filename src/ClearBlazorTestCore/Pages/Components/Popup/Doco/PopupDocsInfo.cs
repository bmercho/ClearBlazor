/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record PopupDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Popup";
        public string Description {get; set; } = "";
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
            new ApiComponentInfo("UseTransition", "bool", "true", ""),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Open", "bool", "false", ""),
            new ApiComponentInfo("CloseOnOutsideClick", "bool", "true", ""),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", ""),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", ""),
            new ApiComponentInfo("OpenChanged", "EventCallback<bool>", "", ""),
            new ApiComponentInfo("Text", "string?", "null", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomCentre", ""),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopCentre", ""),
            new ApiComponentInfo("Delay", "int?", "null; // Milliseconds", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task MouseDown()", "async", "", ""),
        };
    }
}
