using System.Text;

namespace ClearBlazor
{
    public partial class IconButton : Button
    {
        protected Size IconSize { get; set; }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            
            if (ButtonStyle == null)
                ButtonStyle = TextEditFillMode.None;

            VerticalAlignment = Alignment.Center;
        }

        protected override string GetBorderRadius()
        {
            switch (Shape)
            {
                case ContainerShape.Circle:
                    return "border-radius:50%; ";
                case ContainerShape.Square:
                    return "";
                case ContainerShape.SquareRounded:
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
                sb.Append("clear-ripple clear-ripple-icon ");
        }
    }
}