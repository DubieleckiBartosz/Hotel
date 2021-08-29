using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Booking.Queries.GetBooking
{
    public class GetBookingByIdQuery:IRequest<Response<GetBookingVM>>
    {
        public Guid BookingId { get; set; }
    }
}
