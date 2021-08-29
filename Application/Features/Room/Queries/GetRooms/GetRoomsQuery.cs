using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Room.Queries.GetRooms
{
    public class GetRoomsQuery:IRequest<Response<IEnumerable<GetRoomsVM>>>
    {
        public Guid HotelId { get; set; }
    }
}
