using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace ClearBlazor
{
    public partial class Switch : InputBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        [Parameter]
        public LabelLocation LabelLocation { get; set; } = LabelLocation.End;

        [Parameter]
        public Color? UncheckedColour { get; set; } = null;

        private bool MouseOver = false;
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            HorizontalAlignment = Alignment.Start;
            VerticalAlignment = Alignment.Start;
            Colour = Color.Primary;
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
                css += $"background-color:{ThemeManager.CurrentPalette.TextDisabled.Lighten(.2).Value};";
            else if (Checked)
                css += $"background-color:{Colour!.Lighten(.3).Value};";
            else
                if (UncheckedColour == null)
                css += $"background-color:{Colour!.Lighten(.3).Value}; ";
            else
                css += $"background-color:{UncheckedColour!.Lighten(.3).Value}; ";

            (double height, double width, double margin) = GetSwitchSize();

            css += $"margin:{height / 4}px {height/4}px {height/4}px {height/4}px; border-radius:{height / 2}px; ";
            return css;
        }


        private string GetThumbStyle()
        {
            string css = string.Empty;

            if (IsDisabled)
                css += $"background-color:{ThemeManager.CurrentPalette.TextDisabled.Value};";
            else if (Checked)
                css += $"background-color:{Colour!.Value}; ";
            else
                if (UncheckedColour == null)
                css += $"background-color:{Colour!.Value}; ";
            else
                css += $"background-color:{UncheckedColour!.Value}; ";

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
                css += $"background-color: {ThemeManager.CurrentPalette.GrayLighter.SetAlpha(.5).Value}; ";
            else
                css += $"background-color: {Color.Transparent.Value}; ";
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
                await ToolTipElement.ShowToolTip();
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