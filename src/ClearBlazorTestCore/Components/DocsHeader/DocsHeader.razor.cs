using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsHeader:ClearComponentBase
    {
        [Parameter]
        public string DocsName { get; set; } = string.Empty;

        [Parameter]
        public bool ForApi { get; set; } = false;

        [Parameter]
        public string DocsDescription { get; set; } = string.Empty;

        private string[] _lines = Array.Empty<string>();    


        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _lines = DocsDescription.Split('\r');   
        }
        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

    }
}