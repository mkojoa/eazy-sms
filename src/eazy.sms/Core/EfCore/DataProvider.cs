using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Core.EfCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace eazy.sms.Core.EfCore
{
    public class DataProvider : IDataProvider
    {
        private readonly DataContext _context;

        public DataProvider(DataContext dataContext)
        {
            _context = dataContext;

            try
            {
                if (!_context.Database.CanConnect()) _context.Database.EnsureCreated();
            }
            catch (System.Exception ex)
            {

                //throw;
            }
        }

        public void Commit()
        {
            var b = _context.SaveChanges() >= 0;
        }

        public async Task<EventMessage> CreateDataAsync(EventMessage eventMessage)
        {
            await _context.AddAsync(eventMessage);
            return eventMessage;
        }

        public async Task<IEnumerable<EventMessage>> FetchDataAsync(int level)
        {
            if (level == 2) // get all
            {
                return _context.EventMessages.AsEnumerable();
            }
            return _context.EventMessages.Where(rt => rt.SentStatus == level).AsEnumerable();
        }

        public async Task UpdateDataAsync(EventMessage eventMessage)
        {
            
            try
            {
                _context.Entry(eventMessage).State = EntityState.Modified;
                _context.Entry(eventMessage).Property(x => x.CreatedAt).IsModified = false;
               
                //_context.Update(eventMessage);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}