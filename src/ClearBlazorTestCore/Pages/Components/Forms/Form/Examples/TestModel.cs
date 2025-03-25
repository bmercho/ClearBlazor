using ClearBlazor;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClearBlazorTest
{
    public enum TestEnum
    {
        Enum1, Enum2, Enum3, Enum4
    }

    [Flags]
    public enum DaysOfWeek
    {
        Monday = 1, Tuesday = 2, Wednesday = 4, Thursday = 8, Friday = 16, Saturday = 32, Sunday = 64
    }

    public class TestModel
    {
        [DisplayName("Name(max 10)")]   
        [Required(ErrorMessage = "A value is required.")]
        [MaxLength(10, ErrorMessage = "The maximum length is 10.")]
        public string TextValue { get; set; } = string.Empty;

        [DisplayName("Int value")]
        public int IntValue { get; set; }

        [DisplayName("I agree")]
        public bool BoolValue { get; set; } = true;

        [DisplayName("I agree")]
        public bool SwitchValue { get; set; } = true;

        [DisplayName("Radio group")]
        [Required(ErrorMessage = "A selection is required.")]
        public string? RadioValue { get; set; } = null;

        [DisplayName("Select enum")]
        public TestEnum EnumValue { get; set; } = TestEnum.Enum2;

        [DisplayName("MultiSelect-flags enum")]
        public DaysOfWeek Days { get; set; } = DaysOfWeek.Wednesday | DaysOfWeek.Friday;

        [DisplayName("Slider")]
        public double SliderValue { get; set; } = 50;

        [DisplayName("Color selection")]
        public Color? ColorValue { get; set; } = Color.Secondary;

        [DisplayName("Date selection")]
        public DateOnly? DateValue { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [DisplayName("Time selection")]
        public TimeOnly? TimeValue { get; set; } = new TimeOnly(22, 15);
    }
}
