namespace ClearBlazorTest
{
    public record ClearComponentBaseDocsInfo:IDocsInfo
    {
        public string Name => "ClearComponentBase";
        public string Description => "";
        public (string, string) ApiLink => ("API", "ClearComponentBaseApi");
        public (string, string) ExamplesLink => ("", "");
        public (string, string) InheritsLink => ("", "Api");
        public List<(string, string)> ImplementsLinks => new()
        {
        };
        public List<ApiComponentInfo> ParameterApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("OnClicked", "EventCallback<MouseEventArgs>", "", "Event raised when the component is clicked \r"),
        };
        public List<ApiComponentInfo> PropertyApi => new List<ApiComponentInfo>
        {
        };
        public List<ApiComponentInfo> MethodApi => new List<ApiComponentInfo>
        {
        };
    }
}
