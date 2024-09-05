
namespace ClearBlazor
{
    public class ListDataItem<TItem>
    {
        public string Name { get; set; } = string.Empty;
        public TItem? Value { get; set; } = default;
        public string? Icon { get; set; } = null;
        public string? Avatar { get; set; } = null;

        public ListDataItem()
        {
        }

        public ListDataItem(string name, TItem value, string? icon, string? avatar)
        {
            Name = name;
            Value = value;
            Icon = icon;
            Avatar = avatar;
        }
        public ListDataItem(string name, TItem value, string? icon)
        {
            Name = name;
            Value = value;
            Icon = icon;
        }
        public ListDataItem(string name, TItem value)
        {
            Name = name;
            Value = value;
        }
    }
}
