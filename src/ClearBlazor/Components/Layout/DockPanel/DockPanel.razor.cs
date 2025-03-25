using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A Dock Panel is used to dock child elements in the left, right, top, and bottom positions of the panel. 
    /// The position of child elements is determined by the Dock property of the respective child elements
    /// If a child does not have a <a href="DockApi">Dock</a> property it uses the remaining available space of the panel.
    /// </summary>
    public partial class DockPanel : ClearComponentBase, IBackground
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

        protected override bool HaveParametersChanged(ClearComponentBase child, ParameterView parameters)
        {
            var oldDock = child.Dock;
            var match = parameters.TryGetValue<Dock>(nameof(Dock), out var newDock);

            if (match && newDock != oldDock)
                return true;

            return false;
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display:grid; grid-template-columns: {ColumnsStyle}; " +
                   $"grid-template-rows: {RowsStyle};";

            return css;
        }

        private string ColumnsStyle =>
        this.ComputeFramesStyle(ClearBlazor.Dock.Left, ClearBlazor.Dock.Right);

        private string RowsStyle =>
        this.ComputeFramesStyle(ClearBlazor.Dock.Top, ClearBlazor.Dock.Bottom);

        private string ComputeFramesStyle(Dock start, Dock end)
        {
            return $"{ComputeFramesPartialStyle(start)} 1fr {ComputeFramesPartialStyle(end)}";
        }

        private string ComputeFramesPartialStyle(Dock dock)
        {
            return string.Join(" ", Children.Where(d => d.Dock == dock).Select(d =>"auto"));
        }

        protected override string UpdateChildStyle(ClearComponentBase child, string css)
        {
            return css + $"grid-column: {GetColumnStyle(child)}; grid-row: {GetRowStyle(child)}; {Style}";

        }

        private string GetColumnStyle(ClearComponentBase child)
        {
            return GetFrameStyle(child, ClearBlazor.Dock.Left, ClearBlazor.Dock.Right);
        }

        private string GetRowStyle(ClearComponentBase child)
        {
            return GetFrameStyle(child, ClearBlazor.Dock.Top, ClearBlazor.Dock.Bottom);
        }
        private string GetFrameStyle(ClearComponentBase child, Dock start, Dock end)
        {
            if (child.Dock == start)
            {
                // | L0 | L1 | ***** | R2 | R1 | R0 |
                // 1    2    3       4    5    6    7

                var index = GetIndexOf(child, start);
                return $"{index + 1} / {index + 2}";
            }
            else if (child.Dock == end)
            {
                // | L0 | L1 | ***** | R2 | R1 | R0 |
                // 1    2    3       4    5    6    7
                // count = 5 

                // | ***** | R2 | R1 | R0 |
                // 1       2    3    4    5
                // count = 3 

                var count = Children.Count(d => d.Dock == start || d.Dock == end);

                var index = count - GetIndexOf(child, end);
                return $"{index + 1} / {index + 2}";
            }
            else
            {
                // L0 L1 T3 T3 T3 T3 R2  1
                // L0 L1 L6 T7 T7 R4 R2  2
                // L0 L1 L6 ** R8 R4 R2  3
                // L0 L1 L6 B9 R8 R4 R2  4
                // L0 L1 B5 B5 B5 R4 R2  5
                //1  2  3  4  5  6  7  8|6

                var count = Children.Count(d => d.Dock == start || d.Dock == end);
                var countLeft = Children.TakeWhile(d => d != child).Count(d => d.Dock == start);
                var countRight = Children.TakeWhile(d => d != child).Count(d => d.Dock == end);

                return $"{countLeft + 1} / {count - countRight + 2}";
            }
        }

        private int GetIndexOf(ClearComponentBase child, Dock dock)
        {
            var index = 0;
            foreach (var dockItem in Children.Where(d => d.Dock == dock))
            {
                if (object.ReferenceEquals(child, dockItem))
                {
                    return index;
                }
                index++;
            }
            throw new KeyNotFoundException();
        }
    }
}