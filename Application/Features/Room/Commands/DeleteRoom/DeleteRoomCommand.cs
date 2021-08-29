using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.DeleteRoom
{
    public class DeleteRoomCommand:IRequest
    {
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
    }
}
