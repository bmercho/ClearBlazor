namespace ClearBlazorTest
{
    public record NumericInputDocsInfo:IDocsInfo
    {
        public string Name => "NumericInput";
        public string Description => "";
        public (string, string) ApiLink => ("API", "NumericInputApi");
        public (string, string) ExamplesLink => ("Examples", "NumericInput");
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
