/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TextWrapDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "TextWrap";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Wrap", "TextWrap", ""),
            new ApiFieldInfo("WrapOnNewLines", "TextWrap", ""),
            new ApiFieldInfo("NoWrap", "TextWrap", ""),
        };
    }
}
