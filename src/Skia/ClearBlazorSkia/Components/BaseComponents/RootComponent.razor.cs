using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using Topten.RichTextKit;

namespace ClearBlazor
{
    public partial class RootComponent : PanelBase, IObserver<BrowserSizeInfo>, IBackground
    {
        [Parameter]
        public EventCallback LayoutComplete { get; set; }

        ThemeManager ThemeManager { get; set; }
        private IDisposable? unsubscriber;
        private bool LoadingComplete = false;
        SkiaDrawingCanvas _canvasView = null!;
        //private string _canvasId = Guid.NewGuid().ToString();

        public RootComponent()
        {
            ThemeManager = new ThemeManager(this, false);
        }

        protected override void OnInitialized()
        {
            _isVisualTreeDirty = true;
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
                // Load all java script

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
                if (_isVisualTreeDirty)
                {
                    // PerformLayout may change _isVisualTreeDirty to true in which case
                    // this will be executed again.
                    _isVisualTreeDirty = false; 
                    PerformLayout(Width, Height);
                    StateHasChanged();
                    if (!_isVisualTreeDirty)
                        await LayoutComplete.InvokeAsync();
                }
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

            if (Children.Count == 1)
            {
                var child = Children[0] as PanelBase;
                if (child != null)
                {
                    child.Measure(availableSize);
                    resultSize.Width = child.DesiredSize.Width;
                    resultSize.Height = child.DesiredSize.Height;
                }
            }

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize, 
                                                double offsetHeight, 
                                                double offsetWidth)
        {
            if (Children.Count > 1)
                throw new Exception("The RootComponent can only have a single child.");

            Rect boundRect = new Rect(finalSize);

            if (Children.Count == 1)
            {
                var panel = Children[0] as PanelBase;
                if (panel != null)
                    panel.Arrange(boundRect);
            }

            return finalSize;
        }
        public void Refresh()
        {
            try
            {
                _isVisualTreeDirty = true;
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
