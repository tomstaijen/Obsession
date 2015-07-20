using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core.Model
{
    public interface IResource
    {
        string Url { get; set; }
    }

    public interface IVideo : IResource
    {
    }

    public interface IImage : IResource
    {
    }
}
