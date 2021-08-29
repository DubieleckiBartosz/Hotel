using Application.ResourceParameters;
using MediatR;


namespace Application.Features.Hotel.Queries.GetHotels
{
    public class GetHotelsQueryParameters: HotelQueryParameters, IRequest<PagedList<GetHotelsVM>>
    {
    }
}
