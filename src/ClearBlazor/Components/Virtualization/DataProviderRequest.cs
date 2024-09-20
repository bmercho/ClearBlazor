namespace ClearBlazor
{
    public sealed class DataProviderRequest
    {
        public DataProviderRequest(int startIndex, int count, CancellationToken cancellationToken)
        {
            StartIndex = startIndex;
            Count = count;
            CancellationToken = cancellationToken;
        }

        public int StartIndex { get; }
        public int Count { get; }
        public CancellationToken CancellationToken { get; }

    }
    public delegate Task<(int TotalNumItems, IEnumerable<T> Items)> 
                           DataProviderRequestDelegate<T>(DataProviderRequest request);
}
