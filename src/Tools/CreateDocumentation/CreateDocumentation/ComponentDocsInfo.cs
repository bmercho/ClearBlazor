namespace ClearBlazor.Common
{
    public record ComponentDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public (string, string) ApiLink { get; set; }

        public (string, string) ExamplesLink { get; set; }

        public (string, string) InheritsLink { get; set; }

        public List<(string, string)> ImplementsLinks { get; set; } = new();

        public List<ApiComponentInfo> ParameterApi { get; set; } = new();

        public List<ApiComponentInfo> MethodApi { get; set; } = new();
    }
}
