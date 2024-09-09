
namespace ClearBlazor
{
    public class SelectItem<TItem> : ListBoxItem<TItem>
    {
        protected override async Task OnInitializedAsync()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent),
                    "SelectItem must exist within a Select control");

            await base.OnInitializedAsync();

            Select<TItem>? parent = FindParent<Select<TItem>>(Parent);
            if (parent != null)
                parent.HandleChild(this);
        }
    }
}