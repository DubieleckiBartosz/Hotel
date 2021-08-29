using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Booking.Queries.GetUserBookingIds
{
    public class BookingDto
    {
        public DateTime Created{ get; set; }
        public string Status { get; set; }
        public Guid BookingId { get; set; }
    }
}
