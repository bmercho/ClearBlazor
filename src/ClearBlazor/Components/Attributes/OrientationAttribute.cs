
namespace ClearBlazor
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class OrientationAttribute : Attribute
    {
        public OrientationAttribute(Orientation orientation)
        {
            Orientation = orientation;
        }

        public Orientation Orientation { get; }
    }
}
