/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record AvatarStyleDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "AvatarStyle";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Filled", "AvatarStyle", "The avatar has a background fill color\r"),
            new ApiFieldInfo("Outlined", "AvatarStyle", "The avatar is outlined\r"),
            new ApiFieldInfo("LabelOnly", "AvatarStyle", "The avatar just has a label and no background and no outline\r"),
        };
    }
}
