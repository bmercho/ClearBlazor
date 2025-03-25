/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record FormDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Form";
        public string Description {get; set; } = "Represents a form control that can contain child content and manage input validation.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "FormApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Form");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Model", "object", "null!", "Represents the data model for the component. It is a public property that can be set to any object.\r"),
            new ApiComponentInfo("ShowLabels", "bool", "false", "Indicates if the label will be shown above the field\r"),
            new ApiComponentInfo("ReadOnly", "bool", "false", "Indicates whether the component is in read-only mode. When set to true, user input is disabled.\r"),
            new ApiComponentInfo("ValidationErrorLocation", "<a href=ValidationErrorLocationApi>ValidationErrorLocation</a>", "ValidationErrorLocation.ErrorIcon", "Specifies the location of validation error indicators. Defaults to displaying the error icon.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task<bool> Validate()", "async", "", "Validates each input field asynchronously. \r"),
        };
    }
}
