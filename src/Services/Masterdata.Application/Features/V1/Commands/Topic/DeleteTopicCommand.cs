using Core.Common;
using Core.Exceptions;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Commands.Topic
{
    public class DeleteTopicCommand : IRequest<bool>
    {
        public Guid? TopicId { get; set; }
    }

    public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, bool>
    {
        private readonly IOT_SOCKETContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTopicCommandHandler(IOT_SOCKETContext context, IUnitOfWork UnitOfWork)
        {
            _context = context;
            _unitOfWork = UnitOfWork;
        }
        public async Task<bool> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _context.TopicModels.FirstOrDefaultAsync(x => x.TopicId == request.TopicId);
            if (topic == null) throw new BadRequestException("TopicId không tồn tại!");

            topic.Actived = false;
            //var beginGames = await _context.BeginGameModels.Where(x => x.TopicId == topic.TopicId).ToListAsync();
            //if (beginGames.Any())
            //{
            //    // Xóa bắt đầu trò chơi theo chủ đề
            //    _context.BeginGameModels.RemoveRange(beginGames);
            //}

            //var questions = await _context.QuestionModels.Where(x => x.TopicId == topic.TopicId).ToListAsync();
            //if (questions.Any())
            //{
            //    // Xóa câu hỏi theo chủ đề
            //    _context.QuestionModels.RemoveRange(questions);
            //}

            //// Xóa chủ đề
            //_context.TopicModels.Remove(topic);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
