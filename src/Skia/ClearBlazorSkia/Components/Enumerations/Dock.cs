namespace ClearBlazor
{
    /// <summary>
    /// Used by DockPanel to indicate which side a child is docked to. 
    /// If a child does not have Dock specified it uses the remaining avialable space.
    /// </summary>
    public enum Dock
    {
        /// <summary>
        /// Dock to the left side.
        /// </summary>
        Left,
        /// <summary>
        /// Dock to the top side.
        /// </summary>
        Top,
        /// <summary>
        /// Dock to the right side.
        /// </summary>
        Right,
        /// <summary>
        /// Dock to the bottom side.
        /// </summary>
        Bottom
    }
}
