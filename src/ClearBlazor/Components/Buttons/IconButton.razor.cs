using Microsoft.AspNetCore.Components;
using System.Text;

namespace ClearBlazor
{
    /// <summary>
    /// A button that shows an icon and possibly text.
    /// </summary>
    public partial class IconButton : Button
    {
        /// <summary>
        /// Used when the IconButton is inside an AppBar
        /// Indicates if this icon is a leading icon. (otherwise its a trailing icon)
        /// Leading and trailing icons get slightly different colors.
        /// </summary>
        [Parameter]
        public bool LeadingIcon { get; set; } = true;

        protected Size IconSize { get; set; }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            
            if (ButtonStyle == null)
                StyleOverride = ClearBlazor.ButtonStyle.LabelOnly;
        }

        protected override string GetBorderRadius()
        {
            switch (Shape)
            {
                case ContainerShape.Circle:
                    return "border-radius:50%; ";
                case ContainerShape.Square:
                    return "";
                case ContainerShape.Rounded:
                    return "border-radius:4px; ";
                case ContainerShape.FullyRounded:
                    return "border-radius:4px; ";
            }
            return "";
        }

        protected override string GetIconSize(Size size)
        {
            IconSize = size;
            switch (size)
            {
                case Size.VerySmall:
                    return "width:19px; ";
                case Size.Small:
                    return "width:25px; ";
                case Size.Normal:
                    return "width:38px; ";
                case Size.Large:
                    return "width:43px; ";
                case Size.VeryLarge:
                    return "width:50px; ";
            }
            return "width:38px; ";
        }

        protected override string GetHeight(Size size)
        {
            switch (size)
            {
                case Size.VerySmall:
                    return "height:19px; ";
                case Size.Small:
                    return "height:25px; ";
                case Size.Normal:
                    return "height:38px; ";
                case Size.Large:
                    return "height:43px; ";
                case Size.VeryLarge:
                    return "height:50px; ";
            }
            return "height:38px; ";
        }

        protected override string GetPadding(Size size)
        {
            return "";
        }
        protected override void ComputeOwnClasses(StringBuilder sb)
        {
            base.ComputeOwnClasses(sb);
            if (!Disabled)
                sb.Append("clear-ripple-icon ");
        }
    }
}