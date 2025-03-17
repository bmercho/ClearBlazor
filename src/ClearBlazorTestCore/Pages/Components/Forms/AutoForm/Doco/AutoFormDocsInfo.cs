/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AutoFormDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "AutoForm";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "AutoFormApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "AutoForm");
        public (string, string) InheritsLink {get; set; } = ("Form", "FormApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("HasLabelColumn", "bool", "false", ""),
            new ApiComponentInfo("LabelValueCols", "string", "*,*", ""),
            new ApiComponentInfo("TextEditFillMode", "<a href=InputContainerStyleApi>InputContainerStyle</a>", "InputContainerStyle.Underlined", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Immediate", "bool", "false", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("GetLabelTypography()", "TypographyBase", "", ""),
            new ApiComponentInfo("GetMargin(AutoFormField field)", "string", "", ""),
            new ApiComponentInfo("GetFieldIndex(AutoFormField field)", "int", "", ""),
        };
    }
}
