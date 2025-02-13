using Microsoft.AspNetCore.Components;
using SkiaSharp;
using System.Data.Common;

namespace ClearBlazor
{
    /// <summary>
    /// A ScrollViewer provides a scrollable area that can contain child elements. 
    /// </summary>
    public partial class ScrollViewer:PanelBase
    {
        /// <summary>
        /// The horizontal scroll bar mode.
        /// </summary>
        [Parameter]
        public ScrollMode HorizontalScrollMode { get; set; } = ScrollMode.Disabled;

        /// <summary>
        /// The vertical scroll bar mode.
        /// </summary>
        [Parameter]
        public ScrollMode VerticalScrollMode { get; set; } = ScrollMode.Auto;

        /// <summary>
        /// Indicates when the vertical scrollbar gutter exists
        /// </summary>
        [Parameter]
        public ScrollbarGutter VerticalGutter { get; set; } = ScrollbarGutter.OnlyWhenOverflowed;

        /// <summary>
        /// Indicates when the horizontal scrollbar gutter exists
        /// </summary>
        [Parameter]
        public ScrollbarGutter HorizontalGutter { get; set; } = ScrollbarGutter.OnlyWhenOverflowed;

        /// <summary>
        /// Defines what happens when the boundary of a scrolling area is reached in the vertical direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour VerticalOverscrollBehaviour { get; set; } = OverscrollBehaviour.Auto;

        /// <summary>
        /// Defines what happens when the boundary of a scrolling area is reached in the horizontal direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour HorizontalOverscrollBehaviour { get; set; } = OverscrollBehaviour.Auto;

        /// <summary>
        /// Indicates if scrollbars will 'overlay' style scrollbars where the scrollbars are overlayed 
        /// on top of the content. Overlay scrollbars do not show the scrollbar background.
        /// </summary>
        [Parameter]
        public bool UseOverlayScrollbars { get; set; } = false;

        // Used to notify subscribers when any scrollbar is scrolled
        private static List<IObserver<bool>> _observers = new List<IObserver<bool>>();
        //private bool _doRender = true;
        private int _scrollbarWidth = 0;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _scrollbarWidth = ThemeManager.CurrentTheme.ScrollbarWidth;
            //_scrollbarBackgroundBoxShadowWidth = ThemeManager.CurrentTheme.ScrollbarBackgroundBoxShadowWidth;
            //_scrollbarCornerRadius = ThemeManager.CurrentTheme.ScrollbarCornerRadius;
            //_scrollbarBackgroundColor = ThemeManager.CurrentPalette.ScrollbarBackgroundColor;
            //_scrollbarThumbColor = ThemeManager.CurrentPalette.ScrollbarThumbColor;
            //_scrollbarBackgroundBoxShadowColor = ThemeManager.CurrentPalette.ScrollbarBackgroundBoxShadowColor;
            //_scrollbarOverlayThumbColor = ThemeManager.CurrentPalette.ScrollbarOverlayThumbColor;
        }
        public static IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        //protected override bool ShouldRender()
        //{
        //    return _doRender;
        //}

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        //    _doRender = true;
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            Size resultSize = new Size(0, 0);

            if (Children.Count > 1)
                throw new Exception("The ScrollViewer can only have a single child.");

