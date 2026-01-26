/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record VideoDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Video";
        public string Description {get; set; } = "A control thats plays a video\r";
        public (string, string) ApiLink  {get; set; } = ("API", "VideoApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Video");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Source", "string", "string.Empty", "The source uri of the video\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
