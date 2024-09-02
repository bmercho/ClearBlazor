using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Tabs : IContent, IBackground, IBorder
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColour { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }


        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public Color? Colour { get; set; } = null;

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

        protected Color GetButtonColour(Tab page)
        {
            
            if (page == ActivePage)
            {
                if (Colour == null)
                    return ThemeManager.CurrentPalette.Primary;
                return Colour;
            }
            else
            {
                if (Colour == null)
                    return ThemeManager.CurrentPalette.Primary.Darken(.25);
                return Colour.Darken(.25);
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