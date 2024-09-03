namespace ClearBlazorTest
{
    public record AutoFormDocsInfo:IDocsInfo
    {
        public string Name => "AutoForm";
        public string Description => "";
        public (string, string) ApiLink => ("API", "AutoFormApi");
        public (string, string) ExamplesLink => ("Examples", "AutoForm");
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
