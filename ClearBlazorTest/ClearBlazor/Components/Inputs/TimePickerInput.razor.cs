using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class TimePickerInput : ContainerInputBase<TimeOnly?>, IBackground
    {
        [Parameter]
        public string TimeFormat { get; set; } = "hh:mm tt";

        [Parameter]
        public bool Hours24 { get; set; } = false;

        [Parameter]
        public MinuteStep MinuteStep { get; set; } = MinuteStep.One;

        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopLeft;
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;
        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;

        private string? TimeString => Value == null ? string.Empty : ((TimeOnly)Value).ToString(TimeFormat);

        private bool PopupOpen = false;
        private SizeInfo? SizeInfo = null;
        private ElementReference PickerElement;
        private TimeOnly? CurrentTime = null;
        private TimePicker? TimePicker = null;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            SizeInfo? existing = null;
            if (SizeInfo != null)
                existing = SizeInfo;
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", PickerElement);
            //if (existing == null ||
            //    !existing.Equals(SizeInfo))
            //    StateHasChanged();
           if (CurrentTime == null)
                CurrentTime = Value;
        }

        private string GetSize()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return "width:100px;height:100px; ";
                case Size.Small:
                    return "width:100px;height:100px; ";
                case Size.Normal:
                    return "width:350px;height:350px; ";
                case Size.Large:
                    return "width:100px;height:100px; ";
                case Size.VeryLarge:
                    return "width:100px;height:100px; ";
            }
            return "width:350px;height:350px; ";
        }

        private bool IsMouseNotOver()
        {
            return !MouseOver;
        }

        private void TogglePopup()
        {
            PopupOpen = !PopupOpen;
            StateHasChanged();
        }

        private int GetPopupWidth()
        {
            if (SizeInfo == null)
                return 200;
            else
                return (int)SizeInfo.ParentWidth + 40;
        }

        protected override async Task ClearEntry()
        {
            await Task.CompletedTask;
        }
        protected override string ComputeInputStyle()
        {
            string css = base.ComputeInputStyle();
            return css;
        }

        protected override string GetInputType()
        {
            return string.Empty;
        }

        private async Task OpenChanged()
        {
            if (TimePicker != null && PopupOpen == false)
            {
                if (Hours24)
                    TimePicker.SetMode(PickerMode.Hour24);
                else
                    TimePicker.SetMode(PickerMode.Hour12);
                Value = CurrentTime;
                await ValueChanged.InvokeAsync(Value);
            }
        }

        private async Task MinuteSelected()
        {
            PopupOpen = false;
            StateHasChanged();
            Value = CurrentTime;
            await ValueChanged.InvokeAsync(Value);
            if (TimePicker != null)
                if (Hours24)
                    TimePicker.SetMode(PickerMode.Hour24);
                else
                    TimePicker.SetMode(PickerMode.Hour12);
        }
    }
}