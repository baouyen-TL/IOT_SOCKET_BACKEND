using Core.Common;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.Commands.Question;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Commands.SaveAnswer
{
    public class CreateSaveAnswerCommand : IRequest<bool>
    {
        public Guid? AnswerId { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? RemoteId { get; set; }
        public int? CountTime { get; set; }
    }

    public class CreateSaveAnswerCommandHandler : IRequestHandler<CreateSaveAnswerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOT_SOCKETContext _context;

        public CreateSaveAnswerCommandHandler(IOT_SOCKETContext context, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<bool> Handle(CreateSaveAnswerCommand request, CancellationToken cancellationToken)
        {
            // Add câu trả lời
            //var saveAnswerEntity = new SaveAnswerModel
            //{
            //    SaveAnswerId = Guid.NewGuid(),
                
            //};
            return true;
        }
    }
}
