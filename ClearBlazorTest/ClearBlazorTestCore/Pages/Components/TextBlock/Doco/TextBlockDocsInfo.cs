/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TextBlockDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TextBlock";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "TextBlockApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TextBlock");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
            (" IBackground", " IBackgroundApi"),
            ("IColour", "IColourApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("OnMouseOver", "EventCallback<MouseOverEventArgs>", "", ""),
            new ApiComponentInfo("Colour", "Color?", "null", ""),
            new ApiComponentInfo("BackgroundColour", "Color?", "null", ""),
            new ApiComponentInfo("Typo", "<a href=TypoApi>Typo?</a>", "null", ""),
            new ApiComponentInfo("Typography", "TypographyBase?", "null", ""),
            new ApiComponentInfo("FontFamily", "string?", "null", ""),
            new ApiComponentInfo("FontSize", "string?", "null", ""),
            new ApiComponentInfo("FontWeight", "int?", "null", ""),
            new ApiComponentInfo("FontStyle", "<a href=FontStyleApi>FontStyle?</a>", "null", ""),
            new ApiComponentInfo("LineHeight", "double?", "null", ""),
            new ApiComponentInfo("LetterSpacing", "string?", "null", ""),
            new ApiComponentInfo("TextTransform", "<a href=TextTransformApi>TextTransform?</a>", "null", ""),
            new ApiComponentInfo("TextWrapping", "<a href=TextWrapApi>TextWrap?</a>", "null", ""),
            new ApiComponentInfo("TextTrimming", "bool?", "null", ""),
            new ApiComponentInfo("IsTextSelectionEnabled", "bool", "false", ""),
            new ApiComponentInfo("ToolTip", "string", "", ""),
            new ApiComponentInfo("Clickable", "bool", "false", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
