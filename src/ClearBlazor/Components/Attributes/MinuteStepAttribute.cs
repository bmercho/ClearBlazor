
namespace ClearBlazor
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class MinuteStepAttribute : Attribute
    {
        public MinuteStepAttribute(MinuteStep minuteStep)
        {
            MinuteStep = minuteStep;
        }

        public MinuteStep MinuteStep { get; }
    }
}
