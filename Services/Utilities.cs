using System.IO;

namespace Honeymustard
{
    public class Utilitis : IUtilities
    {
        public string ReadFile(string path, string file)
        {
            return File.ReadAllText(Path.Combine(path, file));
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
}