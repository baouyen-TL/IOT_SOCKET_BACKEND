using Core.Exceptions;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.DTOs.Report;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Queries.Report
{
    public interface IReportQuery
    {
        /// <summary>
        /// Thống kê số lượng đáp án được chọn theo từng câu hỏi
        /// </summary>
        /// <param name="BeginGameId"></param>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        Task<List<ReportCountAnswerResponse>> GetReportCountAnswerBy(Guid BeginGameId, Guid QuestionId);

        /// <summary>
        /// Bảng xếp hạng top 3 remote
        /// </summary>
        /// <param name="BeginGameId"></param>
        /// <returns></returns>
        Task<List<ReportTopRankingRemoteResponse>> GetReportTopRankingBy(Guid BeginGameId);

        /// <summary>
        /// Bảng xếp hạng Remote
        /// </summary>
        /// <param name="BeginGameId"></param>
        /// <returns></returns>
        Task<List<ReportRankingDetailResponse>> GetReportRankingDetailBy(Guid BeginGameId);
    }
    public class ReportQuery : IReportQuery
    {
        private readonly IOT_SOCKETContext _context;

        public ReportQuery(IOT_SOCKETContext context) {
            _context = context;
        }
        public async Task<List<ReportCountAnswerResponse>> GetReportCountAnswerBy(Guid BeginGameId, Guid QuestionId)
        {
            var query = await _context.SaveAnswerModels.Where(x => x.BeginGameId == BeginGameId && x.QuestionId == QuestionId)
                .GroupBy(x => x.AnswerId)
                .Select(x => new 
                {
                    AnswerId = x.Key,
                    TotalUserSelected = x.Count(x => x.RemoteId.HasValue)
                })
                .ToListAsync();
            var ListRes = new List<ReportCountAnswerResponse>();
            foreach (var item in query)
            {
                var anwser = await _context.AnswerModels.FirstOrDefaultAsync(x => x.AnswerId == item.AnswerId);
                var res = new ReportCountAnswerResponse
                {
                    AnswerId = anwser.AnswerId,
                    AnswerKey = anwser.AnswerKey,
                    TotalUserSelected = item.TotalUserSelected,
                };
                ListRes.Add(res);
            }

            return ListRes;
        }

        public Task<List<ReportRankingDetailResponse>> GetReportRankingDetailBy(Guid BeginGameId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReportTopRankingRemoteResponse>> GetReportTopRankingBy(Guid BeginGameId)
        {
            var res = new List<ReportTopRankingRemoteResponse>();
            var begingames = await _context.SaveAnswerModels.Where(x => x.BeginGameId == BeginGameId)
                .ToListAsync();
            if (!begingames.Any()) throw new BadRequestException("BeginGameId này không tồn tại!");

            // List đáp án đúng
            var ListAnswerCorrectRes = new List<ReportTopRankingRemoteTempResponse>();

            foreach (var item in begingames)
            {
                var listAnswers = await _context.AnswerModels.FirstOrDefaultAsync(x => x.AnswerId == item.AnswerId && x.IsCorrect == true);
                if (listAnswers == null) continue;

                var tempRes = new ReportTopRankingRemoteTempResponse
                {
                    RemoteId = item.RemoteId,
                    AnwserId = item.AnswerId,
                };
                ListAnswerCorrectRes.Add(tempRes);
            }

            var bxhs = ListAnswerCorrectRes.GroupBy(x => x.RemoteId).Select(x => new
            {
                RemoteId = x.Key,
                Total = x.Count(),
            }).OrderByDescending(x => x.Total);

            var query = bxhs.Take(3).Select((x,i) => new ReportTopRankingRemoteResponse
            {
                Position = i + 1,
                UserName = _context.UserGameModels.FirstOrDefault(y => y.RemoteId == x.RemoteId && y.BeginGameId == BeginGameId)?.UserName,
                RemoteName = _context.RemoteModels.FirstOrDefault(y => y.RemoteId == x.RemoteId)?.RemoteName
            }).ToList();
            return query;
        }  
    }
}
