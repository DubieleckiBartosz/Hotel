using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelRooms
{
    public class GetHotelWithRoomsQuery:IRequest<Response<GetHotelWithRoomsVM>>
    {
        public Guid HotelId { get; set; }
    }
}
