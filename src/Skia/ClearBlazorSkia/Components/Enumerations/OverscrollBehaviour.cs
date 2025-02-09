namespace ClearBlazor
{
    /// <summary>
    /// Defines what happens when the boundary of a scrolling area is reached.
    /// </summary>
    public enum OverscrollBehaviour
    {
        /// <summary>
        /// The default scroll overflow behavior occurs as normal.
        /// ie a parent scrolling area may scroll
        /// </summary>
        Auto,
        /// <summary>
        /// Default scroll overflow behavior (e.g., "bounce" effects) 
        /// is observed inside the element where this value is set. 
        /// However, no scroll chaining occurs on neighboring scrolling areas. 
        /// The underlying elements will not scroll. 
        /// The contain value disables native browser navigation, 
        /// including the vertical pull-to-refresh gesture and horizontal swipe navigation.
        /// </summary>
        Contain,
        /// <summary>
        /// No scroll chaining occurs to neighboring scrolling areas, 
        /// and default scroll overflow behavior is prevented.
        /// </summary>
        None
    }
}
