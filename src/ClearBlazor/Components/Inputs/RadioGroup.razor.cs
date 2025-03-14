using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class RadioGroup<TItem> : InputBase, IBackground
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public bool Required { get; set; } = false;

        [Parameter]
        public TItem? Value { get; set; }

        [Parameter]
        public List<RadioGroupDataItem<TItem>> RadioGroupData { get; set; } = new List<RadioGroupDataItem<TItem>>();

        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Landscape;

        [Parameter]
        public int Spacing { get; set; } = 0;

        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        protected Radio<TItem>? _selectedRadio = null;
        private string? RequiredErrorMessage = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            (Required, RequiredErrorMessage) = IsRequired(Required);

        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

        }

        internal void HandleChild(Radio<TItem> radio)
        {
            radio.Size = Size;
            radio.ColorOverride = Color;
            radio.IsDisabled = IsDisabled;
            radio.IsReadOnly = IsReadOnly;
            if (Value != null && Value.Equals(radio.Value))
            {
                _selectedRadio = radio;
                radio.Check();
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; ";

            return css;
        }

        public async Task SetNewSelection(Radio<TItem> radio, TItem? value)
        {
            if (_selectedRadio != null)
                _selectedRadio.Uncheck();
            _selectedRadio = radio;
            Value = value;
            await ValueChanged.InvokeAsync(value);
            await ValidateField();
            StateHasChanged();
        }

        public override async Task<bool> ValidateField()
        {
            IsValid = true;
            ValidationErrorMessages.Clear();
            if (Required && Value == null)
            {
                IsValid = false;
                if (RequiredErrorMessage != null)
                    ValidationErrorMessages.Add(RequiredErrorMessage);
            }
            await Task.CompletedTask;
            return IsValid;
        }
    }

    public class RadioGroupDataItem<TItem>
    {
        public string Name { get; set; } = string.Empty;
        public TItem? Value { get; set; } = default;

        public RadioGroupDataItem()
        {
        }

        public RadioGroupDataItem(string name, TItem value)
        {
            Name = name;
            Value = value;
        }
    }
}