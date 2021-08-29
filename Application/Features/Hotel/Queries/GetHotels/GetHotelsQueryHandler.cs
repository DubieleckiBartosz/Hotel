using Application.Contracts;
using Application.Exceptions;
using Application.ResourceParameters;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotels
{
    public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQueryParameters,PagedList<GetHotelsVM>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        public GetHotelsQueryHandler(IHotelRepository hotelRepository,IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<GetHotelsVM>> Handle(GetHotelsQueryParameters request, CancellationToken cancellationToken)
        {
            (IEnumerable<Domain.Entities.Hotel> hotels,int count ) = await _hotelRepository.FindAllHotelsAsync(request.PageNumber, request.PageSize, 
                request.SearchQuery,request.Stars);
           
            if (!hotels.Any())
            {
                throw new NotFoundException("List of hotels is empty");
            }
            var hotelsMap = _mapper.Map<IEnumerable<GetHotelsVM>>(hotels);

            return new PagedList<GetHotelsVM>(hotelsMap, count, request.PageNumber, request.PageSize);
        }
    }
}
