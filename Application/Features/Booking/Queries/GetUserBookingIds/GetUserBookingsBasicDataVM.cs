using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Booking.Queries.GetUserBookingIds
{
    public class GetUserBookingsBasicDataVM
    {
        public IEnumerable<BookingDto> Bookings{ get; set; }
    }
}
