namespace CreateExamplesMarkup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var success = new ExamplesMarkup().Execute();

            Console.WriteLine("Creating Examples markup completed.");
        }
    }
}