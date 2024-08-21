using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class RootComponent : ComponentBase, IObserver<BrowserSizeInfo>
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Inject]
        IJSRuntime JSRuntime { get; set; } = null!;

        ThemeManager ThemeManager { get; set; }
        private ElementReference Element;
        private double? Height = null;
        private double? Width = null;
        private IDisposable? unsubscriber;
        
        public RootComponent()
        {
            ThemeManager = new ThemeManager(this, false);
        }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();

            BrowserSizeService.Init(JSRuntime);
            Subscribe(BrowserSizeService);
        }

        private string GetStyle()
        {
            string css = string.Empty;
            if (Height != null)
                css += $"overflow: hidden; position: relative;height:{Height}px; width:{Width}px; ";
            else
                css += $"height: 100vh; overflow: hidden; position: relative; ";
            return css;
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
            StateHasChanged();
        }


    }
}
