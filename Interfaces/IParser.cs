namespace Honeymustard
{
    public interface IParser<E>
    {
        /// <summary>
        /// Attempts to parse a model from a given text.
        /// </summary>
        /// <param name="chunk">A chunk of text</param>
        E Parse(string chunk);
    }
}