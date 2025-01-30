namespace ClearBlazor
{
    public struct Rect
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }

        public Rect(double left, double top, double width, double height)
        {
            Width = width;
            Height = height;
            Left = left;
            Top = top;
        }
        public Rect(Size size)
        {
            Width = size.Width;
            Height = size.Height;
            Left = 0;
            Top = 0;
        }
    }
}
