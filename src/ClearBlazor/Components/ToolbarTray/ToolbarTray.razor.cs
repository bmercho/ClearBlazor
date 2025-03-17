using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control that holds a number of toolbars, that can be reordered and placed on new lines.
    /// </summary>
    public partial class ToolbarTray:ClearComponentBase, IBackground
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        List<Toolbar> Toolbars = new List<Toolbar>();

        protected override string UpdateStyle(string css)
        {
            css += $"display: flex; flex-wrap: wrap; ";
 
            return css;
        }
 
        private bool FirstTime { get; set; } = true;

        internal int GetTrayOrder(Toolbar toolbar)
        {
            var toolbars = Toolbars.OrderBy(t => t.TrayOrder).ToList();
            int trayOrder = 1;
            foreach (var tb in toolbars)
            {
                if (toolbar != null)
                {
                     if (tb == toolbar)
                        return trayOrder;
                    if (tb.NewLine)
                        trayOrder++;
                    trayOrder++;
                }
            }
            return 0;
        }

        internal void AddToolbar(Toolbar toolbar)
        {
            if (!Toolbars.Contains(toolbar))
                Toolbars.Add(toolbar);
        }

        private void OnDragOver()
        {

        }
    }
}