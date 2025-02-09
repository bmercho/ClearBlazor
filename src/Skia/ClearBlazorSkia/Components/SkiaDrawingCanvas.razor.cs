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
    public partial class SkiaDrawingCanvas
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = null!;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Event raised when the canvas size changes
        /// </summary>
        [Parameter]
        public EventCallback<Size> OnCanvasSizeChange { get; set; }

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
        /// Event raised when the canvas is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasDblClick { get; set; }

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
        /// Event raised when the pointer down event occurs
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerDown { get; set; }

        /// <summary>
        /// Event raised when the pointer up event occurs
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerUp { get; set; }

        /// <summary>
        /// Event raised when the pointer enters the canvas
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerEnter { get; set; }

        /// <summary>
        /// Event raised when the pointer leaves the canvas
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerLeave { get; set; }

        /// <summary>
        /// Event raised when the pointer moves
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerMove { get; set; }

        /// <summary>
        /// Event raised when the pointer moves out of the canvas
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerOut { get; set; }

        /// <summary>
        /// Event raised when the pointer moves over of the canvas
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerOver { get; set; }

        /// <summary>
        /// Event raised when the pointer is cancelled
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasPointerCancel { get; set; }

        /// <summary>
        /// Event raised when pointer capture has been obtained
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasGotPointerCapture { get; set; }

        /// <summary>
        /// Event raised when pointer capture has been lost
        /// </summary>
        [Parameter]
        public EventCallback<PointerEventArgs> OnCanvasLostPointerCapture { get; set; }

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

        /// <summary>
        /// Event raised when a finger enters the canvas
        /// </summary>
        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchEnter { get; set; }

        /// <summary>
        /// Event raised when a finger leaves the canvas
        /// </summary>
        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchLeave { get; set; }

        /// <summary>
        /// Event raised when a finger event is cancelled
        /// </summary>
        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchCancel { get; set; }

        SKCanvasView _canvasView = null!;
        private string _canvasId = Guid.NewGuid().ToString();
        private string _id = Guid.NewGuid().ToString();
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
                await JSRuntime.InvokeVoidAsync("ResizeCanvas", _canvasId);
                List<string> elementIds = new List<string>() { _id };

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
            canvas.Scale(1 / _pixelToDeviceX, 1 / _pixelToDeviceY);

            OnPaint.InvokeAsync(canvas);
        }

        internal async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _id)
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
                        await OnCanvasSizeChange.InvokeAsync(new Size(_canvasWidth, _canvasHeight));
                        RefreshCanvas();
                    }
                }
            }
        }

        private async Task OnClick(MouseEventArgs e)
        {
            await OnCanvasClick.InvokeAsync(e);
        }

        private async Task OnDblClick(MouseEventArgs e)
        {
            await OnCanvasDblClick.InvokeAsync(e);
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

        private async Task OnPointerDown(PointerEventArgs e)
        {
            await OnCanvasPointerDown.InvokeAsync(e);
        }

        private async Task OnPointerUp(PointerEventArgs e)
        {
            await OnCanvasPointerUp.InvokeAsync(e);
        }

        private async Task OnPointerEnter(PointerEventArgs e)
        {
            await OnCanvasPointerEnter.InvokeAsync(e);
        }

        private async Task OnPointerLeave(PointerEventArgs e)
        {
            await OnCanvasPointerLeave.InvokeAsync(e);
        }
        private async Task OnPointerMove(PointerEventArgs e)
        {
            await OnCanvasPointerMove.InvokeAsync(e);
        }

        private async Task OnPointerOut(PointerEventArgs e)
        {
            await OnCanvasPointerOut.InvokeAsync(e);
        }
        private async Task OnPointerOver(PointerEventArgs e)
        {
            await OnCanvasPointerOver.InvokeAsync(e);
        }

        private async Task OnPointerCancel(PointerEventArgs e)
        {
            await OnCanvasPointerCancel.InvokeAsync(e);
        }

        private async Task OnGotPointerCapture(PointerEventArgs e)
        {
            await OnCanvasGotPointerCapture.InvokeAsync(e);
        }

        private async Task OnLostPointerCapture(PointerEventArgs e)
        {
            await OnCanvasLostPointerCapture.InvokeAsync(e);
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
        private async Task OnTouchEnter(TouchEventArgs e)
        {
            if (e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchEnter.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _canvasWidth,
                                            e.ChangedTouches[0].ClientY - _canvasHeight));
        }
        private async Task OnTouchLeave(TouchEventArgs e)
        {
            if (e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchLeave.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _canvasWidth,
                                            e.ChangedTouches[0].ClientY - _canvasHeight));
        }
        private async Task OnTouchCancel(TouchEventArgs e)
        {
            if (e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchCancel.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _canvasWidth,
                                            e.ChangedTouches[0].ClientY - _canvasHeight));
        }
    }

}
