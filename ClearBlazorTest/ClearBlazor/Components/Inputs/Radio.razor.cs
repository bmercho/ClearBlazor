using global::Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;

namespace ClearBlazor
{
    public partial class Radio<TItem> : InputBase
    {
        [Parameter]
        public bool Checked { get; set; } = false;

        [Parameter]
        public TItem? Value { get; set; }

        [Parameter]
        public LabelLocation LabelLocation { get; set; } = LabelLocation.End;

        [Parameter]
        public string CheckedIcon { get; set; } = Icons.Material.Filled.RadioButtonChecked;

        [Parameter]
        public string UncheckedIcon { get; set; } = Icons.Material.Filled.RadioButtonUnchecked;

        internal Color? ColourOverride { get; set; } = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var parent = Parent?.Parent?.Parent?.Parent as RadioGroup<TItem>;

            if (parent == null)
                return;

            parent.HandleChild(this);

        }

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

        public void Check()
        {
            Checked = true;
            StateHasChanged();
        }

        public void Uncheck()
        {
            Checked = false;
            StateHasChanged();
        }

        private async Task OnIconClicked()
        {
            if (IsReadOnly || Checked)
                return;

            var parent = Parent?.Parent?.Parent?.Parent as RadioGroup<TItem>;
            if (parent != null)
            {
                Checked = true;
                await parent.SetNewSelection(this, Value);
            }
        }

        protected string GetIcon()
        {
            if (Checked)
                return CheckedIcon;
            else
                return UncheckedIcon;
        }

        private Color? GetColour()
        {
            if (Colour != null)
                return Colour;

            if (ColourOverride != null)
                return ColourOverride;

            return Colour;
        }
    }
}