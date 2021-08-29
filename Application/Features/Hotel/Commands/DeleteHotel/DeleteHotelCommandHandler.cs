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

namespace Application.Features.Hotel.Commands.DeleteHotel
{
    public class DeleteHotelCommandHandler:IRequestHandler<DeleteHotelCommand,Response<string>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<DeleteHotelCommandHandler> _logger;
        private readonly IUserContextService _userContextService;
        public DeleteHotelCommandHandler(IUserContextService userContextService,ILogger<DeleteHotelCommandHandler> logger,IHotelRepository hotelRepository)
        {
            _userContextService = userContextService;
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            var userId = _userContextService.UserId;
            if(hotel.UserId!=userId && !_userContextService.IsAdmin())
            {
                _logger.LogInformation($"User {userId} tried delete hotel {request.HotelId}");
                throw new BadRequestException("You are not authorized to delete this hotel");
            }
            if(hotel is null)
            {
                throw new NotFoundException($"Hotel {request.HotelId} not found.");
            }
             await _hotelRepository.DeleteAsync(hotel);
            _logger.LogInformation($"Removed hotel with Id {hotel.Id}");
            return new Response<string>() { Message = "Hotel removed correctly", Succeeded = true };
        }
    }
}
