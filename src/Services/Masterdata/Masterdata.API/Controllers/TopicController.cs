using Core.Responses;
using Masterdata.Application.Features.V1.Commands.Topic;
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

        public TopicController(IMediator mediator) {
            _mediator = mediator;
        }
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



    }
}
