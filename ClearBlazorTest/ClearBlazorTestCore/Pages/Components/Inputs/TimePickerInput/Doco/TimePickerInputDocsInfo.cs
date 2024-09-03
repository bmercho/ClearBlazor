namespace ClearBlazorTest
{
    public record TimePickerInputDocsInfo:IDocsInfo
    {
        public string Name => "TimePickerInput";
        public string Description => "";
        public (string, string) ApiLink => ("API", "TimePickerInputApi");
        public (string, string) ExamplesLink => ("Examples", "TimePickerInput");
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
