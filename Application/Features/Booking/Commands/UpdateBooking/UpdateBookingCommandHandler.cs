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

namespace Application.Features.Booking.Commands.UpdateBooking
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand,Response<Guid>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        public UpdateBookingCommandHandler(IBookingRepository bookingRepository,IMapper mapper)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }

        public async Task<Response<Guid>> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
            if (booking is null)
            {
                throw new NotFoundException($"{request.BookingId} not found");
            }
            var bookingMap = _mapper.Map<UpdateBookingDto>(booking);
            request.Patch.ApplyTo(bookingMap);

            bookingMap.Days = (bookingMap.To.Date - bookingMap.From.Date).Days;
            bookingMap.FullPrice = booking.Room.GetFullPriceBooking(bookingMap.Days,booking.Room.Promotion);
            _mapper.Map(bookingMap, booking);
        
            await _bookingRepository.UpdateAsync(booking);
          
            return new Response<Guid>(booking.Id);
        }
    }
}
