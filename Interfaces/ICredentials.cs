namespace Honeymustard
{
    public interface ICredentials
    {
        /// <summary>
        /// The name of the database.
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// The connection string for the database.
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// The salt for user authentication.
        /// </summary>
        string UserSalt { get; set; }
    }
}