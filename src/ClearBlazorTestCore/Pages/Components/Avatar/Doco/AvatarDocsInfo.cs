/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AvatarDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Avatar";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "AvatarApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Avatar");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Icon", "string", "string.Empty", ""),
            new ApiComponentInfo("Image", "string", "string.Empty", ""),
            new ApiComponentInfo("Alt", "string", "string.Empty", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("AvatarStyle", "<a href=TextEditFillModeApi>TextEditFillMode</a>", "TextEditFillMode.Filled", ""),
            new ApiComponentInfo("Shape", "<a href=ContainerShapeApi>ContainerShape</a>", "ContainerShape.Circle", ""),
            new ApiComponentInfo("Color", "Color", "AvatarTokens.Color", ""),
            new ApiComponentInfo("Text", "string", "string.Empty", ""),
            new ApiComponentInfo("IconColor", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
