using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using System.Text;

namespace ClearBlazor
{
    /// <summary>
    /// An abstract base class for all components.
    /// </summary>
    public abstract class ClearComponentBase : ComponentBase, IDisposable, IHandleEvent
    {
        [CascadingParameter]
        public ClearComponentBase? Parent { get; set; } = null;

        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = null!;

        /// <summary>
        /// Name of component
        /// </summary>
        [Parameter]
        public string? Name { get; set; } = null;

        /// <summary>
        /// The width of the component. Takes precedence over other layout requirements.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// The height of the component. Takes precedence over other layout requirements.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// The minimum width of the component. Takes precedence over other layout requirements.
        /// </summary>
        [Parameter]
        public double MinWidth { get; set; } = 0;

        /// <summary>
        /// The minimum height of the component. Takes precedence over other layout requirements.
        /// </summary>
        [Parameter]
        public double MinHeight { get; set; } = 0;

        /// <summary>
        /// The maximum width of the component. Takes precedence over other layout requirements, apart from width.
        /// </summary>
        [Parameter]
        public double MaxWidth { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// The maximum height of the component. Takes precedence over other layout requirements, apart from height.
        /// </summary>
        [Parameter]
        public double MaxHeight { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// The margin of the component.
        /// Can be in the format of:
        ///     4 - all margins are 4px
        ///     4,8 - top and bottom margins are 4px radius, left and right margins have 8px radius
        ///     20,10,30,40 - top has 20px margin, right has 10px margin, bottom has 30px margin and left has 40px margin
        /// </summary>
        [Parameter]
        public string Margin { get; set; } = String.Empty;

        /// <summary>
        /// The padding of the component.
        /// Can be in the format of:
        ///     4 - all paddings are 4px
        ///     4,8 - top and bottom paddings are 4px radius, left and right paddings have 8px radius
        ///     20,10,30,40 - top has 20px padding, right has 10px padding, bottom has 30px padding and left has 40px padding
        /// </summary>
        [Parameter]
        public string Padding { get; set; } = String.Empty;

        /// <summary>
        /// The horizontal alignment of the component in its available space.
        /// </summary>
        [Parameter]
        public Alignment? HorizontalAlignment { get; set; } = null;

        /// <summary>
        /// The vertical alignment of the component in its available space.
        /// </summary>
        [Parameter]
        public Alignment? VerticalAlignment { get; set; } = null;

        /// <summary>
        /// Tag is user definable value.
        /// </summary>
        [Parameter]
        public int? Tag { get; set; } = null;


        // For Grid children

        /// <summary>
        /// Applies to children of a grid. Indicates the start row of the grid that the child will occupy. 
        /// The first row is 0.
        /// </summary>
        [Parameter]
        public int Row { get; set; } = 0;
        /// <summary>
        /// Applies to children of a grid. Indicates the start column of the grid that the child will occupy. 
        /// The first column is 0.
        /// </summary>
        [Parameter]
        public int Column { get; set; } = 0;
        /// <summary>
        /// Applies to children of a grid. Indicates how many rows of the grid that the child will occupy (starting at Row). 
        /// </summary>
        [Parameter]
        public int RowSpan { get; set; } = 1;
        /// <summary>
        /// Applies to children of a <a href=GridPage>Grid</a>. Indicates how many columns of the grid that the child will occupy (starting at Column). 
        /// </summary>
        [Parameter]
        public int ColumnSpan { get; set; } = 1;


        // For DockPanel children
        /// <summary>
        /// Applies to children of a <a href=GridPage>DockPanel</a>. 
        /// Indicates how the component will dock in its parent.
        /// </summary>
        [Parameter]
        public Dock? Dock { get; set; } = null;

        public string Id { get; set; }

        internal List<ClearComponentBase> Children { get; set; } = new List<ClearComponentBase>();

        internal double ActualHeight { get; set; } = 0;
        internal double ActualWidth { get; set; } = 0;
        internal double Top { get; private set; } = 0;
        internal double Left { get; private set; } = 0;
        internal Size DesiredSize { get; private set; }
        internal double ContentWidth { get; set; } = 0;
        internal double ContentHeight { get; set; } = 0;
        internal double ContentLeft { get; set; } = 0;
        internal double ContentTop { get; set; } = 0;

        internal Thickness _borderThickness;
        internal Thickness _marginThickness;
        internal Thickness _paddingThickness;
        static internal SKCanvas? DrawingCanvas { get; set; } = null;
        public ClearComponentBase()
        {
            Id = GetType().Name + "-" + Guid.NewGuid().ToString();
        }

        protected override void OnInitialized()
        {
            if (Parent != null)
                Parent.AddChild(this);

            base.OnInitialized();
        }

        protected virtual void AddChild(ClearComponentBase child)
        {
            Children.Add(child);
        }
        
        protected virtual void RemoveChild(ClearComponentBase child)
        {
            Children.Remove(child);
        }

        protected string GetContentDivStyle()
        {
            return $"position:absolute; top:{ContentTop}px; left:{ContentLeft}px; " +
                   $"width:{ContentWidth}px;height:{ContentHeight}px;";
        }

        protected abstract Size MeasureOverride(Size availableSize);
        protected abstract Size ArrangeOverride(Size finalSize);

        internal void Measure(Size availableSize)
        {
            if (this is IBorder)
            {
                IBorder border = (IBorder)this;
                if (border.BorderThickness != null)
                {
                    _borderThickness = Thickness.Parse(border.BorderThickness);
                    availableSize.Width -= _borderThickness.Left + _borderThickness.Right;
                    availableSize.Height -= _borderThickness.Top + _borderThickness.Bottom;
                    Left = _borderThickness.Left;
                    Top = _borderThickness.Top;
                }
            }
            _marginThickness = Thickness.Parse(Margin);
            availableSize.Width -= _marginThickness.Left + _marginThickness.Right;
            availableSize.Height -= _marginThickness.Top + _marginThickness.Bottom;

            _paddingThickness = Thickness.Parse(Padding);
            availableSize.Width -= _paddingThickness.Left + _paddingThickness.Right;
            availableSize.Height -= _paddingThickness.Top + _paddingThickness.Bottom;

            DesiredSize = MeasureOverride(availableSize);
        }

        internal void Arrange(Rect finalRect)
        {
            ContentTop = finalRect.Top;
            ContentLeft = finalRect.Left;
            ContentWidth = finalRect.Width;
            ContentHeight = finalRect.Height;

            Size finalSize = new Size(finalRect.Width, finalRect.Height);
            var actualSize = ArrangeOverride(finalSize);
        }

        internal virtual void PaintCanvas(SKCanvas canvas)
        {
            foreach(var child in Children)
                child.PaintCanvas(canvas);
        }

        Task IHandleEvent.HandleEventAsync(
                  EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

        public virtual void Dispose()
        {
            if (Parent != null)
                Parent.RemoveChild(this);
        }
    }
}
