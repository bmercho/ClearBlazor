namespace ClearBlazorTest
{
    public record ListBoxDocsInfo:IDocsInfo
    {
        public string Name => "ListBox";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ListBoxApi");
        public (string, string) ExamplesLink => ("Examples", "ListBox");
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
