using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room: AuditableEntity
    {
        public Guid Id { get; set; }
        public int NumberOfBeds { get; set; }
        public decimal PricePerPerson { get; set; }
        public string? Promotion { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public virtual ICollection<BookingRoom> BookingRooms { get; set; }
        public virtual ICollection<RoomPicture> RoomPictures { get; set; }
        public virtual ICollection<RoomAttachment> RoomAttachments { get; set; }
        public Room() { }
        
        public Room(int beds, decimal price, Guid hotelId)
        {
            NumberOfBeds = beds;
            PricePerPerson = price;
            HotelId = hotelId;
        }

    }
}
