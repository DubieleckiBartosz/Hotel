using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.UpdateRoom
{
    public class UpdateRoomCommand:IRequest<Response<string>>
    {
        [JsonIgnore]
        public Guid HotelId { get; set; }
        [JsonIgnore]
        public Guid RoomId { get; set; }
        [JsonPropertyName("New Price Per Person")]
        public decimal NewPrice { get; set; }
        [JsonPropertyName("Promotion in percent")]
        public int Promotion { get; set; }
    }
}
