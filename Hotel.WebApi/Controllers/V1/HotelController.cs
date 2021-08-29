using Application.Attributes;
using Application.Features.Hotel.Commands.CreateHotel;
using Application.Features.Hotel.Commands.CreatePictureHotel;
using Application.Features.Hotel.Commands.DeleteHotel;
using Application.Features.Hotel.Commands.UpdateHotelData;
using Application.Features.Hotel.Commands.UpdateStars;
using Application.Features.Hotel.Queries.GetHotelByID;
using Application.Features.Hotel.Queries.GetHotelRooms;
using Application.Features.Hotel.Queries.GetHotels;
using Application.Features.Hotel.Queries.GetHotelsByID;
using Application.ResourceParameters;
using Application.Wrappers;
using Hotel.WebApi.Cache;
using Hotel.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotel.WebApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class HotelController : BaseController
    {
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary ="Get All Hotels")]
        [Cache()]
        [HttpGet(Name ="GetAllHotels")]
        public async Task<IActionResult> GetHotels([FromQuery]GetHotelsQueryParameters hotelQuery)
        {
            var user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); //HttpContext.Request.Headers.ContainsKey("Authorization");

            if (user is null)
            {
                return NotFound(new { P = "Nioe znaleziono usera chuju " });
            }

            var result = await Mediator.Send(hotelQuery);
            var nextPageLink = result.HasNext ?
                CreateHotelsResourceUri(hotelQuery, ResourceUriType.NextPage) : null;
            var previousNextLink = result.HasPrevious ?
                CreateHotelsResourceUri(hotelQuery, ResourceUriType.PreviousPage) : null;
            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious,
                nextPageLink,
                previousNextLink
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(result);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get Hotel By Unique ID")]
        [HttpGet("(unique){id}", Name = "GetHotelById")]
        public async Task<IActionResult> GetHotel([FromRoute] Guid hotelId) =>
            Ok(await Mediator.Send(new GetHotelQuery { HotelId = hotelId }));


        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Hotel With Rooms")]
        [HttpGet("{id}", Name = "GetHotelWithRooms")]
        public async Task<IActionResult> GetHotelRooms([FromRoute] Guid id) =>
            Ok(await Mediator.Send(new GetHotelWithRoomsQuery { HotelId = id }));


        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get Selected Hotels")]
        [HttpGet("{ids}", Name = "GetSelectedHotels")]
        public async Task<IActionResult> GetHotelsById([FromRoute]
        [ModelBinder(BinderType = typeof(ModelBinder))] IEnumerable<Guid> ids)
        {
            var result = await Mediator.Send(new GetHotelsByIdQuery { Ids = ids });
            return Ok(result);
        }

        [Authorize(Roles ="Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Update Selected Properties ")]
        [HttpPatch("{id}", Name ="Updatehotel")]
        public async Task<IActionResult> UpdateHotel([FromRoute] Guid id, [FromBody] JsonPatchDocument<UpdateHotelDto> jsonPatch)
        {
            await Mediator.Send(new UpdateHotelCommand { Id = id, Patch = jsonPatch });
            return NoContent();
        }

        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Create Hotel")]
        [HttpPost(Name = "CreateHotel")]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
        {
            command.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(command));
        }


        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       // [ServiceFilter(typeof(FileValidationFilter))]
        [SwaggerOperation(Summary = "Add Picture")]
        [HttpPost("[action]", Name = "CreateHotelPicture")]
        public async Task<IActionResult> CreatePictureHotel([FromQuery] CreateHotelPictureCommand command) =>
            Ok(await Mediator.Send(command));

        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Change Number Of Stars")]
        [HttpPut("{id}", Name ="ChangeNumberOfStars")]
        public async Task<IActionResult> UpdateStars([FromRoute]Guid id,[FromBody]int stars)
        {
            var result = await Mediator.Send(new UpdateHotelStarsCommand() { HotelId = id, Stars = stars });
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Remove Hotel By ID")]
        [HttpDelete("{id}", Name = "DeleteHotel")]
        public async Task<IActionResult> DeleteHotel([FromRoute] Guid id) =>
             Ok(await Mediator.Send(new DeleteHotelCommand(){ HotelId = id }));

        private string CreateHotelsResourceUri(GetHotelsQueryParameters parameters,ResourceUriType uriType)
        {
            const string nameMethod = "GetAllHotels";
            switch (uriType)
            {
                case ResourceUriType.NextPage:
                    return Url.Link(nameMethod, new
                    {
                        PageNumber = parameters.PageNumber + 1,
                        PageSize =parameters.PageSize,
                        Phrase=parameters.SearchQuery
                    });
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameMethod, new
                    {
                        PageNumber = parameters.PageNumber - 1,
                        PageSize = parameters.PageSize,
                        Phrase = parameters.SearchQuery
                    });
                default:
                    return Url.Link(nameMethod, new
                    {
                        PageNumber = parameters.PageNumber,
                        PageSize = parameters.PageSize,
                        Phrase = parameters.SearchQuery
                    });
            }
        }
    }
}
