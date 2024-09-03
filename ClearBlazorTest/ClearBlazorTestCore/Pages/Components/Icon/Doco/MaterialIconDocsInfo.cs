namespace ClearBlazorTest
{
    public record MaterialIconDocsInfo:IDocsInfo
    {
        public string Name => "MaterialIcon";
        public string Description => "";
        public (string, string) ApiLink => ("API", "MaterialIconApi");
        public (string, string) ExamplesLink => ("Examples", "MaterialIcon");
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
