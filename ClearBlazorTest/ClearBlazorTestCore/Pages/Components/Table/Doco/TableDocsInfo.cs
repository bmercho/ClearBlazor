namespace ClearBlazorTest
{
    public record TableDocsInfo:IDocsInfo
    {
        public string Name => "Table";
        public string Description => "";
        public (string, string) ApiLink => ("API", "TableApi");
        public (string, string) ExamplesLink => ("Examples", "Table");
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
