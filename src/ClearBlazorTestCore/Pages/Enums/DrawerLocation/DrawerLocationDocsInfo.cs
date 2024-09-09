/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DrawerLocationDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "DrawerLocation";
        public string Description {get; set; } = "Used by Drawer to indicate which side the sides in from.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Left", "DrawerLocation", "Draw slides in from the left.\r"),
            new ApiFieldInfo("Right", "DrawerLocation", "Draw slides in from the right.\r"),
            new ApiFieldInfo("Top", "DrawerLocation", "Draw slides in from the top.\r"),
            new ApiFieldInfo("Bottom", "DrawerLocation", "Draw slides in from the bottom.\r"),
        };
    }
}
