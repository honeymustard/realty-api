namespace Honeymustard
{
    public interface IEnvironment
    {
        /// <summary>
        /// Gets the absolute path to the local data folder.
        /// </summary>
        string GetDataPath();
    }
}