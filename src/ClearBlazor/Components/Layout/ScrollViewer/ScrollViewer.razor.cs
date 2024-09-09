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
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        // Used to notify subscribers when any scrollbar is scrolled
        private static List<IObserver<bool>> _observers = new List<IObserver<bool>>();

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
                    //                    css += $"overflow-y: scroll;";
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
                    //                   css += $"overflow-x: scroll;";
                    css += $"overflow-x: scroll; scrollbar-gutter:stable; ";
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