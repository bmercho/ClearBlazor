/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record InputContainerStyleDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "InputContainerStyle";
        public string Description {get; set; } = "The style of the input container\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Filled", "InputContainerStyle", "The container has a background fill color\r"),
            new ApiFieldInfo("Outlined", "InputContainerStyle", "The container is outlined\r"),
            new ApiFieldInfo("LabelOnly", "InputContainerStyle", "The container just has a label and no background and no outline\r"),
            new ApiFieldInfo("Underlined", "InputContainerStyle", "The container is underlined and no background and no outline\r"),
        };
    }
}
