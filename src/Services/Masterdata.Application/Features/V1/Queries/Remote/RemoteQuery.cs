using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Queries.Remote
{
    public interface IRemoteQuery
    {
        Task<bool> GetStatusRemoteByRemoteName(string remoteName);
    }
    public class RemoteQuery : IRemoteQuery
    {
        private readonly IOT_SOCKETContext _context;

        public RemoteQuery(IOT_SOCKETContext context) {
            _context = context;
        }
        public async Task<bool> GetStatusRemoteByRemoteName(string remoteName)
        {
            var remote = await _context.RemoteModels.FirstOrDefaultAsync(x => x.RemoteName == remoteName);
            return true;
        }
    }
}
