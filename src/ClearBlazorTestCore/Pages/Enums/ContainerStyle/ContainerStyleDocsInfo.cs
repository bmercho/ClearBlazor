/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ContainerStyleDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "ContainerStyle";
        public string Description {get; set; } = "The style of an input container\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("None", "ContainerStyle", "The container is not filled or outlined\r"),
            new ApiFieldInfo("Filled", "ContainerStyle", "The container is color filled\r"),
            new ApiFieldInfo("Outline", "ContainerStyle", "The container is outlined\r"),
        };
    }
}
