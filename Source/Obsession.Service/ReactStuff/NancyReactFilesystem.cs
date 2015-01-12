using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using React;

namespace Obsession.Service.ReactStuff
{
    public class NancyReactFileSystem : IFileSystem
    {
        private readonly IRootPathProvider _rootPathProvider;

        public NancyReactFileSystem(IRootPathProvider rootPathProvider)
        {
            _rootPathProvider = rootPathProvider;
        }

        public string MapPath(string relativePath)
        {
            if (relativePath.StartsWith("~/"))
            {
                return Path.Combine(_rootPathProvider.GetRootPath(), relativePath.Substring(2));
            }

            return relativePath;
        }

        public string ReadAsString(string relativePath)
        {
            return File.ReadAllText(MapPath(relativePath));
        }

        public void WriteAsString(string relativePath, string contents)
        {
            // TODO: async
            File.WriteAllText(MapPath(relativePath), contents);
        }

        public bool FileExists(string relativePath)
        {
            return File.Exists(MapPath(relativePath));
        }
    }
}
