namespace Honeymustard
{
    public interface IUtilities
    {
        /// <summary>
        /// Reads an entire file into memory.
        /// Combines a directory path with a given filename.
        /// </summary>
        /// <param name="path">A fully qualified path to a directory</param>
        /// <param name="file">A file in said directory</param>
        string ReadFile(string path, string file);

        /// <summary>
        /// Reads an entire file into memory.
        /// </summary>
        /// <param name="path">Fully qualified path</param>
        string ReadFile(string path);
    }
}