using Application.Contracts;
using Application.Exceptions;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.UpdateStars
{
    public class UpdateHotelStarsCommandHandler : IRequestHandler<UpdateHotelStarsCommand,Response<string>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<UpdateHotelStarsCommandHandler> _logger;
        private readonly IUserContextService _userContextService;
        public UpdateHotelStarsCommandHandler(IUserContextService userContextService,
            IHotelRepository hotelRepository,ILogger<UpdateHotelStarsCommandHandler> logger)
        {
            _userContextService = userContextService;
            _logger = logger;
            _hotelRepository=hotelRepository;
        }

        public async Task<Response<string>> Handle(UpdateHotelStarsCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            var userId = _userContextService.UserId;
            if (hotel.UserId != userId && !_userContextService.IsAdmin())
            {
                _logger.LogInformation($"User {userId} tried add stars to hotel {request.HotelId}");
                throw new BadRequestException("You are not authorized to add stars to this hotel");
            }
            if (hotel is null)
            {
                throw new NotFoundException($"Hotel {request.HotelId} not found");
            }
            if (hotel.Stars==request.Stars)
            {
                throw new BadRequestException($"The hotel already has {request.Stars} stars");
            }
            hotel.Stars = request.Stars;
            await _hotelRepository.UpdateAsync(hotel);
            _logger.LogInformation($"Star rating changed from {hotel.Stars} to {request.Stars}");
            return new Response<string>($"Star rating changed");
        }
    }
}
