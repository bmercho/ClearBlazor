using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    /// <summary>
    /// Base class for all container input components   
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ContainerInputBase<TItem>: InputBase
    {
        /// <summary>
        /// The value of the input
        /// </summary>
        [Parameter]
        public TItem? Value { get; set; } = default;

        /// <summary>
        /// Event callback when the value changes
        /// </summary>
        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        /// <summary>
        /// Indicates whether the input is immediate. ie Changes the Value as soon as input is received,
        /// otherwise, Value is updated when the user presses Enter or the input loses focus.
        /// Defaults to false.
        /// </summary>
        [Parameter]
        public bool Immediate { get; set; } = false;

        /// <summary>
        /// The fill mode of the text edit
        /// </summary>
        [Parameter]
        public InputContainerStyle InputContainerStyle { get; set; } = InputContainerStyle.Outlined;

        /// <summary>
        /// The text wrapping of the text edit
        /// </summary>
        [Parameter]
        public TextWrap TextWrapping { get; set; } = TextWrap.NoWrap;

        /// <summary>
        /// The text trimming of the text edit
        /// </summary>
        [Parameter]
        public TextTrimming TextTrimming { get; set; } = TextTrimming.None;

        /// <summary>
        /// Indicates whether text selection is enabled
        /// </summary>
        [Parameter]
        public bool IsTextSelectionEnabled { get; set; } = false;

        /// <summary>
        /// The placeholder of the input    
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; } = "";

        /// <summary>
        /// Indicates whether the input is clearable via a clear button
        /// </summary>
        [Parameter]
        public bool Clearable { get; set; } = false;

        /// <summary>
        /// The debounce interval in milliseconds. Defaults to null (effectively 0).
        /// </summary>
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

        protected Color? GetBackgroundColor()
        {
            switch (InputContainerStyle)
            {
                case InputContainerStyle.LabelOnly:
                case InputContainerStyle.Underlined:
                case InputContainerStyle.Outlined:
                    return null;
                case InputContainerStyle.Filled:
                    if (IsDisabled)
                        return ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.12);
                    return ThemeManager.CurrentColorScheme.SurfaceContainerHighest;
                default:
                    return null;
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
            switch (InputContainerStyle)
            {
                case InputContainerStyle.LabelOnly:
                    return "0";
                case InputContainerStyle.Underlined:
                    if (HasFocus && !IsDisabled)
                        return "0,0,2,0";
                    else
                        return "0,0,1,0";
                default:
                case InputContainerStyle.Outlined:
                    if (HasFocus && !IsDisabled)
                        return "2";
                    return "1";
                case InputContainerStyle.Filled:
                    return "0";
            }
        }
        protected Color? GetBorderColor()
        {
            if (IsDisabled)
            {
                if (InputContainerStyle == InputContainerStyle.LabelOnly)
                    return null;
                return ThemeManager.CurrentColorScheme.OnSurface;
            }

            switch (InputContainerStyle)
            {
                case InputContainerStyle.LabelOnly:
                    return null;
                case InputContainerStyle.Underlined:
                case InputContainerStyle.Outlined:
                default:
                    if (!IsValid)
                        return Color.Error;
                    else if (HasFocus)
                        return Color.Primary;
                    else if (MouseOver)
                        return ThemeManager.CurrentColorScheme.Outline.Darken(0.1);
                    else
                        return ThemeManager.CurrentColorScheme.Outline;
                case InputContainerStyle.Filled:
                    return null;
            }
        }

        protected string GetCornerRadius()
        {
            switch (InputContainerStyle)
            {
                case InputContainerStyle.LabelOnly:
                    return "0";
                case InputContainerStyle.Underlined:
                    return "0";
                case InputContainerStyle.Filled:
                    return "4";
                case InputContainerStyle.Outlined:
                    return "4";
                default:
                    return "4";
            }
        }

        protected virtual string ComputeInputStyle()
        {
            string css = string.Empty;
            if (IsDisabled)
                css += $"color: {ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.12).Value}; ";
            else if (!IsValid)
                css += $"color: {Color.Error.Value}; ";
            else
            {
                var color = GetBackgroundColor();
                if (color == null)
                    css += $"color: {ThemeManager.CurrentColorScheme.OnSurfaceVariant.Value}; ";
                else
                    css += $"color: {Color.GetAssocTextColor(color).Value}; ";
            }

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
            {
                css += "user-select: none; -ms-user-select: none; ";
                if (!ClearComponentBase.Dragging)
                    css += "cursor:default; ";
            }

            css += "border:0; background-color:transparent; outline:none; ";

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
                        if (InputContainerStyle == InputContainerStyle.Underlined)
                            return "2px 2px 1px 2px";
                        else if (InputContainerStyle == InputContainerStyle.Outlined)
                            return "1px 2px 2px 1px";
                    }
                    return "2px";
                case Size.Small:
                    if (HasFocus && !IsDisabled)
                    {
                        if (InputContainerStyle == InputContainerStyle.Underlined)
                            return "4px 4px 3px 4px";
                        else if (InputContainerStyle == InputContainerStyle.Outlined)
                            return "3px 4px 4px 3px";
                    }
                    return "4px";
                case Size.Normal:
                default:
                    if (HasFocus && !IsDisabled)
                    {
                        if (InputContainerStyle == InputContainerStyle.Underlined)
                            return "6px 6px 5px 6px";
                        else if (InputContainerStyle == InputContainerStyle.Outlined)
                            return "5px 6px 6px 5px";
                    }
                    return "6px";
                case Size.Large:
                    if (HasFocus && !IsDisabled)
                    {
                        if (InputContainerStyle == InputContainerStyle.Underlined)
                            return "8px 8px 7px 8px";
                        else if (InputContainerStyle == InputContainerStyle.Outlined)
                            return "7px 8px 8px 7px";
                    }
                    return "8px";
                case Size.VeryLarge:
                    if (HasFocus && !IsDisabled)
                    {
                        if (InputContainerStyle == InputContainerStyle.Underlined)
                            return "10px 10px 9px 10px";
                        else if (InputContainerStyle == InputContainerStyle.Outlined)
                            return "9px 10px 10px 9px";
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
                ToolTipElement.ShowToolTip();
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
            StateHasChanged();
        }

        protected async Task OnFocusOut(FocusEventArgs e)
        {
            HasFocus = false;
            await ValidateField();
            StateHasChanged();
        }
    }
}