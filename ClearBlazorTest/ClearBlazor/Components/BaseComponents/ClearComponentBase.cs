using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text;

namespace ClearBlazor
{
    /// <summary>
    /// An abstract base class for all components.
    /// </summary>
    public abstract class ClearComponentBase : ComponentBase, IDisposable, IHandleEvent
    {
        private bool _preventFromRecursiveSetParameters;

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
        /// Optional classes to be applied to component
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Optional styles to be applied to the component
        /// </summary>
        [Parameter]
        public string Style { get; set; }

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
        public Alignment? HorizontalAlignment { get; set; } = null;

        /// <summary>
        /// The vertical alignment of the component in its available space.
        /// </summary>
        [Parameter]
        public Alignment? VerticalAlignment { get; set; } = null;


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
        /// Event raised when the component is clicked 
        /// </summary>
        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnClicked { get; set; }

        /// <summary>
        /// Event raised when the component is double clicked 
        /// </summary>
        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnDoubleClicked { get; set; }

        /// <summary>
        /// Event raised when the mouse is moved over the component 
        /// </summary>
        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnMouseMoved { get; set; }

        public string Id { get; set; }

        protected ElementSizeInfo? SizeInfo { get; private set; } = null;
        protected ElementSizeInfo? ParentSizeInfo { get; private set; } = null;

        internal Alignment? HorizontalAlignmentDefaultOverride { get; set; } = null;

        internal Alignment? VerticalAlignmentDefaultOverride { get; set; } = null;

        internal List<ClearComponentBase> Children { get; set; } = new List<ClearComponentBase>();

        protected string Classes { get; set; }

        protected bool IsScroller { get; set; } = false;

        private bool _doubleClickRaised = false;

        private ElementSizeInfo? previousSizeInfo = null;

        private ElementSizeInfo? previousParentSizeInfo = null;

        public ClearComponentBase()
        {
            Class = String.Empty;
            Style = String.Empty;

            Classes = String.Empty;

            Id = GetType().Name + "-" + Guid.NewGuid().ToString();
        }

        protected override void OnInitialized()
        {
            if (Parent != null)
                Parent.AddChild(this);

            base.OnInitialized();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            Logger.AddLog($"SetParametersAsync: {Id}");
            foreach (var parameter in parameters)
            {
                if (parameter.Name == "Data")
                {
                    var d = parameter.Value;
                }

            }

            if (!HaveParametersChanged(parameters))
                return;

            // Make sure we won't SetParameters recursively (through the Parent.DockPanelChanged call).
            if (_preventFromRecursiveSetParameters)
            {
                return;
            }

            parameters.TryGetValue<ClearComponentBase>(nameof(Parent), out var parent);

            bool paramsChanged = parent != null && parent.HaveParametersChanged(this, parameters);

            // first set parameters
            await base.SetParametersAsync(parameters).ConfigureAwait(false);

            if (Parent != null)
            {
                if (paramsChanged)
                {
                    _preventFromRecursiveSetParameters = true;
                    Parent.StateHasChanged();
                    _preventFromRecursiveSetParameters = false;
                }
            }


            if (this is IComponentSize)
            {
                SizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", Id);

                if (previousSizeInfo == null ||
                    !previousSizeInfo.Equals(SizeInfo))
                {
                    previousSizeInfo = SizeInfo;
                    StateHasChanged();
                }
            }

            if (this is IParentComponentSize && Parent != null)
            {
                ParentSizeInfo = await JSRuntime.InvokeAsync<ElementSizeInfo>("GetElementSizeInfoById", Parent.Id);

                if (previousParentSizeInfo == null ||
                    !previousParentSizeInfo.Equals(ParentSizeInfo))
                {
                    previousParentSizeInfo = ParentSizeInfo;
                    StateHasChanged();
                }
            }

            UpdateClasses();

        }

        protected virtual bool HaveParametersChanged(ClearComponentBase child, ParameterView parameters)
        {
            return false;
        }

        protected virtual bool HaveParametersChanged(ParameterView parameters)
        {
            return true;
        }

