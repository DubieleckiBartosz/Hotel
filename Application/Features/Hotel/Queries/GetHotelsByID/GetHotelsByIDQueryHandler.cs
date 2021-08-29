using Application.Contracts;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelsByID
{
    public class GetHotelsByIDQueryHandler : IRequestHandler<GetHotelsByIdQuery, IEnumerable<GetHotelsByIdVM>>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        public GetHotelsByIDQueryHandler(IHotelRepository hotelRepository,IMapper mapper)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<GetHotelsByIdVM>> Handle(GetHotelsByIdQuery request, CancellationToken cancellationToken)
        {
            var hotels =await _hotelRepository.GetHotelsByIdAsync(request.Ids);
            if(!hotels.Any() ||
                hotels.Count() != request.Ids.Count())
            {
                throw new BadRequestException("Your IDs are not correct");
            }
            var hotelsMap = _mapper.Map<IEnumerable<GetHotelsByIdVM>>(hotels);
            return hotelsMap;
        }
    }
}
