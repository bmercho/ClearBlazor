namespace ClearBlazor
{
    public interface IBorder
    {
        public string? BorderThickness { get; set; }

        public Color? BorderColour { get; set; }

        public BorderStyle? BorderStyle { get; set; }

        public string? CornerRadius { get; set; }
    }
}
