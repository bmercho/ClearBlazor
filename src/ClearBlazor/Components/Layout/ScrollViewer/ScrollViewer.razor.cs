using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A ScrollViewer provides a scrollable area that can contain child elements. 
    /// </summary>
    public partial class ScrollViewer:ClearComponentBase
    {
        /// <summary>
        /// The horizontal scroll mode.
        /// </summary>
        [Parameter]
        public ScrollMode HorizontalScrollMode { get; set; } = ScrollMode.Disabled;

        /// <summary>
        /// The horizontal scroll mode.
        /// </summary>
        [Parameter]
        public ScrollMode VerticalScrollMode { get; set; } = ScrollMode.Auto;


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
        /// Indicates when the scrollbar gutter exists
        /// </summary>
        [Parameter]
        public ScrollbarGutter ScrollBarGutter { get; set; } = ScrollbarGutter.OnlyWhenOverflowed;

        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        // Used to notify subscribers when any scrollbar is scrolled
        private static List<IObserver<bool>> _observers = new List<IObserver<bool>>();
        private bool _doRender = true;

        public static IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            IsScroller = true;
        }

        protected override bool ShouldRender()
        {
            return _doRender;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine("ScrollViewer:Render");
            base.OnAfterRender(firstRender);
            _doRender = true;
        }

        protected override string UpdateStyle(string css)
        {
            switch (VerticalScrollMode)
            {
                case ScrollMode.Disabled:
                    css += $"overflow-y: hidden; ";
                    break;
                case ScrollMode.Auto:
                    css += $"overflow-y: auto; ";
                    break;
                case ScrollMode.Enabled:
                    css += $"overflow-y: scroll;  scrollbar-gutter:stable; ";
                    break;
            }

            switch (HorizontalScrollMode)
            {
                case ScrollMode.Disabled:
                    css += $"overflow-x: hidden; ";
                    break;
                case ScrollMode.Auto:
                    css += $"overflow-x: auto;";
                    break;
                case ScrollMode.Enabled:
                    css += $"overflow-x: scroll; scrollbar-gutter:stable; ";
                    break;
            }

            switch (ScrollBarGutter)
            {
                case ScrollbarGutter.OnlyWhenOverflowed:
                    css += $"scrollbar-gutter:auto; ";
                    break;
                case ScrollbarGutter.Always:
                    css += $"scrollbar-gutter:stable; ";
                    break;
                case ScrollbarGutter.AlwaysBothEdges:
                    css += $"scrollbar-gutter:stable both-edges; ";
                    break;
            }

            switch (VerticalOverscrollBehaviour)
            {
                case OverscrollBehaviour.Auto:
                    css += "overscroll-behavior-y:auto; ";
                    break;
                case OverscrollBehaviour.Contain:
                    css += "overscroll-behavior-y:contain; ";
                    break;
                case OverscrollBehaviour.None:
                    css += "overscroll-behavior-y:none; ";
                    break;
            }

            switch (HorizontalOverscrollBehaviour)
            {
                case OverscrollBehaviour.Auto:
                    css += "overscroll-behavior-x:auto; ";
                    break;
                case OverscrollBehaviour.Contain:
                    css += "overscroll-behavior-x:contain; ";
                    break;
                case OverscrollBehaviour.None:
                    css += "overscroll-behavior-x:none; ";
                    break;
            }
            var margin = Thickness.Parse(Margin);
            var padding = Thickness.Parse(Padding);
            if (!css.Contains("width:"))
                css += $"width: Calc(100% - {margin.HorizontalThickness}px) - {padding.HorizontalThickness}px); ";
            if (!css.Contains("height:"))
                css += $"height: Calc(100% - {margin.VerticalThickness}px) - {padding.VerticalThickness}px); ";

            return css;
        }

        private void OnScroll()
        {
            foreach (var observer in _observers)
                observer.OnNext(true);
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