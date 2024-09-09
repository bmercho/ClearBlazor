/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IColorDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IColor";
        public string Description {get; set; } = "The foreground of the component, usually used for text color.\rIf Color is null then an attempt to find is made by getting the contrasting color\rto the background color of this component or a descendent component (if the component background is null)\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Color", "Color?", "The foreground color.\r"),
        };
    }
}
