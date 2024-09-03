namespace ClearBlazorTest
{
    public record CardDocsInfo:IDocsInfo
    {
        public string Name => "Card";
        public string Description => "";
        public (string, string) ApiLink => ("API", "CardApi");
        public (string, string) ExamplesLink => ("Examples", "Card");
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
