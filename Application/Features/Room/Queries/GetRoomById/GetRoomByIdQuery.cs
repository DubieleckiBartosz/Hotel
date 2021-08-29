using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Room.Queries.GetRoomById
{
    public class GetRoomByIdQuery:IRequest<Response<GetRoomVM>>
    {
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
    }
}
