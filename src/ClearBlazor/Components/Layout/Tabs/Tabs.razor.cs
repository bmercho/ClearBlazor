using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Allows you to split the interface up into different areas, 
    /// each accessible by clicking on the tab header, positioned at the top of the control. 
    /// </summary>
    public partial class Tabs : IBackground, IBorder
    {
        /// <summary>
        /// The size of tab header
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// The color of the tab header
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; } = null;

        /// <summary>
        /// See <a href=IBackgroundApi>IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// An event that is raised when the Tab is changed.
        /// </summary>
        [Parameter]
        public EventCallback<Tab> OnTabChanged { get; set; }

        private string? _gridCornerRadius { get; set; } = null;

        internal Tab? _activePage { get; set; } = null;

        private List<Tab> _pages = new List<Tab>();

        bool? _backgroundIsNull = null;
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_backgroundIsNull == null)
                _backgroundIsNull = BackgroundColor == null;

            if (_backgroundIsNull == true)
                BackgroundColor = TabsTokens.ContainerColor;
        }

        internal void AddPage(Tab tabPage)
        {
            _pages.Add(tabPage);
            if (_pages.Count == 1)
                _activePage = tabPage;

            StateHasChanged();
        }

        protected override string UpdateStyle(string css)
        {
            if (CornerRadius != null)
                _gridCornerRadius = CornerRadius;

            return css;
        }

        protected Color GetButtonColor(Tab page)
        {
            
            if (page == _activePage)
            {
                if (Color == null)
                    return TabsTokens.ActiveColor;
                return Color;
            }
            else
            {
                return TabsTokens.InactiveColor;
            }
        }

        void ActivatePage(Tab page)
        {
            if (!page.Disabled)
            {
                _activePage = page;
                OnTabChanged.InvokeAsync(page);
                StateHasChanged();
            }
        }
    }
}