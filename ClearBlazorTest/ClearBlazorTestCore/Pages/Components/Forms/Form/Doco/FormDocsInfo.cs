/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record FormDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Form";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "FormApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Form");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("Model", "object", "null!", ""),
            new ApiComponentInfo("ReadOnly", "bool", "false", ""),
            new ApiComponentInfo("ValidationErrorLocation", "<a href=ValidationErrorLocationApi>ValidationErrorLocation</a>", "ValidationErrorLocation.ErrorIcon", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task<bool> Validate()", "async", "", ""),
        };
    }
}
