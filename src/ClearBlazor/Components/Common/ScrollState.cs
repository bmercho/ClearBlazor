namespace ClearBlazor
{
    public class ScrollState:IEquatable<ScrollState>
    {
        public double ScrollTop { get; set; }
        public double ScrollLeft { get; set; }
        public double ScrollHeight { get; set; }
        public double ScrollWidth { get; set; }
        public double ClientHeight { get; set; }
        public double ClientWidth { get; set; }

        public bool Equals(ScrollState? other)
        {
            if (other == null)
                return false;
            if (ScrollTop != other.ScrollTop ||
                ScrollLeft != other.ScrollLeft ||
                ScrollHeight != other.ScrollHeight ||
                ScrollWidth != other.ScrollWidth ||
                ClientHeight != other.ClientHeight ||
                ClientWidth != other.ClientWidth)
                return false;
            return true;
        }
    }
}
