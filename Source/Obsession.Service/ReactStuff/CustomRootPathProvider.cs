using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace Obsession.Service.ReactStuff
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            var s = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\.."));
            return s;
        }
    }
}
