namespace ClearBlazorTest
{
    public record DockPanelDocsInfo:IDocsInfo
    {
        public string Name => "DockPanel";
        public string Description => "";
        public (string, string) ApiLink => ("API", "DockPanelApi");
        public (string, string) ExamplesLink => ("Examples", "DockPanel");
        public (string, string) InheritsLink => ("", "Api");
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
    }
}
