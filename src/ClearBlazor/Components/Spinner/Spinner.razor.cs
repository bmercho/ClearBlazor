using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Spinner:ClearComponentBase
    {
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        protected override string UpdateStyle(string css)
        {
            int borderSize = 0;
            int size = 0;
            switch (Size)
            {
                case Size.VerySmall:
                    borderSize = 4;
                    size = 10;
                    break;
                case Size.Small:
                    borderSize = 6;
                    size = 15;
                    break;
                case Size.Normal:
                    borderSize = 8;
                    size = 20;
                    break;
                case Size.Large:
                    borderSize = 10;
                    size = 25;
                    break;
                case Size.VeryLarge:
                    borderSize = 12;
                    size = 30;
                    break;
            }
            css += $"border: {borderSize}px solid silver; border-top: {borderSize}px solid #337AB7; " +
                   $"border-radius: 50%; width: {size}px; height: {size}px; " +
                   $"animation: spin 700ms linear infinite; top: 40 %;";

            return css;
        }

}
}