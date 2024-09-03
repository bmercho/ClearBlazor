namespace ClearBlazorTest
{
    public record DatePickerInputDocsInfo:IDocsInfo
    {
        public string Name => "DatePickerInput";
        public string Description => "";
        public (string, string) ApiLink => ("API", "DatePickerInputApi");
        public (string, string) ExamplesLink => ("Examples", "DatePickerInput");
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
