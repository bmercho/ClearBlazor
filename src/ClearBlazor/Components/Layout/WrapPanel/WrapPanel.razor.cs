using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// The WrapPanel positions children next to the other, horizontally(default) or vertically,
    /// until there is no more room, where it will wrap to the next line(or column) and then continue.
    /// </summary>
    public partial class WrapPanel:ClearComponentBase, IBackground, IBorder
    {
        /// <summary>
        /// The spacing between rows.
        /// </summary>
        [Parameter]
        public double RowSpacing { get; set; } = 0;

        /// <summary>
        /// The spacing between columns.
        /// </summary>
        [Parameter]
        public double ColumnSpacing { get; set; } = 0;

        /// <summary>
        /// The direction for the wrapping. 
        /// </summary>
        [Parameter]
        public Direction Direction { get; set; } = Direction.Row;

        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href=IBorderApi>IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href=IBoxShadowApi>IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; } = null;

        /// <summary>
        /// See <a href=IBackgroundApi>IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.TryGetValue<Direction>(nameof(Direction), out var direction);

            switch (direction)
            {
                case Direction.Row:
                    VerticalAlignment = Alignment.Start;
                    HorizontalAlignment = Alignment.Stretch;
                    break;
                case Direction.RowReverse:
                    VerticalAlignment = Alignment.End;
                    HorizontalAlignment = Alignment.Stretch;
                    break;
                case Direction.Column:
                    VerticalAlignment = Alignment.Stretch;
                    HorizontalAlignment = Alignment.Start;
                    break;
                case Direction.ColumnReverse:
                    VerticalAlignment = Alignment.Stretch;
                    HorizontalAlignment = Alignment.End;
                    break;
            }
            return base.SetParametersAsync(parameters);
        }

        protected override string UpdateStyle(string css)
        {
            if (Direction == Direction.Column || Direction == Direction.ColumnReverse)
                css += "height:100vh; ";

            css += $"display: flex; {GetDirection()} flex-wrap:wrap; {GetVerticalAlignItems()} {GetHorizontalAlignItems()}";
            if (RowSpacing != 0)
                css += $"row-gap: {RowSpacing}px; ";

            if (ColumnSpacing != 0)
                css += $"column-gap: {ColumnSpacing}px; ";

            return css;
        }

        private string GetVerticalAlignItems()
        {
            if (Direction == Direction.Row || Direction == Direction.RowReverse)
            {
                switch (VerticalAlignment)
                {
                    case Alignment.Stretch:
                        return "align-items:stretch; ";
                    case Alignment.Start:
                        return "align-items:flex-start; ";
                    case Alignment.Center:
                        return "align-items:center; ";
                    case Alignment.End:
                        return "align-items:flex-end; ";
                }
                return "Align-items:flex-start; ";
            }
            return string.Empty;
        }

        private string GetHorizontalAlignItems()
        {
            if (Direction == Direction.Column || Direction == Direction.ColumnReverse)
            {
                switch (HorizontalAlignment)
                {
                    case Alignment.Stretch:
                        return "align-self:stretch; ";
                    case Alignment.Start:
                        return "align-self:flex-start; ";
                    case Alignment.Center:
                        return "align-self:center; ";
                    case Alignment.End:
                        return "align-self:flex-end; ";
                }
                return "align-self:flex-start; ";
            }
            return string.Empty;
        }

        private string GetDirection()
        {
            switch (Direction)
            {
                case Direction.Row:
                    return "flex-direction:row; ";
                case Direction.RowReverse:
                    return "flex-direction:row-reverse; ";
                case Direction.Column:
                    return "flex-direction:column; ";
                case Direction.ColumnReverse:
                    return "flex-direction:column-reverse; ";
            }
            return "flex-direction:row; ";
        }
    }
}