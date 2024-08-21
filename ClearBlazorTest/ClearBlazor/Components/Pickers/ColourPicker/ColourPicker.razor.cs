using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class ColourPicker : ClearComponentBase, IBackground
    {
        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

        [Parameter]
        public Color? Colour { get; set; } = null;

        [Parameter]
        public EventCallback<Color> ColourChanged { get; set; }

        const int DragWidth = 312;
        const int DragHeight = 250;
        const int DragDiameter = 20;
        const int DragBorderWidth = 2;

        private double hValue;
        private double sValue;
        private double lValue;
        private byte rValue;
        private byte gValue;
        private byte bValue;
        private int aValue;
        private bool hlsMode = true;
        private ElementReference DragElement;
        private ElementReference GridElement;
        private bool MouseDown = false;
        private Color BaseColour = new Color(0, 0, 0, 0);
        private SizeInfo? SizeInfo = null;
        private Color? CurrentColour = null;
        private double DragMarginLeft = 0;
        private double DragMarginTop = 0;


        public double HValue
        {
            get { return hValue; }
            set
            {
                hValue = value;
                GetNewColour(true);
            }
        }
        public double SValue
        {
            get { return sValue; }
            set
            {
                sValue = value;
                GetNewColour(true);
            }
        }
        public double LValue
        {
            get { return lValue; }
            set
            {
                lValue = value;
                GetNewColour(true);
            }
        }
        public byte RValue
        {
            get { return rValue; }
            set
            {
                rValue = value;
                GetNewColour(false);
            }
        }
        public byte GValue
        {
            get { return gValue; }
            set
            {
                gValue = value;
                GetNewColour(false);
            }
        }
        public byte BValue
        {
            get { return bValue; }
            set
            {
                bValue = value;
                GetNewColour(false);
            }
        }
        public int AValue
        {
            get { return aValue; }
            set
            {
                aValue = value;
                GetNewColour(true);
            }
        }

        private int GetWidth()
        {
            return DragWidth;
        }
        private int GetHeight()
        {
            return DragHeight;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            SizeInfo? existing = null;
            if (SizeInfo != null)
                existing = SizeInfo;
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", GridElement);
            if (existing == null ||
                !existing.Equals(SizeInfo) || CurrentColour == null || !CurrentColour.Equals(Colour))
            {
                CurrentColour = Colour;
                if (Colour != null)
                {
                    hValue = Colour.H;
                    sValue = Colour.S;
                    lValue = Colour.L;
                    rValue = Colour.R;
                    gValue = Colour.G;
                    bValue = Colour.B;
                    aValue = Colour.A;
                    BaseColour = GetBaseColour();
                    UpdateColorSelectorBasedOnRgb();
                }
                StateHasChanged();
            }
        }

        private void UpdateColorSelectorBasedOnRgb()
        {
            var hueValue = (int)((HValue / 360.0) * 6.0 * 255.0);

            var index = hueValue / 255;
            if (index == 6)
                index = 5;

            int r = 0;
            int g = 0;
            int b = 0;
            string dominantColorPart = string.Empty;

            switch (index)
            {
                case 0:
                    r = 255;
                    g = hueValue;
                    b = 0;
                    dominantColorPart = "rb";
                    break;
                case 1:
                    r = 255 - hueValue;
                    g = 255;
                    b = 0;
                    dominantColorPart = "gb";
                    break;
                case 2:
                    r = 0;
                    g = 255;
                    b = hueValue;
                    dominantColorPart = "gr";
                    break;
                case 3:
                    r = 0;
                    g = 255 - hueValue;
                    b = 255;
                    dominantColorPart = "br";
                    break;
                case 4:
                    r = hueValue;
                    g = 0;
                    b = 255;
                    dominantColorPart = "bg";
                    break;
                case 5:
                    r = 255;
                    g = 0;
                    b = 255 - hueValue;
                    dominantColorPart = "rg";
                    break;
            }

            var colorValues = dominantColorPart switch
            {
                "rb" => (RValue, BValue),
                "rg" => (RValue, GValue),
                "gb" => (GValue, BValue),
                "gr" => (GValue, RValue),
                "br" => (BValue, RValue),
                "bg" => (BValue, GValue),
                _ => (255, 255)
            };

            var primaryDiff = 255 - colorValues.Item1;
            var primaryDiffDelta = colorValues.Item1 / 255.0;

            DragMarginTop = (int)(primaryDiff / 255.0 * DragHeight) - DragDiameter / 2 - DragBorderWidth;

            var secondaryColorX = colorValues.Item2 * (1.0 / primaryDiffDelta);
            var relation = (255 - secondaryColorX) / 255.0;

            DragMarginLeft = relation * DragWidth - DragDiameter / 2 - DragBorderWidth;
        }

        private async void GetNewColour(bool fromHLS)
        {
            if (fromHLS)
            {
                Colour = new Color(HValue, SValue, LValue, AValue);
                BaseColour = GetBaseColour();
            }
            else
                Colour = new Color(RValue, GValue, BValue, AValue);

            hValue = Colour.H;
            sValue = Colour.S;
            lValue = Colour.L;
            rValue = Colour.R;
            gValue = Colour.G;
            bValue = Colour.B;
            aValue = Colour.A;

            CurrentColour = Colour;
            await ColourChanged.InvokeAsync(Colour);

            StateHasChanged();
        }

        private Color GetBaseColour()
        {
            var index = (int)hValue / 60;
            if (index == 6)
                index = 5;

            var valueInDeg = (int)HValue - (index * 60);

            var value = (int)(valueInDeg / 60.0 * 255.0);

            int r = 0;
            int g = 0;
            int b = 0;

            switch (index)
            {
                case 0:
                    r = 255;
                    g = value;
                    b = 0;
                    break;
                case 1:
                    r = 255 - value;
                    g = 255;
                    b = 0;
                    break;
                case 2:
                    r = 0;
                    g = 255;
                    b = value;
                    break;
                case 3:
                    r = 0;
                    g = 255 - value;
                    b = 255;
                    break;
                case 4:
                    r = value;
                    g = 0;
                    b = 255;
                    break;
                case 5:
                    r = 255;
                    g = 0;
                    b = 255 - value;
                    break;
            }
            return new(r, g, b, 255);
        }

        private void Clicked()
        {
            hlsMode = !hlsMode;
            StateHasChanged();
        }

        private string GetDragStyle()
        {
            string css = string.Empty;
            css += $"display:grid;height:{DragDiameter}px;width:{DragDiameter}px;border-width:{DragBorderWidth}px;border-style:solid;" +
                   $"margin-left:{DragMarginLeft}px; margin-top:{DragMarginTop}px;" +
                   $"border-radius:50%;border-color:{Color.Dark.Value};" +
                   $"background-color:{Color.Transparent}; ";
            return css;
        }

        private string GetInnerDragStyle()
        {
            string css = string.Empty;
            css += $"border-width:{DragBorderWidth}px; border-radius:50%; border-style:solid; " +
                   $"border-color:{Color.Light.Value}; ";
            return css;
        }

        private async Task OnMouseDown(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("CaptureMouse", DragElement, 1);
            MouseDown = true;
        }
        private async Task OnMouseUp(MouseEventArgs e)
        {
            MouseDown = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", DragElement, 1);
        }

        private async Task OnMouseMove(MouseEventArgs e)
        {
            if (MouseDown)
            {
                if (SizeInfo == null)
                    return;

                var offsetX = e.ClientX - SizeInfo.ElementX;
                var offsetY = e.ClientY - SizeInfo.ElementY;
                double x = (offsetX / SizeInfo.ElementWidth) * DragWidth;
                double y = (offsetY / SizeInfo.ElementHeight) * DragHeight;

                if (x < 0)
                    x = 0;
                if (x >= DragWidth)
                    x = DragWidth;
                if (y < 0)
                    y = 0;
                if (y >= DragHeight)
                    y = DragHeight;

                await UpdateColorFromLocation(x, y);

                DragMarginLeft = x - DragDiameter / 2 - DragBorderWidth;
                DragMarginTop = y - DragDiameter / 2 - DragBorderWidth;
                StateHasChanged();
            }
        }

        private async Task OnDragAreaClicked(MouseEventArgs e)
        {
            if (SizeInfo == null)
                return;

            var offsetX = e.ClientX - SizeInfo.ElementX;
            var offsetY = e.ClientY - SizeInfo.ElementY;
            double x = (offsetX / SizeInfo.ElementWidth) * DragWidth;
            double y = (offsetY / SizeInfo.ElementHeight) * DragHeight;

            if (x < 0)
                x = 0;
            if (x >= DragWidth)
                x = DragWidth;
            if (y < 0)
                y = 0;
            if (y >= DragHeight)
                y = DragHeight;

            await UpdateColorFromLocation(x, y);

            DragMarginLeft = x - DragDiameter / 2 - DragBorderWidth;
            DragMarginTop = y - DragDiameter / 2 - DragBorderWidth;
            StateHasChanged();

        }

        private async Task UpdateColorFromLocation(double x, double y)
        {
            var xRatio = x / DragWidth;
            var yRatio = 1.0 - (y / DragHeight);

            var rx = 255 - (int)((255 - BaseColour.R) * xRatio);
            var gx = 255 - (int)((255 - BaseColour.G) * xRatio);
            var bx = 255 - (int)((255 - BaseColour.B) * xRatio);

            var r = rx * yRatio;
            var g = gx * yRatio;
            var b = bx * yRatio;

            Colour = new Color((byte)r, (byte)g, (byte)b, Colour);
            hValue = Colour.H;
            sValue = Colour.S;
            lValue = Colour.L;
            rValue = Colour.R;
            gValue = Colour.G;
            bValue = Colour.B;
            aValue = Colour.A;

            CurrentColour = Colour;

            await ColourChanged.InvokeAsync(Colour);
        }
    }
}