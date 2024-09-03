namespace ClearBlazorTest
{
    public record ImageDocsInfo:IDocsInfo
    {
        public string Name => "Image";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ImageApi");
        public (string, string) ExamplesLink => ("Examples", "Image");
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
