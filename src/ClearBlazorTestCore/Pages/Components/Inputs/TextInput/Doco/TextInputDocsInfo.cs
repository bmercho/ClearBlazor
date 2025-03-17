/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TextInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TextInput";
        public string Description {get; set; } = "Represents a text input field\r";
        public (string, string) ApiLink  {get; set; } = ("API", "TextInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TextInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<string>", "ContainerInputBase<string>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Required", "bool", "false", "When used in a form, indicates if the field is required\r"),
            new ApiComponentInfo("MaxLength", "int?", "null", "Specifies the maximum length of a value. It can be set to a nullable integer, allowing for no limit if null.\r"),
            new ApiComponentInfo("Lines", "int?", "1", "Specifies the number of lines, allowing for a nullable integer value. Defaults to 1 if not set.\r"),
            new ApiComponentInfo("IsPassword", "bool", "false", "Indicates whether the input field is for a password. Defaults to false.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
