namespace ClearBlazorTest
{
    public record SwitchDocsInfo:IDocsInfo
    {
        public string Name => "Switch";
        public string Description => "";
        public (string, string) ApiLink => ("API", "SwitchApi");
        public (string, string) ExamplesLink => ("Examples", "Switch");
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
