using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ButtonGroup : ClearComponentBase
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
        public Color? OutlineColor { get; set; } = null;

        [Parameter]
        public bool DisableBoxShadow { get; set; } = false;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public IconLocation IconLocation { get; set; } = IconLocation.Start;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
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
                btn._outlineColorOverride = OutlineColor;
                btn.SizeOverride = Size;
            }
        }

        protected override string UpdateChildStyle(ClearComponentBase child, string css)
        {
            if (child is Button)
            {
                var btn = (Button) child;
                if (IsFirst(btn))
                    if (Orientation == Orientation.Landscape)
                    {
                        css = UpdateBorderRadius(css, "border-radius:4px 0 0 4px; ");
                        css = UpdateBorderWidth(css, 
                                $"border-width: 1px 0 1px 1px; border-style:solid; " +
                                $"border-color:{btn.GetOutlineColor(btn.GetColor()).Value}; ");
                    }
                    else
                    {
                        css = UpdateBorderRadius(css, "border-radius:4px 4px 0 0; ");
                        css = UpdateBorderWidth(css, 
                                 "border-width: 1px 1px 0 1px; border-style:solid; " +
                                 $"border-color:{btn.GetOutlineColor(btn.GetColor()).Value}; ");
                    }
                else if (IsLast(btn))
                    if (Orientation == Orientation.Landscape)
                    {
                        css = UpdateBorderRadius(css, "border-radius:0 4px 4px 0; ");
                        if (ButtonStyle == TextEditFillMode.None)
                            css += "border-width: 0 0 0 1px; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, 
                                "border-width: 1px 1px 1px 1px; border-style:solid;  " +
                                $"border-color:{btn.GetOutlineColor(btn.GetColor()).Value};");
                    }
                    else
                    {
                        css = UpdateBorderRadius(css, "border-radius: 0 0 4px 4px; ");
                        if (ButtonStyle == TextEditFillMode.None)
                            css += "border-width: 1px 0 0 0; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, 
                                "border-width: 1px 1px 1px 1px; border-style:solid; " +
                                $"border-color:{btn.GetOutlineColor(btn.GetColor()).Value}; ");
                    }
                else
                {
                    css = UpdateBorderRadius(css, "border-radius:0; ");
                    if (Orientation == Orientation.Landscape)
                        if (ButtonStyle == TextEditFillMode.None)
                            css += "border-width: 0 0 0 1px; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, 
                                "border-width: 1px 0 1px 1px; border-style:solid; " +
                                $"border-color:{btn.GetOutlineColor(btn.GetColor()).Value};");
                    else
                        if (ButtonStyle == TextEditFillMode.None)
                            css += "border-width: 1px 0 0 0; border-style: solid; ";
                        else
                            css = UpdateBorderWidth(css, 
                                "border-width: 1px 1px 0 1px; border-style:solid; " +
                                $"border-color:{btn.GetOutlineColor(btn.GetColor())}.Value;");
                }
            }
            return css;
        }

        private bool IsFirst(Button btn)
        {
            // Use tag to identify the first and last items in button group 
            // when button group changes dynamically (because the order of the Children changes).
            // Tag = 1 for start item 
            // Tag = 2 for end item 
            // Tag = 0 for other items 
            if (btn.Tag != null)
            {
                if (btn.Tag == 1)
                    return true;
            }
            else if (btn == Children.First())
                return true;
            return false;
        }

        private bool IsLast(Button btn)
        {
            // Use tag to identify the first and last items in button group 
            // when button group changes dynamically (because the order of the Children changes).
            // Tag = 1 for start item 
            // Tag = 2 for end item 
            // Tag = 0 for other items 
            if (btn.Tag != null)
            {
                if (btn.Tag == 2)
                    return true;
            }
            else if (btn == Children.Last())
                return true;
            return false;
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