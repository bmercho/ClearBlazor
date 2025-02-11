namespace ClearBlazor
{
    public class TypographyBase
    { 
        public string[] FontFamily { get; set; } = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" };

        public int FontWeight { get; set; } = 400;

        public double FontSize { get; set; } = 14;

        public FontStyle FontStyle { get; set; } = FontStyle.Normal;

        public double LineHeight { get; set; } = 1.43;

        public string LetterSpacing { get; set; } = ".01071em";

        public TextTransform? TextTransform { get; set; } = null;

        public TextWrap TextWrapping { get; set; } = TextWrap.Wrap;

        public bool TextTrimming { get; set; } = false;
    }
}
