using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookingRoom: AuditableEntity
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Days { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfGuests { get; set; }
        public BookingStatus Status { get; set; } = 0;
        public PaymentStatus PaymentStatus { get; set; } = 0;
        public decimal FullPrice { get; set; }
        public decimal? Discount { get; set; }

        public bool IsPaid { get; set; }
        public BookingRoom() { }

        public BookingRoom(Guid roomId, int guests,DateTime to,DateTime from,string phoneNumber)
        {
            From = from;
            To = to;
            RoomId = roomId;
            NumberOfGuests = guests;
            PhoneNumber = phoneNumber;
            IsPaid = false;
        }
    }
}
