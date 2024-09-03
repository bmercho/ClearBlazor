namespace ClearBlazorTest
{
    public interface IDocsInfo
    {
        public string Name { get; }
        public string Description { get; }
        public (string, string) ApiLink { get; }
        public (string, string) ExamplesLink { get; }
        public (string, string) InheritsLink { get; }
        public List<(string, string)> ImplementsLinks { get; }
        public List<ApiComponentInfo> ParameterApi { get; }
        public List<ApiComponentInfo> PropertyApi { get; }
        public List<ApiComponentInfo> MethodApi { get; }
    }
}