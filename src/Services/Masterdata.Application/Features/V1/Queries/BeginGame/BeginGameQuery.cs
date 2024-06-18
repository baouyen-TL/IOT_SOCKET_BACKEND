using Core.SeedWork;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.Commands.BeginGame;
using Masterdata.Application.Features.V1.DTOs.BeginGame;
using Masterdata.Application.Features.V1.DTOs.Topic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Queries.BeginGame
{
    public interface IBeginGameQuery
    {
        /// <summary>
        /// Lấy ra danh sách các lớp đã chơi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<BeginGameResponse>> GetListBeginGameAsync(SearchBeginGameCommand request);
    }
    public class BeginGameQuery : IBeginGameQuery
    {
        private readonly IOT_SOCKETContext _context;

        public BeginGameQuery(IOT_SOCKETContext context) {
            _context = context;
        }
        public async Task<PagingResultSP<BeginGameResponse>> GetListBeginGameAsync(SearchBeginGameCommand request)
        {
            var query = _context.BeginGameModels
                .Include(x => x.Topic)
                .Select(x => new BeginGameResponse
                {
                    BeginGameId = x.BeginGameId,
                    TopicName = x.Topic.TopicName,
                    CreateTime = x.CreateTime
                });

            var totalRecords = await query.CountAsync();

            //Sorting
            query = PagingSorting.Sorting(request.Paging, query);

            //Phân trang
            var responsePaginated = await PaginatedList<BeginGameResponse>.CreateAsync(query, request.Paging.Offset, request.Paging.PageSize);
            var response = new PagingResultSP<BeginGameResponse>(responsePaginated, totalRecords, request.Paging.PageIndex, request.Paging.PageSize);

            if (response.Data.Any())
            {
                int i = request.Paging.Offset;

                foreach (var item in response.Data)
                {
                    i++;
                    item.STT = i;
                }
            }

            return response;
        }
    }
}
