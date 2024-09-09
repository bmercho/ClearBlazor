namespace ClearBlazor
{
    public class ElementSizeInfo : IEquatable<ElementSizeInfo>
    {
        public double ElementX { get; set; }
        public double ElementY { get; set; }
        public double ElementWidth { get; set; }
        public double ElementHeight { get; set; }

        public bool Equals(ElementSizeInfo? other)
        {
            if (other == null)
                return false;

            if (ElementX == other.ElementX &&
                ElementY == other.ElementY &&
                ElementWidth == other.ElementWidth &&
                ElementHeight == other.ElementHeight)
                return true;

            return false;
        }
    }
}
