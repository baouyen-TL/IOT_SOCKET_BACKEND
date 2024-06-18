using Core.Responses;
using Masterdata.Application.Features.V1.Commands.BeginGame;
using Masterdata.Application.Features.V1.Commands.Topic;
using Masterdata.Application.Features.V1.Queries.Topic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Masterdata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITopicQuery _topicQuery;

        public TopicController(IMediator mediator, ITopicQuery topicQuery) {
            _mediator = mediator;
            _topicQuery = topicQuery;
        }

        /// <summary>
        /// Tạo chủ đề
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateTopic(CreateTopicCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Tạo chủ đề thành công"
            });
        }

        /// <summary>
        /// List seach chủ đề
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetListTopic(SearchTopicCommand request)
        {
            var result = await _topicQuery.GetListTopics(request);
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

        /// <summary>
        /// Xóa bắt đầu trò chơi theo chủ đề
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBeginGameByTopicId([FromQuery] DeleteTopicCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Xóa thành công"
            });
        }

    }
}
