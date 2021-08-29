using Application.Contracts;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.UpdateHotelData
{
    public class UpdateHotelCommandHandler:IRequestHandler<UpdateHotelCommand>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateHotelCommandHandler> _logger;
        private readonly IUserContextService _userContextService;
        public UpdateHotelCommandHandler(IMapper mapper,IHotelRepository hotelRepository,
            IUserContextService userContextService,ILogger<UpdateHotelCommandHandler> logger)
        {
            _hotelRepository = hotelRepository;
            _userContextService = userContextService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel =await _hotelRepository.GetHotelByIdAsync(request.Id);
            var userId = _userContextService.UserId;
            if (hotel.UserId != userId && !_userContextService.IsAdmin())
            {
                _logger.LogInformation($"User {userId} tried update hotel {request.Id}");
                throw new BadRequestException("You are not authorized to update this hotel");
            }
            if(hotel is null)
            {
                throw new NotFoundException($"Hotel {request.Id} not found");
            }

            var hotelDto = _mapper.Map<UpdateHotelDto>(hotel);
            request.Patch.ApplyTo(hotelDto);
            _mapper.Map(hotelDto, hotel);
            await _hotelRepository.UpdateAsync(hotel);

            return Unit.Value;
        }
    }
}
