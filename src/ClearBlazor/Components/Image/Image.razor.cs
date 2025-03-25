using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control thats shows an image
    /// </summary>
    public partial class Image : ClearComponentBase,IBackground
    {
        /// <summary>
        /// The source uri of the image
        /// </summary>
        [Parameter]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// The alternative string show if the source does not exist
        /// </summary>
        [Parameter]
        public string Alternative { get; set; } = string.Empty;

        /// <summary>
        /// How the image is shown in regard to aspect ratio and stretching
        /// </summary>
        [Parameter]
        public ImageStretch Stretch { get; set; } = ImageStretch.Uniform;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        private string ImageStyle { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override string UpdateStyle(string css)
        {
            if (BackgroundColor != null)
                css += $"background-color: {BackgroundColor.Value}; ";

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