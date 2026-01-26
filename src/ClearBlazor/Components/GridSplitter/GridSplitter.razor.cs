using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a GridSplitter component.
    /// </summary>
    public partial class GridSplitter:ClearComponentBase
    {
        /// <summary>
        /// The direction of the splitter
        /// </summary>
        [Parameter]
        public SplitterDirection Direction { get; set; } = SplitterDirection.Vertical;

        /// <summary>
        /// Color of spinner.
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;


        private bool MouseDown = false;
        private bool MouseOver = false;

        private Grid? ParentGrid = null;
        private double LeftColumnWidth = 0;
        private double RightColumnWidth = 0;
        private double TopRowWidth = 0;
        private double BottomRowWidth = 0;
        private int SplitterColumn = 0;
        private int SplitterRow = 0;
        BrowserSizeService _browserSizeService = BrowserSizeService.GetInstance();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _browserSizeService.OnBrowserResize += BrowserResized;
        }

        private async Task BrowserResized(BrowserSizeInfo browserSizeInfo)
        {
            if (ParentGrid == null)
                return;
            await GetGridSizes();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                ParentGrid = Parent as Grid;
                if (ParentGrid == null)
                    throw new Exception("A GridSplitter's parent must a Grid component.");

                if (Direction == SplitterDirection.Vertical)
                {
                    if (ParentGrid.NumberOfColumns() != 3)
                        throw new Exception("A GridSplitter's parent must have 3 columns.");
                }
                else
                {
                    if (ParentGrid.NumberOfRows() != 3)
                        throw new Exception("A GridSplitter's parent must have 3 rows.");
                }

                foreach (var child in ParentGrid.Children)
                {
                    if (child is GridSplitter splitter && splitter == this)
                    {
                        if (Direction == SplitterDirection.Vertical)
                        {
                            if (child.Column != 1)
                                throw new Exception("A GridSplitter in vertical mode must be in column 1 of its parent Grid.");
                            SplitterColumn = child.Column;
                            var leftColumn = ParentGrid.GetColumnDefinition(SplitterColumn - 1);
                            if (!leftColumn.Contains("fr"))
                                throw new Exception("Column must contain a '*' definition with no min or max.");

                            var rightColumn = ParentGrid.GetColumnDefinition(SplitterColumn + 1);
                            if (!rightColumn.Contains("fr"))
                                throw new Exception("Column must contain a '*' definition with no min or max.");
                        }
                        else
                        {
                            if (child.Row != 1)
                                throw new Exception("A GridSplitter in horizontal mode must be in row 1 of its parent Grid.");
                            SplitterRow = child.Row;
                            var topRow = ParentGrid.GetRowDefinition(SplitterRow - 1);
                            if (!topRow.Contains("fr"))
                                throw new Exception("Row must contain a '*' definition with no min or max.");
                            var bottomRow = ParentGrid.GetRowDefinition(SplitterRow + 1);
                            if (!bottomRow.Contains("fr"))
                                throw new Exception("Row must contain a '*' definition with no min or max.");
                        }
                    }
                }
                await GetGridSizes();
            }
        }

        private async Task GetGridSizes()
        {
            if (ParentGrid == null)
                return; 

            if (Direction == SplitterDirection.Vertical)
            {
                var columnSizes = await ParentGrid.GetGridColumnSizes();
                LeftColumnWidth = columnSizes[SplitterColumn - 1];
                RightColumnWidth = columnSizes[SplitterColumn + 1];
            }
            else
            {
                var rowSizes = await ParentGrid.GetGridRowSizes();
                TopRowWidth = rowSizes[SplitterRow - 1];
                BottomRowWidth = rowSizes[SplitterRow + 1];
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; ";
            return css;
        }

        private Color GetColor()
        {
            if (Color == null)
                return Color.Custom("lightgrey");
            else
                return Color;
        }

        private async Task OnMouseDown(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("CaptureMouse", Id, 1);
            MouseDown = true;
        }
        private async Task OnMouseUp(MouseEventArgs e)
        {
            MouseDown = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", Id, 1);
        }

        private async Task OnMouseEnter(MouseEventArgs e)
        {
            MouseOver = true;
            Logger.AddLog($"OnMouseOver  MouseOver:{MouseOver}");

            if (Direction == SplitterDirection.Vertical)
                await JSRuntime.InvokeVoidAsync("Cursor.setCursor", "col-resize");
            else
                await JSRuntime.InvokeVoidAsync("Cursor.setCursor", "row-resize");

        }
        private async Task OnMouseLeave(MouseEventArgs e)
        {
            MouseOver = false;
            Logger.AddLog($"OnMouseOut  MouseOver:{MouseOver}");
            await JSRuntime.InvokeVoidAsync("Cursor.resetCursor");

        }

        private async Task OnMouseMove(MouseEventArgs e)
        {
            if (ParentGrid == null)
                return;

            if (MouseDown)
            {
                Logger.AddLog($"OnMouseMove  MouseOver:{MouseOver}");

                if (Direction == SplitterDirection.Vertical)
                {
                    if (e.MovementX == 0 || 
                        LeftColumnWidth + e.MovementX < 0 ||
                        RightColumnWidth - e.MovementX < 0)
                        return;
                    LeftColumnWidth += e.MovementX;
                    RightColumnWidth -= e.MovementX;
                    ParentGrid.AdjustColumns(LeftColumnWidth, RightColumnWidth);
                    Refresh(ParentGrid);
                }
                else
                {
                    if (e.MovementY == 0 ||
                        TopRowWidth + e.MovementY < 0 ||
                        BottomRowWidth - e.MovementY < 0)
                        return;
                    TopRowWidth += e.MovementY;
                    BottomRowWidth -= e.MovementY;
                    ParentGrid.AdjustRows(TopRowWidth, BottomRowWidth);
                    Refresh(ParentGrid);
                }
            }
        }
        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            _browserSizeService.OnBrowserResize -= BrowserResized;
        }
    }
}