using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using SkiaSharp.Views.Blazor;

namespace ClearBlazor
{
    /// <summary>
    /// A canvas that can be used to draw on, using the third party library, SkiaSharp
    /// which is a wrapper around the Skia.
    /// NB. Currently only works for Blazor Wasm.
    /// </summary>
    public partial class SkiaDrawingCanvas : ClearComponentBase, IBorder, IBackground
    {
        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; } = null;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Event raised when the canvas size changes
        /// </summary>
        [Parameter]
        public EventCallback<CanvasSize> OnCanvasSizeChange { get; set; }

        /// <summary>
        /// Event raised when the canvas should be redrawn
        /// </summary>
        [Parameter]
        public EventCallback<SKCanvas?> OnPaint { get; set; }

        /// <summary>
        /// Event raised when the canvas is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasClick { get; set; }

        /// <summary>
        /// Event raised when the mouse down event occurs on the canvas
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseDown { get; set; }

        /// <summary>
        /// Event raised when the mouse is moved over the canvas
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseMove { get; set; }

        /// <summary>
        /// Event raised when the mouse up event occurs on the canvas
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseUp { get; set; }

        /// <summary>
        /// Event raised when the mouse moves onto a canvas
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseOver { get; set; }

        /// <summary>
        /// Event raised when the mouse moves out of a canvas
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseOut { get; set; }

        /// <summary>
        /// Event raised when the a context menu should be shown
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasContextMenu { get; set; }

        /// <summary>
        /// Event raised when the mouse wheel is rolled
        /// </summary>
        [Parameter]
        public EventCallback<WheelEventArgs> OnCanvasMouseWheel { get; set; }

        /// <summary>
        /// Event raised when a finger is dragged across the canvas
        /// </summary>
        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchMove { get; set; }

        /// <summary>
        /// Event raised when a finger is placed on the canvas
        /// </summary>
        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchStart { get; set; }

        /// <summary>
        /// Event raised when a finger is removed from the canvas
        /// </summary>
        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchEnd { get; set; }


        SKCanvasView _canvasView = null!;
        private string _canvasId = Guid.NewGuid().ToString();
        private double _canvasHeight = 0;
        private double _canvasWidth = 0;
        private double _deviceHeight = 0;
        private double _deviceWidth = 0;
        private float _pixelToDeviceX = 0;
        private float _pixelToDeviceY = 0;
        internal string? _resizeObserverId = null;

        /// <summary>
        /// Call to refresh the canvas
        /// </summary>
        /// <returns></returns>
        public void RefreshCanvas()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            _canvasView.Invalidate();
#pragma warning restore CA1416 // Validate platform compatibility
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                List<string> elementIds = new List<string>() { Id };

                _resizeObserverId = await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds);

            }
        }
        private void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var clipBounds = e.Surface.Canvas.DeviceClipBounds;

            if (_canvasWidth > 0)
            {
                _deviceWidth = clipBounds.Width;
                _deviceHeight = clipBounds.Height;
                _pixelToDeviceX = (float)(_canvasWidth / _deviceWidth);
                _pixelToDeviceY = (float)(_canvasHeight / _deviceHeight);
            }


            if (_pixelToDeviceX == 0)
                return;

            var canvas = e.Surface.Canvas;
            canvas.Scale(1/_pixelToDeviceX, 1/_pixelToDeviceY);
            OnPaint.InvokeAsync(canvas);
        }

        internal async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == Id)
                {
                    if (observedSize.ElementHeight > 0 && _canvasHeight != observedSize.ElementHeight)
                    {
                        _canvasHeight = observedSize.ElementHeight;
                        _canvasWidth = observedSize.ElementWidth;
                        if (_deviceWidth > 0)
                        {
                            _pixelToDeviceX = (float)(_canvasWidth / _deviceWidth);
                            _pixelToDeviceY = (float)(_canvasHeight / _deviceHeight);
                        }
                        await JSRuntime.InvokeVoidAsync("ResizeCanvas", _canvasId);
                        await OnCanvasSizeChange.InvokeAsync(new CanvasSize(_canvasWidth, _canvasHeight));
                        RefreshCanvas();
                    }
                }
            }
        }

        private async Task OnClick(MouseEventArgs e)
        {
            await OnCanvasClick.InvokeAsync(e);
        }

        private async Task OnMouseDown(MouseEventArgs e)
        {
            await OnCanvasMouseDown.InvokeAsync(e);
        }

        private async Task OnMouseUp(MouseEventArgs e)
        {
            await OnCanvasMouseUp.InvokeAsync(e);
        }

        private async Task OnMouseMove(MouseEventArgs e)
        {
            await OnCanvasMouseMove.InvokeAsync(e);
        }

        private async Task OnMouseOver(MouseEventArgs e)
        {
            await OnCanvasMouseOver.InvokeAsync(e);
        }

        private async Task OnMouseOut(MouseEventArgs e)
        {
            await OnCanvasMouseOut.InvokeAsync(e);
        }

        private async Task OnMouseWheel(WheelEventArgs e)
        {
            await OnCanvasMouseOut.InvokeAsync(e);
        }

        private async Task OnContextMenu(MouseEventArgs e)
        {
            await OnCanvasContextMenu.InvokeAsync(e);
        }

        private async Task OnTouchStart(TouchEventArgs e)
        {
            if (e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchStart.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _canvasWidth,
                                            e.ChangedTouches[0].ClientY - _canvasHeight));
        }
        private async Task OnTouchEnd(TouchEventArgs e)
        {
            if (e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchEnd.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _canvasWidth,
                                            e.ChangedTouches[0].ClientY - _canvasHeight));
        }
        private async Task OnTouchMove(TouchEventArgs e)
        {
            if (e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchMove.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _canvasWidth,
                                            e.ChangedTouches[0].ClientY - _canvasHeight));
        }
    }

}
