using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Common
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task Dispose();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IOT_SOCKETContext _context;
        private bool disposed = false;

        public UnitOfWork(IOT_SOCKETContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        private async Task Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
            }
            disposed = true;
        }

        public async Task Dispose()
        {
            await Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
