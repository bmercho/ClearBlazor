using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class Link : ClearComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public Underline Underline { get; set; } = Underline.Default;

        [Parameter]
        public string? HRef { get; set; } = null;

        [Inject]
        private NavigationManager NavManager { get; set; } = null!;

        private bool _mouseOver = false;

        protected override string UpdateStyle(string css)
        {
            css += $"color: {ThemeManager.CurrentColorScheme.ToolTipTextColor.Value}; ";
            return css;
        }

        private TextDecoration? GetTextDecoration()
        {
            switch (Underline)
            {
                case Underline.Default:
                    if (_mouseOver)
                        return TextDecoration.Underline;
                    else
                        return null;
                case Underline.Always:
                    return TextDecoration.Underline;
                case Underline.None:
                    return null;
            }
            return null;
        }

        protected void OnMouseEnter(MouseEventArgs e)
        {
            _mouseOver = true;
            StateHasChanged();
        }

        protected void OnMouseLeave(MouseEventArgs e)
        {
            _mouseOver = false;
            StateHasChanged();
        }

        private void OnLinkClicked()
        {
            if (HRef != null)
                NavManager.NavigateTo(HRef);
        }
    }
}