using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A radio group input component
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class RadioGroup<TItem> : InputBase, IBackground
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// When used in a form, indicates if the field is required
        /// </summary>
        [Parameter]
        public bool Required { get; set; } = false;

        /// <summary>
        /// The value of the radio group
        /// </summary>
        [Parameter]
        public TItem? Value { get; set; }

        /// <summary>
        /// The data to be displayed in the radio group
        /// </summary>
        [Parameter]
        public List<RadioGroupDataItem<TItem>> RadioGroupData { get; set; } = new List<RadioGroupDataItem<TItem>>();

        /// <summary>
        /// The orientation of the radio group
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Landscape;

        /// <summary>
        /// The spacing between the radio buttons
        /// </summary>
        [Parameter]
        public int Spacing { get; set; } = 0;

        /// <summary>
        /// Event that is triggered when the value of the radio group changes
        /// </summary>
        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
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

        internal async Task SetNewSelection(Radio<TItem> radio, TItem? value)
        {
            if (_selectedRadio != null)
                _selectedRadio.Uncheck();
            _selectedRadio = radio;
            Value = value;
            await ValueChanged.InvokeAsync(value);
            await ValidateField();
            StateHasChanged();
        }

        internal override async Task<bool> ValidateField()
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
}