using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelRooms
{
    public class RoomInListDto
    {
        public Guid Id { get; set; }
        public int NumberOfBeds { get; set; }
        public decimal PricePerPerson { get; set; }
        public bool IsAvailable { get; set; }
    }
}
