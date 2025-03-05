using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClearBlazor
{
    public abstract class InputBase: ClearComponentBase
    {
        [Parameter]
        public string? Label { get; set; } = null;
        [Parameter]
        public string FieldName { get; set; } = string.Empty;
        [Parameter]
        public string ToolTip { get; set; } = "";

        [Parameter]
        public Color? Color { get; set; }

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public bool IsDisabled { get; set; } = false;

        [Parameter]
        public bool IsReadOnly { get; set; } = false;

        protected Form? ParentForm { get; set; } = null;
        protected List<string> ValidationErrorMessages { get; set; } = new List<string>();
        protected bool IsValid { get; set; } = true;
        protected string ValidationMessage => string.Join("\n\r", ValidationErrorMessages);
        protected ValidationErrorLocation ValidationErrorLocation { get; set; } = ValidationErrorLocation.ErrorIcon;
        protected ToolTip? ToolTipElement { get; set; } = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ParentForm = GetParentForm();

            if (ParentForm == null)
                return;

            ParentForm.AddPage(this);

            ValidationErrorLocation = ParentForm.ValidationErrorLocation;
        }

        public virtual async Task<bool> ValidateField()
        {
            ValidationErrorMessages.Clear();
            IsValid = true;
            await Task.CompletedTask;
            return IsValid;
        }

        private Form? GetParentForm()
        {
            var parent = Parent;
            while (parent != null && !(parent is Form))
            {
                parent = parent.Parent;
            }
            if (parent == null)
                return null;
            else
                return parent as Form;
        }

        protected (bool,string?) IsRequired(bool defaultRequired)
        {
            if (ParentForm == null || ParentForm.Model == null)
                return (defaultRequired, null);

            var prop = ParentForm.Model.GetType().GetProperty(FieldName);
            if (prop == null)
                return (defaultRequired, null);
            var attrib = prop.GetCustomAttribute<RequiredAttribute>(true);
            if (attrib == null)
                return (defaultRequired, null);
            return (!attrib.AllowEmptyStrings, attrib.ErrorMessage);
        }

        protected string GetLabelStyle()
        {
            string css = string.Empty;
            if (IsDisabled)
                css += $"color: {ThemeManager.CurrentColorScheme.BackgroundDisabled.Value}; ";
            else if (!IsValid)
                css += $"color: {Color.Error.Value}; ";
            //else if (Color != null)
            //    css += $"color: {Color.Value}; ";

            TypographyBase typo = ThemeManager.CurrentTheme.Typography.InputLabelNormal;
            switch (Size)
            {
                case Size.VerySmall:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelVerySmall;
                    break;
                case Size.Small:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelSmall;
                    break;
                case Size.Normal:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelNormal;
                    break;
                case Size.Large:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelLarge;
                    break;
                case Size.VeryLarge:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelVeryLarge;
                    break;
            }
            css += $"font-weight: {typo.FontWeight}; ";
            css += $"font-style: {GetFontStyle(typo.FontStyle)}; ";
            css += $"font-size: {typo.FontSize}; ";
            css += $"font-family: {string.Join(",", typo.FontFamily)}; ";
            return css;
        }

        protected TypographyBase GetTypograghy()
        {
            TypographyBase typo = ThemeManager.CurrentTheme.Typography.InputLabelNormal;
            switch (Size)
            {
                case Size.VerySmall:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelVerySmall;
                    break;
                case Size.Small:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelSmall;
                    break;
                case Size.Normal:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelNormal;
                    break;
                case Size.Large:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelLarge;
                    break;
                case Size.VeryLarge:
                    typo = ThemeManager.CurrentTheme.Typography.InputLabelVeryLarge;
                    break;
            }
            return typo;
        }

        protected bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}