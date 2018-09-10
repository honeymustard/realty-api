namespace Honeymustard
{
    public interface ICredentials
    {
        string Database { get; set; }
        string Connection { get; set; }
    }
}