namespace ClearBlazorTest
{
    public record ToolbarDocsInfo:IDocsInfo
    {
        public string Name => "Toolbar";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ToolbarApi");
        public (string, string) ExamplesLink => ("Examples", "Toolbar");
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
