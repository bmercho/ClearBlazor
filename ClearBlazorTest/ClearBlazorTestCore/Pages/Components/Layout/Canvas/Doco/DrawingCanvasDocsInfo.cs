namespace ClearBlazorTest
{
    public record DrawingCanvasDocsInfo:IDocsInfo
    {
        public string Name => "DrawingCanvas";
        public string Description => "";
        public (string, string) ApiLink => ("API", "DrawingCanvasApi");
        public (string, string) ExamplesLink => ("Examples", "DrawingCanvas");
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
