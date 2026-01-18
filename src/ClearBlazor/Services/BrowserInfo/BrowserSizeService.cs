using Microsoft.JSInterop;

namespace ClearBlazor
{
    public class BrowserSizeService
    {
        public delegate Task BrowserResizeHandlerAsync(BrowserSizeInfo browserSizeInfo);
        public event BrowserResizeHandlerAsync OnBrowserResize = null!;

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
            await js.InvokeAsync<string>("resizeListener",
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
    }
}
