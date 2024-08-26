namespace ClearBlazorTest
{
    public record AlignmentDocsInfo:IDocsEnumInfo
    {
        public string Name => "IBackground";

        public string Description => "Defines the background of a component.";

        public List<ApiFieldInfo> FieldApi => new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Start", "Alignment", "Aligns the component to the top or left within the parent's container."),
            new ApiFieldInfo("End", "Alignment", "Aligns the component to the bottom or right within the parent's container."),
            new ApiFieldInfo("Center", "Alignment", "Aligns the component to the center within the parent's container" +
                             " in the direction of the alignment."),
            new ApiFieldInfo("Stretch", "Alignment", "Aligns the component to fill the parent's container in the direction" +
                             " of the alignment. Explicit Width and Height values take precedence.")
        };
    }
}