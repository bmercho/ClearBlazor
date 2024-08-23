using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Card: ClearComponentBase
    {
        [Parameter]
        public RenderFragment? CardHeader { get; set; }

        [Parameter]
        public RenderFragment? CardSection { get; set; }

        [Parameter]
        public RenderFragment? CardFooter { get; set; }
    }
}