namespace ClearBlazorTest
{
    public record DrawerDocsInfo:IDocsInfo
    {
        public string Name => "Drawer";
        public string Description => "";
        public (string, string) ApiLink => ("API", "DrawerApi");
        public (string, string) ExamplesLink => ("Examples", "Drawer");
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
