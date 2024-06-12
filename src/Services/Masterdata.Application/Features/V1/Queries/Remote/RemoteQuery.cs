using Core.Common;
using Core.Constant;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.DTOs.Remote;
using Masterdata.Application.Features.V1.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        Task<bool> UpdateStatusRemoteByRemoteName(string clientId);

        /// <summary>
        /// Lấy danh sách remote đang hoạt động
        /// </summary>
        /// <returns></returns>
        Task<List<RemoteResponse>> GetStatusRemoteByRemoteName();


        Task<List<RemoteResponse>> GetListConnectedRemote();
        Task<bool> DisconnectRemote();
    }
    public class RemoteQuery : IRemoteQuery 
    {
        private readonly IOT_SOCKETContext _context;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IHubContext<ChathubService> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RemoteQuery(IOT_SOCKETContext context, IUnitOfWork UnitOfWork, IHubContext<ChathubService> hubContext, IServiceScopeFactory serviceScopeFactory) {
            _context = context;
            _UnitOfWork = UnitOfWork;
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> DisconnectRemote()
        {
            object sendmessage = new
            {
                next = "true"
            };
            MQTTService mqttService = new MQTTService(_hubContext, _serviceScopeFactory);
            bool status = mqttService.PublishMessageAsync(ConstTopicConnect.NextQuestion, sendmessage);
            if(status)
            {
                var listRemote = await _context.RemoteModels.ToListAsync();
                foreach(var item in listRemote)
                {
                    item.Status = false;
                }
                _context.RemoteModels.UpdateRange(listRemote);
                await _UnitOfWork.SaveChangesAsync();
            }    
            return status;
        }

        public async Task<List<RemoteResponse>> GetListConnectedRemote()
        {
            var query = await _context.RemoteModels.Where(x=>x.Status==true).Select(x => new RemoteResponse
            {
                RemoteId = x.RemoteId,
                RemoteName = x.RemoteName,
                Status = x.Status,
            }).ToListAsync();
            return query;
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
