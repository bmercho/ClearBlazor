using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A checkbox component
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class CheckBox<TItem> : InputBase
    {
        /// <summary>
        /// The value of the checkbox
        /// </summary>
        [Parameter]
        public TItem? Checked { get; set; }

        /// <summary>
        /// Event that is triggered when the checkbox is checked or unchecked
        /// </summary>
        [Parameter]
        public EventCallback<TItem> CheckedChanged { get; set; }

        /// <summary>
        /// The location of the label
        /// </summary>
        [Parameter]
        public LabelLocation LabelLocation { get; set; } = LabelLocation.End;

        /// <summary>
        /// The icon to display when the checkbox is checked
        /// </summary>
        [Parameter]
        public string CheckedIcon { get; set; } = Icons.Material.Filled.CheckBox;

        /// <summary>
        /// The icon to display when the checkbox is unchecked
        /// </summary>
        [Parameter]
        public string UncheckedIcon { get; set; } = Icons.Material.Filled.CheckBoxOutlineBlank;

        /// <summary>
        /// The icon to display when the checkbox is in an indeterminate state
        /// </summary>
        [Parameter]
        public string IndeterminateIcon { get; set; } = Icons.Material.Filled.IndeterminateCheckBox;

        /// <summary>
        /// Indicates whether the checkbox can show an indeterminate state
        /// </summary>
        [Parameter]
        public bool TriState { get; set; } = false;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            HorizontalAlignment = Alignment.Start;
            VerticalAlignment = Alignment.Start;
            await base.SetParametersAsync(parameters);
        }

        protected override string UpdateStyle(string css)
        {
            return css;
        }

        private async Task OnIconClicked()
        {
            if (IsReadOnly)
                return;

            object? value;
            if (Checked == null)
                value = true;
            else
            {
                value = Checked;
                bool isNullable = typeof(TItem) == typeof(bool?);
                if (TriState && isNullable && (bool)value == false)
                    value = null;
                else
                    value = !(bool)value;
            }
            if (value == null)
                Checked = default;
            else
                Checked = (TItem?)value;

            await ValidateField();
            await CheckedChanged.InvokeAsync(Checked);
            StateHasChanged();
        }

        protected string GetIcon()
        {
            object? value = Checked;
            if (Checked == null)
                return IndeterminateIcon;
            else if (value is bool && (bool)value == true)
                return CheckedIcon;
            else
                return UncheckedIcon;
        }
    }
}