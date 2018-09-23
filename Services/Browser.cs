using System;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace Honeymustard
{
    public class Browser : IBrowser
    {
        public static string UserAgent {
            get => string.Join(" ", new String[] {
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
                "AppleWebKit/537.36 (KHTML, like Gecko)",
                "Chrome/68.0.3440.106 Safari/537.36"
            });
        }

        public string Fetch(Uri uri)
        {
            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                try {
                    client.Headers.Add("User-Agent", UserAgent);
                    client.Headers.Add("Accept", "*/*");
                    client.Headers.Add("Accept-Language", "nb-no");
                    client.Headers.Add("Accept-Charset", "UTF-8");

                    return client.DownloadString(uri);
                }
                catch (WebException e)
                {
                    Console.WriteLine(e);
                }
            }

            return "";
        }
    }
}