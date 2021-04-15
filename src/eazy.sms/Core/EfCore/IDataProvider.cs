using eazy.sms.Core.EfCore.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core.EfCore
{
    public interface IDataProvider
    {
        Task<IEnumerable<EventMessage>> FetchDataAsync(bool level);

        Task CreateDataAsync(EventMessage eventMessage);
        Task UpdateDataAsync(EventMessage eventMessage);
    }
}
