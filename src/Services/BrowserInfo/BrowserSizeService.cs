using Microsoft.JSInterop;

namespace ClearBlazor
{
    public class BrowserSizeService : IAsyncDisposable
    {
        public delegate Task BrowserResizeHandlerAsync(BrowserSizeInfo browserSizeInfo);
        public event BrowserResizeHandlerAsync OnBrowserResize = null!;

        private IJSObjectReference? module = null;

        //private List<IObserver<BrowserSizeInfo>> observers = new List<IObserver<BrowserSizeInfo>>();
        private IJSRuntime _JSRuntime = null!;
        private BrowserSizeInfo browserSizeInfo = new BrowserSizeInfo();
        private static BrowserSizeService? Instance = null;

        public static DeviceSize DeviceSize { get; private set; } = DeviceSize.Large;

        public static BrowserSizeService GetInstance()
        {
            if (Instance == null)
                Instance = new BrowserSizeService();
            return Instance;
        }       
        public BrowserSizeService()
        {
            Instance = this;
        }

        public async void Init(IJSRuntime js)
        {
            if (js != null)
            {
                _JSRuntime = js;

                if (module == null)
                    module = await _JSRuntime.InvokeAsync<IJSObjectReference>("import",
                                              "./_content/BrowserInfo/ResizeListener.js");

            }

            if (module != null)
                await _JSRuntime.InvokeAsync<string>("resizeListener",
                                            DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public async Task NotifyBrowserDimensions(int jsBrowserHeight, int jsBrowserWidth)
        {
            browserSizeInfo = new BrowserSizeInfo
            {
                BrowserWidth = jsBrowserWidth,
                BrowserHeight = jsBrowserHeight,
                DeviceSize = GetDeviceSize(jsBrowserWidth)
            };

            OnBrowserResize?.Invoke(browserSizeInfo);
            await Task.CompletedTask;
        }

        private DeviceSize GetDeviceSize(int browserWidth)
        {
            if (browserWidth < (int)DeviceSize.Medium)
                DeviceSize = DeviceSize.Compact;
            else if (browserWidth < (int)DeviceSize.Expanded)
                DeviceSize = DeviceSize.Medium;
            else if (browserWidth < (int)DeviceSize.Large)
                DeviceSize = DeviceSize.Expanded;
            else if (browserWidth < (int)DeviceSize.ExtraLarge)
                DeviceSize = DeviceSize.Large;
            else
                DeviceSize = DeviceSize.ExtraLarge;
            return DeviceSize;
        }

        public async ValueTask DisposeAsync()
        {
            if (module != null)
                await module.DisposeAsync();
        }
    }
}
