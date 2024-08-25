namespace ClearBlazorTest
{
    public static class AlignmentApi
    {
        public static List<ApiFieldInfo> FieldApi = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Start", "Alignment", "Child elements are aligned to the top or left of the parent element's allocated layout space."),
            new ApiFieldInfo("End", "Alignment", "Child elements are aligned to the bottom or right of the parent element's allocated layout space."),
            new ApiFieldInfo("Center", "Alignment", "Child elements are aligned to the center of the parent element's allocated layout space."),
            new ApiFieldInfo("Stretch", "Alignment", "Child elements are stretched to fill the parent element's allocated layout space. Explicit Width and Height values take precedence.")
        };
    }
}