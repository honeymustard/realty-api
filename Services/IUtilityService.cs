namespace Honeymustard
{
    public interface IUtilityService
    {
        string ReadFile(string path, string file);
        string ReadFile(string path);
    }
}