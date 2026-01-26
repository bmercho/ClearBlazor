using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Tab
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// The text displayed in the tab header
        /// </summary>
        [Parameter]
        public string? Text { get; set; } = null;

        /// <summary>
        /// The icon shown in the tab header
        /// </summary>
        [Parameter]
        public string? Icon { get; set; } = null;

        /// <summary>
        /// The text shown as a tooltip when a tabbed is hovered over.
        /// </summary>
        [Parameter]
        public string? ToolTip { get; set; } = null;

        /// <summary>
        /// Indicates if the tab is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; } = false;

        private Tabs? _parent = null;

        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent),
                    "TabPage must exist within a TabControl");

            base.OnInitialized();

            _parent = Parent?.Parent?.Parent as Tabs;

            if (_parent == null)
                return;

            _parent.AddPage(this);
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; ";
            return css;
        }

    }
}