/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record InputBaseDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "InputBase";
        public string Description {get; set; } = "Base class for all input components\r";
        public (string, string) ApiLink  {get; set; } = ("API", "InputBaseApi");
        public (string, string) ExamplesLink {get; set; } = ("", "");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Label", "string?", "null", "The label of the input\r"),
            new ApiComponentInfo("FieldName", "string", "string.Empty", "The name of the field in the model\r"),
            new ApiComponentInfo("ToolTip", "string", "", "The tooltip of the input    \r"),
            new ApiComponentInfo("Color", "Color?", "null", "The color of the input\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of the input\r"),
            new ApiComponentInfo("IsDisabled", "bool", "false", "Indicates whether the input is disabled\r"),
            new ApiComponentInfo("IsReadOnly", "bool", "false", "Indicates whether the input is read only\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
