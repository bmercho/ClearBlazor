namespace CreateDocumentation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool success = false;
            var srcPath = Paths.SrcPath;
            if (srcPath == null)
            {
                Console.WriteLine($"Main: srcPath is null");
                return ;
            }


            //var success = new ExamplesMarkup().Execute(srcPath);

            Console.WriteLine("Creating Examples markup completed.");

            success = new ApiDoco().Execute(srcPath);

            if (success) 
                Console.WriteLine("Creating API Documentation completed.");
            else
                Console.WriteLine("Creating API Documentation had errors.");
        }
    }
}