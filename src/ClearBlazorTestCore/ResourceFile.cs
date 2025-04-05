using System.Reflection;

namespace ClearBlazor
{
    public static class ResourceFile
    {
        public static string ReadResourceFile(string fileName)
        {
            string result = string.Empty;
            Stream? stream = null;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

                stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            finally
            {
                stream?.Close();
            }
            return result;
        }
    }
}
