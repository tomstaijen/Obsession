using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace Obsession.Core
{
    public interface IModuleFactory
    {
        IServiceModule Create(Configuration configuration);
    }

    public class ModuleFactory : IModuleFactory
    {
        private readonly IIndex<string, Func<Configuration, IServiceModule>> _moduleRegister;

        public ModuleFactory(IIndex<string, Func<Configuration, IServiceModule>> moduleRegister)
        {
            _moduleRegister = moduleRegister;
        }


        public IServiceModule Create(Configuration configuration)
        {
            return _moduleRegister[configuration.ModuleName](configuration);
        }
    }

}
