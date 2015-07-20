
using System;
using Akka.Event;

namespace Obsession.Akka
{
    public class OurLoggingAdapter : LoggingAdapterBase
    {
        public OurLoggingAdapter(ILogMessageFormatter logMessageFormatter) : base(logMessageFormatter)
        {
        }

        protected override void NotifyError(object message)
        {
            throw new NotImplementedException();
        }

        protected override void NotifyError(Exception cause, object message)
        {
            throw new NotImplementedException();
        }

        protected override void NotifyWarning(object message)
        {
            throw new NotImplementedException();
        }

        protected override void NotifyInfo(object message)
        {
            throw new NotImplementedException();
        }

        protected override void NotifyDebug(object message)
        {
            throw new NotImplementedException();
        }

        public override bool IsDebugEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsInfoEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsWarningEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsErrorEnabled
        {
            get { throw new NotImplementedException(); }
        }
    }
}
