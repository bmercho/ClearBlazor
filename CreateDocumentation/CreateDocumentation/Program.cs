namespace CreateDocumentation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var srcPath = Paths.SrcPath;
            if (srcPath == null)
            {
                Console.WriteLine($"Main: srcPath is null");
                return ;
            }

            //new ExamplesMarkup().Execute(srcPath);

            Console.WriteLine("Creating Examples markup completed.");

            new ApiDoco().Execute(srcPath);

            Console.WriteLine("Creating API Documentation completed.");
        }
    }
}