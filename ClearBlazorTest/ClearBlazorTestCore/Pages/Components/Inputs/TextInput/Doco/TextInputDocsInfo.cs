namespace ClearBlazorTest
{
    public record TextInputDocsInfo:IDocsInfo
    {
        public string Name => "TextInput";
        public string Description => "";
        public (string, string) ApiLink => ("API", "TextInputApi");
        public (string, string) ExamplesLink => ("Examples", "TextInput");
        public (string, string) InheritsLink => ("", "");
        public List<(string, string)> ImplementsLinks => new()
        {
        };
        public List<ApiComponentInfo> ParameterApi => new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> PropertyApi => new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> MethodApi => new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> EventApi => new List<ApiComponentInfo>
        {
        };
    }
}
