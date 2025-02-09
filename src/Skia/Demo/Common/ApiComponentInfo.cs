using ClearBlazor;

namespace ClearBlazor.Common
{
    public class ApiComponentInfo:ListItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
        public ApiComponentInfo(string name, string type, string def, string description)
        { 
            Name = name;
            Type = type;
            Default = def;
            Description = description;
        }

    }
}