        protected T? FindParent<T>(ClearComponentBase? Parent) where T : ClearComponentBase
        {
            ClearComponentBase? parent = Parent;
            while (parent != null && parent.GetType() != typeof(T))
                parent = parent.Parent;
            if (parent == null)
                return null;
            else
                return parent as T;
        }
        protected string ComputeStyle()
        {
            var parent = Parent;
            int level = 1;
            while (parent != null)
            {
                parent.UpdateChildParams(this, level++);
                parent = parent.Parent;
            }

            var css = string.Empty;
            css += CssBuilder();

            if (this is IBorder)
                css += CssBuilder((IBorder)this);

            if (this is IBackground)
                css += CssBuilder((IBackground)this);

            if (this is IBackgroundGradient)
                css += CssBuilder((IBackgroundGradient)this);

            if (this is IBoxShadow)
                css += CssBuilder((IBoxShadow)this);

            if (this is IDraggable)
                css += CssBuilder((IDraggable)this);

            if (this is IColor)
                SetColor((IColor)this);

            css = UpdateStyle(css);

            if (Parent != null)
                css = Parent.UpdateChildStyle(this, css);

            if (!string.IsNullOrEmpty(Style))
                css += $" {Style}";

            return css;
        }

        protected virtual string UpdateStyle(string css)
        {
            return css;
        }

        protected virtual void UpdateChildParams(ClearComponentBase child, int level)
        {
        }

        protected virtual string UpdateChildStyle(ClearComponentBase child, string css)
        {
            return css;
        }

        protected virtual void AddChild(ClearComponentBase child)
        {
            Children.Add(child);
        }

        protected async Task OnElementClicked(MouseEventArgs e)
        {
            _doubleClickRaised = false;
            await Task.Delay(250);
            if (!_doubleClickRaised)
                await OnClicked.InvokeAsync(e);
        }

        protected async Task OnElementDblClicked(MouseEventArgs e)
        {
            _doubleClickRaised = true;
            await OnDoubleClicked.InvokeAsync(e);
        }

        protected async Task OnElementMouseMoved(MouseEventArgs e)
        {
            await OnMouseMoved.InvokeAsync(e);
        }

        protected virtual void ComputeOwnClasses(StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(Class))
                sb.Append($"{Class} ");
        }

        protected virtual string ComputeChildClasses(StringBuilder sb, ClearComponentBase child)
        {
            return string.Empty;
        }

        private void UpdateClasses()
        {
            var sb = new StringBuilder();
            ComputeOwnClasses(sb);
            Parent?.ComputeChildClasses(sb, this);
            Classes = sb.ToString();
        }

        private string CssBuilder()
        {
            var css = string.Empty;
            if (!double.IsNaN(Width))
                css += $"width: {Width}px; ";

            if (!double.IsNaN(Height))
                css += $"height: {Height}px; ";

            if (MinWidth > 0)
                css += $"min-width: {MinWidth}px; ";

            if (MinHeight > 0)
                css += $"min-height: {MinHeight}px; ";

            var margin = Thickness.Parse(Margin);
            var padding = Thickness.Parse(Padding);
            if (MaxWidth == double.PositiveInfinity)
            {
                if (!double.IsNaN(Width) && MinWidth == 0)
                    css += $"min-width: {Width}px; ";
            }
            if (MaxWidth != double.PositiveInfinity)
            {
                css += $"max-width: {MaxWidth}px; ";
                if (double.IsNaN(Width))
                    css += $"width: 100%; ";
                else if (MinWidth == 0)
                    css += $"min-width: {Width}px; ";
            }
            else if (GetHorizontalAlignment() != Alignment.Stretch)
                css += $"max-width: calc(100% - {margin.HorizontalThickness}px); ";
            //else
            //    sb.Append("max-width: 100%; ");

            if (MaxHeight == double.PositiveInfinity)
            {
                if (!double.IsNaN(Height) && MinHeight == 0)
                    css += $"min-height: {Height}px; ";
            }
            if (MaxHeight != double.PositiveInfinity)
            {
                css += $"max-height: {MaxHeight}px; ";
                if (double.IsNaN(Height))
                    css += $"height: 100%; ";
                else if (MinHeight == 0)
                    css += $"min-height: {Height}px; ";
            }
            else if (GetVerticalAlignment() != Alignment.Stretch)
                css += $"max-height: calc(100% - {margin.VerticalThickness}px); ";
            //else
            //    sb.Append("max-width: 100%; ");

            if (margin != Thickness.Zero)
                css += $"margin: {margin.ThicknessToCss()}; ";

            if (padding != Thickness.Zero)
                css += $"padding: {padding.ThicknessToCss()}; ";

            // If the element cannot effectively stretch due to size constraints (Width/MaxWidth),
            // CSS will align it at 'start' instead. To get the typical XAML behavior ('center'),
            // we explicitly set the CSS alignment to 'center' instead of 'stretch' in such cases.
            var canStretchH =
                GetHorizontalAlignment() == Alignment.Stretch &&
                double.IsNaN(Width) && MaxWidth == double.PositiveInfinity;

            var canStretchV =
                GetVerticalAlignment() == Alignment.Stretch &&
                double.IsNaN(Height) && MaxHeight == double.PositiveInfinity;

            string alignSelf;
            string justifySelf;
            var stackPanel = Parent as StackPanel;
            var buttonGroup = Parent as ButtonGroup;
            if (stackPanel != null || buttonGroup != null)
            {
                Orientation orientation = Orientation.Portrait;
                if (stackPanel != null)
                    orientation = stackPanel.Orientation;
                else if (buttonGroup != null)
                    orientation = buttonGroup.Orientation;
                if (orientation == Orientation.Portrait)
                {
                    alignSelf = AlignmentToCss(GetHorizontalAlignment(), canStretchH);
                    justifySelf = AlignmentToCss(GetVerticalAlignment(), canStretchV);
                }
                else
                {
                    justifySelf = AlignmentToCss(GetHorizontalAlignment(), canStretchH);
                    alignSelf = AlignmentToCss(GetVerticalAlignment(), canStretchV);
                }
            }
            else
            {
                justifySelf = AlignmentToCss(GetHorizontalAlignment(), canStretchH);
                alignSelf = AlignmentToCss(GetVerticalAlignment(), canStretchV);
            }

            if (justifySelf != "stretch")
                css += $"justify-self: {justifySelf}; ";

            if (alignSelf != "stretch")
                css += $"align-self: {alignSelf}; ";

            if (stackPanel != null)
                css += "flex-shrink:0; ";

            // The line below affected images in a scrollviewer
            // Removing it does not seem to affect anything else
            //if (!InScroller())
            css += $"overflow:hidden; ";
            css += $"white-space: nowrap; ";

            return css;
        }

