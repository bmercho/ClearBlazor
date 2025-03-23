using ClearBlazorInternal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace ClearBlazor
{
    public partial class TreeViewNode<TItem> :ListRowBase<TItem>, IDisposable
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

        [Parameter]
        public int RowSpacing { get; set; } = 5;

        [Parameter]
        public int ColumnSpacing { get; set; } = 5;

        /// <summary>
        /// Indicates if vertical grid lines are to be shown.
        /// </summary>
        [Parameter]
        public GridLines VerticalGridLines { get; set; } = GridLines.None;

        [Parameter]
        public List<TableColumn<TItem>> Columns { get; set; } = new List<TableColumn<TItem>>();

        private ListBase<TItem>? _parent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            RowData = NodeData;
            _parent = FindParent<ListBase<TItem>>(Parent);
            if (_parent != null)
                _parent.AddListRow(this);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (_parent != null)
            {
                switch (_parent.VirtualizeMode)
                {
                    case VirtualizeMode.None:
                        break;
                    case VirtualizeMode.Virtualize:
                    case VirtualizeMode.InfiniteScroll:
                        _doRender = true;
                        break;
                    case VirtualizeMode.Pagination:
                        _doRender = true;
                        break;
                }
            }
            await base.SetParametersAsync(parameters);
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            _doRender = false;
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
            if (_parent == null)
                return;
            bool ctrlDown = args.CtrlKey;
            bool shiftDown = args.ShiftKey;
            await _parent.HandleRowSelection(NodeData, RowIndex, ctrlDown, shiftDown);
        }

        public async Task OnNodeClicked(MouseEventArgs args, TItem item)
        {
            if (_parent == null)
                return;
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
            {
                var treeViewBase = _parent as TreeViewBase<TItem>;
                treeViewBase?.Refresh();
            }
            Refresh();
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
            string css = string.Empty;
            if (_parent is TreeView<TItem>)
                css += $"margin-left:{item.Level * 20}px; align-self:center;";
            else
                css += $"margin-left:{item.Level * 20}px; align-self:start;";
            return css;
        }

        protected string GetContentStyle(TItem item)
        {
            if (_parent == null)
                return string.Empty;

            int margin = item.Level * 5;
            if (!item.HasChildren)
                margin += item.Level * 20 + (int)_parent._iconWidth;
            string css = string.Empty;  
            if (_parent is TreeView<TItem>)
                css += $"margin-left:{margin}px; align-self:center; ";
            else
                css += $"margin-left:{margin}px; align-self:start; ";
            switch (_parent._horizontalContentAlignment)
                {
                    case Alignment.Stretch:
                        css += $"justify-self:stretch; ";
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
                return string.Empty;

            var css = string.Empty;

            //if (Columns.Count == 0)
            //    css = "display:flex; flex-direction: row; ";
            //else 
            //    css = "flex-direction: row; display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
            //          $"grid-area: {Index + 1 + header} / 1 /span 1 / span {Columns.Count}; ";

            //if (_parent.VirtualizeMode == VirtualizeMode.Virtualize && _parent._rowHeight > 0)
            //    css += $"position:absolute; height: {_parent._rowHeight}px; width: {_parent._itemWidth}px; " +
            //           $"top: {Index * _parent._rowHeight}px;";


            int header = _parent._showHeader ? 1 : 0;
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize)
            {
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                         $"grid-column: 1 / span {Columns.Count}; grid-row: 1/ span 1;";
                css += $"justify-self:start; position:relative; " +
                       $"top:{(_parent._skipItems + Index + header) * (_parent._rowHeight + RowSpacing)}px;" +
                       $" height: {(_parent._rowHeight + RowSpacing)}px;";
            }
            else
                css += "display:grid; grid-template-columns: subgrid; grid-template-rows: 1fr;" +
                             $"grid-area: {Index + 1 + header} / 1 /span 1 / span {Columns.Count}; ";

            if (_mouseOver)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SurfaceContainerHighest.SetAlpha(.8).Value}; ";

            if (RowData.IsSelected)
                css += $"background-color: {ThemeManager.CurrentColorScheme.SecondaryContainer.Value}; ";


            return css;
        }

        private string GetRowStyle(int row, int column)
        {
            if (_parent == null)
                return string.Empty;

            return $"display:grid; grid-column: {column} /span 1; justify-self: stretch; " +
                   $"padding:0px 0px 0px {ColumnSpacing / 2}px;";
        }

        private string GetVerticalGridLineStyle(int column)
        {
            string css =  $"display:grid; " +
                   $"border-width:0 0 0 1px; border-style:solid; " +
                   $"grid-area: 1 / 2 / span 3 / span 1; " +
                   $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
            return css;
        }

        private string GetHorizontalGridLineStyle(int row, int columnCount)
        {
            if (_parent == null)
                return string.Empty;

            string css = string.Empty;
            if (_parent.VirtualizeMode == VirtualizeMode.Virtualize)
            {
                css += $"align-self:start; border-width:1px 0 0 0; border-style:solid;" +
                       $"display:grid; grid-template-columns: subgrid; " +
                       $"grid-area: 2 / 1 /span 1 / span {Columns.Count};  " +
                       $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
                css += $"justify-self:start; position:relative; " +
                       $"top:{(_parent._skipItems + Index) * (_parent._rowHeight + RowSpacing)}px;";

            }
            else
            {
                css += $"align-self:start; border-width:1px 0 0 0; border-style:solid;" +
                       $"grid-area: {row} / 1 / span 1 / span {columnCount}; " +
                       $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
            }
            return css;
        }

        private string[] GetLines(string? content)
        {
            return content == null ? Array.Empty<string>() : content.Split('\r');
        }

        private string GetContainerDivStyle()
        {
            if (_parent == null)
                return string.Empty;

            string css = "display:grid; grid-template-columns: 1fr auto; ";
            css += $"grid-template-rows: {RowSpacing / 2}px 1fr {RowSpacing / 2}px; ";

            return css;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_parent != null)
                _parent.RemoveListRow(this);
        }
    }
}