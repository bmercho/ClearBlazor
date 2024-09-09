namespace ClearBlazor
{
    public class ItemsProviderRequest
    {
        public ItemsProviderRequest(int startIndex)
        {
            StartIndex = startIndex;
        }

        public int StartIndex { get; }
    }

    public delegate Task<IEnumerable<T>> ItemsProviderRequestDelegate<T>(ItemsProviderRequest request);
}
