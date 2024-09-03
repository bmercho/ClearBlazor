namespace ClearBlazorTest
{
    public record StackPanelDocsInfo:IDocsInfo
    {
        public string Name => "StackPanel";
        public string Description => "";
        public (string, string) ApiLink => ("API", "StackPanelApi");
        public (string, string) ExamplesLink => ("Examples", "StackPanel");
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
