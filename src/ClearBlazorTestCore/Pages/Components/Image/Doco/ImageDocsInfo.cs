/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ImageDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Image";
        public string Description {get; set; } = "A control thats shows an image\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ImageApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Image");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Source", "string", "string.Empty", "The source uri of the image\r"),
            new ApiComponentInfo("Alternative", "string", "string.Empty", "The alternative string show if the source does not exist\r"),
            new ApiComponentInfo("Stretch", "<a href=ImageStretchApi>ImageStretch</a>", "ImageStretch.Uniform", "How the image is shown in regard to aspect ratio and stretching\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
