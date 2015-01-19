using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public interface IBahavior
    {
        void Behave();
    }

    public class Behavior
    {
        private readonly IEnumerable<IParamValueProvider> _valueProviders;

        public Behavior(IEnumerable<IParamValueProvider> valueProviders)
        {
            _valueProviders = valueProviders;
        }

        /// <summary>
        /// Please behave!
        /// </summary>
        public void Behave()
        {
        }
    }

    public interface INotifier
    {
        void Notify(Notification message);
    }

    public interface IAction
    {
        
    }

    public class Notification
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// An abstraction for a file, so that we can download and upload
    /// </summary>
    public interface IFile
    {
        string Name { get; }

        Stream Read();
        void Write(Stream stream);
    }
}
