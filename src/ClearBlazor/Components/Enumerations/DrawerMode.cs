namespace ClearBlazor
{
    /// <summary>
    /// Used by Drawer to indicate its mode.
    /// </summary>
    public enum DrawerMode
    {
        /// <summary>
        /// When the drawer is open it stays open until the Open parameter is set to false.
        /// </summary>
        Permanent,
        /// <summary>
        /// When the drawer is open it automatically closes when the overlay is clicked, 
        /// provided the overlay is showing.
        /// </summary>
        Temporary,
        /// <summary>
        /// The drawer closes if the Browser size is reduced to a DeviceSize of less than 
        /// medium and reopens when the DeviceSize is greater than or equal to medium.
        /// </summary>
        Responsive
    }
}
