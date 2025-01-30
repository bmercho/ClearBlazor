using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;


namespace ClearBlazor
{
    public partial class RootComponent : ClearComponentBase, IObserver<BrowserSizeInfo>, IBackground
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = Color.Light;

        [Parameter]
        public EventCallback LayoutComplete { get; set; }

        ThemeManager ThemeManager { get; set; }
        private IDisposable? unsubscriber;
        private bool LoadingComplete = false;
        SkiaDrawingCanvas _canvasView = null!;
        private string _canvasId = Guid.NewGuid().ToString();

        public RootComponent()
        {
            ThemeManager = new ThemeManager(this, false);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
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
            else
            {
                PerformLayout(Width, Height);
                await LayoutComplete.InvokeAsync();
            }
        }

        private string GetStyle()
        {
            return $"overflow: hidden; position: relative;height:{Height}px; width:{Width}px; ";
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size resultSize = new Size(0, 0);

            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            if (Children.Count ==1)
            {
                var child = Children[0];
                child.Measure(availableSize);
                resultSize.Width = child.DesiredSize.Width;
                resultSize.Height = child.DesiredSize.Height;
            }

            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ?
                resultSize.Width : availableSize.Width;

            resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ?
                resultSize.Height : availableSize.Height;

            return resultSize;
        }

        protected override void ArrangeOverride(double left, double top)
        {
            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            if (Children.Count == 1)
            {
                var child = Children[0];
                child.Arrange(left, Top);
            }
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

            if (Height == double.PositiveInfinity)
                Height = browserSizeInfo.BrowserHeight;
            if (Width == double.PositiveInfinity)
                Width = browserSizeInfo.BrowserWidth;
            LoadingComplete = true;
            StateHasChanged();
        }

        internal void PaintCanvas(SKCanvas canvas)
        {
            RefreshCanvas(canvas);
        }
    }
}
