namespace ClearBlazorTest
{
    public record ButtonGroupDocsInfo:IDocsInfo
    {
        public string Name => "ButtonGroup";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ButtonGroupApi");
        public (string, string) ExamplesLink => ("Examples", "ButtonGroup");
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
