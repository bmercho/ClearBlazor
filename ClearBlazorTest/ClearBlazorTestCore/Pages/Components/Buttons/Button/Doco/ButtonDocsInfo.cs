/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ButtonDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Button";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ButtonApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Button");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IColor", " IColorApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("ButtonStyle", "<a href=TextEditFillModeApi>TextEditFillMode?</a>", "null", ""),
            new ApiComponentInfo("Color", "Color?", "null", ""),
            new ApiComponentInfo("DisableBoxShadow", "bool", "false", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Shape", "<a href=ContainerShapeApi>ContainerShape</a>", "ContainerShape.Square", ""),
            new ApiComponentInfo("Icon", "string?", "null", ""),
            new ApiComponentInfo("IconColor", "Color?", "null", ""),
            new ApiComponentInfo("IconLocation", "<a href=IconLocationApi>IconLocation</a>", "IconLocation.Start", ""),
            new ApiComponentInfo("ToolTip", "string?", "null", ""),
            new ApiComponentInfo("ToolTipPosition", "<a href=ToolTipPositionApi>ToolTipPosition?</a>", "null", ""),
            new ApiComponentInfo("ToolTipDelay", "int?", "null; // Milliseconds", ""),
            new ApiComponentInfo("Disabled", "bool", "false", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
