/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DrawingCanvasDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "DrawingCanvas";
        public string Description {get; set; } = "";
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
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
            new ApiComponentInfo("OnCanvasSizeChange", "EventCallback<CanvasSize>", "", ""),
            new ApiComponentInfo("OnPaint", "EventCallback<Batch2D?>", "", ""),
            new ApiComponentInfo("OnCanvasClick", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasMouseDown", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasMouseMove", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasMouseUp", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasMouseOver", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasMouseOut", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasContextMenu", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasMouseWheel", "EventCallback<WheelEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasTouchMove", "EventCallback<CanvasTouchEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasTouchStart", "EventCallback<CanvasTouchEventArgs>", "", ""),
            new ApiComponentInfo("OnCanvasTouchEnd", "EventCallback<CanvasTouchEventArgs>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task RefreshCanvas()", "async", "", ""),
            new ApiComponentInfo("Task OnClick(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnMouseDown(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnMouseUp(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnMouseMove(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnMouseOver(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnMouseOut(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnMouseWheel(WheelEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnContextMenu(MouseEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnTouchStart(TouchEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnTouchEnd(TouchEventArgs e)", "async", "", ""),
            new ApiComponentInfo("Task OnTouchMove(TouchEventArgs e)", "async", "", ""),
        };
    }
}
