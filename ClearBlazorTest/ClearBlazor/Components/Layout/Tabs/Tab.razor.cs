using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Tab
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public string? Text { get; set; } = null;

        [Parameter]
        public string? Value { get; set; } = null;

        [Parameter]
        public string? Icon { get; set; } = null;

        [Parameter]
        public string? ToolTip { get; set; } = null;

        [Parameter]
        public bool Disabled { get; set; } = false;

        public Color? IconColor { get; set; } = null;

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
    }
}