using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using React;

namespace Obsession.Service.ReactStuff
{
    public class NullReactCache : ICache
    {
//        public T GetOrInsert<T>(string key, TimeSpan slidingExpiration, Func<T> getData, IEnumerable<string> cacheDependencyFiles = null,
//            IEnumerable<string> cacheDependencyKeys = null)
//        {
//            return getData();
//        }
        
        public T Get<T>(string key, T fallback = default(T))
        {
            return fallback;
        }

        public void Set<T>(string key, T data, TimeSpan slidingExpiration, IEnumerable<string> cacheDependencyFiles = null,
            IEnumerable<string> cacheDependencyKeys = null)
        {
            Console.WriteLine("{0} => {1}", key, data);
        }
    }
}
