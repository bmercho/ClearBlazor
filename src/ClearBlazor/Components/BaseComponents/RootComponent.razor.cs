using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;

namespace ClearBlazor
{
    public partial class RootComponent : ClearComponentBase, IDisposable, IBackground
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// Dialog control.
        /// </summary>
        [Parameter]
        public RenderFragment? CurrentDialog { get; set; } = null;

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
        private bool LoadingComplete = false;
        BrowserSizeService _browserSizeService = BrowserSizeService.GetInstance();

        private Type? dialogType = null;
        private Dictionary<string, object> dialogParameters = [];

        public RootComponent()
        {
            ThemeManager = new ThemeManager(this, false);
        }

        public async Task ShowDialog(Type dialogType, Dictionary<string, object> parameters)
        {
            this.dialogType = dialogType;
            this.dialogParameters = parameters;
            await InvokeAsync(StateHasChanged);
        }

        public async Task HideTheDialog()
        {
            dialogType = null;
            dialogParameters = [];
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            RootComponent = this;
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (BackgroundColor == null)
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
                                 "./_content/ClearBlazor/ImageSize.js");
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
                DoRender = true;
                StateHasChanged();
                await OnLoadingComplete.InvokeAsync();
            }
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
                await Refresh();
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
            DoRender = true;
            StateHasChanged();
        }

        public void Dispose()
        {
            _browserSizeService.OnBrowserResize -= BrowserResized;
        }

    }
}
