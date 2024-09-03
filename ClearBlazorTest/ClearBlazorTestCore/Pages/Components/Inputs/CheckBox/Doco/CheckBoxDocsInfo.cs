namespace ClearBlazorTest
{
    public record CheckBoxDocsInfo:IDocsInfo
    {
        public string Name => "CheckBox";
        public string Description => "";
        public (string, string) ApiLink => ("API", "CheckBoxApi");
        public (string, string) ExamplesLink => ("Examples", "CheckBox");
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
