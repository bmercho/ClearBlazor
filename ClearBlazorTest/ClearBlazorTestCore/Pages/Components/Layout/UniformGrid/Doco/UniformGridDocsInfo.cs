namespace ClearBlazorTest
{
    public record UniformGridDocsInfo:IDocsInfo
    {
        public string Name => "UniformGrid";
        public string Description => "";
        public (string, string) ApiLink => ("API", "UniformGridApi");
        public (string, string) ExamplesLink => ("Examples", "UniformGrid");
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
