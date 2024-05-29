using Core.Responses;
using Masterdata.Application.Features.V1.Commands.BeginGame;
using Masterdata.Application.Features.V1.Commands.Question;
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
        private readonly IQuestionQuery _query;

        public BeginGameController(IMediator mediator, IQuestionQuery query)
        {
            _mediator = mediator;
            _query = query;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBeginGame(CreateBeginGameCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Tạo thành công"
            });
        }
    }
}
