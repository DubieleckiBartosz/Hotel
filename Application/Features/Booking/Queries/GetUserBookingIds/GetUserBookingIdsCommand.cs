﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Booking.Queries.GetUserBookingIds
{
    public class GetUserBookingIdsCommand:IRequest<GetUserBookingsBasicDataVM>
    {
        public Guid HotelId { get; set; }
    }
}
