using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomCommand:IRequest<Response<Guid>>
    {
        [JsonIgnore]
        public Guid HotelId { get; set; }
        public decimal Promotion { get; set; } = 0;
        public int NumberOfBeds { get; set; }
        public int PricePerPerson { get; set; }
    }
}
