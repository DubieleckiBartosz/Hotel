using Application.Features.Booking.Commands.CreateBooking;
using Application.Features.Booking.Commands.DeleteBooking;
using Application.Features.Booking.Commands.UpdateBooking;
using Application.Features.Booking.Queries.GetBooking;
using Application.Features.Booking.Queries.GetUserBookingIds;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.WebApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/{hotelId}/{roomId}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BookingController : BaseController
    {
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get bookings")]
        [HttpGet("~/api/v{version:apiVersion}/{hotelId}/[action]", Name = "GetBookingsForUser")]
        public async Task<IActionResult> GetBookingsForUser([FromRoute]Guid hotelId)
        {
            var result = await Mediator.Send(new GetUserBookingIdsCommand {HotelId=hotelId });
            return Ok(result);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get booking by id")]
        [HttpGet("~/api/v{version:apiVersion}/[action]/{bookingId}", Name ="GetBooking")]
        public async Task<IActionResult> GetBooking([FromRoute] Guid bookingId)
        {
            var result = await Mediator.Send(new GetBookingByIdQuery { BookingId = bookingId});
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create booking")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromRoute]Guid hotelId,
            [FromRoute]Guid roomId, [FromBody]CreateBookingCommand command)
        {
            command.RoomId = roomId;
            command.HotelId = hotelId;
            
            return Ok(await Mediator.Send(command));
        }
        
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update booking")]
        [HttpPatch("~/api/v{version:apiVersion}/[action]/{bookingId}")]
        public async Task<IActionResult> UpdateBooking([FromRoute] Guid bookingId,
            [FromBody] JsonPatchDocument<UpdateBookingDto> patch)
        {
            var result=await Mediator.Send(new UpdateBookingCommand { BookingId = bookingId, Patch = patch });
            return RedirectToAction("GetBooking", new { bookingId = result.Data });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(Summary = "Remove booking")]
        [HttpDelete("~/api/v{version:apiVersion}/[action]/{bookingId}")]
        public async Task<IActionResult> DeleteBooking([FromRoute]Guid bookingId)
        {
            await Mediator.Send(new DeleteBookingCommand {BookingId=bookingId});
            return NoContent();
        }

    }
}
