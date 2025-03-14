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
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("ButtonStyle", "<a href=ButtonStyleApi>ButtonStyle?</a>", "null", " The button style for all the buttons in the button group\r"),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Landscape", "Orientation of the button group\r"),
            new ApiComponentInfo("Color", "Color?", "null", " The color used for all the buttons in the button group\r"),
            new ApiComponentInfo("OutlineColor", "Color?", "null", " The outline colour used all the buttons in the button group\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", " The button size used for all the buttons in the button group\r"),
            new ApiComponentInfo("IconLocation", "<a href=IconLocationApi>IconLocation</a>", "IconLocation.Start", " The icon location used for all the buttons in the button group\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
