using Application.Contracts;
using Application.Exceptions;
using Application.Features.File.CSV.Queries.GetRoomBookings;
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

namespace Application.Features.File.CSV.Queries
{
    public class ExportBookingsOfRoomToCSVHandler : IRequestHandler<GetBookingsRoomCommandCSV, FileVM>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICsvExporter _csvExporter;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IHotelRepository _hotelRepository;
        public ExportBookingsOfRoomToCSVHandler(IHotelRepository hotelRepository,IUserContextService userContextService,
            IRoomRepository roomRepository,ICsvExporter csvExporter,IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _userContextService = userContextService;
            _csvExporter = csvExporter;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }
        public async Task<FileVM> Handle(GetBookingsRoomCommandCSV request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            if (hotel.UserId != request.UserId && !_userContextService.IsAdmin())
            {
                throw new BadRequestException("You are not authorized to download file");

            }
            var bookingsRoom=await _roomRepository.GetRoomByParametersAsync(request.HotelId, request.RoomId, 
               request.StartDate.GetDateTime(), request.EndDate.GetDateTime());
           
            if(bookingsRoom is null)
            {
                throw new BadRequestException("Incorrect data");
            }
            if (!bookingsRoom.BookingRooms.Any())
            {
                throw new NotFoundException($"There is no reservation for a room {request.RoomId}");
            }
            var bookings = bookingsRoom.BookingRooms;
            var bookingsResult = _mapper.Map<IEnumerable<BookingFileVM>>(bookings);
            var fileData = _csvExporter.GetToCsvExport<BookingFileVM>(bookingsResult);
            var name = request.FileName is not null? request.FileName : Guid.NewGuid().ToString();

            var exportFile = new FileVM { ContentType = "text/csv", ExportFileName = $"{name}.csv", Data = fileData };
            return exportFile;
        }
    }
}
