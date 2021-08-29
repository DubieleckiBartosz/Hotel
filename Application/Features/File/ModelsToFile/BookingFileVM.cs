using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.File.ModelsToFile
{
    public class BookingFileVM
    {
        public Guid RoomId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Days { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public decimal FullPrice { get; set; }
        public decimal Discount { get; set; }
    }
}
