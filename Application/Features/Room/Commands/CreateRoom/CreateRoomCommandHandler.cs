using Application.Contracts;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Response<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoomCommandHandler> _logger;
        private readonly IRoomRepository _roomRepository;
        public CreateRoomCommandHandler(IRoomRepository roomrepository,IMapper mapper
            ,ILogger<CreateRoomCommandHandler> logger)
        {
            _roomRepository = roomrepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Response<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {

            var promotion = request.Promotion != default ? 
                request.Promotion.ToString() + "%" : "0%";
            
            var room = _mapper.Map<Domain.Entities.Room>(request);
            room.Promotion = promotion;
            await _roomRepository.CreateAsync(room);
            _logger.LogInformation($"Created new room in hotel {request.HotelId}");
            return new Response<Guid>(room.Id) { Message = "Created new room" };
        }
    }
}
