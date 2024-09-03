namespace ClearBlazorTest
{
    public record ToggleIconButtonDocsInfo:IDocsInfo
    {
        public string Name => "ToggleIconButton";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ToggleIconButtonApi");
        public (string, string) ExamplesLink => ("Examples", "ToggleIconButton");
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
