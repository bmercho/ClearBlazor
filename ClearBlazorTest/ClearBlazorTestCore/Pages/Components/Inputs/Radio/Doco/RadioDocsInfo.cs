namespace ClearBlazorTest
{
    public record RadioDocsInfo:IDocsInfo
    {
        public string Name => "Radio";
        public string Description => "";
        public (string, string) ApiLink => ("API", "RadioApi");
        public (string, string) ExamplesLink => ("Examples", "Radio");
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
