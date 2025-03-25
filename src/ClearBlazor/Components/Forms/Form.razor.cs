using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a form control that can contain child content and manage input validation.
    /// </summary>
    public partial class Form : ClearComponentBase
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// Represents the data model for the component. It is a public property that can be set to any object.
        /// </summary>
        [Parameter]
        public object Model { get; set; } = null!;

        /// <summary>
        /// Indicates if the label will be shown above the field
        /// </summary>
        [Parameter]
        public bool ShowLabels { get; set; } = false;

        /// <summary>
        /// Indicates whether the component is in read-only mode. When set to true, user input is disabled.
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// Specifies the location of validation error indicators. Defaults to displaying the error icon.
        /// </summary>
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

        /// <summary>
        /// Validates each input field asynchronously. 
        /// </summary>
        /// <returns>Returns a boolean indicating the success of the validation process.</returns>
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
