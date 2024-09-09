namespace ClearBlazor
{
    /// <summary>
    /// Used by WrapPanel to indicates the direction of wrapping
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// The items are laid out in rows from left to right and wraps to a new row.  
        /// </summary>
        Row,
        /// <summary>
        /// The items are laid out in rows from right to left and wraps to a new row.  
        /// </summary>
        RowReverse,
        /// <summary>
        /// The items are laid out in columns from top to bottom and wraps to a new column.  
        /// </summary>
        Column,
        /// <summary>
        /// The items are laid out in columns from bottom to top and wraps to a new column.  
        /// </summary>
        ColumnReverse
    }
}
