using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.CreateHotel
{
    public class CreateHotelCommand :IRequest<Response<Guid>>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string HotelName { get; set; }
        public int Stars { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        [Phone(ErrorMessage = "Please enter a valid Phone No")]//hmm?
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
