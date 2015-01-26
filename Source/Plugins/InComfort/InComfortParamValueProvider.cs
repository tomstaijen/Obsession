using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obsession.Core;
using Obsession.Core.Params;

namespace InComfort
{
    public class InComfortParamValueProvider : IParamValueProvider
    {
        private readonly IStateProvider<ReadableInComfortData> _stateProvider;

        public InComfortParamValueProvider(IStateProvider<ReadableInComfortData> stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public IEnumerable<Param> GetParams()
        {
            var state = _stateProvider.GetCurrent();

            return new Param[]
                {
                    new NumericParam()
                        {
                            Name = "Home.Thermostat.Temp",
                            Value = Convert.ToDouble(state.RoomSetpoint1)
                        }, 
                    new NumericParam()
                        {
                            Name = "Home.Livingroom.Temp",
                            Value = Convert.ToDouble(state.RoomTemp1)
                        }
                };

            throw new NotImplementedException();
        }
    }
}
