/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AlignmentDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Alignment";
        public string Description {get; set; } = "Defines the alignment of content within its available space\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Stretch", "Alignment", "The content is stretched using all available space\r"),
            new ApiFieldInfo("Start", "Alignment", "The content is aligned to the start of the available space\r"),
            new ApiFieldInfo("Center", "Alignment", "The content is centered in the available space\r"),
            new ApiFieldInfo("End", "Alignment", "The content is aligned to the end of the available space\r"),
        };
    }
}
