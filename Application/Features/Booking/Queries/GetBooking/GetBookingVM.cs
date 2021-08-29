using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Booking.Queries.GetBooking
{
    public class GetBookingVM
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Days { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal PricePerPerson { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string FullPrice { get; set; }
        public string Discount { get; set; }
    }
}
