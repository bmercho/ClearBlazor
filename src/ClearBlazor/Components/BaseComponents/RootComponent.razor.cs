using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class RootComponent : ClearComponentBase, IDisposable, IBackground
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        //[Inject]
        //IJSRuntime JSRuntime { get; set; } = null!;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// An event that is raised when the loading of all the java script is complete
        /// </summary>
        [Parameter]
        public EventCallback OnLoadingComplete { get; set; }


        [Inject]
        NavigationManager NavManager { get; set; } = null!;

        ThemeManager ThemeManager { get; set; }
        private ElementReference Element;
        //private double? Height = null;
        //private double? Width = null;
        private bool LoadingComplete = false;
        BrowserSizeService _browserSizeService = BrowserSizeService.GetInstance();

        public RootComponent()
        {
            ThemeManager = new ThemeManager(this, false);
        }

        bool? _backgroundIsNull = null;
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_backgroundIsNull == null)
                _backgroundIsNull = BackgroundColor == null;

            if (_backgroundIsNull == true)
                BackgroundColor = ThemeManager.CurrentColorScheme.Surface;
        }

        protected override async  Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                // Load all javascript
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/ClearBlazor.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/MouseCapture.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/KeyboardCapture.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/ResizeCanvas.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/ScrollManager.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/SizeInfo.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/ElementSizeInfo.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/SetClasses.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/SetStyleProperty.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/InfiniteScrolling.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/StopPropagation.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/Cursor.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/GridSizeInfo.js");
                await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                 "./_content/ClearBlazor/ResizeListener.js");
                await ThemeManager.UpdateTheme(JSRuntime);
    
                _browserSizeService.Init(JSRuntime);
                _browserSizeService.OnBrowserResize += BrowserResized;

                var resizeObserverService = new ResizeObserverService();
                await resizeObserverService.Init(JSRuntime);

                LoadingComplete = true;
                StateHasChanged();
                await OnLoadingComplete.InvokeAsync();
            }

            RenderAll = false;
        }

        private string GetStyle()
        {
            string css = string.Empty;
            //if (Height != null)
                css += $"overflow: hidden; position: relative;height:{Height}px; width:{Width}px; ";
            //else
            //    css += $"height: 100vh; overflow: hidden; position: relative; ";

            if (BackgroundColor != null)
                css += $"background-color: {BackgroundColor.Value} ;";   
            return css;
        }

        public void Refresh()
        {
            try
            {
                ClearComponentBase.RenderAll = true;
                StateHasChanged();
            }
            catch
            {
            }
        }

        /// <summary>
        /// The theme has changed so re-navigate to the current uri to allow 
        /// new theme to take affect 
        /// </summary>
        /// <returns></returns>
        public async Task ThemeChanged()
        {
            try
            {
                await ThemeManager.UpdateTheme(JSRuntime);
                var uri = NavManager.Uri;
                NavManager.NavigateTo("/");
                NavManager.NavigateTo(uri);
            }
            catch
            {
            }
        }

        private async Task BrowserResized(BrowserSizeInfo browserSizeInfo)
        {
            if (browserSizeInfo.BrowserHeight == 0 || browserSizeInfo.BrowserWidth == 0)
                return;

            Height = browserSizeInfo.BrowserHeight;
            Width = browserSizeInfo.BrowserWidth;
            StateHasChanged();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _browserSizeService.OnBrowserResize -= BrowserResized;
        }

    }
}
