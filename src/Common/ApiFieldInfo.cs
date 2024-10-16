namespace ClearBlazor.Common
{
    public class ApiFieldInfo : ListItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public ApiFieldInfo(string name, string type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }

    }

}
