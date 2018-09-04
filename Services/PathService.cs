using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Honeymustard
{
    public class PathService : IPathService
    {
        protected IHostingEnvironment Env;

        public PathService(IHostingEnvironment env)
        {
            Env = env;
        }

        public string GetDataPath()
        {
            return Path.Combine(Env.ContentRootPath, "Data");
        }
    }
}