        private string CssBuilder(IBorder border)
        {
            var css = string.Empty;

            if (border.BorderThickness != null && border.BorderThickness != "0")
            {
                if (border.BorderColor != null)
                    css += $"border-color: {border.BorderColor.Value}; ";
                else
                    css += $"border-color: {ThemeManager.CurrentPalette.GrayLight.Value}; ";


                if (border.BorderThickness != null)
                {
                    var borderThickness = Thickness.Parse(border.BorderThickness);
                    if (borderThickness != Thickness.Zero)
                        css += $"border-width: {borderThickness.ThicknessToCss()}; border-style: solid;";
                }
                else
                    css += $"border-width: {new Thickness(1).ThicknessToCss()}; border-style: solid;";

                if (border.BorderStyle == null)
                    css += "border-style: solid; ";
                else
                    switch (border.BorderStyle)
                    {
                        case BorderStyle.Solid:
                            css += "border-style: solid; ";
                            break;
                        case BorderStyle.Dotted:
                            css += "border-style: dotted; ";
                            break;
                        case BorderStyle.Dashed:
                            css += "border-style: dashed; ";
                            break;
                }
            }

            if (border.CornerRadius != null)
            {
                var cornerRadius = Thickness.Parse(border.CornerRadius);

                if (cornerRadius != Thickness.Zero)
                    css += $"border-radius: {cornerRadius.ThicknessToCss()}; ";
            }
            else
                css += $"border-radius: {Thickness.Zero.ThicknessToCss()}; ";

            return css;
        }

        private string CssBuilder(IBoxShadow boxShadow)
        {
            var css = string.Empty;
            if (boxShadow.BoxShadow != null)
            {
                if (boxShadow.BoxShadow < 0)
                    boxShadow.BoxShadow = 0;
                if (boxShadow.BoxShadow > 5)
                    boxShadow.BoxShadow = 5;

                css += GetBoxShadowCss(boxShadow.BoxShadow);
            }

            return css;
        }

        private string CssBuilder(IBackground background)
        {
            var css = string.Empty;

            if (background.BackgroundColor != null)
                css += $"background-color: {background.BackgroundColor.Value}; ";
            else if (this is Grid || this is StackPanel ||
                     this is DockPanel || this is UniformGrid || this is WrapPanel || this is Tabs ||
                     this is Drawer)
                css += $"background-color: {ThemeManager.CurrentPalette.Background.Value}; ";

            return css;
        }

        private string CssBuilder(IBackgroundGradient gradient)
        {
            var css = string.Empty;

            if (gradient.BackgroundGradient1 != null)
                css += $"background: {gradient.BackgroundGradient1}";

            return css;
        }

