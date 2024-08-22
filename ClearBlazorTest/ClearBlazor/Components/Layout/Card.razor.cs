using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Card: ClearComponentBase,IContent
    {
        public RenderFragment? ChildContent { get; set; }
    }
}