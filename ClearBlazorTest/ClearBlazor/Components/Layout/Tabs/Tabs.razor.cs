using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Tabs : IBackground, IBorder
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColor { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }


        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public Color? Color { get; set; } = null;

        [Parameter]
        public EventCallback<Tab> OnTabChanged { get; set; }

        public string? GridCornerRadius { get; set; } = null;

        public Tab? ActivePage { get; set; } = null;

        List<Tab> Pages = new List<Tab>();

        internal void AddPage(Tab tabPage)
        {
            Pages.Add(tabPage);
            if (Pages.Count == 1)
                ActivePage = tabPage;

            StateHasChanged();
        }

        protected override string UpdateStyle(string css)
        {
            if (CornerRadius != null)
                GridCornerRadius = CornerRadius;

            return css;
        }

        protected Color GetButtonColor(Tab page)
        {
            
            if (page == ActivePage)
            {
                if (Color == null)
                    return ThemeManager.CurrentPalette.Primary;
                return Color;
            }
            else
            {
                if (Color == null)
                    return ThemeManager.CurrentPalette.Primary.Darken(.25);
                return Color.Darken(.25);
            }
        }

        void ActivatePage(Tab page)
        {
            if (!page.Disabled)
            {
                ActivePage = page;
                OnTabChanged.InvokeAsync(page);
            }
        }
    }
}