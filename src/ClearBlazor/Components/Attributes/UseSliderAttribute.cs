
namespace ClearBlazor
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class UseSliderAttribute : Attribute
    {
        public UseSliderAttribute(double min=0, double max=100, double step=1,
                                  bool showTickMarks=true, bool showTickMarkLabels=true)
        {
            Min = min;
            Max = max;
            Step = step;
            ShowTickMarks = showTickMarks;
            ShowTickMarkLabels = showTickMarkLabels;
        }

        public double Min { get; }
        public double Max { get; }
        public double Step { get; }
        public bool ShowTickMarkLabels { get; }
        public bool ShowTickMarks { get; }
    }
}
