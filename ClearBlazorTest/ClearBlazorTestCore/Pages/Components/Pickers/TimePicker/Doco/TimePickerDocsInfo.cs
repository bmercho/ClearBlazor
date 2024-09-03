namespace ClearBlazorTest
{
    public record TimePickerDocsInfo:IDocsInfo
    {
        public string Name => "TimePicker";
        public string Description => "";
        public (string, string) ApiLink => ("API", "TimePickerApi");
        public (string, string) ExamplesLink => ("Examples", "TimePicker");
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
