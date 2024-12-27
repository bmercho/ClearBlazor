using Microsoft.JSInterop;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ClearBlazor
{
    public class ResizeObserverService : IAsyncDisposable
    {
        private IJSRuntime? _jsRuntime = null;
        private IJSObjectReference? _module = null;
        private ConcurrentDictionary<string, ResizeObserverInfo> _observers = new();


        public static ResizeObserverService Service { get; private set; } = null!;

        public ResizeObserverService()
        {
            Service = this;
        }

        public async Task Init(IJSRuntime js)
        {
            if (_jsRuntime == null)
            {
                _jsRuntime = js;

                _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                                          "./_content/ClearBlazorSkia/ResizeObserverManager.js");
            }
        }

        public async Task<string> AddResizeObserver(Func<List<ObservedSize>,Task> callback,
                                                    IEnumerable<string> elementIds)
        {
            if (_module == null)
                return string.Empty;

            var id = Guid.NewGuid().ToString();
            ResizeObserverInfo info = new ResizeObserverInfo()
            {
                Callback = callback,
                ElementIds = elementIds,
                ObserverId = id
            };
            if (!_observers.TryAdd(id, info))
                throw new Exception($"Unable to add observer Id:{id}");

            await _module.InvokeAsync<string>("ResizeObserverManager.AddResizeObserver", 
                                      id, DotNetObjectReference.Create(this), elementIds.ToArray());
            return id;
        }

        public async Task ObserveElement(string resizeObserverId, string elementId)
        {
            if (_module == null)
                return;

            if (!_observers.ContainsKey(resizeObserverId))
                return;

            await _module.InvokeAsync<string>("ResizeObserverManager.Observe",
                                          resizeObserverId, elementId);
        }

        public async Task UnobserveElement(string resizeObserverId, string elementId)
        {
            if (_module == null)
                return;

            if (!_observers.ContainsKey(resizeObserverId))
                return;

            await _module.InvokeAsync<string>("ResizeObserverManager.Unobserve",
                                          resizeObserverId, elementId);
        }

        public async Task RemoveResizeObserver(string id)
        {
            if (_module == null)
                return;

            if (!_observers.TryRemove(id, out _))
                throw new Exception($"Unable to remove observer Id:{id}");

            await _module.InvokeVoidAsync("ResizeObserverManager.RemoveResizeObserver", id);
        }

        [JSInvokable]
        public async Task NotifyObservedSizes(string observerId, string jsonObservedSizes)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var observedSizes = JsonSerializer.Deserialize<List<ObservedSize>>(jsonObservedSizes, options);

            if (observedSizes == null)
                return;

            if (!_observers.ContainsKey(observerId))
                return;

            var observer = _observers[observerId];
            await observer.Callback(observedSizes);
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null) 
                await _module.DisposeAsync();

            GC.SuppressFinalize(this);
        }

        internal struct ResizeObserverInfo
        {
            public string ObserverId;
            public IEnumerable<string> ElementIds;
            public Func<List<ObservedSize>, Task> Callback;
        }
    }
}
