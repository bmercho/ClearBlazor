/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IColourDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IColour";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Colour", "Color?", ""),
        };
    }
}
