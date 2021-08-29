using Application.AccountModels.Enums;
using Application.Contracts;
using Application.Exceptions;
using Application.Helpers;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.CreatePictureHotel
{
    public class CreateHotelPictureCommandHandler : IRequestHandler<CreateHotelPictureCommand, Response<Guid>>
    {
        private readonly IHotelPictureRepository _pictureRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IAttachmentHotelRepository _attachmentHotelRepository;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<CreateHotelPictureCommandHandler> _logger;
        public CreateHotelPictureCommandHandler(IHotelRepository hotelRepository,
            IAttachmentHotelRepository attachmentHotelRepository,
            IHotelPictureRepository pictureRepository,
            IUserContextService userContextService,ILogger<CreateHotelPictureCommandHandler> logger)
        {
            _userContextService = userContextService;
            _attachmentHotelRepository = attachmentHotelRepository;
            _hotelRepository = hotelRepository;
            _pictureRepository = pictureRepository;
            _logger = logger;
        }
        public async Task<Response<Guid>> Handle(CreateHotelPictureCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            var userId = _userContextService.UserId;
            if (hotel.UserId != userId && !_userContextService.IsAdmin())
            {
                _logger.LogInformation($"User {userId} tried to add a new photo to the hotel {request.HotelId}");
                throw new BadRequestException("You are not authorized to add this image");
            }
            var pictures = await _pictureRepository.GetPicturesByHotelIdAsync(request.HotelId);

            if (hotel is null)
            {
                throw new NotFoundException($"Hotel {request.HotelId} not found");
            }
            if (request.File.Length < 1024*1024)
            {
                HotelPicture picture = new()
                {
                    Name = request.Name is null ? request.File.FileName : request.Name,
                    Main = pictures.Count() is 0 ? true : false,
                    Image = request.File.GetContentBytes(),
                    HotelId = hotel.Id,
                };

                await _pictureRepository.CreateAsync(picture);
                return new Response<Guid>(picture.Id)
                { Message = GetResponseMessage(picture.Name, picture.HotelId) };
            }
            else
            {
                HotelAttachment attachment = new()
                {
                    Name= request.Name is null ? request.File.FileName : request.Name,
                    Path=request.File.SaveFile(),
                    HotelId=hotel.Id
                };
                await _attachmentHotelRepository.CreateAsync(attachment);
                return new Response<Guid>(attachment.Id)
                { Message = GetResponseMessage(attachment.Name,attachment.HotelId) };
            }
        }

        private string GetResponseMessage(string name, Guid id) =>
            $"Added new picture with name {name} to hotel {id}";
    }
}
