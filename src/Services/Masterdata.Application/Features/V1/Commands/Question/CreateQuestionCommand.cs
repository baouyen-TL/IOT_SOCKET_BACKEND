using Core.Common;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.Commands.Topic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Commands.Question
{
    public class CreateQuestionCommand : IRequest<bool>
    {
        public Guid? TopicId { get; set; }
        public string QuestionName { get; set; }
        public int? QuestionTime { get; set; }
    }
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOT_SOCKETContext _context;

        public CreateQuestionCommandHandler(IOT_SOCKETContext context, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<bool> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var questionEntity = new QuestionModel
            {
                QuestionId = Guid.NewGuid(),
                TopicId = request.TopicId,
                QuestionName = request.QuestionName,
                QuestionTime = request.QuestionTime,
                CreateTime = DateTime.Now
            };

            _context.QuestionModels.Add(questionEntity);

            await _unitOfWork.SaveChangesAsync();   

            return true;
        }
    }
}
