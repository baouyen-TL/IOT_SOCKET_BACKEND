using Core.Common;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.DTOs.Remote;
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
        /// <summary>
        ///  Cập nhật trạng thái Remote
        /// </summary>
        /// <param name="remoteName"></param>
        /// <returns></returns>
        Task<bool> UpdateStatusRemoteByRemoteName(string remoteName);

        /// <summary>
        /// Lấy danh sách remote đang hoạt động
        /// </summary>
        /// <returns></returns>
        Task<List<RemoteResponse>> GetStatusRemoteByRemoteName();
    }
    public class RemoteQuery : IRemoteQuery
    {
        private readonly IOT_SOCKETContext _context;
        private readonly IUnitOfWork _UnitOfWork;

        public RemoteQuery(IOT_SOCKETContext context, IUnitOfWork UnitOfWork) {
            _context = context;
            _UnitOfWork = UnitOfWork;
        }

        public async Task<List<RemoteResponse>> GetStatusRemoteByRemoteName()
        {
            var query = await _context.RemoteModels.Select(x => new RemoteResponse
            {
                RemoteId = x.RemoteId,
                RemoteName = x.RemoteName,
                Status = x.Status,
            }).ToListAsync();
            return query;
        }

        public async Task<bool> UpdateStatusRemoteByRemoteName(string clientId)
        {
            var remote = await _context.RemoteModels.FirstOrDefaultAsync(x => x.RemoteId == Guid.Parse(clientId));
            if(remote != null)
            {
                remote.Status = true;
            }

            await _UnitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
