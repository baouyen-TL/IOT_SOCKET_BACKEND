using Core.Responses;
using Masterdata.Application.Features.V1.Queries.Remote;
using Masterdata.Application.Features.V1.Queries.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Masterdata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportQuery _query;

        public ReportController(IReportQuery query)
        {
            _query = query;
        }

        /// <summary>
        /// Đếm số lượng đáp án được chọn theo từng câu hỏi
        /// </summary>
        /// <param name="BeginGameId"></param>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        [HttpGet("report-count-answer")]
        public async Task<IActionResult> GetReportCountAnswer([FromQuery] Guid BeginGameId, Guid QuestionId)
        {
            var result = await _query.GetReportCountAnswerBy(BeginGameId, QuestionId);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Lấy sanh sách báo cáo thành công"
            });
        }

        [HttpGet("report-top-ranking")]
        public async Task<IActionResult> GetReportTopRanking([FromQuery] Guid BeginGameId)
        {
            var result = await _query.GetReportTopRankingBy(BeginGameId);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Lấy sanh sách báo cáo thành công"
            });
        }
    }
}
