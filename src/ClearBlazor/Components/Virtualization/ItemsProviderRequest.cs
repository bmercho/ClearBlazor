namespace ClearBlazor
{
    public sealed class ItemsProviderRequest
    {
        public ItemsProviderRequest(int startIndex, int count, CancellationToken cancellationToken)
        {
            StartIndex = startIndex;
            Count = count;
            CancellationToken = cancellationToken;
        }

        public int StartIndex { get; }
        public int Count { get; }
        public CancellationToken CancellationToken { get; }

    }
    public delegate Task<IEnumerable<int>> ItemsProviderRequestDelegate(ItemsProviderRequest request);

    public delegate Task<IEnumerable<T>> ItemsProviderRequestDelegate<T>(ItemsProviderRequest request);
}
