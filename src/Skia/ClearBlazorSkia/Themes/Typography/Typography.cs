namespace ClearBlazor
{
    public class Typography
    {

        public TypographyBase Default { get; set; } = new TypographyBase
        {
            FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
            FontSize = 14,
            FontWeight = 400,
            LineHeight = 1.43,
            LetterSpacing = ".01071em",
        };

        public TypographyBase H1 { get; set; } = new TypographyBase
        {
            FontSize = 96,
            FontWeight = 300,
            LineHeight = 1.167,
            LetterSpacing = "-.01562em"
        };

        public TypographyBase H2 { get; set; } = new TypographyBase
        {
            FontSize = 60,
            FontWeight = 300,
            LineHeight = 1.2,
            LetterSpacing = "-.00833em"
        };

        public TypographyBase H3 { get; set; } = new TypographyBase
        {
            FontSize = 48,
            FontWeight = 400,
            LineHeight = 1.167,
            LetterSpacing = "0"
        };

        public TypographyBase H4 { get; set; } = new TypographyBase
        {
            FontSize = 34,
            FontWeight = 400,
            LineHeight = 1.235,
            LetterSpacing = ".00735em"
        };

        public TypographyBase H5 { get; set; } = new TypographyBase
        {
            FontSize = 24,
            FontWeight = 400,
            LineHeight = 1.334,
            LetterSpacing = "0"
        };

        public TypographyBase H6 { get; set; } = new TypographyBase
        {
            FontSize = 20,
            FontWeight = 500,
            LineHeight = 1.6,
            LetterSpacing = ".0075em"
        };

        public TypographyBase Subtitle1 { get; set; } = new TypographyBase
        {
            FontSize = 16,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".0938em"
        };

        public TypographyBase Subtitle2 { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 500,
            LineHeight = 1.57,
            LetterSpacing = ".00714em"
        };

        public TypographyBase Body1 { get; set; } = new TypographyBase
        {
            FontSize = 16,
            FontWeight = 400,
            LineHeight = 1.5,
            LetterSpacing = ".00938em"
        };

        public TypographyBase Body2 { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 400,
            LineHeight = 1.43,
            LetterSpacing = ".01071em"
        };

        public TypographyBase ButtonVerySmall { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ButtonSmall { get; set; } = new TypographyBase
        {
            FontSize = 13,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ButtonNormal { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ButtonLarge { get; set; } = new TypographyBase
        {
            FontSize = 15,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ButtonVeryLarge { get; set; } = new TypographyBase
        {
            FontSize = 16,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };

        public TypographyBase AvatarVerySmall { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 700,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };

        public TypographyBase AvatarSmall { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 700,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase AvatarNormal { get; set; } = new TypographyBase
        {
            FontSize = 15,
            FontWeight = 700,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase AvatarLarge { get; set; } = new TypographyBase
        {
            FontSize = 16,
            FontWeight = 700,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase AvatarVeryLarge { get; set; } = new TypographyBase
        {
            FontSize = 17,
            FontWeight = 700,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };

        public TypographyBase ListItemVerySmall { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };

        public TypographyBase ListItemSmall { get; set; } = new TypographyBase
        {
            FontSize = 13,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ListItemNormal { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ListItemLarge { get; set; } = new TypographyBase
        {
            FontSize = 15,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase ListItemVeryLarge { get; set; } = new TypographyBase
        {
            FontSize = 16,
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };

        public TypographyBase Caption { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 400,
            LineHeight = 1.66,
            LetterSpacing = ".03333em"
        };

        public TypographyBase Overline { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 400,
            LineHeight = 2.66,
            LetterSpacing = ".08333em"
        };

        public TypographyBase InputVerySmall { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputSmall { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputNormal { get; set; } = new TypographyBase
        {
            FontSize = 16,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputLarge { get; set; } = new TypographyBase
        {
            FontSize = 20,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputVeryLarge { get; set; } = new TypographyBase
        {
            FontSize = 24,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };

        public TypographyBase InputLabelVerySmall { get; set; } = new TypographyBase
        {
            FontSize = 7,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputLabelSmall { get; set; } = new TypographyBase
        {
            FontSize = 12,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputLabelNormal { get; set; } = new TypographyBase
        {
            FontSize = 14,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputLabelLarge { get; set; } = new TypographyBase
        {
            FontSize = 17,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
        public TypographyBase InputLabelVeryLarge { get; set; } = new TypographyBase
        {
            FontSize = 18,
            FontWeight = 400,
            LineHeight = 1.75,
            LetterSpacing = ".02857em",
            //TextTransform = TextTransform.Uppercase
        };
    }
}
