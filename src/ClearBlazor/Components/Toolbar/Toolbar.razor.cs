using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control with a row of IconButtons that provide quick access to 
    /// frequently used functions or tools.
    /// </summary>
    public partial class Toolbar : ClearComponentBase, IBorder, IBoxShadow
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// Orientation of the control.
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Landscape;

        /// <summary>
        /// Used when in ToolbarTray. Gives the order of this toolbar within the tray
        /// </summary>
        [Parameter]
        public int TrayOrder { get; set; } = 0;

        /// <summary>
        /// Used when in ToolbarTray. Indicates that this toolbar should be displayed on the next line.
        /// </summary>
        [Parameter]
        public bool NewLine { get; set; } = false;

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href="IBoxShadowApi">IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; }

        private bool IsInToolbarTray { get; set; } = false;

        private int Order { get; set; } = 0;

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

        private string GetIconButtonStyle()
        {
            if (!Dragging)
                return "cursor: move; ";
            return "";
        }
     }
}