/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AutoFormDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "AutoForm";
        public string Description {get; set; } = "AutoForm is a form component that dynamically generates fields based on a model's properties. It supports\rvarious configurations like label columns and input styles.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "AutoFormApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "AutoForm");
        public (string, string) InheritsLink {get; set; } = ("Form", "FormApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("HasLabelColumn", "bool", "false", "Indicates whether a label column is present. Defaults to false.\r"),
            new ApiComponentInfo("LabelValueCols", "string", "*,*", "Defines the columns for label values in a string format, defaulting to '*,*'. This allows for flexible\rcolumn configuration.\r"),
            new ApiComponentInfo("InputContainerStyle", "<a href=InputContainerStyleApi>InputContainerStyle</a>", "InputContainerStyle.Underlined", "Defines the style of the input container, defaulting to 'Underlined'. It allows customization of the input's\rappearance.\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "Defines the size of a component, with a default value of 'Normal'. \r"),
            new ApiComponentInfo("Immediate", "bool", "false", "Indicates whether the input is immediate. ie Changes the Value as soon as input is received,\rotherwise, Value is updated when the user presses Enter or the input loses focus.\rDefaults to false.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
