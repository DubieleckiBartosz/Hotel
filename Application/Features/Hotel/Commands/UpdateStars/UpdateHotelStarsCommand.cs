using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.UpdateStars
{
    public class UpdateHotelStarsCommand:IRequest<Response<string>>
    {
        [JsonIgnore]
        public Guid HotelId { get; set; }
        public int Stars { get; set; }
    }
}
