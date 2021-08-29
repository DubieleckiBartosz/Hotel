using Application.Features.File;
using Application.Features.File.CSV.Queries.GetBookingsOfRooms;
using Application.Features.File.CSV.Queries.GetRoomBookings;
using Application.Features.File.TXT.Queries.GetBookingsOfRoomsRaportTXT;
using Hotel.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotel.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : BaseController
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Export to csv reservations of the selected room")]
        [HttpGet("export",Name = "ExportBookingsOneRoom")]
        [FileContentType("text/csv")]
        public async Task<FileResult> ExportBookingsRoom([FromQuery] GetBookingsRoomCommandCSV command)
        {
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileDto = await Mediator.Send(command);
            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Export bookings to csv")]
        [HttpGet("exportBookings", Name = "ExportBookingsRooms")]
        [FileContentType("text/csv")]
        public async Task<FileResult> ExportBookingsRooms([FromQuery] GetBookingsOfRoomsCommand command)
        {
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileDto = await Mediator.Send(command);
            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Export bookings to txt")]
        [HttpGet("exportBookingsToTxt", Name = "ExportBookingsRoomsTxt")]
        [FileContentType("text/Text")]
        public async Task<FileResult> ExportBookingsRooms([FromQuery] GetBookingsOfRoomsTXTCommand command)
        {
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileDto = await Mediator.Send(command);
            return File(fileDto.Data, fileDto.ContentType, fileDto.ExportFileName);
        }
    }
}
