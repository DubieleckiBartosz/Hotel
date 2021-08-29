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

namespace Application.Features.Room.Queries.GetRoomById
{
    public class GetRoomByIdQueryHandler:IRequestHandler<GetRoomByIdQuery,Response<GetRoomVM>>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomrepository;
        public GetRoomByIdQueryHandler(IRoomRepository roomRepository,IMapper mapper)
        {
            _mapper = mapper;
            _roomrepository = roomRepository;
        }

        public async Task<Response<GetRoomVM>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomrepository.GetRoomWithBookingsAsync(request.HotelId, request.RoomId);
            if(room is null)
            {
                throw new BadRequestException("The IDs entered are invalid");
            }
            var roomMap = _mapper.Map<GetRoomVM>(room);
            return new Response<GetRoomVM>(roomMap);
        }
    }
}
