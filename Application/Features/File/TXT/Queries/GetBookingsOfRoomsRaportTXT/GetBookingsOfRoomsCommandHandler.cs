using Application.Contracts;
using Application.Exceptions;
using Application.Features.File.ModelsToFile;
using Application.Helpers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.File.TXT.Queries.GetBookingsOfRoomsRaportTXT
{
    class GetBookingsOfRoomsCommandHandler:IRequestHandler<GetBookingsOfRoomsTXTCommand,FileVM>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IHotelRepository _hotelRepository;
        public GetBookingsOfRoomsCommandHandler(IHotelRepository hotelRepository,IUserContextService userContextService,
            IRoomRepository roomRepository,IMapper mapper)
        {
            _userContextService = userContextService;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<FileVM> Handle(GetBookingsOfRoomsTXTCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            if (hotel.UserId != request.UserId && !_userContextService.IsAdmin())
            {
                throw new BadRequestException("You are not authorized to download file");

            }

            var roomBookings = await _roomRepository.GetRoomsByParametersAsync(request.HotelId,
               request.StartDate.GetDateTime(), request.EndDate.GetDateTime());

            if (!roomBookings.Any())
            {
                throw new BadRequestException("Incorrect data");
            }
            var bookings = roomBookings.SelectMany(c => c.BookingRooms);
            var bookingsfile = _mapper.Map<IEnumerable<BookingFileVM>>(bookings);
            var fileData = bookingsfile.GetContentToTXT();
            var name = request.FileName is not null ? request.FileName : Guid.NewGuid().ToString();

            var exportFile = new FileVM { ContentType = "text/Text", ExportFileName = $"{name}.txt", Data = fileData };
            return exportFile;
        }
    }
}
