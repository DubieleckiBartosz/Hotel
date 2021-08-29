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

namespace Application.Features.Booking.Queries.GetUserBookingIds
{
    public class GetUserBookingIdsHandler : IRequestHandler<GetUserBookingIdsCommand, GetUserBookingsBasicDataVM>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        public GetUserBookingIdsHandler(IHotelRepository hotelRepository,
            IUserContextService userContextService,IMapper mapper)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _userContextService = userContextService;
        }
        public async Task<GetUserBookingsBasicDataVM> Handle(GetUserBookingIdsCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.UserId;
            if(!string.IsNullOrEmpty(userId))
            {
                var hotel = await _hotelRepository.GetbookingsHotelForUserAsync(userId,request.HotelId);
                if (hotel!=null)
                {
                    var bookings = hotel.Rooms.SelectMany(s => s.BookingRooms);
                    if (bookings.Any())
                    {
                        var bookingsMap = _mapper.Map<IEnumerable<BookingDto>>(bookings);
                        return new GetUserBookingsBasicDataVM() { Bookings = bookingsMap };
                    }
                    throw new NotFoundException($"Not found bookings for user {userId}");
                }               
            }
            throw new BadRequestException("Log in and try again");
        }
    }
}
