namespace Honeymustard
{
    public interface IParser<E>
    {
        /// <summary>
        /// Attempts to parse a model from a given text.
        /// </summary>
        /// <param name="blob">A blob of text</param>
        E Parse(string blob);
    }
}