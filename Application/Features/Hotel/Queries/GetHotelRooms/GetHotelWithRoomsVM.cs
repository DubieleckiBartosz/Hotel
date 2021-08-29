using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelRooms
{
    public class GetHotelWithRoomsVM
    {
        public Guid HotelId { get; set; }
        public string HotelName { get; set; }
        public IEnumerable<RoomInListDto> Rooms { get; set; }
    }
}
