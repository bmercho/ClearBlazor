namespace ClearBlazorTest
{
    public record TextBlockDocsInfo:IDocsInfo
    {
        public string Name => "TextBlock";
        public string Description => "";
        public (string, string) ApiLink => ("API", "TextBlockApi");
        public (string, string) ExamplesLink => ("Examples", "TextBlock");
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
