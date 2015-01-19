using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obsession.Core;
using Obsession.Core.Params;

namespace YahooWeather
{
    public class WeatherParamValueProvider : IParamValueProvider
    {
        private readonly IStateProvider<Forecast> _stateProvider;

        public WeatherParamValueProvider(IStateProvider<Forecast> stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public IEnumerable<Param> GetParams()
        {
            var state = _stateProvider.GetCurrent();
            var param = new NumericParam
                {
                    Name = "Home.Temperature",
                    Value = state.Item.Condition.TempCelcius
                };

            return new[] { param };
        }
    }
}
