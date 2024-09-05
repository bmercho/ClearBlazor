namespace ClearBlazor
{
    /// <summary>
    /// Defines the background gradient.
    /// Two backgrounds can be defined
    /// </summary>

    public interface IBackgroundGradient
    {
        public string? BackgroundGradient1 { get; set; }
        public string? BackgroundGradient2 { get; set; }
    }
}
