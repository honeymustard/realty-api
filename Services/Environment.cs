using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Honeymustard
{
    public class Environment : IEnvironment
    {
        protected IHostingEnvironment Env;

        public Environment(IHostingEnvironment env)
        {
            Env = env;
        }

        public string GetDataPath()
        {
            return Path.Combine(Env.ContentRootPath, "Data");
        }
    }
}