namespace ClearBlazorTest
{
    public record ButtonDocsInfo:IDocsInfo
    {
        public string Name => "Button";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ButtonApi");
        public (string, string) ExamplesLink => ("Examples", "Button");
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
