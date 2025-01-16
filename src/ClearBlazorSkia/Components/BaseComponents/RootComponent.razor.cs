using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;


namespace ClearBlazor
{
    public partial class RootComponent : ClearComponentBase, IObserver<BrowserSizeInfo>
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color BackgroundColor1 { get; set; } = Color.Light;

        ThemeManager ThemeManager { get; set; }
        private IDisposable? unsubscriber;
        private bool LoadingComplete = false;
        SkiaDrawingCanvas _canvasView = null!;
        private string _canvasId = Guid.NewGuid().ToString();

        public RootComponent()
        {
            ThemeManager = new ThemeManager(this, false);
            BackgroundColor1 = Color.Light;
        }


        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            { 
                // Load all javascript

                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazorSkia/ClearBlazor.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazorSkia/ResizeListener.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazorSkia/ResizeCanvas.js");

                var browserSizeService = new BrowserSizeService();
                browserSizeService.Init(JSRuntime);
                var resizeObserverService = new ResizeObserverService();
                await resizeObserverService.Init(JSRuntime);
                Subscribe(browserSizeService);
            }
   
            PerformLayout(Width, Height);
        }

        private string GetStyle()
        {
            string css = string.Empty;
            //if (Height != null)
                css += $"overflow: hidden; position: relative;height:{Height}px; width:{Width}px; ";
            //else
            //    css += $"height: 100vh; overflow: hidden; position: relative; ";
            return css;
        }

        private string GetContentStyle()
        {
            string css = string.Empty;
            css += $"position:absolute;width:{Width}px;height:{Height}px;";
            return css;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size resultSize = new Size(0, 0);

            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            foreach (ClearComponentBase child in Children)
            {
                child.Measure(availableSize);
                resultSize.Width = Math.Max(resultSize.Width, child.DesiredSize.Width);
                resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height);
            }

            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ?
                resultSize.Width : availableSize.Width;

            resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ?
                resultSize.Height : availableSize.Height;

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            foreach (ClearComponentBase child in Children)
            {
                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
            }

            return finalSize;
        }
        public void Refresh()
        {
            try
            {
                StateHasChanged();
            }
            catch
            {
            }
        }

        public virtual void Subscribe(IObservable<BrowserSizeInfo> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            if (unsubscriber != null)
                unsubscriber.Dispose();
        }
        public virtual void OnCompleted()
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnNext(BrowserSizeInfo browserSizeInfo)
        {
            if (browserSizeInfo.BrowserHeight == 0 || browserSizeInfo.BrowserWidth == 0)
                return;

            Height = browserSizeInfo.BrowserHeight;
            Width = browserSizeInfo.BrowserWidth;
            LoadingComplete = true;
            StateHasChanged();
        }

        private void PerformLayout(double width, double height)
        {
            Measure(new Size(width, height));
            Arrange(new Rect(width, height, 0, 0));
        }
        internal override void PaintCanvas(SKCanvas canvas)
        {
            //DrawingCanvas = canvas;
            canvas.Clear(BackgroundColor1.ToSKColor());
            canvas.Clear(SKColors.Red);
            base.PaintCanvas(canvas);
        }
    }
}
