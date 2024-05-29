using Core.Responses;
using Masterdata.Application.Features.V1.Commands.Question;
using Masterdata.Application.Features.V1.Queries.Question;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Masterdata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQuestionQuery _query;

        public QuestionController(IMediator mediator, IQuestionQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestion(CreateQuestionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Tạo câu hỏi thành công"
            });
        }

        [HttpGet("list-question-by-topicId")]
        public async Task<IActionResult> GetListQuestionByTopicId([FromQuery] Guid TopicId)
        {
            var result = await _query.GetListQuestionByTopicId(TopicId);
            return Ok(new ApiSuccessResponse
            {
                Data = result,
                Message = "Get list câu hỏi thành công"
            });
        }
    }
}
