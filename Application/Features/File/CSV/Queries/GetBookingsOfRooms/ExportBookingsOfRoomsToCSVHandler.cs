using Application.Contracts;
using Application.Exceptions;
using Application.Features.File.ModelsToFile;
using Application.Helpers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.File.CSV.Queries.GetBookingsOfRooms
{
    public class ExportBookingsOfRoomsToCSVHandler : IRequestHandler<GetBookingsOfRoomsCommand, FileVM>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExporter _csvExporter;
        private readonly IUserContextService _userContextService;
        private readonly IHotelRepository _hotelRepository;
        public ExportBookingsOfRoomsToCSVHandler(IUserContextService userContextService,
            IHotelRepository hotelRepository, IRoomRepository roomRepository,
            IMapper mapper, ICsvExporter csvExporter)
        {
            _hotelRepository = hotelRepository;
            _userContextService = userContextService;
            _csvExporter = csvExporter;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        public async Task<FileVM> Handle(GetBookingsOfRoomsCommand request, CancellationToken cancellationToken)
        {

            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            if(hotel.UserId!=request.UserId && !_userContextService.IsAdmin())
            {
                throw new BadRequestException("You are not authorized to download file");

            }

            var roomBookings =await _roomRepository.GetRoomsByParametersAsync(request.HotelId, 
                request.StartDate.GetDateTime(), request.EndDate.GetDateTime());

            if (!roomBookings.Any())
            {
                throw new BadRequestException("Incorrect data");
            }
            var bookings = roomBookings.SelectMany(c => c.BookingRooms);
            var bookingsResult = _mapper.Map<IEnumerable<BookingFileVM>>(bookings);
            
            var fileData = _csvExporter.GetToCsvExport<BookingFileVM>(bookingsResult);
            var name = request.FileName is not null ? request.FileName : Guid.NewGuid().ToString();

            var exportFile = new FileVM { ContentType = "text/csv", ExportFileName = $"{name}.csv", Data = fileData };
            return exportFile;
        }
    }
}
