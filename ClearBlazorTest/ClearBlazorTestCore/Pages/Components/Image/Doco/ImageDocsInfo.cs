/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ImageDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Image";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ImageApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Image");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Source", "string", "string.Empty", ""),
            new ApiComponentInfo("Alternative", "string", "string.Empty", ""),
            new ApiComponentInfo("Stretch", "<a href=ImageStretchApi>ImageStretch</a>", "ImageStretch.Uniform", ""),
            new ApiComponentInfo("StretchDirection", "<a href=StretchDirectionApi>StretchDirection</a>", "StretchDirection.Both", ""),
            new ApiComponentInfo("ImageId", "string", "string.Empty", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
