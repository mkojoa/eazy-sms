using System.Collections.Generic;
using System.Threading.Tasks;
using eazy.sms.Core.EfCore.Entity;

namespace eazy.sms.Core.EfCore
{
    public interface IDataProvider
    {
        Task<IEnumerable<EventMessage>> FetchDataAsync(int level);

        Task CreateDataAsync(EventMessage eventMessage);
        Task UpdateDataAsync(EventMessage eventMessage);
        void Commit();
    }
}