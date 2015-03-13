using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Obsession.Core
{
    public class ModuleStart{}

    public class ModuleCoordinator : ReceiveActor
    {
        public ModuleCoordinator()
        {
            Receive<ModuleStart>(s => { });
        }

        public void Start()
        {
        }
    }
}
