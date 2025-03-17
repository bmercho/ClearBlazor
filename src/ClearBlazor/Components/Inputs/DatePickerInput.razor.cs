using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A date picker input component
    /// </summary>
    public partial class DatePickerInput : ContainerInputBase<DateOnly?>, IBackground
    {
        [Parameter]
        public string DateFormat { get; set; } = "dd MMM yyyy";

        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomLeft;
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopLeft;
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;
        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;

        private string? DateString => Value == null ? string.Empty : ((DateOnly)Value).ToString(DateFormat);

        private bool PopupOpen = false;
        private ElementReference PickerElement;
        private DateOnly? CurrentDate = null;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
           if (CurrentDate == null)
                CurrentDate = Value;
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

        private async Task DateSelected()
        {
            Value = CurrentDate;
            await ValueChanged.InvokeAsync(Value);
            PopupOpen = false;
            StateHasChanged();
        }
    }
}