using Application.Contracts;
using Application.Exceptions;
using Application.Helpers;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Booking.Queries.GetBooking
{
    public class GetBookingByIdQueryHandler:IRequestHandler<GetBookingByIdQuery, Response<GetBookingVM>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository,IMapper mapper)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }

        public async Task<Response<GetBookingVM>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
            if(booking is null)
            {
                throw new NotFoundException($"Booking {request.BookingId} not found");
            }
            var bookingMap = _mapper.Map<GetBookingVM>(booking);
            bookingMap.Discount=bookingMap.Discount.CheckDiscount();
            return new Response <GetBookingVM>(bookingMap);
        }
    }
}
