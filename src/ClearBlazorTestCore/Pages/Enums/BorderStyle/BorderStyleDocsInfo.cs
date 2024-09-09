/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record BorderStyleDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "BorderStyle";
        public string Description {get; set; } = "The style of the border around a component\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Solid", "BorderStyle", "A solid border\r"),
            new ApiFieldInfo("Dotted", "BorderStyle", "A dotted border\r"),
            new ApiFieldInfo("Dashed", "BorderStyle", "A dashed border\r"),
        };
    }
}
