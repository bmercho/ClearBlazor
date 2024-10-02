namespace ClearBlazor
{   
    /// <summary>
    /// Defines a interface for the list type components, NonVirtualizedList, VirtualizedList or InfiniteScrollerList
    /// </summary>
    public interface IList<TItem>
    {
        Task<List<TItem>> GetSelections(int firstIndex, int secondIndex);
    }
}
