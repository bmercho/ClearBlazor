/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ScrollbarGutterDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "ScrollbarGutter";
        public string Description {get; set; } = "Indicates when the scrollbar gutters are shown.\rIf the scrollbar is set to an Overlay scrollbar then no gutters are shown.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("OnlyWhenOverflowed", "ScrollbarGutter", "The gutter is only shown when the scrollbar is also shown,\r"),
            new ApiFieldInfo("Always", "ScrollbarGutter", "The gutter is always shown if the scrollbar mode is Auto or Scroll but not disabled\r"),
            new ApiFieldInfo("AlwaysBothEdges", "ScrollbarGutter", "A gutter is also shown on the other side to match the normal gutter.\rHas same conditions for showing as Always.\r"),
        };
    }
}
