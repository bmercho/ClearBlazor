using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Toolbar : ClearComponentBase, IBorder
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
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
        public Orientation Orientation { get; set; } = Orientation.Landscape;

        [Parameter]
        public OverflowMode OverflowMode { get; set; } = OverflowMode.Wrap;

        [Parameter]
        // When in ToolBarTray gives the order of this toolbar within the tray
        public int TrayOrder { get; set; } = 0;

        [Parameter]
        public bool NewLine { get; set; } = false;

        public bool IsInToolbarTray { get; set; } = false;

        public int Order { get; set; } = 0;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            VerticalAlignment = Alignment.Start;
            HorizontalAlignment = Alignment.Start;
            
            var parent = Parent?.Parent as ToolbarTray;
            if (parent != null)
            {
                BorderThickness = "0";
                parent?.AddToolbar(this);
                IsInToolbarTray = true;    
            }
        }

        protected override string UpdateStyle(string css)
        {
            var parent = Parent?.Parent as ToolbarTray;
            if (parent != null)
            {
                Order = parent.GetTrayOrder(this);
                css += $"order: {Order}; ";
            }
 
            return css;
        }
     }
}