        private string CssBuilder(IDraggable draggable)
        {
            var css = string.Empty;
            if (draggable.IsDraggable)
            {

            }

            return css;
        }

        private void SetColor(IColor thisComponent)
        {
            if (thisComponent.Color == null)
            {
                var backgroundColor = FindBackgroundColor();
                if (backgroundColor != null)
                    thisComponent.Color = Color.ContrastingColor(backgroundColor);
            }
        }

        private Color? FindBackgroundColor()
        {
            if (this is IBackground backgroundComponent && backgroundComponent.BackgroundColor != null)
                return backgroundComponent.BackgroundColor;

            return FindParentColor();
        }

        private Color? FindParentColor()
        {
            var backgroundParent = Parent as IBackground;
            var parent = Parent;
            while (parent != null && backgroundParent == null)
            {
                parent = parent.Parent;
                backgroundParent = parent as IBackground;
            }
            return backgroundParent == null ? null : backgroundParent.BackgroundColor;
        }

        private Alignment GetHorizontalAlignment()
        {
            if (HorizontalAlignment != null)
                return (Alignment)HorizontalAlignment;

            if (HorizontalAlignmentDefaultOverride != null)
                return (Alignment)HorizontalAlignmentDefaultOverride;

            return Alignment.Stretch;
        }

        private Alignment GetVerticalAlignment()
        {
            if (VerticalAlignment != null)
                return (Alignment)VerticalAlignment;

            if (VerticalAlignmentDefaultOverride != null)
                return (Alignment)VerticalAlignmentDefaultOverride;

            return Alignment.Stretch;
        }

        protected string AlignmentToCss(Alignment alignment, bool allowStretch)
        {
            switch (alignment)
            {
                case Alignment.Start: return "start";
                case Alignment.End: return "end";
                case Alignment.Center: return "center";
                case Alignment.Stretch: return allowStretch ? "stretch" : "center";
                default: throw new NotImplementedException();
            }
        }

        protected string GetFontStyle(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case ClearBlazor.FontStyle.Normal:
                    return "normal";
                case ClearBlazor.FontStyle.Italic:
                    return "italic";
                case ClearBlazor.FontStyle.Oblique:
                    return "oblique";
            }
            return "normal";
        }

        protected string GetTextTransform(TextTransform textTransform)
        {
            switch (textTransform)
            {
                case TextTransform.Uppercase:
                    return "uppercase";
                case TextTransform.Lowercase:
                    return "lowercase";
                case TextTransform.Capitalize:
                    return "capitalize";
            }
            return "none";
        }

        //private bool InScroller()
        //{
        //    var parent = Parent;
        //    while (parent != null)
        //    {
        //        if (parent.IsScroller)
        //            return true;
        //        parent = parent.Parent;
        //    }
        //    return false;
        //}

        protected string GetBoxShadowCss(int? boxShadow)
        {
            if (boxShadow == null)
                return string.Empty;

            switch (boxShadow)
            {
                case 0:
                    return $"box-shadow: none; ";
                case 1:
                    return $"box-shadow: 0px 2px 1px -1px rgba(0,0,0,0.2),0px 1px 1px 0px rgba(0,0,0,0.14),0px 1px 3px 0px rgba(0,0,0,0.12); ";
                case 2:
                    return $"box-shadow: 0px 4px 5px -2px rgba(0,0,0,0.2),0px 7px 10px 1px rgba(0,0,0,0.14),0px 2px 16px 1px rgba(0,0,0,0.12); ";
                case 3:
                    return $"box-shadow: 0px 7px 8px -4px rgba(0,0,0,0.2),0px 13px 19px 2px rgba(0,0,0,0.14),0px 5px 24px 4px rgba(0,0,0,0.12); ";
                case 4:
                    return $"box-shadow: 0px 9px 12px -6px rgba(0,0,0,0.2),0px 19px 29px 2px rgba(0,0,0,0.14),0px 7px 36px 6px rgba(0,0,0,0.12); ";
                case 5:
                    return $"box-shadow: 0px 11px 15px -7px rgba(0,0,0,0.2),0px 24px 38px 3px rgba(0,0,0,0.14),0px 9px 46px 8px rgba(0,0,0,0.12); ";
            }
            return $"box-shadow: none; ";
        }

        Task IHandleEvent.HandleEventAsync(
                  EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

        public virtual void Dispose()
        {
        }
    }
}
