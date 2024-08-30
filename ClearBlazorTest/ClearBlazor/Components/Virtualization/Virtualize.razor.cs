using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class Virtualize<TItem> : ClearComponentBase
    {
        [Parameter]
        public RenderFragment<TItem>? ChildContent { get; set; }

        [Parameter]
        public required ScrollViewer ScrollViewer { get; set; }

        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = null!;

        [Parameter]
        public int ItemHeight { get; set; } = 40;

        /// <summary>
        /// Gets or sets the index of the Items to be displayed in the centre of the visible area 
        /// (except if it near the start or end of list, where it wont be in the centre) 
        /// </summary>
        [Parameter]
        public int VisibleIndex { get; set; } = 0;

        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Start;

        private string _contentElementId = Guid.NewGuid().ToString();
        private double _containerHeight = -1;
        private double? _previousContainerHeight = null;
        private double _height = 0;
        private double _itemWidth = 0;
        private int _skipItems = 0;
        private int _takeItems = 0;
        private bool _initialising = true;
        private bool _initialScroll = true;
        private ScrollState _scrollState = new ScrollState();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            bool changed = false;
            var containerSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetParentElementSizeInfoById", Id);

            if (containerSizeInfo == null ||
                _previousContainerHeight != containerSizeInfo.ElementHeight)
            {
                if (containerSizeInfo != null)
                {
                    _previousContainerHeight = containerSizeInfo.ElementHeight;
                    _containerHeight = containerSizeInfo.ElementHeight;
                    _itemWidth = containerSizeInfo.ElementWidth - ThemeManager.CurrentTheme.GetScrollBarProperties().width;
                    changed = true;
                }
                StateHasChanged();
            }

            if (changed && _containerHeight >= 0)
            {
                if (_initialising)
                {
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", ScrollViewer.Id, 
                                                    DotNetObjectReference.Create(this));
                    CalculateScrollItems(true);
                }
                _initialising = false;
                StateHasChanged();
            }
        }

        [JSInvokable]
        public async Task HandleScrollEvent(ScrollState scrollState)
        {
            _scrollState = scrollState;
            await CalculateScrollItems(false);
            StateHasChanged();
        }

        protected override string UpdateStyle(string css)
        {
            return css + $"display: grid; ";
        }

        protected string GetContentStyle()
        {
            var css = "display:flex; position:absolute; ";
            switch (HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += "justify-content:center; ";
                    break;
                case Alignment.Start:
                    css += "justify-content:start; ";
                    break;
                case Alignment.Center:
                    css += "justify-content:center; ";
                    break;
                case Alignment.End:
                    css += "justify-content:end; ";
                    break;
            }
            return css;
        }

        protected string GetItemStyle()
        {
            return "position: absolute;  overflow:hidden; ";
        }

        protected string GetItemTop()
        {
            return $"";
        }

        private async Task CalculateScrollItems(bool initial)
        {
            _height = Items.Count() * ItemHeight;
            if (initial)
            {
                _skipItems = VisibleIndex;
                var maxItemsInContainer = _containerHeight / ItemHeight;
                if (maxItemsInContainer + _skipItems > Items.Count())
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer + _skipItems - Items.Count());  
                else
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                // Not sure why this has to be called twice. Does not change the scroll position if it is only called once???
                // Also this function (ie CalculateScrollItems) cannot be awaited otherwise it does not change scroll position???
                await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", ScrollViewer.Id, 
                                                (_skipItems - maxItemsInContainer /2 + 0.5)* ItemHeight);
                await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", ScrollViewer.Id,
                                                (_skipItems - maxItemsInContainer / 2 + 0.5) * ItemHeight);
            }
            else
            {
                _skipItems = (int)(_scrollState.ScrollTop / ItemHeight);
                _takeItems = (int)Math.Ceiling((double)(_scrollState.ScrollTop + _containerHeight) / ItemHeight) - _skipItems;
            }
        }
    }
}