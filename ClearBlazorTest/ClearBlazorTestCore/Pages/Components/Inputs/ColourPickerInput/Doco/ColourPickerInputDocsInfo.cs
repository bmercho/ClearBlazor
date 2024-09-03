namespace ClearBlazorTest
{
    public record ColourPickerInputDocsInfo:IDocsInfo
    {
        public string Name => "ColourPickerInput";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ColourPickerInputApi");
        public (string, string) ExamplesLink => ("Examples", "ColourPickerInput");
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
