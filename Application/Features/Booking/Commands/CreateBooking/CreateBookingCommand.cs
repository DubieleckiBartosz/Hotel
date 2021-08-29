using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand:IRequest<Response<Guid>>
    {
        [JsonIgnore]
        public Guid RoomId { get; set; }

        [JsonIgnore]
        public Guid HotelId { get; set; }

        [JsonIgnore]
        public string UserId { get; set; } 
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, 
            DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime From { get; set; }
      
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,
            DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime To { get; set; }
        public string Email { get; set; }
       
        [JsonIgnore]
        public decimal FullPrice { get; set; }

        [JsonIgnore]
        public int Days { get; set; }

        [Phone(ErrorMessage = "Please enter a valid Phone No")]
        public string PhoneNumber { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
