using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class Virtualize<TItem> : ClearComponentBase
    {
        [Parameter]
        public RenderFragment<(TItem,int)>? ChildContent { get; set; }

        [Parameter]
        public required ScrollViewer ScrollViewer { get; set; }

        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = null!;

        [Parameter]
        public int? ItemHeight { get; set; }

        /// <summary>
        /// Gets or sets the index of the Items to be displayed in the centre of the visible area 
        /// (except if it near the start or end of list, where it wont be in the centre) 
        /// </summary>
        [Parameter]
        public (int index, Alignment verticalAlignment) VisibleIndex { get; set; } = (0, Alignment.Start);

        [Parameter]
        public Alignment HorizontalContentAlignment { get; set; } = Alignment.Stretch;

        private string _firstItemId = Guid.NewGuid().ToString();
        private double _containerHeight = -1;
        private double? _previousContainerHeight = null;
        private double? _previousFirstItemHeight = null;
        private int _visibleIndex = 0;
        private double _height = 0;
        private double _itemWidth = 0;
        private double _itemHeight = -1;
        private int _skipItems = 0;
        private int _takeItems = 0;
        private bool _initialising = true;
        private bool _initialScroll = true;
        private ScrollState _scrollState = new();

        protected override void OnParametersSet()
        {
            base.OnParametersSetAsync();

            // index is 1 based - convert to 0 based
            if (VisibleIndex.index > 0)
                _visibleIndex = VisibleIndex.index - 1;
            else
                _visibleIndex = 0;
            if (ItemHeight != null) 
                _itemHeight = ItemHeight.Value; 
        }

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

            if (ItemHeight == null && _initialising)
            {
                var firstItemSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", _firstItemId);

                if (firstItemSizeInfo == null ||
                     _previousFirstItemHeight != firstItemSizeInfo.ElementHeight)
                {
                    if (firstItemSizeInfo != null)
                    {
                        _previousFirstItemHeight = firstItemSizeInfo.ElementHeight;
                        _itemHeight = firstItemSizeInfo.ElementHeight;
                        changed = true;
                    }
                    StateHasChanged();
                }
            }

            if (changed && _containerHeight > 0 && _itemHeight > 0)
            {
                if (_initialising)
                {
                    _initialising = false;
                    await JSRuntime.InvokeVoidAsync("window.scrollbar.ListenForScrollEvents", ScrollViewer.Id, 
                                                    DotNetObjectReference.Create(this));
                    // Do not await otherwise initially the scroll top is not set???
                    CalculateScrollItems(true);
                }
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

        public async Task GotoIndex(int index, Alignment verticalAlignment)
        {
            // index is 1 based - convert to 0 based
            if (index > 0)
                _visibleIndex = index - 1;
            else
                _visibleIndex = 0;

            await GotoIndex(verticalAlignment);
        }

        private async Task GotoIndex(Alignment verticalAlignment)
        {
            double scrollTop = 0;
            var maxItemsInContainer = _containerHeight / _itemHeight;

            switch (verticalAlignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    _skipItems = _visibleIndex;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * _itemHeight;
                    else
                        scrollTop = (_skipItems - maxItemsInContainer / 2 + 0.5) * _itemHeight;
                    break;
                case Alignment.Start:
                    _skipItems = _visibleIndex;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    scrollTop = _skipItems * _itemHeight;
                    break;
                case Alignment.End:
                    if (_visibleIndex < maxItemsInContainer)
                        _skipItems = 0;
                    else
                        _skipItems = _visibleIndex;
                    _takeItems = (int)Math.Ceiling(maxItemsInContainer);

                    if (_skipItems < maxItemsInContainer)
                        scrollTop = _skipItems * _itemHeight;
                    else
                        scrollTop = (_skipItems - maxItemsInContainer + 1) * _itemHeight;
                    break;
            }

            // Not sure why this has to be called twice. Does not change the scroll position if it is only called once???
            // Also CalculateScrollItems cannot be awaited otherwise it does not change scroll position???
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", ScrollViewer.Id, scrollTop);
            await JSRuntime.InvokeVoidAsync("window.scrollbar.SetScrollTop", ScrollViewer.Id, scrollTop);
        }

        protected string GetContentStyle()
        {
            var css = "display:grid; position:absolute; ";
            switch (HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += $"justify-self:stretch; width:{_itemWidth}px; ";
                    break;
                case Alignment.Start:
                    css += "justify-self:start; ";
                    break;
                case Alignment.Center:
                    css += "justify-self:center; ";
                    break;
                case Alignment.End:
                    css += "justify-self:end; ";
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
            _height = Items.Count() * _itemHeight;
            if (initial)
                await GotoIndex(VisibleIndex.verticalAlignment);
            else
            {
                _skipItems = (int)(_scrollState.ScrollTop / _itemHeight);
                _takeItems = (int)Math.Ceiling((double)(_scrollState.ScrollTop + _containerHeight) / _itemHeight) - _skipItems;
            }
        }
    }
}