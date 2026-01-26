using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A control thats plays a video
    /// </summary>
    public partial class Video : ClearComponentBase,IBackground
    {
        /// <summary>
        /// The source uri of the video
        /// </summary>
        [Parameter]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        private string VideoStyle { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override string UpdateStyle(string css)
        {
            if (BackgroundColor != null)
                css += $"background-color: {BackgroundColor.Value}; ";


            if (!double.IsNaN(Width))
                VideoStyle += $"width: {Width}px; ";
            else
                VideoStyle += $"width: 100%; ";

            if (!double.IsNaN(Height))
                VideoStyle += $"height: {Height}px; ";
            else
                VideoStyle += $"height: 100%; ";

            if (MinWidth > 0)
                VideoStyle += $"min-width: {MinWidth}px; ";

            if (MinHeight > 0)
                VideoStyle += $"min-height: {MinHeight}px; ";

            if (MaxWidth != double.PositiveInfinity)
                VideoStyle += $"max-width: {MaxWidth}px; ";

            if (MaxHeight != double.PositiveInfinity)
                VideoStyle += $"max-height: {MaxHeight}px; ";

            VideoStyle += $"object-position: {AlignmentToPosition(HorizontalAlignment)} {AlignmentToPosition(VerticalAlignment)}; ";

            return css;
        }


        private static string AlignmentToPosition(Alignment? a) =>
            a == null ? "0%" :
            a == Alignment.Start ? "0%" :
            a == Alignment.End ? "100%" :
            a == Alignment.Center ? "50%" :
            a == Alignment.Stretch ? "50%" :
            throw new NotImplementedException();
    }
}