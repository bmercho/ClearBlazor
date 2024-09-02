using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;

namespace ClearBlazor
{
    public partial class DrawingCanvas : ClearComponentBase, IBorder, IBackground
    {
        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColour { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }


        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

        [Parameter]
        public EventCallback<CanvasSize> OnCanvasSizeChange { get; set; }

        [Parameter]
        public EventCallback<Batch2D?> OnPaint { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasClick { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseDown { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseMove { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseUp { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseOver { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasMouseOut { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCanvasContextMenu { get; set; }

        [Parameter]
        public EventCallback<WheelEventArgs> OnCanvasMouseWheel { get; set; }

        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchMove { get; set; }

        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchStart { get; set; }

        [Parameter]
        public EventCallback<CanvasTouchEventArgs> OnCanvasTouchEnd { get; set; }


        SizeInfo? previousSizeInfo = null;
        private SizeInfo? SizeInfo = null;
        private Context2D? Canvas = null;
        public string CanvasId = Guid.NewGuid().ToString();
        private Canvas? CanvasReference = null;
        private ElementReference CanvasContainerElement;
        private bool RenderingInProgress = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (RenderingInProgress)
                return;

            RenderingInProgress = true;
            try
            {
                if (firstRender)
                {
                    Canvas = await CanvasReference!.GetContext2DAsync();
                    await JSRuntime.InvokeVoidAsync("ResizeCanvas", CanvasId);

                }
                SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", CanvasContainerElement);

                if (previousSizeInfo == null ||
                    !previousSizeInfo.Equals(SizeInfo))
                {
                    if (previousSizeInfo == null || previousSizeInfo.ElementWidth != SizeInfo.ElementWidth ||
                        previousSizeInfo.ElementHeight != SizeInfo.ElementHeight)
                    {
                        await OnCanvasSizeChange.InvokeAsync(new CanvasSize(SizeInfo.ElementWidth, SizeInfo.ElementHeight));
                        await RefreshCanvas();
                    }
                    StateHasChanged();
                    previousSizeInfo = SizeInfo;
                }
            }
            finally
            {
                RenderingInProgress = false;
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += $"touch-action: none; ";
            return css;
        }


        public async Task RefreshCanvas()
        {
            if (Canvas == null || SizeInfo == null)
                return;

            await using (var context = Canvas.CreateBatch())
            {
                await OnPaint.InvokeAsync(context);
            }
        }

        public async Task OnClick(MouseEventArgs e)
        {
            await OnCanvasClick.InvokeAsync(e);
        }

        public async Task OnMouseDown(MouseEventArgs e)
        {
            await OnCanvasMouseDown.InvokeAsync(e);
        }

        public async Task OnMouseUp(MouseEventArgs e)
        {
            await OnCanvasMouseUp.InvokeAsync(e);
        }

        public async Task OnMouseMove(MouseEventArgs e)
        {
            await OnCanvasMouseMove.InvokeAsync(e);
        }

        public async Task OnMouseOver(MouseEventArgs e)
        {
            await OnCanvasMouseOver.InvokeAsync(e);
        }

        public async Task OnMouseOut(MouseEventArgs e)
        {
            await OnCanvasMouseOut.InvokeAsync(e);
        }

        public async Task OnMouseWheel(WheelEventArgs e)
        {
            await OnCanvasMouseOut.InvokeAsync(e);
        }

        public async Task OnContextMenu(MouseEventArgs e)
        {
            await OnCanvasContextMenu.InvokeAsync(e);
        }

        public async Task OnTouchStart(TouchEventArgs e)
        {
            if (SizeInfo == null || e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchStart.InvokeAsync(new CanvasTouchEventArgs(e,
                                                                          e.ChangedTouches[0].ClientX - SizeInfo.ElementX,
                                                                          e.ChangedTouches[0].ClientY - SizeInfo.ElementY));
        }
        public async Task OnTouchEnd(TouchEventArgs e)
        {
            if (SizeInfo == null || e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchEnd.InvokeAsync(new CanvasTouchEventArgs(e,
                                                                        e.ChangedTouches[0].ClientX - SizeInfo.ElementX,
                                                                        e.ChangedTouches[0].ClientY - SizeInfo.ElementY));
        }
        public async Task OnTouchMove(TouchEventArgs e)
        {
            if (SizeInfo == null || e.ChangedTouches.Length == 0)
                return;

            await OnCanvasTouchMove.InvokeAsync( new CanvasTouchEventArgs(e, 
                                                                          e.ChangedTouches[0].ClientX - SizeInfo.ElementX,
                                                                          e.ChangedTouches[0].ClientY - SizeInfo.ElementY));
        }
    }
}