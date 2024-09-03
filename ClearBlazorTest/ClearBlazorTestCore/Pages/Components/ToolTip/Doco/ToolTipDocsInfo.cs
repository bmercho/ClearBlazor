namespace ClearBlazorTest
{
    public record ToolTipDocsInfo:IDocsInfo
    {
        public string Name => "ToolTip";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ToolTipApi");
        public (string, string) ExamplesLink => ("Examples", "ToolTip");
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
