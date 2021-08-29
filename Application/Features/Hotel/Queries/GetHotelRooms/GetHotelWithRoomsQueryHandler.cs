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

namespace Application.Features.Hotel.Queries.GetHotelRooms
{
    public class GetHotelWithRoomsQueryHandler:IRequestHandler<GetHotelWithRoomsQuery,Response<GetHotelWithRoomsVM>>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        public GetHotelWithRoomsQueryHandler(IMapper mapper,IHotelRepository hotelRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<Response<GetHotelWithRoomsVM>> Handle(GetHotelWithRoomsQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelWithRoomsAsync(request.HotelId);
            if(hotel is null)
            {
                throw new BadRequestException($"Hotel id is incorrect {request.HotelId}");
            }
            if (!hotel.Rooms.Any())
            {
                throw new NotFoundException($"Rooms not found in hotel {request.HotelId}");
            }

            var hotelMap = _mapper.Map<GetHotelWithRoomsVM>(hotel);
            return new Response<GetHotelWithRoomsVM>(hotelMap);
        }
    }
}
