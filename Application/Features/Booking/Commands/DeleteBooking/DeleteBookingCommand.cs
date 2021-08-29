using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Booking.Commands.DeleteBooking
{
    public class DeleteBookingCommand:IRequest
    {
        public Guid BookingId { get; set; }
    }
}
