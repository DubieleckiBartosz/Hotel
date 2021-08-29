using Application.Contracts;
using Application.Exceptions;
using Application.Helpers;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand,Response<Guid>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        public CreateBookingCommandHandler(IRoomRepository roomRepository,IUserContextService userContextService,
            IMapper mapper,IBookingRepository bookingRepository)
        {
            _userContextService = userContextService;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }
        public async Task<Response<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomWithBookingsAsync(request.HotelId, request.RoomId);
  
            if (room is null || request.NumberOfGuests > room.NumberOfBeds)
            {
                throw new BadRequestException("Incorrect data");
            }
            if (!room.GetAvailabilityByDate(request.From,request.To))
            {
                throw new BadRequestException
                    ("You cannot book the room on this date as it is already booked");
            }

            request.Days = (request.To.Date - request.From.Date).Days;
            var userId = _userContextService.UserId;
            if (!string.IsNullOrEmpty(userId))
            {
                request.UserId = userId;
                request.FullPrice = room.GetFullPriceBooking(request.Days, room.Promotion);
            }
            else
            {
                request.FullPrice = room.GetFullPriceBooking(request.Days, "0%");
            }     
            var booking = _mapper.Map<BookingRoom>(request);
            
            await _bookingRepository.CreateAsync(booking);

            return new Response<Guid>(booking.Id) {Message= "Check email and confirm booking" };
        }
    }
}
