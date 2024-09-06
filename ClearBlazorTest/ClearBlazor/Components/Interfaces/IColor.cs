namespace ClearBlazor
{
    /// <summary>
    /// The foreground of the component, usually used for text color.
    /// If Color is null then an attempt to find is made by getting the contrasting color
    /// to the background color of this component or a descendent component (if the component background is null)
    /// </summary>
    public interface IColor
    {
        /// <summary>
        /// The foreground color.
        /// </summary>
        public Color? Color { get; set; }
    }
}
