using System;

namespace Honeymustard
{
    public interface IBrowser
    {
        string Fetch(Uri uri);
    }
}