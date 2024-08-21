using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public interface IBaseProperties
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double MinWidth { get; set; }

        public double MinHeight { get; set; }

        public double MaxWidth { get; set; }

        public double MaxHeight { get; set; }

        public string Margin { get; set; }

        public string Padding { get; set; }

        public Alignment? HorizontalAlignment { get; set; }

        public Alignment? VerticalAlignment { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
    }
}
