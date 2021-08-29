using Application.Contracts;
using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.UpdateRoom
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        public UpdateRoomCommandHandler(IRoomRepository roomRepository,IMapper mapper)
        {
            _mapper = mapper;
            _roomRepository = roomRepository;
        }
        public async Task<Response<string>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.HotelId, request.RoomId);
            if(room is null)
            {
                throw new BadRequestException("The IDs entered are invalid");
            }
            if(request.NewPrice == room.PricePerPerson)
            {
                throw new BadRequestException("You tried entered data the same like before");
            }
            await _roomRepository.UpdateAsync(room);
            return new Response<string>("Changed price per person");
        }
    }
}
