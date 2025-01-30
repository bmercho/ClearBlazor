namespace ClearBlazor
{
    /// <summary>
    /// Defines a box shadow surrounding a component.
    /// Note that it is drawn outside of the component so there needs to space around the component to show the box shadow.
    /// Set a padding if there is no room to show it.
    /// </summary>
    public interface IBoxShadow
    {
        /// <summary>
        /// The level of box shadow:
        ///     0    - no box shadow
        ///     1-5  - greater shadow as number increases
        /// </summary>
        public int? BoxShadow { get; set; }

    }
}
