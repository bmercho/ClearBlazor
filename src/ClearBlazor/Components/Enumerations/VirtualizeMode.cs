namespace ClearBlazor
{
    /// <summary>
    /// Indicates how a list of items is Virtualized.
    /// </summary>
    public enum VirtualizeMode
    {
        /// <summary>
        /// No virtualization.
        /// </summary>
        None,

        /// <summary>
        /// Virtualizes a list of items when the height of each item is the same and the total number of items is known.
        /// </summary>
        Virtualize,

        /// <summary>
        /// Virtualizes a list of items when the height of each item is not the same or the total number of 
        /// items is not known.
        /// </summary>
        InfiniteScroll,

        /// <summary>
        /// Virtualizes a list of items when the height of each item is not the same or the total number of 
        /// items is not known. In addition it places the first items from the list data at the bottom 
        /// of the scroll viewer and the thumb starts at the bottom. The scroll viewer can be scrolled up to
        /// get to the last items in the list data. Allows controls like 'WhatsApp' to be implemented. 
        /// </summary>
        InfiniteScrollReverse,

        /// <summary>
        /// Allows for paging through a list of items. 
        /// </summary>
        Pagination
    }
}
