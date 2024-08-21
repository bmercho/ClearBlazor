using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public interface IContent
    {
        public RenderFragment? ChildContent { get; set; }
    }
}
