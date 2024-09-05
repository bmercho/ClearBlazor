using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class CheckBox<TItem> : InputBase,IBackground
    {
        [Parameter]
        public TItem? Checked { get; set; }

        [Parameter]
        public EventCallback<TItem> CheckedChanged { get; set; }

        [Parameter]
        public LabelLocation LabelLocation { get; set; } = LabelLocation.End;

        [Parameter]
        public string CheckedIcon { get; set; } = Icons.Material.Filled.CheckBox;

        [Parameter]
        public string UncheckedIcon { get; set; } = Icons.Material.Filled.CheckBoxOutlineBlank;

        [Parameter]
        public string IndeterminateIcon { get; set; } = Icons.Material.Filled.IndeterminateCheckBox;

        [Parameter]
        public bool TriState { get; set; } = false;
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

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
            else if ((bool)value == true)
                return CheckedIcon;
            else
                return UncheckedIcon;
        }
    }
}