            if (Children.Count == 1)
            {
                var child = Children[0] as PanelBase;
                if (child != null)
                {
                    child.Measure(availableSize);
                    resultSize.Width = child.DesiredSize.Width;
                    resultSize.Height = child.DesiredSize.Height;
                }
            }

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize,
                                                double offsetHeight,
                                                double offsetWidth)
        {
            if (Children.Count > 1)
                throw new Exception("The ScrollViewer can only have a single child.");

            Rect boundRect = new Rect(finalSize);

            if (Children.Count == 1)
            {
                var panel = Children[0] as PanelBase;
                if (panel != null)
                    panel.Arrange(boundRect);
            }

            return finalSize;
        }

        protected override void DoPaint(SKCanvas canvas)
        {
            base.DoPaint(canvas);
        }

        private void OnScroll()
        {
            foreach (var observer in _observers)
                observer.OnNext(true);
        }

        private bool ShowVerticalScrollBar()
        {
            if (VerticalScrollMode == ScrollMode.Disabled)
                return false;

            if (VerticalScrollMode == ScrollMode.Enabled)
                return true;

            if (Children == null || Children.Count != 1 || Children[0].Children.Count != 1 ||
                Children[0].Children[0].Children.Count != 1)
                return false;

            var contentChild = Children[0]?.Children[0]?.Children[0];
            if (contentChild == null)
                return false;
            if (VerticalScrollMode == ScrollMode.Auto && contentChild.ContentSize.Height > ActualHeight)
                return true;

            return false;
        }

        private bool ShowHorizontalScrollBar()
        {
            if (HorizontalScrollMode == ScrollMode.Disabled)
                return false;

            if (HorizontalScrollMode == ScrollMode.Enabled)
                return true;

            if (Children == null || Children.Count != 1)
                return false;

            if (HorizontalScrollMode == ScrollMode.Auto && Children[0].ActualWidth > ActualWidth)
                return true;

            return false;
        }

        private int GetContentRow()
        {
            if (UseOverlayScrollbars)
                return 1;
            var area = GetContentArea();
            return GetContentArea().Row;

        }

        private int GetContentColumn()
        {
            if (UseOverlayScrollbars)
                return 1;
            return GetContentArea().Column;

        }

        private int GetContentRowSpan()
        {
            if (UseOverlayScrollbars)
                return 3;
            return GetContentArea().RowSpan;
        }

        private int GetContentColumnSpan()
        {
            if (UseOverlayScrollbars)
                return 3;
            return GetContentArea().ColumnSpan;
        }

        private (int Row, int Column, int RowSpan, int ColumnSpan) GetContentArea()
        {
            int row = 0;
            int column = 0;
            int rowSpan = 1;
            int columnSpan = 1;

            string css = string.Empty;

            if (VerticalScrollMode != ScrollMode.Disabled)
            {
                if (VerticalGutter == ScrollbarGutter.Always)
                {
                    column = 0;
                    columnSpan = 2;
                }
                else if (VerticalGutter == ScrollbarGutter.AlwaysBothEdges)
                {
                    column = 1;
                    columnSpan = 1;
                }
                else if (ShowVerticalScrollBar())
                {
                    column = 0;
                    columnSpan = 2;
                }
                else
                {
                    column = 0;
                    columnSpan = 3;
                }
            }
            else
            {
                column = 0;
                columnSpan = 3;
            }

            if (HorizontalScrollMode != ScrollMode.Disabled)
            {
                if (HorizontalGutter == ScrollbarGutter.Always)
                {
                    row = 0;
                    rowSpan = 2;
                }
                else if (HorizontalGutter == ScrollbarGutter.AlwaysBothEdges)
                {
                    row = 1;
                    rowSpan = 1;
                }
                else if (ShowHorizontalScrollBar())
                {
                    row = 0;
                    rowSpan = 2;
                }
                else
                {
                    row = 0;
                    rowSpan = 3;
                }
            }
            else
            {
                row = 0;
                rowSpan = 3;
            }
            return (row, column, rowSpan, columnSpan);
        }

        private async Task HandleClick(ComponentMouseEventArgs e)
        {
            await Task.CompletedTask;
        }

        private async Task HandlePointerMove(ComponentPointerEventArgs e)
        {
            await Task.CompletedTask;
        }

        private async Task HandlePointerDown(ComponentPointerEventArgs e)
        {
            await Task.CompletedTask;
        }

        private async Task HandlePointerUp(ComponentPointerEventArgs e)
        {
            await Task.CompletedTask;
        }

        private async Task HandlePointerEnter(ComponentPointerEventArgs e)
        {
            await Task.CompletedTask;
        }
        private async Task HandlePointerLeave(ComponentPointerEventArgs e)
        {
            await Task.CompletedTask;
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<bool>> _observers;
            private IObserver<bool> _observer;

            public Unsubscriber(List<IObserver<bool>> observers, IObserver<bool> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }
    }
}