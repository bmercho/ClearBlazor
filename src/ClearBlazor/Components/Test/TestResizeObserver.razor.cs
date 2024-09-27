
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
namespace ClearBlazor
{
    public partial class TestResizeObserver:ClearComponentBase, IAsyncDisposable
    {
        [Inject]
        public NavigationManager? NavManager { get; set; }


        double _div1Height = 0;
        double _div1Width = 0;
        double _div2Height = 0;
        double _div2Width = 0;

        double _div1ObservedHeight = 0;
        double _div1ObservedWidth = 0;
        double _div2ObservedHeight = 0;
        double _div2ObservedWidth = 0;

        Random _random = new Random(); 

        string _resizeObserverId = string.Empty;
        string _div1Id = Guid.NewGuid().ToString();
        string _div2Id = Guid.NewGuid().ToString();


        protected override void OnInitialized()
        {
            base.OnInitialized();
            GetRandomSizes();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                _resizeObserverId = await ResizeObserverService.Service.
                                          AddResizeObserver(NotifyObservedSizes,
                                                            new List<string>() {_div1Id,_div2Id });

        }

        private void GetRandomSizes()
        {
            _div1Height = _random.Next(30, 300);
            _div1Width = _random.Next(30, 300);
            _div2Height = _random.Next(30, 300);
            _div2Width = _random.Next(30, 300);
        }

        private void OnClicked1()
        {
            GetRandomSizes();
            StateHasChanged();

        }

        public async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            foreach(var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _div1Id)
                {
                    _div1ObservedHeight = observedSize.ElementHeight;
                    _div1ObservedWidth = observedSize.ElementWidth;
                }
                else
                {
                    _div2ObservedHeight = observedSize.ElementHeight;
                    _div2ObservedWidth = observedSize.ElementWidth;

                }
                StateHasChanged();
            }

            await Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            await ResizeObserverService.Service.RemoveResizeObserver(_resizeObserverId);
        }

    }
}