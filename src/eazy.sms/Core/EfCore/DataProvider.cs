using eazy.sms.Core.EfCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core.EfCore
{
    public class DataProvider : IDataProvider
    {
        private readonly DataContext _context;

        public DataProvider(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task CreateDataAsync(EventMessage eventMessage)
            => await _context.EventMessages.AddAsync(eventMessage);

        public async Task<IEnumerable<EventMessage>> FetchDataAsync(bool level)
            => _context.EventMessages.Where(rt => rt.Status == level).AsEnumerable();

        public async Task UpdateDataAsync(EventMessage eventMessage)
            => _context.EventMessages.Update(eventMessage);
    }
}
