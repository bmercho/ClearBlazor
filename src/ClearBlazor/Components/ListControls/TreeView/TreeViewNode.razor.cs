using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class TreeViewNode<TItem> :ClearComponentBase, IDisposable
           where TItem : TreeItem<TItem>
    {
        [Parameter]
        public required TItem NodeData { get; set; }

        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the node
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? NodeTemplate { get; set; }

        [Parameter]
        public int Index { get; set; }

        private bool _mouseOver = false;
        private bool _doRender = true;    
        private TreeView<TItem>? _parent;

        public void Refresh()
        {
            _doRender = true;
            StateHasChanged();
        }

        internal void Unhighlight()
        {
            _mouseOver = false;
            Refresh();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _parent = FindParent<TreeView<TItem>>(Parent);
            if (_parent != null)
                _parent.AddTreeNode(this);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (_parent != null)
            {
                //switch (_parent.VirtualizeMode)
                //{
                //    case VirtualizeMode.None:
                parameters.TryGetValue<TItem>(nameof(NodeData), out var nodeData);
                //if (nodeData != null)
                //    if (NodeData == null || nodeData.Id != NodeData.Id)
                //        _doRender = true;
                //        break;
                //    case VirtualizeMode.Virtualize:
                //    case VirtualizeMode.InfiniteScroll:
                //        _doRender = true;
                //        break;
                //    case VirtualizeMode.Pagination:
                //        _doRender = true;
                //        break;
                //}
            }
            await base.SetParametersAsync(parameters);

        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            _doRender = true;
        }
        protected override bool ShouldRender()
        {
            return _doRender;
        }

        protected async Task OnMouseEnter()
        {
            if (_parent == null)
                return;
            if (_parent.HoverHighlight)
            {
                _parent.SetHighlightedItem(this);
                _mouseOver = true;
                await Task.CompletedTask;
                _doRender = true;
                StateHasChanged();
            }
        }

        protected async Task OnMouseLeave()
        {
            if (_parent == null)
                return;
            if (_parent.HoverHighlight)
            {
                _parent.SetHighlightedItem(null);
                _mouseOver = false;
                await Task.CompletedTask;
                _doRender = true;
                StateHasChanged();
            }
        }

        public async Task OnRowClicked(MouseEventArgs args, TItem item)
        {
            if (item.HasChildren)
            {
                item.IsExpanded = !item.IsExpanded;
                foreach (var child in item.Children)
                {
                    if (item.IsExpanded)
                        MakeVisible(child);
                    else
                        MakeInvisible(child);
                }
            }
            if (_parent != null)
                await _parent.Refresh();
    
            bool ctrlDown = args.CtrlKey;
            bool shiftDown = args.ShiftKey;
            //await _parent.HandleRowSelection(this, ctrlDown, shiftDown);
        }

        private void MakeVisible(TItem item)
        {
            if (item.Parent != null)
                if (item.Parent.IsExpanded)
                {
                    item.IsVisible = true;
                    foreach (var child in item.Children)
                        MakeVisible(child);
                }
        }

        private void MakeInvisible(TItem item)
        {
            item.IsVisible = false;
            foreach (var child in item.Children)
                MakeInvisible(child);
        }

        protected string GetExpandStyle(TItem item)
        {
            var css = $"margin-left:{item.Level * 20}px; align-self:center;";
            return css;
        }

        protected string GetContentStyle(TItem item)
        {
            if (_parent == null)
                return string.Empty;

            int margin = item.Level * 5;
            if (!item.HasChildren)
                margin += item.Level * 20 + (int)_parent._iconWidth;
            var css = $"margin-left:{margin}px; align-self:center; " +
                      $"background-color:transparent; ";
            switch (_parent.HorizontalContentAlignment)
            {
                case Alignment.Stretch:
                    css += $"justify-self:stretch; width:{Width}px; ";
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

        protected string GetFullRowStyle()
        {
            if (_parent == null)
                return "display:flex; flex-direction: row; ";

            var css = "display:flex; flex-direction: row; ";
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize && _parent.RowHeight > 0)
                css += $"position:absolute; height: {_parent.RowHeight}px; width: {_parent._itemWidth}px; " +
                       $"top: {Index * _parent.RowHeight}px;";
            if (_mouseOver)
                css += $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            if (NodeData.IsSelected)
                css += $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; ";

            return css;
        }


        private string GetContainerDivStyle()
        {
            if (_parent == null)
                return "display:block; ";

            return $"display:block; "; // width:{_parent._itemWidth}px; ";
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_parent != null)
                _parent.RemoveTreeNode(this);
        }
    }
}