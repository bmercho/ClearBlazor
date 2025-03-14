/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AvatarDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Avatar";
        public string Description {get; set; } = "Represents a component which displays circular user profile pictures, icons or text.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "AvatarApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Avatar");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Icon", "string", "string.Empty", "The icon to be shown within the avatar\r"),
            new ApiComponentInfo("Image", "string", "string.Empty", "The image to be shown within the avatar\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of the avatar\r"),
            new ApiComponentInfo("AvatarStyle", "<a href=AvatarStyleApi>AvatarStyle</a>", "AvatarStyle.Filled", "The style of the avatar\r"),
            new ApiComponentInfo("Shape", "<a href=ContainerShapeApi>ContainerShape</a>", "ContainerShape.Circle", "The shape of the avatar\r"),
            new ApiComponentInfo("Color", "Color", "Color.Primary", "The color used for the button. What gets this color (background, text or outline)\rdepends on the button style.\r"),
            new ApiComponentInfo("Text", "string", "string.Empty", "The text to be shown within the avatar\r"),
            new ApiComponentInfo("IconColor", "Color?", "null", "The color of the icon within the avatar\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
