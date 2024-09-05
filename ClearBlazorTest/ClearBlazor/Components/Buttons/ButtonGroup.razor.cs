using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ButtonGroup : ClearComponentBase, IContent
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public TextEditFillMode? ButtonStyle { get; set; } = null;

        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Landscape;

        [Parameter]
        public Color? Color { get; set; } = null;

        [Parameter]
        public bool DisableBoxShadow { get; set; } = false;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public IconLocation IconLocation { get; set; } = IconLocation.Start;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }

        protected override string UpdateStyle(string css)
        {
            if (Orientation == Orientation.Landscape)
                css += $"display: flex; flex-direction: row; ";
            else
                css += $"display: flex; flex-direction: column; ";

            return css;
        }

        protected override void UpdateChildParams(ClearComponentBase child, int level)
        {
            if (level == 1 && child is Button)
            {
                var btn = (Button)child;
                if (ButtonStyle != null)
                    btn.ButtonStyleOverride = ButtonStyle;
                if (Color != null)
                    btn.ColorOverride = Color;
                btn.SizeOverride = Size;
            }
        }

        protected override string UpdateChildStyle(ClearComponentBase child, string css)
        {
            if (child is Button)
            {
                var btn = (Button) child;
                if (btn == Children.First())
                    if (Orientation == Orientation.Landscape)
                    {
                        css = UpdateBorderRadius(css, "border-radius:4px 0 0 4px; ");
                        css = UpdateBorderWidth(css, "border-width: 1px 0 1px 1px; ");
                    }
                    else
                    {
                        css = UpdateBorderRadius(css, "border-radius:4px 4px 0 0; ");
                        css = UpdateBorderWidth(css, "border-width: 1px 1px 0 1px; ");
                    }
                else if (btn == Children.Last())
                    if (Orientation == Orientation.Landscape)
                    {
                        css = UpdateBorderRadius(css, "border-radius:0 4px 4px 0; ");
                        if (ButtonStyle == ClearBlazor.TextEditFillMode.None)
                            css += "border-width: 0 0 0 1px; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, "border-width: 1px 1px 1px 1px; ");
                    }
                    else
                    {
                        css = UpdateBorderRadius(css, "border-radius: 0 0 4px 4px; ");
                        if (ButtonStyle == ClearBlazor.TextEditFillMode.None)
                            css += "border-width: 1px 0 0 0; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, "border-width: 1px 1px 1px 1px; ");
                    }
                else
                {
                    css = UpdateBorderRadius(css, "border-radius:0; ");
                    if (Orientation == Orientation.Landscape)
                        if (ButtonStyle == ClearBlazor.TextEditFillMode.None)
                            css += "border-width: 0 0 0 1px; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, "border-width: 1px 0 1px 1px; ");
                    else
                        if (ButtonStyle == ClearBlazor.TextEditFillMode.None)
                            css += "border-width: 1px 0 0 0; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, "border-width: 1px 1px 0 1px; ");
                }
            }
            return css;
        }

        private string UpdateBorderRadius(string css, string newRadius)
        {
            try
            {
                var startIndex = css.IndexOf("border-radius");
                if (startIndex >= 0)
                {
                    var endIndex = css.IndexOf(";", startIndex);
                    if (endIndex >= 0)
                    {
                        var length = endIndex - startIndex;
                        return css.Remove(startIndex, length + 1).Insert(startIndex, newRadius);
                    }
                }
            }
            catch(Exception) 
            { 
            }
            return css;
        }
        private string UpdateBorderWidth(string css, string newBorder)
        {
            try
            {
                var startIndex = css.IndexOf("border-width");
                if (startIndex >= 0)
                {
                    var endIndex = css.IndexOf(";", startIndex);

                    if (endIndex >= 0)
                    {
                        var length = endIndex - startIndex;
                        return css.Remove(startIndex, length + 1).Insert(startIndex, newBorder);
                    }
                }
            }
            catch (Exception) 
            { 
            }

            return css;
        }
    }
}