using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public abstract class ContainerInputBase<TItem>: InputBase
    {
        [Parameter]
        public TItem? Value { get; set; } = default;

        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        [Parameter]
        public bool Immediate { get; set; } = false;

        [Parameter]
        public TextEditFillMode TextEditFillMode { get; set; } = TextEditFillMode.Outline;

        [Parameter]
        public TextWrap TextWrapping { get; set; } = TextWrap.NoWrap;

        [Parameter]
        public TextTrimming TextTrimming { get; set; } = TextTrimming.None;

        [Parameter]
        public bool IsTextSelectionEnabled { get; set; } = false;

        [Parameter]
        public string Placeholder { get; set; } = "";

        [Parameter]
        public bool Clearable { get; set; } = false;

        [Parameter]
        public int? DebounceInterval { get; set; } = null;

        protected bool MouseOver { get; set; } = false;
        protected bool HasFocus { get; set; } = false;
        protected string id = string.Empty;
        protected ElementReference TextInput { get; set; }
        int _debounceInterval = 0;
        private bool _debouncing = false;


        protected abstract string GetInputType();
        protected abstract Task ClearEntry();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (string.IsNullOrEmpty(id))
                id = Guid.NewGuid().ToString();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (DebounceInterval == null)
                _debounceInterval = ThemeManager.CurrentTheme.TextEntryDebounceInterval;
            else
                _debounceInterval = (int)DebounceInterval;
        }

        protected Color GetBackgroundColour()
        {
            switch (TextEditFillMode)
            {
                case TextEditFillMode.None:
                    return Color.Background;
                case TextEditFillMode.Underline:
                    return Color.Background;
                case TextEditFillMode.Filled:
                    if (MouseOver && !IsDisabled)
                        return Color.BackgroundGrey.Darken(0.2);
                    else
                        return Color.BackgroundGrey.Darken(0.1);
                case TextEditFillMode.Outline:
                    return Color.Background;
                default:
                    return Color.Background;
            }
        }

        protected async Task HandleValueChange()
        {
            if (_debouncing)
                return;

            if (_debounceInterval == 0 || !Immediate)
            {
                await ValidateField();
                await ValueChanged.InvokeAsync(Value);
                //StateHasChanged();
                return;
            }

            _debouncing = true;
            try
            {
                PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_debounceInterval));
                while (await timer.WaitForNextTickAsync())
                {
                    timer.Dispose();
                    _debouncing = false;
                    await ValidateField();
                    await ValueChanged.InvokeAsync(Value);
                    StateHasChanged();
                }
            }
            finally
            {
                _debouncing = false;
            }
        }

        protected string GetBorderThickness()
        {
            switch (TextEditFillMode)
            {
                case TextEditFillMode.None:
                    return "0";
                case TextEditFillMode.Underline:
                    if (HasFocus && !IsDisabled)
                        return "0,0,2,0";
                    else
                        return "0,0,1,0";
                default:
                case TextEditFillMode.Filled:
                    if (HasFocus && !IsDisabled)
                        return "2";
                    return "1";
                case TextEditFillMode.Outline:
                    if (HasFocus && !IsDisabled)
                        return "2";
                    return "1";
            }
        }
        protected Color GetBorderColour()
        {
            if (IsDisabled)
            {
                if (TextEditFillMode == TextEditFillMode.None)
                    return Color.Transparent;
                return Color.BackgroundGrey.Darken(0.3);
            }

            switch (TextEditFillMode)
            {
                case TextEditFillMode.None:
                    return Color.Transparent;
                case TextEditFillMode.Underline:
                    if (!IsValid)
                        return Color.Error;
                    else if (HasFocus)
                        return Color.Primary;
                    else if (MouseOver)
                        return Color.BackgroundGrey.Darken(0.5);
                    else
                        return Color.BackgroundGrey.Darken(0.3);
                case TextEditFillMode.Filled:
                default:
                    if (!IsValid)
                        return Color.Error;
                    else if (HasFocus)
                        return Color.Primary;
                    else if (MouseOver)
                        return Color.BackgroundGrey.Darken(0.5);
                    else
                        return Color.BackgroundGrey.Darken(0.3);
                case TextEditFillMode.Outline:
                    if (!IsValid)
                        return Color.Error;
                    else if (HasFocus)
                        return Color.Primary;
                    else if (MouseOver)
                        return Color.BackgroundGrey.Darken(0.5);
                    else
                        return Color.BackgroundGrey.Darken(0.3);
            }
        }

        protected string GetCornerRadius()
        {
            switch (TextEditFillMode)
            {
                case TextEditFillMode.None:
                    return "0";
                case TextEditFillMode.Underline:
                    return "0";
                case TextEditFillMode.Filled:
                    return "4";
                case TextEditFillMode.Outline:
                    return "4";
                default:
                    return "4";
            }
        }

        protected virtual string ComputeInputStyle()
        {
            string css = string.Empty;
            if (IsDisabled)
                css += $"color: {ThemeManager.CurrentPalette.BackgroundDisabled.Value}; ";
            else if (!IsValid)
                css += $"color: {Color.Error.Value}; ";
            else if (Colour != null)
                css += $"color: {Colour.Value}; ";

            TypographyBase typo = ThemeManager.CurrentTheme.Typography.InputNormal;
            switch (Size)
            {
                case Size.VerySmall:
                    typo = ThemeManager.CurrentTheme.Typography.InputVerySmall;
                    break;
                case Size.Small:
                    typo = ThemeManager.CurrentTheme.Typography.InputSmall;
                    break;
                case Size.Normal:
                    typo = ThemeManager.CurrentTheme.Typography.InputNormal;
                    break;
                case Size.Large:
                    typo = ThemeManager.CurrentTheme.Typography.InputLarge;
                    break;
                case Size.VeryLarge:
                    typo = ThemeManager.CurrentTheme.Typography.InputVeryLarge;
                    break;
            }
            css += $"font-weight: {typo.FontWeight}; ";
            css += $"font-style: {GetFontStyle(typo.FontStyle)}; ";
            css += $"font-size: {typo.FontSize}; ";
            css += $"font-family: {string.Join(",", typo.FontFamily)}; ";

            css += $"white-space: {(TextWrapping == TextWrap.NoWrap ? "nowrap" : "pre-line")}; ";

            if (TextWrapping == TextWrap.Wrap)
                css += $"overflow-wrap: break-word; ";

            if (TextTrimming != TextTrimming.None)
                css += $"text-overflow: ellipsis; ";

            if (!IsTextSelectionEnabled)
                css += "user-select: none; -ms-user-select: none; cursor: default; ";

            css += "border:0; background:{transparent}; outline:none; ";

            css += $"padding: {GetPadding()};";
            return css;
        }


        protected string GetPadding()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    if (HasFocus && !IsDisabled)
                    {
                        if (TextEditFillMode == TextEditFillMode.Underline)
                            return "2px 2px 2px 1px";
                        else if (TextEditFillMode == TextEditFillMode.Outline || TextEditFillMode == TextEditFillMode.Filled)
                            return "2px 2px 2px 1px";
                    }
                    return "4px";
                case Size.Small:
                    if (HasFocus && !IsDisabled)
                    {
                        if (TextEditFillMode == TextEditFillMode.Underline)
                            return "4px 4px 4px 3px";
                        else if (TextEditFillMode == TextEditFillMode.Outline || TextEditFillMode == TextEditFillMode.Filled)
                            return "4px 4px 4px 2px";
                    }
                    return "4px";
                case Size.Normal:
                default:
                    if (HasFocus && !IsDisabled)
                    {
                        if (TextEditFillMode == TextEditFillMode.Underline)
                            return "6px 6px 6px 5px";
                        else if (TextEditFillMode == TextEditFillMode.Outline || TextEditFillMode == TextEditFillMode.Filled)
                            return "6px 6px 6px 4px";
                    }
                    return "6px";
                case Size.Large:
                    if (HasFocus && !IsDisabled)
                    {
                        if (TextEditFillMode == TextEditFillMode.Underline)
                            return "8px 8px 8px 7px";
                        else if (TextEditFillMode == TextEditFillMode.Outline || TextEditFillMode == TextEditFillMode.Filled)
                            return "8px 8px 8px 6px";
                    }
                    return "8px";
                case Size.VeryLarge:
                    if (HasFocus && !IsDisabled)
                    {
                        if (TextEditFillMode == TextEditFillMode.Underline)
                            return "10px 10px 10px 9px";
                        else if (TextEditFillMode == TextEditFillMode.Outline || TextEditFillMode == TextEditFillMode.Filled)
                            return "10px 10px 10px 8px";
                    }
                    return "10px";
            }
        }

        protected async Task OnMouseEnter(MouseEventArgs e)
        {
            MouseOver = true;
            if (ToolTipElement == null)
                await Task.CompletedTask;
            else
                await ToolTipElement.ShowToolTip();
            StateHasChanged();
        }

        protected async Task OnMouseLeave(MouseEventArgs e)
        {
            MouseOver = false;
            ToolTipElement?.HideToolTip();
            await Task.CompletedTask;
            StateHasChanged();
        }

        protected async Task OnFocusIn(FocusEventArgs e)
        {
            HasFocus = true;
            await Task.CompletedTask;
        }

        protected async Task OnFocusOut(FocusEventArgs e)
        {
            HasFocus = false;
            await ValidateField();
        }
    }
}