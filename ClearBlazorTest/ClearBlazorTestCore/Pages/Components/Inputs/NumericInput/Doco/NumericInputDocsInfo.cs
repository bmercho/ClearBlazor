/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record NumericInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "NumericInput";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "NumericInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "NumericInput");
        public (string, string) InheritsLink {get; set; } = ("<TItem> : ContainerInputBase<TItem>", "<TItem> : ContainerInputBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("DecimalPlaces", "int", "0", ""),
            new ApiComponentInfo("AllowNegativeNumbers", "bool", "true", ""),
            new ApiComponentInfo("ShowSpinButtons", "bool", "false", ""),
            new ApiComponentInfo("Step", "TItem?", "null", ""),
            new ApiComponentInfo("Culture", "CultureInfo", "CultureInfo.InvariantCulture", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
