namespace ClearBlazor
{
    public class TreeView<TItem> : TreeViewBase<TItem>  
             where TItem : TreeItem<TItem>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            _showHeader = false;
        }
    }
}