/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ButtonDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Button";
        public string Description {get; set; } = "Represents a button for actions, links, and commands.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ButtonApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Button");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("ButtonStyle", "<a href=ButtonStyleApi>ButtonStyle?</a>", "null", "Defines the button style.\r"),
            new ApiComponentInfo("Color", "Color?", "null", "The color used for the button. What gets this color (background, text or outline)\rdepends on the button style.\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of the button\r"),
            new ApiComponentInfo("Shape", "<a href=ContainerShapeApi>ContainerShape</a>", "ContainerShape.FullyRounded", "The shape of the button\r"),
            new ApiComponentInfo("Icon", "string?", "null", "The icon to be shown within the button\r"),
            new ApiComponentInfo("IconColor", "Color?", "null", "The color of the icon within the button\r"),
            new ApiComponentInfo("IconLocation", "<a href=IconLocationApi>IconLocation</a>", "IconLocation.Start", "The location of the icon within the button\r"),
            new ApiComponentInfo("ToolTip", "string?", "null", "Tooltip string for the button\r"),
            new ApiComponentInfo("ToolTipPosition", "<a href=ToolTipPositionApi>ToolTipPosition?</a>", "null", "Tooltip position (when shown)\r"),
            new ApiComponentInfo("Disabled", "bool", "false", "Indicates if button is disabled.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
