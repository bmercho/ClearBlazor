using ClearBlazor;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClearBlazorTest
{
    public class AutoFormTestModel
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
        [UseSwitch]
        public bool SwitchValue { get; set; } = true;

        [DisplayName("Radio group")]
        [Required(ErrorMessage = "A selection is required.")]
        public TestEnum EnumValue { get; set; }

        [DisplayName("Select enum")]
        [UseDropDown]
        public TestEnum SelectValue { get; set; } = TestEnum.Enum2;

        [DisplayName("Select-flags enum")]
        public DaysOfWeek Days { get; set; } = DaysOfWeek.Wednesday | DaysOfWeek.Friday;

        [DisplayName("Slider")]
        [UseSlider(0, 100, 10)]
        public double SliderValue { get; set; } = 50;

        [DisplayName("Color selection")]
        [ShowHexColor]
        public Color? ColorValue { get; set; } = Color.Secondary;

        [DisplayName("Date selection")]
        [DateFormat("dd MMM yy")]
        public DateOnly? DateValue { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [DisplayName("Time selection")]
        [TimeFormat("hh:mm tt")]
        [Hours24]
        [MinuteStep(MinuteStep.Five)]
        public TimeOnly? TimeValue { get; set; } = new TimeOnly(22, 15);
    }
}
