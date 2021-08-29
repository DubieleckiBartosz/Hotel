using Application.Contracts;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.DeleteRoom
{
    public class DeleteRoomCommandHandler:IRequestHandler<DeleteRoomCommand>
    {
        private readonly ILogger<DeleteRoomCommandHandler> _logger;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserContextService _userContextService;
        public DeleteRoomCommandHandler(IRoomRepository roomrepository,
            ILogger<DeleteRoomCommandHandler> logger,IUserContextService userContext)
        {
            _roomRepository = roomrepository;
            _logger = logger;
            _userContextService = userContext;
        }

        public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.HotelId, request.RoomId);
            var userId = _userContextService.UserId;
            if(room.Hotel.UserId!=userId && !_userContextService.IsAdmin())
            {
                _logger.LogInformation($"User {userId} tried delete room {request.RoomId}");
                throw new BadRequestException("You are not authorized to delete this room");
            }
            if(room is null)
            {
                throw new BadRequestException("The IDs entered are invalid");
            }
            await _roomRepository.DeleteAsync(room);
            _logger.LogInformation($"Removed room {room.Id}");
            return Unit.Value;
        }
    }
}
