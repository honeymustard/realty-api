using System.IO;

namespace Honeymustard
{
    public static class Utils
    {
        public static string ReadFile(string path, string file)
        {
            return File.ReadAllText(Path.Combine(path, file));
        }

        public static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
}