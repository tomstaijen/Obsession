namespace Obsession.Core.Persistence
{

    public interface IPersister
    {
        void Put<T>(T o) where T : class;
    }
}