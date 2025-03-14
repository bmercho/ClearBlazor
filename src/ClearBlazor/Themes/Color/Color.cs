using System.Globalization;

namespace ClearBlazor;

public class Color:IEquatable<Color>
{
    private const double Epsilon = 0.000000000000001;

    public static Color Primary { get; private set; } = null!;
    public static Color Secondary { get; private set; } = null!;
    public static Color Tertiary { get; private set; } = null!;
    public static Color Info { get; private set; } = null!;
    public static Color Success { get; private set; } = null!;
    public static Color Warning { get; private set; } = null!;
    public static Color Error { get; private set; } = null!;
    public static Color Dark { get; private set; } = null!;
    public static Color Light { get; private set; } = null!;
    public static Color Transparent { get; private set; } = null!;
    public static Color Background { get; set; } = null!;
    public static Color BackgroundGrey { get; set; } = null!;
    public static Color Surface { get; set; } = null!;
    public static Color SurfaceContainerHigh { get; set; } = null!;
    
    public static void SetColors()
    {
        Primary = ThemeManager.CurrentColorScheme.Primary;
        Secondary = ThemeManager.CurrentColorScheme.Secondary;
        Tertiary = ThemeManager.CurrentColorScheme.Tertiary;
        Info = ThemeManager.CurrentColorScheme.Info;
        Success = ThemeManager.CurrentColorScheme.Success;
        Warning = ThemeManager.CurrentColorScheme.Warning;
        Error = ThemeManager.CurrentColorScheme.Error;
        Dark = ThemeManager.CurrentColorScheme.Dark;
        Light = ThemeManager.CurrentColorScheme.GrayLighter;
        Transparent = new("#00000000");

        Background = ThemeManager.CurrentColorScheme.Background;
        BackgroundGrey = ThemeManager.CurrentColorScheme.BackgroundGrey;
        Surface = ThemeManager.CurrentColorScheme.Surface;
        SurfaceContainerHigh = ThemeManager.CurrentColorScheme.SurfaceContainerHigh;
    }

    public static Color GetAssocTextColor(Color? color)
    {
        if (color == null)
            return ThemeManager.CurrentColorScheme.OnPrimary;
        if (color.Equals(Color.Primary))
            return ThemeManager.CurrentColorScheme.OnPrimary;
        if (color.Equals(Color.Secondary))
            return ThemeManager.CurrentColorScheme.OnSecondary;
        if (color.Equals(Color.Tertiary))
            return ThemeManager.CurrentColorScheme.OnTertiary;
        if (color.Equals(Color.Info))
            return ThemeManager.CurrentColorScheme.OnInfo;
        if (color.Equals(Color.Success))
            return ThemeManager.CurrentColorScheme.OnSuccess;
        if (color.Equals(Color.Warning))
            return ThemeManager.CurrentColorScheme.OnWarning;
        if (color.Equals(Color.Error))
            return ThemeManager.CurrentColorScheme.OnError;

        return ContrastingColor(color);
    }

    public static Color Custom(string colorValue)
    {
        return new Color(colorValue);
    }

    public string Value => $"#{R:x2}{G:x2}{B:x2}{A:x2}";
    public byte R { get; private set; } = 0;
    public byte G { get; private set; } = 0;
    public byte B { get; private set; } = 0;
    public byte A { get; private set; } = 0;
    public double AFraction => Math.Round(A / 255.0, 2);
    public double H { get; private set; } = 0;
    public double S { get; private set; } = 0;
    public double L { get; private set; } = 0;


    public Color(string value)
    {
        GetRGBAFromValue(value);
        GetHSLFromRGB();
    }

    public Color(double h, double s, double l)
    {
        GetRGBAFromHSL(CheckRange(h, 360), CheckRange(s, 1), CheckRange(l, 1));
        A = 255;
    }

    public Color(double h, double s, double l, double a)
    {
        GetRGBAFromHSL(CheckRange(h, 360), CheckRange(s, 1), CheckRange(l, 1));
        A = (byte)CheckRange((int)(a * 255.0), 0, 255);
    }

    public Color(double h, double s, double l, int a)
    {
        GetRGBAFromHSL(CheckRange(h, 360), CheckRange(s, 1), CheckRange(l, 1));
        A = (byte)CheckRange(a, 0, 255);
    }

    public Color(byte r, byte g, byte b, double a)
    {
        R = CheckRange(r, (byte)0, (byte)255);
        G = CheckRange(g, (byte)0, (byte)255);
        B = CheckRange(b, (byte)0, (byte)255);
        A = (byte)CheckRange((int)(a * 255.0), 0, 255);
        GetHSLFromRGB();
    }

    public Color(byte r, byte g, byte b, byte a)
    {
        R = CheckRange(r, (byte)0, (byte)255);
        G = CheckRange(g, (byte)0, (byte)255);
        B = CheckRange(b, (byte)0, (byte)255);
        A = (byte)CheckRange((int)a, 0, 255);
        GetHSLFromRGB();
    }

