namespace ClearBlazorTest
{
    public record SelectDocsInfo:IDocsInfo
    {
        public string Name => "Select";
        public string Description => "";
        public (string, string) ApiLink => ("API", "SelectApi");
        public (string, string) ExamplesLink => ("Examples", "Select");
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
