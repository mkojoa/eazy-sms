using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Core.EfCore.Entity;

namespace eazy.sms.Core.EfCore
{
    public class DataProvider : IDataProvider
    {
        private readonly DataContext _context;

        public DataProvider(DataContext dataContext)
        {
            _context = dataContext;

            if (!_context.Database.CanConnect()) _context.Database.EnsureCreated();
        }

        public void Commit()
        {
            var testVar = _context.SaveChanges() >= 0;
            //return (_context.SaveChanges() >= 0);
        }

        public async Task CreateDataAsync(EventMessage eventMessage)
        {
            await _context.EventMessages.AddAsync(eventMessage);
        }

        public async Task<IEnumerable<EventMessage>> FetchDataAsync(int level)
        {
            return _context.EventMessages.Where(rt => rt.Status == level).AsEnumerable();
        }

        public async Task UpdateDataAsync(EventMessage eventMessage)
        {
            _context.EventMessages.Update(eventMessage);
        }
    }
}