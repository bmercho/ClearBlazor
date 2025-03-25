namespace ClearBlazor
{
    /// <summary>
    /// The scroll bar mode
    /// </summary>
    public enum ScrollMode
    {
        /// <summary>
        /// No scroll bar is shown
        /// </summary>
        Disabled, 
        /// <summary>
        /// Scroll bar will always be shown.
        /// </summary>
        Enabled, 
        /// <summary>
        /// Scroll bar will be shown only when container has overflowed.
        /// </summary>
        Auto
    }
}
