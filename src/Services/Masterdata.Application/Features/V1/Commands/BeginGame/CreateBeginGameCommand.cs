using Core.Common;
using Core.Responses;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.Commands.Question;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Commands.BeginGame
{
    public class CreateBeginGameCommand : IRequest<ApiSuccessResponse>
    {
        public Guid? TopicId { get; set; }
        public string ClassName { get; set; }
        public List<CreateUserNameCommand> ListUserNames { get; set; } = new();
    }

    public class CreateUserNameCommand
    {
        public Guid? RemoteId { get; set; }
        public string UserName { get; set; }
    }

    public class CreateBeginGameCommandHandler : IRequestHandler<CreateBeginGameCommand, ApiSuccessResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOT_SOCKETContext _context;

        public CreateBeginGameCommandHandler(IOT_SOCKETContext context, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<ApiSuccessResponse> Handle(CreateBeginGameCommand request, CancellationToken cancellationToken)
        {
            ApiSuccessResponse apiSuccess = new ApiSuccessResponse();
            try
            {
                // Add BeginGame
                var beginGameEntity = new BeginGameModel
                {
                    BeginGameId = Guid.NewGuid(),
                    TopicId = request.TopicId,
                    ClassName = request.ClassName,
                    CreateTime = DateTime.Now,
                };

                _context.BeginGameModels.Add(beginGameEntity);

                // Add UserName
                if (request.ListUserNames.Any())
                {
                    foreach (var item in request.ListUserNames)
                    {
                        var userNameEntity = new UserGameModel
                        {
                            UserGameId = Guid.NewGuid(),
                            BeginGameId = beginGameEntity.BeginGameId,
                            RemoteId = item.RemoteId,
                            UserName = item.UserName,
                            CreateTime = DateTime.Now
                        };

                        _context.UserGameModels.Add(userNameEntity);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                apiSuccess.Data = beginGameEntity.BeginGameId;
                apiSuccess.Message = "Tạo thông tin thành công, bắt đầu trò chơi!!";
                return apiSuccess;
            }
            catch (Exception ex)
            {
                apiSuccess.IsSuccess = false;
                apiSuccess.Message = ex.Message;
                return apiSuccess;
            }
        }
    }
}
