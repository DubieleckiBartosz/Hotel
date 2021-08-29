using Application.Contracts;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Booking.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<DeleteBookingCommandHandler> _logger;
        public DeleteBookingCommandHandler(IBookingRepository bookingRepository,
            IUserContextService userContextService,ILogger<DeleteBookingCommandHandler> logger)
        {
            _logger = logger;
            _userContextService = userContextService;
            _bookingRepository = bookingRepository;
        }
        public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.UserId;
            if (!string.IsNullOrEmpty(userId))
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
                if(booking.UserId!=userId && (!_userContextService.IsAdmin()
                    && booking.Room.Hotel.UserId != userId))
                {
                    throw new BadRequestException("You can't perform this operation");
                }
                else
                {
                    if (booking is null)
                    {
                        throw new NotFoundException($"{request.BookingId} not found");
                    }
                    await _bookingRepository.DeleteAsync(booking);
                    _logger.LogInformation($"{DateTime.Now}: {userId} deleted the booking {booking.Id}");
                }
            }
            else
            {
                throw new BadRequestException("You can't perform this operation");
            }
            return Unit.Value;
        }
    }
}
