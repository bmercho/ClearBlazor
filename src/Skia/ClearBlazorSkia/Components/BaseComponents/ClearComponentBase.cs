using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        public double Width { get; set; } = double.NaN;

        /// <summary>
        /// The height of the component. Takes precedence over other layout requirements.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = double.NaN;

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

        /// <summary>
        /// Event raised when the canvas is clicked
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Event raised when the canvas is clicked
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnDblClick { get; set; }

        /// <summary>
        /// Event raised when the mouse down event occurs on the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnMouseDown { get; set; }

        /// <summary>
        /// Event raised when the mouse is moved over the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnMouseMove { get; set; }

        /// <summary>
        /// Event raised when the mouse up event occurs on the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnMouseUp { get; set; }

        /// <summary>
        /// Event raised when the mouse moves onto a canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnMouseOver { get; set; }

        /// <summary>
        /// Event raised when the mouse moves out of a canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnMouseOut { get; set; }

        /// <summary>
        /// Event raised when the a context menu should be shown
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnContextMenu { get; set; }

        /// <summary>
        /// Event raised when the mouse wheel is rolled
        /// </summary>
        [Parameter]
        public EventCallback<ComponentMouseEventArgs> OnMouseWheel { get; set; }

        /// <summary>
        /// Event raised when the pointer down event occurs
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerDown { get; set; }

        /// <summary>
        /// Event raised when the pointer up event occurs
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerUp { get; set; }

        /// <summary>
        /// Event raised when the pointer enters the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerEnter { get; set; }

        /// <summary>
        /// Event raised when the pointer leaves the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerLeave { get; set; }

        /// <summary>
        /// Event raised when the pointer moves
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerMove { get; set; }

        /// <summary>
        /// Event raised when the pointer moves out of the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerOut { get; set; }

        /// <summary>
        /// Event raised when the pointer moves over of the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerOver { get; set; }

        /// <summary>
        /// Event raised when the pointer is cancelled
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnPointerCancel { get; set; }

        /// <summary>
        /// Event raised when pointer capture has been obtained
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnGotPointerCapture { get; set; }

        /// <summary>
        /// Event raised when pointer capture has been lost
        /// </summary>
        [Parameter]
        public EventCallback<ComponentPointerEventArgs> OnLostPointerCapture { get; set; }

        /// <summary>
        /// Event raised when a finger is dragged across the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentTouchEventArgs> OnTouchMove { get; set; }

        /// <summary>
        /// Event raised when a finger is placed on the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentTouchEventArgs> OnTouchStart { get; set; }

        /// <summary>
        /// Event raised when a finger is removed from the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentTouchEventArgs> OnTouchEnd { get; set; }

        /// <summary>
        /// Event raised when a finger enters the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentTouchEventArgs> OnTouchEnter { get; set; }

        /// <summary>
        /// Event raised when a finger leaves the canvas
        /// </summary>
        [Parameter]
        public EventCallback<ComponentTouchEventArgs> OnTouchLeave { get; set; }

        /// <summary>
        /// Event raised when a finger event is cancelled
        /// </summary>
        [Parameter]
        public EventCallback<ComponentTouchEventArgs> OnTouchCancel { get; set; }

        public string Id { get; set; }

        internal List<ClearComponentBase> Children { get; set; } = new List<ClearComponentBase>();

        internal double ActualHeight => _renderSize.Height;
        internal double ActualWidth => _renderSize.Width;
        internal double Top { get; set; } = 0;
        internal double Left { get; set; } = 0;
        internal Size DesiredSize { get; set; }
        internal Size AvailableSize { get; set; }

        internal Size ContentSize { get; set; } = new Size();

        internal double _prevActualHeight = 0;
        internal double _prevActualWidth = 0;
        internal double _prevTop = 0; 
        internal double _prevLeft = 0;
        private double _absoluteTop = 0;
        private double _absoluteLeft = 0;


        internal Size? _unclippedDesiredSizeField = null;

        internal Size _renderSize = new Size(0, 0);

        //        internal double ContentWidth { get; set; } = 0;
        //        internal double ContentHeight { get; set; } = 0;
        //        internal double ContentLeft { get; set; } = 0;
        //        internal double ContentTop { get; set; } = 0;

        internal Thickness _borderThickness;
        internal Thickness _marginThickness;
        internal Thickness _paddingThickness;

        protected static bool _isVisualTreeDirty = false;
        protected bool _isDirty = false;


        static double _xOffset = 0;
        static double _yOffset = 0;

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

        protected void PerformLayout(double width, double height)
        {
            Measure(new Size(width, height));
            Arrange(new Rect(0, 0, width, height));
            CalculateAbsolutePositions(this, 0, 0);
            if (ActualHeight != _prevActualHeight ||
                ActualWidth != _prevActualWidth ||
                Top != _prevTop ||
                Left != _prevLeft)
            {
                _isVisualTreeDirty = true;
                _prevActualHeight = ActualHeight;
                _prevActualWidth = ActualWidth;
                _prevTop = Top;
                _prevLeft = Left;
            }
        }
        protected virtual Size MeasureOverride(Size availableSize)
        {
            return new Size(0, 0);
        }
        protected virtual Size ArrangeOverride(Size finalSize, 
                                               double offsetHeight,
                                               double offsetWidth)
        {
            return finalSize;
        }

        internal void Measure(Size availableSize)
        {
            _marginThickness = Thickness.Parse(Margin);
            _paddingThickness = Thickness.Parse(Padding);
            if (this is IBorder)
            {
                IBorder border = (IBorder)this;
                if (border.BorderThickness != null)
                    _borderThickness = Thickness.Parse(border.BorderThickness);
            }

            //enforce that Measure can not receive NaN size .
            if (DoubleUtils.IsNaN(availableSize.Width) || DoubleUtils.IsNaN(availableSize.Height))
                throw new InvalidOperationException("AvailableSize.Width or availableSize.Height cannot be NaN.");

            Size prevSize = DesiredSize;

            Size desiredSize = new Size(0, 0);

            desiredSize = MeasureCore(availableSize);

            //enforce that MeasureCore can not return PositiveInfinity size even if given Infinite available size.
            //Note: NegativeInfinity can not be returned by definition of Size structure.
            if (double.IsPositiveInfinity(desiredSize.Width) || double.IsPositiveInfinity(desiredSize.Height))
                throw new InvalidOperationException("MeasureCore cannot return PositiveInfinity");

            //enforce that MeasureCore can not return NaN size .
            if (DoubleUtils.IsNaN(desiredSize.Width) || DoubleUtils.IsNaN(desiredSize.Height))
                throw new InvalidOperationException("MeasureCore cannot return NaN");

            //cache desired size
            DesiredSize = desiredSize;

        }

        protected Size MeasureCore(Size availableSize)
        {
            double marginWidth = _marginThickness.Left + _marginThickness.Right;
            double marginHeight = _marginThickness.Top + _marginThickness.Bottom;

            //  parent size is what parent want us to be
            Size frameworkAvailableSize = new Size(
                          Math.Max(availableSize.Width - marginWidth, 0),
                          Math.Max(availableSize.Height - marginHeight, 0));

            MinMax mm = new MinMax(this);

            frameworkAvailableSize.Width = Math.Max(mm.minWidth, Math.Min(frameworkAvailableSize.Width, mm.maxWidth));
            frameworkAvailableSize.Height = Math.Max(mm.minHeight, Math.Min(frameworkAvailableSize.Height, mm.maxHeight));


            Size border = HelperCollapseThickness(_borderThickness);
            Size padding = HelperCollapseThickness(_paddingThickness);

            Size combined = new Size(border.Width + padding.Width, border.Height + padding.Height);
            frameworkAvailableSize.Width =
                Math.Max(0.0, frameworkAvailableSize.Width - combined.Width);
            frameworkAvailableSize.Height =
                Math.Max(0.0, frameworkAvailableSize.Height - combined.Height);

            //  call to specific layout to measure
            Size desiredSize = MeasureOverride(frameworkAvailableSize);

            //  maximize desiredSize with user provided min size
            desiredSize = new Size(
                Math.Max(desiredSize.Width, mm.minWidth),
                Math.Max(desiredSize.Height, mm.minHeight));

            //here is the "true minimum" desired size - the one that is
            //for sure enough for the control to render its content.
            Size unclippedDesiredSize = desiredSize;

            bool clipped = false;

            // User-specified max size starts to "clip" the control here.
            //Starting from this point desiredSize could be smaller then actually
            //needed to render the whole control
            if (desiredSize.Width > mm.maxWidth)
            {
                desiredSize.Width = mm.maxWidth;
                clipped = true;
            }

            if (desiredSize.Height > mm.maxHeight)
            {
                desiredSize.Height = mm.maxHeight;
                clipped = true;
            }

            //  because of negative margins, clipped desired size may be negative.
            //  need to keep it as doubles for that reason and maximize with 0 at the
            //  very last point - before returning desired size to the parent.
            double clippedDesiredWidth = desiredSize.Width + marginWidth;
            double clippedDesiredHeight = desiredSize.Height + marginHeight;

            // In over-constrained scenario, parent wins and measured size of the child,
            // including any sizes set or computed, can not be larger then
            // available size. We will clip the guy later.
            if (clippedDesiredWidth > availableSize.Width)
            {
                clippedDesiredWidth = availableSize.Width;
                clipped = true;
            }

            if (clippedDesiredHeight > availableSize.Height)
            {
                clippedDesiredHeight = availableSize.Height;
                clipped = true;
            }

            //  Note: unclippedDesiredSize is needed in ArrangeCore,
            //  because due to the layout protocol, arrange should be called
            //  with constraints greater or equal to child's desired size
            //  returned from MeasureOverride. But in most circumstances
            //  it is possible to reconstruct original unclipped desired size.
            //  In such cases we want to optimize space and save 16 bytes by
            //  not storing it on each FrameworkElement.
            //
            //  The if statement conditions below lists the cases when
            //  it is NOT possible to recalculate unclipped desired size later
            //  in ArrangeCore, thus we save it into Uncommon Fields...
            //
            Size? sb = _unclippedDesiredSizeField;
            if (clipped
                || clippedDesiredWidth < 0
                || clippedDesiredHeight < 0)
            {
                if (sb == null) //not yet allocated, allocate the box
                    _unclippedDesiredSizeField = new Size(unclippedDesiredSize.Width, unclippedDesiredSize.Height);
                else //we already have allocated size box, simply change it
                {
                    Size box = (Size)sb;
                    box.Width = unclippedDesiredSize.Width;
                    box.Height = unclippedDesiredSize.Height;
                    _unclippedDesiredSizeField = sb;
                }
            }
            else
            {
                if (_unclippedDesiredSizeField != null)
                {
                    Size box = (Size)_unclippedDesiredSizeField;
                    box.Width = 0;
                    box.Height = 0;
                    _unclippedDesiredSizeField = box;
                }
            }

            clippedDesiredWidth += combined.Width;
            clippedDesiredHeight += combined.Height;

            return new Size(Math.Max(0, clippedDesiredWidth), Math.Max(0, clippedDesiredHeight));
        }

        internal void Arrange(Rect finalRect)
        {
            //enforce that Arrange can not come with Infinity size or NaN
            if (double.IsPositiveInfinity(finalRect.Width)
                || double.IsPositiveInfinity(finalRect.Height)
                || DoubleUtils.IsNaN(finalRect.Width)
                || DoubleUtils.IsNaN(finalRect.Height))
                throw new InvalidOperationException("Arrange cannot have with Infinity size or NaN");

            //This has to update RenderSize
            ArrangeCore(finalRect);
        }

        internal void ArrangeCore(Rect finalRect)
        {
            Size arrangeSize = new Size(finalRect.Width, finalRect.Height);

            double marginWidth = _marginThickness.Left + _marginThickness.Right;
            double marginHeight = _marginThickness.Top + _marginThickness.Bottom;

            arrangeSize.Width = Math.Max(0, arrangeSize.Width - marginWidth);
            arrangeSize.Height = Math.Max(0, arrangeSize.Height - marginHeight);

            Size? sb = _unclippedDesiredSizeField;
            Size unclippedDesiredSize;
            if (sb == null)
            {
                unclippedDesiredSize = new Size(Math.Max(0, this.DesiredSize.Width - marginWidth),
                                                Math.Max(0, this.DesiredSize.Height - marginHeight));

            }
            else
            {
                Size box = (Size)sb;
                unclippedDesiredSize = new Size(box.Width, box.Height);
            }

            if (DoubleUtils.LessThan(arrangeSize.Width, unclippedDesiredSize.Width))
            {
                arrangeSize.Width = unclippedDesiredSize.Width;
            }

            if (DoubleUtils.LessThan(arrangeSize.Height, unclippedDesiredSize.Height))
            {
                arrangeSize.Height = unclippedDesiredSize.Height;
            }

            // Alignment==Stretch --> arrange at the slot size minus margins
            // Alignment!=Stretch --> arrange at the unclippedDesiredSize
            if (HorizontalAlignment != Alignment.Stretch)
            {
                arrangeSize.Width = unclippedDesiredSize.Width;
            }

            if (VerticalAlignment != Alignment.Stretch)
            {
                arrangeSize.Height = unclippedDesiredSize.Height;
            }

            MinMax mm = new MinMax(this);

            //we have to choose max between UnclippedDesiredSize and Max here, because
            //otherwise setting of max property could cause arrange at less then unclippedDS.
            //Clipping by Max is needed to limit stretch here
            double effectiveMaxWidth = Math.Max(unclippedDesiredSize.Width, mm.maxWidth);
            if (DoubleUtils.LessThan(effectiveMaxWidth, arrangeSize.Width))
            {
                arrangeSize.Width = effectiveMaxWidth;
            }

            double effectiveMaxHeight = Math.Max(unclippedDesiredSize.Height, mm.maxHeight);
            if (DoubleUtils.LessThan(effectiveMaxHeight, arrangeSize.Height))
            {
                arrangeSize.Height = effectiveMaxHeight;
            }

            Size oldRenderSize = _renderSize;

            Size border = HelperCollapseThickness(_borderThickness);
            Size padding = HelperCollapseThickness(_paddingThickness);

            Size combined = new Size(border.Width + padding.Width, border.Height + padding.Height);

            arrangeSize = new Size(Math.Max(0.0, arrangeSize.Width - combined.Width),
                                  Math.Max(0.0, arrangeSize.Height - combined.Height));

            Size innerInkSize = ArrangeOverride(arrangeSize,
                                                _borderThickness.Top + _paddingThickness.Top,
                                                _borderThickness.Left + _paddingThickness.Left);

            innerInkSize.Width += combined.Width;
            innerInkSize.Height += combined.Height;

            //Here we use un-clipped InkSize because element does not know that it is
            //clipped by layout system and it should have as much space to render as
            //it returned from its own ArrangeOverride
            _renderSize = innerInkSize;


            //clippedInkSize differs from InkSize only what MaxWidth/Height explicitly clip the
            //otherwise good arrangement. For ex, DS<clientSize but DS>MaxWidth - in this
            //case we should initiate clip at MaxWidth and only show Top-Left portion
            //of the element limited by Max properties. It is Top-left because in case when we
            //are clipped by container we also degrade to Top-Left, so we are consistent.
            Size clippedInkSize = new Size(Math.Min(innerInkSize.Width, mm.maxWidth),
                                           Math.Min(innerInkSize.Height, mm.maxHeight));

            // The client size is the size of layout slot decreased by margins.
            // This is the "window" through which we see the content of the child.
            // Alignments position ink of the child in this "window".
            // Max with 0 is necessary because layout slot may be smaller then unclipped desired size.
            Size clientSize = new Size(Math.Max(0, finalRect.Width - marginWidth),
                                    Math.Max(0, finalRect.Height - marginHeight));

            Vector offset = ComputeAlignmentOffset(clientSize, clippedInkSize);

            offset.X += finalRect.Left + _marginThickness.Left;
            offset.Y += finalRect.Top + _marginThickness.Top;

            Left = offset.X;
            Top = offset.Y;
        }

        private Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
        {
            Vector offset = new Vector();

            Alignment ha = HorizontalAlignment;
            Alignment va = VerticalAlignment;

            //this is to degenerate Stretch to Top-Left in case when clipping is about to occur
            //if we need it to be Center instead, simply remove these 2 ifs
            if (ha == Alignment.Stretch
                && inkSize.Width > clientSize.Width)
            {
                ha = Alignment.Start;
            }

            if (va == Alignment.Stretch
                && inkSize.Height > clientSize.Height)
            {
                va = Alignment.Start;
            }
            //end of degeneration of Stretch to Top-Left

            if (ha == Alignment.Center
                || ha == Alignment.Stretch)
            {
                offset.X = (clientSize.Width - inkSize.Width) * 0.5;
            }
            else if (ha == Alignment.End)
            {
                offset.X = clientSize.Width - inkSize.Width;
            }
            else
            {
                offset.X = 0;
            }

            if (va == Alignment.Center
                || va == Alignment.Stretch)
            {
                offset.Y = (clientSize.Height - inkSize.Height) * 0.5;
            }
            else if (va == Alignment.End)
            {
                offset.Y = clientSize.Height - inkSize.Height;
            }
            else
            {
                offset.Y = 0;
            }

            return offset;
        }

        private void CalculateAbsolutePositions(ClearComponentBase component,
                                                double left, double top)
        {
            component._absoluteLeft = component.Left + left;
            component._absoluteTop = component.Top + top;
            foreach (var child in component.Children)
                CalculateAbsolutePositions(child, _absoluteLeft, _absoluteTop);
        }

        protected virtual void AddChild(ClearComponentBase child)
        {
            Children.Add(child);
        }
        
        protected virtual void RemoveChild(ClearComponentBase child)
        {
            Children.Remove(child);
        }

        //protected string GetContentDivStyle()
        //{
        //    return $"position:absolute; top:{Top}px; left:{Left}px; " +
        //           $"width:{ActualWidth}px;height:{ActualHeight}px;";
        //}


        /// <summary>
        /// Call to refresh the canvas
        /// </summary>
        /// <returns></returns>
        public void RefreshCanvas(SKCanvas canvas)
        {
            canvas.ResetMatrix();
            canvas.Clear();
            PaintAll(canvas);
        }

        public void PaintAll(SKCanvas canvas)
        {
            canvas.ResetMatrix();
            canvas.Save();
            _xOffset += Left;
            _yOffset += Top;
            //double yOffset = 0;
            //double xOffset = 0;
            //if (Parent != null && Parent.Parent != null && Parent.Parent.Parent != null &&
            //    Parent.Parent.Parent is ScrollViewer)
            //{
            //    yOffset = 200;
            //    xOffset = 0;
            //    _yOffset += yOffset;
            //    _xOffset += xOffset; 
            //}

            canvas.Translate((float)_xOffset, (float)_yOffset);
            var clipRect = new SKRect(0, 0, (float)ActualWidth, (float)ActualHeight);
            canvas.ClipRect(clipRect);
            DoPaint(canvas);
            foreach (var child in Children)
            {
                child.PaintAll(canvas);
            }
            _xOffset -= Left;// +xOffset;
            _yOffset -= Top;// +yOffset;
            canvas.Restore();
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
                {
                    SKPaint fillPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = background!.BackgroundColor.ToSKColor()
                    };
                    canvas.DrawRect(0, 0, (float)ActualWidth, (float)ActualHeight, fillPaint);
                    //canvas.Clear(background!.BackgroundColor.ToSKColor());
                }
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

        
        protected async Task HandleEvent(EventUtils.DomEvent domEvent, EventArgs args)
        {
            if (EventUtils.IsMouseEvent(domEvent))
                await HandleMouseEvent((EventUtils.MouseEvent)domEvent,
                                       args as MouseEventArgs, false);
            else if (EventUtils.IsKeyboardEvent(domEvent))
                await HandleKeyboardEvent((EventUtils.KeyboardEvent)domEvent,
                                          args as KeyboardEventArgs, false);
            else if (EventUtils.IsPointerEvent(domEvent))
                await HandlePointerEvent((EventUtils.PointerEvent)domEvent,
                                          args as PointerEventArgs, false);
        }

        protected async Task<bool> HandleMouseEvent(EventUtils.MouseEvent mouseEvent, 
                                              MouseEventArgs? args, bool handled)
        {
            if (args == null)
                return false;

            if (InComponent(args.OffsetX, args.OffsetY))
            {
                foreach (var child in Children)
                    handled = await child.HandleMouseEvent(mouseEvent, args, handled);

                if (!handled)
                {
                    var e = new ComponentMouseEventArgs(args, this);
                    e.Handled = handled;

                    switch (mouseEvent)
                    {
                        case EventUtils.MouseEvent.MouseOver:
                            await OnMouseOver.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.MouseOut:
                            await OnMouseOut.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.MouseMove:
                            await OnMouseMove.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.MouseDown:
                            await OnMouseDown.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.MouseUp:
                            await OnMouseUp.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.Click:
                            await OnClick.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.DblClick:
                            await OnDblClick.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.ContextMenu:
                            await OnContextMenu.InvokeAsync(e);
                            break;
                        case EventUtils.MouseEvent.Wheel:
                            break;
                        case EventUtils.MouseEvent.MouseWheel:
                            await OnMouseWheel.InvokeAsync(e);
                            break;
                    }
                    return e.Handled;
                }
            }
            return handled;
        }
        protected async Task<bool> HandleKeyboardEvent(EventUtils.KeyboardEvent keyboardEvent,
                                              KeyboardEventArgs? args, bool handled)
        {
            if (args == null)
                return false;

            switch (keyboardEvent)
            {
                case EventUtils.KeyboardEvent.KeyDown:
                    //await OnMouseWheel.InvokeAsync(e);
                    break;
                case EventUtils.KeyboardEvent.KeyUp:
                    break;
                case EventUtils.KeyboardEvent.KeyPress:
                    break;
            }
            await Task.CompletedTask;
            return false;

        }

        protected async Task<bool> HandlePointerEvent(EventUtils.PointerEvent pointerEvent,
                                                      PointerEventArgs? args, bool handled)
        {
            if (args == null)
                return false;

            if (InComponent(args.OffsetX, args.OffsetY))
            {
                foreach (var child in Children)
                    handled = await child.HandlePointerEvent(pointerEvent, args, handled);

                if (!handled)
                {
                    var e = new ComponentPointerEventArgs(args, this);
                    e.Handled = handled;

                    switch (pointerEvent)
                    {
                        case EventUtils.PointerEvent.PointerCancel:
                            await OnPointerCancel.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.PointerDown:
                            await OnPointerDown.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.PointerEnter:
                            await OnPointerEnter.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.PointerLeave:
                            await OnPointerLeave.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.PointerMove:
                            await OnPointerMove.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.PointerOut:
                            await OnPointerOut.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.PointerUp:
                            await OnPointerUp.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.GotPointerCapture:
                            await OnGotPointerCapture.InvokeAsync(e);
                            break;
                        case EventUtils.PointerEvent.LostPointerCapture:
                            await OnLostPointerCapture.InvokeAsync(e);
                            break;
                    }
                    return e.Handled;
                }
            }
            return handled;
        }

        //protected async Task HandleMouseEvent(EventUtils.MouseEvent, MouseEventArgs args)
        //{
        //    await HandleMouseMove(args, false);
        //}

        //protected async Task HandleMouseMove(MouseEventArgs args, bool handled)
        //{

        //    if (InComponent(args.OffsetX, args.OffsetY))
        //    {
        //        foreach (var child in Children)
        //        {
        //            await child.HandleMouseMove(args, handled);
        //        }
        //        if (!handled)
        //        {
        //            var e = new ComponentMouseEventArgs(args, this);
        //            e.Handled = handled;
        //            await OnMouseMove.InvokeAsync(e);
        //            handled = e.Handled;
        //        }
        //    }
        //}

        private bool InComponent(double offsetX, double offsetY)
        {
            if (offsetX >= _absoluteLeft && offsetX <= _absoluteLeft + ActualWidth &&
                offsetY >= _absoluteTop && offsetY <= _absoluteTop + ActualHeight)
                return true;
            return false;
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

        private struct MinMax
        {
            internal MinMax(ClearComponentBase e)
            {
                maxHeight = e.MaxHeight;
                minHeight = e.MinHeight;
                double l = e.Height;

                double height = (DoubleUtils.IsNaN(l) ? Double.PositiveInfinity : l);
                maxHeight = Math.Max(Math.Min(height, maxHeight), minHeight);

                height = (DoubleUtils.IsNaN(l) ? 0 : l);
                minHeight = Math.Max(Math.Min(maxHeight, height), minHeight);

                maxWidth = e.MaxWidth;
                minWidth = e.MinWidth;
                l = e.Width;

                double width = (DoubleUtils.IsNaN(l) ? Double.PositiveInfinity : l);
                maxWidth = Math.Max(Math.Min(width, maxWidth), minWidth);

                width = (DoubleUtils.IsNaN(l) ? 0 : l);
                minWidth = Math.Max(Math.Min(maxWidth, width), minWidth);
            }

            internal double minWidth;
            internal double maxWidth;
            internal double minHeight;
            internal double maxHeight;
        }

    }
}
