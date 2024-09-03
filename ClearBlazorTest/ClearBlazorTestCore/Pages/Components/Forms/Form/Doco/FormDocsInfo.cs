namespace ClearBlazorTest
{
    public record FormDocsInfo:IDocsInfo
    {
        public string Name => "Form";
        public string Description => "";
        public (string, string) ApiLink => ("API", "FormApi");
        public (string, string) ExamplesLink => ("Examples", "Form");
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
