namespace ClearBlazor
{
    /// <summary>
    /// Defines the border for a component
    /// </summary>
    public interface IBorder
    {
        /// <summary>
        /// The thickness of the border in pixels.
        /// Can be in the format of:
        ///     20 - all borders 20px
        ///     20,10 - top and bottom borders are 10, left and right borders are 10
        ///     20,10,30,40 - top is 20px, right is 10px, bottom is 30px and left is 40px
        /// </summary>
        public string? BorderThickness { get; set; }

        /// <summary>
        /// The color of the border
        /// </summary>
        public Color? BorderColour { get; set; }

        /// <summary>
        /// The style of the border. Solid,Dotted or Dashed.
        /// </summary>
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// The corner radius of the border
        /// Can be in the format of:
        ///     4 - all borders have 4px radius
        ///     4,8 - top and bottom borders have 4px radius, left and right borders have 8px radius
        ///     20,10,30,40 - top has 20px radius, right has 10px radius, bottom has 30px radius and left has 40px radius
        /// </summary>
        public string? CornerRadius { get; set; }
    }
}
