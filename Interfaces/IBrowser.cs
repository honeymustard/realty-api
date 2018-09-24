using System;

namespace Honeymustard
{
    public interface IBrowser
    {
        /// <summary>
        /// Download the content from a given URI.
        /// </summary>
        /// <param name="uri">A complete URI</param>
        /// <returns>Returns the document body as a string.</returns>
        string Fetch(Uri uri);
    }
}