    public Color(byte r, byte g, byte b, int a)
    {
        R = CheckRange(r, (byte)0, (byte)255);
        G = CheckRange(g, (byte)0, (byte)255);
        B = CheckRange(b, (byte)0, (byte)255);
        A = (byte)CheckRange(a, 0, 255);
        GetHSLFromRGB();
    }

    public Color(int r, int g, int b, double a)
    {
        R = (byte)CheckRange(r, 0, 255);
        G = (byte)CheckRange(g, 0, 255);
        B = (byte)CheckRange(b, 0, 255);
        A = (byte)CheckRange((int)(a * 255.0), 0, 255);
        GetHSLFromRGB();
    }

    public Color(int r, int g, int b, Color existingColor)
    {
        R = (byte)CheckRange(r, 0, 255);
        G = (byte)CheckRange(g, 0, 255);
        B = (byte)CheckRange(b, 0, 255);
        A = (byte)CheckRange((int)existingColor.A, 0, 255);
        GetHSLFromRGB();
        H = existingColor.H;
    }

    public Color(int r, int g, int b, int a)
    {
        R = (byte)CheckRange(r, 0, 255);
        G = (byte)CheckRange(g, 0, 255);
        B = (byte)CheckRange(b, 0, 255);
        A = (byte)CheckRange(a, 0, 255);
        GetHSLFromRGB();
    }

    public Color SetH(double h) => new(h, S, L, A);
    public Color SetS(double s) => new(H, s, L, A);
    public Color SetL(double l) => new(H, S, l, A);

    public Color SetR(int r) => new(r, G, B, A);
    public Color SetG(int g) => new(R, g, B, A);
    public Color SetB(int b) => new(R, G, b, A);


    public Color SetAlpha(int a) => new(R, G, B, a);

    public Color SetAlpha(double a) => new(R, G, B, a);

    public Color ChangeLightness(double amount) => new(H, S, Math.Max(0, Math.Min(1, L + amount)), A);
    public Color Lighten(double amount) => ChangeLightness(+amount);
    public Color Darken(double amount) => ChangeLightness(-amount);
    public Color RgbLighten() => Lighten(0.075);
    public Color RgbDarken() => Darken(0.075);

