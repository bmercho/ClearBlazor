using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClearBlazor
{
    /// <summary>
    /// Base class for all input components
    /// </summary>
    public abstract class InputBase: ClearComponentBase
    {
        /// <summary>
        /// The label of the input
        /// </summary>
        [Parameter]
        public string? Label { get; set; } = null;

        /// <summary>
        /// The name of the field in the model
        /// </summary>
        [Parameter]
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// The tooltip of the input    
        /// </summary>
        [Parameter]
        public string ToolTip { get; set; } = "";

        /// <summary>
        /// The color of the input
        /// </summary>
        [Parameter]
        public Color? Color { get; set; }

        /// <summary>
        /// The size of the input
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// Indicates whether the input is disabled
        /// </summary>
        [Parameter]
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// Indicates whether the input is read only
        /// </summary>
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

            Label = GetLabel();

            ParentForm.AddPage(this);

            ValidationErrorLocation = ParentForm.ValidationErrorLocation;
        }

        internal virtual async Task<bool> ValidateField()
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

        protected string? GetLabel()
        {
            if (Label != null)
                return Label;

            if (ParentForm == null || ParentForm.Model == null || !ParentForm.ShowLabels)
                return Label;

            var prop = ParentForm.Model.GetType().GetProperty(FieldName);
            if (prop == null)
                return FieldName;
            var attrib = prop.GetCustomAttribute<DisplayNameAttribute>(true);
            if (attrib == null)
                return prop.Name;
            return attrib.DisplayName;
        }

        protected string GetLabelStyle()
        {
            string css = string.Empty;
            if (!IsValid)
                css += $"color: {Color.Error.Value}; ";

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