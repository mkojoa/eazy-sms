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
            var b = _context.SaveChanges() >= 0;
        }

        public async Task<EventMessage> CreateDataAsync(EventMessage eventMessage)
        {
            await DataContext.EventMessages.AddAsync(eventMessage);
            return eventMessage;
        }

        public async Task<IEnumerable<EventMessage>> FetchDataAsync(int level)
        {
            return DataContext.EventMessages.Where(rt => rt.Status == level).AsEnumerable();
        }

        public async Task UpdateDataAsync(EventMessage eventMessage)
        {
            DataContext.EventMessages.Update(eventMessage);
        }
    }
}