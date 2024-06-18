using Core.Responses;
using Masterdata.Application.Features.V1.Commands.BeginGame;
using Masterdata.Application.Features.V1.Commands.Question;
using Masterdata.Application.Features.V1.Queries.BeginGame;
using Masterdata.Application.Features.V1.Queries.Question;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Masterdata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeginGameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBeginGameQuery _query;

        public BeginGameController(IMediator mediator, IBeginGameQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        /// <summary>
        /// Bắt đầu trò chơi
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ApiSuccessResponse> CreateBeginGame(CreateBeginGameCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        /// <summary>
        /// Xóa từng trò chơi đã chơi
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBeginGameById([FromQuery] DeleteBeginGameCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Xóa thành công"
            });
        }

        /// <summary>
        /// Lấy sanh sách chủ đề đã chơi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetListBeginGame(SearchBeginGameCommand request)
        {
            var result = await _query.GetListBeginGameAsync(request);
            return Ok(new ApiPagingSuccessResponse
            {
                Data = result.Data,
                Paging = new PagingResponse
                {
                    TotalCount = result.Paging.TotalCount,
                    TotalPage = result.Paging.TotalPages,
                    PageSize = result.Paging.PageSize
                }
            });
        }
    }
}
