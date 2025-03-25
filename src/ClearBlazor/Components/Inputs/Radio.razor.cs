using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A radio button input component
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class Radio<TItem> : InputBase,IBackground
    {
        /// <summary>
        /// Indicates whether the radio button is checked   
        /// </summary>
        [Parameter]
        public bool Checked { get; set; } = false;

        /// <summary>
        /// The value of the radio button
        /// </summary>
        [Parameter]
        public TItem? Value { get; set; }

        /// <summary>
        /// The location of the label
        /// </summary>
        [Parameter]
        public LabelLocation LabelLocation { get; set; } = LabelLocation.End;

        /// <summary>
        /// The icon to display when the radio button is checked
        /// </summary>
        [Parameter]
        public string CheckedIcon { get; set; } = Icons.Material.Filled.RadioButtonChecked;

        /// <summary>
        /// The icon to display when the radio button is unchecked
        /// </summary>
        [Parameter]
        public string UncheckedIcon { get; set; } = Icons.Material.Filled.RadioButtonUnchecked;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        internal Color? ColorOverride { get; set; } = null;

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

        internal void Check()
        {
            Checked = true;
            StateHasChanged();
        }

        internal void Uncheck()
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

        private Color? GetColor()
        {
            if (Color != null)
                return Color;

            if (ColorOverride != null)
                return ColorOverride;

            return Color;
        }
    }
}