    public static Color ContrastingColor(Color color)
    {
        if (color.A == 0)
            return Color.Dark;

        int nThreshold = 105;
        int bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) +
                                      (color.B * 0.114));

        return (255 - bgDelta < nThreshold) ? Color.Dark : Color.Light;
    }

    private void GetRGBAFromValue(string value)
    {

        if (value.StartsWith("rgba") == true)
        {
            var parts = SplitInputIntoParts(value);
            if (parts.Length != 4)
            {
                throw new ArgumentException("invalid color format");
            }

            R = byte.Parse(parts[0], CultureInfo.InvariantCulture);
            G = byte.Parse(parts[1], CultureInfo.InvariantCulture);
            B = byte.Parse(parts[2], CultureInfo.InvariantCulture);
            A = (byte)Math.Max(0, Math.Min(255, 255 * double.Parse(parts[3], CultureInfo.InvariantCulture)));
        }
        else if (value.StartsWith("rgb") == true)
        {
            var parts = SplitInputIntoParts(value);
            if (parts.Length != 3)
            {
                throw new ArgumentException("invalid color format");
            }

            R = byte.Parse(parts[0], CultureInfo.InvariantCulture);
            G = byte.Parse(parts[1], CultureInfo.InvariantCulture);
            B = byte.Parse(parts[2], CultureInfo.InvariantCulture);
            A = 255;
        }
        else if (value.StartsWith("hsl"))
        {
            var parts = SplitInputIntoParts(value);
            if (parts.Length != 3 || !parts[1].EndsWith("%") || !parts[2].EndsWith("%"))    
            {
                throw new ArgumentException("invalid color format");
            }

            H = double.Parse(parts[0], CultureInfo.InvariantCulture);
            S = double.Parse(parts[1].TrimEnd('%'), CultureInfo.InvariantCulture);
            L = double.Parse(parts[2].TrimEnd('%'), CultureInfo.InvariantCulture);
            A = 255;
        }
        else if (value.StartsWith("hsla"))
        {
            var parts = SplitInputIntoParts(value);
            if (parts.Length != 4 || !parts[1].EndsWith("%") || !parts[2].EndsWith("%"))
            {
                throw new ArgumentException("invalid color format");
            }

            H = double.Parse(parts[0], CultureInfo.InvariantCulture);
            S = double.Parse(parts[1].TrimEnd('%'), CultureInfo.InvariantCulture);
            L = double.Parse(parts[2].TrimEnd('%'), CultureInfo.InvariantCulture);
            A = (byte)Math.Max(0, Math.Min(255, 255 * double.Parse(parts[3], CultureInfo.InvariantCulture)));
        }
        else if (value.StartsWith("#") == true)
        {
            GetRGBFromHashValue(value);
        }
        else
            GetRGBFromHashValue(ColorNames.GetRgbForColorName(value));
    }

    private void GetHSLFromRGB()
    {
        var h = 0D;
        var s = 0D;
        double l;

        // normalize red, green, blue values
        var r = R / 255D;
        var g = G / 255D;
        var b = B / 255D;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        // hue
        if (Math.Abs(max - min) < Epsilon)
            h = 0D; // undefined
        else if ((Math.Abs(max - r) < Epsilon)
                && (g >= b))
            h = (60D * (g - b)) / (max - min);
        else if ((Math.Abs(max - r) < Epsilon)
                && (g < b))
            h = ((60D * (g - b)) / (max - min)) + 360D;
        else if (Math.Abs(max - g) < Epsilon)
            h = ((60D * (b - r)) / (max - min)) + 120D;
        else if (Math.Abs(max - b) < Epsilon)
            h = ((60D * (r - g)) / (max - min)) + 240D;

        // luminance
        l = (max + min) / 2D;

        // saturation
        if ((Math.Abs(l) < Epsilon)
                || (Math.Abs(max - min) < Epsilon))
            s = 0D;
        else if ((0D < l)
                && (l <= .5D))
            s = (max - min) / (max + min);
        else if (l > .5D)
            s = (max - min) / (2D - (max + min)); //(max-min > 0)?

        H = Math.Round(h.EnsureRange(360), 0);
        S = Math.Round(s.EnsureRange(1), 2);
        L = Math.Round(l.EnsureRange(1), 2);
    }

    private void GetRGBAFromHSL(double h, double s, double l)
    {
        // achromatic argb (gray scale)
        if (Math.Abs(s) < Epsilon)
        {
            R = (byte)Math.Max(0, Math.Min(255, (int)Math.Ceiling(l * 255D)));
            G = (byte)Math.Max(0, Math.Min(255, (int)Math.Ceiling(l * 255D)));
            B = (byte)Math.Max(0, Math.Min(255, (int)Math.Ceiling(l * 255D)));
        }
        else
        {
            var q = l < .5D
                    ? l * (1D + s)
                    : (l + s) - (l * s);
            var p = (2D * l) - q;

            var hk = h / 360D;
            var T = new double[3];
            T[0] = hk + (1D / 3D); // Tr
            T[1] = hk; // Tb
            T[2] = hk - (1D / 3D); // Tg

            for (var i = 0; i < 3; i++)
            {
                if (T[i] < 0D)
                    T[i] += 1D;
                if (T[i] > 1D)
                    T[i] -= 1D;

                if ((T[i] * 6D) < 1D)
                    T[i] = p + ((q - p) * 6D * T[i]);
                else if ((T[i] * 2D) < 1)
                    T[i] = q;
                else if ((T[i] * 3D) < 2)
                    T[i] = p + ((q - p) * ((2D / 3D) - T[i]) * 6D);
                else
                    T[i] = p;
            }

            R = ((int)Math.Round(T[0] * 255D)).EnsureRangeToByte();
            G = ((int)Math.Round(T[1] * 255D)).EnsureRangeToByte();
            B = ((int)Math.Round(T[2] * 255D)).EnsureRangeToByte();
        }

        H = Math.Round(h, 0);
        S = Math.Round(s, 2);
        L = Math.Round(l, 2);
    }

    private void GetRGBFromHashValue(string colorValue)
    {
        var value = colorValue[1..];

        switch (value.Length)
        {
            case 3:
                value = new string(new char[8] { value[0], value[0], value[1], value[1], 
                                                 value[2], value[2], 'F', 'F' });
                break;
            case 4:
                value = new string(new char[8] { value[0], value[0], value[1], value[1], 
                                                 value[2], value[2], value[3], value[3] });
                break;
            case 6:
                value += "FF";
                break;
            case 8:
                break;
            default:
                throw new ArgumentException("not a valid color", nameof(value));
        }
        R = GetByteFromValuePart(value, 0);
        G = GetByteFromValuePart(value, 2);
        B = GetByteFromValuePart(value, 4);
        A = GetByteFromValuePart(value, 6);
    }

    private string[] SplitInputIntoParts(string value)
    {
        var startIndex = value.IndexOf('(');
        var lastIndex = value.LastIndexOf(')');
        var subString = value[(startIndex + 1)..lastIndex];
        var parts = subString.Split(',', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < parts.Length; i++)
        {
            parts[i] = parts[i].Trim();
        }
        return parts;
    }

    private byte GetByteFromValuePart(string input, int index) => 
                     byte.Parse(new string(new char[] { input[index], input[index + 1] }), 
                                           NumberStyles.HexNumber);

    private static double CheckRange(double input, double max) => CheckRange(input, 0.0, max);
    private static byte CheckRange(byte input, byte min, byte max) => Math.Max(min, Math.Min(max, input));
    private static int CheckRange(int input, int min, int max) => Math.Max(min, Math.Min(max, input));
    private static double CheckRange(double input, double min, double max) => Math.Max(min, Math.Min(max, input));

    public bool Equals(Color? other)
    {
        if (other == null)
            return false;

        return
                H == other.H &&
                L == other.L &&
                S == other.S;
    }

}
