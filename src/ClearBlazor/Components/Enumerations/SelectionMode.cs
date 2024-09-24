namespace ClearBlazor
{
    // Defines the selection mode for the component.
    public enum SelectionMode
    {
        /// <summary>
        /// Not selectable
        /// </summary>
        None, 

        /// <summary>
        /// Can only select a single item
        /// </summary>
        Single,

        /// <summary>
        /// Can select multiple rows by clicking rows.
        /// Click to select and then click to unselect
        /// </summary>
        SimpleMulti,

        /// <summary>
        /// Can select multiple rows clicking rows using the standard
        /// windows selection method, using click, ctl click and ctl shift click.
        /// </summary>
        Multi
    }
}
