namespace ClearBlazorTest
{
    public record AvatarDocsInfo:IDocsInfo
    {
        public string Name => "Avatar";
        public string Description => "";
        public (string, string) ApiLink => ("API", "AvatarApi");
        public (string, string) ExamplesLink => ("Examples", "Avatar");
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
