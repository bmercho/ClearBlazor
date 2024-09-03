namespace ClearBlazorTest
{
    public record ColourDocsInfo : IDocsInterfaceInfo
    {
        public string Name => "IBackground";

        public string Description => "Defines the background of a component.";

        public List<ApiFieldInfo> FieldApi => new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BackgroundColour", "<a href=\"/Colour\">Colour</a>", "The background colour of the component."),
        };
    }
}