using Microsoft.AspNetCore.Components;
using System.Text;

namespace ClearBlazor
{
    public partial class Image : ClearComponentBase,IBackground
    {
        [Parameter]
        public string Source { get; set; } = string.Empty;

        [Parameter]
        public string Alternative { get; set; } = string.Empty;

        [Parameter]
        public ImageStretch Stretch { get; set; } = ImageStretch.Uniform;

        [Parameter]
        public StretchDirection StretchDirection { get; set; } = StretchDirection.Both;

        [Parameter]
        public string ImageId { get; set; } = string.Empty;

        [Parameter]
        public Color? BackgroundColour { get; set; }
        private string ImageStyle { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            //if (string.IsNullOrEmpty(Source))
            //    Source = "./AppData/Default.png";
        }

        protected override string UpdateStyle(string css)
        {
            if (BackgroundColour != null)
                css += $"background-color: {BackgroundColour.Value}; ";

            var size =
                 Stretch == ImageStretch.Fill ? "100% 100%" :
                 Stretch == ImageStretch.Uniform ? "contain" :
                 Stretch == ImageStretch.UniformToFill ? "cover" :
                 Stretch == ImageStretch.None ? "none" :
                 throw new NotImplementedException();

            if (!double.IsNaN(Width))
                ImageStyle += $"width: {Width}px; ";
            else
                ImageStyle += $"width: 100%; ";

            if (!double.IsNaN(Height))
                ImageStyle += $"height: {Height}px; ";
            else
                ImageStyle += $"height: 100%; ";

            if (MinWidth > 0)
                ImageStyle += $"min-width: {MinWidth}px; ";

            if (MinHeight > 0)
                ImageStyle += $"min-height: {MinHeight}px; ";

            if (MaxWidth != double.PositiveInfinity)
                ImageStyle += $"max-width: {MaxWidth}px; ";

            if (MaxHeight != double.PositiveInfinity)
                ImageStyle += $"max-height: {MaxHeight}px; ";

            switch (Stretch)
            {
                case ImageStretch.Fill:
                    break;

                case ImageStretch.Uniform:
                    ImageStyle += $"object-fit: contain; ";
                    break;

                case ImageStretch.UniformToFill:
                    ImageStyle += $"object-fit: cover; ";
                    break;

                case ImageStretch.None:
                    ImageStyle += $"object-fit: none; ";
                    break;
            }

            ImageStyle += $"object-position: {AlignmentToPosition(HorizontalAlignment)} {AlignmentToPosition(VerticalAlignment)}; ";

            return css;
        }


        private static string AlignmentToPosition(Alignment? a) =>
            a == null ? "0%" :
            a == Alignment.Start ? "0%" :
            a == Alignment.End ? "100%" :
            a == Alignment.Center ? "50%" :
            a == Alignment.Stretch ? "50%" :
            throw new NotImplementedException();

        // TODO: 'Stretch' behavior does not match XAML 100%. If image fits into space, centering (50%) is correct.
        // If image exceeds available space, XAML aligns at start instead of center.
    }
}