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

namespace Application.Features.Hotel.Queries.GetHotelByID
{
    public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, Response<GetHotelVM>>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        public GetHotelQueryHandler(IMapper mapper,IHotelRepository hotelRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<Response<GetHotelVM>> Handle(GetHotelQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelWithDetailsByIdAsync(request.HotelId);
            if(hotel is null)
            {
                throw new NotFoundException($"Hotel {request.HotelId} not found.");
            }

            var result = _mapper.Map<GetHotelVM>(hotel);
            return new Response<GetHotelVM>(result);
        }
    }
}
