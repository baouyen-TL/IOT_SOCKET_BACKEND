using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Masterdata.Application.Features.V1.Commands;
using Core.ApiResponse;

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
            });
        }



    }
}
