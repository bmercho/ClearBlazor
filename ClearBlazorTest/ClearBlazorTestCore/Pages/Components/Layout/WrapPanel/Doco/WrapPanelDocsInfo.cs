namespace ClearBlazorTest
{
    public record WrapPanelDocsInfo:IDocsInfo
    {
        public string Name => "WrapPanel";
        public string Description => "";
        public (string, string) ApiLink => ("API", "WrapPanelApi");
        public (string, string) ExamplesLink => ("Examples", "WrapPanel");
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
