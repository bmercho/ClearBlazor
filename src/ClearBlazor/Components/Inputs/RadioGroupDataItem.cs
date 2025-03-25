namespace ClearBlazor
{
    public class RadioGroupDataItem<TItem>
    {
        public string Name { get; set; } = string.Empty;
        public TItem? Value { get; set; } = default;

        public RadioGroupDataItem()
        {
        }

        public RadioGroupDataItem(string name, TItem value)
        {
            Name = name;
            Value = value;
        }
    }

}
