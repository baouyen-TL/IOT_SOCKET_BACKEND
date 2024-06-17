using Core.Common;
using Core.Exceptions;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Commands.BeginGame
{
    public class DeleteBeginGameCommand : IRequest<bool>
    {
        public Guid? BeginGameId { get; set; }
    }

    public class DeleteBeginGameCommandHandler : IRequestHandler<DeleteBeginGameCommand, bool>
    {
        private readonly IOT_SOCKETContext _context;
        private readonly IUnitOfWork _UnitOfWork;

        public DeleteBeginGameCommandHandler(IOT_SOCKETContext context, IUnitOfWork UnitOfWork) {
            _context = context;
            _UnitOfWork = UnitOfWork;
        }
        public async Task<bool> Handle(DeleteBeginGameCommand request, CancellationToken cancellationToken)
        {
            var beginGame = await _context.BeginGameModels.FirstOrDefaultAsync(x => x.BeginGameId == request.BeginGameId);
            if (beginGame == null) throw new BadRequestException("BeginGameId không tồn tại!");

            
            return true;
        }
    }
}
