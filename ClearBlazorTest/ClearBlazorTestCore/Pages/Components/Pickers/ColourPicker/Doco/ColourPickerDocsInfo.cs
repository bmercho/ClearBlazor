namespace ClearBlazorTest
{
    public record ColourPickerDocsInfo:IDocsInfo
    {
        public string Name => "ColourPicker";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ColourPickerApi");
        public (string, string) ExamplesLink => ("Examples", "ColourPicker");
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
