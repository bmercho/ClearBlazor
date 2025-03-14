/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ButtonStyleDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "ButtonStyle";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Filled", "ButtonStyle", "The button has a background fill color\r"),
            new ApiFieldInfo("Outlined", "ButtonStyle", "The button is outlined\r"),
            new ApiFieldInfo("LabelOnly", "ButtonStyle", "The button just has a label and no background and no outline\r"),
            new ApiFieldInfo("Elevated", "ButtonStyle", "The button is the same as a Filled button but has a box shadow\r"),
        };
    }
}
