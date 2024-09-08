using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// A canvas that can be used to draw on, using the third party library, Excubo.Blazor.Canvas
    /// which is a wrapper around the Html5 canvas element.
    /// </summary>
    public partial class DrawingCanvas : ClearComponentBase, IBorder, IBackground
    {
        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; } = null;

        /// <summary>
        /// See <a href=IBackgroundApi>IBackground</a>
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
        public EventCallback<Batch2D?> OnPaint { get; set; }

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

        private SizeInfo? _previousSizeInfo = null;
        private SizeInfo? _sizeInfo = null;
        private Context2D? _canvas = null;
        private string _canvasId = Guid.NewGuid().ToString();
        private Canvas? _canvasReference = null;
        private ElementReference _canvasContainerElement;
        private bool _renderingInProgress = false;
        private SynchronizationContext? _context;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _context = SynchronizationContext.Current;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_renderingInProgress)
                return;

            _renderingInProgress = true;
            try
            {
                if (firstRender)
                {
                    _canvas = await _canvasReference!.GetContext2DAsync();
                    await JSRuntime.InvokeVoidAsync("ResizeCanvas", _canvasId);

                }
                _sizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", _canvasContainerElement);

                if (_previousSizeInfo == null ||
                    !_previousSizeInfo.Equals(_sizeInfo))
                {
                    if (_previousSizeInfo == null || _previousSizeInfo.ElementWidth != _sizeInfo.ElementWidth ||
                        _previousSizeInfo.ElementHeight != _sizeInfo.ElementHeight)
                    {
                        await OnCanvasSizeChange.InvokeAsync(new CanvasSize(_sizeInfo.ElementWidth, _sizeInfo.ElementHeight));
                        await RefreshCanvas();
                    }
                    StateHasChanged();
                    _previousSizeInfo = _sizeInfo;
                }
            }
            finally
            {
                _renderingInProgress = false;
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += $"touch-action: none; ";
            return css;
        }

        /// <summary>
        /// Call to refresh the canvas
        /// </summary>
        /// <returns></returns>
        public async Task RefreshCanvas()
        {
            if (_canvas == null || _sizeInfo == null)
                return;

            if (_context == null)
            {
                await using (var context = _canvas.CreateBatch())
                {
                    await OnPaint.InvokeAsync(context);
                }
            }
            else
            {
                // Required when running in WPF app
                _context.Post(async
                    delegate
                    {
                        await using (var context = _canvas.CreateBatch())
                        {
                            await OnPaint.InvokeAsync(context);
                        }
                    }, null);
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
            if (_sizeInfo == null || e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchStart.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _sizeInfo.ElementX,
                                            e.ChangedTouches[0].ClientY - _sizeInfo.ElementY));
        }
        private async Task OnTouchEnd(TouchEventArgs e)
        {
            if (_sizeInfo == null || e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchEnd.InvokeAsync(
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _sizeInfo.ElementX,
                                            e.ChangedTouches[0].ClientY - _sizeInfo.ElementY));
        }
        private async Task OnTouchMove(TouchEventArgs e)
        {
            if (_sizeInfo == null || e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchMove.InvokeAsync( 
                new CanvasTouchEventArgs(e, e.ChangedTouches[0].ClientX - _sizeInfo.ElementX,
                                            e.ChangedTouches[0].ClientY - _sizeInfo.ElementY));
        }
    }
}