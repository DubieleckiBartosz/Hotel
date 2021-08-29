using Application.Attributes;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json.Serialization;


namespace Application.Features.Hotel.Commands.CreatePictureHotel
{
    public class CreateHotelPictureCommand:IRequest<Response<Guid>>
    {
        [JsonIgnore]
        public Guid HotelId { get; set; }
      
        [FileValidation]
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public bool Main { get; set; }
    }
    
}
