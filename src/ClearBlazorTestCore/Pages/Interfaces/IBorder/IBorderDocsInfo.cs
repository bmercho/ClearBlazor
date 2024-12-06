/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IBorderDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IBorder";
        public string Description {get; set; } = "Defines the border for a component\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BorderThickness", "string?", "The thickness of the border in pixels.\rCan be in the format of:\r    20 - all borders 20px\r    20,10 - top and bottom borders are 10, left and right borders are 10\r    20,10,30,40 - top is 20px, right is 10px, bottom is 30px and left is 40px\r"),
            new ApiFieldInfo("BorderColor", "Color?", "The color of the border\r"),
            new ApiFieldInfo("BorderStyle", "BorderStyle?", "The style of the border. Solid,Dotted or Dashed.\r"),
            new ApiFieldInfo("CornerRadius", "string?", "The corner radius of the control\rCan be in the format of:\r    4 - all borders have 4px radius\r    4,8 - top and bottom borders have 4px radius, left and right borders have 8px radius\r    20,10,30,40 - top has 20px radius, right has 10px radius, bottom has 30px radius and left has 40px radius\r"),
        };
    }
}
