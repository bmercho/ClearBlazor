namespace ClearBlazorTest
{
    public record DatePickerDocsInfo:IDocsInfo
    {
        public string Name => "DatePicker";
        public string Description => "";
        public (string, string) ApiLink => ("API", "DatePickerApi");
        public (string, string) ExamplesLink => ("Examples", "DatePicker");
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
