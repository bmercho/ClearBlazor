namespace ClearBlazorTest
{
    public record TabsDocsInfo:IDocsInfo
    {
        public string Name => "Tabs";
        public string Description => "";
        public (string, string) ApiLink => ("API", "TabsApi");
        public (string, string) ExamplesLink => ("Examples", "Tabs");
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
