using Data;

namespace ClearBlazorTest
{
    public static class ClientData
    {
        public static List<TestListRow> LocalTestListRows100 { get; set; } = new();
        public static List<TestListRow> LocalTestListRows5000 { get; set; } = new();

        public static List<TestTreeRow> LocalTestTreeRows100 { get; set; } = new();
        public static List<TestTreeRow> LocalTestTreeRows5000 { get; set; } = new();

        public static List<TestTreeRowFlat> LocalTestTreeRowsFlat100 { get; set; } = new();
        public static List<TestTreeRowFlat> LocalTestTreeRowsFlat5000 { get; set; } = new();

        public static void LoadTestData()
        {
            try
            {
                LocalTestListRows5000 = Data.TestData.GetTestListRows(5000);
                LocalTestListRows100 = Data.TestData.GetTestListRows(100);

                LocalTestTreeRows5000 = Data.TestData.GetTestTreeRows(5000);
                LocalTestTreeRows100 = Data.TestData.GetTestTreeRows(100);

                LocalTestTreeRowsFlat5000 = Data.TestData.GetTestTreeRowsFlat(LocalTestTreeRows5000);
                LocalTestTreeRowsFlat100 = Data.TestData.GetTestTreeRowsFlat(LocalTestTreeRows100);
            }
            catch (Exception)
            {

            }
        }
    }
}
