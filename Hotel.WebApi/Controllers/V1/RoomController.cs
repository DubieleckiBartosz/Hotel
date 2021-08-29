using Application.Features.Room.Commands.CreateRoom;
using Application.Features.Room.Commands.DeleteRoom;
using Application.Features.Room.Commands.UpdateRoom;
using Application.Features.Room.Queries.GetRoomById;
using Application.Features.Room.Queries.GetRooms;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.WebApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/{hotelId}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RoomController : BaseController
    {
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Rooms with details")]
        [HttpGet(Name = "GetRooms")]
        public async Task<IActionResult> GetRooms([FromRoute] Guid hotelId) =>
              Ok(await Mediator.Send(new GetRoomsQuery { HotelId = hotelId }));

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Room with pictures")]
        [HttpGet("{id}", Name = "GetRoom")]
        public async Task<IActionResult> GetRoom([FromRoute] Guid hotelId, [FromRoute] Guid id) =>
             Ok(await Mediator.Send(new GetRoomByIdQuery { HotelId = hotelId, RoomId = id }));


        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Create New Room")]
        [HttpPost(Name = "CreateRoom")]
        public async Task<IActionResult> CreateRoom([FromRoute] Guid hotelId, [FromBody] CreateRoomCommand command)
        {
            command.HotelId = hotelId;
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Update Room")]
        [HttpPut("{id}", Name = "UpdateRoom")]
        public async Task<IActionResult> UpdateRoom([FromRoute] Guid hotelId, [FromRoute] Guid id, [FromBody] decimal price) =>
          Ok(await Mediator.Send(new UpdateRoomCommand { HotelId = hotelId, RoomId = id, NewPrice = price }));


        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Remove Room")]
        [HttpDelete("{id}", Name = "DeleteRoom")]
        public async Task<IActionResult> DeleteRoom([FromRoute] Guid hotelId, [FromRoute] Guid id)
        {
            await Mediator.Send(new DeleteRoomCommand { HotelId = hotelId, RoomId = id });
            return NoContent();
        }


    }
}
