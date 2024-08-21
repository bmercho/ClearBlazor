using Microsoft.JSInterop;

namespace ClearBlazor
{
    public class BrowserSizeService : IObservable<BrowserSizeInfo>
    {
        private List<IObserver<BrowserSizeInfo>> observers = new List<IObserver<BrowserSizeInfo>>();
        private IJSRuntime JSRuntime = null!;
        private BrowserSizeInfo browserSizeInfo = new BrowserSizeInfo();

        public async void Init(IJSRuntime js)
        {
            if (JSRuntime == null)
            {
                JSRuntime = js;

                await JSRuntime.InvokeAsync<string>("resizeListener", DotNetObjectReference.Create(this));
            }
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

            foreach (var observer in observers)
                observer.OnNext(browserSizeInfo);
            await Task.CompletedTask;
        }

        private DeviceSize GetDeviceSize(int browserWidth)
        {
            if (browserWidth < (int)DeviceSize.Small)
                return DeviceSize.ExtraSmall;
            if (browserWidth < (int)DeviceSize.Medium)
                return DeviceSize.Small;
            if (browserWidth < (int)DeviceSize.Large)
                return DeviceSize.Medium;
            if (browserWidth < (int)DeviceSize.ExtraLarge)
                return DeviceSize.Large;
            return DeviceSize.ExtraLarge;
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<BrowserSizeInfo>> _observers;
            private IObserver<BrowserSizeInfo> _observer;

            public Unsubscriber(List<IObserver<BrowserSizeInfo>> observers, IObserver<BrowserSizeInfo> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

        public IDisposable Subscribe(IObserver<BrowserSizeInfo> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                observer.OnNext(browserSizeInfo);
            }

            return new Unsubscriber(observers, observer);
        }
    }
}
