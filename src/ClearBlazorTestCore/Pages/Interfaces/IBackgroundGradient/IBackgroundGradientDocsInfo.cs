/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IBackgroundGradientDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IBackgroundGradient";
        public string Description {get; set; } = "Defines the background gradient.\rTwo backgrounds can be defined each in its own div allowing complex gradients to be created\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BackgroundGradient1", "string?", "The first background gradient on the main div of the component \r"),
            new ApiFieldInfo("BackgroundGradient2", "string?", "The second background gradient on a div inside the main div of the component \r"),
        };
    }
}
