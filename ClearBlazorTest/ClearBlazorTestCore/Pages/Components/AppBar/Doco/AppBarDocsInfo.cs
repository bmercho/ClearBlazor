namespace ClearBlazorTest
{
    public record AppBarDocsInfo:IDocsInfo
    {
        public string Name => "AppBar";
        public string Description => "Represents a bar used to display actions, branding, navigation and screen titles.\r";
        public (string, string) ApiLink => ("API", "AppBarApi");
        public (string, string) ExamplesLink => ("Examples", "AppBar");
        public (string, string) InheritsLink => ("DockPanel", "DockPanelApi");
        public List<(string, string)> ImplementsLinks => new()
        {
        ("IColour", "IColour"),
        };
        public List<ApiComponentInfo> ParameterApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Colour", "Color?", "null", "The foreground color of the AppBar\r"),
        };
        public List<ApiComponentInfo> PropertyApi => new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> MethodApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("DoSomething()", "void", "", "Do some thing\r"),
        };
    }
}
