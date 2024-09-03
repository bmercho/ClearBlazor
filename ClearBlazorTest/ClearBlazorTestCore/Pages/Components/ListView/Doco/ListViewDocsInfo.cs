namespace ClearBlazorTest
{
    public record ListViewDocsInfo:IDocsInfo
    {
        public string Name => "ListView";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ListViewApi");
        public (string, string) ExamplesLink => ("Examples", "ListView");
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
