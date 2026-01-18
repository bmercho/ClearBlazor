/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TextBlockDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TextBlock";
        public string Description {get; set; } = "A control that shows text.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "TextBlockApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TextBlock");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Text", "string?", "null", "The text to be displayed. If null the control show the ChildContent\r"),
            new ApiComponentInfo("Color", "Color?", "null", "The color of the text\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("Typo", "<a href=TypoApi>Typo?</a>", "null", "Defines a predefined topography for the text to be shown.\rGenerally this is all that is required to define the topography\rof the text. \r"),
            new ApiComponentInfo("Typography", "TypographyBase?", "null", "Defines the topography for the text to be shown\rNot used if typo is not null\r"),
            new ApiComponentInfo("FontFamily", "string?", "null", "The font family of the text\rOverrides the font family defined in Typo or Typography\r"),
            new ApiComponentInfo("FontSize", "string?", "null", "The font size of the text\rOverrides the font size defined in Typo or Typography\r"),
            new ApiComponentInfo("FontWeight", "int?", "null", "The font weight of the text\rOverrides the font weight defined in Typo or Typography\r"),
            new ApiComponentInfo("FontStyle", "<a href=FontStyleApi>FontStyle?</a>", "null", "The font style of the text\rOverrides the font style defined in Typo or Typography\r"),
            new ApiComponentInfo("LineHeight", "double?", "null", "The line height of the text\rOverrides the line height defined in Typo or Typography\r"),
            new ApiComponentInfo("LetterSpacing", "string?", "null", "The letter spacing of the text\rOverrides the letter spacing defined in Typo or Typography\r"),
            new ApiComponentInfo("TextTransform", "<a href=TextTransformApi>TextTransform?</a>", "null", "The transform applied to the text\rOverrides the text transform defined in Typo or Typography\r"),
            new ApiComponentInfo("TextWrapping", "<a href=TextWrapApi>TextWrap?</a>", "null", "The text wrapping of the text\rOverrides the text wrapping defined in Typo or Typography\r"),
            new ApiComponentInfo("TextTrimming", "bool?", "null", "The text trimming of the text\rOverrides the text trimming defined in Typo or Typography\r"),
            new ApiComponentInfo("TextDecoration", "<a href=TextDecorationApi>TextDecoration?</a>", "null", "The text decoration of the text\r"),
            new ApiComponentInfo("TextAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Start", "The horizontal alignment of the text within the TextBlock. \rIf alignment is set to Stretch the text is centered.\r"),
            new ApiComponentInfo("IsTextSelectionEnabled", "bool", "false", "Indicates if the text can be selected\r"),
            new ApiComponentInfo("ToolTip", "string", "", "The tooltip string\r"),
            new ApiComponentInfo("Clickable", "bool", "false", "Indicates if the text can be clicked\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
