/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DrawingCanvasDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "DrawingCanvas";
        public string Description {get; set; } = "A canvas that can be used to draw on, using the third party library, Excubo.Blazor.Canvas\rwhich is a wrapper around the Html5 canvas element.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "DrawingCanvasApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "DrawingCanvas");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("OnCanvasSizeChange", "EventCallback<CanvasSize>", "", "Event raised when the canvas size changes\r"),
            new ApiComponentInfo("OnPaint", "EventCallback<Batch2D?>", "", "Event raised when the canvas should be redrawn\r"),
            new ApiComponentInfo("OnCanvasClick", "EventCallback<MouseEventArgs>", "", "Event raised when the canvas is clicked\r"),
            new ApiComponentInfo("OnCanvasMouseDown", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse down event occurs on the canvas\r"),
            new ApiComponentInfo("OnCanvasMouseMove", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse is moved over the canvas\r"),
            new ApiComponentInfo("OnCanvasMouseUp", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse up event occurs on the canvas\r"),
            new ApiComponentInfo("OnCanvasMouseOver", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse moves onto a canvas\r"),
            new ApiComponentInfo("OnCanvasMouseOut", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse moves out of a canvas\r"),
            new ApiComponentInfo("OnCanvasContextMenu", "EventCallback<MouseEventArgs>", "", "Event raised when the a context menu should be shown\r"),
            new ApiComponentInfo("OnCanvasMouseWheel", "EventCallback<WheelEventArgs>", "", "Event raised when the mouse wheel is rolled\r"),
            new ApiComponentInfo("OnCanvasTouchMove", "EventCallback<CanvasTouchEventArgs>", "", "Event raised when a finger is dragged across the canvas\r"),
            new ApiComponentInfo("OnCanvasTouchStart", "EventCallback<CanvasTouchEventArgs>", "", "Event raised when a finger is placed on the canvas\r"),
            new ApiComponentInfo("OnCanvasTouchEnd", "EventCallback<CanvasTouchEventArgs>", "", "Event raised when a finger is removed from the canvas\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task RefreshCanvas()", "async", "", "Call to refresh the canvas\r"),
        };
    }
}
