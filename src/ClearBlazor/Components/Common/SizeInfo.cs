namespace ClearBlazor
{
    public class SizeInfo : IEquatable<SizeInfo>
    {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double ParentX { get; set; }
        public double ParentY { get; set; }
        public double ParentWidth { get; set; }
        public double ParentHeight { get; set; }
        public double ElementX { get; set; }
        public double ElementY { get; set; }
        public double ElementWidth { get; set; }
        public double ElementHeight { get; set; }

        public bool Equals(SizeInfo? other)
        {
            if (other == null)
                return false;

            if (WindowWidth == other.WindowWidth &&
                WindowHeight == other.WindowHeight &&
                ParentX == other.ParentX &&
                ParentY == other.ParentY &&
                ParentWidth == other.ParentWidth &&
                ParentHeight == other.ParentHeight &&
                ElementX == other.ElementX &&
                ElementY == other.ElementY &&
                ElementWidth == other.ElementWidth &&
                ElementHeight == other.ElementHeight)
                return true;

            return false;
        }
    }
}
