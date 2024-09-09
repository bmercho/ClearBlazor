/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ContainerShapeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "ContainerShape";
        public string Description {get; set; } = "Defines the shape of Icon buttons and Avatars\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Circle", "ContainerShape", "Circle shape\r"),
            new ApiFieldInfo("Square", "ContainerShape", "Square shape\r"),
            new ApiFieldInfo("SquareRounded", "ContainerShape", "Square but with rounded corners\r"),
        };
    }
}
