namespace ClearBlazorTest
{
    public record BackgroundDocsInfo : IDocsInterfaceInfo
    {
        public string Name => "Alignment";

        public string Description => "The alignment of a component within its parent container.";

        public List<ApiFieldInfo> FieldApi => new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BackgroundColour", "<a href=\"/Colour\">Colour</a>", "The background colour of the component."),
        };
    }
}