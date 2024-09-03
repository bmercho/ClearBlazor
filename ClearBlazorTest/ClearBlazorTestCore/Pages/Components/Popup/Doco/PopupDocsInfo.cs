namespace ClearBlazorTest
{
    public record PopupDocsInfo:IDocsInfo
    {
        public string Name => "Popup";
        public string Description => "";
        public (string, string) ApiLink => ("API", "PopupApi");
        public (string, string) ExamplesLink => ("Examples", "Popup");
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
