namespace ClearBlazor
{
    public struct Thickness : IEquatable<Thickness>
    {
        public static readonly Thickness Zero = new Thickness(0);

        public double Left { get; }

        public double Top { get; }

        public double Right { get; }

        public double Bottom { get; }

        public double HorizontalThickness => Left + Right;

        public double VerticalThickness => Top + Bottom;

        public Point TopLeft => new Point(Left, Top);

        public Point Size => new Point(Left + Right, Top + Bottom);

        internal bool IsDefault => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;

        public Thickness(double uniformSize) : this(uniformSize, uniformSize, uniformSize, uniformSize)
        {
        }

        public Thickness(double verticalSize, double horizontalSize)  : this(verticalSize, horizontalSize, verticalSize, horizontalSize)
        {
        }

        public Thickness(double top, double right, double bottom, double left) : this()
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public string ThicknessToCss()
        {
            if (Top == Right && Top == Bottom && Top == Left)
                return $"{Top}px";
            else if (Top == Bottom && Left == Right)
                return $"{Top}px {Left}px";
            else if (Left == Right)
                return $"{Top}px {Left}px {Bottom}px";
            else
                return $"{Top}px {Right}px {Bottom}px {Left}px";
        }

        public static Thickness Parse(string s)
        {
            var values = s.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 0)
                return new Thickness(0);

            if (values.Length > 4 || values.Length == 3)
                throw new FormatException($"'{s}' is not a valid format for '{nameof(Thickness)}'");

            double value1;
            double value2;
            double value3;
            double value4;
            if (values.Length == 1)
            {
                if (!double.TryParse(values[0], out value1))
                    throw new FormatException($"'{s}' is not a valid format for '{nameof(Thickness)}'");

                return new Thickness(value1);
            }

            if (values.Length == 2)
            {
                if (!double.TryParse(values[0], out value1) ||
                    !double.TryParse(values[1], out value2))
                    throw new FormatException($"'{s}' is not a valid format for '{nameof(Thickness)}'");

                return new Thickness(value1, value2);
            }

            if (!double.TryParse(values[0], out value1) ||
                !double.TryParse(values[1], out value2) ||
                !double.TryParse(values[2], out value3) ||
                !double.TryParse(values[3], out value4))
                throw new FormatException($"'{s}' is not a valid format for '{nameof(Thickness)}'");

            return new Thickness(value1, value2, value3, value4);
        }

        public static implicit operator Thickness(double uniformSize) =>
            new Thickness(uniformSize);

        public static implicit operator Thickness((double verticalSize, double horizontalSize) t) =>
            new Thickness(t.verticalSize, t.horizontalSize);

        public static implicit operator Thickness((double top, double right, double bottom, double left) t) =>
            new Thickness(t.top, t.right, t.bottom, t.left);

        public static bool operator ==(Thickness a, Thickness b) => a.Equals(b);

        public static bool operator !=(Thickness a, Thickness b) => !(a == b);

        public override string ToString() => $"[{Top}, {Right}, {Bottom}, {Left}]";

        public override bool Equals(object? obj) => obj is Thickness t && Equals(t);

        public bool Equals(Thickness other) =>
            Left == other.Left &&
            Top == other.Top &&
            Right == other.Right &&
            Bottom == other.Bottom;

        public override int GetHashCode()
        {
            var hashCode = -1819631549;
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            return hashCode;
        }
    }
}
