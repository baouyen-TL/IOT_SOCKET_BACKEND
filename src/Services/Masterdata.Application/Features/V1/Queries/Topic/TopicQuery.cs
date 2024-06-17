using Core.SeedWork;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.Commands.Topic;
using Masterdata.Application.Features.V1.DTOs.Topic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Masterdata.Application.Features.V1.Queries.Topic
{
    public interface ITopicQuery
    {
        Task<PagingResultSP<TopicResponse>> GetListTopics(SearchTopicCommand request);
    }
    public class TopicQuery : ITopicQuery
    {
        private readonly IOT_SOCKETContext _context;

        public TopicQuery(IOT_SOCKETContext context) {
            _context = context;
        }
        public async Task<PagingResultSP<TopicResponse>> GetListTopics(SearchTopicCommand request)
        {
            var query = _context.TopicModels.Where(x => x.Actived == true)
                .Select(x => new TopicResponse
                {
                    TopicId = x.TopicId,
                    TopicName = x.TopicName,
                    CountQuestion = _context.QuestionModels.Where(y => y.TopicId == x.TopicId).Count(),
                    CreateTime = x.CreateTime
                });

            var totalRecords = await query.CountAsync();

            //Sorting
            query = PagingSorting.Sorting(request.Paging, query);

            //Phân trang
            var responsePaginated = await PaginatedList<TopicResponse>.CreateAsync(query, request.Paging.Offset, request.Paging.PageSize);
            var response = new PagingResultSP<TopicResponse>(responsePaginated, totalRecords, request.Paging.PageIndex, request.Paging.PageSize);

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
