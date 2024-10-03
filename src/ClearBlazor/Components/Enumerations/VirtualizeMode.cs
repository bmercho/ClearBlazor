﻿namespace ClearBlazor
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
        /// Allows for paging through a list of items. 
        /// </summary>
        Pagination
    }
}
