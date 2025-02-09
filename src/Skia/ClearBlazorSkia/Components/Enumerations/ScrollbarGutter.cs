namespace ClearBlazor
{
    /// <summary>
    /// Indicates when the scrollbar gutters are shown.
    /// If the scrollbar is set to an Overlay scrollbar then no gutters are shown.
    /// </summary>
    public enum ScrollbarGutter
    {
        /// <summary>
        /// The gutter is only shown when the scrollbar is also shown,
        /// </summary>
        OnlyWhenOverflowed,
        /// <summary>
        /// The gutter is always shown if the scrollbar mode is Auto or Scroll but not disabled
        /// </summary>
        Always,
        /// <summary>
        /// A gutter is also shown on the other side to match the normal gutter.
        /// Has same conditions for showing as Always.
        /// </summary>
        AlwaysBothEdges
    }
}
