namespace ClearBlazor
{
    public interface IBorder:IBoxShadow
    {
        public string? BorderThickness { get; set; }

        public Color? BorderColour { get; set; }

        public string? CornerRadius { get; set; }
    }
}
