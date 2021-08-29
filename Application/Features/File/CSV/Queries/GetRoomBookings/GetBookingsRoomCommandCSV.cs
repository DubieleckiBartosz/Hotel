using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.File.CSV.Queries.GetRoomBookings
{
    public class GetBookingsRoomCommandCSV:GetCSV,IRequest<FileVM>
    {
        public Guid RoomId { get; set; }
    }
}
