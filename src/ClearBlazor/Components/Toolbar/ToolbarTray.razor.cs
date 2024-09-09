using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ToolbarTray:ClearComponentBase, IBackground
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        List<Toolbar> Toolbars = new List<Toolbar>();

        protected override string UpdateStyle(string css)
        {
            css += $"display: flex; flex-wrap: wrap; ";
 
            return css;
        }
 
        public bool FirstTime { get; set; } = true;

        public int GetTrayOrder(Toolbar toolbar)
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

        public void AddToolbar(Toolbar toolbar)
        {
            if (!Toolbars.Contains(toolbar))
                Toolbars.Add(toolbar);
        }

        public void OnDragOver()
        {

        }
    }
}