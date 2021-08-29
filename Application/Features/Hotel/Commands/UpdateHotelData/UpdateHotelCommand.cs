using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.UpdateHotelData
{
    public class  UpdateHotelCommand : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public JsonPatchDocument<UpdateHotelDto> Patch { get; set; }
        
    }
    public class UpdateHotelDto
    {
        public string HotelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
   
}
