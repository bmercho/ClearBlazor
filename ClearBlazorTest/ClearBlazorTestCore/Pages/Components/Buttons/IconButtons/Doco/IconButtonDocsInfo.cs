namespace ClearBlazorTest
{
    public record IconButtonDocsInfo:IDocsInfo
    {
        public string Name => "IconButton";
        public string Description => "";
        public (string, string) ApiLink => ("API", "IconButtonApi");
        public (string, string) ExamplesLink => ("Examples", "IconButton");
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
