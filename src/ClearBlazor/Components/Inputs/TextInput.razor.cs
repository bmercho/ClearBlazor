using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a text input field
    /// </summary>
    public partial class TextInput:ContainerInputBase<string>
    {
        /// <summary>
        /// When used in a form, indicates if the field is required
        /// </summary>
        [Parameter]
        public bool Required { get; set; } = false;

        /// <summary>
        /// Specifies the maximum length of a value. It can be set to a nullable integer, allowing for no limit if null.
        /// </summary>
        [Parameter]
        public int? MaxLength { get; set; } = null;

        /// <summary>
        /// Specifies the number of lines, allowing for a nullable integer value. Defaults to 1 if not set.
        /// </summary>
        [Parameter]
        public int? Lines { get; set; } = 1;

        /// <summary>
        /// Indicates whether the input field is for a password. Defaults to false.
        /// </summary>
        [Parameter]
        public bool IsPassword { get; set; } = false;

        protected string? StringValue = null;

        private string? RequiredErrorMessage = null;
        private int MaxAllowableLength = -1;
        private string? MaxLengthErrorMessage = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            (Required, RequiredErrorMessage) = IsRequired(Required);
            (MaxAllowableLength, MaxLengthErrorMessage) = GetMaxLength(MaxLength);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (StringValue == null || StringValue != Value)
                StringValue = Value;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (MaxLength != null)
                    await JSRuntime.InvokeVoidAsync("window.clearBlazor.textInput.initialize", id, MaxLength);
            }
        }

        protected (int, string?) GetMaxLength(int? defaultMaxLength)
        {
            if (ParentForm == null || ParentForm.Model == null)
                return (defaultMaxLength ?? -1, null);

            var prop = ParentForm.Model.GetType().GetProperty(FieldName);
            if (prop == null)
                return (defaultMaxLength ?? -1, null);
            var attrib = prop.GetCustomAttribute<MaxLengthAttribute>(true);
            if (attrib == null)
                return (defaultMaxLength ?? -1, null);
            return (attrib.Length, attrib.ErrorMessage);
        }

        protected async Task OnInput(ChangeEventArgs e)
        {
            Value = e.Value as string;
            StringValue = Value;
            if (Immediate)
                await HandleValueChange();
            else if (Clearable)
                StateHasChanged();
        }

        protected async Task OnChange(ChangeEventArgs e)
        {
            await HandleValueChange();
        }

        protected override string GetInputType()
        {
            if (IsPassword)
                return "password";
            else
                return "text";
        }

        protected override async Task ClearEntry()
        {
            StringValue = Value = string.Empty;
            await ValueChanged.InvokeAsync(string.Empty);
            await TextInput.FocusAsync();
        }

        internal override async Task<bool> ValidateField()
        {
            IsValid = true;
            ValidationErrorMessages.Clear();
            if (Required && string.IsNullOrEmpty(Value))
            {
                IsValid = false;
                if (RequiredErrorMessage != null)
                    ValidationErrorMessages.Add(RequiredErrorMessage);
            }
            if (MaxAllowableLength > -1 && !string.IsNullOrEmpty(Value) && Value.Length > MaxAllowableLength)
            {
                IsValid = false;
                if (MaxLengthErrorMessage != null)
                    ValidationErrorMessages.Add(MaxLengthErrorMessage);
            }
            await Task.CompletedTask;
            return IsValid;
        }
    }
}