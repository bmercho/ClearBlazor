namespace ClearBlazorTest
{
    public record SliderDocsInfo:IDocsInfo
    {
        public string Name => "Slider";
        public string Description => "";
        public (string, string) ApiLink => ("API", "SliderApi");
        public (string, string) ExamplesLink => ("Examples", "Slider");
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
