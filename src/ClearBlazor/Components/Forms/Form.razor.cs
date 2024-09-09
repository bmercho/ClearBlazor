using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Form : ClearComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public object Model { get; set; } = null!;

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public ValidationErrorLocation ValidationErrorLocation { get; set; } = ValidationErrorLocation.ErrorIcon;

        protected List<InputBase> Inputs = new List<InputBase>();

        internal void AddPage(InputBase input)
        {
            Inputs.Add(input);
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; ";
            return css;
        }

        public async Task<bool> Validate()
        {
            foreach (var input in Inputs)
            {
                await input.ValidateField();
            }
            await Task.CompletedTask;
            return true;
        }
    }
}
