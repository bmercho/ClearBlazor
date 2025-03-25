using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    /// <summary>
    /// A switch input component
    /// </summary>
    public partial class Switch : InputBase
    {
        /// <summary>
        /// Indicates whether the switch is checked. 
        /// </summary>
        [Parameter]
        public bool Checked { get; set; }

        /// <summary>
        /// Represents a callback that is triggered when the checked state changes. 
        /// </summary>
        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        /// <summary>
        /// Specifies the location of the label, defaulting to the end position.
        /// </summary>
        [Parameter]
        public LabelLocation LabelLocation { get; set; } = LabelLocation.End;

        /// <summary>
        /// Represents the color used when an option is unchecked. 
        /// </summary>
        [Parameter]
        public Color? UncheckedColor { get; set; } = null;

        private bool MouseOver = false;
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            HorizontalAlignment = Alignment.Start;
            VerticalAlignment = Alignment.Start;
            Color = Color.Primary;
            await base.SetParametersAsync(parameters);
        }

        protected override string UpdateStyle(string css)
        {
            return css;
        }

        private async Task OnSwitchClicked()
        {
            if (IsReadOnly || IsDisabled)
                return;

            Checked = !Checked;

            await ValidateField();
            await CheckedChanged.InvokeAsync(Checked);
            StateHasChanged();
        }

        private string GetSwitchStyle()
        {
            string css = string.Empty;

            (double height, double width, double margin) = GetSwitchSize();

            css += $"width:{width}px; height:{height}px;overflow:visible;";
            if (IsDisabled)
                css += "cursor: default; ";
            else
                css += "cursor: pointer; ";

            return css;
        }

        private (double height, double width, double margin) GetSwitchSize()
        {
            double width = 60;
            switch (Size)
            {
                case Size.VerySmall:
                    width = 40;
                    break;
                case Size.Small:
                    width = 50;
                    break;
                case Size.Normal:
                    width = 60;
                    break;
                case Size.Large:
                    width = 70;
                    break;
                case Size.VeryLarge:
                    width = 80;
                    break;
            }
            return (width*0.6, width, width/4);
        }


        private string GetTrackStyle()
        {
            string css = string.Empty;
            if (IsDisabled)
                css += $"background-color:{ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.08).Value};";
            else if (Checked)
                css += $"background-color:{Color!.Lighten(.3).Value};";
            else
                if (UncheckedColor == null)
                css += $"background-color:{Color!.Lighten(.3).Value}; ";
            else
                css += $"background-color:{UncheckedColor!.Lighten(.3).Value}; ";

            (double height, double width, double margin) = GetSwitchSize();

            css += $"margin:{height / 4}px {height/4}px {height/4}px {height/4}px; border-radius:{height / 2}px; ";
            return css;
        }


        private string GetThumbStyle()
        {
            string css = string.Empty;

            if (IsDisabled)
                css += $"background-color:{ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.12).Value};";
            else if (Checked)
                css += $"background-color:{Color!.Value}; ";
            else
                if (UncheckedColor == null)
                css += $"background-color:{Color!.Value}; ";
            else
                css += $"background-color:{UncheckedColor!.Value}; ";

            (double height, double width, double margin) = GetSwitchSize();

            double diameter = height*.6;
            css += $"width:{diameter}px; height:{diameter}px; border-radius:50%; ";

            return css;
        }

        private string GetHoverStyle()
        {
            string css = string.Empty;
            (double height, double width, double margin) = GetSwitchSize();

            double diameter = height;
            css += $"width:{diameter}px; height:{diameter}px; border-radius:50%; ";
            if (MouseOver)
                css += $"background-color: {ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.12).Value}; ";
            if (Checked)
                css += $"margin-left:{width-height}px; ";
            else
                css += $"margin-left:0px; ";
            if (IsDisabled)
                css += "cursor: default; { ";
            else
                css += "cursor: pointer; ";

            return css;
        }

        private string GetLabelMargin()
        {
            int margin = 5;
            switch (Size)
            {
                case Size.VerySmall:
                    margin = 3;
                    break;
                case Size.Small:
                    margin = 4;
                    break;
                case Size.Normal:
                    margin = 5;
                    break;
                case Size.Large:
                    margin = 6;
                    break;
                case Size.VeryLarge:
                    margin = 7;
                    break;
            }

            if (LabelLocation == LabelLocation.Start)
                return $"0,{margin},0,0";
            else
                return $"0,0,0,{margin}";

        }

        protected async Task OnMouseEnter(MouseEventArgs e)
        {
            MouseOver = true;
            if (ToolTipElement == null)
                await Task.CompletedTask;
            else
                ToolTipElement.ShowToolTip();
            StateHasChanged();
        }

        protected async Task OnMouseLeave(MouseEventArgs e)
        {
            MouseOver = false;
            ToolTipElement?.HideToolTip();
            await Task.CompletedTask;
            StateHasChanged();
        }

    }
}