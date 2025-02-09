namespace ClearBlazor.Common
{
    public interface IComponentDocsInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public (string, string) ApiLink { get; set; }

        public (string, string) ExamplesLink { get; set; }

        public (string, string) InheritsLink { get; set; }

        public List<(string, string)> ImplementsLinks { get; set; }

        public List<ApiComponentInfo> ParameterApi { get; set; }

        public List<ApiComponentInfo> MethodApi { get; set; }
    }
}
