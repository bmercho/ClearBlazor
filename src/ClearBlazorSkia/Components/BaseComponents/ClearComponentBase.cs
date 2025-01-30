using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;

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
        public Alignment HorizontalAlignment { get; set; } = Alignment.Stretch;

        /// <summary>
        /// The vertical alignment of the component in its available space.
        /// </summary>
        [Parameter]
        public Alignment VerticalAlignment { get; set; } = Alignment.Stretch;

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

        private Size _availableSize = new Size(0, 0);

//        internal double ContentWidth { get; set; } = 0;
//        internal double ContentHeight { get; set; } = 0;
//        internal double ContentLeft { get; set; } = 0;
//        internal double ContentTop { get; set; } = 0;

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
            return $"position:absolute; top:{Top}px; left:{Left}px; " +
                   $"width:{ActualWidth}px;height:{ActualHeight}px;";
        }

        protected void PerformLayout(double width, double height)
        {
            Measure(new Size(width, height));
            Arrange(0, 0);
        }

        protected abstract Size MeasureOverride(Size availableSize);
        protected abstract void ArrangeOverride(double left, double top);

        internal void Measure(Size availableSize)
        {
            _marginThickness = Thickness.Parse(Margin);
            if (double.IsPositiveInfinity(Width))
                availableSize.Width = availableSize.Width - _marginThickness.Left - _marginThickness.Right;
            else
                availableSize.Width = Width;

            if (double.IsPositiveInfinity(Height))
                availableSize.Height = availableSize.Height - _marginThickness.Top - _marginThickness.Bottom;
            else
                availableSize.Height = Height;

            if (availableSize.Width > MaxWidth)
                availableSize.Width = MaxWidth;
            if (availableSize.Height > MaxHeight)
                availableSize.Height = MaxHeight;
            if (availableSize.Width < MinWidth)
                availableSize.Width = MinWidth;
            if (availableSize.Height < MinHeight)
                availableSize.Height = MinHeight;

            if (!double.IsPositiveInfinity(Width))
                availableSize.Width = Width;
            if (!double.IsPositiveInfinity(Height))
                availableSize.Height = Height;

            if (this is IBorder)
            {
                IBorder border = (IBorder)this;
                if (border.BorderThickness != null)
                    _borderThickness = Thickness.Parse(border.BorderThickness);
            }

            _paddingThickness = Thickness.Parse(Padding);

            Size _desiredSize = MeasureOverride(availableSize);

            if (_desiredSize.Width > MaxWidth)
                _desiredSize.Width = MaxWidth;
            if (_desiredSize.Height > MaxHeight)
                _desiredSize.Height = MaxHeight;
            if (_desiredSize.Width < MinWidth)
                _desiredSize.Width = MinWidth;
            if (_desiredSize.Height < MinHeight)
                _desiredSize.Height = MinHeight;
            if (HorizontalAlignment != Alignment.Stretch)
                _desiredSize.Width = Math.Max(0.0, _desiredSize.Width);
            else
                _desiredSize.Width = availableSize.Width;
            if (VerticalAlignment != Alignment.Stretch)
                _desiredSize.Height = Math.Max(0.0, _desiredSize.Height);
            else
                _desiredSize.Height = availableSize.Height;

            DesiredSize = _desiredSize;
        }

        internal void Arrange(double left, double top)
        {
            var availableWidth = DesiredSize.Width;
            if (Parent != null)
            {
                availableWidth = Parent._availableSize.Width;
            }
            var availableHeight = DesiredSize.Height;
            if (Parent != null)
            {
                availableHeight = Parent._availableSize.Height;
            }

            if (double.IsPositiveInfinity(Width))
            {
                switch (HorizontalAlignment)
                {
                    case Alignment.Stretch:
                        Left = left + _marginThickness.Left;
                        break;
                    case Alignment.Start:
                        Left = left + _marginThickness.Left;
                        break;
                    case Alignment.Center:
                        Left = left  + availableWidth / 2 - DesiredSize.Width / 2;
                        break;
                    case Alignment.End:
                        Left = left + availableWidth - DesiredSize.Width - _marginThickness.Right;
                        break;
                }
            }
            else
            {
                switch (HorizontalAlignment)
                {
                    case Alignment.Stretch:
                        Left = left + _marginThickness.Left +
                               availableWidth / 2 - DesiredSize.Width / 2;
                        break;
                    case Alignment.Start:
                        Left = left + _marginThickness.Left;
                        break;
                    case Alignment.Center:
                        Left = left + _marginThickness.Left + 
                               DesiredSize.Width/2 - availableWidth / 2;
                        break;
                    case Alignment.End:
                        Left = left + _marginThickness.Left;
                        break;
                }
            }
            if (double.IsPositiveInfinity(Height))
            {
                switch (VerticalAlignment)
                {
                    case Alignment.Stretch:
                        Top = top + _marginThickness.Top;
                        break;
                    case Alignment.Start:
                        Top = top + _marginThickness.Top;
                        break;
                    case Alignment.Center:
                        Top = top + availableHeight / 2 - DesiredSize.Height / 2;
                        break;
                    case Alignment.End:
                        Top = top + availableHeight - DesiredSize.Height - _marginThickness.Bottom; 
                        break;
                }
            }
            else
            {
                switch (VerticalAlignment)
                {
                    case Alignment.Stretch:
                        Top = top + _marginThickness.Top +
                              availableHeight / 2 - DesiredSize.Height / 2;
                        break;
                    case Alignment.Start:
                        Top = top + _marginThickness.Top;
                        break;
                    case Alignment.Center:
                        Top = top + _marginThickness.Top +
                               availableHeight / 2 - DesiredSize.Height / 2;
                        break;
                    case Alignment.End:
                        Top = top + _marginThickness.Top;
                        break;
                }
            }
            ActualHeight = DesiredSize.Height;
            ActualWidth = DesiredSize.Width;
            _availableSize = DeflateSize(DeflateSize(DesiredSize, _borderThickness),
                                         _paddingThickness);

            double innerLeft = Left + _borderThickness.Left + _paddingThickness.Left;
            double innerTop = Top + _borderThickness.Top + _paddingThickness.Top;
            ArrangeOverride(innerLeft, innerTop);
        }

        /// <summary>
        /// Call to refresh the canvas
        /// </summary>
        /// <returns></returns>
        public void RefreshCanvas(SKCanvas canvas)
        {
            PaintAll(canvas);
        }

        public void PaintAll(SKCanvas canvas)
        {
            canvas.ResetMatrix();
            canvas.Translate((float)Left, (float)Top);
            var clipRect = new SKRect(0, 0, (float)ActualWidth, (float)ActualHeight);
            canvas.ClipRect(clipRect);

            DoPaint(canvas);
            foreach (var child in Children)
            {
                child.PaintAll(canvas);
            }
        }

        protected virtual void DoPaint(SKCanvas canvas)
        {
            FillBackground(canvas);

            if (this is IBorder)
                DrawBorder(canvas, (IBorder)this);
        }

        private void FillBackground(SKCanvas canvas)
        {
            if (this is IBackground)
            {
                var background = this as IBackground;
                if (background!.BackgroundColor != null)
                    canvas.Clear(background!.BackgroundColor.ToSKColor());
            }
        }

        private void DrawBorder(SKCanvas canvas, IBorder border)
        {
            SKColor borderColor;
            if (border.BorderColor == null)
                borderColor = ThemeManager.CurrentPalette.GrayLight.ToSKColor();
            else
                borderColor = border.BorderColor.ToSKColor();

            SKPaint strokePaint1 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = borderColor,
                IsAntialias = false,
                StrokeWidth = (float)_borderThickness.Top
            };
            if (_borderThickness.Top > 0)
                canvas.DrawLine(0,
                                (float)(_borderThickness.Top / 2),
                                (float)(ActualWidth),
                                (float)(_borderThickness.Top / 2), 
                                strokePaint1);
            strokePaint1.StrokeWidth = (float)_borderThickness.Right;
            if (_borderThickness.Right > 0)
                canvas.DrawLine((float)(ActualWidth - _borderThickness.Right / 2),
                                0,
                                (float)(ActualWidth - _borderThickness.Right / 2),
                                (float)ActualHeight, 
                                strokePaint1);
            strokePaint1.StrokeWidth = (float)_borderThickness.Bottom;
            if (_borderThickness.Bottom > 0)
                canvas.DrawLine(0,
                                (float)(ActualHeight -_borderThickness.Bottom/2),
                                (float)(ActualWidth),
                                (float)(ActualHeight - _borderThickness.Bottom / 2), 
                                strokePaint1);
            strokePaint1.StrokeWidth = (float)_borderThickness.Left;
            if (_borderThickness.Left > 0)
                canvas.DrawLine((float)(_borderThickness.Left / 2),
                                0,
                                (float)(_borderThickness.Left / 2),
                                (float)ActualHeight,
                                strokePaint1);

        }

        protected static Rect DeflateRect(Rect rt, Thickness thick)
        {
            return new Rect(rt.Left + thick.Left,
                            rt.Top + thick.Top,
                            Math.Max(0.0, rt.Width - thick.Left - thick.Right),
                            Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
        }

        protected static Size DeflateSize(Size size, Thickness thick)
        {
            return new Size(Math.Max(0.0, size.Width - thick.Left - thick.Right),
                            Math.Max(0.0, size.Height - thick.Top - thick.Bottom));
        }

        protected static Size HelperCollapseThickness(Thickness th)
        {
            return new Size(th.Left + th.Right, th.Top + th.Bottom);
        }

        protected static Rect HelperDeflateRect(Rect rt, Thickness thick)
        {
            return new Rect(rt.Left + thick.Left,
                            rt.Top + thick.Top,
                            Math.Max(0.0, rt.Width - thick.Left - thick.Right),
                            Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
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
