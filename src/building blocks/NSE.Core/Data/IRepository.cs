using NSE.Core.ModelObjects;

namespace NSE.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggragateRoot
    {
    }
}
