using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace ClearBlazor
{
    public partial class NumericInput<TItem> : ContainerInputBase<TItem>
    {
        [Parameter]
        public int DecimalPlaces { get; set; } = 0;

        [Parameter]
        public bool AllowNegativeNumbers { get; set; } = true;

        [Parameter]
        public bool ShowSpinButtons { get; set; } = false;

        [Parameter]
        public TItem? Step
        {
            get => GetValueFromString(_step);
            set => _step = GetValueAsString(value);
        }

        [Parameter]
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        private string _step = "1";

        protected string? StringValue = null;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            try
            {
                if (StringValue != null &&
                    (StringValue.EndsWith(Culture.NumberFormat.NumberDecimalSeparator) ||
                     StringValue.EndsWith("-") ||
                GetValueAsString(Value) != StringValue &&
                     Value.Equals(GetValueFromString(StringValue))))
                    return;

                StringValue = GetValueAsString(Value);
            }
            catch { }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("window.clearBlazor.numberInput.initialize", id, IsFloatingNumber(), AllowNegativeNumbers,
                                                Culture.NumberFormat.NumberDecimalSeparator);
            }
        }

        protected async Task OnInput(ChangeEventArgs e)
        {
            StringValue = e.Value as string;
            Value = GetValueFromString(StringValue);
            if (Immediate)
                await HandleValueChange();
        }

        protected async Task OnChange(ChangeEventArgs e)
        {
            StringValue = GetValueAsString(Value);
            await HandleValueChange();
        }

        protected override string GetInputType()
        {
            if (ShowSpinButtons)
                return "number";
            else
                return "text";
        }

        protected TItem? GetStep()
        {
            return Step;
        }

        protected override async Task ClearEntry()
        {
            await JSRuntime.InvokeVoidAsync("window.clearBlazor.numericInput.setValue", id, string.Empty);
            await TextInput.FocusAsync();
        }

        private string GetValueAsString(TItem? value)
        {
            if (value is null) return string.Empty;

            if (value is float floatValue) return floatValue.ToString(Culture);

            if (value is double doubleValue) return doubleValue.ToString(Culture);

            if (value is decimal decimalValue) return decimalValue.ToString(Culture);

            // All numbers without decimal places work fine by default
            return value?.ToString() ?? string.Empty;
        }

        public override async Task<bool> ValidateField()
        {
            IsValid = true;
            ValidationErrorMessages.Clear();
            await Task.CompletedTask;
            return IsValid;
        }

        private TItem? GetValueFromString(string? stringValue)
        {
            try
            {
                if (stringValue == null)
                    return default;
                Type t = Nullable.GetUnderlyingType(typeof(TItem)) ?? typeof(TItem);
                if (stringValue.Contains(Culture.NumberFormat.CurrencyDecimalSeparator))
                    return (TItem)Convert.ChangeType(stringValue, t, Culture);
                else
                    return (TItem)Convert.ChangeType(stringValue, t);
            }
            catch
            {
                return default;
            }
        }

        private bool IsFloatingNumber() =>
                        typeof(TItem) == typeof(float) ||
                        typeof(TItem) == typeof(float?) ||
                        typeof(TItem) == typeof(double) ||
                        typeof(TItem) == typeof(double?) ||
                        typeof(TItem) == typeof(decimal) ||
                        typeof(TItem) == typeof(decimal?);

    }
}