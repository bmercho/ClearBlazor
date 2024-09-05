/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ButtonGroupDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ButtonGroup";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ButtonGroupApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ButtonGroup");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("ButtonStyle", "<a href=TextEditFillModeApi>TextEditFillMode?</a>", "null", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Landscape", ""),
            new ApiComponentInfo("Color", "Color?", "null", ""),
            new ApiComponentInfo("DisableBoxShadow", "bool", "false", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("IconLocation", "<a href=IconLocationApi>IconLocation</a>", "IconLocation.Start", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
