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

namespace Application.Features.Room.Queries.GetRooms
{
    public class GetRoomsQueryHadler : IRequestHandler<GetRoomsQuery, Response<IEnumerable<GetRoomsVM>>>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        public GetRoomsQueryHadler(IMapper mapper, IRoomRepository roomRepository)
        {
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<Response<IEnumerable<GetRoomsVM>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _roomRepository.GetRoomsWithAvailabilityAsync(request.HotelId);
            if (!rooms.Any())
            {
                throw new NotFoundException($"Rooms not found in hotel {request.HotelId}");
            }

            var roomsMap = _mapper.Map<IEnumerable<GetRoomsVM>>(rooms);
            return new Response<IEnumerable<GetRoomsVM>>(roomsMap);
        }
    }
}
