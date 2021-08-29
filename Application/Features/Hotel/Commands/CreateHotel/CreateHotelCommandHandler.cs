using Application.Contracts;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Wrappers;
using Microsoft.Extensions.Logging;

namespace Application.Features.Hotel.Commands.CreateHotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Response<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private ILogger<CreateHotelCommandHandler> _logger;
        public CreateHotelCommandHandler(IMapper mapper, IHotelRepository hotelRepository,ILogger<CreateHotelCommandHandler> logger)
        {
            _logger = logger;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<Response<Guid>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
                var modelDb = _mapper.Map<Domain.Entities.Hotel>(request);
                await _hotelRepository.CreateAsync(modelDb);
            _logger.LogInformation($"Created new Hotel {modelDb.Id}");
                return new Response<Guid>(modelDb.Id);
        }

    }
}
