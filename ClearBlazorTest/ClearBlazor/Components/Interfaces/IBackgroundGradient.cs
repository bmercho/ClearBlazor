namespace ClearBlazor
{
    /// <summary>
    /// Defines the background gradient.
    /// Two backgrounds can be defined each in its own div allowing complex gradients to be created
    /// </summary>

    public interface IBackgroundGradient
    {
        /// <summary>
        /// The first background gradient on the main div of the component 
        /// </summary>
        public string? BackgroundGradient1 { get; set; }
        /// <summary>
        /// The second background gradient on a div inside the main div of the component 
        /// </summary>
        public string? BackgroundGradient2 { get; set; }
    }
}
