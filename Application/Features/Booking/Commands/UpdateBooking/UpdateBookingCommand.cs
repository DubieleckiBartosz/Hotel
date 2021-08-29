using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Application.Features.Booking.Commands.UpdateBooking
{
    public class UpdateBookingCommand:IRequest<Response<Guid>>
    {
        [JsonIgnore]
        public Guid BookingId { get; set; }

        [JsonIgnore]
        public Guid RoomId { get; set; }

        [JsonIgnore]
        public Guid HotelId { get; set; }
        public JsonPatchDocument<UpdateBookingDto> Patch { get; set; }
    }
    public class UpdateBookingDto
    {

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,
            DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,
            DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime To { get; set; }

        [JsonIgnore]
        public decimal FullPrice { get; set; }

        [JsonIgnore]
        public int Days { get; set; }

        public int NumberOfGuests { get; set; }
    }
}
