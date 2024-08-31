using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ScrollViewer:ClearComponentBase,IContent
    {
        [Parameter]
        public ScrollMode HorizontalScrollMode { get; set; } = ScrollMode.Disabled;

        [Parameter]
        public ScrollMode VerticalScrollMode { get; set; } = ScrollMode.Auto;

        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;


        private static List<IObserver<bool>> observers = new List<IObserver<bool>>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            IsScroller = true;
        }

        protected override string UpdateStyle(string css)
        {
            css += "display:grid; ";
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

        protected override string UpdateChildStyle(ClearComponentBase child, string css)
        {
            //switch (VerticalScrollMode)
            //{
            //    case ScrollMode.Disabled:
            //        css += $"overflow-y: hidden; ";
            //        break;
            //    case ScrollMode.Auto:
            //        css += $"overflow-y: auto;";
            //        break;
            //    case ScrollMode.Enabled:
            //        //css += $"overflow-y: scroll;";
            //                            css += $"overflow-y: scroll; scrollbar-gutter:stable;";
            //        break;
            //}

            //switch (HorizontalScrollMode)
            //{
            //    case ScrollMode.Disabled:
            //        css += $"overflow-x: hidden; ";
            //        break;
            //    case ScrollMode.Auto:
            //        css += $"overflow-x: auto; ";
            //        break;
            //    case ScrollMode.Enabled:
            //        //css += $"overflow-x: scroll;";
            //                            css += $"overflow-x: scroll; scrollbar-gutter:stable;";
            //        break;
            //}

            //var margin = Thickness.Parse(child.Margin);
            //var padding = Thickness.Parse(child.Padding);
            //if (!css.Contains("width:"))
            //    css += $"width: Calc(100% - {margin.HorizontalThickness}px - {padding.HorizontalThickness}px);";
            //if (!css.Contains("height:"))
            //    css += $"height: Calc(100% - {margin.VerticalThickness}px - {padding.VerticalThickness}px);";

            return css;
        }

        public void OnScroll()
        {
            foreach (var observer in observers)
                observer.OnNext(true);
        }

        public static IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
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