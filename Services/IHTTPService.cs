using System;

namespace Honeymustard
{
    public interface IHTTPService
    {
        string Fetch(Uri uri);
    }
}