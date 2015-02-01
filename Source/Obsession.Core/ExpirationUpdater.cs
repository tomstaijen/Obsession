using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;


namespace Obsession.Core
{
    public class Expiration<T>
    {
        public TimeSpan Value { get; set; }
    }

    public class ContinuousUpdater<T> : IStateProvider<T> where T : class, IDisposable
    {
        private readonly Func<T> _updateFunc;
        private readonly Expiration<T> _expiration;
        private T _current;

        private Timer _timer;

        public ContinuousUpdater(Func<T> updateFunc, Expiration<T> expiration)
        {
            _updateFunc = updateFunc;
            _expiration = expiration;
            _timer = new Timer();
            _timer.Elapsed += Elapsed;
        }

        protected void Elapsed(object source, ElapsedEventArgs args)
        {
            _current = _updateFunc();
        }

        public T GetCurrent()
        {
            return _current;
        }
    }

    public class ExpirationUpdater<T> : IStateProvider<T> where T : class
    {
        private State<T> LastState { get; set; } 

        private readonly Func<T> _updateFunc;
        private readonly TimeSpan _expiration;

        public ExpirationUpdater(Func<T> updateFunc, Expiration<T> expiration)
        {
            _updateFunc = updateFunc;
            _expiration = expiration.Value;
        }

        public T GetCurrent()
        {
            if (LastState != null && DateTime.Now < LastState.Expires)
            {
                return LastState.Value;
            }

            var value = _updateFunc();
            if (value != null)
            {
                var state = new State<T>
                    {
                        Value = value,
                        Expires = DateTime.Now.Add(_expiration)
                    };

                PushState(state);

                return state.Value;
            }
            return null;
        }

        public void PushState(State<T> value)
        {
            // detect events, can be done synchronisly beause the events will be handled sequentially using a queue

            // set the value
            LastState = value;
        }
    }

    public class State<T>
    {
        public T Value { get; set; }
        public DateTime Expires { get; set; }
    }


    public class StateUpdate<T>
    {
        public State<T> Old { get; set; }
        public State<T> New { get; set; } 
    }
}
